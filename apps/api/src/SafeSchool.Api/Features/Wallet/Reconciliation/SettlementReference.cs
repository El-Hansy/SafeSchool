using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class SettlementReference : TenantOwnedEntity
{
    public string SettlementScope { get; set; } = string.Empty;
    public long ExpectedTotalMinor { get; set; }
    public long LedgerTotalMinor { get; set; }
    public long DifferenceMinor { get; set; }
    public SettlementStatus Status { get; set; } = SettlementStatus.Open;
    public string ClosedBy { get; set; } = string.Empty;
    public DateTimeOffset? ClosedAt { get; set; }
    public string ReviewReason { get; set; } = string.Empty;
    public string SourceEvidenceLinks { get; set; } = string.Empty;
}

public sealed class WalletReconciliationRun : TenantOwnedEntity
{
    public string RunScope { get; set; } = "daily";
    public DateOnly DateFrom { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);
    public DateOnly DateUntil { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);
    public ReconciliationStatus Status { get; set; } = ReconciliationStatus.Draft;
    public long LedgerTotalMinor { get; set; }
    public long SourceTotalMinor { get; set; }
    public long DifferenceMinor { get; set; }
    public string CreatedBy { get; set; } = "finance";
    public string ReviewReason { get; set; } = string.Empty;
}

public sealed class WalletReconciliationMismatch : TenantOwnedEntity
{
    public Guid ReconciliationRunId { get; set; }
    public string MismatchType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public long DifferenceMinor { get; set; }
    public string ReviewState { get; set; } = "Open";
}
