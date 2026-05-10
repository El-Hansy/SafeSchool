namespace SafeSchool.Api.Features.Mobile;

public sealed class MedicalStaffMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.MedicalStaff, "Medical profile and emergency capture", ["view_medical", "capture_emergency", "handover"], true);
}
