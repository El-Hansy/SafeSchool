using SafeSchool.Api.Features.AttendanceAccess.Audit;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyAuditAdapter(IAttendanceAccessAuditWriter auditWriter)
{
    public Task RecordAsync(AttendanceAnomaly anomaly, string eventType, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AttendanceAccessAuditEvent
        {
            TenantId = anomaly.TenantId,
            EventCategory = "Attendance Anomaly",
            EventType = eventType,
            SubjectType = "AttendanceAnomaly",
            SubjectReference = anomaly.Id.ToString(),
            Reason = anomaly.ResolutionReason
        }, cancellationToken);
}

