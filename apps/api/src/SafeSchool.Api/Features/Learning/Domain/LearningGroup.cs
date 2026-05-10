using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningGroup : TenantOwnedEntity
{
    public Guid? CourseId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public LearningGroupStatus GroupStatus { get; set; } = LearningGroupStatus.Draft;
    public string VisibilityPolicy { get; set; } = "standard";
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
