using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Idempotency;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Ledger;

public sealed class WalletLedgerPostingService(
    SafeSchoolDbContext dbContext,
    WalletPermissionGuard guard,
    WalletIdempotencyService idempotencyService,
    IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<LedgerEntryResponse>> PostAsync(string tenantId, PostLedgerRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.Ledger, WalletPermissionCatalog.LedgerPost, targetType: "StudentWallet", targetReference: request.WalletId.ToString(), cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<LedgerEntryResponse>.Failure(allowed.Errors.ToArray());
        if (request.AmountMinor <= 0) return OperationResult<LedgerEntryResponse>.Failure(new ValidationError("invalid_amount", "Ledger posting amount must be positive minor units.", nameof(request.AmountMinor)));

        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.WalletId, cancellationToken);
        if (wallet is null) return OperationResult<LedgerEntryResponse>.Failure(new ValidationError("wallet_not_found", "Wallet was not found in this school account."));
        if (wallet.WalletStatus is WalletStatus.Closed or WalletStatus.Suspended) return OperationResult<LedgerEntryResponse>.Failure(new ValidationError("wallet_not_spendable", "Closed or suspended wallets cannot receive normal ledger postings."));

        var idempotency = await idempotencyService.ReserveAsync(tenantId, "ledger", request.IdempotencyKey, request.SourceReference, $"{request.EntryType}:{request.AmountMinor}:{request.CurrencyCode}", cancellationToken);
        if (!idempotency.Succeeded) return OperationResult<LedgerEntryResponse>.Failure(idempotency.Errors.ToArray());
        var existing = await dbContext.WalletLedgerEntries.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.IdempotencyKey == request.IdempotencyKey, cancellationToken);
        if (existing is not null) return OperationResult<LedgerEntryResponse>.Success(existing.ToResponse());

        var debitTypes = new[] { WalletLedgerEntryType.Debit, WalletLedgerEntryType.Hold, WalletLedgerEntryType.Chargeback, WalletLedgerEntryType.Reversal };
        var signedAmount = debitTypes.Contains(request.EntryType) ? -request.AmountMinor : request.AmountMinor;
        if (signedAmount < 0 && wallet.AvailableBalanceMinor + signedAmount < 0 && request.EntryType != WalletLedgerEntryType.Chargeback)
        {
            return OperationResult<LedgerEntryResponse>.Failure(new ValidationError("insufficient_funds", "Wallet available balance cannot go below zero for normal debits."));
        }

        wallet.AvailableBalanceMinor += signedAmount;
        if (request.EntryType == WalletLedgerEntryType.Hold) wallet.HeldBalanceMinor += request.AmountMinor;
        if (request.EntryType == WalletLedgerEntryType.Release) wallet.HeldBalanceMinor = Math.Max(0, wallet.HeldBalanceMinor - request.AmountMinor);
        if (request.EntryType == WalletLedgerEntryType.Chargeback)
        {
            wallet.PendingRecoveryMinor += request.AmountMinor;
            wallet.Restrict(request.Reason ?? "Chargeback recovery pending", request.CreatedBy);
        }
        wallet.UpdatedAt = DateTimeOffset.UtcNow;

        var entry = new WalletLedgerEntry
        {
            TenantId = tenantId,
            StudentWalletId = wallet.Id,
            EntryType = request.EntryType,
            EntryStatus = WalletLedgerEntryStatus.Approved,
            AmountMinor = request.AmountMinor,
            CurrencyCode = request.CurrencyCode,
            BalanceAvailableAfterMinor = wallet.AvailableBalanceMinor,
            BalancePendingAfterMinor = wallet.PendingBalanceMinor,
            BalanceHeldAfterMinor = wallet.HeldBalanceMinor,
            SourceType = request.SourceType,
            SourceReference = request.SourceReference,
            IdempotencyKey = request.IdempotencyKey,
            ReviewReason = request.Reason ?? string.Empty,
            CreatedBy = request.CreatedBy,
            PostedAt = DateTimeOffset.UtcNow
        };
        dbContext.WalletLedgerEntries.Add(entry);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "Ledger", EventType = "wallet.ledger.post", SubjectType = "WalletLedgerEntry", SubjectReference = entry.Id.ToString(), ActorReference = request.CreatedBy, Reason = request.Reason ?? "ledger posting" }, cancellationToken);
        return OperationResult<LedgerEntryResponse>.Success(entry.ToResponse());
    }
}
