using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationEligibilityTests
{
    [Fact]
    public async Task EvaluateAsync_SuppressesDeniedScansAndIneligibleLinks()
    {
        var provider = new FakeGuardianLinkEligibilityProvider { IsEligible = false, Reason = "suspended" };
        var service = new EntryExitNotificationEligibilityService(provider);
        var scan = new GateScanEvent { TenantId = "school-1", StudentProfileId = "student-1", Status = ScanEventStatus.Accepted };

        var result = await service.EvaluateAsync("school-1", "guardian-1", scan);

        result.Status.Should().Be(NotificationEligibilityStatus.Suppressed);
        result.Reason.Should().Be("suspended");
    }
}

