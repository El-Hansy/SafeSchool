using SafeSchool.Api.Infrastructure.FeatureFlags;

namespace SafeSchool.Api.Features.Learning.Common;

public sealed class LearningFeatureGate(IFeatureGateService featureGateService)
{
    public OperationResult<string> Require(string tenantId, string capabilityKey)
    {
        if (featureGateService.IsEnabled(tenantId, capabilityKey))
        {
            return OperationResult<string>.Success(capabilityKey);
        }

        return OperationResult<string>.Failure(new ValidationError("feature_disabled", featureGateService.Explain(tenantId, capabilityKey), capabilityKey));
    }
}
