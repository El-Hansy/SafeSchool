using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class RouteTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<RouteTraceResponse?> TraceAsync(string tenantId, Guid routeId, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.TransportRoutes.AnyAsync(x => x.TenantId == tenantId && x.Id == routeId, cancellationToken);
        if (!exists) return null;
        var references = new List<TransportTraceReference>
        {
            new("Route", routeId.ToString(), "Route plan"),
            new("Assignments", routeId.ToString(), "Assignments referencing this route"),
            new("Trips", routeId.ToString(), "Trips using this route")
        };
        return new RouteTraceResponse(routeId, references);
    }
}
