using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class CredentialLifecycleService(SafeSchoolDbContext dbContext, ITenantContext tenantContext, CredentialAuditAdapter auditAdapter)
{
    public async Task<CredentialResponse> IssueNfcAsync(string tenantId, Guid studentProfileId, IssueNfcCredentialRequest request, CancellationToken cancellationToken = default)
    {
        var duplicateActive = await dbContext.NfcCardCredentials.AnyAsync(x => x.TenantId == tenantId && x.CardReference == request.CardReference, cancellationToken);
        if (duplicateActive)
        {
            throw new InvalidOperationException("Duplicate active NFC card reference requires review.");
        }

        var credential = new IdentityCredential
        {
            TenantId = tenantId,
            StudentProfileId = studentProfileId,
            CredentialType = CredentialType.NfcCard,
            CredentialReference = request.CardReference,
            CredentialStatus = CredentialStatus.Active,
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            IssuedBy = tenantContext.ActorReference ?? "unknown",
            StatusReason = request.StatusReason
        };
        dbContext.IdentityCredentials.Add(credential);
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.NfcCardCredentials.Add(new NfcCardCredential { TenantId = tenantId, IdentityCredentialId = credential.Id, CardReference = request.CardReference, CardLabel = request.CardLabel, ProvisioningStatus = "Provisioned" });
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(credential, credential.IssuedBy, "identity.credentials.issue_nfc", request.StatusReason, cancellationToken);
        return ToResponse(credential);
    }

    public async Task<CredentialResponse> ChangeStateAsync(string tenantId, Guid credentialId, CredentialStatus status, string reason, CancellationToken cancellationToken = default)
    {
        var credential = await dbContext.IdentityCredentials.SingleAsync(x => x.TenantId == tenantId && x.Id == credentialId, cancellationToken);
        credential.CredentialStatus = status;
        credential.StatusReason = reason;
        credential.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(credential, tenantContext.ActorReference ?? "unknown", $"identity.credentials.{status.ToString().ToLowerInvariant()}", reason, cancellationToken);
        return ToResponse(credential);
    }

    public async Task<IReadOnlyList<CredentialResponse>> ListForStudentAsync(string tenantId, Guid studentProfileId, CancellationToken cancellationToken = default) =>
        await dbContext.IdentityCredentials
            .Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId)
            .OrderByDescending(x => x.IssuedAt)
            .Select(x => ToResponse(x))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CredentialResponse>> HistoryAsync(string tenantId, Guid credentialId, CancellationToken cancellationToken = default) =>
        await dbContext.IdentityCredentials
            .Where(x => x.TenantId == tenantId && (x.Id == credentialId || x.ReplacedByCredentialId == credentialId))
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => ToResponse(x))
            .ToListAsync(cancellationToken);

    public static CredentialResponse ToResponse(IdentityCredential credential) => new(credential.Id, credential.TenantId, credential.StudentProfileId, credential.CredentialType.ToString(), credential.CredentialReference, credential.CredentialStatus.ToString(), credential.ValidFrom, credential.ValidUntil, credential.IssuedAt, credential.StatusReason);
}
