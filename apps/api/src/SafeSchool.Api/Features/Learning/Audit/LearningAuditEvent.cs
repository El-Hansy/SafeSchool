using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Audit;

public sealed class LearningAuditEvent : TenantOwnedEntity
{
    public string EventType { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public string PayloadSummary { get; set; } = string.Empty;
    public bool SensitivePayloadRedacted { get; set; } = true;
    public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
}
