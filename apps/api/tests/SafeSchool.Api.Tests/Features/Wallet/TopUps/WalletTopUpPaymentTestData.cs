using SafeSchool.Api.Tests.Features.Wallet.Fixtures;

namespace SafeSchool.Api.Tests.Features.Wallet.TopUps;

public static class WalletTopUpPaymentTestData
{
    public static string TenantId => WalletTestData.TenantId;
    public static string StudentProfileId => WalletTestData.StudentProfileId;
    public static long AmountMinor => 1250;
}
