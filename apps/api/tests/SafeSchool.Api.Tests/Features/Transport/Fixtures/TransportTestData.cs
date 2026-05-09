using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Routes;
using SafeSchool.Api.Features.Transport.Assignments;
using SafeSchool.Api.Features.Transport.Trips;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public static class TransportTestData
{
    public static TransportRoute ActiveRoute(string tenantId = "school-1") => new() { TenantId = tenantId, RouteName = "North Morning", RouteCode = "NORTH-AM", RouteStatus = RouteStatus.Active, ServiceDirection = ServiceDirection.Pickup };
    public static TransportStop ActiveStop(string tenantId = "school-1", string code = "STOP-1") => new() { TenantId = tenantId, StopName = code, StopCode = code, StopStatus = StopStatus.Active, PickupAllowed = true, DropAllowed = true };
    public static TransportVehicle ActiveVehicle(string tenantId = "school-1") => new() { TenantId = tenantId, VehicleName = "Bus 12", VehicleCode = "BUS-12", VehicleStatus = VehicleStatus.Active, Capacity = 40 };
    public static StudentTransportAssignment ActiveAssignment(Guid routeId, string tenantId = "school-1") => new() { TenantId = tenantId, StudentProfileId = "student-1", TransportRouteId = routeId, AssignmentStatus = AssignmentStatus.Active, VisibilityState = GuardianVisibilityState.GuardianVisible };
    public static TransportTrip ActiveTrip(Guid routeId, Guid vehicleId, string tenantId = "school-1") => new() { TenantId = tenantId, TransportRouteId = routeId, TransportVehicleId = vehicleId, TrackingDeviceReference = "device-1", TripStatus = TripStatus.Active };
}
