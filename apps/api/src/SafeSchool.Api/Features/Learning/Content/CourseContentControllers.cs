namespace SafeSchool.Api.Features.Learning.Content;

public static class CourseContentEndpointExtensions
{
    public static RouteGroupBuilder MapCourseContentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/courses", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "courses", status = "demo-ready" }));
        group.MapGet("/groups", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "groups", status = "demo-ready" }));
        group.MapGet("/content", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "content", status = "demo-ready" }));
        group.MapGet("/progress", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "progress", status = "demo-ready" }));
        group.MapGet("/trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "trace", status = "demo-ready" }));
        return group;
    }
}
