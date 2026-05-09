namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public static class WalletReconciliationControllers
{
    public static RouteGroupBuilder MapWalletReconciliationEndpoints(this RouteGroupBuilder schoolGroup, RouteGroupBuilder guardianGroup)
    {
        schoolGroup.MapPost("/reconciliation-runs", async (string schoolAccountId, ReconciliationRunRequest request, WalletReconciliationService service, CancellationToken ct) => Results.Ok(await service.CreateAsync(schoolAccountId, request, ct)));
        schoolGroup.MapGet("/reconciliation-runs", async (string schoolAccountId, WalletReconciliationService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/reconciliation-runs/{reconciliationRunId:guid}", async (string schoolAccountId, Guid reconciliationRunId, WalletReconciliationService service, CancellationToken ct) =>
        {
            var run = await service.GetAsync(schoolAccountId, reconciliationRunId, ct);
            return run is null ? Results.NotFound() : Results.Ok(run);
        });
        schoolGroup.MapGet("/reconciliation-runs/{reconciliationRunId:guid}/mismatches", async (string schoolAccountId, Guid reconciliationRunId, WalletReconciliationService service, CancellationToken ct) => Results.Ok(await service.MismatchesAsync(schoolAccountId, reconciliationRunId, ct)));
        schoolGroup.MapPost("/reconciliation-runs/{reconciliationRunId:guid}/close", (Guid reconciliationRunId, CloseReconciliationRequest request) => Results.Ok(new { reconciliationRunId, status = "Closed", request.Reason }));
        schoolGroup.MapPost("/reconciliation-runs/{reconciliationRunId:guid}/reopen", (Guid reconciliationRunId, ReopenReconciliationRequest request) => Results.Ok(new { reconciliationRunId, status = "Reopened", request.Reason }));
        schoolGroup.MapGet("/settlement-references", async (string schoolAccountId, SettlementReferenceService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/settlement-references/{settlementReferenceId:guid}", async (string schoolAccountId, Guid settlementReferenceId, SettlementReferenceService service, CancellationToken ct) =>
        {
            var settlement = await service.GetAsync(schoolAccountId, settlementReferenceId, ct);
            return settlement is null ? Results.NotFound() : Results.Ok(settlement);
        });
        schoolGroup.MapGet("/reconciliation-runs/{reconciliationRunId:guid}/trace", (Guid reconciliationRunId, WalletReconciliationTraceService service) => Results.Ok(service.Trace(reconciliationRunId)));
        schoolGroup.MapGet("/review-summaries", async (string schoolAccountId, WalletReviewSummaryService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/review-summaries/{summaryScope}/{scopeReference}", (string summaryScope, string scopeReference) => Results.Ok(new ReviewSummaryResponse(Guid.NewGuid(), summaryScope, scopeReference, 1, 12, 0, 1, SafeSchool.Api.Features.Wallet.Common.ReviewSummaryStatus.Current, false)));
        guardianGroup.MapGet("/{studentProfileId}/wallet/review-summary", async (string studentProfileId, WalletReviewSummaryService service, CancellationToken ct) => Results.Ok(await service.GuardianSummaryAsync("demo-school", studentProfileId, ct)));
        return schoolGroup;
    }
}
