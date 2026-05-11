using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SafeSchool.Api.Infrastructure.Configuration;

public static class RuntimeConfigurationValidator
{
    private static readonly string[] UnsafeJwtAuthorities =
    [
        "https://identity.local",
        "http://identity.local",
        "https://localhost",
        "http://localhost"
    ];

    private const string DefaultConnectionString = "Host=localhost;Database=safeschool;Username=safeschool;Password=safeschool";

    public static void Validate(IConfiguration configuration, IHostEnvironment environment) =>
        Validate(configuration, environment.EnvironmentName);

    public static void Validate(IConfiguration configuration, string environmentName)
    {
        if (string.Equals(environmentName, Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var errors = new List<string>();
        if (configuration.GetValue<bool>("Demo:AllowAnonymousApi"))
        {
            errors.Add("Demo:AllowAnonymousApi must be false outside Development.");
        }

        var jwtAuthority = configuration["Jwt:Authority"];
        if (string.IsNullOrWhiteSpace(jwtAuthority) || IsUnsafeJwtAuthority(jwtAuthority))
        {
            errors.Add("Jwt:Authority must be a real HTTPS identity provider outside Development.");
        }

        if (string.IsNullOrWhiteSpace(configuration["Jwt:Audience"]))
        {
            errors.Add("Jwt:Audience is required outside Development.");
        }

        var connectionString = configuration.GetConnectionString("SafeSchool");
        if (string.IsNullOrWhiteSpace(connectionString) ||
            string.Equals(connectionString, DefaultConnectionString, StringComparison.OrdinalIgnoreCase))
        {
            errors.Add("ConnectionStrings:SafeSchool must be configured for a non-default PostgreSQL database outside Development.");
        }

        var paymentProviderAdapter = configuration["Wallet:PaymentProvider:Adapter"];
        if (string.IsNullOrWhiteSpace(paymentProviderAdapter))
        {
            errors.Add("Wallet:PaymentProvider:Adapter is required outside Development.");
        }
        else if (IsDemoPaymentProvider(paymentProviderAdapter) &&
                 !configuration.GetValue<bool>("Demo:AllowDemoPaymentProvider"))
        {
            errors.Add("Wallet:PaymentProvider:Adapter cannot use DemoPay outside Development unless Demo:AllowDemoPaymentProvider is explicitly true.");
        }
        else if (!IsDemoPaymentProvider(paymentProviderAdapter) &&
                 configuration.GetValue<bool>("Wallet:PaymentProvider:RequireWebhookSignature") &&
                 string.IsNullOrWhiteSpace(configuration["Wallet:PaymentProvider:WebhookSigningSecret"]))
        {
            errors.Add("Wallet:PaymentProvider:WebhookSigningSecret is required when webhook signatures are required outside Development.");
        }

        if (errors.Count > 0)
        {
            throw new InvalidOperationException("SafeSchool API runtime configuration is not production-ready: " + string.Join(" ", errors));
        }
    }

    public static bool IsDemoPaymentProvider(string? adapter) =>
        string.Equals(adapter, "DemoPay", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(adapter, "LocalDemo", StringComparison.OrdinalIgnoreCase);

    private static bool IsUnsafeJwtAuthority(string authority)
    {
        if (!Uri.TryCreate(authority, UriKind.Absolute, out var uri) ||
            !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return UnsafeJwtAuthorities.Any(unsafeAuthority =>
            authority.StartsWith(unsafeAuthority, StringComparison.OrdinalIgnoreCase));
    }
}
