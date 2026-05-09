using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class RewardRedemption : TenantOwnedEntity
{
    public Guid RewardCatalogItemId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public int StarCostSnapshot { get; set; }
    public RedemptionStatus RedemptionStatus { get; set; } = RedemptionStatus.Requested;
    public FulfillmentStatus FulfillmentStatus { get; set; } = FulfillmentStatus.Pending;
    public string LedgerReference { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string ReviewReason { get; set; } = string.Empty;
}
