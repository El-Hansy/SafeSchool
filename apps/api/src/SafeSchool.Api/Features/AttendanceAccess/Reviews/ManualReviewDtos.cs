using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Reviews;

public sealed record CreateManualReviewRequest(string TargetType, string TargetReference, string Reason);
public sealed record ManualReviewResponse(Guid ManualReviewId, string TargetType, string TargetReference, ManualReviewStatus Status, string ReviewerReference, string Reason);

public static class ManualReviewMappings
{
    public static ManualReviewResponse ToResponse(this ManualReview review) =>
        new(review.Id, review.TargetType, review.TargetReference, review.Status, review.ReviewerReference, review.Reason);
}

