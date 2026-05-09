namespace SafeSchool.Api.Features.Wallet.Limits;

public static class SpendingLimitsController
{
    public static RouteGroupBuilder MapSpendingLimitEndpoints(this RouteGroupBuilder schoolGroup, RouteGroupBuilder guardianGroup)
    {
        schoolGroup.MapPost("/spending-limits", async (string schoolAccountId, SpendingLimitRequest request, SpendingLimitManagementService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{result.Value!.SpendingLimitId}", result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/spending-limits", async (string schoolAccountId, SpendingLimitManagementService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, cancellationToken: ct)));
        schoolGroup.MapGet("/spending-limits/{spendingLimitId:guid}", async (string schoolAccountId, Guid spendingLimitId, SpendingLimitManagementService service, CancellationToken ct) => Results.Ok((await service.ListAsync(schoolAccountId, cancellationToken: ct)).SingleOrDefault(x => x.SpendingLimitId == spendingLimitId)));
        schoolGroup.MapPatch("/spending-limits/{spendingLimitId:guid}", (Guid spendingLimitId, SpendingLimitRequest request) => Results.Ok(new { spendingLimitId, request.LimitType, request.AmountMinor }));
        schoolGroup.MapPost("/spending-limits/{spendingLimitId:guid}/activate", async (string schoolAccountId, Guid spendingLimitId, SpendingLimitManagementService service, CancellationToken ct) =>
        {
            var result = await service.SetStatusAsync(schoolAccountId, spendingLimitId, SafeSchool.Api.Features.Wallet.Common.SpendingLimitStatus.Active, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapPost("/spending-limits/{spendingLimitId:guid}/suspend", async (string schoolAccountId, Guid spendingLimitId, SpendingLimitManagementService service, CancellationToken ct) =>
        {
            var result = await service.SetStatusAsync(schoolAccountId, spendingLimitId, SafeSchool.Api.Features.Wallet.Common.SpendingLimitStatus.Suspended, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/student-wallets/{walletId:guid}/spending-limits/effective", async (string schoolAccountId, Guid walletId, SpendingLimitEvaluationService service, CancellationToken ct) => Results.Ok(await service.EvaluateAsync(schoolAccountId, walletId, 0, cancellationToken: ct)));
        schoolGroup.MapGet("/spending-limits/{spendingLimitId:guid}/trace", (Guid spendingLimitId, SpendingLimitTraceService service) => Results.Ok(service.Trace(spendingLimitId)));
        guardianGroup.MapPost("/{studentProfileId}/wallet/spending-limits", async (string studentProfileId, SpendingLimitRequest request, SpendingLimitManagementService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync("demo-school", request with { OwnerType = SafeSchool.Api.Features.Wallet.Common.SpendingLimitOwnerType.Guardian }, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        guardianGroup.MapGet("/{studentProfileId}/wallet/spending-limits", async (string studentProfileId, SpendingLimitManagementService service, CancellationToken ct) => Results.Ok(await service.ListAsync("demo-school", cancellationToken: ct)));
        return schoolGroup;
    }
}
