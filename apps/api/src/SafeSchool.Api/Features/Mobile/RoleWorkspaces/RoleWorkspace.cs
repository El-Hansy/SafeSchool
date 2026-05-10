namespace SafeSchool.Api.Features.Mobile;

public sealed class RoleWorkspace
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string WorkspaceCode { get; init; } = MobileRoleCodes.Guardian;
    public string DisplayNameKey { get; init; } = "workspace.guardian";
    public string RequiredRoleCode { get; init; } = MobileRoleCodes.Guardian;
    public string RequiredPermissionCodes { get; init; } = "mobile.workspace.view";
    public string EnabledFeatureCodes { get; init; } = string.Join(',', MobileFeatureCodes.All);
    public WorkspaceStatus WorkspaceStatus { get; init; } = WorkspaceStatus.Active;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}

public sealed class RoleWorkspaceAction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public Guid WorkspaceId { get; init; }
    public string WorkspaceCode { get; init; } = MobileRoleCodes.Guardian;
    public string ActionCode { get; init; } = "view_summary";
    public string SourceFeatureCode { get; init; } = "identity";
    public string RequiredPermissionCode { get; init; } = "mobile.action.view";
    public MobileOfflinePolicy OfflinePolicy { get; init; } = MobileOfflinePolicy.NotSupported;
    public bool RequiresOnlineConfirmation { get; init; } = true;
    public MobileAuditLevel AuditLevel { get; init; } = MobileAuditLevel.Standard;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
