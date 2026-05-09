using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Scans;

public sealed class GateScanFlowContractTests
{
    [Fact]
    public void RoutePrefix_MatchesGateScanContract()
    {
        AttendanceAccessEndpointRegistration.RoutePrefix.Should().Be("/api/v1/schools/{schoolAccountId}/attendance-access");
    }
}

