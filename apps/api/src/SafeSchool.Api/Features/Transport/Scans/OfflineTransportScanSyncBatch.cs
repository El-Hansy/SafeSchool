using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class OfflineTransportScanSyncBatch : TenantOwnedEntity
{
    public string ClientBatchId { get; set; } = string.Empty;
    public Guid TransportTripId { get; set; }
    public string TrackingDeviceReference { get; set; } = string.Empty;
    public TransportSyncStatus BatchStatus { get; set; } = TransportSyncStatus.Pending;
    public int AcceptedCount { get; set; }
    public int NeedsReviewCount { get; set; }
    public int DuplicateCount { get; set; }
    public int RejectedCount { get; set; }
}
