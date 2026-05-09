using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationEligibilityService(IGuardianLinkEligibilityProvider guardianLinkEligibilityProvider)
{
    public async Task<(NotificationEligibilityStatus Status, string Reason)> EvaluateAsync(string tenantId, string guardianReference, GateScanEvent scanEvent, CancellationToken cancellationToken = default)
    {
        if (scanEvent.Status != ScanEventStatus.Accepted)
        {
            return (NotificationEligibilityStatus.Suppressed, "Scan is not accepted.");
        }

        var eligibility = await guardianLinkEligibilityProvider.CheckAsync(tenantId, guardianReference, scanEvent.StudentProfileId, cancellationToken);
        return eligibility.IsEligible
            ? (NotificationEligibilityStatus.Visible, eligibility.Reason)
            : (NotificationEligibilityStatus.Suppressed, eligibility.Reason);
    }
}

