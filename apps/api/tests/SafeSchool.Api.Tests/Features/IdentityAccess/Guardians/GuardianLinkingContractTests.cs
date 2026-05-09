using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Guardians;

public sealed class GuardianLinkingContractTests
{
    [Fact]
    public void GuardianLinkRequest_CarriesExplicitAccessScope()
    {
        var request = new GuardianLinkRequest(
            Guid.NewGuid(),
            "Parent",
            new Dictionary<string, string> { ["student_profile"] = "read" },
            DateTimeOffset.UtcNow,
            null,
            "Approved",
            "Verified documents.",
            "request-1");

        request.AccessScope.Should().ContainKey("student_profile");
    }
}
