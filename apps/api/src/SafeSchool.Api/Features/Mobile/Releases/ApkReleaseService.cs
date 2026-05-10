namespace SafeSchool.Api.Features.Mobile;

public sealed class ApkReleaseService(ApkReleaseRepository repository, MobileFeatureAvailabilityService features, MobileAuditService audit)
{
    public ApkReleaseDto Current(string tenantId, string? roleCode, int? versionCode)
    {
        if (!features.IsApkReleaseEnabled(tenantId))
        {
            return new ApkReleaseDto("none", "0.0.0", 0, "Pilot", "Disabled", false, false, "Mobile release disabled.", "support@safeschool.local", "", null);
        }

        var release = repository.Current();
        var updateRequired = versionCode is not null && versionCode < release.MinimumSupportedVersionCode;
        return ToDto(release, !updateRequired, updateRequired);
    }

    public ApkReleaseDto Create(CreateApkReleaseRequest request)
    {
        var release = repository.Add(new ApkRelease
        {
            VersionName = request.VersionName,
            VersionCode = request.VersionCode,
            Environment = Enum.TryParse<ApkEnvironment>(request.Environment, true, out var env) ? env : ApkEnvironment.Pilot,
            ReleaseStatus = ApkReleaseStatus.Draft,
            ArtifactUri = request.ArtifactUri,
            ArtifactChecksum = request.ArtifactChecksum,
            ReleaseNotesKey = request.ReleaseNotes,
            SupportContact = request.SupportContact,
            MinimumSupportedVersionCode = request.MinimumSupportedVersionCode
        });
        audit.Record("school-demo", "release-operator", "admin-web", "mobile.release.created", MobileRoleCodes.PlatformSupport, "mobile.release.manage", "created", "ok", "apk_release", release.Id.ToString("N"));
        return ToDto(release, false, false);
    }

    public ApkReleaseDto Approve(string releaseId, ApproveReleaseRequest request) => ToDto(repository.Current(), true, false) with { ReleaseStatus = "Active" };
    public ApkReleaseDto Revoke(string releaseId, RevokeReleaseRequest request) => ToDto(repository.Current(), false, true) with { ReleaseStatus = "Revoked" };

    public static ApkReleaseDto ToDto(ApkRelease release, bool downloadAllowed, bool updateRequired) =>
        new(release.Id.ToString("N"), release.VersionName, release.VersionCode, release.Environment.ToString(), release.ReleaseStatus.ToString(), downloadAllowed, updateRequired, release.ReleaseNotesKey, release.SupportContact, release.ArtifactChecksum, DateTimeOffset.UtcNow.AddDays(30));
}
