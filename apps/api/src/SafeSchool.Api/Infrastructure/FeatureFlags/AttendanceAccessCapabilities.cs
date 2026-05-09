namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class AttendanceAccessCapabilities
{
    public const string GateScanning = "attendance_access.gate_scanning";
    public const string AttendanceGeneration = "attendance_access.attendance_generation";
    public const string EntryExitNotifications = "attendance_access.entry_exit_notifications";
    public const string AnomalyDetection = "attendance_access.anomaly_detection";

    public static readonly string[] All =
    [
        GateScanning,
        AttendanceGeneration,
        EntryExitNotifications,
        AnomalyDetection
    ];
}

