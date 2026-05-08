using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Guardians;

public sealed class GuardianRecordTests
{
    [Fact]
    public void GuardianRecord_DefaultsToActiveAndUnverified()
    {
        var guardian = new GuardianRecord
        {
            TenantId = "school-1",
            DisplayName = "Mona Hassan",
            CreatedBy = "staff:admin",
            UpdatedBy = "staff:admin"
        };

        guardian.GuardianStatus.Should().Be(GuardianStatus.Active);
        guardian.IdentityReviewStatus.Should().Be(GuardianIdentityReviewStatus.Unverified);
    }
}
