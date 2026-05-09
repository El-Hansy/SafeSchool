namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public static class AccessControlControllers
{
    public static RouteGroupBuilder MapAccessControlEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/roles", async (string schoolAccountId, RolePermissionService service, CancellationToken ct) =>
            Results.Ok(await service.ListRolesAsync(schoolAccountId, ct)));
        group.MapPost("/roles", async (string schoolAccountId, RoleRequest request, RolePermissionService service, CancellationToken ct) =>
            Results.Created("/roles", await service.CreateRoleAsync(schoolAccountId, request, ct)));
        group.MapPatch("/roles/{roleId:guid}", () => Results.Accepted());
        group.MapGet("/permissions", () => Results.Ok(PermissionCatalog.SchoolAdministratorPermissions));
        group.MapPut("/roles/{roleId:guid}/permissions", () => Results.Accepted());
        group.MapPost("/role-assignments", async (string schoolAccountId, RoleAssignmentRequest request, RolePermissionService service, CancellationToken ct) =>
            Results.Created("/role-assignments", await service.AssignRoleAsync(schoolAccountId, request, ct)));
        group.MapPatch("/role-assignments/{assignmentId:guid}", () => Results.Accepted());
        group.MapReviewEndpoints();
        return group;
    }
}
