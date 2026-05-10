using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Complaints;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintTenantScopeTests
{
    [Fact]
    public async Task Complaint_idempotency_and_visibility_are_scoped_per_school()
    {
        await using var dbContext = CreateDbContext();
        var service = new ComplaintWorkflowService(dbContext);
        var request = new ComplaintSubmissionRequest("student-1", "transport", "Bus was late", "Review", "request-1");

        var schoolA = await service.SubmitAsync("school-a", request);
        var schoolB = await service.SubmitAsync("school-b", request with { Description = "Different tenant same request id" });
        var conflict = await service.SubmitAsync("school-a", request with { Description = "Changed inside same tenant" });

        schoolA.Status.Should().Be("Received");
        schoolB.Status.Should().Be("Received");
        schoolB.ComplaintId.Should().NotBe(schoolA.ComplaintId);
        conflict.ComplaintId.Should().Be(schoolA.ComplaintId);
        conflict.Status.Should().Be("ManualReviewRequired");

        var schoolASummary = await service.TriageAsync("school-a");
        var schoolBSummary = await service.TriageAsync("school-b");
        schoolASummary.Should().ContainSingle(x => x.ComplaintId == schoolA.ComplaintId);
        schoolBSummary.Should().ContainSingle(x => x.ComplaintId == schoolB.ComplaintId);
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
