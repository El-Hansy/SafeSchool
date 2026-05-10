using FluentAssertions;
using SafeSchool.Api.Features.Complaints;
using Xunit;
namespace SafeSchool.Api.Tests.Features.Complaints;
public sealed class ComplaintFoundationTests { [Fact] public void Complaints_are_idempotent_auditable_and_boundary_safe() { ComplaintCapabilities.All.Should().Contain(ComplaintCapabilities.Submission); var idem = new ComplaintIdempotencyService(); idem.Record("submit", "1", "a").Should().Be(ComplaintIdempotencyOutcome.Accepted); idem.Record("submit", "1", "a").Should().Be(ComplaintIdempotencyOutcome.Duplicate); idem.Record("submit", "1", "b").Should().Be(ComplaintIdempotencyOutcome.Conflict); new ComplaintBoundaryGuard().Allows("wallet").Should().BeFalse(); new ComplaintWorkflowService(idem).Submit("school-demo", new ComplaintSubmissionRequest("student-1", "safety", "Concern", "Review", "req-2")).Status.Should().Be("Received"); } }
