using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class ActorRoleAssignment : TenantOwnedEntity
{
    public required string ActorReference { get; set; }
    public Guid RoleId { get; set; }
    public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.Pending;
    public DateTimeOffset ValidFrom { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ValidUntil { get; set; }
    public required string AssignedBy { get; set; }
    public required string ReviewReason { get; set; }
}
