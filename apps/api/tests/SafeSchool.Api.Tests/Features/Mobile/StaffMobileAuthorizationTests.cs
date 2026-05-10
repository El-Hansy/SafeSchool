using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class StaffMobileAuthorizationTests
{
    [Fact]
    public void Staff_workspaces_cover_operational_roles()
    {
        var roles = new StaffWorkspaceService().AllStaffWorkspaces().Select(x => x.RoleCode);
        roles.Should().Contain([MobileRoleCodes.TransportDriver, MobileRoleCodes.GateAccess, MobileRoleCodes.CanteenCashier, MobileRoleCodes.MedicalStaff]);
    }
}
