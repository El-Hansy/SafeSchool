namespace SafeSchool.Api.Features.Wallet.Limits;

public sealed class SpendingLimitTraceService
{
    public LimitTraceResponse Trace(Guid spendingLimitId) => new(spendingLimitId, [$"limit:{spendingLimitId}", "purchases:rule-snapshot", "audit:limit-change"]);
}
