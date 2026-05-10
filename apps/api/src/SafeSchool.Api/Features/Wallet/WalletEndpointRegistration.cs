using SafeSchool.Api.Features.Wallet.History;
using SafeSchool.Api.Features.Wallet.Limits;
using SafeSchool.Api.Features.Wallet.Pos;
using SafeSchool.Api.Features.Wallet.Reconciliation;
using SafeSchool.Api.Features.Wallet.Rules;
using SafeSchool.Api.Features.Wallet.TopUps;
using SafeSchool.Api.Features.Wallet.Wallets;

namespace SafeSchool.Api.Features.Wallet;

public static class WalletEndpointRegistration
{
    public const string RoutePrefix = "/api/v1/schools/{schoolAccountId}/wallet";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/students";

    public static IEndpointRouteBuilder MapWalletEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var schoolGroup = endpoints.MapGroup(RoutePrefix);
        var guardianGroup = endpoints.MapGroup(GuardianRoutePrefix);
        schoolGroup.MapStudentWalletEndpoints(guardianGroup);
        schoolGroup.MapWalletRuleSettingEndpoints();
        schoolGroup.MapWalletTopUpPaymentEndpoints(guardianGroup);
        schoolGroup.MapCanteenPosPurchaseEndpoints();
        schoolGroup.MapSpendingLimitEndpoints(guardianGroup);
        schoolGroup.MapTransactionHistoryReviewEndpoints(guardianGroup);
        schoolGroup.MapWalletReconciliationEndpoints(guardianGroup);
        return endpoints;
    }
}
