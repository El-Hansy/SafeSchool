using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed record ExternalIdentityReferenceDto(string ReferenceType, string ReferenceValue);

public sealed record CreateStudentProfileRequest(
    string SchoolStudentNumber,
    IReadOnlyList<ExternalIdentityReferenceDto> ExternalIdentityReferences,
    string LegalName,
    string? PreferredName,
    DateOnly DateOfBirth,
    string GradeLevel,
    string? CampusOrDivision,
    string EnrollmentStatus,
    ProfileStatus ProfileStatus,
    string ReviewReason,
    string ClientRequestId);

public sealed record UpdateStudentProfileRequest(
    string? LegalName,
    string? PreferredName,
    string? GradeLevel,
    string? CampusOrDivision,
    string? EnrollmentStatus,
    string ReviewReason,
    string ClientRequestId);

public sealed record StudentProfileResponse(
    Guid StudentProfileId,
    string SchoolAccountId,
    string SchoolStudentNumber,
    IReadOnlyList<string> ExternalIdentityReferences,
    string LegalName,
    string? PreferredName,
    DateOnly DateOfBirth,
    string GradeLevel,
    string? CampusOrDivision,
    string EnrollmentStatus,
    ProfileStatus ProfileStatus,
    DuplicateReviewStatus DuplicateReviewStatus,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record DuplicateCheckRequest(string SchoolStudentNumber, IReadOnlyList<ExternalIdentityReferenceDto> ExternalIdentityReferences);

public sealed record DuplicateCheckResponse(DuplicateReviewStatus Status, IReadOnlyList<Guid> MatchingProfileIds, string ReviewReason);

public sealed record DeactivateStudentProfileRequest(string ReviewReason, string ClientRequestId);

public static class StudentProfileMapping
{
    public static StudentProfileResponse ToResponse(this StudentProfile profile) => new(
        profile.Id,
        profile.TenantId,
        profile.SchoolStudentNumber,
        profile.ExternalIdentityReferences,
        profile.LegalName,
        profile.PreferredName,
        profile.DateOfBirth,
        profile.GradeLevel,
        profile.CampusOrDivision,
        profile.EnrollmentStatus,
        profile.ProfileStatus,
        profile.DuplicateReviewStatus,
        profile.CreatedAt,
        profile.UpdatedAt);
}
