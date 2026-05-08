using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianVisibilityService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<Guid>> VisibleStudentIdsAsync(string tenantId, Guid guardianId, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        return await dbContext.GuardianLinks
            .Where(x => x.TenantId == tenantId && x.GuardianId == guardianId)
            .Where(x => x.LinkStatus == Common.GuardianLinkStatus.Approved && x.ValidFrom <= now && (x.ValidUntil == null || x.ValidUntil > now))
            .Where(x => x.AccessScope.ContainsKey("student_profile"))
            .Select(x => x.StudentProfileId)
            .ToListAsync(cancellationToken);
    }
}
