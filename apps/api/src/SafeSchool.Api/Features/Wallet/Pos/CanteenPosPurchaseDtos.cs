using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed record MerchantRequest(string MerchantName, string MerchantCode, MerchantStatus Status = MerchantStatus.Active, string AllowedCategoryCodes = "meal,snack,drink", string ActorReference = "canteen-manager");
public sealed record ItemCategoryRequest(string ItemCategoryCode, string DisplayName, CanteenItemCategoryStatus Status = CanteenItemCategoryStatus.Active);
public sealed record PurchaseEligibilityRuleRequest(Guid CanteenMerchantId, string ItemCategoryCode, PurchaseEligibilityRuleStatus Status = PurchaseEligibilityRuleStatus.Active, string DenialReason = "");
public sealed record PosTerminalRequest(Guid CanteenMerchantId, string TerminalCode, string DeviceReference, string OperatorReference, bool OfflineEnabled = false, long PerTerminalReserveMinor = 0, PosTerminalStatus Status = PosTerminalStatus.Active);
public sealed record OnlinePurchaseRequest(Guid WalletId, Guid MerchantId, Guid PosTerminalId, string CredentialReference, string ClientPurchaseId, long AmountMinor, string CurrencyCode = "SAR", string ItemCategoryCode = "meal", string ItemSummary = "canteen item", string OperatorReference = "pos-operator", string DeviceReference = "pos-device");
public sealed record OfflinePurchaseItem(string ClientPurchaseId, Guid WalletId, string CredentialReference, long AmountMinor, string ItemCategoryCode = "meal", string ItemSummary = "offline item", DateTimeOffset? LocalCapturedAt = null);
public sealed record OfflineSyncRequest(Guid PosTerminalId, string ClientBatchId, IReadOnlyList<OfflinePurchaseItem> Purchases);
public sealed record ReviewOutcomeRequest(string Action, string Reason, string ActorReference = "reviewer");
public sealed record MerchantResponse(Guid MerchantId, string MerchantCode, string MerchantName, MerchantStatus Status);
public sealed record PosTerminalResponse(Guid TerminalId, string TerminalCode, Guid MerchantId, PosTerminalStatus Status, bool OfflineEnabled, long PerTerminalReserveMinor);
public sealed record PurchaseResponse(Guid PurchaseId, Guid WalletId, Guid MerchantId, Guid PosTerminalId, string ClientPurchaseId, long AmountMinor, string CurrencyCode, PosPurchaseDecision Decision, string DecisionReason, string RuleSnapshotReference, string ReserveSnapshotReference);
public sealed record OfflineSyncResponse(Guid BatchId, string ClientBatchId, OfflinePosSyncStatus Status, int AcceptedCount, int HeldCount, int DuplicateCount);
public sealed record PurchaseTraceResponse(Guid PurchaseId, IReadOnlyList<string> TraceReferences);

public static class CanteenPosMappingExtensions
{
    public static MerchantResponse ToResponse(this SafeSchool.Api.Features.Wallet.Canteen.CanteenMerchant merchant) => new(merchant.Id, merchant.MerchantCode, merchant.MerchantName, merchant.MerchantStatus);
    public static PosTerminalResponse ToResponse(this POSTerminal terminal) => new(terminal.Id, terminal.TerminalCode, terminal.CanteenMerchantId, terminal.Status, terminal.OfflineEnabled, terminal.PerTerminalReserveMinor);
    public static PurchaseResponse ToResponse(this CanteenPurchaseTransaction purchase) => new(purchase.Id, purchase.StudentWalletId, purchase.CanteenMerchantId, purchase.PosTerminalId, purchase.ClientPurchaseId, purchase.AmountMinor, purchase.CurrencyCode, purchase.Decision, purchase.DecisionReason, purchase.RuleSnapshotReference, purchase.ReserveSnapshotReference);
}
