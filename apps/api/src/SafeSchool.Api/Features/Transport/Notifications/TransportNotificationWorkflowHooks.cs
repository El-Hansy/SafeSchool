using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationWorkflowHooks(TransportNotificationService notificationService)
{
    public Task<TransportNotificationRecordResponse> BoardingAcceptedAsync(string tenantId, string guardianReference, string studentProfileId, string sourceReference, Guid tripId, CancellationToken cancellationToken = default) =>
        notificationService.CreateAsync(tenantId, guardianReference, studentProfileId, TransportEventType.Boarding, sourceReference, tripId, true, cancellationToken);

    public Task<TransportNotificationRecordResponse> MaterialEtaChangedAsync(string tenantId, string guardianReference, string studentProfileId, string sourceReference, Guid tripId, CancellationToken cancellationToken = default) =>
        notificationService.CreateAsync(tenantId, guardianReference, studentProfileId, TransportEventType.MaterialEtaChange, sourceReference, tripId, true, cancellationToken);
}
