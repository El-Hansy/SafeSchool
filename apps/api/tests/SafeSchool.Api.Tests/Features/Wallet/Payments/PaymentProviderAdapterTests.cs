using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Payments;
using SafeSchool.Api.Features.Wallet.TopUps;
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

    private static IConfiguration BuildConfiguration(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();
}
