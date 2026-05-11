using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class CapabilityEndpointFilters
{
    public static RouteHandlerBuilder RequireCapability(this RouteHandlerBuilder builder, string capabilityKey, string? permissionKey = null) =>
        builder.AddEndpointFilter(async (context, next) =>
        {
            var tenantContext = context.HttpContext.RequestServices.GetRequiredService<ITenantContext>();
            var featureGate = context.HttpContext.RequestServices.GetRequiredService<IFeatureGateService>();
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
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

            if (!string.IsNullOrWhiteSpace(permissionKey))
            {
                var permissions = ReadActorPermissions(context.HttpContext);
                if (configuration.GetValue<bool>("Demo:AllowAnonymousApi") && permissions.Count == 0)
                {
                    permissions.Add("*");
                }

                if (!permissions.Contains("*") && !permissions.Contains(permissionKey))
                {
                    await RecordDeniedAccessAsync(context.HttpContext, tenantId, capabilityKey, permissionKey, "Actor lacks the required permission.");
                    return Results.Json(new
                    {
                        code = "missing_permission",
                        message = $"Actor lacks {permissionKey}.",
                        tenantId,
                        capabilityKey,
                        permissionKey
                    }, statusCode: StatusCodes.Status403Forbidden);
                }
            }

            return await next(context);
        });

    private static HashSet<string> ReadActorPermissions(HttpContext httpContext)
    {
        var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var value in httpContext.Request.Headers["X-Actor-Permissions"])
        {
            AddPermissionValues(permissions, value);
        }

        foreach (var claim in httpContext.User.Claims.Where(x => x.Type is "permission" or "permissions" or "scope" or "scp"))
        {
            AddPermissionValues(permissions, claim.Value);
        }

        return permissions;
    }

    private static void AddPermissionValues(HashSet<string> permissions, string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue)) return;
        foreach (var permission in rawValue.Split([',', ' ', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            permissions.Add(permission);
        }
    }

    private static async Task RecordDeniedAccessAsync(HttpContext httpContext, string tenantId, string capabilityKey, string permissionKey, string reason)
    {
        try
        {
            var tenantContext = httpContext.RequestServices.GetRequiredService<ITenantContext>();
            var writer = httpContext.RequestServices.GetService<IAccessDecisionWriter>();
            if (writer is null) return;

            await writer.RecordAsync(new AccessDecision
            {
                TenantId = tenantId,
                ActorReference = tenantContext.ActorReference ?? "unknown",
                AttemptedAction = permissionKey,
                TargetType = "FeatureEndpoint",
                TargetReference = httpContext.Request.Path,
                Decision = AccessDecisionResult.Denied,
                DecisionReason = reason,
                FeatureCapabilityKey = capabilityKey,
                DecidedAt = DateTimeOffset.UtcNow
            }, httpContext.RequestAborted);
        }
        catch
        {
            // Access denial must fail closed even when audit storage is unavailable.
        }
    }
}
