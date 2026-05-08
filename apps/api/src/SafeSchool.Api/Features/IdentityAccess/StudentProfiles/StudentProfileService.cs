using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.AccessControl.Authorization;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfileService(
    SafeSchoolDbContext dbContext,
    DuplicateStudentProfileDetector duplicateDetector,
    PermissionGuard permissionGuard,
    StudentProfileAuditAdapter auditAdapter,
    ITenantContext tenantContext)
{
    private static readonly string[] AdminPermissions = [PermissionCatalog.StudentProfilesRead, PermissionCatalog.StudentProfilesCreate, PermissionCatalog.StudentProfilesUpdate, PermissionCatalog.StudentProfilesDeactivate, PermissionCatalog.StudentProfilesReviewHistory];

    public async Task<OperationResult<StudentProfileResponse>> CreateAsync(string tenantId, CreateStudentProfileRequest request, CancellationToken cancellationToken = default)
    {
        var guard = permissionGuard.Require(tenantId, IdentityAccessCapabilities.StudentProfiles, PermissionCatalog.StudentProfilesCreate, AdminPermissions);
        if (!guard.Succeeded)
        {
            await auditAdapter.DeniedAsync(tenantId, tenantContext.ActorReference ?? "unknown", PermissionCatalog.StudentProfilesCreate, request.SchoolStudentNumber, guard.Errors[0].Message, cancellationToken);
            return OperationResult<StudentProfileResponse>.Failure(guard.Errors.ToArray());
        }

        var duplicate = await duplicateDetector.CheckAsync(tenantId, new(request.SchoolStudentNumber, request.ExternalIdentityReferences), cancellationToken);
        if (request.ProfileStatus == ProfileStatus.Active && duplicate.Status != DuplicateReviewStatus.Clear)
        {
            return OperationResult<StudentProfileResponse>.Failure(new ValidationError("duplicate_active_identity", duplicate.ReviewReason));
        }

        var profile = new StudentProfile
        {
            TenantId = tenantId,
            SchoolStudentNumber = request.SchoolStudentNumber,
            ExternalIdentityReferences = request.ExternalIdentityReferences.Select(x => $"{x.ReferenceType}:{x.ReferenceValue}").ToArray(),
            LegalName = request.LegalName,
            PreferredName = request.PreferredName,
            DateOfBirth = request.DateOfBirth,
            GradeLevel = request.GradeLevel,
            CampusOrDivision = request.CampusOrDivision,
            EnrollmentStatus = request.EnrollmentStatus,
            ProfileStatus = request.ProfileStatus,
            CreatedBy = tenantContext.ActorReference ?? "unknown",
            UpdatedBy = tenantContext.ActorReference ?? "unknown",
            ReviewReason = request.ReviewReason
        };

        dbContext.StudentProfiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.ProfileChangedAsync(profile, profile.CreatedBy, "identity.student_profiles.create", request.ReviewReason, cancellationToken);
        return OperationResult<StudentProfileResponse>.Success(profile.ToResponse());
    }

    public async Task<PagedResponse<StudentProfileResponse>> ListAsync(string tenantId, int page = 1, int pageSize = 25, CancellationToken cancellationToken = default)
    {
        var query = dbContext.StudentProfiles.Where(x => x.TenantId == tenantId).OrderBy(x => x.LegalName);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
        return new(items, page, pageSize, total);
    }

    public async Task<OperationResult<StudentProfileResponse>> ReadAsync(string tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.StudentProfiles.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken);
        return profile is null
            ? OperationResult<StudentProfileResponse>.Failure(new ValidationError("not_found", "Student profile was not found in this school account."))
            : OperationResult<StudentProfileResponse>.Success(profile.ToResponse());
    }

    public async Task<OperationResult<StudentProfileResponse>> UpdateAsync(string tenantId, Guid id, UpdateStudentProfileRequest request, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.StudentProfiles.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken);
        if (profile is null)
        {
            return OperationResult<StudentProfileResponse>.Failure(new ValidationError("not_found", "Student profile was not found in this school account."));
        }

        var previousSummary = $"{profile.LegalName}|{profile.GradeLevel}|{profile.EnrollmentStatus}";
        profile.LegalName = request.LegalName ?? profile.LegalName;
        profile.PreferredName = request.PreferredName ?? profile.PreferredName;
        profile.GradeLevel = request.GradeLevel ?? profile.GradeLevel;
        profile.CampusOrDivision = request.CampusOrDivision ?? profile.CampusOrDivision;
        profile.EnrollmentStatus = request.EnrollmentStatus ?? profile.EnrollmentStatus;
        profile.ReviewReason = request.ReviewReason;
        profile.UpdatedBy = tenantContext.ActorReference ?? "unknown";
        profile.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.ProfileChangedAsync(profile, profile.UpdatedBy, "identity.student_profiles.update", $"{request.ReviewReason} Previous: {previousSummary}", cancellationToken);
        return OperationResult<StudentProfileResponse>.Success(profile.ToResponse());
    }

    public async Task<OperationResult<StudentProfileResponse>> DeactivateAsync(string tenantId, Guid id, DeactivateStudentProfileRequest request, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.StudentProfiles.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken);
        if (profile is null)
        {
            return OperationResult<StudentProfileResponse>.Failure(new ValidationError("not_found", "Student profile was not found in this school account."));
        }

        if (!profile.CanTransitionTo(ProfileStatus.Deactivated))
        {
            return OperationResult<StudentProfileResponse>.Failure(new ValidationError("invalid_transition", "Student profile cannot be deactivated from its current state."));
        }

        profile.ProfileStatus = ProfileStatus.Deactivated;
        profile.ReviewReason = request.ReviewReason;
        profile.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditAdapter.ProfileChangedAsync(profile, tenantContext.ActorReference ?? "unknown", "identity.student_profiles.deactivate", request.ReviewReason, cancellationToken);
        return OperationResult<StudentProfileResponse>.Success(profile.ToResponse());
    }

    public async Task<IReadOnlyList<AuditEventResponse>> HistoryAsync(string tenantId, Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.AuditEvents
            .Where(x => x.TenantId == tenantId && x.SubjectType == "Student Profile" && x.SubjectReference == id.ToString())
            .OrderByDescending(x => x.EventTime)
            .Select(x => new AuditEventResponse(x.Id, x.TenantId, x.EventCategory, x.EventType, x.ActorReference, x.SubjectType, x.SubjectReference, x.Reason, x.EventTime))
            .ToListAsync(cancellationToken);
}
