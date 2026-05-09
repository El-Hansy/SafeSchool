using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.History;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Anomalies;

public sealed class WalletAnomalyReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<AnomalyResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.WalletAnomalies.Where(x => x.TenantId == tenantId).Select(x => new AnomalyResponse(x.Id, x.AnomalyType, x.Severity, x.Status, x.RelatedSourceReference, x.ResolutionReason)).ToListAsync(cancellationToken);
}
