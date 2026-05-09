using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class RewardCatalogItem : TenantOwnedEntity
{
    public string RewardCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int StarCost { get; set; }
    public string EligibilityScope { get; set; } = "School";
    public int? InventoryLimit { get; set; }
    public int? RedemptionLimit { get; set; }
    public RewardStatus RewardStatus { get; set; } = RewardStatus.Draft;
    public DateTimeOffset? AvailableFrom { get; set; }
    public DateTimeOffset? AvailableUntil { get; set; }
}
