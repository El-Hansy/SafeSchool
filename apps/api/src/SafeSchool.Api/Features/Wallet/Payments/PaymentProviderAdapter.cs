using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.TopUps;

namespace SafeSchool.Api.Features.Wallet.Payments;

public sealed record NormalizedPaymentConfirmation(string ProviderReference, string ProviderEventId, PaymentConfirmationStatus Status, long AmountMinor, string CurrencyCode, string SafePaymentMethodSummary, string RawPayloadReference);

public interface IPaymentProviderAdapter
{
    NormalizedPaymentConfirmation Normalize(PaymentConfirmationRequest request);
}

public sealed class DemoPaymentProviderAdapter : IPaymentProviderAdapter
{
    public NormalizedPaymentConfirmation Normalize(PaymentConfirmationRequest request)
    {
        if (request.SafePaymentMethodSummary.Contains("card_number", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Full external payment credentials are not accepted by the wallet boundary.");
        }
        return new NormalizedPaymentConfirmation(request.ProviderReference, request.ProviderEventId, request.ConfirmationStatus, request.AmountMinor, request.CurrencyCode, request.SafePaymentMethodSummary, request.RawPayloadReference);
    }
}
