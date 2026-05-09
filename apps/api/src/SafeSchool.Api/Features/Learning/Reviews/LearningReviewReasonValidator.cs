using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Reviews;

public sealed class LearningReviewReasonValidator
{
    private static readonly HashSet<ReviewAction> RequiresReason = [ReviewAction.Correct, ReviewAction.Reopen, ReviewAction.Close, ReviewAction.Resolve, ReviewAction.Dismiss, ReviewAction.Escalate, ReviewAction.MigrateRuleVersion, ReviewAction.Withdraw, ReviewAction.Deny, ReviewAction.Cancel, ReviewAction.ChangeSensitiveVisibility];

    public OperationResult<string> Validate(ReviewAction action, string reason)
    {
        if (RequiresReason.Contains(action) && string.IsNullOrWhiteSpace(reason))
        {
            return OperationResult<string>.Failure(new ValidationError("review_reason_required", $"{action} requires a reason.", nameof(reason)));
        }

        return OperationResult<string>.Success(reason);
    }
}
