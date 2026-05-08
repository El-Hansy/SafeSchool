using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationWithdrawalService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<TransportNotificationRecordResponse>> WithdrawAsync(string tenantId, Guid notificationRecordId, WithdrawNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var record = await dbContext.TransportNotificationRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == notificationRecordId, cancellationToken);
        if (record is null) return OperationResult<TransportNotificationRecordResponse>.Failure(new ValidationError("not_found", "Notification record was not found."));
        if (!record.CanWithdraw()) return OperationResult<TransportNotificationRecordResponse>.Failure(new ValidationError("invalid_transition", "Notification cannot be withdrawn."));
        record.NotificationStatus = Common.NotificationStatus.Withdrawn;
        record.VisibleStatus = request.ReplacementVisibleStatus;
        record.SuppressionReason = request.WithdrawalReason;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<TransportNotificationRecordResponse>.Success(record.ToResponse());
    }
}
