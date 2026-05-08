using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;

public sealed class PermissionGuard(IFeatureGateService featureGateService, ITenantContext tenantContext)
{
    public OperationResult<string> Require(string tenantId, string capabilityKey, string permissionKey, IEnumerable<string> actorPermissions)
    {
        if (tenantContext.TenantId != tenantId && !tenantContext.HasPlatformReviewScope)
        {
            return OperationResult<string>.Failure(new ValidationError("tenant_mismatch", "Actor is not scoped to this school account."));
        }

        if (!featureGateService.IsEnabled(tenantId, capabilityKey))
        {
            return OperationResult<string>.Failure(new ValidationError("feature_disabled", featureGateService.Explain(tenantId, capabilityKey), capabilityKey));
        }

        if (!actorPermissions.Contains(permissionKey) && !actorPermissions.Contains("*"))
        {
            return OperationResult<string>.Failure(new ValidationError("missing_permission", $"Actor lacks {permissionKey}.", permissionKey));
        }

        return OperationResult<string>.Success("allowed");
    }
}
