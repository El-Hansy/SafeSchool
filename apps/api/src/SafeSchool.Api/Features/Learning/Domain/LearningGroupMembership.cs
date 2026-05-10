using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningGroupMembership : TenantOwnedEntity
{
    public Guid LearningGroupId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public MembershipStatus MembershipStatus { get; set; } = MembershipStatus.Active;
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
    public string ReviewReason { get; set; } = string.Empty;
}
