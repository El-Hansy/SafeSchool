using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class Role : TenantOwnedEntity
{
    public required string RoleKey { get; set; }
    public required string DisplayName { get; set; }
    public required string RoleScope { get; set; }
    public RoleStatus RoleStatus { get; set; } = RoleStatus.Draft;
}
