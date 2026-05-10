using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public static class StudentWalletsController
{
    public static RouteGroupBuilder MapStudentWalletEndpoints(this RouteGroupBuilder group, RouteGroupBuilder guardianGroup)
    {
        var wallets = group.MapGroup("/student-wallets");
        wallets.MapGet("/", async (string schoolAccountId, StudentWalletService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        wallets.MapPost("/", async (string schoolAccountId, WalletCreateRequest request, StudentWalletService service, CancellationToken ct) =>
        {
            var result = await service.CreateOrActivateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{result.Value!.WalletId}", result.Value) : Results.BadRequest(result.Errors);
        });
        wallets.MapPost("/bulk-activate", async (string schoolAccountId, BulkActivateWalletsRequest request, StudentWalletService service, CancellationToken ct) => Results.Ok(await service.BulkActivateAsync(schoolAccountId, request, ct)));
        wallets.MapGet("/{walletId:guid}", async (string schoolAccountId, Guid walletId, StudentWalletService service, CancellationToken ct) =>
        {
            var wallet = await service.GetAsync(schoolAccountId, walletId, ct);
            return wallet is null ? Results.NotFound() : Results.Ok(wallet);
        });
        wallets.MapPost("/{walletId:guid}/restrict", async (string schoolAccountId, Guid walletId, WalletRestrictionRequest request, StudentWalletService service, CancellationToken ct) =>
        {
            var result = await service.ChangeStatusAsync(schoolAccountId, walletId, WalletStatus.Restricted, request.Reason, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        wallets.MapPost("/{walletId:guid}/restore", async (string schoolAccountId, Guid walletId, WalletRestoreRequest request, StudentWalletService service, CancellationToken ct) =>
        {
            var result = await service.ChangeStatusAsync(schoolAccountId, walletId, WalletStatus.Active, request.Reason, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        wallets.MapPost("/{walletId:guid}/suspend", async (string schoolAccountId, Guid walletId, WalletRestrictionRequest request, StudentWalletService service, CancellationToken ct) =>
        {
            var result = await service.ChangeStatusAsync(schoolAccountId, walletId, WalletStatus.Suspended, request.Reason, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        wallets.MapPost("/{walletId:guid}/close", async (string schoolAccountId, Guid walletId, WalletRestrictionRequest request, StudentWalletService service, CancellationToken ct) =>
        {
            var result = await service.ChangeStatusAsync(schoolAccountId, walletId, WalletStatus.Closed, request.Reason, request.ActorReference, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        wallets.MapGet("/{walletId:guid}/ledger-entries", async (string schoolAccountId, Guid walletId, WalletLedgerQueryService service, CancellationToken ct) => Results.Ok(await service.ListForWalletAsync(schoolAccountId, walletId, ct)));
        wallets.MapGet("/{walletId:guid}/trace", async (string schoolAccountId, Guid walletId, WalletTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, walletId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        group.MapGet("/ledger-entries/{ledgerEntryId:guid}", async (string schoolAccountId, Guid ledgerEntryId, WalletLedgerQueryService service, CancellationToken ct) =>
        {
            var entry = await service.GetAsync(schoolAccountId, ledgerEntryId, ct);
            return entry is null ? Results.NotFound() : Results.Ok(entry);
        });
        guardianGroup.MapGet("/{studentProfileId}/wallet", async (string studentProfileId, ITenantContext tenantContext, GuardianWalletVisibilityService service, CancellationToken ct) =>
        {
            var result = await service.GetAsync(GuardianTenantResolver.Resolve(tenantContext), tenantContext.ActorReference ?? "anonymous", studentProfileId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
