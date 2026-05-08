namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605030003AccessControl
{
    public const string MigrationId = "202605030003_AccessControl";
    public static readonly string[] Tables =
    [
        "identity_access_roles",
        "identity_access_permissions",
        "identity_access_role_permissions",
        "identity_access_actor_role_assignments"
    ];
}
