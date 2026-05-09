using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class LearningException : TenantOwnedEntity
{
    public string SourceType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public string AffectedStudentProfileId { get; set; } = string.Empty;
    public LearningExceptionType ExceptionType { get; set; } = LearningExceptionType.ManualReviewRequired;
    public LearningExceptionSeverity Severity { get; set; } = LearningExceptionSeverity.Medium;
    public LearningExceptionStatus Status { get; set; } = LearningExceptionStatus.Open;
    public string ReviewerActorId { get; set; } = string.Empty;
    public string ResolutionReason { get; set; } = string.Empty;
}
