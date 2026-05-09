using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Ledger;

public sealed class WalletBalanceProjectionService(SafeSchoolDbContext dbContext)
{
    public async Task<long> ProjectAvailableBalanceAsync(string tenantId, Guid walletId, CancellationToken cancellationToken = default) =>
        await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId && x.StudentWalletId == walletId && x.EntryStatus == SafeSchool.Api.Features.Wallet.Common.WalletLedgerEntryStatus.Approved)
            .SumAsync(x => x.EntryType == SafeSchool.Api.Features.Wallet.Common.WalletLedgerEntryType.Debit ? -x.AmountMinor : x.AmountMinor, cancellationToken);
}
