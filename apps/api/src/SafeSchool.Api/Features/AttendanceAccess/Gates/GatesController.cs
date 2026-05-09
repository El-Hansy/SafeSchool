using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public static class GatesController
{
    public static RouteGroupBuilder MapGateEndpoints(this RouteGroupBuilder group)
    {
        var gates = group.MapGroup("/gates");
        gates.MapPost("/", async (string schoolAccountId, CreateGateRequest request, GateService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/gates/{result.Value!.GateId}", result.Value) : Results.BadRequest(result.Errors);
        });
        gates.MapGet("/", async (string schoolAccountId, GateService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        gates.MapPatch("/{gateId:guid}", async (string schoolAccountId, Guid gateId, UpdateGateRequest request, GateService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, gateId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}

