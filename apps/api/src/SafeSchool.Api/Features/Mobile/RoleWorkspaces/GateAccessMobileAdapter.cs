namespace SafeSchool.Api.Features.Mobile;

public sealed class GateAccessMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.GateAccess, "Campus entry/exit scan capture", ["scan_nfc", "scan_qr", "review_denial"], true);
}
