using SafeSchool.Api.Features.Learning;

namespace SafeSchool.Api.Features.Learning.Quizzes;

public static class QuizEndpointExtensions
{
    public static RouteGroupBuilder MapQuizEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/quizzes", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.QuizzesAsync(schoolAccountId, ct)));
        group.MapGet("/quiz-attempts", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.QuizAttemptsAsync(schoolAccountId, ct)));
        group.MapPost("/quiz-attempts", async (string schoolAccountId, StartQuizAttemptCommand request, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.StartQuizAttemptAsync(schoolAccountId, request, ct)));
        group.MapGet("/quiz-review", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.QuizReviewAsync(schoolAccountId, ct)));
        group.MapGet("/quiz-trace", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.QuizTraceAsync(schoolAccountId, ct)));
        return group;
    }
}

public sealed record StartQuizAttemptCommand(string QuizReference, string StudentProfileId, string ClientRequestId);
