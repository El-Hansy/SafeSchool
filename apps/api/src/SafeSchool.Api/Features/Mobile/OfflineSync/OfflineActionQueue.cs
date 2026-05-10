namespace SafeSchool.Api.Features.Mobile;

public sealed class OfflineActionQueue
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string UserId { get; init; } = "user-demo";
    public string DeviceId { get; init; } = "device-demo";
    public string SourceFeatureCode { get; init; } = "attendance";
    public string SourceActionCode { get; init; } = "scan.capture";
    public string ClientActionId { get; init; } = Guid.NewGuid().ToString("N");
    public DateTimeOffset LocalOccurredAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ServerReceivedAt { get; init; }
    public OfflineSyncStatus SyncStatus { get; init; } = OfflineSyncStatus.Queued;
    public string? ConflictReason { get; init; }
    public Guid? AuditEventId { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
