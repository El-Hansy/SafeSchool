namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileAuditEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string ActorUserId { get; init; } = "user-demo";
    public string DeviceId { get; init; } = "device-demo";
    public string EventType { get; init; } = "mobile.sign_in";
    public string TargetType { get; init; } = "mobile";
    public string TargetId { get; init; } = "target-demo";
    public string RoleCode { get; init; } = MobileRoleCodes.Guardian;
    public string PermissionCode { get; init; } = "mobile.workspace.view";
    public string LanguageCode { get; init; } = "en";
    public string Result { get; init; } = "allowed";
    public string ReasonCode { get; init; } = "ok";
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString("N");
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
