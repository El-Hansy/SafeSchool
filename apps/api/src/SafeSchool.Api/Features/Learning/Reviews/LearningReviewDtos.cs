namespace SafeSchool.Api.Features.Learning.Reviews;

public sealed record LearningReviewDto(string RuleId, string HistoryId, string ExceptionId, string ReviewId, string SummaryId, string TraceId, string Status, string Visibility, string TraceReference);
public sealed record LearningReviewDtoList(IReadOnlyList<LearningReviewDto> Items, int Page, int PageSize, int TotalCount);
