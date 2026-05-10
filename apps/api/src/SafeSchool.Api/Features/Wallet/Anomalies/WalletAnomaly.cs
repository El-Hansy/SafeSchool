using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Anomalies;

public sealed class WalletAnomaly : TenantOwnedEntity
{
    public string RelatedSourceType { get; set; } = string.Empty;
    public string RelatedSourceReference { get; set; } = string.Empty;
    public WalletAnomalyType AnomalyType { get; set; } = WalletAnomalyType.ManualReviewRequired;
    public WalletAnomalySeverity Severity { get; set; } = WalletAnomalySeverity.Medium;
    public WalletAnomalyStatus Status { get; set; } = WalletAnomalyStatus.New;
    public string ReviewerAssignment { get; set; } = string.Empty;
    public string ResolutionReason { get; set; } = string.Empty;
    public string ResolutionHistory { get; set; } = string.Empty;
    public DateTimeOffset DetectedAt { get; set; } = DateTimeOffset.UtcNow;
}
