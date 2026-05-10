namespace SafeSchool.Api.Features.Mobile;

public static class MobileSupportAuditEventEndpoints
{
    public static RouteGroupBuilder MapMobileSupportAuditEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/support/audit-events", (string tenantId, MobileAuditService audit) => Results.Ok(new { items = audit.Events(tenantId), nextCursor = (string?)null }));
        return group;
    }
}
