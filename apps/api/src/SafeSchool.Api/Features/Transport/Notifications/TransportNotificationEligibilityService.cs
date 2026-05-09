using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed record TransportNotificationEligibility(bool Eligible, string Reason);

public sealed class TransportNotificationEligibilityService(ITransportGuardianLinkProvider guardianLinkProvider)
{
    public async Task<TransportNotificationEligibility> EvaluateAsync(string tenantId, string guardianReference, string studentProfileId, TransportEventType eventType, bool sourceAcceptedOrReviewed, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinkProvider.GetLinkAsync(tenantId, guardianReference, studentProfileId, cancellationToken);
        if (link.Status != GuardianLinkStatus.Approved || !link.HasTransportVisibilityScope) return new(false, "Guardian link is not approved for transport visibility.");
        if (!sourceAcceptedOrReviewed) return new(false, "Source evidence is unresolved or denied.");
        return new(true, $"Eligible {eventType} notification.");
    }
}
