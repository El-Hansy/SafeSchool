using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfilesContractTests
{
    [Fact]
    public void RoutePrefix_ContainsVersionedSchoolIdentityPath()
    {
        IdentityAccessEndpointRegistration.RoutePrefix
            .Should().Be("/api/v1/schools/{schoolAccountId}/identity");
    }
}
