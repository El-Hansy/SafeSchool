using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class ProfileResolverTests
{
    [Fact]
    public void Profile_resolves_guardian_links_and_arabic_direction()
    {
        var profile = new MobileProfileService(new MobileLanguageService(), new MobilePermissionResolver(new MobileFeatureAvailabilityService()))
            .GetProfile("school-demo", MobileRoleCodes.Guardian, "guardian-demo", "ar");
        profile.LinkedStudents.Should().Contain("student-amina");
        profile.TextDirection.Should().Be("rtl");
    }
}
