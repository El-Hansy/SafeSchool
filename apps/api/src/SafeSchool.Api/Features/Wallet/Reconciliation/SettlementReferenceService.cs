using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class SettlementReferenceService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<SettlementReferenceResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.SettlementReferences.Where(x => x.TenantId == tenantId).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<SettlementReferenceResponse?> GetAsync(string tenantId, Guid settlementReferenceId, CancellationToken cancellationToken = default) => await dbContext.SettlementReferences.Where(x => x.TenantId == tenantId && x.Id == settlementReferenceId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);
}
