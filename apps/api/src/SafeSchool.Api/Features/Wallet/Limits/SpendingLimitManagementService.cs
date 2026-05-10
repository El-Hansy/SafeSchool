using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Limits;

public sealed class SpendingLimitManagementService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<SpendingLimitResponse>> CreateAsync(string tenantId, SpendingLimitRequest request, CancellationToken cancellationToken = default)
    {
        if (request.AmountMinor < 0) return OperationResult<SpendingLimitResponse>.Failure(new ValidationError("invalid_amount", "Limit amount cannot be negative."));
        var limit = new SpendingLimit { TenantId = tenantId, StudentWalletId = request.WalletId, OwnerType = request.OwnerType, LimitType = request.LimitType, AmountMinor = request.AmountMinor, CurrencyCode = request.CurrencyCode, MerchantCode = request.MerchantCode, ItemCategoryCode = request.ItemCategoryCode, CreatedBy = request.ActorReference };
        dbContext.SpendingLimits.Add(limit);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<SpendingLimitResponse>.Success(limit.ToResponse());
    }
    public async Task<IReadOnlyList<SpendingLimitResponse>> ListAsync(string tenantId, Guid? walletId = null, CancellationToken cancellationToken = default) => await dbContext.SpendingLimits.Where(x => x.TenantId == tenantId && (walletId == null || x.StudentWalletId == walletId)).OrderBy(x => x.CreatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<OperationResult<SpendingLimitResponse>> SetStatusAsync(string tenantId, Guid limitId, SpendingLimitStatus status, CancellationToken cancellationToken = default)
    {
        var limit = await dbContext.SpendingLimits.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == limitId, cancellationToken);
        if (limit is null) return OperationResult<SpendingLimitResponse>.Failure(new ValidationError("limit_not_found", "Spending limit was not found."));
        limit.Status = status; limit.UpdatedAt = DateTimeOffset.UtcNow; await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<SpendingLimitResponse>.Success(limit.ToResponse());
    }
}
