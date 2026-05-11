using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Requests;

public sealed class RequestFeatureGateBoundaryTests
{
    [Fact]
    public async Task Requests_endpoints_reject_disabled_tenant_capabilities()
    {
        await using var factory = FactoryWithFeatureDefaults("Disabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");

        var response = await client.GetAsync("/api/v1/schools/school-demo/requests");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("feature_disabled");
        body.Should().Contain("requests.history");
    }

    [Fact]
    public async Task Requests_endpoints_reject_school_tenant_mismatch()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "other-school");

        var response = await client.GetAsync("/api/v1/schools/school-demo/requests");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).Should().Contain("tenant_mismatch");
    }

    private static WebApplicationFactory<Program> FactoryWithFeatureDefaults(string availability) =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration(configuration =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Demo:AllowAnonymousApi"] = "true",
                    ["FeatureSettings:DefaultAvailability"] = availability,
                    ["ConnectionStrings:SafeSchool"] = "Host=localhost;Database=safeschool_dev;Username=safeschool;Password=safeschool"
                }));
        });
}
