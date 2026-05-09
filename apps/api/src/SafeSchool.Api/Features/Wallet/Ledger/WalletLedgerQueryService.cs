using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Ledger;

public sealed class WalletLedgerQueryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<LedgerEntryResponse>> ListForWalletAsync(string tenantId, Guid walletId, CancellationToken cancellationToken = default) =>
        await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId && x.StudentWalletId == walletId).OrderByDescending(x => x.PostedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<LedgerEntryResponse?> GetAsync(string tenantId, Guid ledgerEntryId, CancellationToken cancellationToken = default) =>
        await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId && x.Id == ledgerEntryId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);
}
