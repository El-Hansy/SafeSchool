using SafeSchool.Api.Features.AttendanceAccess.Audit;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationAuditAdapter(IAttendanceAccessAuditWriter auditWriter)
{
    public Task RecordAsync(EntryExitNotificationRecord record, string eventType, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AttendanceAccessAuditEvent
        {
            TenantId = record.TenantId,
            EventCategory = "Entry Exit Notification",
            EventType = eventType,
            SubjectType = "EntryExitNotificationRecord",
            SubjectReference = record.Id.ToString(),
            Reason = record.SuppressionReason
        }, cancellationToken);
}

