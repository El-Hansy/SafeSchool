namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605030004GuardianLinking
{
    public const string MigrationId = "202605030004_GuardianLinking";
    public static readonly string[] Tables = ["identity_access_guardians", "identity_access_guardian_links"];
}
