using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class BehaviorEvent : TenantOwnedEntity
{
    public Guid BehaviorCategoryId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public BehaviorClassification Classification { get; set; } = BehaviorClassification.Neutral;
    public BehaviorSeverity Severity { get; set; } = BehaviorSeverity.Low;
    public string SourceContext { get; set; } = string.Empty;
    public string StaffNote { get; set; } = string.Empty;
    public string VisibilityPolicy { get; set; } = "guardian_summary";
    public BehaviorReviewState ReviewState { get; set; } = BehaviorReviewState.Accepted;
    public string RelatedStarLedgerReference { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
}
