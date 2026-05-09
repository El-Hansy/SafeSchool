using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Common.Idempotency;

public sealed class TransportIdempotencyRecord : TenantOwnedEntity
{
    public string IdempotencyKind { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
    public string ResultReference { get; set; } = string.Empty;
}

public sealed record TransportIdempotencyDecision(bool IsRetry, bool IsConflict, string? ResultReference);

public sealed class TransportIdempotencyService
{
    private readonly Dictionary<string, TransportIdempotencyRecord> _records = [];

    public TransportIdempotencyDecision Register(string tenantId, string kind, string idempotencyKey, string requestHash, string resultReference)
    {
        var key = $"{tenantId}:{kind}:{idempotencyKey}";
        if (_records.TryGetValue(key, out var existing))
        {
            return existing.RequestHash == requestHash
                ? new(true, false, existing.ResultReference)
                : new(false, true, existing.ResultReference);
        }

        _records[key] = new TransportIdempotencyRecord
        {
            TenantId = tenantId,
            IdempotencyKind = kind,
            IdempotencyKey = idempotencyKey,
            RequestHash = requestHash,
            ResultReference = resultReference
        };
        return new(false, false, resultReference);
    }

    public TransportIdempotencyDecision RegisterRequest(string tenantId, string clientRequestId, string requestHash, string resultReference) => Register(tenantId, "request", clientRequestId, requestHash, resultReference);
    public TransportIdempotencyDecision RegisterScan(string tenantId, string clientScanId, string requestHash, string resultReference) => Register(tenantId, "scan", clientScanId, requestHash, resultReference);
    public TransportIdempotencyDecision RegisterBatch(string tenantId, string clientBatchId, string requestHash, string resultReference) => Register(tenantId, "batch", clientBatchId, requestHash, resultReference);
    public TransportIdempotencyDecision RegisterLocation(string tenantId, string clientLocationId, string requestHash, string resultReference) => Register(tenantId, "location", clientLocationId, requestHash, resultReference);
}
