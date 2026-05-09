using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class GuardianTransportNotificationVisibilityService(SafeSchoolDbContext dbContext)
{
    public async Task<GuardianTransportNotificationListResponse> ListAsync(string tenantId, string guardianRecordId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.TransportNotificationRecords.Where(x => x.TenantId == tenantId && x.GuardianRecordId == guardianRecordId && x.StudentProfileId == studentProfileId && x.NotificationStatus == Common.NotificationStatus.Visible).OrderByDescending(x => x.CreatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
        return new GuardianTransportNotificationListResponse(studentProfileId, records);
    }
}
