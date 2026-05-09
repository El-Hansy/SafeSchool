namespace SafeSchool.Api.Features.Learning.Behavior;

public sealed record BehaviorLoggingDto(string CategoryId, string EventId, string TraceId, string Status, string Visibility, string TraceReference);
public sealed record BehaviorLoggingDtoList(IReadOnlyList<BehaviorLoggingDto> Items, int Page, int PageSize, int TotalCount);
