using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Reviews;

public sealed record ManualTransportReviewRequest(string SourceRecordType, string SourceRecordReference, ManualReviewAction ReviewAction, string CorrectedStatus, string ReviewReason, string ClientRequestId);

public sealed class ManualTransportReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<ManualTransportReview>> CreateAsync(string tenantId, ManualTransportReviewRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ReviewReason)) return OperationResult<ManualTransportReview>.Failure(new ValidationError("missing_reason", "Manual review requires a reason."));
        var review = new ManualTransportReview { TenantId = tenantId, SourceRecordType = request.SourceRecordType, SourceRecordReference = request.SourceRecordReference, ReviewAction = request.ReviewAction, CorrectedStatus = request.CorrectedStatus, ReviewReason = request.ReviewReason, ReviewedBy = "reviewer" };
        dbContext.ManualTransportReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<ManualTransportReview>.Success(review);
    }
}
