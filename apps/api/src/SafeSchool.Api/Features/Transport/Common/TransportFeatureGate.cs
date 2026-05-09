using SafeSchool.Api.Infrastructure.FeatureFlags;

namespace SafeSchool.Api.Features.Transport.Common;

public sealed class TransportFeatureGate(IFeatureGateService featureGateService)
{
    public bool IsEnabled(string tenantId, string capabilityKey) => featureGateService.IsEnabled(tenantId, capabilityKey);

    public OperationResult<string> Require(string tenantId, string capabilityKey) => IsEnabled(tenantId, capabilityKey)
        ? OperationResult<string>.Success("enabled")
        : OperationResult<string>.Failure(new ValidationError("feature_disabled", featureGateService.Explain(tenantId, capabilityKey), capabilityKey));
}
