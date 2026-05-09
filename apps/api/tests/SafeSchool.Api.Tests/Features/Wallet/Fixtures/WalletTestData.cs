using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public static class WalletTestData
{
    public const string TenantId = "school-1";
    public const string StudentProfileId = "student-1";
    public const string CurrencyCode = "SAR";
    public static IReadOnlyList<string> AllCapabilities => SafeSchool.Api.Infrastructure.FeatureFlags.WalletCapabilities.All;
    public static WalletSourceMetadata Source() => new("finance-user", "device-1");
}
