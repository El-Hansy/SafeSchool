using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Reviews;
using SafeSchool.Api.Features.Transport.Rules;

namespace SafeSchool.Api.Features.Transport.Anomalies;

public static class TransportAnomalyControllers
{
    public static RouteGroupBuilder MapTransportReviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/anomaly-runs", async (string schoolAccountId, TransportAnomalyRunRequest request, TransportAnomalyDetectionService service, CancellationToken ct) => Results.Ok(await service.RunAsync(schoolAccountId, request, ct)));
        group.MapPost("/anomalies/{anomalyId:guid}/assign", async (string schoolAccountId, Guid anomalyId, TransportAnomalyReviewService service, CancellationToken ct) => Results.Ok(await service.ChangeStatusAsync(schoolAccountId, anomalyId, AnomalyStatus.Assigned, "Assigned", "reviewer", ct)));
        group.MapPost("/anomalies/{anomalyId:guid}/resolve", async (string schoolAccountId, Guid anomalyId, TransportAnomalyReviewService service, CancellationToken ct) => Results.Ok(await service.ChangeStatusAsync(schoolAccountId, anomalyId, AnomalyStatus.Resolved, "Resolved", "reviewer", ct)));
        group.MapPost("/anomalies/{anomalyId:guid}/dismiss", async (string schoolAccountId, Guid anomalyId, TransportAnomalyReviewService service, CancellationToken ct) => Results.Ok(await service.ChangeStatusAsync(schoolAccountId, anomalyId, AnomalyStatus.Dismissed, "Dismissed", "reviewer", ct)));
        group.MapPost("/anomalies/{anomalyId:guid}/reopen", async (string schoolAccountId, Guid anomalyId, TransportAnomalyReviewService service, CancellationToken ct) => Results.Ok(await service.ChangeStatusAsync(schoolAccountId, anomalyId, AnomalyStatus.Reopened, "Reopened", "reviewer", ct)));
        group.MapManualReviewEndpoints();
        group.MapTransportRuleSettingsEndpoints();
        group.MapTransportReviewSummaryEndpoints();
        return group;
    }
}
