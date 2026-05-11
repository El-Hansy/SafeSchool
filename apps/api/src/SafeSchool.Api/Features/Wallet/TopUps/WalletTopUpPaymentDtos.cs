using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed record GuardianTopUpRequest(Guid WalletId, string ClientRequestId, long AmountMinor, string CurrencyCode = "SAR", string PaymentProvider = "ConfiguredProvider", string GuardianActorId = "guardian-demo");
public sealed record CashierTopUpRequest(Guid WalletId, string ClientRequestId, long AmountMinor, string CurrencyCode = "SAR", string CashierReference = "cashier-receipt", string Reason = "cashier top-up", string ActorReference = "cashier-demo");
public sealed record PaymentConfirmationRequest(Guid? WalletTopUpId, string ProviderReference, string ProviderEventId, PaymentConfirmationStatus ConfirmationStatus, long AmountMinor, string CurrencyCode = "SAR", string SafePaymentMethodSummary = "Provider reference", string RawPayloadReference = "normalized-payload", string ProviderPayloadHash = "", string ProviderSignature = "");
public sealed record ChargebackReviewRequest(long AmountMinor, string Reason, string ActorReference = "finance-reviewer");
public sealed record TopUpResponse(Guid TopUpId, Guid WalletId, string StudentProfileId, WalletTopUpSource Source, long AmountMinor, long NetCreditMinor, string CurrencyCode, WalletTopUpStatus Status, string SafeReference, string ReviewReason);
public sealed record PaymentConfirmationResponse(Guid PaymentConfirmationId, Guid? WalletTopUpId, string ProviderReference, string ProviderEventId, PaymentConfirmationStatus Status, long AmountMinor, string CurrencyCode, string SafePaymentMethodSummary, string ReviewReason);
public sealed record TopUpTraceResponse(Guid TopUpId, IReadOnlyList<string> TraceReferences);

public static class WalletTopUpPaymentMappingExtensions
{
    public static TopUpResponse ToResponse(this WalletTopUp topUp) => new(topUp.Id, topUp.StudentWalletId, topUp.StudentProfileId, topUp.TopUpSource, topUp.AmountMinor, topUp.NetCreditMinor, topUp.CurrencyCode, topUp.TopUpStatus, string.IsNullOrWhiteSpace(topUp.PaymentProviderReference) ? topUp.CashierReference : topUp.PaymentProviderReference, topUp.ReviewReason);
}
