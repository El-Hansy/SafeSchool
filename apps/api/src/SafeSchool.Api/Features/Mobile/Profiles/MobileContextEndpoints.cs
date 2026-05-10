namespace SafeSchool.Api.Features.Mobile;

public static class MobileContextEndpoints
{
    public static RouteGroupBuilder MapMobileContextEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/context", (ActiveMobileContextRequest request, RoleWorkspaceResolver resolver, DeviceSessionService sessions, MobilePermissionResolver permissions) =>
        {
            var reason = permissions.DeniedReason(request.TenantId, request.RoleCode, request.RoleCode);
            if (reason != "OK") return Results.Json(new { error = reason }, statusCode: StatusCodes.Status403Forbidden);
            sessions.Start(request.TenantId, "demo-user", request.DeviceId, request.RoleCode, request.LanguageCode);
            var summary = resolver.ResolveOne(request.TenantId, request.RoleCode, request.LanguageCode);
            return Results.Ok(new MobileContextResponse(request.TenantId, request.RoleCode, request.LanguageCode, MobileLanguageService.DirectionFor(request.LanguageCode), summary));
        });
        return group;
    }
}
