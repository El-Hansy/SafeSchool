namespace SafeSchool.Api.Features.Learning.Quizzes;

public sealed record QuizEngineDto(string QuizId, string AttemptId, string ResultId, string Status, string Visibility, string TraceReference);
public sealed record QuizEngineDtoList(IReadOnlyList<QuizEngineDto> Items, int Page, int PageSize, int TotalCount);
