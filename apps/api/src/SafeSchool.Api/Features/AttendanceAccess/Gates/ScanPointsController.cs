using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public static class ScanPointsController
{
    public static RouteGroupBuilder MapScanPointEndpoints(this RouteGroupBuilder group)
    {
        var scanPoints = group.MapGroup("/scan-points");
        scanPoints.MapPost("/", async (string schoolAccountId, CreateScanPointRequest request, ScanPointService service, CancellationToken ct) =>
        {
            var result = await service.RegisterAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/scan-points/{result.Value!.ScanPointId}", result.Value) : Results.BadRequest(result.Errors);
        });
        scanPoints.MapPatch("/{scanPointId:guid}", async (string schoolAccountId, Guid scanPointId, UpdateScanPointRequest request, ScanPointService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, scanPointId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}

