using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileEndpointContext
{
    public const string DemoTenantFallback = "school-demo";
    public const string AnonymousUser = "anonymous";
    public const string UnknownDevice = "device-unknown";

    public static string Tenant(string? requestedTenantId, ITenantContext tenantContext) =>
        !string.IsNullOrWhiteSpace(requestedTenantId)
            ? requestedTenantId
            : string.IsNullOrWhiteSpace(tenantContext.TenantId)
                ? DemoTenantFallback
                : tenantContext.TenantId!;

    public static string User(string? requestedUserId, ITenantContext tenantContext) =>
        !string.IsNullOrWhiteSpace(requestedUserId)
            ? requestedUserId
            : string.IsNullOrWhiteSpace(tenantContext.ActorReference)
                ? AnonymousUser
                : tenantContext.ActorReference!;

    public static string Device(string? requestedDeviceId) =>
        string.IsNullOrWhiteSpace(requestedDeviceId) ? UnknownDevice : requestedDeviceId!;

    public static bool HasTenantConflict(string? requestedTenantId, ITenantContext tenantContext) =>
        !string.IsNullOrWhiteSpace(requestedTenantId) &&
        !string.IsNullOrWhiteSpace(tenantContext.TenantId) &&
        !string.Equals(requestedTenantId, tenantContext.TenantId, StringComparison.Ordinal) &&
        !tenantContext.HasPlatformReviewScope;
}
