using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public sealed class Gate : TenantOwnedEntity
{
    public string GateCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string CampusReference { get; set; } = string.Empty;
    public GateStatus Status { get; set; } = GateStatus.Draft;
    public bool AllowsEntry { get; set; } = true;
    public bool AllowsExit { get; set; } = true;
    public bool OfflineAllowed { get; set; } = true;

    public bool CanTransitionTo(GateStatus status) => (Status, status) switch
    {
        (GateStatus.Draft, GateStatus.Active) => true,
        (GateStatus.Active, GateStatus.Suspended) => true,
        (GateStatus.Suspended, GateStatus.Active) => true,
        (GateStatus.Active or GateStatus.Suspended, GateStatus.Decommissioned) => true,
        _ => Status == status
    };

    public bool Allows(AttendanceDirection direction) => direction == AttendanceDirection.Entry ? AllowsEntry : AllowsExit;
}

