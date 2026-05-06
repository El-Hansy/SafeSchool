namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class IdentityAccessCapabilities
{
    public const string StudentProfiles = "identity.student_profiles";
    public const string GuardianLinking = "identity.guardian_linking";
    public const string NfcCredentials = "identity.nfc_credentials";
    public const string QrFallback = "identity.qr_fallback";
    public const string RoleAdministration = "identity.role_administration";
    public const string PermissionEnforcement = "identity.permission_enforcement";

    public static readonly string[] All =
    [
        StudentProfiles,
        GuardianLinking,
        NfcCredentials,
        QrFallback,
        RoleAdministration,
        PermissionEnforcement
    ];
}
