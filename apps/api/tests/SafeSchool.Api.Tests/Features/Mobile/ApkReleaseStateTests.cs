using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class ApkReleaseStateTests
{
    [Fact]
    public void Current_release_blocks_obsolete_versions()
    {
        var service = new ApkReleaseService(new ApkReleaseRepository(), new MobileFeatureAvailabilityService(), new MobileAuditService());
        service.Current("school-demo", MobileRoleCodes.Guardian, 1).UpdateRequired.Should().BeTrue();
        service.Current("school-demo", MobileRoleCodes.Guardian, 1200).DownloadAllowed.Should().BeTrue();
    }
}
