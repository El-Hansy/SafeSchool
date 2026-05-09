using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.History;

public sealed class TransactionHistoryQueryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<TransactionHistoryResponse>> SchoolHistoryAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var ledger = await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.PostedAt).Take(100).Select(x => new TransactionHistoryResponse(x.Id.ToString(), x.StudentWalletId, x.EntryType.ToString(), x.AmountMinor, x.CurrencyCode, x.EntryStatus.ToString(), x.SourceType, x.PostedAt, false)).ToListAsync(cancellationToken);
        return ledger;
    }

    public async Task<IReadOnlyList<TransactionHistoryResponse>> WalletHistoryAsync(string tenantId, Guid walletId, CancellationToken cancellationToken = default) => (await SchoolHistoryAsync(tenantId, cancellationToken)).Where(x => x.WalletId == walletId).ToList();
    public async Task<IReadOnlyList<TransactionHistoryResponse>> GuardianHistoryAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var walletIds = await dbContext.StudentWallets.Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId).Select(x => x.Id).ToListAsync(cancellationToken);
        return (await SchoolHistoryAsync(tenantId, cancellationToken)).Where(x => walletIds.Contains(x.WalletId)).Select(x => x with { StaffOnlyDetailSuppressed = true }).ToList();
    }
}
