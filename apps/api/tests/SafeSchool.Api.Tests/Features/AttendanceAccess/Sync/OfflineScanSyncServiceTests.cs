using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Features.AttendanceAccess.Sync;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Sync;

public sealed class OfflineScanSyncServiceTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public async Task SyncAsync_DeduplicatesClientBatchAndClientScan()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var seeded = await _fixture.SeedActiveGateAsync(dbContext);
        var evidence = new FakeIdentityCredentialEvidenceProvider();
        evidence.Add(AttendanceAccessTestData.ActiveCredential());
        var validation = new ScanValidationService(dbContext, _fixture.Guard(dbContext, _fixture.Tenant()), evidence);
        var service = new OfflineScanSyncService(dbContext, validation, new CampusAccessDecisionService(dbContext), new IdempotencyService());
        var batch = new OfflineScanBatchRequest("batch-1", [AttendanceAccessTestData.EntryScan(seeded.Gate.Id, seeded.ScanPoint.Id)]);

        var first = await service.SyncAsync("school-1", batch);
        var second = await service.SyncAsync("school-1", batch);

        first.Items.Should().ContainSingle();
        second.Status.Should().Be(OfflineSyncStatus.Duplicate);
    }
}
