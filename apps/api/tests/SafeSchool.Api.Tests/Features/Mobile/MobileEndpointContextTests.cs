using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using SafeSchool.Api.Infrastructure.Tenancy;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileEndpointContextTests
{
    [Fact]
    public void Mobile_context_resolves_request_and_header_identity()
    {
        var tenant = new TenantContext { TenantId = "school-header", ActorReference = "guardian-1" };

        MobileEndpointContext.Tenant(null, tenant).Should().Be("school-header");
        MobileEndpointContext.Tenant("school-request", tenant).Should().Be("school-request");
        MobileEndpointContext.User(null, tenant).Should().Be("guardian-1");
        MobileEndpointContext.Device(null).Should().Be(MobileEndpointContext.UnknownDevice);
    }

    [Fact]
    public void Mobile_context_detects_cross_tenant_conflicts()
    {
        MobileEndpointContext.HasTenantConflict(
            "school-request",
            new TenantContext { TenantId = "school-header" }).Should().BeTrue();

        MobileEndpointContext.HasTenantConflict(
            "school-request",
            new TenantContext { TenantId = "school-header", HasPlatformReviewScope = true }).Should().BeFalse();
    }
}
