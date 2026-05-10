using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class GuardianTransportNotificationVisibilityService(SafeSchoolDbContext dbContext, ITransportGuardianLinkProvider guardianLinkProvider)
{
    public async Task<OperationResult<GuardianTransportNotificationListResponse>> ListAsync(string tenantId, string guardianRecordId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinkProvider.GetLinkAsync(tenantId, guardianRecordId, studentProfileId, cancellationToken);
        if (link.Status != GuardianLinkStatus.Approved || !link.HasTransportVisibilityScope)
        {
            return OperationResult<GuardianTransportNotificationListResponse>.Failure(new ValidationError("guardian_scope_denied", "Guardian cannot view these transport notifications."));
        }

        var records = await dbContext.TransportNotificationRecords.Where(x => x.TenantId == tenantId && x.GuardianRecordId == guardianRecordId && x.StudentProfileId == studentProfileId && x.NotificationStatus == Common.NotificationStatus.Visible).OrderByDescending(x => x.CreatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
        return OperationResult<GuardianTransportNotificationListResponse>.Success(new GuardianTransportNotificationListResponse(studentProfileId, records));
    }
}
