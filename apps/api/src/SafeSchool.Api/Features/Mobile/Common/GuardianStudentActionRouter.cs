namespace SafeSchool.Api.Features.Mobile;

public sealed class GuardianStudentActionRouter(SourceDomainActionAdapters adapters)
{
    public MobileActionResponse Route(string tenantId, string roleCode, MobileActionRequest request) =>
        adapters.Execute(tenantId, $"{roleCode}-demo", "device-demo", roleCode, request);
}
