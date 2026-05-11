using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Requests;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Requests;

public sealed class RequestFoundationTests
{
    [Fact]
    public async Task Requests_are_idempotent_auditable_and_approvable_without_source_mutation()
    {
        await using var dbContext = CreateDbContext();
        var service = new RequestWorkflowService(dbContext);
        var releaseAt = DateTimeOffset.UtcNow.AddHours(2);
        var request = new RequestSubmissionRequest("student-1", "early_leave", "Medical appointment", "Release to guardian", "req-1", releaseAt, releaseAt.AddMinutes(30));

        RequestCapabilities.All.Should().Contain(RequestCapabilities.Approval);
        new RequestBoundaryGuard().Allows("attendance").Should().BeFalse();
        new RequestBoundaryGuard().Allows("request_status_event").Should().BeTrue();

        var submitted = await service.SubmitAsync("school-demo", request);
        submitted.Status.Should().Be("PendingApproval");
        submitted.Priority.Should().Be("High");
        submitted.AuditTrail.Should().Contain(["submitted", "approval-routing", "tenant-checked", "feature-checked"]);

        var duplicate = await service.SubmitAsync("school-demo", request);
        duplicate.RequestId.Should().Be(submitted.RequestId);
        duplicate.AuditTrail.Should().Contain("idempotency_duplicate");

        var exactDuplicate = await service.SubmitAsync("school-demo", request with { ClientRequestId = "req-2" });
        exactDuplicate.Status.Should().Be("DuplicateBlocked");
        exactDuplicate.RequestId.Should().Be(submitted.RequestId);
        exactDuplicate.AuditTrail.Should().Contain("duplicate_blocked");

        var overlapping = await service.SubmitAsync("school-demo", request with
        {
            ClientRequestId = "req-3",
            Reason = "Different appointment",
            StartsAt = releaseAt.AddMinutes(10),
            EndsAt = releaseAt.AddMinutes(40)
        });
        overlapping.Status.Should().Be("NeedsReview");
        overlapping.AuditTrail.Should().Contain("overlap_manual_review");

        var conflict = await service.SubmitAsync("school-demo", request with { Reason = "Changed reason" });
        conflict.RequestId.Should().Be(submitted.RequestId);
        conflict.Status.Should().Be("NeedsReview");
        conflict.AuditTrail.Should().Contain("idempotency_conflict");

        var approved = await service.ApproveAsync("school-demo", submitted.TrackingReference, new RequestActionRequest("approve", "Validated guardian pickup", "approver-1", "approve-1"));
        approved.Status.Should().Be("Approved");
        approved.AuditTrail.Should().Contain("approved");

        var finalStateRejected = await service.RejectAsync("school-demo", submitted.TrackingReference, new RequestActionRequest("reject", "Second decision rejected", "approver-2", "reject-1"));
        finalStateRejected.Status.Should().Be("FinalStateRejected");
        finalStateRejected.AuditTrail.Should().Contain("final_state_decision_rejected");

        var eligibility = await service.ReleaseEligibilityAsync("school-demo", submitted.TrackingReference);
        eligibility.Should().BeEquivalentTo(new { status = "Eligible", eligible = true }, options => options.ExcludingMissingMembers());

        var statusEvents = await service.StatusEventsAsync("school-demo");
        statusEvents.Should().Contain(x => x.RequestId.ToString("N") == submitted.RequestId && x.SourceEventType == "approved" && x.NotificationEligible);
        statusEvents.Should().Contain(x => x.SourceEventType == "overlap_manual_review" && x.ReviewRequired);

        var summaries = await service.ReviewSummariesAsync("school-demo");
        summaries.Should().Contain(x => x.TrackingReference == submitted.TrackingReference && x.Status == "Approved" && x.LastEventType == "final_state_decision_rejected");

        var missingRequiredFields = await service.SubmitAsync("school-demo", request with { StudentProfileId = "", ClientRequestId = "req-4" });
        missingRequiredFields.Status.Should().Be("ValidationFailed");
        missingRequiredFields.AuditTrail.Should().Contain("validation_failed");

        var guardianVisible = await service.AudienceSummaryAsync("school-demo", "guardian");
        guardianVisible.Should().ContainSingle(x => x.RequestId == submitted.RequestId);
        dbContext.OperationalRequestEvents.Should().Contain(x => x.EventType == "approval-routing");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
