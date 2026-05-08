using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed record EtaRecalculateRequest(IReadOnlyList<Guid> IncludeRouteStopSequenceIds, string CalculationReason, Guid? SourceLocationUpdateId, string ClientRequestId);
public sealed record EtaRecordResponse(Guid EtaRecordId, string SchoolAccountId, Guid TransportTripId, Guid TransportRouteId, Guid RouteStopSequenceId, string StudentProfileId, DateTimeOffset? EstimatedArrivalTime, EtaState EtaState, EtaConfidenceState ConfidenceState, LocationFreshnessStatus FreshnessStatus, Guid? SourceLocationUpdateId, DateTimeOffset CalculatedAt, TransportReviewStatus ReviewStatus);
public sealed record GuardianEtaResponse(string StudentProfileId, Guid TransportTripId, Guid RouteStopSequenceId, string VisibilityPhase, EtaRecordResponse? PickupEta, bool ExactLiveLocationAvailable);
public sealed record EtaTraceResponse(Guid EtaRecordId, IReadOnlyList<TransportTraceReference> References);

public static class EtaMapping
{
    public static EtaRecordResponse ToResponse(this EtaRecord eta) => new(eta.Id, eta.TenantId, eta.TransportTripId, eta.TransportRouteId, eta.RouteStopSequenceId, eta.StudentProfileId, eta.EstimatedArrivalTime, eta.EtaState, eta.ConfidenceState, eta.FreshnessStatus, eta.SourceLocationUpdateId, eta.CalculatedAt, eta.ReviewStatus);
}
