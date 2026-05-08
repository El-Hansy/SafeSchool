using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Features.Transport.Routes;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class AssignmentEligibilityService(SafeSchoolDbContext dbContext, IStudentProfileEvidenceProvider studentProvider)
{
    public async Task<OperationResult<string>> ValidateAsync(string tenantId, StudentTransportAssignment assignment, CancellationToken cancellationToken = default)
    {
        if (!assignment.HasValidDates()) return OperationResult<string>.Failure(new ValidationError("invalid_date_range", "Assignment valid_to must be after valid_from."));
        var student = await studentProvider.GetEvidenceAsync(tenantId, assignment.StudentProfileId, cancellationToken);
        if (student.Status != StudentEligibilityStatus.Active) return OperationResult<string>.Failure(new ValidationError("student_not_active", $"Student status is {student.Status}."));
        var route = await dbContext.TransportRoutes.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == assignment.TransportRouteId, cancellationToken);
        if (route is null || route.RouteStatus != RouteStatus.Active) return OperationResult<string>.Failure(new ValidationError("route_not_active", "Assignment requires an active route."));
        if (assignment.TransportVehicleId is { } vehicleId)
        {
            var vehicle = await dbContext.TransportVehicles.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == vehicleId, cancellationToken);
            if (vehicle is null || !vehicle.CanServeTrip()) return OperationResult<string>.Failure(new ValidationError("vehicle_unavailable", "Assignment vehicle is not active."));
        }
        return OperationResult<string>.Success("eligible");
    }
}
