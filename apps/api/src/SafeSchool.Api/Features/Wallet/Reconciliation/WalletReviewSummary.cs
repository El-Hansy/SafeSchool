using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class WalletReviewSummary : TenantOwnedEntity
{
    public string SummaryScope { get; set; } = string.Empty;
    public string ScopeReference { get; set; } = string.Empty;
    public int WalletCount { get; set; }
    public int TransactionCount { get; set; }
    public int SpendingLimitCount { get; set; }
    public int SettlementCount { get; set; }
    public int AnomalyCount { get; set; }
    public int ReviewCount { get; set; }
    public DateTimeOffset LatestEvidenceTime { get; set; } = DateTimeOffset.UtcNow;
    public string TraceReferences { get; set; } = string.Empty;
    public ReviewSummaryStatus Status { get; set; } = ReviewSummaryStatus.Current;
}
