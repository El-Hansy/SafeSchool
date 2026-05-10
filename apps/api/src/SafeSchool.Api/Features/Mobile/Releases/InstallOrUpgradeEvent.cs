namespace SafeSchool.Api.Features.Mobile;

public sealed class InstallOrUpgradeEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string UserId { get; init; } = "user-demo";
    public string DeviceId { get; init; } = "device-demo";
    public Guid ReleaseId { get; init; }
    public string VersionName { get; init; } = "1.0.0";
    public int VersionCode { get; init; } = 12;
    public InstallEventType EventType { get; init; } = InstallEventType.Launch;
    public InstallEventResult EventResult { get; init; } = InstallEventResult.Allowed;
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
