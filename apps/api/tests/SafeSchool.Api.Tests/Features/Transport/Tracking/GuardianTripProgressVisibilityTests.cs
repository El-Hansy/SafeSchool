using FluentAssertions;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Scans;
using SafeSchool.Api.Features.Transport.Tracking;
using SafeSchool.Api.Tests.Features.Transport.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Tracking;

public sealed class GuardianTripProgressVisibilityTests
{
    [Fact]
    public async Task ProgressAsync_exposes_exact_location_only_for_onboard_student_in_requested_tenant()
    {
        var fixture = new TransportTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var liveTripId = Guid.NewGuid();
        var otherTripId = Guid.NewGuid();
        dbContext.BoardingDropScanEvents.AddRange(
            new BoardingDropScanEvent
            {
                TenantId = "school-live",
                TransportTripId = liveTripId,
                StudentProfileId = "student-amina",
                TransportStatusAfter = TransportStatusAfter.Onboard,
                ScanDecision = TransportScanDecision.Accepted
            },
            new BoardingDropScanEvent
            {
                TenantId = "school-other",
                TransportTripId = otherTripId,
                StudentProfileId = "student-amina",
                TransportStatusAfter = TransportStatusAfter.Onboard,
                ScanDecision = TransportScanDecision.Accepted
            });
        dbContext.TransportLocationUpdates.AddRange(
            new TransportLocationUpdate
            {
                TenantId = "school-live",
                TransportTripId = liveTripId,
                AcceptanceStatus = LocationAcceptanceStatus.Accepted,
                LocationReference = "geo:live-bus",
                ReportedAt = DateTimeOffset.UtcNow
            },
            new TransportLocationUpdate
            {
                TenantId = "school-other",
                TransportTripId = otherTripId,
                AcceptanceStatus = LocationAcceptanceStatus.Accepted,
                LocationReference = "geo:other-bus",
                ReportedAt = DateTimeOffset.UtcNow.AddMinutes(1)
            });
        await dbContext.SaveChangesAsync();

        var service = new GuardianTripProgressService(dbContext, new FakeTransportGuardianLinkProvider());

        var result = await service.ProgressAsync("school-live", "guardian-live", "student-amina", liveTripId);

        result.Succeeded.Should().BeTrue();
        result.Value!.VisibilityPhase.Should().Be("Onboard");
        result.Value.ExactLiveLocation.Should().Be("geo:live-bus");
    }
}
