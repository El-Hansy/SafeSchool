namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileAuditService
{
    private readonly List<MobileAuditEvent> _events = [];

    public MobileAuditEvent Record(string tenantId, string actorUserId, string deviceId, string eventType, string roleCode, string permissionCode, string result, string reasonCode, string targetType = "mobile", string targetId = "demo")
    {
        var evt = new MobileAuditEvent
        {
            TenantId = tenantId,
            ActorUserId = actorUserId,
            DeviceId = deviceId,
            EventType = eventType,
            RoleCode = roleCode,
            PermissionCode = permissionCode,
            Result = result,
            ReasonCode = reasonCode,
            TargetType = targetType,
            TargetId = targetId
        };
        _events.Add(evt);
        return evt;
    }

    public IReadOnlyList<MobileAuditEventDto> Events(string tenantId) =>
        (_events.Count == 0 ? DemoEvents(tenantId) : _events)
            .Where(evt => evt.TenantId == tenantId)
            .Select(evt => new MobileAuditEventDto(evt.Id.ToString("N"), evt.EventType, evt.ActorUserId, evt.DeviceId, evt.RoleCode, evt.PermissionCode, evt.LanguageCode, evt.Result, evt.ReasonCode, evt.TargetType, evt.TargetId, evt.OccurredAt))
            .ToArray();

    private static IReadOnlyList<MobileAuditEvent> DemoEvents(string tenantId) =>
    [
        new() { TenantId = tenantId, EventType = "mobile.sign_in", ActorUserId = "guardian-demo", DeviceId = "device-guardian", RoleCode = MobileRoleCodes.Guardian },
        new() { TenantId = tenantId, EventType = "mobile.denied_access", ActorUserId = "student-demo", DeviceId = "device-student", RoleCode = MobileRoleCodes.Student, Result = "blocked", ReasonCode = "FEATURE_DISABLED" },
        new() { TenantId = tenantId, EventType = "mobile.sync.accepted", ActorUserId = "driver-demo", DeviceId = "device-driver", RoleCode = MobileRoleCodes.TransportDriver }
    ];
}
