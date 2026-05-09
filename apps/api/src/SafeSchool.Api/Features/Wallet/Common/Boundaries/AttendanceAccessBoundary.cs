namespace SafeSchool.Api.Features.Wallet.Common.Boundaries;

public interface IWalletAttendanceAccessBoundary
{
    bool AllowsWalletScanWithoutAttendanceMutation(string tenantId, string studentProfileId);
}

public sealed class WalletAttendanceAccessBoundary : IWalletAttendanceAccessBoundary
{
    public bool AllowsWalletScanWithoutAttendanceMutation(string tenantId, string studentProfileId) => true;
}
