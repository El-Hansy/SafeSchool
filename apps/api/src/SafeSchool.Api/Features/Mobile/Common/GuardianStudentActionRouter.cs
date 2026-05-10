namespace SafeSchool.Api.Features.Mobile;

public sealed class GuardianStudentActionRouter(SourceDomainActionAdapters adapters)
{
    public MobileActionResponse Route(string tenantId, string userId, string deviceId, string roleCode, MobileActionRequest request) =>
        adapters.Execute(tenantId, userId, deviceId, roleCode, request);
}
