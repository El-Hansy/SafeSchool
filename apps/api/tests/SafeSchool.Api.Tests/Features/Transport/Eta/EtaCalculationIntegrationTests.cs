using FluentAssertions;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Eta;
using SafeSchool.Api.Tests.Features.Transport.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Eta;

public sealed class EtaCalculationIntegrationTests
{
    [Fact]
    public async Task GuardianEtaVisibility_reads_only_eta_records_in_requested_tenant()
    {
        var fixture = new TransportTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var liveTripId = Guid.NewGuid();
        var otherTripId = Guid.NewGuid();
        var liveStopId = Guid.NewGuid();
        dbContext.EtaRecords.AddRange(
            new EtaRecord
            {
                TenantId = "school-live",
                TransportTripId = liveTripId,
                TransportRouteId = Guid.NewGuid(),
                RouteStopSequenceId = liveStopId,
                StudentProfileId = "student-amina",
                EtaState = EtaState.Available,
                ConfidenceState = EtaConfidenceState.High,
                FreshnessStatus = LocationFreshnessStatus.Current,
                CalculatedAt = DateTimeOffset.UtcNow
            },
            new EtaRecord
            {
                TenantId = "school-other",
                TransportTripId = otherTripId,
                TransportRouteId = Guid.NewGuid(),
                RouteStopSequenceId = Guid.NewGuid(),
                StudentProfileId = "student-amina",
                EtaState = EtaState.Available,
                ConfidenceState = EtaConfidenceState.High,
                FreshnessStatus = LocationFreshnessStatus.Current,
                CalculatedAt = DateTimeOffset.UtcNow.AddMinutes(1)
            });
        await dbContext.SaveChangesAsync();

        var service = new GuardianEtaVisibilityService(dbContext, new FakeTransportGuardianLinkProvider());

        var result = await service.ReadAsync("school-live", "guardian-live", "student-amina", liveTripId);

        result.Succeeded.Should().BeTrue();
        result.Value!.PickupEta.Should().NotBeNull();
        result.Value.PickupEta!.SchoolAccountId.Should().Be("school-live");
        result.Value.PickupEta.RouteStopSequenceId.Should().Be(liveStopId);
    }
}
