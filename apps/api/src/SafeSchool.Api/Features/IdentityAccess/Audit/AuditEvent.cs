using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Audit;

public sealed class AuditEvent : TenantOwnedEntity
{
    public required string EventCategory { get; set; }
    public required string EventType { get; set; }
    public required string ActorReference { get; set; }
    public required string SubjectType { get; set; }
    public required string SubjectReference { get; set; }
    public string? PreviousValueSummary { get; set; }
    public string? NewValueSummary { get; set; }
    public required string Reason { get; set; }
    public Guid? AccessDecisionId { get; set; }
    public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
    public AuditReviewStatus ReviewStatus { get; set; } = AuditReviewStatus.Recorded;
}
