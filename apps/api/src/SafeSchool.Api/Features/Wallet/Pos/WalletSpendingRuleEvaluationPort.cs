using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed record WalletPurchaseRuleDecision(bool Allowed, string Reason, string RuleSnapshotReference);

public interface IWalletSpendingRuleEvaluationPort
{
    Task<WalletPurchaseRuleDecision> EvaluateAsync(string tenantId, Guid walletId, string itemCategoryCode, Guid merchantId, long amountMinor, CancellationToken cancellationToken = default);
}

public sealed class WalletSpendingRuleEvaluationPort : IWalletSpendingRuleEvaluationPort
{
    public Task<WalletPurchaseRuleDecision> EvaluateAsync(string tenantId, Guid walletId, string itemCategoryCode, Guid merchantId, long amountMinor, CancellationToken cancellationToken = default)
    {
        var allowed = amountMinor > 0 && !string.Equals(itemCategoryCode, "blocked", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(new WalletPurchaseRuleDecision(allowed, allowed ? "allowed" : "blocked category", $"baseline:{tenantId}:{merchantId}:{itemCategoryCode}"));
    }
}
