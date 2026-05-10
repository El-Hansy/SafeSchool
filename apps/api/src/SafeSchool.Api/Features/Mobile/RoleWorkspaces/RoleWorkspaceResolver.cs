namespace SafeSchool.Api.Features.Mobile;

public sealed class RoleWorkspaceResolver(MobileFeatureAvailabilityService features, MobilePermissionResolver permissions)
{
    public IReadOnlyList<MobileWorkspaceSummary> ResolveVisible(string tenantId, string roleCode, string languageCode) =>
        MobileSeedCatalog.Workspaces
            .Where(workspace => permissions.CanUseWorkspace(tenantId, roleCode, workspace.WorkspaceCode) || roleCode == MobileRoleCodes.SchoolAdministrator || roleCode == MobileRoleCodes.PlatformSupport)
            .Select(workspace => ToSummary(workspace, languageCode))
            .ToArray();

    public MobileWorkspaceSummary ResolveOne(string tenantId, string workspaceCode, string languageCode)
    {
        var workspace = MobileSeedCatalog.Workspaces.FirstOrDefault(item => item.WorkspaceCode == workspaceCode)
            ?? MobileSeedCatalog.Workspaces.First(item => item.WorkspaceCode == MobileRoleCodes.Guardian);
        return ToSummary(workspace, languageCode);
    }

    public MobileWorkspaceDetail Detail(string tenantId, string workspaceCode, string languageCode)
    {
        var summary = ResolveOne(tenantId, workspaceCode, languageCode);
        var notifications = new[]
        {
            new MobileNotificationDto("notif-1", "communications", languageCode == "ar" ? "تحديث المدرسة" : "School update", languageCode == "ar" ? "لديك تحديث جديد" : "You have a new update", DateTimeOffset.UtcNow, "Unread", "/notifications/notif-1")
        };
        return new MobileWorkspaceDetail(summary.WorkspaceCode, summary.RoleCode, workspaceCode is MobileRoleCodes.Guardian or MobileRoleCodes.Student ? "student-amina" : null, summary.SummaryCards, summary.Actions, [], notifications, ["No hidden features are shown."], languageCode, MobileLanguageService.DirectionFor(languageCode));
    }

    private MobileWorkspaceSummary ToSummary(RoleWorkspace workspace, string languageCode) =>
        new(
            workspace.WorkspaceCode,
            languageCode == "ar" ? ArabicName(workspace.WorkspaceCode) : MobileSeedCatalog.DisplayName(workspace.WorkspaceCode),
            workspace.RequiredRoleCode,
            features.EnabledFeatures(workspace.WorkspaceCode).Select(feature => $"{feature}:enabled").ToArray(),
            MobileSeedCatalog.ActionsFor(workspace.WorkspaceCode).Select(action => new MobileWorkspaceActionDto(action.ActionCode, action.SourceFeatureCode, action.RequiredPermissionCode, action.OfflinePolicy.ToString(), action.AuditLevel.ToString())).ToArray(),
            features.EnabledFeatures(workspace.WorkspaceCode),
            MobileSeedCatalog.ActionsFor(workspace.WorkspaceCode).Where(action => action.OfflinePolicy != MobileOfflinePolicy.NotSupported).Select(action => action.ActionCode).ToArray());

    private static string ArabicName(string workspaceCode) => workspaceCode switch
    {
        MobileRoleCodes.Guardian => "ولي الأمر",
        MobileRoleCodes.Student => "الطالب",
        MobileRoleCodes.TransportDriver => "سائق الحافلة",
        MobileRoleCodes.GateAccess => "بوابة المدرسة",
        MobileRoleCodes.CanteenCashier => "المقصف",
        MobileRoleCodes.Teacher => "المعلم",
        MobileRoleCodes.MedicalStaff => "العيادة",
        MobileRoleCodes.ComplaintHandler => "الشكاوى",
        MobileRoleCodes.CommunicationSender => "الرسائل",
        MobileRoleCodes.DocumentAdministrator => "الوثائق",
        MobileRoleCodes.SchoolAdministrator => "إدارة المدرسة",
        MobileRoleCodes.PlatformSupport => "الدعم",
        _ => workspaceCode
    };
}
