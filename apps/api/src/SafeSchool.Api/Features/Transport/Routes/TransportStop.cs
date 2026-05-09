using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class TransportStop : TenantOwnedEntity
{
    public string StopName { get; set; } = string.Empty;
    public string StopCode { get; set; } = string.Empty;
    public string StopReference { get; set; } = string.Empty;
    public bool PickupAllowed { get; set; } = true;
    public bool DropAllowed { get; set; } = true;
    public StopStatus StopStatus { get; set; } = StopStatus.Draft;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    public bool Allows(ServiceDirection direction) => direction switch
    {
        ServiceDirection.Pickup => PickupAllowed,
        ServiceDirection.Dropoff => DropAllowed,
        _ => PickupAllowed && DropAllowed
    };
}
