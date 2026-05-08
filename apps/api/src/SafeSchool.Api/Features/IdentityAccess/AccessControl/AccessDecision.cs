using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class AccessDecision : TenantOwnedEntity
{
    public required string ActorReference { get; set; }
    public required string AttemptedAction { get; set; }
    public required string TargetType { get; set; }
    public required string TargetReference { get; set; }
    public AccessDecisionResult Decision { get; set; }
    public required string DecisionReason { get; set; }
    public string[] RoleSources { get; set; } = [];
    public string? FeatureCapabilityKey { get; set; }
    public DateTimeOffset DecidedAt { get; set; } = DateTimeOffset.UtcNow;
}
