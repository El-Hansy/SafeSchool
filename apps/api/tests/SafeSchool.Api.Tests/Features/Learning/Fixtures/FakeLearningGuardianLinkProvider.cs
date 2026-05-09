using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public sealed class FakeLearningGuardianLinkProvider : ILearningGuardianLinkProvider
{
    public GuardianLearningLinkStatus Status { get; set; } = GuardianLearningLinkStatus.Approved;
    public LearningGuardianLinkEvidence GetGuardianLink(string tenantId, string guardianActorId, string studentProfileId) => new(tenantId, guardianActorId, studentProfileId, Status, Status == GuardianLearningLinkStatus.Approved, Status is GuardianLearningLinkStatus.RestrictedVisibility or GuardianLearningLinkStatus.StaffOnlyVisibility);
}
