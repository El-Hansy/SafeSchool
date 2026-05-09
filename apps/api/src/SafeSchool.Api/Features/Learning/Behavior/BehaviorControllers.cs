namespace SafeSchool.Api.Features.Learning.Behavior;

public static class BehaviorEndpointExtensions
{
    public static RouteGroupBuilder MapBehaviorEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/behavior-categories", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "behavior-categories", status = "demo-ready" }));
        group.MapGet("/behavior-events", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "behavior-events", status = "demo-ready" }));
        group.MapGet("/behavior-review", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "behavior-review", status = "demo-ready" }));
        group.MapGet("/behavior-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "behavior-trace", status = "demo-ready" }));
        return group;
    }
}
