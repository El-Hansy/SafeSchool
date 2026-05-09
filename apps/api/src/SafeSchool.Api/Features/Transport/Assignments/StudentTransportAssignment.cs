using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Assignments;

public sealed class StudentTransportAssignment : TenantOwnedEntity
{
    public string StudentProfileId { get; set; } = string.Empty;
    public Guid TransportRouteId { get; set; }
    public Guid? TransportVehicleId { get; set; }
    public Guid? PickupRouteStopSequenceId { get; set; }
    public Guid? DropRouteStopSequenceId { get; set; }
    public ServiceDirection ServiceDirection { get; set; } = ServiceDirection.Both;
    public DateOnly ValidFrom { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? ValidTo { get; set; }
    public GuardianVisibilityState VisibilityState { get; set; } = GuardianVisibilityState.GuardianVisible;
    public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.Draft;
    public TransportReviewStatus ReviewStatus { get; set; } = TransportReviewStatus.NotRequired;

    public bool HasValidDates() => ValidTo is null || ValidTo >= ValidFrom;
    public bool IsActiveOn(DateOnly date) => AssignmentStatus == AssignmentStatus.Active && ValidFrom <= date && (ValidTo is null || ValidTo >= date);
}
