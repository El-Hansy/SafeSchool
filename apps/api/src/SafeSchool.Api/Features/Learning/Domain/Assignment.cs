using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class Assignment : TenantOwnedEntity
{
    public Guid? CourseId { get; set; }
    public Guid? LearningGroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string RequiredEvidence { get; set; } = string.Empty;
    public string AssignedToScope { get; set; } = "Group";
    public DateTimeOffset? DueAt { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? ClosesAt { get; set; }
    public LatePolicy LatePolicy { get; set; } = LatePolicy.RouteLateToReview;
    public ReviewPolicy ReviewPolicy { get; set; } = ReviewPolicy.Grade;
    public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.Draft;
    public int ConfigurationRevision { get; set; } = 1;
}
