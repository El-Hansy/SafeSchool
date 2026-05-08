using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public static class AttendanceTestData
{
    public static AttendanceSession ActiveSession() => new()
    {
        TenantId = "school-1",
        SessionName = "Morning",
        AttendanceDate = DateOnly.FromDateTime(DateTime.UtcNow),
        ExpectedPopulationRule = "student-active",
        GenerationStatus = AttendanceSessionStatus.Active
    };
}

