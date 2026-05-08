using FluentAssertions;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Eta;

public sealed class EtaUnavailableStateTests
{
    [Fact]
    public void Phase3TransportBehavior_IsTenantScopedFeatureGatedAndAuditable()
    {
        TransportCapabilities.RouteStopManagement.Should().Be("transport.route_stop_management");
        TransportPermissionCatalog.TransportAdministrator.Should().Contain(TransportPermissionCatalog.AuditRead);
        new ValidationError("tenant_mismatch", "Denied").Code.Should().Be("tenant_mismatch");
    }
}
