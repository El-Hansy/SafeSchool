using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class RolePermission : TenantOwnedEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public string PermissionKey { get; set; } = string.Empty;
}
