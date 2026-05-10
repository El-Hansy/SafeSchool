namespace SafeSchool.Api.Features.Mobile;

public static class MobileDemoSeed
{
    public static IReadOnlyList<object> Users =>
    [
        new { userId = "guardian-demo", role = MobileRoleCodes.Guardian, tenantId = "school-demo" },
        new { userId = "student-demo", role = MobileRoleCodes.Student, tenantId = "school-demo" },
        new { userId = "driver-demo", role = MobileRoleCodes.TransportDriver, tenantId = "school-demo" },
        new { userId = "cashier-demo", role = MobileRoleCodes.CanteenCashier, tenantId = "school-demo" },
        new { userId = "admin-demo", role = MobileRoleCodes.SchoolAdministrator, tenantId = "school-demo" },
        new { userId = "support-demo", role = MobileRoleCodes.PlatformSupport, tenantId = "school-demo" }
    ];
}
