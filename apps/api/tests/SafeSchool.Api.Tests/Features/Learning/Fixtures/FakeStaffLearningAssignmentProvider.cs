using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public sealed class FakeStaffLearningAssignmentProvider : IStaffLearningAssignmentProvider
{
    public StaffAssignmentRole Role { get; set; } = StaffAssignmentRole.Teacher;
    public StaffAssignmentStatus Status { get; set; } = StaffAssignmentStatus.Active;
    public StaffLearningAssignmentEvidence GetAssignment(string tenantId, string actorId, string scopeId) => new(tenantId, actorId, scopeId, Role, Status, LearningPermissionCatalog.Teacher);
}
