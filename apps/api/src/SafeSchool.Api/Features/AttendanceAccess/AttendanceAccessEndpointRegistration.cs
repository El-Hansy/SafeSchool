using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess;

public static class AttendanceAccessEndpointRegistration
{
    public const string RoutePrefix = "/api/v1/schools/{schoolAccountId}/attendance-access";

    public static IEndpointRouteBuilder MapAttendanceAccessEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(RoutePrefix);
        group.MapGateEndpoints();
        group.MapScanEndpoints();
        group.MapAttendanceEndpoints();
        group.MapNotificationEndpoints();
        group.MapAnomalyEndpoints();
        return endpoints;
    }
}

