using FluentAssertions;
using SafeSchool.Api.Features.Complaints;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Complaints;

public sealed class ComplaintTenantScopeTests
{
    [Fact]
    public void Complaint_idempotency_is_scoped_per_school()
    {
        var service = new ComplaintWorkflowService(new ComplaintIdempotencyService());
        var request = new ComplaintSubmissionRequest("student-1", "transport", "Bus was late", "Review", "request-1");

        service.Submit("school-a", request).Status.Should().Be("Received");
        service.Submit("school-b", request with { Description = "Different tenant same request id" }).Status.Should().Be("Received");
        service.Submit("school-a", request with { Description = "Changed inside same tenant" }).Status.Should().Be("ManualReviewRequired");
    }
}
