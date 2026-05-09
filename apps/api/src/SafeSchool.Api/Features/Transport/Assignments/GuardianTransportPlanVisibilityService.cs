using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class GuardianTransportPlanVisibilityService(SafeSchoolDbContext dbContext, ITransportGuardianLinkProvider guardianLinkProvider)
{
    public async Task<OperationResult<StudentTransportPlanResponse>> VisiblePlanAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinkProvider.GetLinkAsync(tenantId, guardianReference, studentProfileId, cancellationToken);
        if (link.Status != GuardianLinkStatus.Approved || !link.HasTransportVisibilityScope)
        {
            return OperationResult<StudentTransportPlanResponse>.Failure(new ValidationError("guardian_scope_denied", "Guardian link does not allow transport plan visibility."));
        }

        var assignments = dbContext.StudentTransportAssignments
            .Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId && x.VisibilityState == GuardianVisibilityState.GuardianVisible)
            .Select(x => x.ToResponse())
            .ToList();
        return OperationResult<StudentTransportPlanResponse>.Success(new StudentTransportPlanResponse(studentProfileId, assignments));
    }
}
