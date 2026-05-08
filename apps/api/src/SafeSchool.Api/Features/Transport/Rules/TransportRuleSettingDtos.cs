using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Rules;

public sealed record TransportRuleSettingRequest(string AssignmentEligibilityPolicy, TimeSpan PickupWindow, TimeSpan DropWindow, int RouteDeviationThresholdMeters, TimeSpan LocationStalenessThreshold, TimeSpan EtaChangeThreshold, string NotificationEligibilityPolicy, TimeSpan ScanClockDriftTolerance, string RetryHandlingPolicy, int LocationDetailRetentionDays, string ChangeReason, string ClientRequestId);
public sealed record TransportRuleSettingResponse(Guid RuleSettingId, string SchoolAccountId, FeatureRuleStatus Status, int LocationDetailRetentionDays, TimeSpan LocationStalenessThreshold, string ChangeReason, DateTimeOffset UpdatedAt);

public static class TransportRuleSettingMapping
{
    public static TransportRuleSettingResponse ToResponse(this TransportRuleSetting setting) => new(setting.Id, setting.TenantId, setting.Status, setting.LocationDetailRetentionDays, setting.LocationStalenessThreshold, setting.ChangeReason, setting.UpdatedAt);
}
