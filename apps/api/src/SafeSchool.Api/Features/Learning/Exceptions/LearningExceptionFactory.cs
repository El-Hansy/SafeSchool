using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Domain;

namespace SafeSchool.Api.Features.Learning.Exceptions;

public sealed class LearningExceptionFactory
{
    public LearningException Create(string tenantId, LearningExceptionType exceptionType, string sourceType, string sourceReference, string studentProfileId = "") => new()
    {
        TenantId = tenantId,
        ExceptionType = exceptionType,
        SourceType = sourceType,
        SourceReference = sourceReference,
        AffectedStudentProfileId = studentProfileId,
        Severity = exceptionType is LearningExceptionType.CrossSchoolAccess or LearningExceptionType.DisabledFeature ? LearningExceptionSeverity.High : LearningExceptionSeverity.Medium,
        Status = LearningExceptionStatus.Open
    };
}
