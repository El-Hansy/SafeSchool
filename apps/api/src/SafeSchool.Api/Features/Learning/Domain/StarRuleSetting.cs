using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class StarRuleSetting : TenantOwnedEntity
{
    public RuleArea SourceRuleArea { get; set; } = RuleArea.Content;
    public string EligibleScope { get; set; } = "Course";
    public int StarAmount { get; set; }
    public int? AwardCap { get; set; }
    public RuleStatus RuleStatus { get; set; } = RuleStatus.Draft;
    public int RuleVersion { get; set; } = 1;
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
