namespace SafeSchool.Api.Features.Transport.Routes;

public static class StopsController
{
    public static RouteGroupBuilder MapStopEndpoints(this RouteGroupBuilder group)
    {
        var stops = group.MapGroup("/stops");
        stops.MapGet("/", async (string schoolAccountId, TransportStopService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        stops.MapPost("/", async (string schoolAccountId, CreateStopRequest request, TransportStopService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/transport/stops/{result.Value!.TransportStopId}", result.Value) : Results.BadRequest(result.Errors);
        });
        stops.MapPatch("/{stopId:guid}", async (string schoolAccountId, Guid stopId, UpdateStopRequest request, TransportStopService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, stopId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
