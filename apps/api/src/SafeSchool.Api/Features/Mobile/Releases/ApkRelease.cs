namespace SafeSchool.Api.Features.Mobile;

public sealed class ApkRelease
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VersionName { get; init; } = "1.0.0";
    public int VersionCode { get; init; } = 12;
    public ApkEnvironment Environment { get; init; } = ApkEnvironment.Pilot;
    public ApkReleaseStatus ReleaseStatus { get; init; } = ApkReleaseStatus.Active;
    public string ArtifactUri { get; init; } = "s3://safeschool-mobile/demo/safeschool-1.0.0.apk";
    public string ArtifactChecksum { get; init; } = "demo-checksum";
    public string ReleaseNotesKey { get; init; } = "release.phase12";
    public int MinimumSupportedVersionCode { get; init; } = 12;
    public string SupportContact { get; init; } = "support@safeschool.local";
    public string ApprovedByUserId { get; init; } = "release-operator";
    public DateTimeOffset? ApprovedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}

public sealed class ReleaseAudience
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ReleaseId { get; init; }
    public string TenantId { get; init; } = "school-demo";
    public ReleaseAudienceType AudienceType { get; init; } = ReleaseAudienceType.Tenant;
    public string AudienceRef { get; init; } = "school-demo";
    public ReleaseAudienceStatus AudienceStatus { get; init; } = ReleaseAudienceStatus.Active;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
