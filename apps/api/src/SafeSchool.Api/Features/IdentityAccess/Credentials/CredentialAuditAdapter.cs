using SafeSchool.Api.Features.IdentityAccess.Audit;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed class CredentialAuditAdapter(IAuditWriter auditWriter)
{
    public Task<AuditEvent> RecordAsync(IdentityCredential credential, string actor, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AuditEvent
        {
            TenantId = credential.TenantId,
            EventCategory = "Credential",
            EventType = eventType,
            ActorReference = actor,
            SubjectType = "Identity Credential",
            SubjectReference = credential.Id.ToString(),
            Reason = reason,
            NewValueSummary = credential.CredentialStatus.ToString()
        }, cancellationToken);
}
