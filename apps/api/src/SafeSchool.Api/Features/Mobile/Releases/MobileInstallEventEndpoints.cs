namespace SafeSchool.Api.Features.Mobile;

public static class MobileInstallEventEndpoints
{
    public static RouteGroupBuilder MapMobileInstallEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/install-events", (InstallEventRequest request, MobileVersionPolicyService policy, MobileAuditService audit) =>
        {
            var result = policy.Evaluate(request.VersionCode);
            var auditEvent = audit.Record(request.TenantId ?? "school-demo", request.UserId ?? "anonymous", request.DeviceId, "mobile.install_event", MobileRoleCodes.Guardian, "mobile.release.install", result == InstallEventResult.Allowed ? "allowed" : "blocked", result.ToString(), "apk_release", request.ReleaseId);
            var next = result == InstallEventResult.Allowed ? "continue" : "update_required";
            return Results.Ok(new InstallEventResponse(auditEvent.Id.ToString("N"), true, next, result == InstallEventResult.Allowed ? "Version accepted." : "Update required."));
        });
        return group;
    }
}
