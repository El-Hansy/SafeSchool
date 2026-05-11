using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Stars;

public static class StarRewardEndpointExtensions
{
    public static RouteGroupBuilder MapStarRewardEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/stars", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.StarsAsync(schoolAccountId, ct)));
        group.MapPost("/stars/source-events", async (string schoolAccountId, PostStarSourceEventCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.PostStarSourceEventAsync(schoolAccountId, request, ct)));
        group.MapGet("/star-ledger", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.StarLedgerAsync(schoolAccountId, ct)));
        group.MapGet("/rewards", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.RewardsAsync(schoolAccountId, ct)));
        group.MapGet("/reward-redemptions", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.RewardRedemptionsAsync(schoolAccountId, ct)));
        group.MapPost("/reward-redemptions", async (string schoolAccountId, RedeemRewardCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.RedeemRewardAsync(schoolAccountId, request, ct)));
        group.MapGet("/phase6-star-evidence", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.Phase6StarEvidenceAsync(schoolAccountId, ct)));
        group.MapGet("/star-trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.StarTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record PostStarSourceEventCommand(string StudentProfileId, int Points, string SourceReference, string ClientRequestId);
public sealed record RedeemRewardCommand(string StudentProfileId, string RewardReference, string ClientRequestId);
