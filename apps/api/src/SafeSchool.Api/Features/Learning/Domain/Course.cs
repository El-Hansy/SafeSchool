using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class Course : TenantOwnedEntity
{
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CourseStatus CourseStatus { get; set; } = CourseStatus.Draft;
    public string VisibilityPolicy { get; set; } = "standard";
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
    public string CreatedByActorId { get; set; } = string.Empty;
    public string UpdatedByActorId { get; set; } = string.Empty;
}
