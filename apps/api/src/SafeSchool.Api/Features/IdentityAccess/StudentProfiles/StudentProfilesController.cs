using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public static class StudentProfilesController
{
    public static RouteGroupBuilder MapStudentProfileEndpoints(this RouteGroupBuilder group)
    {
        var students = group.MapGroup("/students");
        students.MapPost("/", async (string schoolAccountId, CreateStudentProfileRequest request, StudentProfileService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request, ct);
            return result.Succeeded ? Results.Created($"/students/{result.Value!.StudentProfileId}", result.Value) : Results.BadRequest(result.Errors);
        });
        students.MapGet("/", async (string schoolAccountId, StudentProfileService service, int page, int pageSize, CancellationToken ct) =>
            Results.Ok(await service.ListAsync(schoolAccountId, page <= 0 ? 1 : page, pageSize <= 0 ? 25 : pageSize, ct)));
        students.MapPost("/{studentProfileId:guid}/deactivate", async (string schoolAccountId, Guid studentProfileId, DeactivateStudentProfileRequest request, StudentProfileService service, CancellationToken ct) =>
        {
            var result = await service.DeactivateAsync(schoolAccountId, studentProfileId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        students.MapPost("/duplicate-check", async (string schoolAccountId, DuplicateCheckRequest request, DuplicateStudentProfileDetector detector, CancellationToken ct) =>
            Results.Ok(await detector.CheckAsync(schoolAccountId, request, ct)));
        students.MapGet("/{studentProfileId:guid}", async (string schoolAccountId, Guid studentProfileId, StudentProfileService service, CancellationToken ct) =>
        {
            var result = await service.ReadAsync(schoolAccountId, studentProfileId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });
        students.MapPatch("/{studentProfileId:guid}", async (string schoolAccountId, Guid studentProfileId, UpdateStudentProfileRequest request, StudentProfileService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(schoolAccountId, studentProfileId, request, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        students.MapGet("/{studentProfileId:guid}/history", async (string schoolAccountId, Guid studentProfileId, StudentProfileService service, CancellationToken ct) =>
            Results.Ok(await service.HistoryAsync(schoolAccountId, studentProfileId, ct)));
        return group;
    }
}
