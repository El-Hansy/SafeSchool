using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public sealed class FakeWalletGuardianLinkProvider : IWalletGuardianLinkProvider
{
    public WalletGuardianLinkStatus Status { get; set; } = WalletGuardianLinkStatus.Approved;
    public Task<WalletGuardianLinkEvidence> GetAsync(string tenantId, string guardianActorId, string studentProfileId, CancellationToken cancellationToken = default) => Task.FromResult(new WalletGuardianLinkEvidence("guardian-link-1", guardianActorId, studentProfileId, Status));
}
