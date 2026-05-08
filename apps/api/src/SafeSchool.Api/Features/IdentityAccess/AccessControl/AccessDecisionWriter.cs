using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public interface IAccessDecisionWriter
{
    Task<AccessDecision> RecordAsync(AccessDecision decision, CancellationToken cancellationToken = default);
}

public sealed class AccessDecisionWriter(SafeSchoolDbContext dbContext, ITenantContext tenantContext) : IAccessDecisionWriter
{
    public async Task<AccessDecision> RecordAsync(AccessDecision decision, CancellationToken cancellationToken = default)
    {
        decision.ActorReference = string.IsNullOrWhiteSpace(decision.ActorReference)
            ? tenantContext.ActorReference ?? "unknown"
            : decision.ActorReference;
        dbContext.AccessDecisions.Add(decision);
        await dbContext.SaveChangesAsync(cancellationToken);
        return decision;
    }
}
