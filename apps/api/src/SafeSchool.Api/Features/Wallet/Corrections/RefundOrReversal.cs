using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Corrections;

public sealed class RefundOrReversal : TenantOwnedEntity
{
    public Guid StudentWalletId { get; set; }
    public CorrectionType CorrectionType { get; set; } = CorrectionType.Refund;
    public string OriginalSourceType { get; set; } = string.Empty;
    public string OriginalSourceReference { get; set; } = string.Empty;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public string ReviewerActor { get; set; } = "reviewer";
    public Guid? ResultingLedgerEntryId { get; set; }
    public string ClientRequestId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public CorrectionStatus Status { get; set; } = CorrectionStatus.Requested;
}
