namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileProfileService(MobileLanguageService languageService, MobilePermissionResolver permissions)
{
    public MobileProfileDto GetProfile(string tenantId, string roleCode = MobileRoleCodes.Guardian, string userId = "demo-user", string languageCode = "en")
    {
        var language = languageService.Resolve(tenantId, userId, languageCode);
        var denied = permissions.CanAccessTenant(tenantId) ? null : "TENANT_ACCESS_DENIED";
        return new MobileProfileDto(
            userId,
            tenantId,
            ["school-demo", "school-pilot"],
            MobileRoleCodes.All,
            roleCode == MobileRoleCodes.Guardian ? ["student-amina", "student-omar"] : roleCode == MobileRoleCodes.Student ? ["student-self"] : [],
            language.LanguageCode,
            language.TextDirection == MobileTextDirection.Rtl ? "rtl" : "ltr",
            denied is null ? "active" : "blocked",
            denied);
    }
}
