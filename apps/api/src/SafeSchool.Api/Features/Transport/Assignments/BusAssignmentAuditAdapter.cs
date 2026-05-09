using SafeSchool.Api.Features.Transport.Audit;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class BusAssignmentAuditAdapter(ITransportAuditWriter auditWriter)
{
    public Task VehicleChangedAsync(TransportVehicle vehicle, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = vehicle.TenantId, EventCategory = "Assignment", EventType = eventType, SubjectType = "TransportVehicle", SubjectReference = vehicle.Id.ToString(), Reason = reason }, cancellationToken);

    public Task AssignmentChangedAsync(StudentTransportAssignment assignment, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new TransportAuditEvent { TenantId = assignment.TenantId, EventCategory = "Assignment", EventType = eventType, SubjectType = "StudentTransportAssignment", SubjectReference = assignment.Id.ToString(), Reason = reason }, cancellationToken);
}
