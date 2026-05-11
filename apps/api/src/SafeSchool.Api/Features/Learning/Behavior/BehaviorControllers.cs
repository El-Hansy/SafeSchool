using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Behavior;

public static class BehaviorEndpointExtensions
{
    public static RouteGroupBuilder MapBehaviorEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/behavior-categories", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.BehaviorCategoriesAsync(schoolAccountId, ct)));
        group.MapGet("/behavior-events", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.BehaviorEventsAsync(schoolAccountId, ct)));
        group.MapPost("/behavior-events", async (string schoolAccountId, LogBehaviorEventCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.LogBehaviorEventAsync(schoolAccountId, request, ct)));
        group.MapGet("/behavior-review", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.BehaviorReviewAsync(schoolAccountId, ct)));
        group.MapGet("/behavior-trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.BehaviorTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record LogBehaviorEventCommand(string StudentProfileId, string CategoryCode, string Summary, string ClientRequestId);
