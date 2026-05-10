namespace SafeSchool.Api.Features.Mobile;

public sealed class TenantMobileFeatureAvailability
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public bool MobileEnabled { get; init; } = true;
    public bool ApkReleaseEnabled { get; init; } = true;
    public string EnabledFeatureCodes { get; init; } = string.Join(',', MobileFeatureCodes.All);
    public string EnabledWorkspaceCodes { get; init; } = string.Join(',', MobileRoleCodes.All);
    public string ConfigurationStatus { get; init; } = "active";
    public string LastReviewedByUserId { get; init; } = "admin-demo";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
