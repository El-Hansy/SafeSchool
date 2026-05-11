using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class CapabilityEndpointFilters
{
    public static RouteHandlerBuilder RequireCapability(this RouteHandlerBuilder builder, string capabilityKey) =>
        builder.AddEndpointFilter(async (context, next) =>
        {
            var tenantContext = context.HttpContext.RequestServices.GetRequiredService<ITenantContext>();
            var featureGate = context.HttpContext.RequestServices.GetRequiredService<IFeatureGateService>();
            var routeTenant = context.HttpContext.Request.RouteValues["schoolAccountId"]?.ToString();
            var tenantId = string.IsNullOrWhiteSpace(routeTenant)
                ? GuardianTenantResolver.Resolve(tenantContext)
                : routeTenant!;

            if (!string.IsNullOrWhiteSpace(routeTenant) && !tenantContext.HasPlatformReviewScope)
            {
                if (string.IsNullOrWhiteSpace(tenantContext.TenantId))
                {
                    return Results.Json(new
                    {
                        code = "tenant_required",
                        message = "X-School-Account-Id is required for school-scoped feature access.",
                        tenantId,
                        capabilityKey
                    }, statusCode: StatusCodes.Status403Forbidden);
                }

                if (!string.Equals(tenantContext.TenantId, routeTenant, StringComparison.OrdinalIgnoreCase))
                {
                    return Results.Json(new
                    {
                        code = "tenant_mismatch",
                        message = "Tenant context does not match the requested school account.",
                        tenantId,
                        capabilityKey
                    }, statusCode: StatusCodes.Status403Forbidden);
                }
            }

            if (!featureGate.IsEnabled(tenantId, capabilityKey))
            {
                return Results.Json(new
                {
                    code = "feature_disabled",
                    message = featureGate.Explain(tenantId, capabilityKey),
                    tenantId,
                    capabilityKey
                }, statusCode: StatusCodes.Status403Forbidden);
            }

            return await next(context);
        });
}
