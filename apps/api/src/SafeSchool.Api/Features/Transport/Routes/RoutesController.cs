namespace SafeSchool.Api.Features.Transport.Routes;

public static class RoutesController
{
    public static RouteGroupBuilder MapRouteStopEndpoints(this RouteGroupBuilder group)
    {
        var routes = group.MapGroup("/routes");
        routes.MapGet("/", async (string schoolAccountId, TransportRouteService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        routes.MapPost("/", async (string schoolAccountId, CreateRouteRequest request, TransportRouteService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/transport/routes/{result.Value!.TransportRouteId}", result.Value) : Results.BadRequest(result.Errors);
        });
        routes.MapPatch("/{routeId:guid}", async (string schoolAccountId, Guid routeId, UpdateRouteRequest request, TransportRouteService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, routeId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        routes.MapPost("/{routeId:guid}/stop-sequences", async (string schoolAccountId, Guid routeId, ReplaceRouteStopSequenceRequest request, RouteStopSequenceService service, CancellationToken ct) =>
        {
            var result = await service.ReplaceAsync(schoolAccountId, routeId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        routes.MapGet("/{routeId:guid}/trace", async (string schoolAccountId, Guid routeId, RouteTraceService service, CancellationToken ct) =>
        {
            var result = await service.TraceAsync(schoolAccountId, routeId, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
        group.MapStopEndpoints();
        return group;
    }
}
