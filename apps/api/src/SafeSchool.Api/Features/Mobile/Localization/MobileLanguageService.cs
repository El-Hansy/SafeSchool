namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileLanguageService
{
    public MobileLanguagePreference Resolve(string tenantId, string userId, string requestedLanguage)
    {
        var language = requestedLanguage.Equals("ar", StringComparison.OrdinalIgnoreCase) ? "ar" : "en";
        return new MobileLanguagePreference
        {
            TenantId = tenantId,
            UserId = userId,
            LanguageCode = language,
            TextDirection = language == "ar" ? MobileTextDirection.Rtl : MobileTextDirection.Ltr
        };
    }

    public static string DirectionFor(string languageCode) => languageCode.Equals("ar", StringComparison.OrdinalIgnoreCase) ? "rtl" : "ltr";
}
