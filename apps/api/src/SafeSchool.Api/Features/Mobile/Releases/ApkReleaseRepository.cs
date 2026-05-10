namespace SafeSchool.Api.Features.Mobile;

public sealed class ApkReleaseRepository
{
    private readonly List<ApkRelease> _releases = [MobileSeedCatalog.ActiveRelease];
    public IReadOnlyList<ApkRelease> Releases => _releases;
    public ApkRelease Current() => _releases.OrderByDescending(release => release.VersionCode).First(release => release.ReleaseStatus == ApkReleaseStatus.Active);
    public ApkRelease Add(ApkRelease release) { _releases.Add(release); return release; }
}
