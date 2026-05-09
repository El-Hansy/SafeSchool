using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed record TripCreateRequest(Guid TransportRouteId, Guid TransportVehicleId, string TrackingDeviceReference, ServiceDirection ServiceDirection, DateOnly TripDate, DateTimeOffset PlannedStartTime, string ClientRequestId);
public sealed record TripPatchRequest(TripStatus? TripStatus, string? DriverReference, string? AttendantReference, string Reason);
public sealed record LocationUpdateRequest(string ClientLocationId, string TrackingDeviceReference, string ActorReference, DateTimeOffset ReportedAt, string LocationReference, LocationProgressState ProgressState, Guid? NearestRouteStopSequenceId);
public sealed record LocationUpdateResponse(Guid TransportLocationUpdateId, Guid TransportTripId, LocationProgressState ProgressState, LocationFreshnessStatus FreshnessStatus, LocationAcceptanceStatus AcceptanceStatus, DateTimeOffset ReportedAt, DateTimeOffset ReceivedAt, string SuppressionReason);
public sealed record TripProgressResponse(Guid TransportTripId, string SchoolAccountId, Guid TransportRouteId, Guid TransportVehicleId, TripStatus TripStatus, ServiceDirection ServiceDirection, LocationUpdateResponse? LatestLocation, string GuardianVisibilityState);
public sealed record GuardianTripProgressResponse(string StudentProfileId, Guid TransportTripId, string VisibilityPhase, string? PickupEta, string? ExactLiveLocation, string? DropStatus);
public sealed record TripTraceResponse(Guid TransportTripId, IReadOnlyList<TransportTraceReference> References);

public static class LiveTrackingMapping
{
    public static LocationUpdateResponse ToResponse(this TransportLocationUpdate update) => new(update.Id, update.TransportTripId, update.ProgressState, update.FreshnessStatus, update.AcceptanceStatus, update.ReportedAt, update.ReceivedAt, update.SuppressionReason);
}
