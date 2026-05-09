namespace SafeSchool.Api.Features.Wallet.Limits;

public static class SpendingLimitEntityTypeConfiguration
{
    public static readonly string[] Tables = ["wallet_spending_limits", "wallet_purchase_rule_snapshots"];
}
