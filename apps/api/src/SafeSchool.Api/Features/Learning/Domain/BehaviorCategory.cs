using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class BehaviorCategory : TenantOwnedEntity
{
    public string CategoryCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public BehaviorClassification Classification { get; set; } = BehaviorClassification.Neutral;
    public BehaviorSeverity DefaultSeverity { get; set; } = BehaviorSeverity.Low;
    public string DefaultVisibility { get; set; } = "guardian_summary";
    public Guid? StarRuleSettingId { get; set; }
    public RuleStatus CategoryStatus { get; set; } = RuleStatus.Draft;
    public bool SensitiveByDefault { get; set; }
}
