namespace SafeSchool.Api.Features.Learning.Quizzes;

public static class QuizEndpointExtensions
{
    public static RouteGroupBuilder MapQuizEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/quizzes", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quizzes", status = "demo-ready" }));
        group.MapGet("/quiz-attempts", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-attempts", status = "demo-ready" }));
        group.MapGet("/quiz-review", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-review", status = "demo-ready" }));
        group.MapGet("/quiz-trace", (string schoolAccountId) => Results.Ok(new { schoolAccountId, area = "quiz-trace", status = "demo-ready" }));
        return group;
    }
}
