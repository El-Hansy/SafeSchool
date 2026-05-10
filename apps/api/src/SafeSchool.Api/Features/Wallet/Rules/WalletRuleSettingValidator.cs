using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Rules;

public sealed class WalletRuleSettingValidator
{
    public OperationResult<string> Validate(WalletRuleSettingRequest request)
    {
        if (request.MinTopUpMinor <= 0 || request.MaxTopUpMinor < request.MinTopUpMinor) return OperationResult<string>.Failure(new ValidationError("invalid_top_up_limits", "Top-up min/max settings are invalid."));
        if (request.OfflinePosEnabled && (request.OfflinePerStudentReserveMinor <= 0 || request.OfflinePerTerminalReserveMinor <= 0)) return OperationResult<string>.Failure(new ValidationError("offline_reserve_required", "Offline POS requires both student and terminal reserve limits."));
        if (!string.Equals(request.ChargebackPolicy, "RestrictAndReview", StringComparison.OrdinalIgnoreCase)) return OperationResult<string>.Failure(new ValidationError("unsupported_chargeback_policy", "Phase 4 supports restrict-and-review chargeback handling."));
        return OperationResult<string>.Success("valid");
    }
}
