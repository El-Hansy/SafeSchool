using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.History;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Reviews;

public sealed class ManualWalletReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<ManualReviewResponse> CreateAsync(string tenantId, ManualReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = new ManualWalletReview { TenantId = tenantId, ReviewScope = request.ReviewScope, ScopeReference = request.ScopeReference, ReviewAction = request.Action, Reason = request.Reason, ReviewerActor = request.ReviewerActor, ClientRequestId = request.ClientRequestId, ReviewStatus = SafeSchool.Api.Features.Wallet.Common.ManualWalletReviewStatus.InReview };
        dbContext.ManualWalletReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ManualReviewResponse(review.Id, review.ReviewScope, review.ScopeReference, review.ReviewAction, review.ReviewStatus, review.Reason);
    }

    public async Task<IReadOnlyList<ManualReviewResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.ManualWalletReviews.Where(x => x.TenantId == tenantId).Select(x => new ManualReviewResponse(x.Id, x.ReviewScope, x.ScopeReference, x.ReviewAction, x.ReviewStatus, x.Reason)).ToListAsync(cancellationToken);
}
