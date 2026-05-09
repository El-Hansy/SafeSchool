using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Anomalies;

public sealed record TransportAnomalyRunRequest(Guid? TransportTripId, Guid? TransportRouteId, DateOnly? TripDate, IReadOnlyList<Common.TransportAnomalyType> IncludeAnomalyTypes, string RequestedReason, string ClientRequestId);

public sealed class TransportAnomalyDetectionService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<TransportAnomaly>> RunAsync(string tenantId, TransportAnomalyRunRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.TransportAnomalies.Where(x => x.TenantId == tenantId && x.TransportTripId == request.TransportTripId && x.Status != Common.AnomalyStatus.Resolved).ToListAsync(cancellationToken);
        if (existing.Count > 0) return existing;
        var anomaly = new TransportAnomaly { TenantId = tenantId, TransportTripId = request.TransportTripId, TransportRouteId = request.TransportRouteId, AnomalyType = request.IncludeAnomalyTypes.FirstOrDefault(Common.TransportAnomalyType.ManualReviewRequired), Severity = Common.AnomalySeverity.Medium, SourceEventReference = request.ClientRequestId };
        dbContext.TransportAnomalies.Add(anomaly);
        await dbContext.SaveChangesAsync(cancellationToken);
        return [anomaly];
    }
}
