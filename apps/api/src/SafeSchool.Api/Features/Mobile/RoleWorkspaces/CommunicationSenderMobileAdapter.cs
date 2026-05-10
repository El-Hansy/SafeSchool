namespace SafeSchool.Api.Features.Mobile;

public sealed class CommunicationSenderMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.CommunicationSender, "Broadcast and direct messaging", ["compose", "send", "review_delivery"], false);
}
