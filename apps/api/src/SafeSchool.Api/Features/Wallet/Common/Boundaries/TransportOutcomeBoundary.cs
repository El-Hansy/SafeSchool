namespace SafeSchool.Api.Features.Wallet.Common.Boundaries;

public interface IWalletTransportOutcomeBoundary
{
    bool AllowsWalletScanWithoutTransportMutation(string tenantId, string studentProfileId);
}

public sealed class WalletTransportOutcomeBoundary : IWalletTransportOutcomeBoundary
{
    public bool AllowsWalletScanWithoutTransportMutation(string tenantId, string studentProfileId) => true;
}
