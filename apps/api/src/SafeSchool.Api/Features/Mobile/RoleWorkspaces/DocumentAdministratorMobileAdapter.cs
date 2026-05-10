namespace SafeSchool.Api.Features.Mobile;

public sealed class DocumentAdministratorMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.DocumentAdministrator, "Document and certificate review", ["review_document", "issue_certificate", "view_search"], false);
}
