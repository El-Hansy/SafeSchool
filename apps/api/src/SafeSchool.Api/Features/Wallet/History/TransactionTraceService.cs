namespace SafeSchool.Api.Features.Wallet.History;

public sealed class TransactionTraceService
{
    public TransactionTraceResponse Trace(string transactionId) => new(transactionId, [$"transaction:{transactionId}", "wallet:balance", "audit:evidence", "review:scope"]);
}
