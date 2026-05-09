using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Audit;

public sealed class LearningStatusEvent : TenantOwnedEntity
{
    public LearningStatusEventType StatusEventType { get; set; } = LearningStatusEventType.ProgressChanged;
    public string SourceType { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public bool ReadyForNotification { get; set; }
    public bool ReadyForStarEvidence { get; set; }
    public DateTimeOffset ExportedAt { get; set; } = DateTimeOffset.UtcNow;
}
