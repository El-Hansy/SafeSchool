namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public interface IFeatureGateService
{
    bool IsEnabled(string tenantId, string capabilityKey);
    string Explain(string tenantId, string capabilityKey);
}

public sealed class FeatureGateService(IConfiguration configuration) : IFeatureGateService
{
    public bool IsEnabled(string tenantId, string capabilityKey)
    {
        var configured = configuration[$"FeatureSettings:{tenantId}:{capabilityKey}"];
        return string.Equals(configured, "Enabled", StringComparison.OrdinalIgnoreCase)
            || string.Equals(configuration["FeatureSettings:DefaultAvailability"], "Enabled", StringComparison.OrdinalIgnoreCase);
    }

    public string Explain(string tenantId, string capabilityKey) =>
        IsEnabled(tenantId, capabilityKey)
            ? $"{capabilityKey} is enabled for {tenantId}."
            : $"{capabilityKey} is not enabled for {tenantId}.";
}
