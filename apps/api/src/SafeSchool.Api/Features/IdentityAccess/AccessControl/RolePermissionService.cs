using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed class RolePermissionService(SafeSchoolDbContext dbContext, ITenantContext tenantContext)
{
    public async Task<RoleResponse> CreateRoleAsync(string tenantId, RoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = new Role
        {
            TenantId = tenantId,
            RoleKey = request.RoleKey,
            DisplayName = request.DisplayName,
            RoleScope = request.RoleScope,
            RoleStatus = Common.RoleStatus.Active
        };
        dbContext.Roles.Add(role);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new(role.Id, role.TenantId, role.RoleKey, role.DisplayName, role.RoleScope, role.RoleStatus.ToString());
    }

    public async Task<IReadOnlyList<RoleResponse>> ListRolesAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.Roles.Where(x => x.TenantId == tenantId)
            .Select(x => new RoleResponse(x.Id, x.TenantId, x.RoleKey, x.DisplayName, x.RoleScope, x.RoleStatus.ToString()))
            .ToListAsync(cancellationToken);

    public async Task<ActorRoleAssignment> AssignRoleAsync(string tenantId, RoleAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        var assignment = new ActorRoleAssignment
        {
            TenantId = tenantId,
            ActorReference = request.ActorReference,
            RoleId = request.RoleId,
            AssignmentStatus = Enum.Parse<Common.AssignmentStatus>(request.AssignmentStatus),
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            AssignedBy = tenantContext.ActorReference ?? "unknown",
            ReviewReason = request.ReviewReason
        };
        dbContext.ActorRoleAssignments.Add(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
        return assignment;
    }
}
