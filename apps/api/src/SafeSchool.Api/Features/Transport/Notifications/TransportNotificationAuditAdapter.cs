using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task NotificationChangedAsync(TransportNotificationRecord record, string eventType, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = record.TenantId, EventCategory = "Notification", EventType = eventType, SubjectType = "TransportNotificationRecord", SubjectReference = record.Id.ToString(), Reason = record.SuppressionReason }, cancellationToken);
}
