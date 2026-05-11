using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Payments;
using SafeSchool.Api.Features.Wallet.TopUps;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Wallet.Payments;

public sealed class PaymentProviderAdapterTests
{
    [Fact]
    public void ConfiguredAdapter_NormalizesSafeProviderConfirmation()
    {
        var adapter = new ConfiguredPaymentProviderAdapter(BuildConfiguration(new Dictionary<string, string?>
        {
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider",
            ["Wallet:PaymentProvider:StoreRawPayloads"] = "false"
        }));
        var request = new PaymentConfirmationRequest(
            Guid.NewGuid(),
            " pay_ref_123 ",
            " event_123 ",
            PaymentConfirmationStatus.Successful,
            5000,
            "sar",
            "Mada ending 1234",
            "provider-payload-ref-123");

        var normalized = adapter.Normalize(request);

        normalized.ProviderReference.Should().Be("pay_ref_123");
        normalized.ProviderEventId.Should().Be("event_123");
        normalized.CurrencyCode.Should().Be("SAR");
    }

    [Fact]
    public void ConfiguredAdapter_RejectsSensitivePaymentCredentials()
    {
        var adapter = new ConfiguredPaymentProviderAdapter(BuildConfiguration(new Dictionary<string, string?>
        {
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider"
        }));
        var request = new PaymentConfirmationRequest(
            Guid.NewGuid(),
            "pay_ref_123",
            "event_123",
            PaymentConfirmationStatus.Successful,
            5000,
            "SAR",
            "card_number=4111111111111111",
            "provider-payload-ref-123");

        var action = () => adapter.Normalize(request);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Full external payment credentials*");
    }

    [Fact]
    public void ConfiguredAdapter_RejectsRawPayloadWhenStorageIsDisabled()
    {
        var adapter = new ConfiguredPaymentProviderAdapter(BuildConfiguration(new Dictionary<string, string?>
        {
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider",
            ["Wallet:PaymentProvider:StoreRawPayloads"] = "false"
        }));
        var request = new PaymentConfirmationRequest(
            Guid.NewGuid(),
            "pay_ref_123",
            "event_123",
            PaymentConfirmationStatus.Successful,
            5000,
            "SAR",
            "Mada ending 1234",
            "{\"provider\":\"raw\"}");

        var action = () => adapter.Normalize(request);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Raw payment payloads are disabled*");
    }

    [Fact]
    public void ConfiguredAdapter_AcceptsSignedProviderConfirmation()
    {
        const string secret = "wallet-provider-secret";
        const string payloadHash = "payload-sha256";
        var adapter = new ConfiguredPaymentProviderAdapter(BuildConfiguration(new Dictionary<string, string?>
        {
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider",
            ["Wallet:PaymentProvider:RequireWebhookSignature"] = "true",
            ["Wallet:PaymentProvider:WebhookSigningSecret"] = secret
        }));
        var request = new PaymentConfirmationRequest(
            Guid.NewGuid(),
            "pay_ref_123",
            "event_123",
            PaymentConfirmationStatus.Successful,
            5000,
            "SAR",
            "Mada ending 1234",
            "provider-payload-ref-123",
            payloadHash,
            Sign(secret, "event_123", "pay_ref_123", 5000, "SAR", payloadHash));

        var normalized = adapter.Normalize(request);

        normalized.ProviderEventId.Should().Be("event_123");
    }

    [Fact]
    public void ConfiguredAdapter_RejectsInvalidProviderSignature()
    {
        var adapter = new ConfiguredPaymentProviderAdapter(BuildConfiguration(new Dictionary<string, string?>
        {
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider",
            ["Wallet:PaymentProvider:RequireWebhookSignature"] = "true",
            ["Wallet:PaymentProvider:WebhookSigningSecret"] = "wallet-provider-secret"
        }));
        var request = new PaymentConfirmationRequest(
            Guid.NewGuid(),
            "pay_ref_123",
            "event_123",
            PaymentConfirmationStatus.Successful,
            5000,
            "SAR",
            "Mada ending 1234",
            "provider-payload-ref-123",
            "payload-sha256",
            "sha256=bad-signature");

        var action = () => adapter.Normalize(request);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*signature is invalid*");
    }

    private static IConfiguration BuildConfiguration(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    private static string Sign(string secret, string eventId, string providerReference, long amountMinor, string currencyCode, string payloadHash)
    {
        var signedContent = string.Join('.', eventId, providerReference, amountMinor, currencyCode, payloadHash);
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return "sha256=" + Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(signedContent))).ToLowerInvariant();
    }
}
