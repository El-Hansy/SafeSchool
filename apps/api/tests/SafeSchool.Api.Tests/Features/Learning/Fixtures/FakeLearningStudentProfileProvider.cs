using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public sealed class FakeLearningStudentProfileProvider : ILearningStudentProfileProvider
{
    public LearningStudentProfileStatus Status { get; set; } = LearningStudentProfileStatus.Active;
    public LearningStudentProfileEvidence GetStudent(string tenantId, string studentProfileId) => new(tenantId, studentProfileId, Status, Status == LearningStudentProfileStatus.Active);
}
