namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040013WalletLedger
{
    public const string MigrationId = "202605040013_WalletLedger";
    public static readonly string[] Tables = ["wallet_student_wallets", "wallet_ledger_entries"];
}
