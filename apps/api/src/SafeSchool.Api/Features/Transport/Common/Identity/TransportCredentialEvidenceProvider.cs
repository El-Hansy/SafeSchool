using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Common.Identity;

public sealed record TransportCredentialEvidence(string TenantId, string StudentProfileId, string CredentialReference, CredentialEvidenceStatus Status, string CredentialType = "NFC Card");

public interface ITransportCredentialEvidenceProvider
{
    Task<TransportCredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default);
}

public sealed class DefaultTransportCredentialEvidenceProvider : ITransportCredentialEvidenceProvider
{
    public Task<TransportCredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default) =>
        Task.FromResult(new TransportCredentialEvidence(tenantId, "student-1", credentialReference, CredentialEvidenceStatus.Active));
}
