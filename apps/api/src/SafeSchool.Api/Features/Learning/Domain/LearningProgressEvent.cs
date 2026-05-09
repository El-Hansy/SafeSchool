using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningProgressEvent : TenantOwnedEntity
{
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid? CourseId { get; set; }
    public Guid? LearningGroupId { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public ProgressStatus ProgressStatus { get; set; } = ProgressStatus.Started;
    public int? ProgressPercent { get; set; }
    public string SourceEventReference { get; set; } = string.Empty;
    public string VisibilityPolicy { get; set; } = "guardian_summary";
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
}
