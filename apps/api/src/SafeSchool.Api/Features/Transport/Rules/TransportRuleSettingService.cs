using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Audit;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Rules;

public sealed class TransportRuleSettingService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard, ITransportAuditWriter auditWriter)
{
    public async Task<OperationResult<TransportRuleSettingResponse>> CreateDraftAsync(string tenantId, TransportRuleSettingRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, "transport.rules", TransportPermissionCatalog.RulesManage, targetType: "TransportRuleSetting", targetReference: request.ClientRequestId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<TransportRuleSettingResponse>.Failure(allowed.Errors.ToArray());
        var setting = new TransportRuleSetting
        {
            TenantId = tenantId,
            AssignmentEligibilityPolicy = request.AssignmentEligibilityPolicy,
            PickupWindow = request.PickupWindow,
            DropWindow = request.DropWindow,
            RouteDeviationThresholdMeters = request.RouteDeviationThresholdMeters,
            LocationStalenessThreshold = request.LocationStalenessThreshold,
            EtaChangeThreshold = request.EtaChangeThreshold,
            NotificationEligibilityPolicy = request.NotificationEligibilityPolicy,
            ScanClockDriftTolerance = request.ScanClockDriftTolerance,
            RetryHandlingPolicy = request.RetryHandlingPolicy,
            LocationDetailRetentionDays = request.LocationDetailRetentionDays,
            ChangeReason = request.ChangeReason
        };
        if (!setting.CanActivate()) return OperationResult<TransportRuleSettingResponse>.Failure(new ValidationError("invalid_rule_setting", "Transport rule settings must keep 30-day detailed location retention and positive windows."));
        dbContext.TransportRuleSettings.Add(setting);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new TransportAuditEvent { TenantId = tenantId, EventCategory = "Rules", EventType = "transport.rules.create", SubjectType = "TransportRuleSetting", SubjectReference = setting.Id.ToString(), Reason = request.ChangeReason }, cancellationToken);
        return OperationResult<TransportRuleSettingResponse>.Success(setting.ToResponse());
    }

    public async Task<TransportRuleSettingResponse?> CurrentAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.TransportRuleSettings.Where(x => x.TenantId == tenantId && x.Status == FeatureRuleStatus.Active).OrderByDescending(x => x.UpdatedAt).Select(x => x.ToResponse()).FirstOrDefaultAsync(cancellationToken);

    public async Task<OperationResult<TransportRuleSettingResponse>> ActivateAsync(string tenantId, Guid settingId, string reason, CancellationToken cancellationToken = default)
    {
        var setting = await dbContext.TransportRuleSettings.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == settingId, cancellationToken);
        if (setting is null) return OperationResult<TransportRuleSettingResponse>.Failure(new ValidationError("not_found", "Rule setting was not found."));
        if (!setting.CanActivate()) return OperationResult<TransportRuleSettingResponse>.Failure(new ValidationError("invalid_rule_setting", "Rule setting cannot be activated."));
        await dbContext.TransportRuleSettings.Where(x => x.TenantId == tenantId && x.Status == FeatureRuleStatus.Active).ExecuteUpdateAsync(x => x.SetProperty(s => s.Status, FeatureRuleStatus.Superseded), cancellationToken);
        setting.Status = FeatureRuleStatus.Active;
        setting.ChangeReason = reason;
        setting.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportRuleSettingResponse>.Success(setting.ToResponse());
    }
}
