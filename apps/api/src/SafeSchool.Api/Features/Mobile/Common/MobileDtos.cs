namespace SafeSchool.Api.Features.Mobile;

public static class MobileRoleCodes
{
    public const string Guardian = "guardian";
    public const string Student = "student";
    public const string TransportDriver = "transport_driver";
    public const string GateAccess = "gate_access";
    public const string CanteenCashier = "canteen_cashier";
    public const string Teacher = "teacher";
    public const string MedicalStaff = "medical_staff";
    public const string ComplaintHandler = "complaint_handler";
    public const string CommunicationSender = "communication_sender";
    public const string DocumentAdministrator = "document_administrator";
    public const string SchoolAdministrator = "school_administrator";
    public const string PlatformSupport = "platform_support";

    public static readonly string[] All =
    [
        Guardian,
        Student,
        TransportDriver,
        GateAccess,
        CanteenCashier,
        Teacher,
        MedicalStaff,
        ComplaintHandler,
        CommunicationSender,
        DocumentAdministrator,
        SchoolAdministrator,
        PlatformSupport
    ];
}

public static class MobileFeatureCodes
{
    public const string MobileApp = "mobile.app";
    public const string ApkRelease = "mobile.apk_release";
    public const string Localization = "mobile.localization";
    public const string OfflineSync = "mobile.offline_sync";
    public const string SupportDiagnostics = "mobile.support_diagnostics";

    public static readonly string[] All = [MobileApp, ApkRelease, Localization, OfflineSync, SupportDiagnostics];
}

public enum MobileAccessStatus { Active, Blocked, Revoked, Pending }
public enum WorkspaceStatus { Active, Disabled, Retired }
public enum MobileGrantStatus { Active, Revoked, Expired, PendingReview }
public enum MobileTextDirection { Ltr, Rtl }
public enum DeviceSessionStatus { Active, SignedOut, Revoked, Expired, BlockedVersion }
public enum ApkReleaseStatus { Draft, Approved, Active, Revoked, Superseded, Retired }
public enum ApkEnvironment { Demo, Pilot, Production }
public enum ReleaseAudienceType { Tenant, Role, Group, User, Pilot }
public enum ReleaseAudienceStatus { Active, Paused, Revoked }
public enum InstallEventType { Install, Launch, Upgrade, Blocked, Obsolete, RevokedVersion }
public enum InstallEventResult { Allowed, Blocked, UpdateRequired, SupportRequired }
public enum OfflineSyncStatus { Queued, Syncing, Accepted, Rejected, Duplicate, Conflict }
public enum MobileOfflinePolicy { NotSupported, QueueAllowed, CaptureOnly }
public enum MobileAuditLevel { Standard, Sensitive, Restricted }

public sealed record MobileProfileDto(
    string UserId,
    string ActiveTenantId,
    IReadOnlyList<string> AvailableTenants,
    IReadOnlyList<string> AvailableRoles,
    IReadOnlyList<string> LinkedStudents,
    string LanguageCode,
    string TextDirection,
    string MobileAccessStatus,
    string? DeniedReason);

public sealed record ActiveMobileContextRequest(string TenantId, string RoleCode, string LanguageCode, string DeviceId);
public sealed record MobileWorkspaceSummary(string WorkspaceCode, string DisplayName, string RoleCode, IReadOnlyList<string> SummaryCards, IReadOnlyList<MobileWorkspaceActionDto> Actions, IReadOnlyList<string> FeatureFlags, IReadOnlyList<string> OfflineCapabilities);
public sealed record MobileWorkspaceActionDto(string ActionCode, string SourceFeatureCode, string PermissionCode, string OfflinePolicy, string AuditLevel);
public sealed record MobileContextResponse(string ActiveTenantId, string ActiveRoleCode, string LanguageCode, string TextDirection, MobileWorkspaceSummary WorkspaceSummary);
public sealed record MobileWorkspaceDetail(string WorkspaceCode, string RoleContext, string? StudentContext, IReadOnlyList<string> SummarySections, IReadOnlyList<MobileWorkspaceActionDto> PrimaryActions, IReadOnlyList<MobileWorkspaceActionDto> SecondaryActions, IReadOnlyList<MobileNotificationDto> NotificationPreview, IReadOnlyList<string> EmptyStates, string LanguageCode, string TextDirection);
public sealed record MobileActionRequest(string WorkspaceCode, string SourceFeatureCode, string TargetId, string ClientActionId, string Payload, DateTimeOffset LocalOccurredAt);
public sealed record MobileActionResponse(string ActionResult, string SourceReference, string AuditEventId, bool Queued, string UserMessage);
public sealed record MobileNotificationDto(string NotificationId, string SourceFeatureCode, string Title, string Body, DateTimeOffset CreatedAt, string ReadState, string? ActionLink);
public sealed record OfflineActionRequest(string TenantId, string DeviceId, string UserId, string ActiveRoleCode, string SourceFeatureCode, string SourceActionCode, string ClientActionId, DateTimeOffset LocalOccurredAt, string Payload);
public sealed record OfflineActionResponse(string ClientActionId, string SyncStatus, string SourceReference, string? ConflictReason, string AuditEventId, string UserMessage);
public sealed record ApkReleaseDto(string ReleaseId, string VersionName, int VersionCode, string Environment, string ReleaseStatus, bool DownloadAllowed, bool UpdateRequired, string ReleaseNotes, string SupportContact, string Checksum, DateTimeOffset? ExpiresAt);
public sealed record CreateApkReleaseRequest(string VersionName, int VersionCode, string Environment, string ArtifactUri, string ArtifactChecksum, string ReleaseNotes, string SupportContact, int MinimumSupportedVersionCode);
public sealed record ApproveReleaseRequest(string ApprovalNote, IReadOnlyList<string> Audiences);
public sealed record RevokeReleaseRequest(string RevocationReason, string? ReplacementReleaseId, bool BlockImmediately);
public sealed record InstallEventRequest(string DeviceId, string? TenantId, string? UserId, string ReleaseId, string VersionName, int VersionCode, string EventType, string EventResult, DateTimeOffset OccurredAt);
public sealed record InstallEventResponse(string EventId, bool Accepted, string NextAction, string UserMessage);
public sealed record DeviceSessionDto(string SessionId, string TenantId, string UserId, string DeviceId, string AppVersion, string ActiveRoleCode, string LanguageCode, string SessionStatus, DateTimeOffset LastSeenAt);
public sealed record MobileAuditEventDto(string AuditEventId, string EventType, string ActorUserId, string DeviceId, string RoleCode, string PermissionCode, string LanguageCode, string Result, string ReasonCode, string TargetType, string TargetId, DateTimeOffset OccurredAt);
public sealed record SupportDiagnosticTimeline(string CorrelationId, string TenantContext, string UserContext, string DeviceContext, string VersionContext, IReadOnlyList<string> Events, string MostLikelyReason, string RecommendedSupportAction);
