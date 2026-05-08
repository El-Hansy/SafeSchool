using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Reviews;

public sealed class TransportReviewSummaryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<TransportReviewSummary>> ListAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var accepted = await dbContext.BoardingDropScanEvents.CountAsync(x => x.TenantId == tenantId && x.ScanDecision == Common.TransportScanDecision.Accepted, cancellationToken);
        var review = await dbContext.BoardingDropScanEvents.CountAsync(x => x.TenantId == tenantId && x.ScanDecision == Common.TransportScanDecision.NeedsReview, cancellationToken);
        var current = await dbContext.TransportLocationUpdates.CountAsync(x => x.TenantId == tenantId && x.FreshnessStatus == Common.LocationFreshnessStatus.Current, cancellationToken);
        var visible = await dbContext.TransportNotificationRecords.CountAsync(x => x.TenantId == tenantId && x.NotificationStatus == Common.NotificationStatus.Visible, cancellationToken);
        var open = await dbContext.TransportAnomalies.CountAsync(x => x.TenantId == tenantId && x.Status == Common.AnomalyStatus.Open, cancellationToken);
        return [new TransportReviewSummary("School", tenantId, tenantId, accepted, review, current, 0, visible, open, DateTimeOffset.UtcNow)];
    }
}
