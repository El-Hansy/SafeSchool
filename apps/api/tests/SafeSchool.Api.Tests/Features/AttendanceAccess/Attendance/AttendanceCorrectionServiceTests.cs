using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceCorrectionServiceTests
{
    [Fact]
    public async Task CorrectAsync_RequiresReasonAndPreservesRecord()
    {
        var fixture = new AttendanceAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var record = new AttendanceRecord { TenantId = "school-1", StudentProfileId = "student-active", Status = AttendanceStatus.Absent };
        dbContext.AttendanceRecords.Add(record);
        await dbContext.SaveChangesAsync();

        var result = await new AttendanceCorrectionService(dbContext).CorrectAsync("school-1", record.Id, new CorrectAttendanceRequest(AttendanceStatus.Excused, "guardian note"));

        result.Succeeded.Should().BeTrue();
        result.Value!.Status.Should().Be(AttendanceStatus.Excused);
    }
}

