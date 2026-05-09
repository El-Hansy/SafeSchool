namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605030005CredentialLifecycle
{
    public const string MigrationId = "202605030005_CredentialLifecycle";
    public static readonly string[] Tables =
    [
        "identity_access_credentials",
        "identity_access_nfc_card_credentials",
        "identity_access_qr_fallback_credentials",
        "identity_access_credential_status_snapshots"
    ];
}
