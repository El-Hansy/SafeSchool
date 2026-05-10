namespace SafeSchool.Api.Features.Learning.Stars;

public sealed record StarRewardDto(string RuleId, string LedgerId, string RewardId, string RedemptionId, string Status, string Visibility, string TraceReference);
public sealed record StarRewardDtoList(IReadOnlyList<StarRewardDto> Items, int Page, int PageSize, int TotalCount);
