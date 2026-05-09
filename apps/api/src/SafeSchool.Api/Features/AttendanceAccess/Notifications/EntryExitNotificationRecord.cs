using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationRecord : TenantOwnedEntity
{
    public Guid GateScanEventId { get; set; }
    public Guid? AttendanceRecordId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public string GuardianReference { get; set; } = string.Empty;
    public NotificationEligibilityStatus EligibilityStatus { get; set; } = NotificationEligibilityStatus.Eligible;
    public string SuppressionReason { get; set; } = string.Empty;
    public DateTimeOffset? GuardianVisibleAt { get; set; }

    public bool CanTransitionTo(NotificationEligibilityStatus status) => status != NotificationEligibilityStatus.Visible || EligibilityStatus is NotificationEligibilityStatus.Eligible or NotificationEligibilityStatus.Attempted;
}

