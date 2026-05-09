using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Limits;

public sealed class SpendingLimitEvaluationService(SafeSchoolDbContext dbContext)
{
    public async Task<EffectiveLimitResponse> EvaluateAsync(string tenantId, Guid walletId, long amountMinor, string merchantCode = "", string itemCategoryCode = "", CancellationToken cancellationToken = default)
    {
        var limits = await dbContext.SpendingLimits.Where(x => x.TenantId == tenantId && x.StudentWalletId == walletId && x.Status == SafeSchool.Api.Features.Wallet.Common.SpendingLimitStatus.Active).ToListAsync(cancellationToken);
        var blocking = limits.OrderBy(x => x.PrecedenceRank).FirstOrDefault(x => (x.AmountMinor > 0 && amountMinor > x.AmountMinor) || (!string.IsNullOrWhiteSpace(x.ItemCategoryCode) && x.ItemCategoryCode == itemCategoryCode && x.LimitType == SafeSchool.Api.Features.Wallet.Common.SpendingLimitType.Category));
        return new EffectiveLimitResponse(walletId, blocking is null, blocking?.LimitType.ToString() ?? "none", blocking?.RuleVersion ?? "no-active-limit");
    }
}
