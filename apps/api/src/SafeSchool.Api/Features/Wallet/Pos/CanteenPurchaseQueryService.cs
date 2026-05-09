using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class CanteenPurchaseQueryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<PurchaseResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.CanteenPurchaseTransactions.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.ReceivedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<PurchaseResponse?> GetAsync(string tenantId, Guid purchaseId, CancellationToken cancellationToken = default) => await dbContext.CanteenPurchaseTransactions.Where(x => x.TenantId == tenantId && x.Id == purchaseId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);
}
