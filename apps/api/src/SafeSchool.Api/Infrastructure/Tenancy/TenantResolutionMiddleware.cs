namespace SafeSchool.Api.Infrastructure.Tenancy;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, ITenantContext tenantContext)
    {
        tenantContext.TenantId = httpContext.Request.Headers["X-School-Account-Id"].FirstOrDefault();
        tenantContext.ActorReference = httpContext.User.Identity?.Name
            ?? httpContext.Request.Headers["X-Actor-Reference"].FirstOrDefault()
            ?? "anonymous";
        tenantContext.HasPlatformReviewScope = httpContext.Request.Headers["X-Platform-Review"].FirstOrDefault() == "true";

        var routeTenant = httpContext.Request.RouteValues["schoolAccountId"]?.ToString();
        if (!string.IsNullOrWhiteSpace(routeTenant) && tenantContext.TenantId != routeTenant && !tenantContext.HasPlatformReviewScope)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsJsonAsync(new { code = "tenant_mismatch", message = "Tenant context does not match the requested school account." });
            return;
        }

        await next(httpContext);
    }
}
