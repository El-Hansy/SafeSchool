using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class BoardingDropScanEvent : TenantOwnedEntity
{
    public string ClientScanId { get; set; } = string.Empty;
    public Guid TransportTripId { get; set; }
    public Guid TransportRouteId { get; set; }
    public Guid RouteStopSequenceId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid? StudentTransportAssignmentId { get; set; }
    public string CredentialReference { get; set; } = string.Empty;
    public ScanDirection ScanDirection { get; set; } = ScanDirection.Boarding;
    public TransportScanMethod ScanMethod { get; set; } = TransportScanMethod.Nfc;
    public DateTimeOffset LocalScanTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool OfflineCaptured { get; set; }
    public TransportSyncStatus SyncStatus { get; set; } = TransportSyncStatus.Reconciled;
    public TransportScanDecision ScanDecision { get; set; } = TransportScanDecision.Accepted;
    public string DecisionReason { get; set; } = string.Empty;
    public TransportStatusAfter TransportStatusAfter { get; set; } = TransportStatusAfter.Unchanged;
    public TransportReviewStatus ReviewStatus { get; set; } = TransportReviewStatus.NotRequired;

    public bool CreatesNormalStatus() => ScanDecision == TransportScanDecision.Accepted && ReviewStatus != TransportReviewStatus.NeedsReview;
}
