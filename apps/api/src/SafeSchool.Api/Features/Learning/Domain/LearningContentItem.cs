using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningContentItem : TenantOwnedEntity
{
    public Guid? CourseId { get; set; }
    public Guid? LearningGroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ResourceReference { get; set; } = string.Empty;
    public ContentStatus ContentStatus { get; set; } = ContentStatus.Draft;
    public DateTimeOffset? ReleaseAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public CompletionPolicy CompletionPolicy { get; set; } = CompletionPolicy.ViewOnly;
    public string StudentVisibilityPolicy { get; set; } = "summary";
    public string GuardianVisibilityPolicy { get; set; } = "summary";
    public int ContentRevision { get; set; } = 1;
}
