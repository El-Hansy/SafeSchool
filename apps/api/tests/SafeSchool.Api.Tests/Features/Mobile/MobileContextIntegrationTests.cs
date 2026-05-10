using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileContextIntegrationTests
{
    [Fact]
    public void Workspace_resolver_returns_only_matching_role_for_standard_user()
    {
        var resolver = new RoleWorkspaceResolver(new MobileFeatureAvailabilityService(), new MobilePermissionResolver(new MobileFeatureAvailabilityService()));
        var workspaces = resolver.ResolveVisible("school-demo", MobileRoleCodes.CanteenCashier, "en");
        workspaces.Should().ContainSingle(x => x.WorkspaceCode == MobileRoleCodes.CanteenCashier);
    }
}
