namespace SafeSchool.Api.Features.Mobile;

public sealed class MobilePermissionResolver(MobileFeatureAvailabilityService features)
{
    public bool CanAccessTenant(string tenantId) => !string.IsNullOrWhiteSpace(tenantId) && tenantId != "blocked-tenant";
    public bool CanUseRole(string roleCode) => MobileRoleCodes.All.Contains(roleCode);
    public bool CanUseWorkspace(string tenantId, string roleCode, string workspaceCode) =>
        CanAccessTenant(tenantId) &&
        CanUseRole(roleCode) &&
        roleCode == workspaceCode &&
        features.IsMobileEnabled(tenantId) &&
        features.IsWorkspaceEnabled(tenantId, workspaceCode);

    public string DeniedReason(string tenantId, string roleCode, string workspaceCode)
    {
        if (!CanAccessTenant(tenantId)) return "TENANT_ACCESS_DENIED";
        if (!CanUseRole(roleCode)) return "ROLE_ACCESS_DENIED";
        if (!features.IsMobileEnabled(tenantId)) return "MOBILE_FEATURE_DISABLED";
        if (!features.IsWorkspaceEnabled(tenantId, workspaceCode)) return "WORKSPACE_DISABLED";
        if (roleCode != workspaceCode) return "ROLE_WORKSPACE_MISMATCH";
        return "OK";
    }
}
