using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

namespace SafeSchool.Api.Features.IdentityAccess;

public static class IdentityAccessEndpointRegistration
{
    public const string RoutePrefix = "/api/v1/schools/{schoolAccountId}/identity";

    public static IEndpointRouteBuilder MapIdentityAccessEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(RoutePrefix);
        group.MapStudentProfileEndpoints();
        group.MapAccessControlEndpoints();
        group.MapGuardianEndpoints();
        group.MapCredentialEndpoints();
        return endpoints;
    }
}
