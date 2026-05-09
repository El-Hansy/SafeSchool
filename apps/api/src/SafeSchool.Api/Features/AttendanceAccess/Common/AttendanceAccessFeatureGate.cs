using SafeSchool.Api.Infrastructure.FeatureFlags;

namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public sealed class AttendanceAccessFeatureGate(IFeatureGateService featureGateService)
{
    public bool IsEnabled(string tenantId, string capabilityKey) => featureGateService.IsEnabled(tenantId, capabilityKey);
    public string Explain(string tenantId, string capabilityKey) => featureGateService.Explain(tenantId, capabilityKey);
}

