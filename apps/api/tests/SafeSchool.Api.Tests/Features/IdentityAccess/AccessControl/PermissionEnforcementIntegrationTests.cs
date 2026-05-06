using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.AccessControl;

public sealed class PermissionEnforcementIntegrationTests
{
    [Fact]
    public void PermissionEvaluator_DeniesCrossTenantTargetBeforeMutation()
    {
        var fixture = new IdentityAccessTestFixture();
        var evaluator = new PermissionEvaluator(new FeatureGateService(fixture.EnabledConfiguration()), fixture.Tenant());

        var result = evaluator.Evaluate("school-1", IdentityAccessCapabilities.PermissionEnforcement, "identity.roles.manage", [], ["identity.roles.manage"], "school-2");

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.Code == "target_tenant_mismatch");
    }
}
