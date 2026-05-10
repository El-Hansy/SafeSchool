namespace SafeSchool.Api.Features.Learning.Stars;

public static class StarRewardEndpointExtensions
{
    public static RouteGroupBuilder MapStarRewardEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/stars", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "stars", status = "demo-ready" }));
        group.MapPost("/stars/source-events", (string schoolAccountId, PostStarSourceEventCommand request) => Results.Ok(new { schoolAccountId, ledgerReference = $"star-ledger-{request.ClientRequestId}", request.StudentProfileId, request.Points, status = "Posted", evidence = new[] { "source-event-linked", "append-only-ledger", "no-wallet-mutation" } }));
        group.MapGet("/star-ledger", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "star-ledger", status = "demo-ready" }));
        group.MapGet("/rewards", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "rewards", status = "demo-ready" }));
        group.MapGet("/reward-redemptions", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "reward-redemptions", status = "demo-ready" }));
        group.MapPost("/reward-redemptions", (string schoolAccountId, RedeemRewardCommand request) => Results.Ok(new { schoolAccountId, redemptionReference = $"reward-{request.ClientRequestId}", request.StudentProfileId, request.RewardReference, status = "Reserved", evidence = new[] { "balance-reserved", "guardian-visible", "no-wallet-mutation" } }));
        group.MapGet("/phase6-star-evidence", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "phase6-star-evidence", status = "demo-ready" }));
        group.MapGet("/star-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "star-trace", status = "demo-ready" }));
        return group;
    }
}

public sealed record PostStarSourceEventCommand(string StudentProfileId, int Points, string SourceReference, string ClientRequestId);
public sealed record RedeemRewardCommand(string StudentProfileId, string RewardReference, string ClientRequestId);
