namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040014WalletTopUpsAndPayments
{
    public const string MigrationId = "202605040014_WalletTopUpsAndPayments";
    public static readonly string[] Tables = ["wallet_top_ups", "wallet_payment_confirmations"];
}
