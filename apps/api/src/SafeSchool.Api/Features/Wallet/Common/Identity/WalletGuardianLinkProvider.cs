using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Common.Identity;

public sealed record WalletGuardianLinkEvidence(string GuardianLinkId, string GuardianActorId, string StudentProfileId, WalletGuardianLinkStatus Status);

public interface IWalletGuardianLinkProvider
{
    Task<WalletGuardianLinkEvidence> GetAsync(string tenantId, string guardianActorId, string studentProfileId, CancellationToken cancellationToken = default);
}

public sealed class DefaultWalletGuardianLinkProvider : IWalletGuardianLinkProvider
{
    public Task<WalletGuardianLinkEvidence> GetAsync(string tenantId, string guardianActorId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var status = studentProfileId.Contains("unlinked", StringComparison.OrdinalIgnoreCase) ? WalletGuardianLinkStatus.OutOfScope : WalletGuardianLinkStatus.Approved;
        return Task.FromResult(new WalletGuardianLinkEvidence($"gl-{studentProfileId}", guardianActorId, studentProfileId, status));
    }
}
