using FluentAssertions;
using SafeSchool.Api.Features.Transport.Assignments;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Tests.Features.Transport.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Assignments;

public sealed class GuardianTransportPlanVisibilityTests
{
    [Fact]
    public async Task VisiblePlanAsync_returns_only_guardian_visible_records_for_active_tenant()
    {
        var fixture = new TransportTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        var liveRoute = TransportTestData.ActiveRoute("school-live");
        var otherRoute = TransportTestData.ActiveRoute("school-other");
        dbContext.TransportRoutes.AddRange(liveRoute, otherRoute);
        dbContext.StudentTransportAssignments.AddRange(
            new StudentTransportAssignment
            {
                TenantId = "school-live",
                StudentProfileId = "student-amina",
                TransportRouteId = liveRoute.Id,
                AssignmentStatus = AssignmentStatus.Active,
                VisibilityState = GuardianVisibilityState.GuardianVisible
            },
            new StudentTransportAssignment
            {
                TenantId = "school-live",
                StudentProfileId = "student-amina",
                TransportRouteId = liveRoute.Id,
                AssignmentStatus = AssignmentStatus.Active,
                VisibilityState = GuardianVisibilityState.StaffOnly
            },
            new StudentTransportAssignment
            {
                TenantId = "school-other",
                StudentProfileId = "student-amina",
                TransportRouteId = otherRoute.Id,
                AssignmentStatus = AssignmentStatus.Active,
                VisibilityState = GuardianVisibilityState.GuardianVisible
            });
        await dbContext.SaveChangesAsync();

        var service = new GuardianTransportPlanVisibilityService(dbContext, new FakeTransportGuardianLinkProvider());

        var result = await service.VisiblePlanAsync("school-live", "guardian-live", "student-amina");

        result.Succeeded.Should().BeTrue();
        result.Value!.Assignments.Should().ContainSingle();
        result.Value.Assignments[0].SchoolAccountId.Should().Be("school-live");
        result.Value.Assignments[0].VisibilityState.Should().Be(GuardianVisibilityState.GuardianVisible);
    }
}
