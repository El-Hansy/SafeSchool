using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public sealed class FakeWalletCredentialEvidenceProvider : IWalletCredentialEvidenceProvider
{
    public WalletCredentialEvidenceStatus Status { get; set; } = WalletCredentialEvidenceStatus.Active;
    public Task<WalletCredentialEvidence> GetAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default) => Task.FromResult(new WalletCredentialEvidence(credentialReference, "student-1", tenantId, Status, "NFC"));
}
