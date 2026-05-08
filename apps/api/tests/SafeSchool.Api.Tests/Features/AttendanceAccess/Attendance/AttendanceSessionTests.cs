using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceSessionTests
{
    [Fact]
    public void Session_ValidatesWindowsAndLifecycle()
    {
        var session = new AttendanceSession { GenerationStatus = AttendanceSessionStatus.Draft };
        session.IsValidWindow().Should().BeTrue();
        session.CanTransitionTo(AttendanceSessionStatus.Active).Should().BeTrue();
    }
}

