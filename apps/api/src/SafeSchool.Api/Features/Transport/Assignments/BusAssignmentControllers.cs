namespace SafeSchool.Api.Features.Transport.Assignments;

using SafeSchool.Api.Infrastructure.Tenancy;

public static class BusAssignmentControllers
{
    public static RouteGroupBuilder MapVehicleAssignmentEndpoints(this RouteGroupBuilder group, RouteGroupBuilder guardianGroup)
    {
        var vehicles = group.MapGroup("/vehicles");
        vehicles.MapGet("/", async (string schoolAccountId, TransportVehicleService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        vehicles.MapPost("/", async (string schoolAccountId, CreateVehicleRequest request, TransportVehicleService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/transport/vehicles/{result.Value!.TransportVehicleId}", result.Value) : Results.BadRequest(result.Errors);
        });
        var assignments = group.MapGroup("/assignments");
        assignments.MapGet("/", async (string schoolAccountId, StudentTransportAssignmentService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        assignments.MapPost("/", async (string schoolAccountId, CreateAssignmentRequest request, StudentTransportAssignmentService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/transport/assignments/{result.Value!.StudentTransportAssignmentId}", result.Value) : Results.BadRequest(result.Errors);
        });
        assignments.MapGet("/{assignmentId:guid}/trace", async (string schoolAccountId, Guid assignmentId, AssignmentTraceService service, CancellationToken ct) =>
        {
            var result = await service.TraceAsync(schoolAccountId, assignmentId, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
        group.MapGet("/students/{studentProfileId}/plan", async (string schoolAccountId, string studentProfileId, StudentTransportAssignmentService service, CancellationToken ct) => Results.Ok(await service.PlanAsync(schoolAccountId, studentProfileId, ct)));
        guardianGroup.MapGet("/{studentProfileId}/transport/plan", async (string studentProfileId, ITenantContext tenantContext, GuardianTransportPlanVisibilityService service, CancellationToken ct) =>
        {
            var result = await service.VisiblePlanAsync(GuardianTenantResolver.Resolve(tenantContext), tenantContext.ActorReference ?? "anonymous", studentProfileId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
