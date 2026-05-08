using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.AccessControl;

public sealed class PermissionEnforcementContractTests
{
    [Fact]
    public void PermissionCatalog_IncludesReviewAndRoleAdministrationFamilies()
    {
        PermissionCatalog.SchoolAdministratorPermissions.Should().Contain(PermissionCatalog.RoleAdministration);
        PermissionCatalog.SchoolAdministratorPermissions.Should().Contain(PermissionCatalog.AuditRead);
    }
}
