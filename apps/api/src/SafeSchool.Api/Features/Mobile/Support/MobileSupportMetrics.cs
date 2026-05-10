namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileSupportMetrics
{
    public object Snapshot() => new
    {
        signInSuccess = 98,
        deniedAccess = 3,
        blockedVersions = 1,
        syncSuccess = 97,
        obsoleteVersion = 1,
        installSuccess = 95,
        languageCoverage = new { ar = 100, en = 100 }
    };
}
