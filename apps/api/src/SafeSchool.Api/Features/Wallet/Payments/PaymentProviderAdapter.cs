using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.TopUps;
using SafeSchool.Api.Infrastructure.Configuration;

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
        var safePaymentMethodSummary = request.SafePaymentMethodSummary ?? string.Empty;
        if (safePaymentMethodSummary.Contains("card_number", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Full external payment credentials are not accepted by the wallet boundary.");
        }
        return new NormalizedPaymentConfirmation(request.ProviderReference, request.ProviderEventId, request.ConfirmationStatus, request.AmountMinor, request.CurrencyCode, safePaymentMethodSummary, request.RawPayloadReference ?? string.Empty);
    }
}

public sealed class ConfiguredPaymentProviderAdapter(IConfiguration configuration) : IPaymentProviderAdapter
{
    private static readonly string[] SensitivePaymentMarkers =
    [
        "card_number",
        "cardNumber",
        "pan",
        "cvv",
        "cvc",
        "expiry",
        "expiration",
        "411111",
        "424242"
    ];

    public NormalizedPaymentConfirmation Normalize(PaymentConfirmationRequest request)
    {
        var adapterName = configuration["Wallet:PaymentProvider:Adapter"];
        if (RuntimeConfigurationValidator.IsDemoPaymentProvider(adapterName))
        {
            throw new InvalidOperationException("Configured payment adapter cannot normalize DemoPay events.");
        }

        if (string.IsNullOrWhiteSpace(request.ProviderReference))
        {
            throw new InvalidOperationException("Payment confirmation requires a provider reference.");
        }

        if (string.IsNullOrWhiteSpace(request.ProviderEventId))
        {
            throw new InvalidOperationException("Payment confirmation requires a provider event id for idempotency.");
        }

        if (request.AmountMinor <= 0)
        {
            throw new InvalidOperationException("Payment confirmation amount must be positive.");
        }

        if (string.IsNullOrWhiteSpace(request.CurrencyCode))
        {
            throw new InvalidOperationException("Payment confirmation currency is required.");
        }

        if (string.IsNullOrWhiteSpace(request.SafePaymentMethodSummary))
        {
            throw new InvalidOperationException("Payment confirmation requires a safe payment method summary.");
        }

        if (string.IsNullOrWhiteSpace(request.RawPayloadReference))
        {
            throw new InvalidOperationException("Payment confirmation requires a normalized raw payload reference.");
        }

        if (ContainsSensitivePaymentData(request.SafePaymentMethodSummary))
        {
            throw new InvalidOperationException("Full external payment credentials are not accepted by the wallet boundary.");
        }

        if (!configuration.GetValue<bool>("Wallet:PaymentProvider:StoreRawPayloads") &&
            LooksLikeRawPayload(request.RawPayloadReference))
        {
            throw new InvalidOperationException("Raw payment payloads are disabled; provide a normalized payload reference instead.");
        }

        return new NormalizedPaymentConfirmation(
            request.ProviderReference.Trim(),
            request.ProviderEventId.Trim(),
            request.ConfirmationStatus,
            request.AmountMinor,
            request.CurrencyCode.Trim().ToUpperInvariant(),
            request.SafePaymentMethodSummary.Trim(),
            request.RawPayloadReference.Trim());
    }

    private static bool ContainsSensitivePaymentData(string value) =>
        SensitivePaymentMarkers.Any(marker => value.Contains(marker, StringComparison.OrdinalIgnoreCase));

    private static bool LooksLikeRawPayload(string value)
    {
        var trimmed = value.TrimStart();
        return trimmed.StartsWith('{') || trimmed.StartsWith('[');
    }
}
