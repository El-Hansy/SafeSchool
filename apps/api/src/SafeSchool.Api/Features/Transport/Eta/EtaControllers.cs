namespace SafeSchool.Api.Features.Transport.Eta;

using SafeSchool.Api.Infrastructure.Tenancy;

public static class EtaControllers
{
    public static RouteGroupBuilder MapEtaEndpoints(this RouteGroupBuilder group, RouteGroupBuilder guardianGroup)
    {
        group.MapPost("/trips/{tripId:guid}/eta/recalculate", async (string schoolAccountId, Guid tripId, EtaRecalculateRequest request, EtaCalculationService service, CancellationToken ct) => Results.Ok(await service.RecalculateAsync(schoolAccountId, tripId, request, ct)));
        group.MapGet("/eta-records/{etaRecordId:guid}/trace", async (string schoolAccountId, Guid etaRecordId, EtaTraceService service, CancellationToken ct) =>
        {
            var result = await service.TraceAsync(schoolAccountId, etaRecordId, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
        guardianGroup.MapGet("/{studentProfileId}/transport/trips/{tripId:guid}/eta", async (string studentProfileId, Guid tripId, ITenantContext tenantContext, GuardianEtaVisibilityService service, CancellationToken ct) =>
        {
            var result = await service.ReadAsync(GuardianTenantResolver.Resolve(tenantContext), tenantContext.ActorReference ?? "anonymous", studentProfileId, tripId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
