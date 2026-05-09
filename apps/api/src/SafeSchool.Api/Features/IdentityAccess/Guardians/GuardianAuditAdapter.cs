using SafeSchool.Api.Features.IdentityAccess.Audit;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianAuditAdapter(IAuditWriter auditWriter)
{
    public Task<AuditEvent> RecordAsync(string tenantId, string actor, string eventType, string subjectReference, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AuditEvent
        {
            TenantId = tenantId,
            EventCategory = "Guardian Link",
            EventType = eventType,
            ActorReference = actor,
            SubjectType = "Guardian",
            SubjectReference = subjectReference,
            Reason = reason
        }, cancellationToken);
}
