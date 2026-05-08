using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationWorkflowHooks(EntryExitNotificationService notificationService)
{
    public Task<EntryExitNotificationRecord> CreateForGuardianAsync(string tenantId, string guardianReference, GateScanEvent scanEvent, CancellationToken cancellationToken = default) =>
        notificationService.CreateFromScanAsync(tenantId, guardianReference, scanEvent, cancellationToken);
}

