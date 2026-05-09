namespace SafeSchool.Api.Features.Wallet.Common.Money;

public sealed record WalletMoney(long AmountMinor, string CurrencyCode)
{
    public bool IsPositive => AmountMinor > 0;
    public WalletMoney Negate() => this with { AmountMinor = -AmountMinor };
    public string Display() => $"{CurrencyCode} {AmountMinor / 100m:0.00}";
}
