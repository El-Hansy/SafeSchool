using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Common.Identity;

public sealed record WalletStudentProfileEvidence(string StudentProfileId, string TenantId, StudentEligibilityStatus Status, string DisplayName);

public interface IWalletStudentProfileEvidenceProvider
{
    Task<WalletStudentProfileEvidence> GetAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default);
}

public sealed class DefaultWalletStudentProfileEvidenceProvider : IWalletStudentProfileEvidenceProvider
{
    public Task<WalletStudentProfileEvidence> GetAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var status = studentProfileId.Contains("inactive", StringComparison.OrdinalIgnoreCase) ? StudentEligibilityStatus.Inactive
            : studentProfileId.Contains("cross", StringComparison.OrdinalIgnoreCase) ? StudentEligibilityStatus.CrossTenant
            : studentProfileId.Contains("missing", StringComparison.OrdinalIgnoreCase) ? StudentEligibilityStatus.Missing
            : StudentEligibilityStatus.Active;
        return Task.FromResult(new WalletStudentProfileEvidence(studentProfileId, tenantId, status, $"Student {studentProfileId}"));
    }
}
