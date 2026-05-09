namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040001AttendanceAccessFoundation
{
    public const string MigrationId = "202605040001_AttendanceAccessFoundation";
    public static readonly string[] Tables =
    [
        "attendance_access_audit_events",
        "attendance_access_idempotency_records",
        "attendance_access_rule_settings"
    ];
}

