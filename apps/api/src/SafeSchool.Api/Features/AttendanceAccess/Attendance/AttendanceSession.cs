using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceSession : TenantOwnedEntity
{
    public string SessionName { get; set; } = string.Empty;
    public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string CampusReference { get; set; } = string.Empty;
    public string ExpectedPopulationRule { get; set; } = string.Empty;
    public TimeOnly EntryWindowStart { get; set; } = new(7, 0);
    public TimeOnly EntryWindowEnd { get; set; } = new(8, 0);
    public TimeOnly LateAfter { get; set; } = new(8, 0);
    public TimeOnly EarlyExitBefore { get; set; } = new(13, 0);
    public AttendanceSessionStatus GenerationStatus { get; set; } = AttendanceSessionStatus.Draft;

    public bool IsValidWindow() => EntryWindowStart < EntryWindowEnd && EntryWindowEnd <= LateAfter && EarlyExitBefore > LateAfter;
    public bool CanTransitionTo(AttendanceSessionStatus status) => (GenerationStatus, status) switch
    {
        (AttendanceSessionStatus.Draft, AttendanceSessionStatus.Active) => true,
        (AttendanceSessionStatus.Active, AttendanceSessionStatus.Generated) => true,
        (AttendanceSessionStatus.Generated, AttendanceSessionStatus.Reopened) => true,
        (AttendanceSessionStatus.Reopened, AttendanceSessionStatus.Generated) => true,
        (AttendanceSessionStatus.Generated or AttendanceSessionStatus.Reopened, AttendanceSessionStatus.Closed) => true,
        _ => GenerationStatus == status
    };
}

