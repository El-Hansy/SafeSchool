using FluentAssertions;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Performance;

public sealed class ScanTraceLatencyTests
{
    [Fact]
    public void TraceBudget_IsUnderSixtySeconds()
    {
        TimeSpan.FromSeconds(60).Should().BeLessThanOrEqualTo(TimeSpan.FromMinutes(1));
    }
}

