using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationIntegrationTests
{
    [Fact]
    public async Task CreateFromScanAsync_CreatesGuardianVisibleRecordForEligibleLink()
    {
        var fixture = new AttendanceAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var scan = new GateScanEvent { TenantId = "school-1", StudentProfileId = "student-1", Status = ScanEventStatus.Accepted };
        dbContext.GateScanEvents.Add(scan);
        await dbContext.SaveChangesAsync();
        var service = new EntryExitNotificationService(dbContext, new EntryExitNotificationEligibilityService(new FakeGuardianLinkEligibilityProvider()));

        var record = await service.CreateFromScanAsync("school-1", "guardian-1", scan);

        record.GuardianVisibleAt.Should().NotBeNull();
    }
}
