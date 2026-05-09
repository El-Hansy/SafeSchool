using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed record NotificationRecordResponse(Guid NotificationRecordId, Guid GateScanEventId, string StudentProfileId, string GuardianReference, NotificationEligibilityStatus EligibilityStatus, string SuppressionReason, DateTimeOffset? GuardianVisibleAt);
public sealed record WithdrawNotificationRequest(string Reason);

public static class NotificationMappings
{
    public static NotificationRecordResponse ToResponse(this EntryExitNotificationRecord record) =>
        new(record.Id, record.GateScanEventId, record.StudentProfileId, record.GuardianReference, record.EligibilityStatus, record.SuppressionReason, record.GuardianVisibleAt);
}

