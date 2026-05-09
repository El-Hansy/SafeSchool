namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class WalletCapabilities
{
    public const string Ledger = "wallet.ledger";
    public const string TopUp = "wallet.top_up";
    public const string PaymentProcessing = "wallet.payment_processing";
    public const string CanteenPos = "wallet.canteen_pos";
    public const string SpendingLimits = "wallet.spending_limits";
    public const string TransactionHistory = "wallet.transaction_history";
    public const string Reconciliation = "wallet.reconciliation";

    public static readonly string[] All =
    [
        Ledger,
        TopUp,
        PaymentProcessing,
        CanteenPos,
        SpendingLimits,
        TransactionHistory,
        Reconciliation
    ];
}
