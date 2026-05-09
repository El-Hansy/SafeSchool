using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Sync;

public sealed class OfflineScanSyncService(
    SafeSchoolDbContext dbContext,
    ScanValidationService validationService,
    CampusAccessDecisionService decisionService,
    IdempotencyService idempotencyService)
{
    public async Task<OfflineSyncResponse> SyncAsync(string tenantId, OfflineScanBatchRequest request, CancellationToken cancellationToken = default)
    {
        var batchDecision = idempotencyService.Register(tenantId, request.ClientBatchId, request.Scans.Count.ToString(), request.ClientBatchId);
        if (batchDecision.IsConflict)
        {
            return new(request.ClientBatchId, OfflineSyncStatus.Conflict, []);
        }

        if (batchDecision.IsRetry)
        {
            var existing = await dbContext.GateScanEvents
                .Where(x => x.TenantId == tenantId && x.ClientBatchId == request.ClientBatchId)
                .Select(x => x.ToResponse())
                .ToListAsync(cancellationToken);
            return new(request.ClientBatchId, OfflineSyncStatus.Duplicate, existing);
        }

        var responses = new List<ScanEventResponse>();
        foreach (var scan in request.Scans)
        {
            var response = await RecordAsync(tenantId, scan with { ClientScanId = scan.ClientScanId }, request.ClientBatchId, cancellationToken);
            responses.Add(response);
        }

        dbContext.OfflineSyncBatches.Add(new OfflineSyncBatch { TenantId = tenantId, ClientBatchId = request.ClientBatchId, ScanCount = responses.Count, Status = OfflineSyncStatus.Accepted });
        await dbContext.SaveChangesAsync(cancellationToken);
        return new(request.ClientBatchId, OfflineSyncStatus.Accepted, responses);
    }

    public async Task<ScanEventResponse> RecordAsync(string tenantId, RecordScanRequest request, string? clientBatchId = null, CancellationToken cancellationToken = default)
    {
        var duplicate = await dbContext.GateScanEvents.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientScanId == request.ClientScanId, cancellationToken);
        if (duplicate is not null)
        {
            return duplicate.ToResponse();
        }

        var validation = await validationService.ValidateAsync(tenantId, request, cancellationToken);
        var scanEvent = new GateScanEvent
        {
            TenantId = tenantId,
            GateId = request.GateId,
            ScanPointId = request.ScanPointId,
            StudentProfileId = validation.StudentProfileId,
            CredentialReference = request.CredentialReference,
            Direction = request.Direction,
            Method = request.Method,
            Status = validation.Status,
            ClientScanId = request.ClientScanId,
            ClientBatchId = clientBatchId ?? string.Empty,
            LocalScanTime = request.LocalScanTime,
            ReceivedAt = DateTimeOffset.UtcNow,
            DecisionReason = validation.Reason
        };
        dbContext.GateScanEvents.Add(scanEvent);
        await dbContext.SaveChangesAsync(cancellationToken);
        await decisionService.DecideAsync(scanEvent, validation.CredentialStatus, cancellationToken);
        return scanEvent.ToResponse();
    }
}

