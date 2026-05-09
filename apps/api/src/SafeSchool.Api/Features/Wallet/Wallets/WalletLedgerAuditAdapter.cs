namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed class WalletLedgerAuditAdapter
{
    public string EventFor(string action) => $"wallet.ledger.{action}";
}
