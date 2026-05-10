using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class StaffLearningAssignment : TenantOwnedEntity
{
    public string ActorId { get; set; } = string.Empty;
    public Guid? CourseId { get; set; }
    public Guid? LearningGroupId { get; set; }
    public StaffAssignmentRole AssignmentRole { get; set; } = StaffAssignmentRole.Teacher;
    public StaffAssignmentStatus AssignmentStatus { get; set; } = StaffAssignmentStatus.Active;
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
