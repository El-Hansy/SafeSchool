using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public static class ReviewControllers
{
    public static RouteGroupBuilder MapReviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/access-decisions", async (string schoolAccountId, SafeSchoolDbContext db, CancellationToken ct) =>
            Results.Ok(await db.AccessDecisions.Where(x => x.TenantId == schoolAccountId)
                .OrderByDescending(x => x.DecidedAt)
                .Select(x => new AccessDecisionResponse(x.Id, x.TenantId, x.ActorReference, x.AttemptedAction, x.TargetType, x.TargetReference, x.Decision.ToString(), x.DecisionReason, x.DecidedAt))
                .ToListAsync(ct)));
        group.MapGet("/audit-events", async (string schoolAccountId, SafeSchoolDbContext db, CancellationToken ct) =>
            Results.Ok(await db.AuditEvents.Where(x => x.TenantId == schoolAccountId)
                .OrderByDescending(x => x.EventTime)
                .Select(x => new AuditEventResponse(x.Id, x.TenantId, x.EventCategory, x.EventType, x.ActorReference, x.SubjectType, x.SubjectReference, x.Reason, x.EventTime))
                .ToListAsync(ct)));
        return group;
    }
}
