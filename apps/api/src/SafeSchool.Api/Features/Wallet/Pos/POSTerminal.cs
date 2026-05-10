using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class POSTerminal : TenantOwnedEntity
{
    public Guid CanteenMerchantId { get; set; }
    public string TerminalCode { get; set; } = string.Empty;
    public string DeviceReference { get; set; } = string.Empty;
    public string OperatorReference { get; set; } = string.Empty;
    public bool OfflineEnabled { get; set; }
    public long PerTerminalReserveMinor { get; set; }
    public DateTimeOffset? LastSyncAt { get; set; }
    public PosTerminalStatus Status { get; set; } = PosTerminalStatus.Draft;

    public bool IsReadyForOnlinePurchase() => Status is PosTerminalStatus.Active or PosTerminalStatus.OfflineAllowed && !string.IsNullOrWhiteSpace(DeviceReference);
    public bool CanCaptureOffline(long amountMinor) => OfflineEnabled && amountMinor <= PerTerminalReserveMinor;
}
