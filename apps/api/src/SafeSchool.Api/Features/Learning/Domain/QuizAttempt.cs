using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class QuizAttempt : TenantOwnedEntity
{
    public Guid QuizId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public int AttemptNumber { get; set; } = 1;
    public AttemptStatus AttemptStatus { get; set; } = AttemptStatus.Started;
    public string ClientRequestId { get; set; } = string.Empty;
    public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SubmittedAt { get; set; }
    public int QuestionSetRevision { get; set; } = 1;
    public ScoreStatus ScoreStatus { get; set; } = ScoreStatus.Pending;
    public decimal? ScoreValue { get; set; }
}
