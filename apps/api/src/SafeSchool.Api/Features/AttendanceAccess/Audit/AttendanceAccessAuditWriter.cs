using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.AttendanceAccess.Audit;

public interface IAttendanceAccessAuditWriter
{
    Task<AttendanceAccessAuditEvent> RecordAsync(AttendanceAccessAuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public sealed class AttendanceAccessAuditWriter(SafeSchoolDbContext dbContext, ITenantContext tenantContext) : IAttendanceAccessAuditWriter
{
    public async Task<AttendanceAccessAuditEvent> RecordAsync(AttendanceAccessAuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        auditEvent.ActorReference = string.IsNullOrWhiteSpace(auditEvent.ActorReference)
            ? tenantContext.ActorReference ?? "unknown"
            : auditEvent.ActorReference;
        dbContext.AttendanceAccessAuditEvents.Add(auditEvent);
        await dbContext.SaveChangesAsync(cancellationToken);
        return auditEvent;
    }
}

