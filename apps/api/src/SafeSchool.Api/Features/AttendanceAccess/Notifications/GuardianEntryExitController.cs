namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public static class GuardianEntryExitController
{
    public static RouteGroupBuilder MapGuardianEntryExitEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/guardian-entry-exit/me", async (string schoolAccountId, string guardianReference, GuardianEntryExitVisibilityService service, CancellationToken ct) =>
            Results.Ok(await service.VisibleForGuardianAsync(schoolAccountId, guardianReference, ct)));
        return group;
    }
}

