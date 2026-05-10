namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040005AttendanceAnomalies
{
    public const string MigrationId = "202605040005_AttendanceAnomalies";
    public static readonly string[] Tables =
    [
        "attendance_access_anomalies",
        "attendance_access_manual_reviews"
    ];
}

