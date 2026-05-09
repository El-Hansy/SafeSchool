namespace SafeSchool.Api.Features.Transport.Common;

public static class TransportPermissionCatalog
{
    public const string VehiclesRead = "transport.vehicles.read";
    public const string VehiclesManage = "transport.vehicles.manage";
    public const string RoutesRead = "transport.routes.read";
    public const string RoutesManage = "transport.routes.manage";
    public const string AssignmentsRead = "transport.assignments.read";
    public const string AssignmentsManage = "transport.assignments.manage";
    public const string TripsRead = "transport.trips.read";
    public const string TripsStart = "transport.trips.start";
    public const string TripsUpdate = "transport.trips.update";
    public const string TripsEnd = "transport.trips.end";
    public const string ScansRecord = "transport.scans.record";
    public const string ScansSync = "transport.scans.sync";
    public const string ScansRead = "transport.scans.read";
    public const string ScansReview = "transport.scans.review";
    public const string LocationSubmit = "transport.location.submit";
    public const string TrackingRead = "transport.tracking.read";
    public const string EtaRead = "transport.eta.read";
    public const string EtaCalculate = "transport.eta.calculate";
    public const string NotificationsRead = "transport.notifications.read";
    public const string GuardianVisibilityRead = "transport.guardian_visibility.read";
    public const string AnomaliesRead = "transport.anomalies.read";
    public const string AnomaliesResolve = "transport.anomalies.resolve";
    public const string RulesRead = "transport.rules.read";
    public const string RulesManage = "transport.rules.manage";
    public const string ReviewRead = "transport.review.read";
    public const string AuditRead = "transport.audit.read";

    public static readonly string[] TransportAdministrator =
    [
        VehiclesRead, VehiclesManage, RoutesRead, RoutesManage, AssignmentsRead, AssignmentsManage,
        TripsRead, TripsStart, TripsUpdate, TripsEnd, ScansRecord, ScansSync, ScansRead, ScansReview,
        LocationSubmit, TrackingRead, EtaRead, EtaCalculate, NotificationsRead, GuardianVisibilityRead,
        AnomaliesRead, AnomaliesResolve, RulesRead, RulesManage, ReviewRead, AuditRead
    ];

    public static readonly string[] DriverOrAttendant = [TripsRead, TripsStart, TripsUpdate, TripsEnd, ScansRecord, ScansSync, LocationSubmit, TrackingRead];
    public static readonly string[] Guardian = [GuardianVisibilityRead, EtaRead, NotificationsRead];
    public static readonly string[] Reviewer = [ScansRead, TrackingRead, EtaRead, NotificationsRead, AnomaliesRead, AnomaliesResolve, ReviewRead, AuditRead];
}
