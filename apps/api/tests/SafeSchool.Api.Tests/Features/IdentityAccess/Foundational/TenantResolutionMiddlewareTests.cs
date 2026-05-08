using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SafeSchool.Api.Infrastructure.Tenancy;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Foundational;

public sealed class TenantResolutionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_StoresTenantAndActor()
    {
        var invoked = false;
        var middleware = new TenantResolutionMiddleware(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });
        var context = new DefaultHttpContext();
        context.Request.Headers["X-School-Account-Id"] = "school-1";
        context.Request.Headers["X-Actor-Reference"] = "staff:admin";
        context.Request.RouteValues["schoolAccountId"] = "school-1";
        var tenantContext = new TenantContext();

        await middleware.InvokeAsync(context, tenantContext);

        invoked.Should().BeTrue();
        tenantContext.TenantId.Should().Be("school-1");
        tenantContext.ActorReference.Should().Be("staff:admin");
    }

    [Fact]
    public async Task InvokeAsync_RejectsTenantMismatch()
    {
        var middleware = new TenantResolutionMiddleware(_ => Task.CompletedTask);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-School-Account-Id"] = "school-1";
        context.Request.RouteValues["schoolAccountId"] = "school-2";

        await middleware.InvokeAsync(context, new TenantContext());

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }
}
