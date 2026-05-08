namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public sealed record CredentialEvidence(
    string TenantId,
    string StudentProfileId,
    string CredentialReference,
    CredentialEvidenceStatus Status,
    DateTimeOffset? ValidUntil = null);

public interface IIdentityCredentialEvidenceProvider
{
    Task<CredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default);
}

public sealed class DefaultIdentityCredentialEvidenceProvider : IIdentityCredentialEvidenceProvider
{
    public Task<CredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default)
    {
        var status = credentialReference.StartsWith("expired", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.Expired :
            credentialReference.StartsWith("suspended", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.Suspended :
            credentialReference.StartsWith("revoked", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.Revoked :
            credentialReference.StartsWith("replaced", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.Replaced :
            credentialReference.StartsWith("cross", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.CrossTenant :
            credentialReference.StartsWith("unknown", StringComparison.OrdinalIgnoreCase) ? CredentialEvidenceStatus.Unknown :
            CredentialEvidenceStatus.Active;

        return Task.FromResult(new CredentialEvidence(tenantId, $"student-{credentialReference}", credentialReference, status));
    }
}

