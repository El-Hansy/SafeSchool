namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040017WalletHistoryCorrectionsAndAnomalies
{
    public const string MigrationId = "202605040017_WalletHistoryCorrectionsAndAnomalies";
    public static readonly string[] Tables = ["wallet_refunds_reversals", "wallet_manual_reviews", "wallet_anomalies"];
}
