using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed class EtaRecord : TenantOwnedEntity
{
    public Guid TransportTripId { get; set; }
    public Guid TransportRouteId { get; set; }
    public Guid RouteStopSequenceId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public DateTimeOffset? EstimatedArrivalTime { get; set; }
    public EtaState EtaState { get; set; } = EtaState.Unavailable;
    public EtaConfidenceState ConfidenceState { get; set; } = EtaConfidenceState.Unavailable;
    public LocationFreshnessStatus FreshnessStatus { get; set; } = LocationFreshnessStatus.Unavailable;
    public Guid? SourceLocationUpdateId { get; set; }
    public bool MaterialChange { get; set; }
    public TransportReviewStatus ReviewStatus { get; set; } = TransportReviewStatus.NotRequired;
    public DateTimeOffset CalculatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsVisibleToGuardian() => EtaState is EtaState.Available or EtaState.Stale;
    public void MarkUnavailable(string reason) { EtaState = EtaState.Unavailable; ConfidenceState = EtaConfidenceState.Unavailable; }
}
