namespace SafeSchool.Api.Features.Learning.Assignments;

public sealed record AssignmentTrackingDto(string AssignmentId, string SubmissionId, string TraceId, string Status, string Visibility, string TraceReference);
public sealed record AssignmentTrackingDtoList(IReadOnlyList<AssignmentTrackingDto> Items, int Page, int PageSize, int TotalCount);
