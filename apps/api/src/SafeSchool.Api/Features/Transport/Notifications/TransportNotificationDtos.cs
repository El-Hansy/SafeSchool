using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Trace;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed record TransportNotificationRecordResponse(Guid TransportNotificationRecordId, string SchoolAccountId, string GuardianRecordId, string StudentProfileId, Guid? TransportTripId, TransportEventType EventType, string SourceEventReference, NotificationStatus NotificationStatus, string SuppressionReason, string VisibleStatus, DateTimeOffset UpdatedAt);
public sealed record GuardianTransportNotificationListResponse(string StudentProfileId, IReadOnlyList<TransportNotificationRecordResponse> Notifications);
public sealed record WithdrawNotificationRequest(string WithdrawalReason, string ReplacementVisibleStatus, string ClientRequestId);
public sealed record TransportNotificationTraceResponse(Guid NotificationRecordId, IReadOnlyList<TransportTraceReference> References);

public static class TransportNotificationMapping
{
    public static TransportNotificationRecordResponse ToResponse(this TransportNotificationRecord record) => new(record.Id, record.TenantId, record.GuardianRecordId, record.StudentProfileId, record.TransportTripId, record.EventType, record.SourceEventReference, record.NotificationStatus, record.SuppressionReason, record.VisibleStatus, record.UpdatedAt);
}
