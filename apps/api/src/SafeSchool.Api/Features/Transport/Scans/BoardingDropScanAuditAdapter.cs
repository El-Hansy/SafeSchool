using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class BoardingDropScanAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task ScanRecordedAsync(BoardingDropScanEvent scan, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = scan.TenantId, EventCategory = "Scan", EventType = "transport.scans.record", SubjectType = "BoardingDropScanEvent", SubjectReference = scan.Id.ToString(), Reason = scan.DecisionReason }, cancellationToken);
}
