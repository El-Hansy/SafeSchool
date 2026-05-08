namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040003AttendanceGeneration
{
    public const string MigrationId = "202605040003_AttendanceGeneration";
    public static readonly string[] Tables =
    [
        "attendance_access_sessions",
        "attendance_access_records"
    ];
}

