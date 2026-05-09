using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class SchoolAccountFeatureSetting : TenantOwnedEntity
{
    public string CapabilityKey { get; set; } = string.Empty;
    public LearningFeatureStatus FeatureStatus { get; set; } = LearningFeatureStatus.Proposed;
    public string DefaultGuardianVisibility { get; set; } = "summary";
    public DateTimeOffset? EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
