using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class TransportLocationUpdate : TenantOwnedEntity
{
    public string ClientLocationId { get; set; } = string.Empty;
    public Guid TransportTripId { get; set; }
    public string TrackingDeviceReference { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public DateTimeOffset ReportedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public string LocationReference { get; set; } = string.Empty;
    public LocationProgressState ProgressState { get; set; } = LocationProgressState.EnRoute;
    public Guid? NearestRouteStopSequenceId { get; set; }
    public LocationFreshnessStatus FreshnessStatus { get; set; } = LocationFreshnessStatus.Current;
    public LocationAcceptanceStatus AcceptanceStatus { get; set; } = LocationAcceptanceStatus.Accepted;
    public RetentionState RetentionState { get; set; } = RetentionState.Detailed;
    public string SuppressionReason { get; set; } = string.Empty;

    public bool IsCurrent(DateTimeOffset now, TimeSpan threshold) => now - ReportedAt <= threshold && FreshnessStatus == LocationFreshnessStatus.Current;
    public void ConvertToSummary() => RetentionState = RetentionState.SummaryOnly;
}
