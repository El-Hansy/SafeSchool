using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed class AttendanceAnomaly : TenantOwnedEntity
{
    public AnomalyType AnomalyType { get; set; }
    public AnomalySeverity Severity { get; set; } = AnomalySeverity.Medium;
    public AnomalyStatus Status { get; set; } = AnomalyStatus.New;
    public string StudentProfileId { get; set; } = string.Empty;
    public string EvidenceReference { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string ResolutionReason { get; set; } = string.Empty;

    public bool CanTransitionTo(AnomalyStatus status) => (Status, status) switch
    {
        (AnomalyStatus.New, AnomalyStatus.Assigned) => true,
        (AnomalyStatus.Assigned, AnomalyStatus.InReview) => true,
        (AnomalyStatus.InReview or AnomalyStatus.Assigned or AnomalyStatus.New, AnomalyStatus.Resolved) => true,
        (AnomalyStatus.InReview or AnomalyStatus.Assigned or AnomalyStatus.New, AnomalyStatus.Dismissed) => true,
        (AnomalyStatus.Resolved or AnomalyStatus.Dismissed, AnomalyStatus.Reopened) => true,
        _ => Status == status
    };
}

