using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;

public sealed class PermissionEvaluator(IFeatureGateService featureGateService, ITenantContext tenantContext)
{
    public OperationResult<AccessDecisionResult> Evaluate(
        string tenantId,
        string capabilityKey,
        string requiredPermission,
        IEnumerable<ActorRoleAssignment> assignments,
        IEnumerable<string> grantedPermissions,
        string targetTenantId)
    {
        if (tenantId != targetTenantId)
        {
            return OperationResult<AccessDecisionResult>.Failure(new ValidationError("target_tenant_mismatch", "Target does not belong to this school account."));
        }

        if (tenantContext.TenantId != tenantId && !tenantContext.HasPlatformReviewScope)
        {
            return OperationResult<AccessDecisionResult>.Failure(new ValidationError("tenant_mismatch", "Actor is not scoped to this school account."));
        }

        if (!featureGateService.IsEnabled(tenantId, capabilityKey))
        {
            return OperationResult<AccessDecisionResult>.Failure(new ValidationError("feature_disabled", featureGateService.Explain(tenantId, capabilityKey), capabilityKey));
        }

        var hasActiveAssignment = assignments.Any(x => x.TenantId == tenantId
            && x.AssignmentStatus == AssignmentStatus.Active
            && x.ValidFrom <= DateTimeOffset.UtcNow
            && (x.ValidUntil is null || x.ValidUntil > DateTimeOffset.UtcNow));

        if (!hasActiveAssignment)
        {
            return OperationResult<AccessDecisionResult>.Failure(new ValidationError("inactive_assignment", "Actor has no active role assignment."));
        }

        if (!grantedPermissions.Contains(requiredPermission) && !grantedPermissions.Contains("*"))
        {
            return OperationResult<AccessDecisionResult>.Failure(new ValidationError("missing_permission", $"Missing {requiredPermission}.", requiredPermission));
        }

        return OperationResult<AccessDecisionResult>.Success(AccessDecisionResult.Allowed);
    }
}
