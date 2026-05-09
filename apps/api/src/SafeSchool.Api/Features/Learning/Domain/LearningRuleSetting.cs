using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningRuleSetting : TenantOwnedEntity
{
    public RuleArea RuleArea { get; set; } = RuleArea.Content;
    public string RulePayload { get; set; } = "{}";
    public int RuleVersion { get; set; } = 1;
    public RuleStatus RuleStatus { get; set; } = RuleStatus.Draft;
    public string ChangeReason { get; set; } = string.Empty;
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
