using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Sync;

public sealed class OfflinePosSyncBatch : TenantOwnedEntity
{
    public Guid PosTerminalId { get; set; }
    public string ClientBatchId { get; set; } = string.Empty;
    public OfflinePosSyncStatus SyncStatus { get; set; } = OfflinePosSyncStatus.Pending;
    public DateTimeOffset LocalCreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public int AcceptedCount { get; set; }
    public int HeldCount { get; set; }
    public int DuplicateCount { get; set; }
    public string ReviewReason { get; set; } = string.Empty;
}
