namespace SafeSchool.Api.Features.Mobile;

public sealed class SchoolAdministratorMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.SchoolAdministrator, "School mobile permissions and dashboard", ["assign_role", "review_release", "view_metrics"], false);
}
