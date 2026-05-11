using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Reviews;

public static class LearningReviewEndpointExtensions
{
    public static RouteGroupBuilder MapLearningReviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/feature-settings", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.FeatureSettingsAsync(schoolAccountId, ct)));
        group.MapGet("/rule-settings", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.RuleSettingsAsync(schoolAccountId, ct)));
        group.MapPost("/rule-settings", async (string schoolAccountId, ConfigureLearningRuleCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ConfigureRuleAsync(schoolAccountId, request, ct)));
        group.MapGet("/history", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.HistoryAsync(schoolAccountId, ct)));
        group.MapGet("/exceptions", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ExceptionsAsync(schoolAccountId, ct)));
        group.MapGet("/manual-reviews", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ManualReviewsAsync(schoolAccountId, ct)));
        group.MapPost("/manual-reviews", async (string schoolAccountId, CreateManualLearningReviewCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.CreateManualReviewAsync(schoolAccountId, request, ct)));
        group.MapGet("/review-summaries", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ReviewSummariesAsync(schoolAccountId, ct)));
        group.MapGet("/lifecycle-trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.LifecycleTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record ConfigureLearningRuleCommand(string RuleKey, bool Enabled, string Reason, string ClientRequestId);
public sealed record CreateManualLearningReviewCommand(string SubjectReference, string Reason, string ClientRequestId);
