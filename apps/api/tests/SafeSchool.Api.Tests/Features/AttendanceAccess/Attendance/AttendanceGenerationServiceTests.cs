using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceGenerationServiceTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public async Task GenerateAsync_UpsertsCurrentRecordWithoutDuplicates()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var session = new AttendanceSession { TenantId = "school-1", ExpectedPopulationRule = "student-active", GenerationStatus = AttendanceSessionStatus.Active };
        dbContext.AttendanceSessions.Add(session);
        dbContext.GateScanEvents.Add(new GateScanEvent { TenantId = "school-1", StudentProfileId = "student-active", Status = ScanEventStatus.Accepted, Direction = AttendanceDirection.Entry, LocalScanTime = DateTimeOffset.UtcNow.Date.AddHours(7) });
        await dbContext.SaveChangesAsync();
        var service = new AttendanceGenerationService(dbContext, new FakeExpectedStudentPopulationProvider(), new AttendanceRuleEvaluator());

        await service.GenerateAsync("school-1", session.Id, new GenerateAttendanceRequest("request-1"));
        await service.GenerateAsync("school-1", session.Id, new GenerateAttendanceRequest("request-1"));

        dbContext.AttendanceRecords.Should().ContainSingle(x => x.StudentProfileId == "student-active");
    }
}

