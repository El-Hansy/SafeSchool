namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileFeatureAvailabilityService
{
    public bool IsMobileEnabled(string tenantId) => tenantId != "mobile-disabled";
    public bool IsApkReleaseEnabled(string tenantId) => tenantId != "release-disabled";
    public bool IsWorkspaceEnabled(string tenantId, string workspaceCode) => IsMobileEnabled(tenantId) && MobileRoleCodes.All.Contains(workspaceCode);
    public IReadOnlyList<string> EnabledFeatures(string workspaceCode) => MobileSeedCatalog.EnabledFeaturesFor(workspaceCode);
}
