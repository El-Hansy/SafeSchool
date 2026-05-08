using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public sealed class ScanTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<ScanTraceResponse?> TraceAsync(string tenantId, Guid scanEventId, CancellationToken cancellationToken = default)
    {
        var scan = await dbContext.GateScanEvents.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == scanEventId, cancellationToken);
        if (scan is null)
        {
            return null;
        }

        var decision = await dbContext.CampusAccessDecisions.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.GateScanEventId == scanEventId, cancellationToken);
        var attendance = await dbContext.AttendanceRecords.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.SourceScanEventId == scanEventId, cancellationToken);
        var notification = await dbContext.EntryExitNotificationRecords.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.GateScanEventId == scanEventId, cancellationToken);
        var anomaly = await dbContext.AttendanceAnomalies.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.EvidenceReference == scanEventId.ToString(), cancellationToken);
        var review = await dbContext.ManualReviews.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.TargetReference == scanEventId.ToString(), cancellationToken);
        var audit = await dbContext.AttendanceAccessAuditEvents.Where(x => x.TenantId == tenantId && x.SubjectReference == scanEventId.ToString()).Select(x => x.Id.ToString()).ToListAsync(cancellationToken);
        return new(scanEventId, decision?.Id, attendance?.Id, notification?.Id, anomaly?.Id, review?.Id, audit);
    }
}

