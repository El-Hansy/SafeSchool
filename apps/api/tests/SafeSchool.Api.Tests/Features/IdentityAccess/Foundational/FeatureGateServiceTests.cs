using FluentAssertions;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Foundational;

public sealed class FeatureGateServiceTests
{
    private readonly IdentityAccessTestFixture _fixture = new();

    [Fact]
    public void IsEnabled_UsesDefaultAvailability()
    {
        var service = new FeatureGateService(_fixture.EnabledConfiguration());

        service.IsEnabled("school-1", IdentityAccessCapabilities.StudentProfiles).Should().BeTrue();
    }

    [Fact]
    public void Explain_ReturnsDisabledCapabilityReason()
    {
        var service = new FeatureGateService(_fixture.DisabledConfiguration());

        service.Explain("school-1", IdentityAccessCapabilities.GuardianLinking)
            .Should().Contain(IdentityAccessCapabilities.GuardianLinking);
    }
}
