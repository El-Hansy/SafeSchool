using SafeSchool.Api.Features.Transport.Anomalies;
using SafeSchool.Api.Features.Transport.Assignments;
using SafeSchool.Api.Features.Transport.Eta;
using SafeSchool.Api.Features.Transport.Notifications;
using SafeSchool.Api.Features.Transport.Routes;
using SafeSchool.Api.Features.Transport.Scans;
using SafeSchool.Api.Features.Transport.Tracking;

namespace SafeSchool.Api.Features.Transport;

public static class TransportEndpointRegistration
{
    public const string RoutePrefix = "/api/v1/schools/{schoolAccountId}/transport";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/students";

    public static IEndpointRouteBuilder MapTransportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var schoolGroup = endpoints.MapGroup(RoutePrefix);
        var guardianGroup = endpoints.MapGroup(GuardianRoutePrefix);
        schoolGroup.MapRouteStopEndpoints();
        schoolGroup.MapVehicleAssignmentEndpoints(guardianGroup);
        schoolGroup.MapScanEndpoints();
        schoolGroup.MapLiveTrackingEndpoints(guardianGroup);
        schoolGroup.MapEtaEndpoints(guardianGroup);
        schoolGroup.MapTransportNotificationEndpoints(guardianGroup);
        schoolGroup.MapTransportReviewEndpoints();
        return endpoints;
    }
}
