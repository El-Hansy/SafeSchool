using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public static class AnomaliesController
{
    public static RouteGroupBuilder MapAnomalyEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/anomaly-runs", async (string schoolAccountId, RunAnomalyDetectionRequest request, AnomalyDetectionService service, CancellationToken ct) =>
            Results.Ok(await service.RunAsync(schoolAccountId, request, ct)));
        var anomalies = group.MapGroup("/anomalies");
        anomalies.MapGet("/", async (string schoolAccountId, SafeSchoolDbContext dbContext, CancellationToken ct) =>
            Results.Ok(await dbContext.AttendanceAnomalies.Where(x => x.TenantId == schoolAccountId).Select(x => x.ToResponse()).ToListAsync(ct)));
        anomalies.MapPost("/{anomalyId:guid}/assign", async (string schoolAccountId, Guid anomalyId, AssignAnomalyRequest request, AnomalyReviewService service, CancellationToken ct) =>
        {
            var result = await service.AssignAsync(schoolAccountId, anomalyId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        anomalies.MapPost("/{anomalyId:guid}/resolve", async (string schoolAccountId, Guid anomalyId, ResolveAnomalyRequest request, AnomalyReviewService service, CancellationToken ct) =>
        {
            var result = await service.ResolveAsync(schoolAccountId, anomalyId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        anomalies.MapPost("/{anomalyId:guid}/dismiss", async (string schoolAccountId, Guid anomalyId, DismissAnomalyRequest request, AnomalyReviewService service, CancellationToken ct) =>
        {
            var result = await service.DismissAsync(schoolAccountId, anomalyId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        anomalies.MapPost("/{anomalyId:guid}/reopen", async (string schoolAccountId, Guid anomalyId, ResolveAnomalyRequest request, AnomalyReviewService service, CancellationToken ct) =>
        {
            var result = await service.ReopenAsync(schoolAccountId, anomalyId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}

