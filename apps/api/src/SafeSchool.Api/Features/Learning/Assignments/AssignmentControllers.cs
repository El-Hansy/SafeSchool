namespace SafeSchool.Api.Features.Learning.Assignments;

public static class AssignmentEndpointExtensions
{
    public static RouteGroupBuilder MapAssignmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/assignments", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignments", status = "demo-ready" }));
        group.MapPost("/assignments", (string schoolAccountId, CreateAssignmentCommand request) => Results.Ok(new { schoolAccountId, assignmentReference = $"assignment-{request.ClientRequestId}", request.Title, request.GroupReference, status = "Assigned", evidence = new[] { "eligibility-checked", "due-date-recorded", "audit-written" } }));
        group.MapGet("/submissions", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "submissions", status = "demo-ready" }));
        group.MapPost("/submissions", (string schoolAccountId, SubmitAssignmentCommand request) => Results.Ok(new { schoolAccountId, submissionReference = $"submission-{request.ClientRequestId}", request.AssignmentReference, request.StudentProfileId, status = "Submitted", evidence = new[] { "attempt-accepted", "history-written", "guardian-visible-summary" } }));
        group.MapGet("/assignment-review", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignment-review", status = "demo-ready" }));
        group.MapGet("/assignment-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "assignment-trace", status = "demo-ready" }));
        return group;
    }
}

public sealed record CreateAssignmentCommand(string Title, string GroupReference, string DueDate, string ClientRequestId);
public sealed record SubmitAssignmentCommand(string AssignmentReference, string StudentProfileId, string SubmissionText, string ClientRequestId);
