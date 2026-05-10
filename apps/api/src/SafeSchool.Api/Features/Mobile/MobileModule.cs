namespace SafeSchool.Api.Features.Mobile;

public static class MobileModule
{
    public const string RoutePrefix = "/api/v1/mobile";

    public static IServiceCollection AddMobileFeature(this IServiceCollection services)
    {
        services.AddSingleton<MobileAuditService>();
        services.AddSingleton<ApkReleaseRepository>();
        services.AddSingleton<MobileOfflineActionSyncService>();
        services.AddSingleton<OfflinePolicyAdapters>();
        services.AddScoped<MobileFeatureAvailabilityService>();
        services.AddScoped<MobilePermissionResolver>();
        services.AddScoped<MobileLanguageService>();
        services.AddScoped<DeviceSessionService>();
        services.AddScoped<MobileProfileService>();
        services.AddScoped<RoleWorkspaceResolver>();
        services.AddScoped<GuardianMobileService>();
        services.AddScoped<StudentMobileService>();
        services.AddScoped<StaffWorkspaceService>();
        services.AddScoped<SourceDomainActionAdapters>();
        services.AddScoped<GuardianStudentActionRouter>();
        services.AddScoped<StaffSourceActionRouter>();
        services.AddScoped<ApkReleaseService>();
        services.AddScoped<ReleaseAudienceService>();
        services.AddScoped<MobileVersionPolicyService>();
        services.AddScoped<ApkArtifactStorage>();
        services.AddScoped<ReleaseNotesValidator>();
        services.AddScoped<MobileSupportMetrics>();
        services.AddScoped<TransportDriverMobileAdapter>();
        services.AddScoped<GateAccessMobileAdapter>();
        services.AddScoped<CanteenCashierMobileAdapter>();
        services.AddScoped<TeacherMobileAdapter>();
        services.AddScoped<MedicalStaffMobileAdapter>();
        services.AddScoped<ComplaintHandlerMobileAdapter>();
        services.AddScoped<CommunicationSenderMobileAdapter>();
        services.AddScoped<DocumentAdministratorMobileAdapter>();
        services.AddScoped<SchoolAdministratorMobileAdapter>();
        return services;
    }

    public static IEndpointRouteBuilder MapMobileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mobile = endpoints.MapGroup(RoutePrefix);
        mobile.MapGet("/", () => Results.Ok(new { phase = "role-based-mobile-apk", roles = MobileRoleCodes.All, languages = new[] { "ar", "en" }, apk = "controlled" }));
        mobile.MapMobileProfileEndpoints();
        mobile.MapMobileContextEndpoints();
        mobile.MapRoleWorkspaceEndpoints();
        mobile.MapGuardianMobileEndpoints();
        mobile.MapStudentMobileEndpoints();
        mobile.MapStaffWorkspaceEndpoints();
        mobile.MapMobileReleaseLookupEndpoints();
        mobile.MapMobileReleaseAdminEndpoints();
        mobile.MapMobileInstallEventEndpoints();
        mobile.MapMobileOfflineActionEndpoints();
        mobile.MapMobileNotificationEndpoints();
        mobile.MapMobileSupportDeviceSessionEndpoints();
        mobile.MapMobileSupportAuditEventEndpoints();
        mobile.MapMobileSupportInstallEventEndpoints();
        mobile.MapMobileSupportDiagnosticsEndpoints();
        return endpoints;
    }
}
