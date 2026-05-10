namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605100012MobileRoleBasedApkRelease
{
    public const string MigrationId = "202605100012_MobileRoleBasedApkRelease";
    public static readonly string[] Tables =
    [
        "mobile_user_profiles",
        "mobile_role_workspaces",
        "mobile_role_workspace_actions",
        "mobile_permission_grants",
        "tenant_mobile_feature_availability",
        "mobile_language_preferences",
        "mobile_device_sessions",
        "mobile_apk_releases",
        "mobile_release_audiences",
        "mobile_install_events",
        "mobile_offline_action_queue",
        "mobile_audit_events"
    ];
}
