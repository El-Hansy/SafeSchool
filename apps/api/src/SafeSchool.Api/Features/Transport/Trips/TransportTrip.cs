using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Trips;

public sealed class TransportTrip : TenantOwnedEntity
{
    public Guid TransportRouteId { get; set; }
    public string RouteVersion { get; set; } = "v1";
    public Guid TransportVehicleId { get; set; }
    public string TrackingDeviceReference { get; set; } = string.Empty;
    public ServiceDirection ServiceDirection { get; set; } = ServiceDirection.Pickup;
    public DateOnly TripDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateTimeOffset PlannedStartTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ActualStartTime { get; set; }
    public DateTimeOffset? ActualEndTime { get; set; }
    public string DriverReference { get; set; } = string.Empty;
    public string AttendantReference { get; set; } = string.Empty;
    public string SupervisorReference { get; set; } = string.Empty;
    public TripStatus TripStatus { get; set; } = TripStatus.Planned;
    public TransportReviewStatus ReviewStatus { get; set; } = TransportReviewStatus.NotRequired;

    public bool IsActiveLike() => TripStatus is TripStatus.Active or TripStatus.Paused;
    public bool CanAcceptScan() => TripStatus == TripStatus.Active;
    public bool CanAcceptLocation(string deviceReference) => TripStatus == TripStatus.Active && TrackingDeviceReference == deviceReference;
    public bool ConflictsWith(TransportTrip other) => TenantId == other.TenantId && IsActiveLike() && other.IsActiveLike() && (TransportVehicleId == other.TransportVehicleId || TrackingDeviceReference == other.TrackingDeviceReference);
}
