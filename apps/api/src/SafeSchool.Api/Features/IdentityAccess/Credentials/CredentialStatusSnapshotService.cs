using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class CredentialStatusSnapshotService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<CredentialStatusSnapshotResponse>> CurrentAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        return await dbContext.IdentityCredentials
            .Where(x => x.TenantId == tenantId && x.CredentialStatus == Common.CredentialStatus.Active && x.ValidFrom <= now && (x.ValidUntil == null || x.ValidUntil > now))
            .Select(x => new CredentialStatusSnapshotResponse(Guid.NewGuid(), x.TenantId, x.StudentProfileId, x.Id, x.CredentialType.ToString(), x.CredentialStatus.ToString(), x.ValidFrom, x.ValidUntil, null, now, now.AddHours(1)))
            .ToListAsync(cancellationToken);
    }
}
