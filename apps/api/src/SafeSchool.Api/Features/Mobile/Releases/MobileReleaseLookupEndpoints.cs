namespace SafeSchool.Api.Features.Mobile;

public static class MobileReleaseLookupEndpoints
{
    public static RouteGroupBuilder MapMobileReleaseLookupEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/releases/current", (string tenantId, string? roleCode, string? deviceId, int? versionCode, ApkReleaseService service) =>
            Results.Ok(service.Current(tenantId, roleCode, versionCode)));
        return group;
    }
}
