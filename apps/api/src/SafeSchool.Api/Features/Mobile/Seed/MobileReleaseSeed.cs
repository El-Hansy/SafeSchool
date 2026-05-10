namespace SafeSchool.Api.Features.Mobile;

public static class MobileReleaseSeed
{
    public static object DemoRelease => new
    {
        release = ApkReleaseService.ToDto(MobileSeedCatalog.ActiveRelease, true, false),
        audience = new[] { "school-demo", MobileRoleCodes.Guardian, MobileRoleCodes.Student, MobileRoleCodes.TransportDriver }
    };
}
