using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class WalletReconciliationService(SafeSchoolDbContext dbContext)
{
    public async Task<ReconciliationRunResponse> CreateAsync(string tenantId, ReconciliationRunRequest request, CancellationToken cancellationToken = default)
    {
        var ledgerTotal = await dbContext.WalletLedgerEntries.Where(x => x.TenantId == tenantId).SumAsync(x => x.EntryType == SafeSchool.Api.Features.Wallet.Common.WalletLedgerEntryType.Debit ? -x.AmountMinor : x.AmountMinor, cancellationToken);
        var sourceTotal = await dbContext.WalletTopUps.Where(x => x.TenantId == tenantId && x.TopUpStatus == SafeSchool.Api.Features.Wallet.Common.WalletTopUpStatus.Credited).SumAsync(x => x.NetCreditMinor, cancellationToken);
        var run = new WalletReconciliationRun { TenantId = tenantId, RunScope = request.Scope, DateFrom = request.DateFrom, DateUntil = request.DateUntil, LedgerTotalMinor = ledgerTotal, SourceTotalMinor = sourceTotal, DifferenceMinor = ledgerTotal - sourceTotal, Status = ledgerTotal == sourceTotal ? SafeSchool.Api.Features.Wallet.Common.ReconciliationStatus.Matched : SafeSchool.Api.Features.Wallet.Common.ReconciliationStatus.Mismatched, CreatedBy = request.ActorReference };
        dbContext.WalletReconciliationRuns.Add(run);
        if (run.DifferenceMinor != 0) dbContext.WalletReconciliationMismatches.Add(new WalletReconciliationMismatch { TenantId = tenantId, ReconciliationRunId = run.Id, MismatchType = "ledger_source_difference", SourceReference = run.Id.ToString(), DifferenceMinor = run.DifferenceMinor });
        await dbContext.SaveChangesAsync(cancellationToken);
        return run.ToResponse();
    }

    public async Task<IReadOnlyList<ReconciliationRunResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) => await dbContext.WalletReconciliationRuns.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.CreatedAt).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    public async Task<ReconciliationRunResponse?> GetAsync(string tenantId, Guid runId, CancellationToken cancellationToken = default) => await dbContext.WalletReconciliationRuns.Where(x => x.TenantId == tenantId && x.Id == runId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);
    public async Task<IReadOnlyList<ReconciliationMismatchResponse>> MismatchesAsync(string tenantId, Guid runId, CancellationToken cancellationToken = default) => await dbContext.WalletReconciliationMismatches.Where(x => x.TenantId == tenantId && x.ReconciliationRunId == runId).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
}
