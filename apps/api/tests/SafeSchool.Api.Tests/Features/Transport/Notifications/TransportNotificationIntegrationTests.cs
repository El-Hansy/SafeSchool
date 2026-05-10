using FluentAssertions;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Features.Transport.Notifications;
using SafeSchool.Api.Tests.Features.Transport.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Notifications;

public sealed class TransportNotificationIntegrationTests
{
    [Fact]
    public async Task GuardianNotificationVisibility_is_tenant_scoped_and_guardian_link_enforced()
    {
        var fixture = new TransportTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        dbContext.TransportNotificationRecords.AddRange(
            new TransportNotificationRecord
            {
                TenantId = "school-live",
                GuardianRecordId = "guardian-live",
                StudentProfileId = "student-amina",
                NotificationStatus = NotificationStatus.Visible,
                VisibleStatus = "Amina boarded BUS-12"
            },
            new TransportNotificationRecord
            {
                TenantId = "school-other",
                GuardianRecordId = "guardian-live",
                StudentProfileId = "student-amina",
                NotificationStatus = NotificationStatus.Visible,
                VisibleStatus = "Wrong tenant notification"
            },
            new TransportNotificationRecord
            {
                TenantId = "school-live",
                GuardianRecordId = "guardian-live",
                StudentProfileId = "student-amina",
                NotificationStatus = NotificationStatus.Suppressed,
                VisibleStatus = "Suppressed detail"
            });
        await dbContext.SaveChangesAsync();
        var guardianLinks = new FakeTransportGuardianLinkProvider();
        guardianLinks.Add(new TransportGuardianLinkEvidence("school-live", "guardian-live", "student-amina", GuardianLinkStatus.Approved, true));
        guardianLinks.Add(new TransportGuardianLinkEvidence("school-live", "guardian-live", "student-denied", GuardianLinkStatus.Suspended, true));
        var service = new GuardianTransportNotificationVisibilityService(dbContext, guardianLinks);

        var result = await service.ListAsync("school-live", "guardian-live", "student-amina");
        var denied = await service.ListAsync("school-live", "guardian-live", "student-denied");

        result.Succeeded.Should().BeTrue();
        result.Value!.Notifications.Should().ContainSingle();
        result.Value.Notifications[0].SchoolAccountId.Should().Be("school-live");
        result.Value.Notifications[0].VisibleStatus.Should().Be("Amina boarded BUS-12");
        denied.Succeeded.Should().BeFalse();
        denied.Errors.Should().ContainSingle(error => error.Code == "guardian_scope_denied");
    }
}
