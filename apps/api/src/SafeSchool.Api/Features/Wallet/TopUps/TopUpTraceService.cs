using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed class TopUpTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<TopUpTraceResponse?> TraceAsync(string tenantId, Guid topUpId, CancellationToken cancellationToken = default)
    {
        var topUp = await dbContext.WalletTopUps.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == topUpId, cancellationToken);
        if (topUp is null) return null;
        var refs = new List<string> { $"topup:{topUp.Id}", $"wallet:{topUp.StudentWalletId}" };
        refs.AddRange(await dbContext.PaymentConfirmations.Where(x => x.TenantId == tenantId && x.WalletTopUpId == topUpId).Select(x => $"payment:{x.Id}:{x.ConfirmationStatus}").ToListAsync(cancellationToken));
        refs.AddRange(await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId && x.SourceReference == topUpId.ToString()).Select(x => $"ledger:{x.Id}:{x.EntryType}").ToListAsync(cancellationToken));
        return new TopUpTraceResponse(topUp.Id, refs);
    }
}
