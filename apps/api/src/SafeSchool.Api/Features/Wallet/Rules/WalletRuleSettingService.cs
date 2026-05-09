using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Rules;

public sealed class WalletRuleSettingService(SafeSchoolDbContext dbContext, WalletRuleSettingValidator validator, IWalletAuditWriter auditWriter)
{
    public async Task<WalletRuleSettingResponse?> CurrentAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.WalletRuleSettings.Where(x => x.TenantId == tenantId && x.Status == WalletRuleSettingStatus.Active).OrderByDescending(x => x.CreatedAt).Select(x => x.ToResponse()).FirstOrDefaultAsync(cancellationToken);

    public async Task<OperationResult<WalletRuleSettingResponse>> CreateDraftAsync(string tenantId, WalletRuleSettingRequest request, CancellationToken cancellationToken = default)
    {
        var valid = validator.Validate(request);
        if (!valid.Succeeded) return OperationResult<WalletRuleSettingResponse>.Failure(valid.Errors.ToArray());
        var setting = new WalletRuleSetting { TenantId = tenantId, CurrencyCode = request.CurrencyCode, MinTopUpMinor = request.MinTopUpMinor, MaxTopUpMinor = request.MaxTopUpMinor, CashierThresholdMinor = request.CashierThresholdMinor, OfflinePosEnabled = request.OfflinePosEnabled, OfflinePerStudentReserveMinor = request.OfflinePerStudentReserveMinor, OfflinePerTerminalReserveMinor = request.OfflinePerTerminalReserveMinor, ChargebackPolicy = request.ChargebackPolicy, DuplicateRetryPolicy = request.DuplicateRetryPolicy, DetailedRetentionDays = request.DetailedRetentionDays, CreatedBy = request.ActorReference, UpdatedBy = request.ActorReference };
        dbContext.WalletRuleSettings.Add(setting);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "Rules", EventType = "wallet.rules.create_draft", SubjectType = "WalletRuleSetting", SubjectReference = setting.Id.ToString(), ActorReference = request.ActorReference, Reason = "draft rule setting" }, cancellationToken);
        return OperationResult<WalletRuleSettingResponse>.Success(setting.ToResponse());
    }

    public async Task<OperationResult<WalletRuleSettingResponse>> ActivateAsync(string tenantId, Guid id, string actor, CancellationToken cancellationToken = default)
    {
        var setting = await dbContext.WalletRuleSettings.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken);
        if (setting is null) return OperationResult<WalletRuleSettingResponse>.Failure(new ValidationError("rule_setting_not_found", "Rule setting was not found."));
        await dbContext.WalletRuleSettings.Where(x => x.TenantId == tenantId && x.Status == WalletRuleSettingStatus.Active).ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, WalletRuleSettingStatus.Superseded), cancellationToken);
        setting.Status = WalletRuleSettingStatus.Active;
        setting.UpdatedBy = actor;
        setting.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<WalletRuleSettingResponse>.Success(setting.ToResponse());
    }

    public async Task<OperationResult<WalletRuleSettingResponse>> SuspendAsync(string tenantId, Guid id, string reason, string actor, CancellationToken cancellationToken = default)
    {
        var setting = await dbContext.WalletRuleSettings.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken);
        if (setting is null) return OperationResult<WalletRuleSettingResponse>.Failure(new ValidationError("rule_setting_not_found", "Rule setting was not found."));
        setting.Status = WalletRuleSettingStatus.Suspended;
        setting.UpdatedBy = actor;
        setting.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<WalletRuleSettingResponse>.Success(setting.ToResponse());
    }
}
