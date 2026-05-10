using FluentAssertions;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Security;

public sealed class GuardianTransportEndpointTenantScopeTests
{
    [Fact]
    public void Guardian_transport_controllers_do_not_hardcode_tenant_or_guardian_actor()
    {
        var projectRoot = FindProjectRoot();
        var controllerFiles = new[]
        {
            "apps/api/src/SafeSchool.Api/Features/Transport/Assignments/BusAssignmentControllers.cs",
            "apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingControllers.cs",
            "apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaControllers.cs",
            "apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationControllers.cs"
        };

        foreach (var relativePath in controllerFiles)
        {
            var source = File.ReadAllText(Path.Combine(projectRoot, relativePath));
            source.Should().NotContain("\"school-1\"");
            source.Should().NotContain("\"guardian:me\"");
            source.Should().Contain("GuardianTenantResolver.Resolve(tenantContext)");
            source.Should().Contain("ActorReference");
        }
    }

    private static string FindProjectRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull();
        return directory!.FullName;
    }
}
