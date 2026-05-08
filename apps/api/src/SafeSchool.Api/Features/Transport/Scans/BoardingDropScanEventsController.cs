using SafeSchool.Api.Features.Transport.Sync;
using SafeSchool.Api.Features.Transport.Trips;

namespace SafeSchool.Api.Features.Transport.Scans;

public static class BoardingDropScanEventsController
{
    public static RouteGroupBuilder MapScanEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/scan-context-trips", async (string schoolAccountId, ScanReadyTripRequest request, TransportTripLifecycleService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(schoolAccountId, request.TransportRouteId, request.TransportVehicleId, request.TrackingDeviceReference, request.ServiceDirection, request.TripDate, request.PlannedStartTime, ct);
            return result.Succeeded ? Results.Created($"/transport/scan-context-trips/{result.Value!.Id}", new ScanReadyTripResponse(result.Value.Id, result.Value.TenantId, result.Value.TransportRouteId, result.Value.TransportVehicleId, result.Value.TripStatus, result.Value.TrackingDeviceReference)) : Results.BadRequest(result.Errors);
        });
        group.MapPost("/scan-context-trips/{tripId:guid}/start", async (string schoolAccountId, Guid tripId, ScanReadyTripRequest request, TransportTripLifecycleService service, CancellationToken ct) =>
        {
            var result = await service.StartAsync(schoolAccountId, tripId, request.TrackingDeviceReference, DateTimeOffset.UtcNow, ct);
            return result.Succeeded ? Results.Ok(new ScanReadyTripResponse(result.Value!.Id, result.Value.TenantId, result.Value.TransportRouteId, result.Value.TransportVehicleId, result.Value.TripStatus, result.Value.TrackingDeviceReference)) : Results.BadRequest(result.Errors);
        });
        group.MapPost("/scan-context-trips/{tripId:guid}/end", async (string schoolAccountId, Guid tripId, TransportTripLifecycleService service, CancellationToken ct) =>
        {
            var result = await service.EndAsync(schoolAccountId, tripId, DateTimeOffset.UtcNow, ct);
            return result.Succeeded ? Results.Ok(new ScanReadyTripResponse(result.Value!.Id, result.Value.TenantId, result.Value.TransportRouteId, result.Value.TransportVehicleId, result.Value.TripStatus, result.Value.TrackingDeviceReference)) : Results.BadRequest(result.Errors);
        });
        var scans = group.MapGroup("/scan-events");
        scans.MapPost("/", async (string schoolAccountId, BoardingDropScanRequest request, OfflineTransportScanSyncService service, CancellationToken ct) => Results.Ok(await service.RecordAsync(schoolAccountId, request, false, ct)));
        scans.MapPost("/sync", async (string schoolAccountId, OfflineTransportScanSyncRequest request, OfflineTransportScanSyncService service, CancellationToken ct) => Results.Ok(await service.SyncAsync(schoolAccountId, request, ct)));
        scans.MapGet("/{scanEventId:guid}/trace", async (string schoolAccountId, Guid scanEventId, BoardingDropScanTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, scanEventId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        scans.MapPost("/{scanEventId:guid}/review-outcome", (Guid scanEventId, ScanReviewOutcomeRequest request) => Results.Ok(new { scanEventId, request.ReviewAction, status = "recorded" }));
        return group;
    }
}
