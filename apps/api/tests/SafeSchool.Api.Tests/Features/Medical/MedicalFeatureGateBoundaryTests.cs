using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Medical;

public sealed class MedicalFeatureGateBoundaryTests
{
    [Fact]
    public async Task Medical_endpoints_reject_disabled_tenant_capabilities()
    {
        await using var factory = FactoryWithFeatureDefaults("Disabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");

        var response = await client.GetAsync("/api/v1/schools/school-demo/medical");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("feature_disabled");
        body.Should().Contain("medical.records");
    }

    [Fact]
    public async Task Medical_endpoints_reject_school_tenant_mismatch()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "other-school");

        var response = await client.GetAsync("/api/v1/schools/school-demo/medical");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).Should().Contain("tenant_mismatch");
    }

    [Fact]
    public async Task Medical_emergency_endpoint_requires_reason_before_opening_access()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");

        var response = await client.PostAsJsonAsync("/api/v1/schools/school-demo/medical/emergency/access", new
        {
            studentProfileId = "student-1",
            reason = "",
            actorId = "nurse-1",
            clientRequestId = "emg-validation"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("ValidationFailed");
        body.Should().Contain("Emergency reason is required");
    }

    [Fact]
    public async Task Medical_break_glass_endpoint_rejects_non_emergency_roles()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");

        var response = await client.PostAsJsonAsync("/api/v1/schools/school-demo/medical/emergency/break-glass", new
        {
            studentProfileId = "student-1",
            reason = "Emergency",
            actorId = "teacher-1",
            clientRequestId = "emg-denied",
            actorRole = "teacher",
            confirmed = true
        });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Medical_endpoints_reject_explicit_missing_permission()
    {
        await using var factory = FactoryWithFeatureDefaults("Enabled");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-School-Account-Id", "school-demo");
        client.DefaultRequestHeaders.Add("X-Actor-Permissions", "unrelated.permission");

        var response = await client.GetAsync("/api/v1/schools/school-demo/medical");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("missing_permission");
        body.Should().Contain("medical.records.read");
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
