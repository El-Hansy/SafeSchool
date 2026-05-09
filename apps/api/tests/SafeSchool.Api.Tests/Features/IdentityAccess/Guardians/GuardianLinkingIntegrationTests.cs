using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Guardians;

public sealed class GuardianLinkingIntegrationTests
{
    [Fact]
    public async Task GuardianVisibilityService_ExcludesSuspendedLinks()
    {
        var fixture = new IdentityAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        dbContext.GuardianLinks.Add(new GuardianLink
        {
            TenantId = "school-1",
            StudentProfileId = Guid.NewGuid(),
            GuardianId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            RelationshipType = "Parent",
            AccessScope = new Dictionary<string, string> { ["student_profile"] = "read" },
            LinkStatus = GuardianLinkStatus.Suspended,
            RequestedBy = "staff:admin",
            ReviewReason = "Suspended during review."
        });
        await dbContext.SaveChangesAsync();
        var service = new GuardianVisibilityService(dbContext);

        var visible = await service.VisibleStudentIdsAsync("school-1", Guid.Parse("11111111-1111-1111-1111-111111111111"));

        visible.Should().BeEmpty();
    }
}
