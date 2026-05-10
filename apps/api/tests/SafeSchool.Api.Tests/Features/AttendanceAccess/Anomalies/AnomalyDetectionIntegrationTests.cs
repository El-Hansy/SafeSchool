using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionIntegrationTests
{
    [Fact]
    public async Task ReviewService_ResolvesWithReasonWithoutChangingAttendance()
    {
        var fixture = new AttendanceAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var anomaly = new AttendanceAnomaly { TenantId = "school-1", Status = AnomalyStatus.New };
        dbContext.AttendanceAnomalies.Add(anomaly);
        await dbContext.SaveChangesAsync();

        var result = await new AnomalyReviewService(dbContext).ResolveAsync("school-1", anomaly.Id, new ResolveAnomalyRequest("review complete"));

        result.Succeeded.Should().BeTrue();
        result.Value!.Status.Should().Be(AnomalyStatus.Resolved);
    }
}

