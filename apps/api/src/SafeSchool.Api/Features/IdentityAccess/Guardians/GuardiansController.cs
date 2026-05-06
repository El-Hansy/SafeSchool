namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public static class GuardiansController
{
    public static RouteGroupBuilder MapGuardianEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/guardians", async (string schoolAccountId, GuardianRequest request, GuardianService service, CancellationToken ct) =>
            Results.Created("/guardians", await service.CreateAsync(schoolAccountId, request, ct)));
        group.MapGet("/guardians", async (string schoolAccountId, GuardianService service, CancellationToken ct) =>
            Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        group.MapGet("/guardians/{guardianId:guid}", async (string schoolAccountId, Guid guardianId, GuardianService service, CancellationToken ct) =>
        {
            var result = await service.ReadAsync(schoolAccountId, guardianId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });
        group.MapPatch("/guardians/{guardianId:guid}", async (string schoolAccountId, Guid guardianId, GuardianUpdateRequest request, GuardianService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, guardianId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapGet("/guardians/{guardianId:guid}/students", async (string schoolAccountId, Guid guardianId, GuardianVisibilityService service, CancellationToken ct) =>
            Results.Ok(await service.VisibleStudentIdsAsync(schoolAccountId, guardianId, ct)));
        group.MapGuardianLinkEndpoints();
        return group;
    }
}
