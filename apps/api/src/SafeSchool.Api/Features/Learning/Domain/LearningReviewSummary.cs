using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningReviewSummary : TenantOwnedEntity
{
    public SummaryScope SummaryScope { get; set; } = SummaryScope.Student;
    public string ScopeReference { get; set; } = string.Empty;
    public SummaryStatus SummaryStatus { get; set; } = SummaryStatus.Current;
    public string LatestEvidenceReference { get; set; } = string.Empty;
    public bool StaffOnlyDetailsHidden { get; set; } = true;
    public DateTimeOffset RefreshedAt { get; set; } = DateTimeOffset.UtcNow;
}
