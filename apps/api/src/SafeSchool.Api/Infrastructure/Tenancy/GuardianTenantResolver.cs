namespace SafeSchool.Api.Infrastructure.Tenancy;

public static class GuardianTenantResolver
{
    public const string DemoTenantFallback = "school-demo";

    public static string Resolve(ITenantContext tenantContext) =>
        string.IsNullOrWhiteSpace(tenantContext.TenantId)
            ? DemoTenantFallback
            : tenantContext.TenantId!;
}
