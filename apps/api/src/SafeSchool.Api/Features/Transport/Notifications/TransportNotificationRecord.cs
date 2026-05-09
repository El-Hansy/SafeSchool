using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationRecord : TenantOwnedEntity
{
    public string GuardianRecordId { get; set; } = string.Empty;
    public string GuardianLinkId { get; set; } = string.Empty;
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid? TransportTripId { get; set; }
    public TransportEventType EventType { get; set; } = TransportEventType.Boarding;
    public string SourceEventReference { get; set; } = string.Empty;
    public NotificationStatus NotificationStatus { get; set; } = NotificationStatus.Created;
    public string SuppressionReason { get; set; } = string.Empty;
    public string VisibleStatus { get; set; } = string.Empty;
    public string ClientEventId { get; set; } = string.Empty;

    public bool IsGuardianVisible() => NotificationStatus == NotificationStatus.Visible && string.IsNullOrWhiteSpace(SuppressionReason);
    public bool CanWithdraw() => NotificationStatus is NotificationStatus.Visible or NotificationStatus.Created;
}
