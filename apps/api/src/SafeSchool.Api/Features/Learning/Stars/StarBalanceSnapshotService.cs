using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Domain;

namespace SafeSchool.Api.Features.Learning.Stars;

public sealed class StarBalanceSnapshotService
{
    public StarBalanceSnapshot Recalculate(string tenantId, string studentProfileId, IEnumerable<StarLedgerEntry> ledgerEntries)
    {
        var entries = ledgerEntries.Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId).ToList();
        var available = entries.Where(x => x.Direction is StarLedgerDirection.Credit or StarLedgerDirection.Release or StarLedgerDirection.Correct).Sum(x => x.Amount)
            - entries.Where(x => x.Direction is StarLedgerDirection.Debit or StarLedgerDirection.Reserve or StarLedgerDirection.Consume or StarLedgerDirection.Expire).Sum(x => x.Amount);
        var reserved = entries.Where(x => x.Direction == StarLedgerDirection.Reserve).Sum(x => x.Amount)
            - entries.Where(x => x.Direction is StarLedgerDirection.Release or StarLedgerDirection.Consume).Sum(x => x.Amount);
        var consumed = entries.Where(x => x.Direction == StarLedgerDirection.Consume).Sum(x => x.Amount);
        var pending = entries.Where(x => x.State == StarLedgerState.NeedsReview).Sum(x => x.Amount);

        return new StarBalanceSnapshot
        {
            TenantId = tenantId,
            StudentProfileId = studentProfileId,
            AvailableStars = Math.Max(0, available),
            ReservedStars = Math.Max(0, reserved),
            ConsumedStars = Math.Max(0, consumed),
            PendingReviewStars = Math.Max(0, pending),
            CalculatedAt = DateTimeOffset.UtcNow
        };
    }
}
