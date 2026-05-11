using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Medical;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Medical;

public sealed class MedicalFoundationTests
{
    [Fact]
    public async Task Medical_records_cover_guardian_updates_emergency_access_and_incidents_with_audit()
    {
        await using var dbContext = CreateDbContext();
        var service = new MedicalWorkflowService(dbContext);
        var profile = new MedicalRecordRequest("student-1", "Asthma care plan current", "Inhaler in clinic cabinet", "med-1", "Routine");

        MedicalCapabilities.All.Should().Contain(MedicalCapabilities.EmergencyAccess);
        new MedicalBoundaryGuard().Allows("diagnosis").Should().BeFalse();
        new MedicalBoundaryGuard().Allows("medical_event").Should().BeTrue();

        var update = await service.GuardianUpdateAsync("school-demo", profile);
        update.Status.Should().Be("PendingMedicalReview");
        update.AuditTrail.Should().Contain(["guardian-update-submitted", "review-required", "tenant-checked", "feature-checked"]);

        var duplicate = await service.GuardianUpdateAsync("school-demo", profile);
        duplicate.MedicalRecordId.Should().Be(update.MedicalRecordId);
        duplicate.AuditTrail.Should().Contain("idempotency_duplicate");

        var conflict = await service.GuardianUpdateAsync("school-demo", profile with { RestrictedDetail = "Changed detail" });
        conflict.MedicalRecordId.Should().Be(update.MedicalRecordId);
        conflict.Status.Should().Be("ManualReviewRequired");

        var emergency = await service.OpenEmergencyAccessAsync("school-demo", new EmergencyAccessRequest("student-1", "Bus arrival emergency", "nurse-1", "emg-1"), true);
        emergency.RecordType.Should().Be("emergency-access");
        emergency.Status.Should().Be("Open");
        emergency.ExpiresAt.Should().NotBeNull();
        emergency.AuditTrail.Should().Contain(["break-glass-confirmed", "minimum-necessary-data-opened", "mandatory-review-created"]);

        var incident = await service.LogIncidentAsync("school-demo", new MedicalIncidentRequest("student-1", "Moderate", "Student felt dizzy", "Guardian called", "inc-1"));
        incident.RecordType.Should().Be("incident");
        incident.AuditTrail.Should().Contain(["incident-logged", "care-action-recorded", "no-diagnosis-created"]);

        var guardianVisible = await service.AudienceSummaryAsync("school-demo", "guardian");
        guardianVisible.Should().Contain(x => x.RecordType == "profile");
        guardianVisible.Should().OnlyContain(x => x.AuditTrail.Contains("minimum-necessary-view"));
        dbContext.OperationalMedicalEvents.Should().Contain(x => x.EventType == "break-glass-confirmed");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
