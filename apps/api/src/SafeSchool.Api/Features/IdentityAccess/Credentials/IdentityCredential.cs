using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class IdentityCredential : TenantOwnedEntity
{
    public Guid StudentProfileId { get; set; }
    public CredentialType CredentialType { get; set; }
    public required string CredentialReference { get; set; }
    public CredentialStatus CredentialStatus { get; set; } = CredentialStatus.Proposed;
    public required string IssuedBy { get; set; }
    public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ValidFrom { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ValidUntil { get; set; }
    public Guid? ReplacedByCredentialId { get; set; }
    public required string StatusReason { get; set; }

    public bool IsCurrent(DateTimeOffset now) => CredentialStatus == CredentialStatus.Active
        && ValidFrom <= now
        && (ValidUntil is null || ValidUntil > now);
}
