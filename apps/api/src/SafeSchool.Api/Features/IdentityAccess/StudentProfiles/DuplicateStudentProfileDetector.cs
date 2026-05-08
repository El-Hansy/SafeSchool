using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed class DuplicateStudentProfileDetector(SafeSchoolDbContext dbContext)
{
    public async Task<DuplicateCheckResponse> CheckAsync(string tenantId, DuplicateCheckRequest request, CancellationToken cancellationToken = default)
    {
        var identifiers = request.ExternalIdentityReferences.Select(x => $"{x.ReferenceType}:{x.ReferenceValue}").ToHashSet(StringComparer.OrdinalIgnoreCase);
        var activeProfiles = await dbContext.StudentProfiles
            .Where(x => x.TenantId == tenantId && x.ProfileStatus == Common.ProfileStatus.Active)
            .ToListAsync(cancellationToken);
        var candidates = activeProfiles
            .Where(x => x.SchoolStudentNumber == request.SchoolStudentNumber || x.ExternalIdentityReferences.Any(identifiers.Contains))
            .Select(x => x.Id)
            .ToList();

        return candidates.Count == 0
            ? new(DuplicateReviewStatus.Clear, [], "No active duplicate identifiers found.")
            : new(DuplicateReviewStatus.PossibleDuplicate, candidates, "Active identity identifiers require review before activation.");
    }
}
