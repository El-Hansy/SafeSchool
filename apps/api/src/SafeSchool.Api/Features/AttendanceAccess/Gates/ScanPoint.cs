using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public sealed class ScanPoint : TenantOwnedEntity
{
    public Guid GateId { get; set; }
    public string DeviceReference { get; set; } = string.Empty;
    public string AssignedActorReference { get; set; } = string.Empty;
    public ScanPointStatus Status { get; set; } = ScanPointStatus.Pending;
    public bool OfflineAllowed { get; set; } = true;
    public bool AllowsEntry { get; set; } = true;
    public bool AllowsExit { get; set; } = true;

    public bool CanTransitionTo(ScanPointStatus status) => (Status, status) switch
    {
        (ScanPointStatus.Pending, ScanPointStatus.Active) => true,
        (ScanPointStatus.Active, ScanPointStatus.Suspended) => true,
        (ScanPointStatus.Suspended, ScanPointStatus.Active) => true,
        (ScanPointStatus.Active or ScanPointStatus.Suspended, ScanPointStatus.Retired) => true,
        _ => Status == status
    };

    public bool Allows(AttendanceDirection direction) => direction == AttendanceDirection.Entry ? AllowsEntry : AllowsExit;
}

