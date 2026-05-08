using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianLinkService(SafeSchoolDbContext dbContext, ITenantContext tenantContext, GuardianAuditAdapter auditAdapter)
{
    public async Task<GuardianLinkResponse> CreateAsync(string tenantId, Guid studentProfileId, GuardianLinkRequest request, CancellationToken cancellationToken = default)
    {
        var link = new GuardianLink
        {
            TenantId = tenantId,
            StudentProfileId = studentProfileId,
            GuardianId = request.GuardianId,
            RelationshipType = request.RelationshipType,
            AccessScope = request.AccessScope,
            LinkStatus = Enum.Parse<GuardianLinkStatus>(request.LinkStatus),
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            RequestedBy = tenantContext.ActorReference ?? "unknown",
            ApprovedBy = tenantContext.ActorReference,
            ReviewReason = request.ReviewReason
        };
        dbContext.GuardianLinks.Add(link);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(tenantId, link.RequestedBy, "identity.guardian_links.create", link.Id.ToString(), request.ReviewReason, cancellationToken);
        return ToResponse(link);
    }

    public async Task<GuardianLinkResponse> ChangeStateAsync(string tenantId, Guid guardianLinkId, GuardianLinkStatus status, string reason, CancellationToken cancellationToken = default)
    {
        var link = await dbContext.GuardianLinks.SingleAsync(x => x.TenantId == tenantId && x.Id == guardianLinkId, cancellationToken);
        link.LinkStatus = status;
        link.ReviewReason = reason;
        link.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(tenantId, tenantContext.ActorReference ?? "unknown", $"identity.guardian_links.{status.ToString().ToLowerInvariant()}", link.Id.ToString(), reason, cancellationToken);
        return ToResponse(link);
    }

    public async Task<GuardianLinkResponse> UpdateScopeAsync(string tenantId, Guid guardianLinkId, GuardianLinkRequest request, CancellationToken cancellationToken = default)
    {
        var link = await dbContext.GuardianLinks.SingleAsync(x => x.TenantId == tenantId && x.Id == guardianLinkId, cancellationToken);
        link.RelationshipType = request.RelationshipType;
        link.AccessScope = request.AccessScope;
        link.ValidFrom = request.ValidFrom;
        link.ValidUntil = request.ValidUntil;
        link.ReviewReason = request.ReviewReason;
        link.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(tenantId, tenantContext.ActorReference ?? "unknown", "identity.guardian_links.update_scope", link.Id.ToString(), request.ReviewReason, cancellationToken);
        return ToResponse(link);
    }

    public async Task<IReadOnlyList<AuditEventResponse>> HistoryAsync(string tenantId, Guid guardianLinkId, CancellationToken cancellationToken = default) =>
        await dbContext.AuditEvents
            .Where(x => x.TenantId == tenantId && x.SubjectReference == guardianLinkId.ToString())
            .OrderByDescending(x => x.EventTime)
            .Select(x => new AuditEventResponse(x.Id, x.TenantId, x.EventCategory, x.EventType, x.ActorReference, x.SubjectType, x.SubjectReference, x.Reason, x.EventTime))
            .ToListAsync(cancellationToken);

    private static GuardianLinkResponse ToResponse(GuardianLink link) => new(link.Id, link.TenantId, link.StudentProfileId, link.GuardianId, link.RelationshipType, link.AccessScope, link.LinkStatus.ToString(), link.ValidFrom, link.ValidUntil, link.ReviewReason);
}
