using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Routes;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Trips;

public sealed class TransportTripLifecycleService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard)
{
    public async Task<OperationResult<TransportTrip>> CreateAsync(string tenantId, Guid routeId, Guid vehicleId, string trackingDeviceReference, ServiceDirection direction, DateOnly tripDate, DateTimeOffset plannedStart, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, TransportCapabilities.BoardingDropScans, TransportPermissionCatalog.TripsStart, targetType: "TransportTrip", targetReference: trackingDeviceReference, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<TransportTrip>.Failure(allowed.Errors.ToArray());
        var route = await dbContext.TransportRoutes.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == routeId, cancellationToken);
        if (route is null || route.RouteStatus != RouteStatus.Active) return OperationResult<TransportTrip>.Failure(new ValidationError("route_not_active", "Trip requires an active route."));
        var conflict = await dbContext.TransportTrips.AnyAsync(x => x.TenantId == tenantId && x.TripStatus == TripStatus.Active && (x.TransportVehicleId == vehicleId || x.TrackingDeviceReference == trackingDeviceReference), cancellationToken);
        if (conflict) return OperationResult<TransportTrip>.Failure(new ValidationError("active_trip_overlap", "Vehicle or tracking device is already assigned to an active trip."));
        var trip = new TransportTrip { TenantId = tenantId, TransportRouteId = routeId, RouteVersion = route.RouteVersion, TransportVehicleId = vehicleId, TrackingDeviceReference = trackingDeviceReference, ServiceDirection = direction, TripDate = tripDate, PlannedStartTime = plannedStart };
        dbContext.TransportTrips.Add(trip);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportTrip>.Success(trip);
    }

    public async Task<OperationResult<TransportTrip>> StartAsync(string tenantId, Guid tripId, string trackingDeviceReference, DateTimeOffset actualStart, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == tripId, cancellationToken);
        if (trip is null) return OperationResult<TransportTrip>.Failure(new ValidationError("not_found", "Trip was not found."));
        if (trip.TrackingDeviceReference != trackingDeviceReference) return OperationResult<TransportTrip>.Failure(new ValidationError("unauthorized_device", "Tracking device is not assigned to this trip."));
        trip.TripStatus = TripStatus.Active;
        trip.ActualStartTime = actualStart;
        trip.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportTrip>.Success(trip);
    }

    public async Task<OperationResult<TransportTrip>> EndAsync(string tenantId, Guid tripId, DateTimeOffset actualEnd, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == tripId, cancellationToken);
        if (trip is null) return OperationResult<TransportTrip>.Failure(new ValidationError("not_found", "Trip was not found."));
        trip.TripStatus = TripStatus.Completed;
        trip.ActualEndTime = actualEnd;
        trip.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportTrip>.Success(trip);
    }
}
