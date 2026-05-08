using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Idempotency;
using SafeSchool.Api.Features.Transport.Scans;
using SafeSchool.Api.Features.Transport.Trips;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Sync;

public sealed class OfflineTransportScanSyncService(SafeSchoolDbContext dbContext, BoardingDropScanValidationService validationService, TransportStatusDecisionService statusDecisionService, TransportIdempotencyService idempotencyService)
{
    public async Task<BoardingDropScanResponse> RecordAsync(string tenantId, BoardingDropScanRequest request, bool offlineCaptured = false, CancellationToken cancellationToken = default)
    {
        var validation = await validationService.ValidateAsync(tenantId, request, cancellationToken);
        var trip = await dbContext.TransportTrips.FindAsync([request.TransportTripId], cancellationToken);
        var scan = new BoardingDropScanEvent { TenantId = tenantId, ClientScanId = request.ClientScanId, TransportTripId = request.TransportTripId, TransportRouteId = trip?.TransportRouteId ?? Guid.Empty, RouteStopSequenceId = request.RouteStopSequenceId, StudentProfileId = validation.StudentProfileId, StudentTransportAssignmentId = validation.AssignmentId, CredentialReference = request.CredentialReference, ScanDirection = request.ScanDirection, ScanMethod = request.ScanMethod, LocalScanTime = request.LocalScanTime, OfflineCaptured = offlineCaptured, ScanDecision = validation.Decision, DecisionReason = validation.Reason };
        scan.TransportStatusAfter = statusDecisionService.Calculate(scan.ScanDirection, scan.ScanDecision);
        var idem = idempotencyService.RegisterScan(tenantId, request.ClientScanId, $"{request.TransportTripId}:{request.LocalScanTime:o}", scan.Id.ToString());
        if (idem.IsRetry)
        {
            scan.ScanDecision = TransportScanDecision.Duplicate;
            scan.SyncStatus = TransportSyncStatus.Duplicate;
        }
        dbContext.BoardingDropScanEvents.Add(scan);
        await dbContext.SaveChangesAsync(cancellationToken);
        return scan.ToResponse();
    }

    public async Task<OfflineTransportScanSyncResponse> SyncAsync(string tenantId, OfflineTransportScanSyncRequest request, CancellationToken cancellationToken = default)
    {
        var items = new List<BoardingDropScanResponse>();
        foreach (var scan in request.Scans)
        {
            items.Add(await RecordAsync(tenantId, scan, true, cancellationToken));
        }
        var batch = new OfflineTransportScanSyncBatch { TenantId = tenantId, ClientBatchId = request.ClientBatchId, TransportTripId = request.TransportTripId, TrackingDeviceReference = request.TrackingDeviceReference, BatchStatus = TransportSyncStatus.Reconciled, AcceptedCount = items.Count(x => x.ScanDecision == TransportScanDecision.Accepted), NeedsReviewCount = items.Count(x => x.ScanDecision == TransportScanDecision.NeedsReview), DuplicateCount = items.Count(x => x.ScanDecision == TransportScanDecision.Duplicate), RejectedCount = items.Count(x => x.ScanDecision == TransportScanDecision.Denied) };
        dbContext.OfflineTransportScanSyncBatches.Add(batch);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new OfflineTransportScanSyncResponse(batch.ClientBatchId, batch.BatchStatus, batch.AcceptedCount, batch.NeedsReviewCount, batch.DuplicateCount, batch.RejectedCount, items);
    }
}
