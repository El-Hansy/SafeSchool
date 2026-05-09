using SafeSchool.Api.Features.Wallet.Common.Boundaries;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public sealed class FakeTransportOutcomeBoundary : IWalletTransportOutcomeBoundary
{
    public bool AllowsWalletScanWithoutTransportMutation(string tenantId, string studentProfileId) => true;
}
