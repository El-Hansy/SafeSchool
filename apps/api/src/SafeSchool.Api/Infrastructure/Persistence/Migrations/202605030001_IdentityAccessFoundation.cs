namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605030001IdentityAccessFoundation
{
    public const string MigrationId = "202605030001_IdentityAccessFoundation";
    public static readonly string[] Tables =
    [
        "identity_access_audit_events",
        "identity_access_access_decisions",
        "identity_access_feature_settings"
    ];
}
