namespace SafeSchool.Api.Features.Mobile;

public static class MobileSeedCatalog
{
    private static readonly Dictionary<string, string> DisplayNames = new()
    {
        [MobileRoleCodes.Guardian] = "Guardian",
        [MobileRoleCodes.Student] = "Student",
        [MobileRoleCodes.TransportDriver] = "Transport driver",
        [MobileRoleCodes.GateAccess] = "Gate/access staff",
        [MobileRoleCodes.CanteenCashier] = "Canteen cashier",
        [MobileRoleCodes.Teacher] = "Teacher",
        [MobileRoleCodes.MedicalStaff] = "Medical staff",
        [MobileRoleCodes.ComplaintHandler] = "Complaint handler",
        [MobileRoleCodes.CommunicationSender] = "Communication sender",
        [MobileRoleCodes.DocumentAdministrator] = "Document administrator",
        [MobileRoleCodes.SchoolAdministrator] = "School administrator",
        [MobileRoleCodes.PlatformSupport] = "Platform support"
    };

    public static IReadOnlyList<RoleWorkspace> Workspaces =>
        MobileRoleCodes.All
            .Select(role => new RoleWorkspace
            {
                WorkspaceCode = role,
                RequiredRoleCode = role,
                DisplayNameKey = $"workspace.{role}",
                RequiredPermissionCodes = $"mobile.{role}.access",
                EnabledFeatureCodes = string.Join(',', EnabledFeaturesFor(role))
            })
            .ToArray();

    public static IReadOnlyList<RoleWorkspaceAction> ActionsFor(string workspaceCode)
    {
        var source = workspaceCode switch
        {
            MobileRoleCodes.Guardian => "guardian",
            MobileRoleCodes.Student => "student",
            MobileRoleCodes.TransportDriver => "transport",
            MobileRoleCodes.GateAccess => "attendance_access",
            MobileRoleCodes.CanteenCashier => "wallet",
            MobileRoleCodes.Teacher => "learning",
            MobileRoleCodes.MedicalStaff => "medical",
            MobileRoleCodes.ComplaintHandler => "complaints",
            MobileRoleCodes.CommunicationSender => "communications",
            MobileRoleCodes.DocumentAdministrator => "documents",
            MobileRoleCodes.SchoolAdministrator => "administration",
            MobileRoleCodes.PlatformSupport => "support",
            _ => "mobile"
        };

        var offline = workspaceCode is MobileRoleCodes.TransportDriver or MobileRoleCodes.GateAccess or MobileRoleCodes.CanteenCashier or MobileRoleCodes.MedicalStaff
            ? MobileOfflinePolicy.QueueAllowed
            : MobileOfflinePolicy.NotSupported;

        return
        [
            new RoleWorkspaceAction { WorkspaceCode = workspaceCode, ActionCode = $"{workspaceCode}.view", SourceFeatureCode = source, RequiredPermissionCode = $"mobile.{workspaceCode}.view", OfflinePolicy = MobileOfflinePolicy.NotSupported },
            new RoleWorkspaceAction { WorkspaceCode = workspaceCode, ActionCode = $"{workspaceCode}.act", SourceFeatureCode = source, RequiredPermissionCode = $"mobile.{workspaceCode}.act", OfflinePolicy = offline, AuditLevel = MobileAuditLevel.Sensitive }
        ];
    }

    public static string DisplayName(string workspaceCode) => DisplayNames.TryGetValue(workspaceCode, out var name) ? name : workspaceCode;

    public static IReadOnlyList<string> EnabledFeaturesFor(string workspaceCode) => workspaceCode switch
    {
        MobileRoleCodes.Guardian => ["attendance", "transport", "wallet", "learning", "requests", "complaints", "communications", "documents", "certificates"],
        MobileRoleCodes.Student => ["learning", "communications", "complaints", "documents", "certificates"],
        MobileRoleCodes.TransportDriver => ["transport", "offline_sync"],
        MobileRoleCodes.GateAccess => ["attendance_access", "offline_sync"],
        MobileRoleCodes.CanteenCashier => ["wallet", "offline_sync"],
        MobileRoleCodes.Teacher => ["learning", "attendance"],
        MobileRoleCodes.MedicalStaff => ["medical", "emergency", "offline_sync"],
        MobileRoleCodes.ComplaintHandler => ["complaints"],
        MobileRoleCodes.CommunicationSender => ["communications"],
        MobileRoleCodes.DocumentAdministrator => ["documents", "certificates"],
        MobileRoleCodes.SchoolAdministrator => ["administration", "mobile_permissions", "release_overview"],
        MobileRoleCodes.PlatformSupport => ["support_diagnostics", "audit", "release_evidence"],
        _ => ["mobile"]
    };

    public static ApkRelease ActiveRelease => new()
    {
        VersionName = "12.0.0",
        VersionCode = 1200,
        Environment = ApkEnvironment.Pilot,
        ReleaseStatus = ApkReleaseStatus.Active,
        ArtifactUri = "s3://safeschool-mobile/releases/safeschool-12.0.0.apk",
        ArtifactChecksum = "sha256-demo-phase12",
        ReleaseNotesKey = "Phase 12 mobile app with Arabic, English, and role workspaces.",
        MinimumSupportedVersionCode = 1200
    };
}
