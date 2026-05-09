using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed record ReconciliationRunRequest(DateOnly DateFrom, DateOnly DateUntil, string Scope = "daily", string ActorReference = "finance");
public sealed record ReconciliationRunResponse(Guid ReconciliationRunId, string Scope, DateOnly DateFrom, DateOnly DateUntil, ReconciliationStatus Status, long LedgerTotalMinor, long SourceTotalMinor, long DifferenceMinor);
public sealed record ReconciliationMismatchResponse(Guid MismatchId, Guid ReconciliationRunId, string MismatchType, string SourceReference, long DifferenceMinor, string ReviewState);
public sealed record SettlementReferenceResponse(Guid SettlementReferenceId, string SettlementScope, long ExpectedTotalMinor, long LedgerTotalMinor, long DifferenceMinor, SettlementStatus Status, string ReviewReason);
public sealed record CloseReconciliationRequest(string Reason, string ActorReference = "finance-reviewer");
public sealed record ReopenReconciliationRequest(string Reason, string ActorReference = "finance-reviewer");
public sealed record ReviewSummaryResponse(Guid SummaryId, string SummaryScope, string ScopeReference, int WalletCount, int TransactionCount, int AnomalyCount, int ReviewCount, ReviewSummaryStatus Status, bool StaffOnlyDetailSuppressed);
public sealed record ReconciliationTraceResponse(Guid ReconciliationRunId, IReadOnlyList<string> TraceReferences);

public static class WalletReconciliationMappingExtensions
{
    public static ReconciliationRunResponse ToResponse(this WalletReconciliationRun run) => new(run.Id, run.RunScope, run.DateFrom, run.DateUntil, run.Status, run.LedgerTotalMinor, run.SourceTotalMinor, run.DifferenceMinor);
    public static ReconciliationMismatchResponse ToResponse(this WalletReconciliationMismatch mismatch) => new(mismatch.Id, mismatch.ReconciliationRunId, mismatch.MismatchType, mismatch.SourceReference, mismatch.DifferenceMinor, mismatch.ReviewState);
    public static SettlementReferenceResponse ToResponse(this SettlementReference settlement) => new(settlement.Id, settlement.SettlementScope, settlement.ExpectedTotalMinor, settlement.LedgerTotalMinor, settlement.DifferenceMinor, settlement.Status, settlement.ReviewReason);
    public static ReviewSummaryResponse ToResponse(this WalletReviewSummary summary, bool staffOnlySuppressed = false) => new(summary.Id, summary.SummaryScope, summary.ScopeReference, summary.WalletCount, summary.TransactionCount, summary.AnomalyCount, summary.ReviewCount, summary.Status, staffOnlySuppressed);
}
