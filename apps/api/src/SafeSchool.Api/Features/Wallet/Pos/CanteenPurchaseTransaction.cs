using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class CanteenPurchaseTransaction : TenantOwnedEntity
{
    public Guid StudentWalletId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid CanteenMerchantId { get; set; }
    public Guid PosTerminalId { get; set; }
    public Guid? OfflinePosSyncBatchId { get; set; }
    public string ClientPurchaseId { get; set; } = string.Empty;
    public string CredentialReference { get; set; } = string.Empty;
    public string ItemCategoryCode { get; set; } = string.Empty;
    public string ItemSummary { get; set; } = string.Empty;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public PosPurchaseMode PurchaseMode { get; set; } = PosPurchaseMode.Online;
    public PosPurchaseDecision Decision { get; set; } = PosPurchaseDecision.Submitted;
    public string DecisionReason { get; set; } = string.Empty;
    public string RuleSnapshotReference { get; set; } = string.Empty;
    public string ReserveSnapshotReference { get; set; } = string.Empty;
    public Guid? LedgerDebitEntryId { get; set; }
    public DateTimeOffset LocalCapturedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
}
