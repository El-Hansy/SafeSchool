using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceRecord : TenantOwnedEntity
{
    public Guid AttendanceSessionId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public AttendanceStatus Status { get; set; }
    public Guid? SourceScanEventId { get; set; }
    public string RuleVersion { get; set; } = "v1";
    public string CorrectionReason { get; set; } = string.Empty;
    public Guid? ManualReviewId { get; set; }
    public Guid? AnomalyId { get; set; }
}

