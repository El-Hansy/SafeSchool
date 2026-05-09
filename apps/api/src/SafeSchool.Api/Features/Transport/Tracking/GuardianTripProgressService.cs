using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class GuardianTripProgressService(SafeSchoolDbContext dbContext, ITransportGuardianLinkProvider guardianLinkProvider)
{
    public async Task<OperationResult<GuardianTripProgressResponse>> ProgressAsync(string tenantId, string guardianReference, string studentProfileId, Guid tripId, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinkProvider.GetLinkAsync(tenantId, guardianReference, studentProfileId, cancellationToken);
        if (link.Status != GuardianLinkStatus.Approved || !link.HasTransportVisibilityScope) return OperationResult<GuardianTripProgressResponse>.Failure(new ValidationError("guardian_scope_denied", "Guardian cannot view this trip progress."));
        var onboard = dbContext.BoardingDropScanEvents.Any(x => x.TenantId == tenantId && x.TransportTripId == tripId && x.StudentProfileId == studentProfileId && x.TransportStatusAfter == TransportStatusAfter.Onboard);
        var dropped = dbContext.BoardingDropScanEvents.Any(x => x.TenantId == tenantId && x.TransportTripId == tripId && x.StudentProfileId == studentProfileId && x.TransportStatusAfter == TransportStatusAfter.Dropped);
        var latest = dbContext.TransportLocationUpdates.Where(x => x.TenantId == tenantId && x.TransportTripId == tripId && x.AcceptanceStatus == LocationAcceptanceStatus.Accepted).OrderByDescending(x => x.ReportedAt).FirstOrDefault();
        var exact = onboard && !dropped ? latest?.LocationReference : null;
        var phase = dropped ? "After Drop" : onboard ? "Onboard" : "Before Boarding";
        return OperationResult<GuardianTripProgressResponse>.Success(new GuardianTripProgressResponse(studentProfileId, tripId, phase, onboard ? null : "Pickup ETA available when calculated", exact, dropped ? "Dropped" : null));
    }
}
