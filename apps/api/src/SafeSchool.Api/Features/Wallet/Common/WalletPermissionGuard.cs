using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Wallet.Common;

public sealed class WalletPermissionGuard(
    IFeatureGateService featureGateService,
    ITenantContext tenantContext,
    IAccessDecisionWriter accessDecisionWriter)
{
    public async Task<OperationResult<string>> RequireAsync(
        string tenantId,
        string capabilityKey,
        string permissionKey,
        IEnumerable<string>? actorPermissions = null,
        string targetType = "Wallet",
        string? targetReference = null,
        string? guardianScopedStudentId = null,
        string? requestedStudentId = null,
        string? posDeviceReference = null,
        string? requestedDeviceReference = null,
        CancellationToken cancellationToken = default)
    {
        actorPermissions ??= WalletPermissionCatalog.WalletAdministrator;
        string? denial = null;
        var code = "allowed";

        if (tenantContext.TenantId != tenantId && !tenantContext.HasPlatformReviewScope)
        {
            denial = "Actor is not scoped to this school account.";
            code = "tenant_mismatch";
        }
        else if (!featureGateService.IsEnabled(tenantId, capabilityKey))
        {
            denial = featureGateService.Explain(tenantId, capabilityKey);
            code = "feature_disabled";
        }
        else if (!actorPermissions.Contains(permissionKey) && !actorPermissions.Contains("*"))
        {
            denial = $"Actor lacks {permissionKey}.";
            code = "missing_permission";
        }
        else if (!string.IsNullOrWhiteSpace(guardianScopedStudentId) && requestedStudentId != guardianScopedStudentId)
        {
            denial = "Guardian scope does not include the requested student.";
            code = "guardian_scope_mismatch";
        }
        else if (!string.IsNullOrWhiteSpace(posDeviceReference) && requestedDeviceReference != posDeviceReference)
        {
            denial = "POS device is not authorized for this terminal.";
            code = "pos_device_mismatch";
        }

        if (denial is null)
        {
            return OperationResult<string>.Success("allowed");
        }

        await accessDecisionWriter.RecordAsync(new AccessDecision
        {
            TenantId = tenantId,
            ActorReference = tenantContext.ActorReference ?? "unknown",
            AttemptedAction = permissionKey,
            TargetType = targetType,
            TargetReference = targetReference ?? string.Empty,
            Decision = AccessDecisionResult.Denied,
            DecisionReason = denial,
            FeatureCapabilityKey = capabilityKey,
            DecidedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        return OperationResult<string>.Failure(new ValidationError(code, denial, permissionKey));
    }
}
