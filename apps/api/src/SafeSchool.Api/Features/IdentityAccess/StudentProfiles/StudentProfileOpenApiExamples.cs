namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public static class StudentProfileOpenApiExamples
{
    public const string CreateExample = """
    POST /api/v1/schools/{schoolAccountId}/identity/students
    { "schoolStudentNumber": "S-10024", "legalName": "Student Legal Name", "profileStatus": "Active" }
    """;

    public const string DuplicateError = "duplicate_active_identity: active identifiers require review before activation";
}
