using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed class CashierTopUpService(SafeSchoolDbContext dbContext, WalletPermissionGuard guard, WalletLedgerPostingService ledger, IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<TopUpResponse>> RecordAsync(string tenantId, CashierTopUpRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.TopUp, WalletPermissionCatalog.TopUpsCashier, targetType: "WalletTopUp", targetReference: request.WalletId.ToString(), cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<TopUpResponse>.Failure(allowed.Errors.ToArray());
        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.WalletId, cancellationToken);
        if (wallet is null || wallet.WalletStatus != WalletStatus.Active) return OperationResult<TopUpResponse>.Failure(new ValidationError("wallet_not_active", "Cashier top-up requires an active wallet."));
        var existing = await dbContext.WalletTopUps.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.IdempotencyKey == request.ClientRequestId, cancellationToken);
        if (existing is not null) return OperationResult<TopUpResponse>.Success(existing.ToResponse());
        var topUp = new WalletTopUp { TenantId = tenantId, StudentWalletId = wallet.Id, StudentProfileId = wallet.StudentProfileId, InitiatedByActorId = request.ActorReference, TopUpSource = WalletTopUpSource.AuthorizedCashier, AmountMinor = request.AmountMinor, NetCreditMinor = request.AmountMinor, CurrencyCode = request.CurrencyCode, TopUpStatus = WalletTopUpStatus.Confirmed, CashierReference = request.CashierReference, IdempotencyKey = request.ClientRequestId, ConfirmedAt = DateTimeOffset.UtcNow };
        dbContext.WalletTopUps.Add(topUp);
        await dbContext.SaveChangesAsync(cancellationToken);
        var posted = await ledger.PostAsync(tenantId, new PostLedgerRequest(wallet.Id, WalletLedgerEntryType.Credit, request.AmountMinor, request.CurrencyCode, "CashierTopUp", topUp.Id.ToString(), $"ledger-{request.ClientRequestId}", request.ActorReference, request.Reason), cancellationToken);
        if (posted.Succeeded) { topUp.TopUpStatus = WalletTopUpStatus.Credited; topUp.CreditedAt = DateTimeOffset.UtcNow; await dbContext.SaveChangesAsync(cancellationToken); }
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "TopUp", EventType = "wallet.topups.cashier_credited", SubjectType = "WalletTopUp", SubjectReference = topUp.Id.ToString(), ActorReference = request.ActorReference, Reason = request.Reason }, cancellationToken);
        return OperationResult<TopUpResponse>.Success(topUp.ToResponse());
    }
}
