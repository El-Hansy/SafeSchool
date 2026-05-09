using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class AssignmentSubmission : TenantOwnedEntity
{
    public Guid AssignmentId { get; set; }
    public string StudentProfileId { get; set; } = string.Empty;
    public int AttemptNumber { get; set; } = 1;
    public SubmissionStatus SubmissionStatus { get; set; } = SubmissionStatus.Draft;
    public string SubmittedEvidenceReference { get; set; } = string.Empty;
    public DateTimeOffset? SubmittedAt { get; set; }
    public string GradeValue { get; set; } = string.Empty;
    public string FeedbackSummary { get; set; } = string.Empty;
    public string ReviewerActorId { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
}
