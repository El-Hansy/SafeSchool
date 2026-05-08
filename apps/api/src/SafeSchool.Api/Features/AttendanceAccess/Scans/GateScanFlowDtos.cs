using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public sealed record CreateGateRequest(string GateCode, string DisplayName, string CampusReference, bool AllowsEntry = true, bool AllowsExit = true, bool OfflineAllowed = true);
public sealed record UpdateGateRequest(string? DisplayName, GateStatus? Status, bool? AllowsEntry, bool? AllowsExit, bool? OfflineAllowed, string Reason);
public sealed record GateResponse(Guid GateId, string SchoolAccountId, string GateCode, string DisplayName, string CampusReference, GateStatus Status, bool AllowsEntry, bool AllowsExit, bool OfflineAllowed);

public sealed record CreateScanPointRequest(Guid GateId, string DeviceReference, string AssignedActorReference, bool OfflineAllowed = true);
public sealed record UpdateScanPointRequest(ScanPointStatus? Status, bool? OfflineAllowed, string Reason);
public sealed record ScanPointResponse(Guid ScanPointId, Guid GateId, string DeviceReference, string AssignedActorReference, ScanPointStatus Status, bool OfflineAllowed);

public sealed record RecordScanRequest(Guid GateId, Guid ScanPointId, string CredentialReference, AttendanceDirection Direction, ScanMethod Method, string ClientScanId, DateTimeOffset LocalScanTime);
public sealed record OfflineScanBatchRequest(string ClientBatchId, IReadOnlyList<RecordScanRequest> Scans);
public sealed record ScanEventResponse(Guid ScanEventId, string SchoolAccountId, string StudentProfileId, string CredentialReference, AttendanceDirection Direction, ScanMethod Method, ScanEventStatus Status, DateTimeOffset LocalScanTime, DateTimeOffset ReceivedAt);
public sealed record CampusAccessDecisionResponse(Guid CampusAccessDecisionId, Guid ScanEventId, CampusAccessDecisionOutcome Decision, string DecisionReason, CampusState CampusStateAfter);
public sealed record OfflineSyncResponse(string ClientBatchId, OfflineSyncStatus Status, IReadOnlyList<ScanEventResponse> Items);
public sealed record ScanTraceResponse(Guid ScanEventId, Guid? CampusAccessDecisionId, Guid? AttendanceRecordId, Guid? NotificationRecordId, Guid? AnomalyId, Guid? ManualReviewId, IReadOnlyList<string> AuditReferences);

public sealed class GateScanEvent : TenantOwnedEntity
{
    public Guid GateId { get; set; }
    public Guid ScanPointId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public string CredentialReference { get; set; } = string.Empty;
    public AttendanceDirection Direction { get; set; }
    public ScanMethod Method { get; set; }
    public ScanEventStatus Status { get; set; }
    public string ClientScanId { get; set; } = string.Empty;
    public string ClientBatchId { get; set; } = string.Empty;
    public DateTimeOffset LocalScanTime { get; set; }
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public string DecisionReason { get; set; } = string.Empty;

    public ScanEventResponse ToResponse() => new(Id, TenantId, StudentProfileId, CredentialReference, Direction, Method, Status, LocalScanTime, ReceivedAt);
}

public sealed class OfflineSyncBatch : TenantOwnedEntity
{
    public string ClientBatchId { get; set; } = string.Empty;
    public OfflineSyncStatus Status { get; set; } = OfflineSyncStatus.Pending;
    public int ScanCount { get; set; }
}

public sealed class CampusAccessDecision : TenantOwnedEntity
{
    public Guid GateScanEventId { get; set; }
    public CampusAccessDecisionOutcome Decision { get; set; }
    public string DecisionReason { get; set; } = string.Empty;
    public string FeatureCapabilityKey { get; set; } = string.Empty;
    public string PermissionKey { get; set; } = string.Empty;
    public CredentialEvidenceStatus CredentialStatusUsed { get; set; }
    public CampusState CampusStateAfter { get; set; }
    public DateTimeOffset DecidedAt { get; set; } = DateTimeOffset.UtcNow;

    public CampusAccessDecisionResponse ToResponse() => new(Id, GateScanEventId, Decision, DecisionReason, CampusStateAfter);
}

