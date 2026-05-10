using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Rules;

public sealed class WalletRuleSetting : TenantOwnedEntity
{
    public string CurrencyCode { get; set; } = "SAR";
    public long MinTopUpMinor { get; set; } = 100;
    public long MaxTopUpMinor { get; set; } = 500000;
    public long CashierThresholdMinor { get; set; } = 100000;
    public bool OfflinePosEnabled { get; set; }
    public long OfflinePerStudentReserveMinor { get; set; }
    public long OfflinePerTerminalReserveMinor { get; set; }
    public string ChargebackPolicy { get; set; } = "RestrictAndReview";
    public string DuplicateRetryPolicy { get; set; } = "Idempotent Client Identifiers";
    public int DetailedRetentionDays { get; set; } = 365;
    public WalletRuleSettingStatus Status { get; set; } = WalletRuleSettingStatus.Draft;
    public string Version { get; set; } = "v1";
    public string CreatedBy { get; set; } = "system";
    public string UpdatedBy { get; set; } = "system";
}
