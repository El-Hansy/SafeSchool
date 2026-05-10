namespace SafeSchool.Api.Features.Mobile;

public sealed class ComplaintHandlerMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.ComplaintHandler, "Complaint triage and escalation", ["triage", "assign", "escalate"], false);
}
