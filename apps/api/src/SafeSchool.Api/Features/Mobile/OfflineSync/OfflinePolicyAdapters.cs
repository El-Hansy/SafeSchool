namespace SafeSchool.Api.Features.Mobile;

public sealed class OfflinePolicyAdapters
{
    private static readonly string[] OfflineSources = ["attendance_access", "transport", "wallet", "medical"];
    public bool IsOfflineAllowed(string sourceFeatureCode) => OfflineSources.Contains(sourceFeatureCode);
}
