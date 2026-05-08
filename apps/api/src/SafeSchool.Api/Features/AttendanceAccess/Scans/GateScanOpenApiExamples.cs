namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public static class GateScanOpenApiExamples
{
    public const string OnlineScan = "POST /api/v1/schools/{schoolAccountId}/attendance-access/scans";
    public const string OfflineSync = "POST /api/v1/schools/{schoolAccountId}/attendance-access/scans/offline-sync";
    public const string Trace = "GET /api/v1/schools/{schoolAccountId}/attendance-access/scans/{scanEventId}/trace";
}

