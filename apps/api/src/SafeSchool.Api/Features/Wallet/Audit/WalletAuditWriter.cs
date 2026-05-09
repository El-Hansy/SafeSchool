using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Audit;

public interface IWalletAuditWriter
{
    Task RecordAsync(WalletAuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public sealed class WalletAuditWriter(SafeSchoolDbContext dbContext) : IWalletAuditWriter
{
    public async Task RecordAsync(WalletAuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        auditEvent.EventTime = auditEvent.EventTime == default ? DateTimeOffset.UtcNow : auditEvent.EventTime;
        dbContext.WalletAuditEvents.Add(auditEvent);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
