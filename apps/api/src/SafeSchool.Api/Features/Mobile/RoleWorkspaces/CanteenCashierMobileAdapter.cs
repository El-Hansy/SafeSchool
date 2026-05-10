namespace SafeSchool.Api.Features.Mobile;

public sealed class CanteenCashierMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.CanteenCashier, "Wallet POS purchase decisions", ["scan_wallet", "charge_purchase", "view_decision"], true);
}
