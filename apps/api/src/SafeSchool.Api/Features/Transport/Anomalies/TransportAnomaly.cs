using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Anomalies;

public sealed class TransportAnomaly : TenantOwnedEntity
{
    public Guid? TransportTripId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid? TransportRouteId { get; set; }
    public Guid? RouteStopSequenceId { get; set; }
    public string SourceEventReference { get; set; } = string.Empty;
    public TransportAnomalyType AnomalyType { get; set; } = TransportAnomalyType.ManualReviewRequired;
    public AnomalySeverity Severity { get; set; } = AnomalySeverity.Medium;
    public AnomalyStatus Status { get; set; } = AnomalyStatus.Open;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTimeOffset DetectedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ResolutionReason { get; set; } = string.Empty;
    public string ResolvedBy { get; set; } = string.Empty;
    public DateTimeOffset? ResolvedAt { get; set; }

    public bool IsOpen() => Status is AnomalyStatus.Open or AnomalyStatus.Assigned or AnomalyStatus.Reopened;
}
