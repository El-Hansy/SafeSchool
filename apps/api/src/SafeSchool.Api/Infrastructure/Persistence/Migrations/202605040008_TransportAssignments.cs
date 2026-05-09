namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040008TransportAssignments
{
    public const string MigrationId = "202605040008_TransportAssignments";
    public static readonly string[] Tables =
    [
        "transport_vehicles",
        "student_transport_assignments"
    ];
}
