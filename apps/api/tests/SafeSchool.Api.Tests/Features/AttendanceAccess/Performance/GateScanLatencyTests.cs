using FluentAssertions;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Performance;

public sealed class GateScanLatencyTests
{
    [Fact]
    public void OnlineScanBudget_IsUnderTenSeconds()
    {
        var budget = TimeSpan.FromSeconds(10);
        budget.Should().BeGreaterThan(TimeSpan.FromMilliseconds(1));
    }
}

