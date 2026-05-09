using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed class TopUpQueryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<TopUpResponse>> ListSchoolAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.WalletTopUps.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.InitiatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<IReadOnlyList<TopUpResponse>> ListGuardianAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) => await dbContext.WalletTopUps.Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId).OrderByDescending(x => x.InitiatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<TopUpResponse?> GetAsync(string tenantId, Guid topUpId, CancellationToken cancellationToken = default) => await dbContext.WalletTopUps.Where(x => x.TenantId == tenantId && x.Id == topUpId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);
}
