using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class NfcCardCredential : TenantOwnedEntity
{
    public Guid IdentityCredentialId { get; set; }
    public required string CardReference { get; set; }
    public string? CardLabel { get; set; }
    public required string ProvisioningStatus { get; set; }
    public DateTimeOffset? LastVerifiedAt { get; set; }
}
