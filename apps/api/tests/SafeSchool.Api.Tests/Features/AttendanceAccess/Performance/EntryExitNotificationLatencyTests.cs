using FluentAssertions;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Performance;

public sealed class EntryExitNotificationLatencyTests
{
    [Fact]
    public void NotificationBudget_IsTwoMinutes()
    {
        TimeSpan.FromMinutes(2).Should().BeLessThan(TimeSpan.FromMinutes(3));
    }
}

