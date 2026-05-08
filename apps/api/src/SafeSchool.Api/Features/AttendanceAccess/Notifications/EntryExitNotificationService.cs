using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationService(SafeSchoolDbContext dbContext, EntryExitNotificationEligibilityService eligibilityService)
{
    public async Task<EntryExitNotificationRecord> CreateFromScanAsync(string tenantId, string guardianReference, GateScanEvent scanEvent, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.EntryExitNotificationRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.GateScanEventId == scanEvent.Id && x.GuardianReference == guardianReference, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var eligibility = await eligibilityService.EvaluateAsync(tenantId, guardianReference, scanEvent, cancellationToken);
        var record = new EntryExitNotificationRecord
        {
            TenantId = tenantId,
            GateScanEventId = scanEvent.Id,
            StudentProfileId = scanEvent.StudentProfileId,
            GuardianReference = guardianReference,
            EligibilityStatus = eligibility.Status,
            SuppressionReason = eligibility.Status == NotificationEligibilityStatus.Suppressed ? eligibility.Reason : string.Empty,
            GuardianVisibleAt = eligibility.Status == NotificationEligibilityStatus.Visible ? DateTimeOffset.UtcNow : null
        };
        dbContext.EntryExitNotificationRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);
        return record;
    }

    public async Task<OperationResult<NotificationRecordResponse>> WithdrawAsync(string tenantId, Guid notificationId, WithdrawNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var record = await dbContext.EntryExitNotificationRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == notificationId, cancellationToken);
        if (record is null)
        {
            return OperationResult<NotificationRecordResponse>.Failure(new ValidationError("not_found", "Notification record was not found."));
        }

        record.EligibilityStatus = NotificationEligibilityStatus.Withdrawn;
        record.SuppressionReason = request.Reason;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<NotificationRecordResponse>.Success(record.ToResponse());
    }
}

