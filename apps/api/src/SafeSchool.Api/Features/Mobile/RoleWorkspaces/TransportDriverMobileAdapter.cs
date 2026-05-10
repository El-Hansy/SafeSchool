namespace SafeSchool.Api.Features.Mobile;

public sealed class TransportDriverMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.TransportDriver, "Assigned trips, boarding/drop scans, and location updates", ["view_trip", "scan_boarding", "scan_drop", "send_location"], true);
}
