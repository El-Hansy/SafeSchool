namespace SafeSchool.Api.Features.Mobile;

public sealed class DeviceSession
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string UserId { get; init; } = "user-demo";
    public string DeviceId { get; init; } = "device-demo";
    public string DeviceLabel { get; init; } = "Android demo";
    public string AppVersion { get; init; } = "1.0.0";
    public Guid? ReleaseId { get; init; }
    public string ActiveRoleCode { get; init; } = MobileRoleCodes.Guardian;
    public string LanguageCode { get; init; } = "en";
    public DeviceSessionStatus SessionStatus { get; init; } = DeviceSessionStatus.Active;
    public DateTimeOffset SignedInAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastSeenAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SignedOutAt { get; init; }
    public DateTimeOffset? RevokedAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
