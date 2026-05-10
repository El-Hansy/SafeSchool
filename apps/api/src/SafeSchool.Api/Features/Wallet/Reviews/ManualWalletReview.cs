using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Reviews;

public sealed class ManualWalletReview : TenantOwnedEntity
{
    public string ReviewScope { get; set; } = string.Empty;
    public string ScopeReference { get; set; } = string.Empty;
    public ManualWalletReviewAction ReviewAction { get; set; } = ManualWalletReviewAction.Assign;
    public ManualWalletReviewStatus ReviewStatus { get; set; } = ManualWalletReviewStatus.Requested;
    public string Reason { get; set; } = string.Empty;
    public string ReviewerActor { get; set; } = string.Empty;
    public Guid? ResultingLedgerEntryId { get; set; }
    public string ClientRequestId { get; set; } = string.Empty;
}
