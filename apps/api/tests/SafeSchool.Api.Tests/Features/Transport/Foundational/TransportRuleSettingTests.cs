using FluentAssertions;
using SafeSchool.Api.Features.Transport.Rules;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Foundational;

public sealed class TransportRuleSettingTests
{
    [Fact]
    public void CanActivate_RequiresThirtyDayLocationRetentionAndPositiveWindows()
    {
        var setting = new TransportRuleSetting();
        setting.CanActivate().Should().BeTrue();
        setting.LocationDetailRetentionDays = 31;
        setting.CanActivate().Should().BeFalse();
    }
}
