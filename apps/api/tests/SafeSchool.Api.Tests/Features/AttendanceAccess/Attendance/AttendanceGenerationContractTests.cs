using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceGenerationContractTests
{
    [Fact]
    public void Dtos_ExposeSessionAndCorrectionContractFields()
    {
        var request = new CorrectAttendanceRequest(AttendanceStatus.Excused, "approved correction");
        request.Reason.Should().Be("approved correction");
        typeof(AttendanceSummaryResponse).GetProperties().Select(x => x.Name).Should().Contain(["Present", "Late", "Absent"]);
    }
}
