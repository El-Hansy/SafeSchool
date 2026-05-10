using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Ledger;

public sealed class WalletLedgerEntry : TenantOwnedEntity
{
    public Guid StudentWalletId { get; set; }
    public WalletLedgerEntryType EntryType { get; set; } = WalletLedgerEntryType.Credit;
    public WalletLedgerEntryStatus EntryStatus { get; set; } = WalletLedgerEntryStatus.Pending;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public long BalanceAvailableAfterMinor { get; set; }
    public long BalancePendingAfterMinor { get; set; }
    public long BalanceHeldAfterMinor { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public Guid? OriginalEntryId { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public string RuleSnapshotReference { get; set; } = string.Empty;
    public DateTimeOffset PostedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ReviewReason { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = "system";

    public bool IsApproved => EntryStatus == WalletLedgerEntryStatus.Approved;
}
