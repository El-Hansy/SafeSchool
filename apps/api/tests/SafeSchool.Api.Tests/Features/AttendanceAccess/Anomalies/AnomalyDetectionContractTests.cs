using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionContractTests
{
    [Fact]
    public void Dtos_ExposeReviewerWorkflowFields()
    {
        typeof(AnomalyResponse).GetProperties().Select(x => x.Name).Should().Contain(["Severity", "Status", "AssignedTo", "ResolutionReason"]);
    }
}

