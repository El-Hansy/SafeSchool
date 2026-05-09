using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Reviews;

public sealed class ManualReviewService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<ManualReviewResponse>> CreateAsync(string tenantId, CreateManualReviewRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return OperationResult<ManualReviewResponse>.Failure(new ValidationError("missing_reason", "Manual review requires a reason."));
        }

        var review = new ManualReview
        {
            TenantId = tenantId,
            TargetType = request.TargetType,
            TargetReference = request.TargetReference,
            Reason = request.Reason
        };
        dbContext.ManualReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<ManualReviewResponse>.Success(review.ToResponse());
    }
}

