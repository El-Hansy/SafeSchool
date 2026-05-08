using FluentAssertions;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Foundational;

public sealed class AttendanceAccessFeatureGateTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public void FeatureGate_UsesDefaultAvailability()
    {
        var gate = new FeatureGateService(_fixture.EnabledConfiguration());
        gate.IsEnabled("school-1", AttendanceAccessCapabilities.GateScanning).Should().BeTrue();
        gate.Explain("school-1", AttendanceAccessCapabilities.AnomalyDetection).Should().Contain("enabled");
    }
}

