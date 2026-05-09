using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class TransportRoute : TenantOwnedEntity
{
    public string RouteName { get; set; } = string.Empty;
    public string RouteCode { get; set; } = string.Empty;
    public ServiceDirection ServiceDirection { get; set; } = ServiceDirection.Pickup;
    public string CampusReference { get; set; } = string.Empty;
    public TimeOnly? PlannedStartTime { get; set; }
    public TimeOnly? PlannedEndTime { get; set; }
    public RouteStatus RouteStatus { get; set; } = RouteStatus.Draft;
    public string RouteVersion { get; set; } = "v1";
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    public bool CanActivate(int activeStopCount) => !string.IsNullOrWhiteSpace(RouteCode) && !string.IsNullOrWhiteSpace(RouteName) && activeStopCount > 0;
    public bool CanTransitionTo(RouteStatus next) => next == RouteStatus.Active ? RouteStatus is RouteStatus.Draft or RouteStatus.Suspended && !string.IsNullOrWhiteSpace(RouteCode) : true;
    public string NextVersion() => $"v{(int.TryParse(RouteVersion.TrimStart('v'), out var number) ? number + 1 : 2)}";
}
