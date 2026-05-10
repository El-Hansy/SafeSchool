using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class StudentTransportAssignmentService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard, AssignmentEligibilityService eligibilityService)
{
    public async Task<OperationResult<AssignmentResponse>> CreateAsync(string tenantId, CreateAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, TransportCapabilities.BusAssignment, TransportPermissionCatalog.AssignmentsManage, targetType: "StudentTransportAssignment", targetReference: request.StudentProfileId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<AssignmentResponse>.Failure(allowed.Errors.ToArray());
        var assignment = new StudentTransportAssignment { TenantId = tenantId, StudentProfileId = request.StudentProfileId, TransportRouteId = request.TransportRouteId, TransportVehicleId = request.TransportVehicleId, PickupRouteStopSequenceId = request.PickupRouteStopSequenceId, DropRouteStopSequenceId = request.DropRouteStopSequenceId, ServiceDirection = request.ServiceDirection, ValidFrom = request.ValidFrom, ValidTo = request.ValidTo, VisibilityState = request.VisibilityState, AssignmentStatus = request.AssignmentStatus };
        if (assignment.AssignmentStatus == AssignmentStatus.Active)
        {
            var eligible = await eligibilityService.ValidateAsync(tenantId, assignment, cancellationToken);
            if (!eligible.Succeeded) return OperationResult<AssignmentResponse>.Failure(eligible.Errors.ToArray());
        }
        dbContext.StudentTransportAssignments.Add(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<AssignmentResponse>.Success(assignment.ToResponse());
    }

    public async Task<IReadOnlyList<AssignmentResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.StudentTransportAssignments.Where(x => x.TenantId == tenantId).OrderBy(x => x.StudentProfileId).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<StudentTransportPlanResponse> PlanAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) =>
        new(studentProfileId, await dbContext.StudentTransportAssignments.Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId).Select(x => x.ToResponse()).ToListAsync(cancellationToken));
}
