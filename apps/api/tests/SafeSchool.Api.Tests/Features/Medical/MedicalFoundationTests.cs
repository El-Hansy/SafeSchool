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

        var verifiedProfile = await service.UpsertProfileAsync("school-demo", profile with { ClientRequestId = "med-verified-1", Summary = "Asthma care plan verified by nurse" });
        verifiedProfile.MedicalRecordId.Should().Be(update.MedicalRecordId);
        verifiedProfile.Status.Should().Be("Verified");
        verifiedProfile.AuditTrail.Should().Contain("profile-upserted");

        var invalidProfile = await service.UpsertProfileAsync("school-demo", profile with { StudentProfileId = "", ClientRequestId = "med-invalid-1" });
        invalidProfile.Status.Should().Be("ValidationFailed");

        var missingReason = await service.OpenEmergencyAccessAsync("school-demo", new EmergencyAccessRequest("student-1", "", "nurse-1", "emg-invalid"), false);
        missingReason.Status.Should().Be("ValidationFailed");

        var deniedBreakGlass = await service.OpenEmergencyAccessAsync("school-demo", new EmergencyAccessRequest("student-1", "Bus arrival emergency", "teacher-1", "emg-denied", "teacher"), true);
        deniedBreakGlass.Status.Should().Be("AccessDenied");

        var emergency = await service.OpenEmergencyAccessAsync("school-demo", new EmergencyAccessRequest("student-1", "Bus arrival emergency", "nurse-1", "emg-1"), true);
        emergency.RecordType.Should().Be("emergency-access");
        emergency.Status.Should().Be("Open");
        emergency.ExpiresAt.Should().NotBeNull();
        emergency.AuditTrail.Should().Contain(["break-glass-confirmed", "minimum-necessary-data-opened", "mandatory-review-created"]);

        var incident = await service.LogIncidentAsync("school-demo", new MedicalIncidentRequest("student-1", "Moderate", "Student felt dizzy", "Guardian called", "inc-1"));
        incident.RecordType.Should().Be("incident");
        incident.AuditTrail.Should().Contain(["incident-logged", "care-action-recorded", "no-diagnosis-created"]);

        var highIncident = await service.LogIncidentAsync("school-demo", new MedicalIncidentRequest("student-1", "High", "Severe allergic reaction", "Emergency contact called", "inc-2"));
        highIncident.AuditTrail.Should().Contain("notification-eligibility-exported");

        var reconfirmed = await service.ReconfirmEmergencyAccessAsync("school-demo", emergency.RecordReference, new MedicalActionRequest("reconfirm", "Emergency still active", "nurse-1", "emg-reconfirm-1"));
        reconfirmed.Status.Should().Be("Open");
        reconfirmed.AuditTrail.Should().Contain("emergency-access-reconfirmed");

        var closed = await service.CloseEmergencyAccessAsync("school-demo", emergency.RecordReference, new MedicalActionRequest("close", "Student stabilized", "nurse-1", "emg-close-1"));
        closed.Status.Should().Be("Closed");
        closed.AuditTrail.Should().Contain("emergency-access-closed");

        var defaultAudience = await service.CreateNotificationAsync("school-demo", new MedicalNotificationRequest("student-1", highIncident.RecordReference, "High", "", "notif-1"));
        defaultAudience.RecordType.Should().Be("notification");
        defaultAudience.VisibleSummary.Should().Contain("approved guardians");

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
