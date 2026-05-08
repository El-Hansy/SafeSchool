using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed record CreateAttendanceSessionRequest(string SessionName, DateOnly AttendanceDate, string CampusReference, string ExpectedPopulationRule, TimeOnly EntryWindowStart, TimeOnly EntryWindowEnd, TimeOnly LateAfter, TimeOnly EarlyExitBefore);
public sealed record UpdateAttendanceSessionRequest(AttendanceSessionStatus? Status, string Reason);
public sealed record AttendanceSessionResponse(Guid AttendanceSessionId, string SchoolAccountId, string SessionName, DateOnly AttendanceDate, AttendanceSessionStatus GenerationStatus);
public sealed record GenerateAttendanceRequest(string ClientRequestId);
public sealed record AttendanceRecordResponse(Guid AttendanceRecordId, Guid AttendanceSessionId, string StudentProfileId, AttendanceStatus Status, Guid? SourceScanEventId, string CorrectionReason);
public sealed record CorrectAttendanceRequest(AttendanceStatus Status, string Reason, Guid? AnomalyId = null, Guid? ManualReviewId = null);
public sealed record AttendanceSummaryResponse(Guid AttendanceSessionId, int Present, int Late, int Absent, int EarlyExit, int NeedsReview);

public static class AttendanceMappings
{
    public static AttendanceSessionResponse ToResponse(this AttendanceSession session) => new(session.Id, session.TenantId, session.SessionName, session.AttendanceDate, session.GenerationStatus);
    public static AttendanceRecordResponse ToResponse(this AttendanceRecord record) => new(record.Id, record.AttendanceSessionId, record.StudentProfileId, record.Status, record.SourceScanEventId, record.CorrectionReason);
}

