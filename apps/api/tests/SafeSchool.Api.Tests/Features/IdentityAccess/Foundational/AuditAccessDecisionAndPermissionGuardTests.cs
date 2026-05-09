using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Features.IdentityAccess.Audit;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Foundational;

public sealed class AuditAccessDecisionAndPermissionGuardTests
{
    private readonly IdentityAccessTestFixture _fixture = new();

    [Fact]
    public void PermissionGuard_AllowsSeededSchoolAdministratorPermission()
    {
        var guard = new PermissionGuard(new FeatureGateService(_fixture.EnabledConfiguration()), _fixture.Tenant());

        var result = guard.Require(
            "school-1",
            IdentityAccessCapabilities.StudentProfiles,
            PermissionCatalog.StudentProfilesCreate,
            PermissionCatalog.SchoolAdministratorPermissions);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task AuditAndAccessDecisionWriters_PersistTenantScopedEvidence()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var tenant = _fixture.Tenant();
        var auditWriter = new AuditWriter(dbContext, tenant);
        var decisionWriter = new AccessDecisionWriter(dbContext, tenant);

        await auditWriter.RecordAsync(new AuditEvent
        {
            TenantId = "school-1",
            EventCategory = "Student Profile",
            EventType = "identity.student_profiles.create",
            ActorReference = "",
            SubjectType = "Student Profile",
            SubjectReference = "S-1001",
            Reason = "Created for enrollment review."
        });
        await decisionWriter.RecordAsync(new AccessDecision
        {
            TenantId = "school-1",
            ActorReference = "",
            AttemptedAction = "identity.student_profiles.create",
            TargetType = "Student Profile",
            TargetReference = "S-1001",
            Decision = AccessDecisionResult.Allowed,
            DecisionReason = "School administrator permission.",
            FeatureCapabilityKey = IdentityAccessCapabilities.StudentProfiles
        });

        dbContext.AuditEvents.Should().ContainSingle(x => x.ActorReference == "staff:admin");
        dbContext.AccessDecisions.Should().ContainSingle(x => x.ActorReference == "staff:admin");
    }
}
