using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Trips;

public sealed class TransportTripScanContextService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<TransportTrip>> RequireActiveTripAsync(string tenantId, Guid tripId, string trackingDeviceReference, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == tripId, cancellationToken);
        if (trip is null) return OperationResult<TransportTrip>.Failure(new ValidationError("not_found", "Trip was not found."));
        if (!trip.CanAcceptScan()) return OperationResult<TransportTrip>.Failure(new ValidationError("trip_not_active", "Trip is not accepting scans."));
        if (trip.TrackingDeviceReference != trackingDeviceReference) return OperationResult<TransportTrip>.Failure(new ValidationError("unauthorized_device", "Tracking device is not assigned to the trip."));
        return OperationResult<TransportTrip>.Success(trip);
    }
}
