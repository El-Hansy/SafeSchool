using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed class EtaAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task EtaCalculatedAsync(EtaRecord eta, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = eta.TenantId, EventCategory = "ETA", EventType = "transport.eta.calculated", SubjectType = "EtaRecord", SubjectReference = eta.Id.ToString(), Reason = eta.EtaState.ToString() }, cancellationToken);
}
