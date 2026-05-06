using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.Audit;

public interface IAuditWriter
{
    Task<AuditEvent> RecordAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public sealed class AuditWriter(SafeSchoolDbContext dbContext, ITenantContext tenantContext) : IAuditWriter
{
    public async Task<AuditEvent> RecordAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        auditEvent.ActorReference = string.IsNullOrWhiteSpace(auditEvent.ActorReference)
            ? tenantContext.ActorReference ?? "unknown"
            : auditEvent.ActorReference;
        dbContext.AuditEvents.Add(auditEvent);
        await dbContext.SaveChangesAsync(cancellationToken);
        return auditEvent;
    }
}
