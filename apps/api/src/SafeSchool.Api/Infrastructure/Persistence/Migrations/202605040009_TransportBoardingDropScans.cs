namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040009TransportBoardingDropScans
{
    public const string MigrationId = "202605040009_TransportBoardingDropScans";
    public static readonly string[] Tables =
    [
        "transport_trips",
        "transport_offline_scan_sync_batches",
        "transport_boarding_drop_scan_events",
        "transport_anomalies",
        "transport_manual_reviews"
    ];
}
