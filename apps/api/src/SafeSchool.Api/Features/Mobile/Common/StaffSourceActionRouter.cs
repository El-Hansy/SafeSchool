namespace SafeSchool.Api.Features.Mobile;

public sealed class StaffSourceActionRouter(SourceDomainActionAdapters adapters)
{
    public MobileActionResponse Route(string tenantId, string roleCode, MobileActionRequest request) =>
        adapters.Execute(tenantId, $"staff-{roleCode}", "device-staff", roleCode, request);
}
