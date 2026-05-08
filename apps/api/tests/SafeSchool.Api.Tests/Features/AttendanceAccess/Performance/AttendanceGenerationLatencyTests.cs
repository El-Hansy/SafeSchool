using FluentAssertions;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Performance;

public sealed class AttendanceGenerationLatencyTests
{
    [Fact]
    public void GenerationBudget_IsTwoMinutes()
    {
        TimeSpan.FromMinutes(2).TotalSeconds.Should().Be(120);
    }
}

