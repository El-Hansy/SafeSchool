namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class WalletReconciliationTraceService
{
    public ReconciliationTraceResponse Trace(Guid reconciliationRunId) => new(reconciliationRunId, [$"reconciliation:{reconciliationRunId}", "ledger:totals", "payments:confirmations", "pos:batches", "audit:evidence"]);
}
