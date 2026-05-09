using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Common.Identity;

public sealed record LearningGuardianLinkEvidence(string TenantId, string GuardianActorId, string StudentProfileId, GuardianLearningLinkStatus Status, bool GuardianVisible, bool StaffOnlyDetailsHidden);

public interface ILearningGuardianLinkProvider
{
    LearningGuardianLinkEvidence GetGuardianLink(string tenantId, string guardianActorId, string studentProfileId);
}

public sealed class DefaultLearningGuardianLinkProvider : ILearningGuardianLinkProvider
{
    public LearningGuardianLinkEvidence GetGuardianLink(string tenantId, string guardianActorId, string studentProfileId) => new(tenantId, guardianActorId, studentProfileId, GuardianLearningLinkStatus.Approved, true, true);
}
