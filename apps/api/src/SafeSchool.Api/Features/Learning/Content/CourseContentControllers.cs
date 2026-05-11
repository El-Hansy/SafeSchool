using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Content;

public static class CourseContentEndpointExtensions
{
    public static RouteGroupBuilder MapCourseContentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/courses", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.CoursesAsync(schoolAccountId, ct)));
        group.MapGet("/groups", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.GroupsAsync(schoolAccountId, ct)));
        group.MapGet("/content", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ContentAsync(schoolAccountId, ct)));
        group.MapPost("/content", async (string schoolAccountId, PublishContentCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.PublishContentAsync(schoolAccountId, request, ct)));
        group.MapGet("/progress", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ProgressAsync(schoolAccountId, ct)));
        group.MapGet("/trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.ContentTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record PublishContentCommand(string Title, string GroupReference, string Visibility, string ClientRequestId);
