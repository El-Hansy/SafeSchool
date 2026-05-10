using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class StaffWorkspaceEndpoints
{
    public static RouteGroupBuilder MapStaffWorkspaceEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/staff/workspaces", (StaffWorkspaceService service) => Results.Ok(service.AllStaffWorkspaces()));
        group.MapPost("/actions/{actionCode}", (string actionCode, string? tenantId, string? userId, string? deviceId, MobileActionRequest request, ITenantContext tenantContext, SourceDomainActionAdapters actions) =>
        {
            if (MobileEndpointContext.HasTenantConflict(tenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            return Results.Ok(actions.Execute(
                MobileEndpointContext.Tenant(tenantId, tenantContext),
                MobileEndpointContext.User(userId, tenantContext),
                MobileEndpointContext.Device(deviceId),
                request.WorkspaceCode,
                request with { ClientActionId = string.IsNullOrWhiteSpace(request.ClientActionId) ? actionCode : request.ClientActionId }));
        });
        return group;
    }
}
