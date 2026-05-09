using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class WalletReviewSummaryService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<ReviewSummaryResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var summaries = await dbContext.WalletReviewSummaries.Where(x => x.TenantId == tenantId).ToListAsync(cancellationToken);
        return summaries.Select(x => x.ToResponse()).ToList();
    }
    public async Task<ReviewSummaryResponse> GuardianSummaryAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var walletCount = await dbContext.StudentWallets.CountAsync(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId, cancellationToken);
        var txCount = await dbContext.WalletLedgerEntries.CountAsync(x => x.TenantId == tenantId, cancellationToken);
        return new ReviewSummaryResponse(Guid.NewGuid(), "student", studentProfileId, walletCount, txCount, 0, 0, SafeSchool.Api.Features.Wallet.Common.ReviewSummaryStatus.Current, true);
    }
}
