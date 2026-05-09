using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Common.Identity;

public sealed record TransportGuardianLinkEvidence(string TenantId, string GuardianRecordId, string StudentProfileId, GuardianLinkStatus Status, bool HasTransportVisibilityScope);

public interface ITransportGuardianLinkProvider
{
    Task<TransportGuardianLinkEvidence> GetLinkAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default);
}

public sealed class DefaultTransportGuardianLinkProvider : ITransportGuardianLinkProvider
{
    public Task<TransportGuardianLinkEvidence> GetLinkAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default) =>
        Task.FromResult(new TransportGuardianLinkEvidence(tenantId, guardianReference, studentProfileId, GuardianLinkStatus.Approved, true));
}
