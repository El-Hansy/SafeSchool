using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Rules;

public sealed class TransportRuleSetting : TenantOwnedEntity
{
    public string AssignmentEligibilityPolicy { get; set; } = "ActiveStudentAndActiveRouteOnly";
    public TimeSpan PickupWindow { get; set; } = TimeSpan.FromMinutes(20);
    public TimeSpan DropWindow { get; set; } = TimeSpan.FromMinutes(20);
    public int RouteDeviationThresholdMeters { get; set; } = 500;
    public TimeSpan LocationStalenessThreshold { get; set; } = TimeSpan.FromMinutes(2);
    public TimeSpan EtaChangeThreshold { get; set; } = TimeSpan.FromMinutes(5);
    public string NotificationEligibilityPolicy { get; set; } = "AcceptedOrReviewedEventsOnly";
    public TimeSpan ScanClockDriftTolerance { get; set; } = TimeSpan.FromMinutes(3);
    public string RetryHandlingPolicy { get; set; } = "IdempotentClientIdentifiers";
    public int LocationDetailRetentionDays { get; set; } = 30;
    public FeatureRuleStatus Status { get; set; } = FeatureRuleStatus.Draft;
    public string ChangeReason { get; set; } = string.Empty;

    public bool HasValidRetention() => LocationDetailRetentionDays == 30;
    public bool CanActivate() => PickupWindow > TimeSpan.Zero && DropWindow > TimeSpan.Zero && LocationStalenessThreshold > TimeSpan.Zero && HasValidRetention();
}
