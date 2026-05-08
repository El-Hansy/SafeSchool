using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Features.Transport.Tracking;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Trips;

public sealed class TripTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<TripTraceResponse?> TraceAsync(string tenantId, Guid tripId, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == tripId, cancellationToken);
        if (trip is null) return null;
        return new TripTraceResponse(tripId, [new TransportTraceReference("Route", trip.TransportRouteId.ToString(), "Route"), new TransportTraceReference("Vehicle", trip.TransportVehicleId.ToString(), "Vehicle"), new TransportTraceReference("Locations", tripId.ToString(), "Location updates"), new TransportTraceReference("Scans", tripId.ToString(), "Scan events")]);
    }
}
