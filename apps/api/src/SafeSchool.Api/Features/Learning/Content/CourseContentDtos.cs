namespace SafeSchool.Api.Features.Learning.Content;

public sealed record CourseContentDto(string CourseId, string GroupId, string ContentId, string ProgressId, string Status, string Visibility, string TraceReference);
public sealed record CourseContentDtoList(IReadOnlyList<CourseContentDto> Items, int Page, int PageSize, int TotalCount);
