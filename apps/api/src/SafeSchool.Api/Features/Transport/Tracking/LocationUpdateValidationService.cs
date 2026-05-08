using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class LocationUpdateValidationService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<string>> ValidateAsync(string tenantId, Guid tripId, LocationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.FindAsync([tripId], cancellationToken);
        if (trip is null || trip.TenantId != tenantId) return OperationResult<string>.Failure(new ValidationError("not_found", "Trip was not found."));
        if (!trip.CanAcceptLocation(request.TrackingDeviceReference)) return OperationResult<string>.Failure(new ValidationError("unauthorized_or_inactive_trip", "Trip is inactive or tracking device is not authorized."));
        if (DateTimeOffset.UtcNow - request.ReportedAt > TimeSpan.FromMinutes(10)) return OperationResult<string>.Failure(new ValidationError("stale_location", "Location update is stale."));
        return OperationResult<string>.Success("accepted");
    }
}
