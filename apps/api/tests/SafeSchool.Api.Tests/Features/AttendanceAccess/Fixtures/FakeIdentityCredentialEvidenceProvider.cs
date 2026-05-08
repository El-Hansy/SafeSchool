using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;

public sealed class FakeIdentityCredentialEvidenceProvider : IIdentityCredentialEvidenceProvider
{
    private readonly Dictionary<string, CredentialEvidence> _evidence = [];

    public void Add(CredentialEvidence evidence) => _evidence[evidence.CredentialReference] = evidence;

    public Task<CredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default) =>
        Task.FromResult(_evidence.TryGetValue(credentialReference, out var evidence)
            ? evidence
            : new CredentialEvidence(tenantId, string.Empty, credentialReference, CredentialEvidenceStatus.Unknown));
}

