using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianRecord : TenantOwnedEntity
{
    public required string DisplayName { get; set; }
    public string[] ContactMethods { get; set; } = [];
    public GuardianIdentityReviewStatus IdentityReviewStatus { get; set; } = GuardianIdentityReviewStatus.Unverified;
    public GuardianStatus GuardianStatus { get; set; } = GuardianStatus.Active;
    public required string CreatedBy { get; set; }
    public required string UpdatedBy { get; set; }
}
