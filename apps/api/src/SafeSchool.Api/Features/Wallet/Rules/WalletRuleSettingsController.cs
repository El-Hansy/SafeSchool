using SafeSchool.Api.Features.Wallet.Wallets;

namespace SafeSchool.Api.Features.Wallet.Rules;

public static class WalletRuleSettingsController
{
    public static RouteGroupBuilder MapWalletRuleSettingEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/rule-settings/current", async (string schoolAccountId, WalletRuleSettingService service, CancellationToken ct) => Results.Ok(await service.CurrentAsync(schoolAccountId, ct)));
        group.MapPost("/rule-settings", async (string schoolAccountId, WalletRuleSettingRequest request, WalletRuleSettingService service, CancellationToken ct) =>
        {
            var result = await service.CreateDraftAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/rule-settings/{result.Value!.RuleSettingId}", result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPatch("/rule-settings/{ruleSettingId:guid}", async (string schoolAccountId, Guid ruleSettingId, WalletRuleSettingRequest request, WalletRuleSettingService service, CancellationToken ct) =>
        {
            var created = await service.CreateDraftAsync(schoolAccountId, request, ct);
            return created.Succeeded ? Results.Ok(created.Value) : Results.BadRequest(created.Errors);
        });
        group.MapPost("/rule-settings/{ruleSettingId:guid}/activate", async (string schoolAccountId, Guid ruleSettingId, WalletRestoreRequest request, WalletRuleSettingService service, CancellationToken ct) =>
        {
            var result = await service.ActivateAsync(schoolAccountId, ruleSettingId, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPost("/rule-settings/{ruleSettingId:guid}/suspend", async (string schoolAccountId, Guid ruleSettingId, WalletRestrictionRequest request, WalletRuleSettingService service, CancellationToken ct) =>
        {
            var result = await service.SuspendAsync(schoolAccountId, ruleSettingId, request.Reason, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
