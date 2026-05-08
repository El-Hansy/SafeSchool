using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.StudentProfiles;

public static class StudentProfileTestData
{
    public static StudentProfile ActiveStudent(string tenantId = "school-1", string number = "S-1001") =>
        new()
        {
            TenantId = tenantId,
            SchoolStudentNumber = number,
            LegalName = "Amina Hassan",
            DateOfBirth = new DateOnly(2015, 1, 1),
            GradeLevel = "5",
            EnrollmentStatus = "Enrolled",
            ProfileStatus = ProfileStatus.Active,
            CreatedBy = "staff:admin",
            UpdatedBy = "staff:admin",
            ReviewReason = "Test profile."
        };
}
