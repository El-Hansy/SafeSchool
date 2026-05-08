using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<AnomalyResponse>> AssignAsync(string tenantId, Guid anomalyId, AssignAnomalyRequest request, CancellationToken cancellationToken = default)
    {
        var anomaly = await LoadAsync(tenantId, anomalyId, cancellationToken);
        if (anomaly is null) return OperationResult<AnomalyResponse>.Failure(new ValidationError("not_found", "Anomaly was not found."));
        anomaly.AssignedTo = request.ReviewerReference;
        anomaly.Status = AnomalyStatus.Assigned;
        anomaly.ResolutionReason = request.Reason;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<AnomalyResponse>.Success(anomaly.ToResponse());
    }

    public Task<OperationResult<AnomalyResponse>> ResolveAsync(string tenantId, Guid anomalyId, ResolveAnomalyRequest request, CancellationToken cancellationToken = default) =>
        SetStatusAsync(tenantId, anomalyId, AnomalyStatus.Resolved, request.Reason, cancellationToken);

    public Task<OperationResult<AnomalyResponse>> DismissAsync(string tenantId, Guid anomalyId, DismissAnomalyRequest request, CancellationToken cancellationToken = default) =>
        SetStatusAsync(tenantId, anomalyId, AnomalyStatus.Dismissed, request.Reason, cancellationToken);

    public Task<OperationResult<AnomalyResponse>> ReopenAsync(string tenantId, Guid anomalyId, ResolveAnomalyRequest request, CancellationToken cancellationToken = default) =>
        SetStatusAsync(tenantId, anomalyId, AnomalyStatus.Reopened, request.Reason, cancellationToken);

    private Task<AttendanceAnomaly?> LoadAsync(string tenantId, Guid anomalyId, CancellationToken cancellationToken) =>
        dbContext.AttendanceAnomalies.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == anomalyId, cancellationToken);

    private async Task<OperationResult<AnomalyResponse>> SetStatusAsync(string tenantId, Guid anomalyId, AnomalyStatus status, string reason, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return OperationResult<AnomalyResponse>.Failure(new ValidationError("missing_reason", "Anomaly review action requires a reason."));
        }

        var anomaly = await LoadAsync(tenantId, anomalyId, cancellationToken);
        if (anomaly is null)
        {
            return OperationResult<AnomalyResponse>.Failure(new ValidationError("not_found", "Anomaly was not found."));
        }

        if (!anomaly.CanTransitionTo(status))
        {
            return OperationResult<AnomalyResponse>.Failure(new ValidationError("invalid_transition", "Anomaly cannot move to the requested status."));
        }

        anomaly.Status = status;
        anomaly.ResolutionReason = reason;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<AnomalyResponse>.Success(anomaly.ToResponse());
    }
}

