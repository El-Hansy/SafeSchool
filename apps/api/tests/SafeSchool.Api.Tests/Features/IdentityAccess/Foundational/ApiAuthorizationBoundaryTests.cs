using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Foundational;

public sealed class ApiAuthorizationBoundaryTests
{
    [Fact]
    public async Task Production_api_requires_authenticated_actor_by_default()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        using var client = factory.CreateClient();

        var protectedResponse = await client.GetAsync("/api/v1/mobile/profile?tenantId=school-demo&roleCode=guardian&languageCode=en");
        protectedResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var healthResponse = await client.GetAsync("/health");
        healthResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
