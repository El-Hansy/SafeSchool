using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Common.Identity;

public sealed record StaffLearningAssignmentEvidence(string TenantId, string ActorId, string ScopeId, StaffAssignmentRole Role, StaffAssignmentStatus Status, IReadOnlyList<string> Permissions);

public interface IStaffLearningAssignmentProvider
{
    StaffLearningAssignmentEvidence GetAssignment(string tenantId, string actorId, string scopeId);
}

public sealed class DefaultStaffLearningAssignmentProvider : IStaffLearningAssignmentProvider
{
    public StaffLearningAssignmentEvidence GetAssignment(string tenantId, string actorId, string scopeId) => new(tenantId, actorId, scopeId, StaffAssignmentRole.Teacher, StaffAssignmentStatus.Active, LearningPermissionCatalog.Teacher);
}
