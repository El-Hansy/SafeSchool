using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed record CreateVehicleRequest(string VehicleName, string VehicleCode, string PlateReference, int Capacity, VehicleStatus VehicleStatus, string DefaultDriverReference, string DefaultSupervisorReference, string ClientRequestId);
public sealed record UpdateVehicleRequest(string? VehicleName, VehicleStatus? VehicleStatus, int? Capacity, string Reason);
public sealed record VehicleResponse(Guid TransportVehicleId, string SchoolAccountId, string VehicleName, string VehicleCode, int Capacity, VehicleStatus VehicleStatus);
public sealed record CreateAssignmentRequest(string StudentProfileId, Guid TransportRouteId, Guid? TransportVehicleId, Guid? PickupRouteStopSequenceId, Guid? DropRouteStopSequenceId, ServiceDirection ServiceDirection, DateOnly ValidFrom, DateOnly? ValidTo, GuardianVisibilityState VisibilityState, AssignmentStatus AssignmentStatus, string ClientRequestId);
public sealed record UpdateAssignmentRequest(AssignmentStatus? AssignmentStatus, GuardianVisibilityState? VisibilityState, DateOnly? ValidTo, string Reason);
public sealed record AssignmentResponse(Guid StudentTransportAssignmentId, string SchoolAccountId, string StudentProfileId, Guid TransportRouteId, Guid? TransportVehicleId, ServiceDirection ServiceDirection, DateOnly ValidFrom, DateOnly? ValidTo, GuardianVisibilityState VisibilityState, AssignmentStatus AssignmentStatus, TransportReviewStatus ReviewStatus, DateTimeOffset UpdatedAt);
public sealed record StudentTransportPlanResponse(string StudentProfileId, IReadOnlyList<AssignmentResponse> Assignments);
public sealed record AssignmentTraceResponse(Guid AssignmentId, IReadOnlyList<TransportTraceReference> References);

public static class BusAssignmentMapping
{
    public static VehicleResponse ToResponse(this TransportVehicle vehicle) => new(vehicle.Id, vehicle.TenantId, vehicle.VehicleName, vehicle.VehicleCode, vehicle.Capacity, vehicle.VehicleStatus);
    public static AssignmentResponse ToResponse(this StudentTransportAssignment assignment) => new(assignment.Id, assignment.TenantId, assignment.StudentProfileId, assignment.TransportRouteId, assignment.TransportVehicleId, assignment.ServiceDirection, assignment.ValidFrom, assignment.ValidTo, assignment.VisibilityState, assignment.AssignmentStatus, assignment.ReviewStatus, assignment.UpdatedAt);
}
