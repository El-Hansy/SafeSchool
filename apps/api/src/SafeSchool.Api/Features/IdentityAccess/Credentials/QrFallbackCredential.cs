using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class QrFallbackCredential : TenantOwnedEntity
{
    public Guid IdentityCredentialId { get; set; }
    public required string QrReference { get; set; }
    public int RotationSequence { get; set; }
    public required string RotationReason { get; set; }
    public DateTimeOffset? LastPresentedAt { get; set; }
}
