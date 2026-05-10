using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed record RunAnomalyDetectionRequest(string ClientRequestId, Guid? AttendanceSessionId = null);
public sealed record AnomalyResponse(Guid AnomalyId, string SchoolAccountId, AnomalyType AnomalyType, AnomalySeverity Severity, AnomalyStatus Status, string StudentProfileId, string EvidenceReference, string AssignedTo, string ResolutionReason);
public sealed record AssignAnomalyRequest(string ReviewerReference, string Reason);
public sealed record ResolveAnomalyRequest(string Reason);
public sealed record DismissAnomalyRequest(string Reason);

public static class AnomalyMappings
{
    public static AnomalyResponse ToResponse(this AttendanceAnomaly anomaly) =>
        new(anomaly.Id, anomaly.TenantId, anomaly.AnomalyType, anomaly.Severity, anomaly.Status, anomaly.StudentProfileId, anomaly.EvidenceReference, anomaly.AssignedTo, anomaly.ResolutionReason);
}

