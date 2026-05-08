namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class TransportCapabilities
{
    public const string BusAssignment = "transport.bus_assignment";
    public const string RouteStopManagement = "transport.route_stop_management";
    public const string LiveTracking = "transport.live_tracking";
    public const string BoardingDropScans = "transport.boarding_drop_scans";
    public const string EtaCalculation = "transport.eta_calculation";
    public const string Notifications = "transport.notifications";

    public static readonly string[] All =
    [
        BusAssignment,
        RouteStopManagement,
        LiveTracking,
        BoardingDropScans,
        EtaCalculation,
        Notifications
    ];
}
