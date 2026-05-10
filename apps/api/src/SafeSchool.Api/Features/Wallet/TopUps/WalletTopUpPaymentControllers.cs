using SafeSchool.Api.Features.Wallet.Payments;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public static class WalletTopUpPaymentControllers
{
    public static RouteGroupBuilder MapWalletTopUpPaymentEndpoints(this RouteGroupBuilder schoolGroup, RouteGroupBuilder guardianGroup)
    {
        guardianGroup.MapPost("/{studentProfileId}/wallet/top-ups", async (string studentProfileId, GuardianTopUpRequest request, ITenantContext tenantContext, GuardianTopUpService service, CancellationToken ct) =>
        {
            var tenantId = GuardianTenantResolver.Resolve(tenantContext);
            var result = await service.InitiateAsync(tenantId, studentProfileId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/guardians/me/students/{studentProfileId}/wallet/top-ups/{result.Value!.TopUpId}", result.Value) : Results.BadRequest(result.Errors);
        });
        guardianGroup.MapGet("/{studentProfileId}/wallet/top-ups", async (string studentProfileId, ITenantContext tenantContext, TopUpQueryService service, CancellationToken ct) =>
            Results.Ok(await service.ListGuardianAsync(GuardianTenantResolver.Resolve(tenantContext), studentProfileId, ct)));

        schoolGroup.MapPost("/top-ups/cashier", async (string schoolAccountId, CashierTopUpRequest request, CashierTopUpService service, CancellationToken ct) =>
        {
            var result = await service.RecordAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/top-ups/{result.Value!.TopUpId}", result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/top-ups", async (string schoolAccountId, TopUpQueryService service, CancellationToken ct) => Results.Ok(await service.ListSchoolAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/top-ups/{topUpId:guid}", async (string schoolAccountId, Guid topUpId, TopUpQueryService service, CancellationToken ct) =>
        {
            var topUp = await service.GetAsync(schoolAccountId, topUpId, ct);
            return topUp is null ? Results.NotFound() : Results.Ok(topUp);
        });
        schoolGroup.MapPost("/payment-confirmations", async (string schoolAccountId, PaymentConfirmationRequest request, PaymentConfirmationService service, CancellationToken ct) =>
        {
            var result = await service.ReceiveAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/payment-confirmations/{paymentConfirmationId:guid}", async (string schoolAccountId, Guid paymentConfirmationId) => Results.Ok(new { paymentConfirmationId, schoolAccountId, safePaymentOnly = true }));
        schoolGroup.MapPost("/top-ups/{topUpId:guid}/mark-disputed", async (string schoolAccountId, Guid topUpId) => Results.Ok(new { topUpId, status = "Disputed", schoolAccountId }));
        schoolGroup.MapPost("/top-ups/{topUpId:guid}/chargeback-review", async (string schoolAccountId, Guid topUpId, ChargebackReviewRequest request, ChargebackRecoveryService service, CancellationToken ct) =>
        {
            var result = await service.ApplyAsync(schoolAccountId, topUpId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/top-ups/{topUpId:guid}/trace", async (string schoolAccountId, Guid topUpId, TopUpTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, topUpId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        return schoolGroup;
    }
}
