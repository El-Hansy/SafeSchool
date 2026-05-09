using SafeSchool.Api.Features.AttendanceAccess.Audit;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceAuditAdapter(IAttendanceAccessAuditWriter auditWriter)
{
    public Task RecordAsync(AttendanceRecord record, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AttendanceAccessAuditEvent
        {
            TenantId = record.TenantId,
            EventCategory = "Attendance",
            EventType = eventType,
            SubjectType = "AttendanceRecord",
            SubjectReference = record.Id.ToString(),
            Reason = reason
        }, cancellationToken);
}

