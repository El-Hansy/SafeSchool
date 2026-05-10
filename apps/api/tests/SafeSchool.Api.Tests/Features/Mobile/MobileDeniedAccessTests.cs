using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileDeniedAccessTests
{
    [Fact]
    public void Denied_access_does_not_leak_restricted_details()
    {
        var resolver = new MobilePermissionResolver(new MobileFeatureAvailabilityService());
        resolver.DeniedReason("blocked-tenant", MobileRoleCodes.Guardian, MobileRoleCodes.Guardian).Should().Be("TENANT_ACCESS_DENIED");
    }
}
