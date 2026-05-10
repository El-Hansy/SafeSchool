namespace SafeSchool.Api.Features.Mobile;

public static class MobileSupportInstallEventEndpoints
{
    public static RouteGroupBuilder MapMobileSupportInstallEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/support/install-events", (string tenantId, string? releaseId, string? userId, string? deviceId, int? versionCode, string? eventType) =>
            Results.Ok(new
            {
                items = new[]
                {
                    new { installEventId = "install-1", releaseId = releaseId ?? "release-12", versionName = "12.0.0", versionCode = versionCode ?? 1200, eventType = eventType ?? "launch", eventResult = "allowed", occurredAt = DateTimeOffset.UtcNow, userMessage = "Version accepted." }
                },
                nextCursor = (string?)null
            }));
        return group;
    }
}
