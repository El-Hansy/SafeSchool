using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Complaints;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintFoundationTests
{
    [Fact]
    public async Task Complaints_are_persisted_idempotent_auditable_and_boundary_safe()
    {
        await using var dbContext = CreateDbContext();
        var service = new ComplaintWorkflowService(dbContext);
        var request = new ComplaintSubmissionRequest("student-1", "safety", "Concern", "Review", "req-2");

        ComplaintCapabilities.All.Should().Contain(ComplaintCapabilities.Submission);
        new ComplaintBoundaryGuard().Allows("wallet").Should().BeFalse();

        var submitted = await service.SubmitAsync("school-demo", request);
        submitted.Status.Should().Be("Received");
        submitted.AuditTrail.Should().Contain(["submitted", "audit-written"]);

        var duplicate = await service.SubmitAsync("school-demo", request);
        duplicate.ComplaintId.Should().Be(submitted.ComplaintId);
        duplicate.AuditTrail.Should().Contain("idempotency_duplicate");

        var conflict = await service.SubmitAsync("school-demo", request with { Description = "Changed detail" });
        conflict.ComplaintId.Should().Be(submitted.ComplaintId);
        conflict.Status.Should().Be("ManualReviewRequired");

        dbContext.OperationalComplaints.Should().ContainSingle(x => x.TenantId == "school-demo");
        dbContext.OperationalComplaintEvents.Should().Contain(x => x.EventType == "idempotency_conflict");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
