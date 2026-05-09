using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Limits;

public sealed class SpendingLimit : TenantOwnedEntity
{
    public Guid StudentWalletId { get; set; }
    public SpendingLimitOwnerType OwnerType { get; set; } = SpendingLimitOwnerType.School;
    public string GuardianLinkId { get; set; } = string.Empty;
    public SpendingLimitType LimitType { get; set; } = SpendingLimitType.DailyAmount;
    public long AmountMinor { get; set; }
    public string CurrencyCode { get; set; } = "SAR";
    public string MerchantCode { get; set; } = string.Empty;
    public string ItemCategoryCode { get; set; } = string.Empty;
    public TimeOnly? WindowStart { get; set; }
    public TimeOnly? WindowEnd { get; set; }
    public DateOnly? ActiveFrom { get; set; }
    public DateOnly? ActiveUntil { get; set; }
    public int PrecedenceRank { get; set; } = 100;
    public SpendingLimitStatus Status { get; set; } = SpendingLimitStatus.Draft;
    public string RuleVersion { get; set; } = "v1";
    public string CreatedBy { get; set; } = "system";
}
