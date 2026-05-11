using System.Net;
using System.Net.Http.Json;
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

    [Fact]
    public async Task Requests_submit_endpoint_returns_bad_request_for_missing_required_fields()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");

        var response = await client.PostAsJsonAsync("/api/v1/schools/school-demo/requests", new
        {
            studentProfileId = "",
            requestType = "early-leave",
            reason = "Medical appointment",
            requestedOutcome = "Release to guardian",
            clientRequestId = "req-validation"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("ValidationFailed");
        body.Should().Contain("Student profile id is required");
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
