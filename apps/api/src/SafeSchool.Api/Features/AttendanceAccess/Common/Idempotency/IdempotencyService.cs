using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;

public sealed class AttendanceAccessIdempotencyRecord : TenantOwnedEntity
{
    public string IdempotencyKey { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
    public string ResultReference { get; set; } = string.Empty;
}

public sealed record IdempotencyDecision(bool IsRetry, bool IsConflict, string? ResultReference);

public sealed class IdempotencyService
{
    private readonly Dictionary<string, AttendanceAccessIdempotencyRecord> _records = [];

    public IdempotencyDecision Register(string tenantId, string idempotencyKey, string requestHash, string resultReference)
    {
        var key = $"{tenantId}:{idempotencyKey}";
        if (_records.TryGetValue(key, out var existing))
        {
            return existing.RequestHash == requestHash
                ? new(true, false, existing.ResultReference)
                : new(false, true, existing.ResultReference);
        }

        _records[key] = new AttendanceAccessIdempotencyRecord
        {
            TenantId = tenantId,
            IdempotencyKey = idempotencyKey,
            RequestHash = requestHash,
            ResultReference = resultReference
        };
        return new(false, false, resultReference);
    }
}

