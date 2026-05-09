using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.TopUps;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Payments;

public sealed class ChargebackRecoveryService(SafeSchoolDbContext dbContext, WalletLedgerPostingService ledger)
{
    public async Task<OperationResult<TopUpResponse>> ApplyAsync(string tenantId, Guid topUpId, ChargebackReviewRequest request, CancellationToken cancellationToken = default)
    {
        var topUp = await dbContext.WalletTopUps.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == topUpId, cancellationToken);
        if (topUp is null) return OperationResult<TopUpResponse>.Failure(new ValidationError("top_up_not_found", "Top-up was not found."));
        topUp.TopUpStatus = WalletTopUpStatus.ChargedBack;
        topUp.ReviewReason = request.Reason;
        await dbContext.SaveChangesAsync(cancellationToken);
        await ledger.PostAsync(tenantId, new PostLedgerRequest(topUp.StudentWalletId, WalletLedgerEntryType.Chargeback, request.AmountMinor, topUp.CurrencyCode, "Chargeback", topUp.Id.ToString(), $"chargeback-{topUp.Id}-{request.AmountMinor}", request.ActorReference, request.Reason), cancellationToken);
        return OperationResult<TopUpResponse>.Success(topUp.ToResponse());
    }
}
