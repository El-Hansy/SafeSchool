using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Audit;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfileAuditAdapter(IAuditWriter auditWriter, IAccessDecisionWriter accessDecisionWriter)
{
    public Task<AuditEvent> ProfileChangedAsync(StudentProfile profile, string actor, string eventType, string reason, CancellationToken cancellationToken = default) =>
        auditWriter.RecordAsync(new AuditEvent
        {
            TenantId = profile.TenantId,
            EventCategory = "Student Profile",
            EventType = eventType,
            ActorReference = actor,
            SubjectType = "Student Profile",
            SubjectReference = profile.Id.ToString(),
            Reason = reason,
            NewValueSummary = profile.ProfileStatus.ToString()
        }, cancellationToken);

    public Task<AccessDecision> DeniedAsync(string tenantId, string actor, string action, string target, string reason, CancellationToken cancellationToken = default) =>
        accessDecisionWriter.RecordAsync(new AccessDecision
        {
            TenantId = tenantId,
            ActorReference = actor,
            AttemptedAction = action,
            TargetType = "Student Profile",
            TargetReference = target,
            Decision = Common.AccessDecisionResult.Denied,
            DecisionReason = reason,
            FeatureCapabilityKey = Infrastructure.FeatureFlags.IdentityAccessCapabilities.StudentProfiles
        }, cancellationToken);
}
