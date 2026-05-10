using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class TransportLocationUpdateService(SafeSchoolDbContext dbContext, LocationUpdateValidationService validationService)
{
    public async Task<LocationUpdateResponse> SubmitAsync(string tenantId, Guid tripId, LocationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await validationService.ValidateAsync(tenantId, tripId, request, cancellationToken);
        var update = new TransportLocationUpdate { TenantId = tenantId, ClientLocationId = request.ClientLocationId, TransportTripId = tripId, TrackingDeviceReference = request.TrackingDeviceReference, ActorReference = request.ActorReference, ReportedAt = request.ReportedAt, LocationReference = request.LocationReference, ProgressState = request.ProgressState, NearestRouteStopSequenceId = request.NearestRouteStopSequenceId, AcceptanceStatus = validation.Succeeded ? LocationAcceptanceStatus.Accepted : LocationAcceptanceStatus.Suppressed, FreshnessStatus = validation.Succeeded ? LocationFreshnessStatus.Current : LocationFreshnessStatus.Stale, SuppressionReason = validation.Succeeded ? string.Empty : validation.Errors[0].Message };
        dbContext.TransportLocationUpdates.Add(update);
        await dbContext.SaveChangesAsync(cancellationToken);
        return update.ToResponse();
    }

    public async Task<LocationUpdateResponse?> LatestAsync(string tenantId, Guid tripId, CancellationToken cancellationToken = default) =>
        await dbContext.TransportLocationUpdates.Where(x => x.TenantId == tenantId && x.TransportTripId == tripId).OrderByDescending(x => x.ReportedAt).Select(x => x.ToResponse()).FirstOrDefaultAsync(cancellationToken);
}
