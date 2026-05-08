using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.AccessControl;

public sealed class PermissionEvaluatorTests
{
    [Fact]
    public void Evaluate_AllowsActiveAssignmentWithRequiredPermission()
    {
        var fixture = new IdentityAccessTestFixture();
        var evaluator = new PermissionEvaluator(new FeatureGateService(fixture.EnabledConfiguration()), fixture.Tenant());
        var assignment = new ActorRoleAssignment
        {
            TenantId = "school-1",
            ActorReference = "staff:admin",
            RoleId = Guid.NewGuid(),
            AssignmentStatus = AssignmentStatus.Active,
            AssignedBy = "platform:owner",
            ReviewReason = "Test assignment."
        };

        var result = evaluator.Evaluate(
            "school-1",
            IdentityAccessCapabilities.PermissionEnforcement,
            PermissionCatalog.RoleAdministration,
            [assignment],
            [PermissionCatalog.RoleAdministration],
            "school-1");

        result.Succeeded.Should().BeTrue();
    }
}
