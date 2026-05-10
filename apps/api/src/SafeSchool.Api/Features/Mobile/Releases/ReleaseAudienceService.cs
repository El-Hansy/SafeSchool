namespace SafeSchool.Api.Features.Mobile;

public sealed class ReleaseAudienceService
{
    public bool IsInAudience(string tenantId, string? roleCode) => tenantId == "school-demo" && (roleCode is null || MobileRoleCodes.All.Contains(roleCode));
}
