namespace SafeSchool.Api.Features.Learning.Quizzes;

public static class QuizEndpointExtensions
{
    public static RouteGroupBuilder MapQuizEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/quizzes", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quizzes", status = "demo-ready" }));
        group.MapGet("/quiz-attempts", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-attempts", status = "demo-ready" }));
        group.MapPost("/quiz-attempts", (string schoolAccountId, StartQuizAttemptCommand request) => Results.Ok(new { schoolAccountId, attemptReference = $"quiz-attempt-{request.ClientRequestId}", request.QuizReference, request.StudentProfileId, status = "Scored", score = 92, evidence = new[] { "eligibility-checked", "feedback-held-until-close", "audit-written" } }));
        group.MapGet("/quiz-review", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-review", status = "demo-ready" }));
        group.MapGet("/quiz-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-trace", status = "demo-ready" }));
        return group;
    }
}

public sealed record StartQuizAttemptCommand(string QuizReference, string StudentProfileId, string ClientRequestId);
