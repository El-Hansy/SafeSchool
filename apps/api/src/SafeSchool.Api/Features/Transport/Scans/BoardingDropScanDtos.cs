using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed record ScanReadyTripRequest(Guid TransportRouteId, Guid TransportVehicleId, string TrackingDeviceReference, ServiceDirection ServiceDirection, DateOnly TripDate, DateTimeOffset PlannedStartTime, string DriverReference, string AttendantReference, string SupervisorReference, string ClientRequestId);
public sealed record ScanReadyTripResponse(Guid TransportTripId, string SchoolAccountId, Guid TransportRouteId, Guid TransportVehicleId, TripStatus TripStatus, string TrackingDeviceReference);
public sealed record BoardingDropScanRequest(string ClientScanId, Guid TransportTripId, Guid RouteStopSequenceId, string CredentialReference, TransportScanMethod ScanMethod, ScanDirection ScanDirection, string ActorReference, string TrackingDeviceReference, DateTimeOffset LocalScanTime, string CredentialSnapshotVersion);
public sealed record OfflineTransportScanSyncRequest(string ClientBatchId, Guid TransportTripId, string TrackingDeviceReference, string ActorReference, DateTimeOffset SubmittedAt, IReadOnlyList<BoardingDropScanRequest> Scans);
public sealed record BoardingDropScanResponse(Guid BoardingDropScanEventId, string SchoolAccountId, Guid TransportTripId, Guid TransportRouteId, Guid RouteStopSequenceId, string StudentProfileId, Guid? StudentTransportAssignmentId, string CredentialReference, ScanDirection ScanDirection, DateTimeOffset LocalScanTime, DateTimeOffset ReceivedAt, bool OfflineCaptured, TransportSyncStatus SyncStatus, TransportScanDecision ScanDecision, string DecisionReason, TransportStatusAfter TransportStatusAfter, TransportReviewStatus ReviewStatus);
public sealed record OfflineTransportScanSyncResponse(string ClientBatchId, TransportSyncStatus BatchStatus, int AcceptedCount, int NeedsReviewCount, int DuplicateCount, int RejectedCount, IReadOnlyList<BoardingDropScanResponse> Items);
public sealed record ScanReviewOutcomeRequest(ManualReviewAction ReviewAction, string ReviewReason, string ClientRequestId);
public sealed record BoardingDropScanTraceResponse(Guid ScanEventId, IReadOnlyList<TransportTraceReference> References);

public static class BoardingDropScanMapping
{
    public static BoardingDropScanResponse ToResponse(this BoardingDropScanEvent scan) => new(scan.Id, scan.TenantId, scan.TransportTripId, scan.TransportRouteId, scan.RouteStopSequenceId, scan.StudentProfileId, scan.StudentTransportAssignmentId, scan.CredentialReference, scan.ScanDirection, scan.LocalScanTime, scan.ReceivedAt, scan.OfflineCaptured, scan.SyncStatus, scan.ScanDecision, scan.DecisionReason, scan.TransportStatusAfter, scan.ReviewStatus);
}
