using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Features.AttendanceAccess.Sync;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public static class ScanEventsController
{
    public static RouteGroupBuilder MapScanEndpoints(this RouteGroupBuilder group)
    {
        group.MapScanPointEndpoints();
        var scans = group.MapGroup("/scans");
        scans.MapPost("/", async (string schoolAccountId, RecordScanRequest request, OfflineScanSyncService service, GateScanAuditAdapter audit, SafeSchoolDbContext dbContext, CancellationToken ct) =>
        {
            var response = await service.RecordAsync(schoolAccountId, request, cancellationToken: ct);
            var scan = await dbContext.GateScanEvents.SingleAsync(x => x.Id == response.ScanEventId, ct);
            await audit.ScanRecordedAsync(scan, ct);
            return Results.Ok(response);
        });
        scans.MapPost("/offline-sync", async (string schoolAccountId, OfflineScanBatchRequest request, OfflineScanSyncService service, CancellationToken ct) =>
            Results.Ok(await service.SyncAsync(schoolAccountId, request, ct)));
        scans.MapGet("/", async (string schoolAccountId, SafeSchoolDbContext dbContext, CancellationToken ct) =>
            Results.Ok(await dbContext.GateScanEvents.Where(x => x.TenantId == schoolAccountId).OrderByDescending(x => x.ReceivedAt).Select(x => x.ToResponse()).ToListAsync(ct)));
        scans.MapGet("/{scanEventId:guid}", async (string schoolAccountId, Guid scanEventId, SafeSchoolDbContext dbContext, CancellationToken ct) =>
        {
            var scan = await dbContext.GateScanEvents.SingleOrDefaultAsync(x => x.TenantId == schoolAccountId && x.Id == scanEventId, ct);
            return scan is null ? Results.NotFound() : Results.Ok(scan.ToResponse());
        });
        scans.MapGet("/{scanEventId:guid}/trace", async (string schoolAccountId, Guid scanEventId, ScanTraceService service, CancellationToken ct) =>
        {
            var trace = await service.TraceAsync(schoolAccountId, scanEventId, ct);
            return trace is null ? Results.NotFound() : Results.Ok(trace);
        });
        return group;
    }
}

