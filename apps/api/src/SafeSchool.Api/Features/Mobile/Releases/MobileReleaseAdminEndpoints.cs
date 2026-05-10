namespace SafeSchool.Api.Features.Mobile;

public static class MobileReleaseAdminEndpoints
{
    public static RouteGroupBuilder MapMobileReleaseAdminEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/releases", (CreateApkReleaseRequest request, ApkReleaseService service) => Results.Ok(service.Create(request)));
        group.MapPost("/releases/{releaseId}/approve", (string releaseId, ApproveReleaseRequest request, ApkReleaseService service) => Results.Ok(service.Approve(releaseId, request)));
        group.MapPost("/releases/{releaseId}/revoke", (string releaseId, RevokeReleaseRequest request, ApkReleaseService service) => Results.Ok(service.Revoke(releaseId, request)));
        return group;
    }
}
