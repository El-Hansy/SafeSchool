using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed class WalletTopUp : TenantOwnedEntity
{
    public Guid StudentWalletId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public string InitiatedByActorId { get; set; } = string.Empty;
    public string GuardianLinkId { get; set; } = string.Empty;
    public WalletTopUpSource TopUpSource { get; set; } = WalletTopUpSource.GuardianOnlineProvider;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public long FeeMinor { get; set; }
    public long NetCreditMinor { get; set; }
    public WalletTopUpStatus TopUpStatus { get; set; } = WalletTopUpStatus.Draft;
    public string PaymentProviderReference { get; set; } = string.Empty;
    public string CashierReference { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public DateTimeOffset InitiatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ConfirmedAt { get; set; }
    public DateTimeOffset? CreditedAt { get; set; }
    public string ReviewReason { get; set; } = string.Empty;
}
