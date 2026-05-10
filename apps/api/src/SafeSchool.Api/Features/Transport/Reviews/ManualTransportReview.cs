using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Reviews;

public sealed class ManualTransportReview : TenantOwnedEntity
{
    public string SourceRecordType { get; set; } = string.Empty;
    public string SourceRecordReference { get; set; } = string.Empty;
    public ManualReviewAction ReviewAction { get; set; } = ManualReviewAction.Correct;
    public string CorrectedStatus { get; set; } = string.Empty;
    public string ReviewReason { get; set; } = string.Empty;
    public string ReviewedBy { get; set; } = string.Empty;
    public DateTimeOffset ReviewedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed record TransportReviewSummary(string SummaryScope, string ScopeReference, string SchoolAccountId, int AcceptedScans, int NeedsReviewScans, int CurrentLocations, int StaleLocations, int VisibleNotifications, int OpenAnomalies, DateTimeOffset LatestEvidenceAt);
