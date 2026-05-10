using SafeSchool.Api.Features.Wallet.Canteen;
using SafeSchool.Api.Features.Wallet.Sync;

namespace SafeSchool.Api.Features.Wallet.Pos;

public static class CanteenPosPurchaseControllers
{
    public static RouteGroupBuilder MapCanteenPosPurchaseEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/canteen/merchants", async (string schoolAccountId, MerchantRequest request, CanteenCatalogService service, CancellationToken ct) =>
        {
            var result = await service.CreateMerchantAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/canteen/merchants/{result.Value!.MerchantId}", result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPatch("/canteen/merchants/{merchantId:guid}", (Guid merchantId, MerchantRequest request) => Results.Ok(new { merchantId, request.MerchantName, request.Status }));
        group.MapPost("/canteen/item-categories", async (string schoolAccountId, ItemCategoryRequest request, CanteenCatalogService service, CancellationToken ct) => Results.Ok(new { itemCategoryId = await service.CreateItemCategoryAsync(schoolAccountId, request, ct) }));
        group.MapPatch("/canteen/item-categories/{itemCategoryId:guid}", (Guid itemCategoryId, ItemCategoryRequest request) => Results.Ok(new { itemCategoryId, request.DisplayName, request.Status }));
        group.MapPost("/canteen/purchase-eligibility-rules", async (string schoolAccountId, PurchaseEligibilityRuleRequest request, CanteenCatalogService service, CancellationToken ct) => Results.Ok(new { purchaseEligibilityRuleId = await service.CreateEligibilityRuleAsync(schoolAccountId, request, ct) }));
        group.MapPatch("/canteen/purchase-eligibility-rules/{purchaseEligibilityRuleId:guid}", (Guid purchaseEligibilityRuleId, PurchaseEligibilityRuleRequest request) => Results.Ok(new { purchaseEligibilityRuleId, request.Status }));
        group.MapPost("/canteen/terminals", async (string schoolAccountId, PosTerminalRequest request, POSTerminalService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/canteen/terminals/{result.Value!.TerminalId}", result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPatch("/canteen/terminals/{terminalId:guid}", (Guid terminalId, PosTerminalRequest request) => Results.Ok(new { terminalId, request.Status }));
        group.MapPost("/canteen/purchases", async (string schoolAccountId, OnlinePurchaseRequest request, OnlinePurchaseAuthorizationService service, CancellationToken ct) =>
        {
            var result = await service.RecordAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapPost("/canteen/purchases/sync", async (string schoolAccountId, OfflineSyncRequest request, OfflinePosSyncService service, CancellationToken ct) =>
        {
            var result = await service.SyncAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        group.MapGet("/canteen/purchases", async (string schoolAccountId, CanteenPurchaseQueryService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        group.MapGet("/canteen/purchases/{purchaseId:guid}", async (string schoolAccountId, Guid purchaseId, CanteenPurchaseQueryService service, CancellationToken ct) =>
        {
            var purchase = await service.GetAsync(schoolAccountId, purchaseId, ct);
            return purchase is null ? Results.NotFound() : Results.Ok(purchase);
        });
        group.MapGet("/canteen/purchases/{purchaseId:guid}/trace", async (string schoolAccountId, Guid purchaseId, CanteenPurchaseTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, purchaseId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        group.MapPost("/canteen/purchases/{purchaseId:guid}/review-outcome", (Guid purchaseId, ReviewOutcomeRequest request) => Results.Ok(new { purchaseId, request.Action, request.Reason }));
        return group;
    }
}
