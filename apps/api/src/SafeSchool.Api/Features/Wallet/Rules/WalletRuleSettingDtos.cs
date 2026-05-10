using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Rules;

public sealed record WalletRuleSettingRequest(string CurrencyCode = "SAR", long MinTopUpMinor = 100, long MaxTopUpMinor = 500000, long CashierThresholdMinor = 100000, bool OfflinePosEnabled = false, long OfflinePerStudentReserveMinor = 0, long OfflinePerTerminalReserveMinor = 0, string ChargebackPolicy = "RestrictAndReview", string DuplicateRetryPolicy = "Idempotent Client Identifiers", int DetailedRetentionDays = 365, string ActorReference = "finance-admin");
public sealed record WalletRuleSettingResponse(Guid RuleSettingId, string CurrencyCode, long MinTopUpMinor, long MaxTopUpMinor, long CashierThresholdMinor, bool OfflinePosEnabled, long OfflinePerStudentReserveMinor, long OfflinePerTerminalReserveMinor, string ChargebackPolicy, string DuplicateRetryPolicy, int DetailedRetentionDays, WalletRuleSettingStatus Status, string Version);

public static class WalletRuleSettingMappingExtensions
{
    public static WalletRuleSettingResponse ToResponse(this WalletRuleSetting setting) => new(setting.Id, setting.CurrencyCode, setting.MinTopUpMinor, setting.MaxTopUpMinor, setting.CashierThresholdMinor, setting.OfflinePosEnabled, setting.OfflinePerStudentReserveMinor, setting.OfflinePerTerminalReserveMinor, setting.ChargebackPolicy, setting.DuplicateRetryPolicy, setting.DetailedRetentionDays, setting.Status, setting.Version);
}
