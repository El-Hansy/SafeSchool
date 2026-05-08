using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed record CreateRouteRequest(string RouteName, string RouteCode, ServiceDirection ServiceDirection, string CampusReference, TimeOnly? PlannedStartTime, TimeOnly? PlannedEndTime, RouteStatus RouteStatus, string ClientRequestId);
public sealed record UpdateRouteRequest(string? RouteName, RouteStatus? RouteStatus, string Reason);
public sealed record RouteResponse(Guid TransportRouteId, string SchoolAccountId, string RouteName, string RouteCode, ServiceDirection ServiceDirection, RouteStatus RouteStatus, string RouteVersion, DateTimeOffset UpdatedAt);
public sealed record CreateStopRequest(string StopName, string StopCode, string StopReference, bool PickupAllowed, bool DropAllowed, StopStatus StopStatus, string ClientRequestId);
public sealed record UpdateStopRequest(string? StopName, StopStatus? StopStatus, bool? PickupAllowed, bool? DropAllowed, string Reason);
public sealed record StopResponse(Guid TransportStopId, string SchoolAccountId, string StopName, string StopCode, bool PickupAllowed, bool DropAllowed, StopStatus StopStatus);
public sealed record RouteStopSequenceItem(Guid TransportStopId, int SequenceNumber, TimeSpan? PlannedArrivalOffset, TimeSpan? PlannedDepartureOffset);
public sealed record ReplaceRouteStopSequenceRequest(ServiceDirection ServiceDirection, string RouteVersionReason, IReadOnlyList<RouteStopSequenceItem> Stops, string ClientRequestId);
public sealed record RouteStopSequenceResponse(Guid RouteStopSequenceId, Guid TransportRouteId, Guid TransportStopId, ServiceDirection ServiceDirection, int SequenceNumber, string RouteVersion, SequenceStatus SequenceStatus);
public sealed record RouteTraceResponse(Guid TransportRouteId, IReadOnlyList<TransportTraceReference> References);

public static class RouteStopMapping
{
    public static RouteResponse ToResponse(this TransportRoute route) => new(route.Id, route.TenantId, route.RouteName, route.RouteCode, route.ServiceDirection, route.RouteStatus, route.RouteVersion, route.UpdatedAt);
    public static StopResponse ToResponse(this TransportStop stop) => new(stop.Id, stop.TenantId, stop.StopName, stop.StopCode, stop.PickupAllowed, stop.DropAllowed, stop.StopStatus);
    public static RouteStopSequenceResponse ToResponse(this RouteStopSequence sequence) => new(sequence.Id, sequence.TransportRouteId, sequence.TransportStopId, sequence.ServiceDirection, sequence.SequenceNumber, sequence.RouteVersion, sequence.SequenceStatus);
}
