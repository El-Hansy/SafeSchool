using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class CanteenPurchaseTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<PurchaseTraceResponse?> TraceAsync(string tenantId, Guid purchaseId, CancellationToken cancellationToken = default)
    {
        var purchase = await dbContext.CanteenPurchaseTransactions.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == purchaseId, cancellationToken);
        if (purchase is null) return null;
        var refs = new List<string> { $"purchase:{purchase.Id}", $"wallet:{purchase.StudentWalletId}", $"terminal:{purchase.PosTerminalId}", $"rule:{purchase.RuleSnapshotReference}" };
        if (purchase.LedgerDebitEntryId is Guid ledgerId) refs.Add($"ledger:{ledgerId}");
        return new PurchaseTraceResponse(purchase.Id, refs);
    }
}
