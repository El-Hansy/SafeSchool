namespace SafeSchool.Api.Features.Mobile;

public sealed class SourceDomainActionAdapters(MobileAuditService audit)
{
    private static readonly string[] SupportedSources = ["guardian", "student", "transport", "attendance_access", "wallet", "learning", "medical", "complaints", "communications", "documents", "administration", "support"];

    public MobileActionResponse Execute(string tenantId, string userId, string deviceId, string roleCode, MobileActionRequest request)
    {
        if (!SupportedSources.Contains(request.SourceFeatureCode))
        {
            var denied = audit.Record(tenantId, userId, deviceId, "mobile.action.denied", roleCode, request.SourceFeatureCode, "blocked", "SOURCE_DOMAIN_REJECTED", request.SourceFeatureCode, request.TargetId);
            return new MobileActionResponse("Rejected", request.TargetId, denied.Id.ToString("N"), false, "Source domain rejected the mobile action.");
        }

        var evt = audit.Record(tenantId, userId, deviceId, "mobile.action.accepted", roleCode, request.SourceFeatureCode, "allowed", "ok", request.SourceFeatureCode, request.TargetId);
        var queued = request.SourceFeatureCode is "transport" or "attendance_access" or "wallet" or "medical";
        return new MobileActionResponse("Accepted", $"{request.SourceFeatureCode}:{request.TargetId}", evt.Id.ToString("N"), queued, "Action routed through owning source workflow.");
    }
}
