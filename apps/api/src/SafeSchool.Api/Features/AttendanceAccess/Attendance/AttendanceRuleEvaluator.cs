using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public sealed class AttendanceRuleEvaluator
{
    public AttendanceStatus Evaluate(AttendanceSession session, GateScanEvent? entryScan, GateScanEvent? exitScan = null)
    {
        if (entryScan is null)
        {
            return AttendanceStatus.Absent;
        }

        if (entryScan.Status != ScanEventStatus.Accepted)
        {
            return AttendanceStatus.NeedsReview;
        }

        var entryTime = TimeOnly.FromDateTime(entryScan.LocalScanTime.LocalDateTime);
        if (entryTime > session.LateAfter)
        {
            return AttendanceStatus.Late;
        }

        if (exitScan is not null && TimeOnly.FromDateTime(exitScan.LocalScanTime.LocalDateTime) < session.EarlyExitBefore)
        {
            return AttendanceStatus.EarlyExit;
        }

        return AttendanceStatus.Present;
    }
}

