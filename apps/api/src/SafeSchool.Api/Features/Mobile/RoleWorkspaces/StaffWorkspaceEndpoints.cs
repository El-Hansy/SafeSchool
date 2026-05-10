namespace SafeSchool.Api.Features.Mobile;

public static class StaffWorkspaceEndpoints
{
    public static RouteGroupBuilder MapStaffWorkspaceEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/staff/workspaces", (StaffWorkspaceService service) => Results.Ok(service.AllStaffWorkspaces()));
        group.MapPost("/actions/{actionCode}", (string actionCode, MobileActionRequest request, SourceDomainActionAdapters actions) =>
            Results.Ok(actions.Execute("school-demo", "demo-user", "device-demo", request.WorkspaceCode, request with { ClientActionId = string.IsNullOrWhiteSpace(request.ClientActionId) ? actionCode : request.ClientActionId })));
        return group;
    }
}
