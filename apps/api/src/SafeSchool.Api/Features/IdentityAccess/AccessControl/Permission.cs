using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class Permission : TenantOwnedEntity
{
    public required string PermissionKey { get; set; }
    public required string Description { get; set; }
    public required string PermissionScope { get; set; }
    public bool SensitiveAction { get; set; }
    public PermissionStatus PermissionStatus { get; set; } = PermissionStatus.Active;
}
