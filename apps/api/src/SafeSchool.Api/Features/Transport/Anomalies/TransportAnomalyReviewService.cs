using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Anomalies;

public sealed class TransportAnomalyReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<TransportAnomaly>> ChangeStatusAsync(string tenantId, Guid anomalyId, AnomalyStatus status, string reason, string reviewer, CancellationToken cancellationToken = default)
    {
        var anomaly = await dbContext.TransportAnomalies.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == anomalyId, cancellationToken);
        if (anomaly is null) return OperationResult<TransportAnomaly>.Failure(new ValidationError("not_found", "Anomaly was not found."));
        anomaly.Status = status;
        anomaly.AssignedTo = status == AnomalyStatus.Assigned ? reviewer : anomaly.AssignedTo;
        anomaly.ResolutionReason = reason;
        anomaly.ResolvedBy = status is AnomalyStatus.Resolved or AnomalyStatus.Dismissed ? reviewer : string.Empty;
        anomaly.ResolvedAt = status is AnomalyStatus.Resolved or AnomalyStatus.Dismissed ? DateTimeOffset.UtcNow : null;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportAnomaly>.Success(anomaly);
    }
}
