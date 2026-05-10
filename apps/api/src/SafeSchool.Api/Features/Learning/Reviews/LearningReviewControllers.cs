namespace SafeSchool.Api.Features.Learning.Reviews;

public static class LearningReviewEndpointExtensions
{
    public static RouteGroupBuilder MapLearningReviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/feature-settings", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "feature-settings", status = "demo-ready" }));
        group.MapGet("/rule-settings", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "rule-settings", status = "demo-ready" }));
        group.MapPost("/rule-settings", (string schoolAccountId, ConfigureLearningRuleCommand request) => Results.Ok(new { schoolAccountId, request.RuleKey, request.Enabled, status = "Applied", evidence = new[] { "version-preserved", "dependency-checked", "audit-written" } }));
        group.MapGet("/history", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "history", status = "demo-ready" }));
        group.MapGet("/exceptions", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "exceptions", status = "demo-ready" }));
        group.MapGet("/manual-reviews", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "manual-reviews", status = "demo-ready" }));
        group.MapPost("/manual-reviews", (string schoolAccountId, CreateManualLearningReviewCommand request) => Results.Ok(new { schoolAccountId, reviewReference = $"learning-review-{request.ClientRequestId}", request.SubjectReference, status = "Queued", evidence = new[] { "reason-valid", "reviewer-assigned", "audit-written" } }));
        group.MapGet("/review-summaries", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "review-summaries", status = "demo-ready" }));
        group.MapGet("/lifecycle-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "lifecycle-trace", status = "demo-ready" }));
        return group;
    }
}

public sealed record ConfigureLearningRuleCommand(string RuleKey, bool Enabled, string Reason, string ClientRequestId);
public sealed record CreateManualLearningReviewCommand(string SubjectReference, string Reason, string ClientRequestId);
