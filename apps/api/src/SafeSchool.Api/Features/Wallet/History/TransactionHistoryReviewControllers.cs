using SafeSchool.Api.Features.Wallet.Anomalies;
using SafeSchool.Api.Features.Wallet.Corrections;
using SafeSchool.Api.Features.Wallet.Reviews;

namespace SafeSchool.Api.Features.Wallet.History;

public static class TransactionHistoryReviewControllers
{
    public static RouteGroupBuilder MapTransactionHistoryReviewEndpoints(this RouteGroupBuilder schoolGroup, RouteGroupBuilder guardianGroup)
    {
        schoolGroup.MapGet("/transactions", async (string schoolAccountId, TransactionHistoryQueryService service, CancellationToken ct) => Results.Ok(await service.SchoolHistoryAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/student-wallets/{walletId:guid}/transactions", async (string schoolAccountId, Guid walletId, TransactionHistoryQueryService service, CancellationToken ct) => Results.Ok(await service.WalletHistoryAsync(schoolAccountId, walletId, ct)));
        guardianGroup.MapGet("/{studentProfileId}/wallet/transactions", async (string studentProfileId, TransactionHistoryQueryService service, CancellationToken ct) => Results.Ok(await service.GuardianHistoryAsync("demo-school", studentProfileId, ct)));
        schoolGroup.MapGet("/transactions/{transactionId}", (string transactionId) => Results.Ok(new { transactionId, staffOnlyDetailSuppressed = false }));
        schoolGroup.MapPost("/reviews", async (string schoolAccountId, ManualReviewRequest request, ManualWalletReviewService service, CancellationToken ct) => Results.Ok(await service.CreateAsync(schoolAccountId, request, ct)));
        schoolGroup.MapGet("/reviews", async (string schoolAccountId, ManualWalletReviewService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        schoolGroup.MapGet("/reviews/{manualWalletReviewId:guid}", (Guid manualWalletReviewId) => Results.Ok(new { manualWalletReviewId }));
        schoolGroup.MapPost("/refunds-reversals", async (string schoolAccountId, RefundOrReversalRequest request, WalletCorrectionService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        schoolGroup.MapGet("/refunds-reversals/{refundReversalId:guid}", (Guid refundReversalId) => Results.Ok(new { refundReversalId }));
        schoolGroup.MapGet("/transactions/{transactionId}/trace", (string transactionId, TransactionTraceService service) => Results.Ok(service.Trace(transactionId)));
        schoolGroup.MapGet("/anomalies", async (string schoolAccountId, WalletAnomalyReviewService service, CancellationToken ct) => Results.Ok(await service.ListAsync(schoolAccountId, ct)));
        return schoolGroup;
    }
}
