using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class TransportVehicle : TenantOwnedEntity
{
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleCode { get; set; } = string.Empty;
    public string PlateReference { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public VehicleStatus VehicleStatus { get; set; } = VehicleStatus.Draft;
    public string DefaultSupervisorReference { get; set; } = string.Empty;
    public string DefaultDriverReference { get; set; } = string.Empty;

    public bool CanServeTrip() => VehicleStatus == VehicleStatus.Active;
    public bool HasValidCapacity() => Capacity >= 0;
}
