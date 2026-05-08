using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class GuardianEntryExitVisibilityService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<NotificationRecordResponse>> VisibleForGuardianAsync(string tenantId, string guardianReference, CancellationToken cancellationToken = default) =>
        await dbContext.EntryExitNotificationRecords
            .Where(x => x.TenantId == tenantId && x.GuardianReference == guardianReference && x.EligibilityStatus == Common.NotificationEligibilityStatus.Visible)
            .OrderByDescending(x => x.GuardianVisibleAt)
            .Select(x => x.ToResponse())
            .ToListAsync(cancellationToken);
}

