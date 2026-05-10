using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileContextEndpoints
{
    public static RouteGroupBuilder MapMobileContextEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/context", (ActiveMobileContextRequest request, ITenantContext tenantContext, RoleWorkspaceResolver resolver, DeviceSessionService sessions, MobilePermissionResolver permissions) =>
        {
            if (MobileEndpointContext.HasTenantConflict(request.TenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            var userId = MobileEndpointContext.User(null, tenantContext);
            var reason = permissions.DeniedReason(request.TenantId, request.RoleCode, request.RoleCode);
            if (reason != "OK") return Results.Json(new { error = reason }, statusCode: StatusCodes.Status403Forbidden);
            sessions.Start(request.TenantId, userId, request.DeviceId, request.RoleCode, request.LanguageCode);
            var summary = resolver.ResolveOne(request.TenantId, request.RoleCode, request.LanguageCode);
            return Results.Ok(new MobileContextResponse(request.TenantId, request.RoleCode, request.LanguageCode, MobileLanguageService.DirectionFor(request.LanguageCode), summary));
        });
        return group;
    }
}
