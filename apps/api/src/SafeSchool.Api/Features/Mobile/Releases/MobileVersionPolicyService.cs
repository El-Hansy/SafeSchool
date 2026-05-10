namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileVersionPolicyService(ApkReleaseRepository repository)
{
    public InstallEventResult Evaluate(int versionCode) => versionCode < repository.Current().MinimumSupportedVersionCode ? InstallEventResult.UpdateRequired : InstallEventResult.Allowed;
}
