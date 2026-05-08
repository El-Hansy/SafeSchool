namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public static class AttendanceAccessPermissionCatalog
{
    public const string GatesRead = "attendance_access.gates.read";
    public const string GatesManage = "attendance_access.gates.manage";
    public const string ScanPointsManage = "attendance_access.scan_points.manage";
    public const string ScansRecord = "attendance_access.scans.record";
    public const string ScansSync = "attendance_access.scans.sync";
    public const string ScansRead = "attendance_access.scans.read";
    public const string AttendanceRead = "attendance_access.attendance.read";
    public const string AttendanceGenerate = "attendance_access.attendance.generate";
    public const string AttendanceCorrect = "attendance_access.attendance.correct";
    public const string NotificationsRead = "attendance_access.notifications.read";
    public const string GuardianEntryExitRead = "attendance_access.guardian_entry_exit.read";
    public const string AnomaliesRead = "attendance_access.anomalies.read";
    public const string AnomaliesResolve = "attendance_access.anomalies.resolve";
    public const string AuditRead = "attendance_access.audit.read";

    public static readonly string[] SchoolAdministrator =
    [
        GatesRead,
        GatesManage,
        ScanPointsManage,
        ScansRecord,
        ScansSync,
        ScansRead,
        AttendanceRead,
        AttendanceGenerate,
        AttendanceCorrect,
        NotificationsRead,
        GuardianEntryExitRead,
        AnomaliesRead,
        AnomaliesResolve,
        AuditRead
    ];
}

