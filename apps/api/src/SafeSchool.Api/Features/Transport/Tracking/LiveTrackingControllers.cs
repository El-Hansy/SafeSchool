using SafeSchool.Api.Features.Transport.Trips;

namespace SafeSchool.Api.Features.Transport.Tracking;

public static class LiveTrackingControllers
{
    public static RouteGroupBuilder MapLiveTrackingEndpoints(this RouteGroupBuilder group, RouteGroupBuilder guardianGroup)
    {
        var trips = group.MapGroup("/trips");
        trips.MapPost("/{tripId:guid}/location-updates", async (string schoolAccountId, Guid tripId, LocationUpdateRequest request, TransportLocationUpdateService service, CancellationToken ct) => Results.Ok(await service.SubmitAsync(schoolAccountId, tripId, request, ct)));
        trips.MapGet("/{tripId:guid}/progress", async (string schoolAccountId, Guid tripId, TransportLocationUpdateService locations, SafeSchool.Api.Infrastructure.Persistence.SafeSchoolDbContext db, CancellationToken ct) =>
        {
            var trip = await db.TransportTrips.FindAsync([tripId], ct);
            if (trip is null || trip.TenantId != schoolAccountId) return Results.NotFound();
            return Results.Ok(new TripProgressResponse(trip.Id, trip.TenantId, trip.TransportRouteId, trip.TransportVehicleId, trip.TripStatus, trip.ServiceDirection, await locations.LatestAsync(schoolAccountId, tripId, ct), "Exact Location Available"));
        });
        trips.MapGet("/{tripId:guid}/trace", async (string schoolAccountId, Guid tripId, TripTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, tripId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        guardianGroup.MapGet("/{studentProfileId}/transport/trips/{tripId:guid}/progress", async (string studentProfileId, Guid tripId, GuardianTripProgressService service, CancellationToken ct) =>
        {
            var result = await service.ProgressAsync("school-1", "guardian:me", studentProfileId, tripId, ct);
            return result.Succeeded ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });
        return group;
    }
}
