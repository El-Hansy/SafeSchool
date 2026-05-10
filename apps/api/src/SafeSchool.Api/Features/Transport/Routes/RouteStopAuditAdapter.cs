using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class RouteStopAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task RouteChangedAsync(TransportRoute route, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = route.TenantId, EventCategory = "Route", EventType = eventType, SubjectType = "TransportRoute", SubjectReference = route.Id.ToString(), Reason = reason }, cancellationToken);

    public Task StopChangedAsync(TransportStop stop, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = stop.TenantId, EventCategory = "Route", EventType = eventType, SubjectType = "TransportStop", SubjectReference = stop.Id.ToString(), Reason = reason }, cancellationToken);
}
