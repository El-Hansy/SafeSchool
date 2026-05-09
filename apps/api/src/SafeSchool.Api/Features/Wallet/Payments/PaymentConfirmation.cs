using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Payments;

public sealed class PaymentConfirmation : TenantOwnedEntity
{
    public Guid? WalletTopUpId { get; set; }
    public string ProviderReference { get; set; } = string.Empty;
    public string ProviderEventId { get; set; } = string.Empty;
    public PaymentConfirmationStatus ConfirmationStatus { get; set; } = PaymentConfirmationStatus.Received;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public string SafePaymentMethodSummary { get; set; } = string.Empty;
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ProviderEventTime { get; set; } = DateTimeOffset.UtcNow;
    public string RawPayloadReference { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public string ReviewReason { get; set; } = string.Empty;
}
