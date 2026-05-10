using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileReleaseAdminEndpoints
{
    public static RouteGroupBuilder MapMobileReleaseAdminEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/releases", (string? tenantId, CreateApkReleaseRequest request, ITenantContext tenantContext, ApkReleaseService service) =>
        {
            if (MobileEndpointContext.HasTenantConflict(tenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            return Results.Ok(service.Create(
                request,
                MobileEndpointContext.Tenant(tenantId, tenantContext),
                MobileEndpointContext.User(null, tenantContext)));
        });
        group.MapPost("/releases/{releaseId}/approve", (string releaseId, ApproveReleaseRequest request, ApkReleaseService service) => Results.Ok(service.Approve(releaseId, request)));
        group.MapPost("/releases/{releaseId}/revoke", (string releaseId, RevokeReleaseRequest request, ApkReleaseService service) => Results.Ok(service.Revoke(releaseId, request)));
        return group;
    }
}
