using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class LiveTrackingAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task LocationRecordedAsync(TransportLocationUpdate update, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = update.TenantId, EventCategory = "Tracking", EventType = "transport.location.record", SubjectType = "TransportLocationUpdate", SubjectReference = update.Id.ToString(), Reason = update.SuppressionReason }, cancellationToken);
}
