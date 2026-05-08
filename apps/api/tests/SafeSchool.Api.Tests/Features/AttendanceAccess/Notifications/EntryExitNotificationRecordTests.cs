using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationRecordTests
{
    [Fact]
    public void CanTransitionTo_AllowsEligibleVisibleTransition()
    {
        var record = new EntryExitNotificationRecord { EligibilityStatus = NotificationEligibilityStatus.Eligible };
        record.CanTransitionTo(NotificationEligibilityStatus.Visible).Should().BeTrue();
    }
}

