namespace SafeSchool.Api.Features.Mobile;

public static class MobileProfileEndpoints
{
    public static RouteGroupBuilder MapMobileProfileEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/profile", (string? tenantId, string? roleCode, string? languageCode, MobileProfileService service) =>
            Results.Ok(service.GetProfile(tenantId ?? "school-demo", roleCode ?? MobileRoleCodes.Guardian, "demo-user", languageCode ?? "en")));
        return group;
    }
}
