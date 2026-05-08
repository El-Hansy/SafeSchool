namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040002GateScanFlow
{
    public const string MigrationId = "202605040002_GateScanFlow";
    public static readonly string[] Tables =
    [
        "attendance_access_gates",
        "attendance_access_scan_points",
        "attendance_access_scan_events",
        "attendance_access_offline_sync_batches",
        "attendance_access_campus_decisions"
    ];
}

