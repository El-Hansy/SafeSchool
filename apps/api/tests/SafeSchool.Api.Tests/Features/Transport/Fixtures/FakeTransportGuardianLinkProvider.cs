using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public sealed class FakeTransportGuardianLinkProvider : ITransportGuardianLinkProvider
{
    private readonly Dictionary<string, TransportGuardianLinkEvidence> _items = [];
    public void Add(TransportGuardianLinkEvidence evidence) => _items[$"{evidence.TenantId}:{evidence.StudentProfileId}"] = evidence;
    public Task<TransportGuardianLinkEvidence> GetLinkAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_items.GetValueOrDefault($"{tenantId}:{studentProfileId}", new TransportGuardianLinkEvidence(tenantId, guardianReference, studentProfileId, GuardianLinkStatus.Approved, true)));
}
