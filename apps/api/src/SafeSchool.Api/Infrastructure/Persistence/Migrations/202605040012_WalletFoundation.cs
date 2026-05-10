namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040012WalletFoundation
{
    public const string MigrationId = "202605040012_WalletFoundation";
    public static readonly string[] Tables = ["wallet_audit_events", "wallet_idempotency_records", "wallet_rule_settings"];
}
