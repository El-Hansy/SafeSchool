using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Common.Money;

public sealed class WalletMoneyValidator
{
    public OperationResult<WalletMoney> RequirePositive(long amountMinor, string currencyCode, string expectedCurrencyCode = "SAR")
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || !string.Equals(currencyCode, expectedCurrencyCode, StringComparison.OrdinalIgnoreCase))
        {
            return OperationResult<WalletMoney>.Failure(new ValidationError("currency_mismatch", "Wallet amount must use the school account currency.", nameof(currencyCode)));
        }

        if (amountMinor <= 0)
        {
            return OperationResult<WalletMoney>.Failure(new ValidationError("invalid_amount", "Wallet amount must be positive minor units.", nameof(amountMinor)));
        }

        return OperationResult<WalletMoney>.Success(new WalletMoney(amountMinor, currencyCode.ToUpperInvariant()));
    }

    public bool PreservesMinorUnits(long amountMinor) => amountMinor == decimal.ToInt64(amountMinor);
}
