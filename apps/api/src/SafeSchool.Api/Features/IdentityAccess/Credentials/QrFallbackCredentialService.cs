using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class QrFallbackCredentialService(SafeSchoolDbContext dbContext, ITenantContext tenantContext, CredentialAuditAdapter auditAdapter)
{
    public async Task<CredentialResponse> CreateAsync(string tenantId, Guid studentProfileId, CreateQrCredentialRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ValidUntil <= request.ValidFrom)
        {
            throw new InvalidOperationException("QR fallback credentials require a valid time window.");
        }

        var credential = new IdentityCredential
        {
            TenantId = tenantId,
            StudentProfileId = studentProfileId,
            CredentialType = CredentialType.QrFallback,
            CredentialReference = request.QrReference,
            CredentialStatus = CredentialStatus.Active,
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            IssuedBy = tenantContext.ActorReference ?? "unknown",
            StatusReason = request.RotationReason
        };
        dbContext.IdentityCredentials.Add(credential);
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.QrFallbackCredentials.Add(new QrFallbackCredential { TenantId = tenantId, IdentityCredentialId = credential.Id, QrReference = request.QrReference, RotationSequence = 1, RotationReason = request.RotationReason });
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(credential, credential.IssuedBy, "identity.credentials.create_qr", request.RotationReason, cancellationToken);
        return CredentialLifecycleService.ToResponse(credential);
    }
}
