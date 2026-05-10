using SafeSchool.Api.Features.Transport.Anomalies;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class ScanAnomalyCreationService(SafeSchoolDbContext dbContext)
{
    public async Task<TransportAnomaly?> CreateForDecisionAsync(BoardingDropScanEvent scan, CancellationToken cancellationToken = default)
    {
        if (scan.ScanDecision != TransportScanDecision.NeedsReview && scan.ScanDecision != TransportScanDecision.Flagged) return null;
        var anomaly = new TransportAnomaly { TenantId = scan.TenantId, TransportTripId = scan.TransportTripId, StudentProfileId = scan.StudentProfileId, TransportRouteId = scan.TransportRouteId, RouteStopSequenceId = scan.RouteStopSequenceId, SourceEventReference = scan.Id.ToString(), AnomalyType = scan.ScanDecision == TransportScanDecision.Flagged ? TransportAnomalyType.InvalidCredential : TransportAnomalyType.ManualReviewRequired, Severity = AnomalySeverity.High };
        dbContext.TransportAnomalies.Add(anomaly);
        await dbContext.SaveChangesAsync(cancellationToken);
        return anomaly;
    }
}
