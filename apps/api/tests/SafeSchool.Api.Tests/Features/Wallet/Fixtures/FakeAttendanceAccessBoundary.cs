using SafeSchool.Api.Features.Wallet.Common.Boundaries;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public sealed class FakeAttendanceAccessBoundary : IWalletAttendanceAccessBoundary
{
    public bool AllowsWalletScanWithoutAttendanceMutation(string tenantId, string studentProfileId) => true;
}
