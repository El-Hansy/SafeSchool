using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class QuizResponse : TenantOwnedEntity
{
    public Guid QuizAttemptId { get; set; }
    public Guid QuizQuestionId { get; set; }
    public string ResponseReference { get; set; } = string.Empty;
    public decimal? AwardedPoints { get; set; }
    public bool RequiresManualReview { get; set; }
}
