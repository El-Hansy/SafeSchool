using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileProfileEndpoints
{
    public static RouteGroupBuilder MapMobileProfileEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/profile", (string? tenantId, string? userId, string? roleCode, string? languageCode, ITenantContext tenantContext, MobileProfileService service) =>
        {
            if (MobileEndpointContext.HasTenantConflict(tenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            return Results.Ok(service.GetProfile(
                MobileEndpointContext.Tenant(tenantId, tenantContext),
                roleCode ?? MobileRoleCodes.Guardian,
                MobileEndpointContext.User(userId, tenantContext),
                languageCode ?? "en"));
        });
        return group;
    }
}
