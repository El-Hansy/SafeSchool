namespace SafeSchool.Api.Features.Mobile;

public static class GuardianMobileEndpoints
{
    public static RouteGroupBuilder MapGuardianMobileEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/guardian/summaries", (GuardianMobileService service) => Results.Ok(service.Summaries()));
        return group;
    }
}
