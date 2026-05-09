using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public static class AnomalyTestData
{
    public static AttendanceAnomaly InvalidCredential() => new()
    {
        TenantId = "school-1",
        AnomalyType = AnomalyType.InvalidCredential,
        Severity = AnomalySeverity.High,
        Status = AnomalyStatus.New,
        StudentProfileId = "student-1",
        EvidenceReference = "scan-1"
    };
}

