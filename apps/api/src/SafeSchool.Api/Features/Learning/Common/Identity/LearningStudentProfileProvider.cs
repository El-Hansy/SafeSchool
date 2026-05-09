using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Common.Identity;

public sealed record LearningStudentProfileEvidence(string TenantId, string StudentProfileId, LearningStudentProfileStatus Status, bool IsActive);

public interface ILearningStudentProfileProvider
{
    LearningStudentProfileEvidence GetStudent(string tenantId, string studentProfileId);
}

public sealed class DefaultLearningStudentProfileProvider : ILearningStudentProfileProvider
{
    public LearningStudentProfileEvidence GetStudent(string tenantId, string studentProfileId) => new(tenantId, studentProfileId, LearningStudentProfileStatus.Active, true);
}
