using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceGenerationIntegrationTests
{
    [Fact]
    public async Task SummaryAsync_ReturnsTenantScopedCounts()
    {
        var fixture = new AttendanceAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var sessionId = Guid.NewGuid();
        dbContext.AttendanceRecords.AddRange(
            new AttendanceRecord { TenantId = "school-1", AttendanceSessionId = sessionId, StudentProfileId = "s1", Status = AttendanceStatus.Present },
            new AttendanceRecord { TenantId = "school-1", AttendanceSessionId = sessionId, StudentProfileId = "s2", Status = AttendanceStatus.Absent });
        await dbContext.SaveChangesAsync();

        var summary = await new AttendanceSummaryService(dbContext).SummaryAsync("school-1", sessionId);

        summary.Present.Should().Be(1);
        summary.Absent.Should().Be(1);
    }
}

