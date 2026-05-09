namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040015WalletCanteenPos
{
    public const string MigrationId = "202605040015_WalletCanteenPos";
    public static readonly string[] Tables = ["wallet_canteen_merchants", "wallet_pos_terminals", "wallet_canteen_purchase_transactions", "wallet_offline_pos_sync_batches"];
}
