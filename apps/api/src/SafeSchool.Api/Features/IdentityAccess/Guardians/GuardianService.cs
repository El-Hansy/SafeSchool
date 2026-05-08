using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianService(SafeSchoolDbContext dbContext, ITenantContext tenantContext, GuardianAuditAdapter auditAdapter)
{
    public async Task<GuardianResponse> CreateAsync(string tenantId, GuardianRequest request, CancellationToken cancellationToken = default)
    {
        var guardian = new GuardianRecord
        {
            TenantId = tenantId,
            DisplayName = request.DisplayName,
            ContactMethods = request.ContactMethods.ToArray(),
            CreatedBy = tenantContext.ActorReference ?? "unknown",
            UpdatedBy = tenantContext.ActorReference ?? "unknown"
        };
        dbContext.GuardianRecords.Add(guardian);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(tenantId, guardian.CreatedBy, "identity.guardians.create", guardian.Id.ToString(), "Guardian record created.", cancellationToken);
        return new(guardian.Id, guardian.TenantId, guardian.DisplayName, guardian.GuardianStatus.ToString(), guardian.IdentityReviewStatus.ToString());
    }

    public async Task<IReadOnlyList<GuardianResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.GuardianRecords.Where(x => x.TenantId == tenantId)
            .Select(x => new GuardianResponse(x.Id, x.TenantId, x.DisplayName, x.GuardianStatus.ToString(), x.IdentityReviewStatus.ToString()))
            .ToListAsync(cancellationToken);

    public async Task<OperationResult<GuardianResponse>> ReadAsync(string tenantId, Guid guardianId, CancellationToken cancellationToken = default)
    {
        var guardian = await dbContext.GuardianRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == guardianId, cancellationToken);
        return guardian is null
            ? OperationResult<GuardianResponse>.Failure(new ValidationError("not_found", "Guardian record was not found in this school account."))
            : OperationResult<GuardianResponse>.Success(new(guardian.Id, guardian.TenantId, guardian.DisplayName, guardian.GuardianStatus.ToString(), guardian.IdentityReviewStatus.ToString()));
    }

    public async Task<OperationResult<GuardianResponse>> UpdateAsync(string tenantId, Guid guardianId, GuardianUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var guardian = await dbContext.GuardianRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == guardianId, cancellationToken);
        if (guardian is null)
        {
            return OperationResult<GuardianResponse>.Failure(new ValidationError("not_found", "Guardian record was not found in this school account."));
        }

        guardian.DisplayName = request.DisplayName ?? guardian.DisplayName;
        guardian.ContactMethods = request.ContactMethods?.ToArray() ?? guardian.ContactMethods;
        guardian.UpdatedBy = tenantContext.ActorReference ?? "unknown";
        guardian.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.RecordAsync(tenantId, guardian.UpdatedBy, "identity.guardians.update", guardian.Id.ToString(), request.ReviewReason, cancellationToken);
        return OperationResult<GuardianResponse>.Success(new(guardian.Id, guardian.TenantId, guardian.DisplayName, guardian.GuardianStatus.ToString(), guardian.IdentityReviewStatus.ToString()));
    }
}
