namespace SafeSchool.Api.Features.Learning.Assignments;

public static class AssignmentEndpointExtensions
{
    public static RouteGroupBuilder MapAssignmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/assignments", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignments", status = "demo-ready" }));
        group.MapGet("/submissions", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "submissions", status = "demo-ready" }));
        group.MapGet("/assignment-review", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignment-review", status = "demo-ready" }));
        group.MapGet("/assignment-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignment-trace", status = "demo-ready" }));
        return group;
    }
}
