using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class ManualLearningReview : TenantOwnedEntity
{
    public string SourceType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public ReviewAction ReviewAction { get; set; } = ReviewAction.Resolve;
    public string ReviewReason { get; set; } = string.Empty;
    public string ReviewerActorId { get; set; } = string.Empty;
    public string OriginalStatus { get; set; } = string.Empty;
    public string ResultingStatus { get; set; } = string.Empty;
}
