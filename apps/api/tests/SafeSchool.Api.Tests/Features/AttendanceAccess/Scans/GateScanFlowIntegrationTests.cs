using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Features.AttendanceAccess.Sync;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Scans;

public sealed class GateScanFlowIntegrationTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public async Task RecordAsync_CreatesScanAndCampusDecision()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var seeded = await _fixture.SeedActiveGateAsync(dbContext);
        var evidence = new FakeIdentityCredentialEvidenceProvider();
        evidence.Add(AttendanceAccessTestData.ActiveCredential());
        var service = new OfflineScanSyncService(dbContext, new ScanValidationService(dbContext, _fixture.Guard(dbContext, _fixture.Tenant()), evidence), new CampusAccessDecisionService(dbContext), new IdempotencyService());

        var response = await service.RecordAsync("school-1", AttendanceAccessTestData.EntryScan(seeded.Gate.Id, seeded.ScanPoint.Id));

        response.StudentProfileId.Should().Be("student-active");
        dbContext.CampusAccessDecisions.Should().ContainSingle(x => x.GateScanEventId == response.ScanEventId);
    }
}

