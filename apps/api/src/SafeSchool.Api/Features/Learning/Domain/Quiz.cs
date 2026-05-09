using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class Quiz : TenantOwnedEntity
{
    public Guid? CourseId { get; set; }
    public Guid? LearningGroupId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizStatus QuizStatus { get; set; } = QuizStatus.Draft;
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? ClosesAt { get; set; }
    public int AttemptLimit { get; set; } = 1;
    public int? TimeLimitMinutes { get; set; }
    public ScoringPolicy ScoringPolicy { get; set; } = ScoringPolicy.AutoScore;
    public FeedbackVisibility FeedbackVisibility { get; set; } = FeedbackVisibility.AfterClose;
    public int QuestionSetRevision { get; set; } = 1;
}
