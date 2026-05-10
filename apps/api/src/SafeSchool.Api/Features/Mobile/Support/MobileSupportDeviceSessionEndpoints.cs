namespace SafeSchool.Api.Features.Mobile;

public static class MobileSupportDeviceSessionEndpoints
{
    public static RouteGroupBuilder MapMobileSupportDeviceSessionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/support/device-sessions", (string tenantId, string? userId, string? deviceId, string? appVersion, string? sessionStatus) =>
            Results.Ok(new
            {
                items = new[]
                {
                    new DeviceSessionDto("session-guardian", tenantId, userId ?? "guardian-demo", deviceId ?? "device-guardian", appVersion ?? "12.0.0", MobileRoleCodes.Guardian, "ar", sessionStatus ?? "Active", DateTimeOffset.UtcNow),
                    new DeviceSessionDto("session-driver", tenantId, "driver-demo", "device-driver", "12.0.0", MobileRoleCodes.TransportDriver, "en", "Active", DateTimeOffset.UtcNow)
                },
                nextCursor = (string?)null
            }));
        return group;
    }
}
