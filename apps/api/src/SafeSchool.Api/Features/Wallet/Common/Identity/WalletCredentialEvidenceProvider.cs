using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Common.Identity;

public sealed record WalletCredentialEvidence(string CredentialReference, string StudentProfileId, string TenantId, WalletCredentialEvidenceStatus Status, string EvidenceType);

public interface IWalletCredentialEvidenceProvider
{
    Task<WalletCredentialEvidence> GetAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default);
}

public sealed class DefaultWalletCredentialEvidenceProvider : IWalletCredentialEvidenceProvider
{
    public Task<WalletCredentialEvidence> GetAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default)
    {
        var status = credentialReference.Contains("invalid", StringComparison.OrdinalIgnoreCase) ? WalletCredentialEvidenceStatus.Unknown
            : credentialReference.Contains("expired", StringComparison.OrdinalIgnoreCase) ? WalletCredentialEvidenceStatus.Expired
            : credentialReference.Contains("cross", StringComparison.OrdinalIgnoreCase) ? WalletCredentialEvidenceStatus.CrossTenant
            : WalletCredentialEvidenceStatus.Active;
        return Task.FromResult(new WalletCredentialEvidence(credentialReference, "student-1", tenantId, status, credentialReference.StartsWith("qr", StringComparison.OrdinalIgnoreCase) ? "QR" : "NFC"));
    }
}
