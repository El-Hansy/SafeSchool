using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.History;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Corrections;

public sealed class WalletCorrectionService(SafeSchoolDbContext dbContext, WalletLedgerPostingService ledger)
{
    public async Task<OperationResult<CorrectionResponse>> CreateAsync(string tenantId, RefundOrReversalRequest request, CancellationToken cancellationToken = default)
    {
        if (request.AmountMinor <= 0) return OperationResult<CorrectionResponse>.Failure(new ValidationError("invalid_amount", "Correction amount must be positive."));
        var correction = new RefundOrReversal { TenantId = tenantId, StudentWalletId = request.WalletId, CorrectionType = request.CorrectionType, OriginalSourceType = request.OriginalSourceType, OriginalSourceReference = request.OriginalSourceReference, AmountMinor = request.AmountMinor, CurrencyCode = request.CurrencyCode, Reason = request.Reason, ClientRequestId = request.ClientRequestId, ReviewerActor = request.ActorReference, Status = CorrectionStatus.Approved };
        dbContext.RefundOrReversals.Add(correction);
        await dbContext.SaveChangesAsync(cancellationToken);
        var entryType = request.CorrectionType is CorrectionType.Refund or CorrectionType.Release ? WalletLedgerEntryType.Refund : WalletLedgerEntryType.Reversal;
        var posted = await ledger.PostAsync(tenantId, new PostLedgerRequest(request.WalletId, entryType, request.AmountMinor, request.CurrencyCode, request.CorrectionType.ToString(), correction.Id.ToString(), $"correction-{request.ClientRequestId}-{correction.Id}", request.ActorReference, request.Reason), cancellationToken);
        if (posted.Succeeded) { correction.Status = CorrectionStatus.Posted; correction.ResultingLedgerEntryId = posted.Value!.LedgerEntryId; await dbContext.SaveChangesAsync(cancellationToken); }
        return OperationResult<CorrectionResponse>.Success(new CorrectionResponse(correction.Id, correction.StudentWalletId, correction.CorrectionType, correction.Status, correction.AmountMinor, correction.Reason, correction.ResultingLedgerEntryId));
    }
}
