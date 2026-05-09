using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Wallet.Fixtures;

public sealed class FakeStudentProfileEvidenceProvider : IWalletStudentProfileEvidenceProvider
{
    public StudentEligibilityStatus Status { get; set; } = StudentEligibilityStatus.Active;
    public Task<WalletStudentProfileEvidence> GetAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) => Task.FromResult(new WalletStudentProfileEvidence(studentProfileId, tenantId, Status, "Test Student"));
}
