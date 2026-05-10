using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileNotificationEndpoints
{
    public static RouteGroupBuilder MapMobileNotificationEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/notifications", (string tenantId, string roleCode, string? studentId, string? cursor, int? limit, string? languageCode) =>
        {
            var language = languageCode ?? "en";
            var title = language == "ar" ? "تنبيه المدرسة" : "School notification";
            var body = language == "ar" ? "تنبيه مرتبط بالدور الحالي" : "Notification scoped to the active role.";
            return Results.Ok(new { items = new[] { new MobileNotificationDto("mobile-notif-1", "communications", title, body, DateTimeOffset.UtcNow, "Unread", "/mobile/notifications/mobile-notif-1") }, nextCursor = (string?)null });
        });
        group.MapPost("/notifications/{notificationId}/read", (string notificationId, string? tenantId, string? userId, string? deviceId, string? roleCode, ITenantContext tenantContext, MobileAuditService audit) =>
        {
            if (MobileEndpointContext.HasTenantConflict(tenantId, tenantContext))
            {
                return Results.Json(new { error = "tenant_mismatch" }, statusCode: StatusCodes.Status403Forbidden);
            }

            var evt = audit.Record(
                MobileEndpointContext.Tenant(tenantId, tenantContext),
                MobileEndpointContext.User(userId, tenantContext),
                MobileEndpointContext.Device(deviceId),
                "mobile.notification.read",
                roleCode ?? MobileRoleCodes.Guardian,
                "mobile.notifications.read",
                "updated",
                "ok",
                "notification",
                notificationId);
            return Results.Ok(new { notificationId, readState = "Read", auditEventId = evt.Id.ToString("N") });
        });
        return group;
    }
}
