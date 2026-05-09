using SafeSchool.Api.Tests.Features.Wallet.Fixtures;

namespace SafeSchool.Api.Tests.Features.Wallet.Pos;

public static class CanteenPosPurchaseTestData
{
    public static string TenantId => WalletTestData.TenantId;
    public static string StudentProfileId => WalletTestData.StudentProfileId;
    public static long AmountMinor => 1250;
}
