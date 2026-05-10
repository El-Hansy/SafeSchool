using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed class StudentWallet : TenantOwnedEntity
{
    public string StudentProfileId { get; set; } = string.Empty;
    public string WalletCode { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = "SAR";
    public long AvailableBalanceMinor { get; set; }
    public long PendingBalanceMinor { get; set; }
    public long HeldBalanceMinor { get; set; }
    public long SettledBalanceMinor { get; set; }
    public long PendingRecoveryMinor { get; set; }
    public WalletStatus WalletStatus { get; set; } = WalletStatus.Draft;
    public string RestrictionReason { get; set; } = string.Empty;
    public Guid? CurrentRuleSettingId { get; set; }
    public string CreatedBy { get; set; } = "system";
    public string UpdatedBy { get; set; } = "system";

    public bool CanActivate() => WalletStatus is WalletStatus.Draft or WalletStatus.Suspended && !string.IsNullOrWhiteSpace(StudentProfileId);
    public bool CanClose() => HeldBalanceMinor == 0 && PendingRecoveryMinor == 0;
    public void Activate(string actor) { WalletStatus = WalletStatus.Active; UpdatedBy = actor; UpdatedAt = DateTimeOffset.UtcNow; }
    public void Restrict(string reason, string actor) { WalletStatus = WalletStatus.Restricted; RestrictionReason = reason; UpdatedBy = actor; UpdatedAt = DateTimeOffset.UtcNow; }
    public void Restore(string actor) { WalletStatus = WalletStatus.Active; RestrictionReason = string.Empty; UpdatedBy = actor; UpdatedAt = DateTimeOffset.UtcNow; }
}
