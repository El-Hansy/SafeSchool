using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Limits;

public sealed record SpendingLimitRequest(Guid WalletId, SpendingLimitOwnerType OwnerType, SpendingLimitType LimitType, long AmountMinor = 0, string CurrencyCode = "SAR", string MerchantCode = "", string ItemCategoryCode = "", string ActorReference = "finance-admin", string ClientRequestId = "");
public sealed record SpendingLimitResponse(Guid SpendingLimitId, Guid WalletId, SpendingLimitOwnerType OwnerType, SpendingLimitType LimitType, long AmountMinor, string CurrencyCode, string MerchantCode, string ItemCategoryCode, SpendingLimitStatus Status, string RuleVersion);
public sealed record EffectiveLimitResponse(Guid WalletId, bool Allowed, string StrictestRule, string RuleSnapshotReference);
public sealed record LimitTraceResponse(Guid SpendingLimitId, IReadOnlyList<string> TraceReferences);

public static class SpendingLimitMappingExtensions
{
    public static SpendingLimitResponse ToResponse(this SpendingLimit limit) => new(limit.Id, limit.StudentWalletId, limit.OwnerType, limit.LimitType, limit.AmountMinor, limit.CurrencyCode, limit.MerchantCode, limit.ItemCategoryCode, limit.Status, limit.RuleVersion);
}
