namespace SafeSchool.Api.Features.Mobile;

public sealed class MobilePermissionGrant
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string GrantType { get; init; } = "role";
    public string SubjectId { get; init; } = MobileRoleCodes.Guardian;
    public string WorkspaceCode { get; init; } = MobileRoleCodes.Guardian;
    public string PermissionCode { get; init; } = "mobile.workspace.view";
    public MobileGrantStatus GrantStatus { get; init; } = MobileGrantStatus.Active;
    public DateTimeOffset EffectiveFrom { get; init; } = DateTimeOffset.UtcNow.AddDays(-1);
    public DateTimeOffset? EffectiveUntil { get; init; }
    public string CreatedByUserId { get; init; } = "admin-demo";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
