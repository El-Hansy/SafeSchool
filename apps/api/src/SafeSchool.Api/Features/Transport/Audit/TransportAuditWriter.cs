using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Transport.Audit;

public interface ITransportAuditWriter
{
    Task RecordAsync(TransportAuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public sealed class TransportAuditWriter(SafeSchoolDbContext dbContext, ITenantContext tenantContext) : ITransportAuditWriter
{
    public async Task RecordAsync(TransportAuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        auditEvent.ActorReference = string.IsNullOrWhiteSpace(auditEvent.ActorReference) ? tenantContext.ActorReference ?? "system" : auditEvent.ActorReference;
        dbContext.TransportAuditEvents.Add(auditEvent);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
