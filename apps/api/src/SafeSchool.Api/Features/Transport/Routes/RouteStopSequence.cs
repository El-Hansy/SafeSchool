using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class RouteStopSequence : TenantOwnedEntity
{
    public Guid TransportRouteId { get; set; }
    public Guid TransportStopId { get; set; }
    public ServiceDirection ServiceDirection { get; set; } = ServiceDirection.Pickup;
    public int SequenceNumber { get; set; }
    public TimeSpan? PlannedArrivalOffset { get; set; }
    public TimeSpan? PlannedDepartureOffset { get; set; }
    public string RouteVersion { get; set; } = "v1";
    public SequenceStatus SequenceStatus { get; set; } = SequenceStatus.Active;

    public bool HasValidOffsets() => PlannedDepartureOffset is null || PlannedArrivalOffset is null || PlannedDepartureOffset >= PlannedArrivalOffset;
}
