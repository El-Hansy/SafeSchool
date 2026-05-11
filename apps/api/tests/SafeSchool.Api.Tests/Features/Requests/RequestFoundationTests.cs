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
        var request = new RequestSubmissionRequest("student-1", "early_leave", "Medical appointment", "Release to guardian", "req-1");

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

        var conflict = await service.SubmitAsync("school-demo", request with { Reason = "Changed reason" });
        conflict.RequestId.Should().Be(submitted.RequestId);
        conflict.Status.Should().Be("NeedsReview");
        conflict.AuditTrail.Should().Contain("idempotency_conflict");

        var approved = await service.ApproveAsync("school-demo", submitted.TrackingReference, new RequestActionRequest("approve", "Validated guardian pickup", "approver-1", "approve-1"));
        approved.Status.Should().Be("Approved");
        approved.AuditTrail.Should().Contain("approved");

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
