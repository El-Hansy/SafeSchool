namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public static class PermissionCatalog
{
    public const string StudentProfilesRead = "identity.student_profiles.read";
    public const string StudentProfilesCreate = "identity.student_profiles.create";
    public const string StudentProfilesUpdate = "identity.student_profiles.update";
    public const string StudentProfilesDeactivate = "identity.student_profiles.deactivate";
    public const string StudentProfilesReviewHistory = "identity.student_profiles.review_history";
    public const string GuardiansRead = "identity.guardians.read";
    public const string GuardiansCreate = "identity.guardians.create";
    public const string GuardiansUpdate = "identity.guardians.update";
    public const string GuardianLinksCreate = "identity.guardian_links.create";
    public const string GuardianLinksApprove = "identity.guardian_links.approve";
    public const string GuardianLinksSuspend = "identity.guardian_links.suspend";
    public const string GuardianLinksRemove = "identity.guardian_links.remove";
    public const string CredentialsRead = "identity.credentials.read";
    public const string CredentialsIssue = "identity.credentials.issue";
    public const string CredentialsSuspend = "identity.credentials.suspend";
    public const string CredentialsRestore = "identity.credentials.restore";
    public const string CredentialsReplace = "identity.credentials.replace";
    public const string CredentialsRevoke = "identity.credentials.revoke";
    public const string CredentialsRotateQr = "identity.credentials.rotate_qr";
    public const string RolesRead = "identity.roles.read";
    public const string RolesManage = "identity.roles.manage";
    public const string RoleAdministration = RolesManage;
    public const string PermissionsRead = "identity.permissions.read";
    public const string PermissionsManage = "identity.permissions.manage";
    public const string PermissionAdministration = PermissionsManage;
    public const string RoleAssignmentsManage = "identity.role_assignments.manage";
    public const string AccessDecisionsRead = "identity.access_decisions.read";
    public const string AuditRead = "identity.audit.read";

    public static readonly IReadOnlySet<string> SchoolAdministratorPermissions = new HashSet<string>
    {
        StudentProfilesRead, StudentProfilesCreate, StudentProfilesUpdate, StudentProfilesDeactivate, StudentProfilesReviewHistory,
        GuardiansRead, GuardiansCreate, GuardiansUpdate, GuardianLinksCreate, GuardianLinksApprove, GuardianLinksSuspend, GuardianLinksRemove,
        CredentialsRead, CredentialsIssue, CredentialsSuspend, CredentialsRestore, CredentialsReplace, CredentialsRevoke, CredentialsRotateQr,
        RolesRead, RolesManage, PermissionsRead, PermissionsManage, RoleAssignmentsManage, AccessDecisionsRead, AuditRead
    };
}
