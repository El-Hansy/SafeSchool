using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class WalletPosCredentialValidationService(IWalletCredentialEvidenceProvider credentials)
{
    public async Task<OperationResult<WalletCredentialEvidence>> ValidateAsync(string tenantId, string credentialReference, CancellationToken cancellationToken = default)
    {
        var evidence = await credentials.GetAsync(tenantId, credentialReference, cancellationToken);
        return evidence.Status == WalletCredentialEvidenceStatus.Active
            ? OperationResult<WalletCredentialEvidence>.Success(evidence)
            : OperationResult<WalletCredentialEvidence>.Failure(new ValidationError("credential_not_active", "Credential is not active for wallet purchases.", nameof(credentialReference)));
    }
}
