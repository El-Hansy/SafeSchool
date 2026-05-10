using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class QuizQuestion : TenantOwnedEntity
{
    public Guid QuizId { get; set; }
    public int QuestionSetRevision { get; set; } = 1;
    public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;
    public string PromptReference { get; set; } = string.Empty;
    public string AnswerKeyReference { get; set; } = string.Empty;
    public int PointsPossible { get; set; } = 1;
    public int DisplayOrder { get; set; }
    public bool Required { get; set; } = true;
}
