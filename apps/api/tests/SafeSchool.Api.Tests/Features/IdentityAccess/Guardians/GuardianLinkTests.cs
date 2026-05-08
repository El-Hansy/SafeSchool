using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Guardians;

public sealed class GuardianLinkTests
{
    [Fact]
    public void GrantsStudentVisibility_RequiresApprovedCurrentReadScope()
    {
        var link = new GuardianLink
        {
            TenantId = "school-1",
            StudentProfileId = Guid.NewGuid(),
            GuardianId = Guid.NewGuid(),
            RelationshipType = "Parent",
            AccessScope = new Dictionary<string, string> { ["student_profile"] = "read" },
            LinkStatus = GuardianLinkStatus.Approved,
            ValidFrom = DateTimeOffset.UtcNow.AddDays(-1),
            RequestedBy = "staff:admin",
            ReviewReason = "Approved by registrar."
        };

        link.GrantsStudentVisibility(DateTimeOffset.UtcNow).Should().BeTrue();
    }
}
