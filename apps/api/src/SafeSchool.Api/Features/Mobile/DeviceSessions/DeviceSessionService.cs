namespace SafeSchool.Api.Features.Mobile;

public sealed class DeviceSessionService(MobileAuditService audit)
{
    public DeviceSession Start(string tenantId, string userId, string deviceId, string roleCode, string languageCode)
    {
        var session = new DeviceSession
        {
            TenantId = tenantId,
            UserId = userId,
            DeviceId = deviceId,
            ActiveRoleCode = roleCode,
            LanguageCode = languageCode,
            AppVersion = "12.0.0"
        };
        audit.Record(tenantId, userId, deviceId, "mobile.context_selected", roleCode, "mobile.context", "allowed", "ok");
        return session;
    }

    public DeviceSessionDto ToDto(DeviceSession session) => new(session.Id.ToString("N"), session.TenantId, session.UserId, session.DeviceId, session.AppVersion, session.ActiveRoleCode, session.LanguageCode, session.SessionStatus.ToString(), session.LastSeenAt);
}
