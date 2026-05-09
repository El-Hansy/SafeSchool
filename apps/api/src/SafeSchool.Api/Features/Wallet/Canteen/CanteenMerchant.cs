using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Canteen;

public sealed class CanteenMerchant : TenantOwnedEntity
{
    public string MerchantName { get; set; } = string.Empty;
    public string MerchantCode { get; set; } = string.Empty;
    public MerchantStatus MerchantStatus { get; set; } = MerchantStatus.Draft;
    public string AllowedCategoryCodes { get; set; } = string.Empty;
    public string SettlementReferencePolicy { get; set; } = "Daily settlement evidence required";
    public string CreatedBy { get; set; } = "system";
    public string UpdatedBy { get; set; } = "system";
}

public sealed class CanteenItemCategory : TenantOwnedEntity
{
    public string ItemCategoryCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public CanteenItemCategoryStatus Status { get; set; } = CanteenItemCategoryStatus.Draft;
}

public sealed class PurchaseEligibilityRule : TenantOwnedEntity
{
    public Guid CanteenMerchantId { get; set; }
    public string ItemCategoryCode { get; set; } = string.Empty;
    public PurchaseEligibilityRuleStatus Status { get; set; } = PurchaseEligibilityRuleStatus.Draft;
    public string DenialReason { get; set; } = string.Empty;
}
