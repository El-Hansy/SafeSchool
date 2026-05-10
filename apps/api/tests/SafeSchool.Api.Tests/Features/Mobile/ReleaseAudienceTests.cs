using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class ReleaseAudienceTests
{
    [Fact]
    public void Audience_matching_is_tenant_and_role_scoped()
    {
        var service = new ReleaseAudienceService();
        service.IsInAudience("school-demo", MobileRoleCodes.Guardian).Should().BeTrue();
        service.IsInAudience("other-tenant", MobileRoleCodes.Guardian).Should().BeFalse();
    }
}
