using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Assignments;

public static class AssignmentEndpointExtensions
{
    public static RouteGroupBuilder MapAssignmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/assignments", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.AssignmentsAsync(schoolAccountId, ct)));
        group.MapPost("/assignments", async (string schoolAccountId, CreateAssignmentCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.CreateAssignmentAsync(schoolAccountId, request, ct)));
        group.MapGet("/submissions", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.SubmissionsAsync(schoolAccountId, ct)));
        group.MapPost("/submissions", async (string schoolAccountId, SubmitAssignmentCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.SubmitAssignmentAsync(schoolAccountId, request, ct)));
        group.MapGet("/assignment-review", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.AssignmentReviewAsync(schoolAccountId, ct)));
        group.MapGet("/assignment-trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.AssignmentTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record CreateAssignmentCommand(string Title, string GroupReference, string DueDate, string ClientRequestId);
public sealed record SubmitAssignmentCommand(string AssignmentReference, string StudentProfileId, string SubmissionText, string ClientRequestId);
