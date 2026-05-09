using SafeSchool.Api.Features.AttendanceAccess.Audit;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public sealed class GateScanAuditAdapter(IAttendanceAccessAuditWriter auditWriter)
{
    public Task ScanRecordedAsync(GateScanEvent scanEvent, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AttendanceAccessAuditEvent
        {
            TenantId = scanEvent.TenantId,
            EventCategory = "Gate Scan",
            EventType = $"attendance_access.scans.{scanEvent.Status.ToString().ToLowerInvariant()}",
            SubjectType = "GateScanEvent",
            SubjectReference = scanEvent.Id.ToString(),
            Reason = scanEvent.DecisionReason
        }, cancellationToken);
}

