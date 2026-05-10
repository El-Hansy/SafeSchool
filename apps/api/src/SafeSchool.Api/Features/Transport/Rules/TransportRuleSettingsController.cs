namespace SafeSchool.Api.Features.Transport.Rules;

public static class TransportRuleSettingsController
{
    public static RouteGroupBuilder MapTransportRuleSettingsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/rule-settings/current", async (string schoolAccountId, TransportRuleSettingService service, CancellationToken ct) => Results.Ok(await service.CurrentAsync(schoolAccountId, ct)));
        group.MapPost("/rule-settings", async (string schoolAccountId, TransportRuleSettingRequest request, TransportRuleSettingService service, CancellationToken ct) =>
        {
            var result = await service.CreateDraftAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPost("/rule-settings/{ruleSettingId:guid}/activate", async (string schoolAccountId, Guid ruleSettingId, TransportRuleSettingService service, CancellationToken ct) =>
        {
            var result = await service.ActivateAsync(schoolAccountId, ruleSettingId, "Activated", ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
