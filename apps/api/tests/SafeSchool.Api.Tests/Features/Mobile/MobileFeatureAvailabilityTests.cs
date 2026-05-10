using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileFeatureAvailabilityTests
{
    [Fact]
    public void Feature_availability_inherits_workspace_and_source_features()
    {
        var service = new MobileFeatureAvailabilityService();
        service.IsWorkspaceEnabled("school-demo", MobileRoleCodes.TransportDriver).Should().BeTrue();
        service.EnabledFeatures(MobileRoleCodes.Guardian).Should().Contain(["attendance", "transport", "wallet"]);
    }
}
