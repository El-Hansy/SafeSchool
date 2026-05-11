using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Infrastructure.Configuration;
using Xunit;

namespace SafeSchool.Api.Tests.Infrastructure.Configuration;

public sealed class RuntimeConfigurationValidatorTests
{
    [Fact]
    public void Validate_RejectsUnsafeProductionDefaults()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Demo:AllowAnonymousApi"] = "true",
            ["Jwt:Authority"] = "https://identity.local",
            ["Jwt:Audience"] = "",
            ["ConnectionStrings:SafeSchool"] = "Host=localhost;Database=safeschool;Username=safeschool;Password=safeschool",
            ["Wallet:PaymentProvider:Adapter"] = "DemoPay"
        });

        var action = () => RuntimeConfigurationValidator.Validate(configuration, "Production");

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*AllowAnonymousApi*Jwt:Authority*Jwt:Audience*ConnectionStrings:SafeSchool*DemoPay*");
    }

    [Fact]
    public void Validate_AllowsDevelopmentDemoConfiguration()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Demo:AllowAnonymousApi"] = "true",
            ["Jwt:Authority"] = "https://identity.local",
            ["Wallet:PaymentProvider:Adapter"] = "DemoPay"
        });

        var action = () => RuntimeConfigurationValidator.Validate(configuration, "Development");

        action.Should().NotThrow();
    }

    [Fact]
    public void Validate_AllowsProductionConfigurationWithRealIdentityDatabaseAndProvider()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Demo:AllowAnonymousApi"] = "false",
            ["Jwt:Authority"] = "https://login.safeschool.example",
            ["Jwt:Audience"] = "safeschool-api",
            ["ConnectionStrings:SafeSchool"] = "Host=safeschool-postgres.internal;Database=safeschool;Username=api;Password=secret",
            ["Wallet:PaymentProvider:Adapter"] = "ConfiguredProvider"
        });

        var action = () => RuntimeConfigurationValidator.Validate(configuration, "Production");

        action.Should().NotThrow();
    }

    private static IConfiguration BuildConfiguration(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();
}
