namespace SafeSchool.Api.Features.Mobile;

public static class MobileOfflineActionEndpoints
{
    public static RouteGroupBuilder MapMobileOfflineActionEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/offline-actions", (OfflineActionRequest request, MobileOfflineActionSyncService service) => Results.Ok(service.Sync(request)));
        group.MapGet("/offline-actions", (MobileOfflineActionSyncService service) => Results.Ok(service.List()));
        return group;
    }
}
