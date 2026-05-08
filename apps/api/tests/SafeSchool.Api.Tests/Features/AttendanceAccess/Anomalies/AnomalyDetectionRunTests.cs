using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionRunTests
{
    [Fact]
    public async Task RunAsync_IsIdempotentForSameEvidence()
    {
        var fixture = new AttendanceAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        dbContext.GateScanEvents.Add(new GateScanEvent { TenantId = "school-1", StudentProfileId = "student-1", Status = ScanEventStatus.Flagged, DecisionReason = "invalid" });
        await dbContext.SaveChangesAsync();
        var service = new AnomalyDetectionService(dbContext, new AnomalyDetectionRuleSet());

        await service.RunAsync("school-1", new RunAnomalyDetectionRequest("run-1"));
        await service.RunAsync("school-1", new RunAnomalyDetectionRequest("run-1"));

        dbContext.AttendanceAnomalies.Should().ContainSingle();
    }
}

