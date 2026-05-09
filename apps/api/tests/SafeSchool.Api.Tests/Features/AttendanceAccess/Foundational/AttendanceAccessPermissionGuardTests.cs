using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Foundational;

public sealed class AttendanceAccessPermissionGuardTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public async Task RequireAsync_RecordsDeniedDecisionForDisabledCapability()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var tenant = _fixture.Tenant();
        var guard = _fixture.Guard(dbContext, tenant, _fixture.DisabledConfiguration());

        var result = await guard.RequireAsync("school-1", AttendanceAccessCapabilities.GateScanning, AttendanceAccessPermissionCatalog.ScansRecord);

        result.Succeeded.Should().BeFalse();
        dbContext.AccessDecisions.Should().ContainSingle(x => x.FeatureCapabilityKey == AttendanceAccessCapabilities.GateScanning);
    }
}

