using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed class WalletTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<WalletTraceResponse?> TraceAsync(string tenantId, Guid walletId, CancellationToken cancellationToken = default)
    {
        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == walletId, cancellationToken);
        if (wallet is null) return null;
        var refs = await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId && x.StudentWalletId == walletId).OrderByDescending(x => x.PostedAt).Take(25).Select(x => new WalletTraceReference("ledger", x.Id.ToString(), x.SourceType, x.PostedAt)).ToListAsync(cancellationToken);
        return new WalletTraceResponse(wallet.Id, refs.Select(x => $"{x.ReferenceType}:{x.ReferenceId}:{x.Label}").ToList(), wallet.AvailableBalanceMinor, wallet.HeldBalanceMinor, wallet.PendingRecoveryMinor);
    }
}
