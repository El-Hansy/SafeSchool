namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileOfflineActionSyncService(OfflinePolicyAdapters policies, MobileAuditService audit)
{
    private readonly Dictionary<string, OfflineActionResponse> _responses = [];

    public OfflineActionResponse Sync(OfflineActionRequest request)
    {
        if (!policies.IsOfflineAllowed(request.SourceFeatureCode))
        {
            var denied = audit.Record(request.TenantId, request.UserId, request.DeviceId, "mobile.offline.rejected", request.ActiveRoleCode, request.SourceFeatureCode, "blocked", "OFFLINE_NOT_SUPPORTED", request.SourceFeatureCode, request.ClientActionId);
            return new OfflineActionResponse(request.ClientActionId, OfflineSyncStatus.Rejected.ToString(), request.SourceFeatureCode, "Offline not supported for this workflow.", denied.Id.ToString("N"), "This action needs an online connection.");
        }

        if (_responses.TryGetValue(request.ClientActionId, out var existing))
        {
            return existing with { SyncStatus = OfflineSyncStatus.Duplicate.ToString(), UserMessage = "Duplicate offline action ignored." };
        }

        var evt = audit.Record(request.TenantId, request.UserId, request.DeviceId, "mobile.offline.accepted", request.ActiveRoleCode, request.SourceFeatureCode, "accepted", "ok", request.SourceFeatureCode, request.ClientActionId);
        var response = new OfflineActionResponse(request.ClientActionId, OfflineSyncStatus.Accepted.ToString(), $"{request.SourceFeatureCode}:{request.ClientActionId}", null, evt.Id.ToString("N"), "Offline action synced.");
        _responses[request.ClientActionId] = response;
        return response;
    }

    public IReadOnlyList<OfflineActionResponse> List() => _responses.Values.ToArray();
}
