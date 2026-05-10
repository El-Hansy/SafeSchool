namespace SafeSchool.Api.Features.Mobile;

public sealed class StaffWorkspaceService
{
    public IReadOnlyList<StaffMobileWorkspace> AllStaffWorkspaces() =>
        MobileRoleCodes.All
            .Except([MobileRoleCodes.Guardian, MobileRoleCodes.Student])
            .Select(role => new StaffMobileWorkspace(role, MobileSeedCatalog.DisplayName(role), MobileSeedCatalog.ActionsFor(role).Select(action => action.ActionCode).ToArray(), MobileSeedCatalog.ActionsFor(role).Any(action => action.OfflinePolicy != MobileOfflinePolicy.NotSupported)))
            .ToArray();
}
