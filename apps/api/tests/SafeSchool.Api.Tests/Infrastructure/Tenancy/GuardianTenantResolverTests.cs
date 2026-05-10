using FluentAssertions;
using SafeSchool.Api.Infrastructure.Tenancy;
using Xunit;

namespace SafeSchool.Api.Tests.Infrastructure.Tenancy;

public sealed class GuardianTenantResolverTests
{
    [Fact]
    public void Guardian_tenant_resolves_from_active_tenant_context()
    {
        GuardianTenantResolver.Resolve(new TenantContext { TenantId = "school-live" })
            .Should().Be("school-live");
    }

    [Fact]
    public void Guardian_tenant_keeps_demo_fallback_when_context_is_missing()
    {
        GuardianTenantResolver.Resolve(new TenantContext())
            .Should().Be(GuardianTenantResolver.DemoTenantFallback);
    }
}
