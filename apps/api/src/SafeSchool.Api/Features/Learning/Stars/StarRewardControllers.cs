namespace SafeSchool.Api.Features.Learning.Stars;

public static class StarRewardEndpointExtensions
{
    public static RouteGroupBuilder MapStarRewardEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/stars", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "stars", status = "demo-ready" }));
        group.MapGet("/star-ledger", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "star-ledger", status = "demo-ready" }));
        group.MapGet("/rewards", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "rewards", status = "demo-ready" }));
        group.MapGet("/reward-redemptions", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "reward-redemptions", status = "demo-ready" }));
        group.MapGet("/phase6-star-evidence", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "phase6-star-evidence", status = "demo-ready" }));
        group.MapGet("/star-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "star-trace", status = "demo-ready" }));
        return group;
    }
}
