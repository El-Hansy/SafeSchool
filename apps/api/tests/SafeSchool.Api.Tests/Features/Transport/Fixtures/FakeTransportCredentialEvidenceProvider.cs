using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public sealed class FakeTransportCredentialEvidenceProvider : ITransportCredentialEvidenceProvider
{
    private readonly Dictionary<string, TransportCredentialEvidence> _items = [];
    public void Add(TransportCredentialEvidence evidence) => _items[$"{evidence.TenantId}:{evidence.CredentialReference}"] = evidence;
    public Task<TransportCredentialEvidence> GetEvidenceAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default) =>
        Task.FromResult(_items.GetValueOrDefault($"{tenantId}:{credentialReference}", new TransportCredentialEvidence(tenantId, "student-1", credentialReference, CredentialEvidenceStatus.Active)));
}
