using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobilePermissionResolverTests
{
    [Fact]
    public void Resolver_blocks_wrong_role_workspace_and_disabled_tenant()
    {
        var resolver = new MobilePermissionResolver(new MobileFeatureAvailabilityService());
        resolver.CanUseWorkspace("school-demo", MobileRoleCodes.Guardian, MobileRoleCodes.Guardian).Should().BeTrue();
        resolver.CanUseWorkspace("school-demo", MobileRoleCodes.Guardian, MobileRoleCodes.Student).Should().BeFalse();
        resolver.DeniedReason("mobile-disabled", MobileRoleCodes.Guardian, MobileRoleCodes.Guardian).Should().Be("MOBILE_FEATURE_DISABLED");
    }
}
