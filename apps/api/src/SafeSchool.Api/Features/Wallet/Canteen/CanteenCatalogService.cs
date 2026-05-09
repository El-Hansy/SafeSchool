using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Pos;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Canteen;

public sealed class CanteenCatalogService(SafeSchoolDbContext dbContext, WalletPermissionGuard guard, IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<MerchantResponse>> CreateMerchantAsync(string tenantId, MerchantRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.CanteenPos, WalletPermissionCatalog.PurchasesRecord, targetType: "CanteenMerchant", targetReference: request.MerchantCode, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<MerchantResponse>.Failure(allowed.Errors.ToArray());
        if (await dbContext.CanteenMerchants.AnyAsync(x => x.TenantId == tenantId && x.MerchantCode == request.MerchantCode, cancellationToken)) return OperationResult<MerchantResponse>.Failure(new ValidationError("duplicate_merchant_code", "Merchant code already exists."));
        var merchant = new CanteenMerchant { TenantId = tenantId, MerchantName = request.MerchantName, MerchantCode = request.MerchantCode, MerchantStatus = request.Status, AllowedCategoryCodes = request.AllowedCategoryCodes, CreatedBy = request.ActorReference, UpdatedBy = request.ActorReference };
        dbContext.CanteenMerchants.Add(merchant);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "POS", EventType = "wallet.canteen.merchant_create", SubjectType = "CanteenMerchant", SubjectReference = merchant.Id.ToString(), ActorReference = request.ActorReference, Reason = "merchant created" }, cancellationToken);
        return OperationResult<MerchantResponse>.Success(merchant.ToResponse());
    }

    public async Task<IReadOnlyList<MerchantResponse>> ListMerchantsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.CanteenMerchants.Where(x => x.TenantId == tenantId).OrderBy(x => x.MerchantCode).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<Guid> CreateItemCategoryAsync(string tenantId, ItemCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new CanteenItemCategory { TenantId = tenantId, ItemCategoryCode = request.ItemCategoryCode, DisplayName = request.DisplayName, Status = request.Status };
        dbContext.CanteenItemCategories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return category.Id;
    }

    public async Task<Guid> CreateEligibilityRuleAsync(string tenantId, PurchaseEligibilityRuleRequest request, CancellationToken cancellationToken = default)
    {
        var rule = new PurchaseEligibilityRule { TenantId = tenantId, CanteenMerchantId = request.CanteenMerchantId, ItemCategoryCode = request.ItemCategoryCode, Status = request.Status, DenialReason = request.DenialReason };
        dbContext.PurchaseEligibilityRules.Add(rule);
        await dbContext.SaveChangesAsync(cancellationToken);
        return rule.Id;
    }
}
