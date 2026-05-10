namespace SafeSchool.Api.Features.Mobile;

public sealed class ApkArtifactStorage
{
    public bool HasIntegrityEvidence(ApkRelease release) => !string.IsNullOrWhiteSpace(release.ArtifactUri) && !string.IsNullOrWhiteSpace(release.ArtifactChecksum);
}
