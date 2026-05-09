namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040018WalletReconciliation
{
    public const string MigrationId = "202605040018_WalletReconciliation";
    public static readonly string[] Tables = ["wallet_reconciliation_runs", "wallet_reconciliation_mismatches", "wallet_settlement_references", "wallet_review_summaries"];
}
