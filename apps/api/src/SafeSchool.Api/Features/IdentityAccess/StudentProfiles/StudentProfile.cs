using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfile : TenantOwnedEntity
{
    public required string SchoolStudentNumber { get; set; }
    public string[] ExternalIdentityReferences { get; set; } = [];
    public required string LegalName { get; set; }
    public string? PreferredName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string GradeLevel { get; set; }
    public string? CampusOrDivision { get; set; }
    public required string EnrollmentStatus { get; set; }
    public ProfileStatus ProfileStatus { get; set; } = ProfileStatus.Draft;
    public DuplicateReviewStatus DuplicateReviewStatus { get; set; } = DuplicateReviewStatus.Clear;
    public required string CreatedBy { get; set; }
    public required string UpdatedBy { get; set; }
    public required string ReviewReason { get; set; }

    public bool CanTransitionTo(ProfileStatus next) => (ProfileStatus, next) switch
    {
        (ProfileStatus.Draft, ProfileStatus.Active) => DuplicateReviewStatus == DuplicateReviewStatus.Clear,
        (ProfileStatus.Draft, ProfileStatus.Archived) => true,
        (ProfileStatus.Active, ProfileStatus.Suspended) => true,
        (ProfileStatus.Active, ProfileStatus.Deactivated) => true,
        (ProfileStatus.Suspended, ProfileStatus.Active) => DuplicateReviewStatus == DuplicateReviewStatus.Clear,
        (ProfileStatus.Suspended, ProfileStatus.Deactivated) => true,
        (ProfileStatus.Deactivated, ProfileStatus.Archived) => true,
        _ => false
    };
}
