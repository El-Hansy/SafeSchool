using SafeSchool.Api.Tests.Features.Wallet.Fixtures;

namespace SafeSchool.Api.Tests.Features.Wallet.History;

public static class TransactionHistoryReviewTestData
{
    public static string TenantId => WalletTestData.TenantId;
    public static string StudentProfileId => WalletTestData.StudentProfileId;
    public static long AmountMinor => 1250;
}
