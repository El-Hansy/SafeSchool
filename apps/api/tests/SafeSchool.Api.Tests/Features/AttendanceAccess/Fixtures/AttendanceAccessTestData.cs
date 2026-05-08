using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;

public static class AttendanceAccessTestData
{
    public static RecordScanRequest EntryScan(Guid gateId, Guid scanPointId, string credential = "credential-active", string clientScanId = "scan-1") =>
        new(gateId, scanPointId, credential, AttendanceDirection.Entry, ScanMethod.Nfc, clientScanId, DateTimeOffset.UtcNow.Date.AddHours(7).AddMinutes(45));

    public static CredentialEvidence ActiveCredential(string tenantId = "school-1", string student = "student-active", string credential = "credential-active") =>
        new(tenantId, student, credential, CredentialEvidenceStatus.Active);
}

