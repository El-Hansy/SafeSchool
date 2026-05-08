namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public sealed record GuardianLinkEligibility(
    string TenantId,
    string GuardianReference,
    string StudentProfileId,
    bool IsEligible,
    string Reason);

public interface IGuardianLinkEligibilityProvider
{
    Task<GuardianLinkEligibility> CheckAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default);
}

public sealed class DefaultGuardianLinkEligibilityProvider : IGuardianLinkEligibilityProvider
{
    public Task<GuardianLinkEligibility> CheckAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var eligible = !guardianReference.Contains("blocked", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(new GuardianLinkEligibility(tenantId, guardianReference, studentProfileId, eligible, eligible ? "approved active link" : "guardian link not eligible"));
    }
}

