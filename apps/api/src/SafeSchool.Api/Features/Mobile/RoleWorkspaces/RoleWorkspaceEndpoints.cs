namespace SafeSchool.Api.Features.Mobile;

public static class RoleWorkspaceEndpoints
{
    public static RouteGroupBuilder MapRoleWorkspaceEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/workspaces", (string? tenantId, string? roleCode, string? languageCode, RoleWorkspaceResolver resolver) =>
            Results.Ok(resolver.ResolveVisible(tenantId ?? "school-demo", roleCode ?? MobileRoleCodes.Guardian, languageCode ?? "en")));
        group.MapGet("/workspaces/{workspaceCode}", (string workspaceCode, string? tenantId, string? languageCode, RoleWorkspaceResolver resolver) =>
            Results.Ok(resolver.Detail(tenantId ?? "school-demo", workspaceCode, languageCode ?? "en")));
        return group;
    }
}
