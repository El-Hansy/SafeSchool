using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileInstallEventEndpoints
{
    public static RouteGroupBuilder MapMobileInstallEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/install-events", (InstallEventRequest request, ITenantContext tenantContext, MobileVersionPolicyService policy, MobileAuditService audit) =>
        {
            if (MobileEndpointContext.HasTenantConflict(request.TenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            var result = policy.Evaluate(request.VersionCode);
            var auditEvent = audit.Record(MobileEndpointContext.Tenant(request.TenantId, tenantContext), MobileEndpointContext.User(request.UserId, tenantContext), request.DeviceId, "mobile.install_event", MobileRoleCodes.Guardian, "mobile.release.install", result == InstallEventResult.Allowed ? "allowed" : "blocked", result.ToString(), "apk_release", request.ReleaseId);
            var next = result == InstallEventResult.Allowed ? "continue" : "update_required";
            return Results.Ok(new InstallEventResponse(auditEvent.Id.ToString("N"), true, next, result == InstallEventResult.Allowed ? "Version accepted." : "Update required."));
        });
        return group;
    }
}
