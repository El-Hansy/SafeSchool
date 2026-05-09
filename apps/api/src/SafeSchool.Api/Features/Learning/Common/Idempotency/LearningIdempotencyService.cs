using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Common.Idempotency;

public sealed class LearningIdempotencyRecord : TenantOwnedEntity
{
    public string IdempotencyKind { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public LearningIdempotencyOutcome Outcome { get; set; } = LearningIdempotencyOutcome.Accepted;
}

public sealed record LearningIdempotencyDecision(LearningIdempotencyOutcome Outcome, string IdempotencyKey, string Fingerprint);

public sealed class LearningIdempotencyService
{
    private readonly Dictionary<string, string> fingerprints = new(StringComparer.OrdinalIgnoreCase);

    public LearningIdempotencyDecision Record(string kind, string key, string fingerprint)
    {
        var composite = $"{kind}:{key}";
        if (!fingerprints.TryGetValue(composite, out var existing))
        {
            fingerprints[composite] = fingerprint;
            return new LearningIdempotencyDecision(LearningIdempotencyOutcome.Accepted, key, fingerprint);
        }

        return new LearningIdempotencyDecision(existing == fingerprint ? LearningIdempotencyOutcome.ExactDuplicate : LearningIdempotencyOutcome.ConflictingDuplicate, key, fingerprint);
    }
}
