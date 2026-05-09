using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Pos;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Sync;

public sealed class OfflinePosSyncService(SafeSchoolDbContext dbContext, OnlinePurchaseAuthorizationService purchases)
{
    public async Task<OperationResult<OfflineSyncResponse>> SyncAsync(string tenantId, OfflineSyncRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.OfflinePosSyncBatches.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientBatchId == request.ClientBatchId, cancellationToken);
        if (existing is not null) return OperationResult<OfflineSyncResponse>.Success(new OfflineSyncResponse(existing.Id, existing.ClientBatchId, existing.SyncStatus, existing.AcceptedCount, existing.HeldCount, existing.DuplicateCount));
        var batch = new OfflinePosSyncBatch { TenantId = tenantId, PosTerminalId = request.PosTerminalId, ClientBatchId = request.ClientBatchId, SyncStatus = OfflinePosSyncStatus.Accepted };
        dbContext.OfflinePosSyncBatches.Add(batch);
        await dbContext.SaveChangesAsync(cancellationToken);
        foreach (var item in request.Purchases)
        {
            var result = await purchases.RecordAsync(tenantId, new OnlinePurchaseRequest(item.WalletId, Guid.Empty, request.PosTerminalId, item.CredentialReference, item.ClientPurchaseId, item.AmountMinor, ItemCategoryCode: item.ItemCategoryCode, ItemSummary: item.ItemSummary), cancellationToken);
            if (result.Succeeded && result.Value!.Decision == PosPurchaseDecision.Approved) batch.AcceptedCount++; else batch.HeldCount++;
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<OfflineSyncResponse>.Success(new OfflineSyncResponse(batch.Id, batch.ClientBatchId, batch.SyncStatus, batch.AcceptedCount, batch.HeldCount, batch.DuplicateCount));
    }
}
