namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040007TransportRoutesAndStops
{
    public const string MigrationId = "202605040007_TransportRoutesAndStops";
    public static readonly string[] Tables =
    [
        "transport_routes",
        "transport_stops",
        "transport_route_stop_sequences"
    ];
}
