namespace SafeSchool.Api.Features.Wallet.Common;

public static class WalletPermissionCatalog
{
    public const string WalletsRead = "wallet.wallets.read";
    public const string WalletsManage = "wallet.wallets.manage";
    public const string LedgerRead = "wallet.ledger.read";
    public const string LedgerPost = "wallet.ledger.post";
    public const string TopUpsInitiate = "wallet.topups.initiate";
    public const string TopUpsCashier = "wallet.topups.cashier";
    public const string PaymentsConfirm = "wallet.payments.confirm";
    public const string PurchasesRecord = "wallet.purchases.record";
    public const string PurchasesSync = "wallet.purchases.sync";
    public const string PurchasesRead = "wallet.purchases.read";
    public const string LimitsRead = "wallet.limits.read";
    public const string LimitsManage = "wallet.limits.manage";
    public const string HistoryRead = "wallet.history.read";
    public const string GuardianHistoryRead = "wallet.guardian_history.read";
    public const string CorrectionsManage = "wallet.corrections.manage";
    public const string ChargebacksReview = "wallet.chargebacks.review";
    public const string ReconciliationRead = "wallet.reconciliation.read";
    public const string ReconciliationManage = "wallet.reconciliation.manage";
    public const string AnomaliesRead = "wallet.anomalies.read";
    public const string AnomaliesResolve = "wallet.anomalies.resolve";
    public const string RulesRead = "wallet.rules.read";
    public const string RulesManage = "wallet.rules.manage";
    public const string AuditRead = "wallet.audit.read";

    public static readonly string[] WalletAdministrator =
    [
        WalletsRead, WalletsManage, LedgerRead, LedgerPost, TopUpsInitiate, TopUpsCashier,
        PaymentsConfirm, PurchasesRecord, PurchasesSync, PurchasesRead, LimitsRead, LimitsManage,
        HistoryRead, GuardianHistoryRead, CorrectionsManage, ChargebacksReview, ReconciliationRead,
        ReconciliationManage, AnomaliesRead, AnomaliesResolve, RulesRead, RulesManage, AuditRead
    ];

    public static readonly string[] Guardian = [TopUpsInitiate, GuardianHistoryRead, LimitsRead, LimitsManage];
    public static readonly string[] PosOperator = [PurchasesRecord, PurchasesSync, PurchasesRead];
    public static readonly string[] FinanceReviewer = [HistoryRead, CorrectionsManage, ChargebacksReview, ReconciliationManage, AuditRead];
}
