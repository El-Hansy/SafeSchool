using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class CredentialStatusSnapshot : TenantOwnedEntity
{
    public Guid IdentityCredentialId { get; set; }
    public Guid StudentProfileId { get; set; }
    public CredentialType CredentialType { get; set; }
    public CredentialStatus CredentialStatus { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidUntil { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public DateTimeOffset SnapshotGeneratedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset SnapshotExpiresAt { get; set; } = DateTimeOffset.UtcNow.AddHours(1);
}
