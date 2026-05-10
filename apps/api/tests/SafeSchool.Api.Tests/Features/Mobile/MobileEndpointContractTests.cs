using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileEndpointContractTests
{
    [Fact]
    public async Task Mobile_endpoints_match_flutter_bootstrap_contracts()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var profile = await client.GetFromJsonAsync<JsonObject>(
            "/api/v1/mobile/profile?tenantId=school-demo&roleCode=guardian&languageCode=ar");
        profile.Should().NotBeNull();
        profile!["activeTenantId"]!.GetValue<string>().Should().Be("school-demo");
        profile["textDirection"]!.GetValue<string>().Should().Be("rtl");
        profile["linkedStudents"]!.AsArray().Select(node => node!.GetValue<string>())
            .Should().Contain("student-amina");

        var workspaces = await client.GetFromJsonAsync<JsonArray>(
            "/api/v1/mobile/workspaces?tenantId=school-demo&roleCode=guardian&languageCode=en");
        workspaces.Should().NotBeNull();
        workspaces!.Should().ContainSingle(node =>
            node!["workspaceCode"]!.GetValue<string>() == "guardian" &&
            node["actions"]!.AsArray().Count > 0);

        var contextResponse = await client.PostAsJsonAsync("/api/v1/mobile/context", new
        {
            tenantId = "school-demo",
            roleCode = "transport_driver",
            languageCode = "en",
            deviceId = "device-contract"
        });
        contextResponse.EnsureSuccessStatusCode();
        var context = await contextResponse.Content.ReadFromJsonAsync<JsonObject>();
        context.Should().NotBeNull();
        context!["activeRoleCode"]!.GetValue<string>().Should().Be("transport_driver");
        context["workspaceSummary"]!["workspaceCode"]!.GetValue<string>().Should().Be("transport_driver");

        var actionResponse = await client.PostAsJsonAsync(
            "/api/v1/mobile/actions/transport.scan?tenantId=school-demo&userId=driver-contract&deviceId=device-contract",
            new
            {
                workspaceCode = "transport_driver",
                sourceFeatureCode = "transport",
                targetId = "trip-1",
                clientActionId = "",
                payload = "{}",
                localOccurredAt = DateTimeOffset.UtcNow
            });
        actionResponse.EnsureSuccessStatusCode();
        var action = await actionResponse.Content.ReadFromJsonAsync<JsonObject>();
        action.Should().NotBeNull();
        action!["actionResult"]!.GetValue<string>().Should().Be("Accepted");

        var release = await client.GetFromJsonAsync<JsonObject>(
            "/api/v1/mobile/releases/current?tenantId=school-demo&roleCode=guardian&deviceId=device-contract&versionCode=1200");
        release.Should().NotBeNull();
        release!["versionName"]!.GetValue<string>().Should().Be("12.0.0");
        release["downloadAllowed"]!.GetValue<bool>().Should().BeTrue();
        release["updateRequired"]!.GetValue<bool>().Should().BeFalse();

        var installResponse = await client.PostAsJsonAsync("/api/v1/mobile/install-events", new
        {
            deviceId = "device-contract",
            tenantId = "school-demo",
            userId = "guardian-demo",
            releaseId = release["releaseId"]!.GetValue<string>(),
            versionName = "12.0.0",
            versionCode = 1200,
            eventType = "Launch",
            eventResult = "Allowed",
            occurredAt = DateTimeOffset.UtcNow
        });
        installResponse.EnsureSuccessStatusCode();
        var install = await installResponse.Content.ReadFromJsonAsync<JsonObject>();
        install.Should().NotBeNull();
        install!["accepted"]!.GetValue<bool>().Should().BeTrue();
        install["nextAction"]!.GetValue<string>().Should().Be("continue");

        var readResponse = await client.PostAsync(
            "/api/v1/mobile/notifications/mobile-notif-1/read?tenantId=school-demo&userId=guardian-contract&deviceId=device-contract&roleCode=guardian",
            content: null);
        readResponse.EnsureSuccessStatusCode();
        var read = await readResponse.Content.ReadFromJsonAsync<JsonObject>();
        read.Should().NotBeNull();
        read!["readState"]!.GetValue<string>().Should().Be("Read");

        var audit = await client.GetFromJsonAsync<JsonObject>(
            "/api/v1/mobile/support/audit-events?tenantId=school-demo");
        audit.Should().NotBeNull();
        var auditEvents = audit!["items"]!.AsArray();
        auditEvents.Select(node => node!["actorUserId"]!.GetValue<string>())
            .Should().Contain(["driver-contract", "guardian-contract"]);
    }
}
