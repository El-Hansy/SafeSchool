using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfilesIntegrationTests
{
    [Fact]
    public void PermissionGuard_DeniesProfileMutationWithoutPermission()
    {
        var fixture = new IdentityAccessTestFixture();
        var guard = new PermissionGuard(new FeatureGateService(fixture.EnabledConfiguration()), fixture.Tenant());

        var result = guard.Require("school-1", IdentityAccessCapabilities.StudentProfiles, PermissionCatalog.StudentProfilesCreate, []);

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.Code == "missing_permission");
    }
}
