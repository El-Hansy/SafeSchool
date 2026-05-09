using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed class StudentWalletService(
    SafeSchoolDbContext dbContext,
    WalletPermissionGuard guard,
    IWalletStudentProfileEvidenceProvider studentProfiles,
    IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<WalletResponse>> CreateOrActivateAsync(string tenantId, WalletCreateRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.Ledger, WalletPermissionCatalog.WalletsManage, targetType: "StudentWallet", targetReference: request.StudentProfileId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<WalletResponse>.Failure(allowed.Errors.ToArray());
        var student = await studentProfiles.GetAsync(tenantId, request.StudentProfileId, cancellationToken);
        if (student.Status != StudentEligibilityStatus.Active) return OperationResult<WalletResponse>.Failure(new ValidationError("student_not_active", "Wallet activation requires an active in-tenant student profile."));

        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentProfileId == request.StudentProfileId && x.WalletStatus != WalletStatus.Closed, cancellationToken);
        if (wallet is null)
        {
            wallet = new StudentWallet { TenantId = tenantId, StudentProfileId = request.StudentProfileId, WalletCode = request.WalletCode, CurrencyCode = request.CurrencyCode, CreatedBy = request.CreatedBy, UpdatedBy = request.CreatedBy };
            dbContext.StudentWallets.Add(wallet);
        }
        wallet.Activate(request.CreatedBy);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "Wallet", EventType = "wallet.wallets.activate", SubjectType = "StudentWallet", SubjectReference = wallet.Id.ToString(), ActorReference = request.CreatedBy, Reason = "wallet activated" }, cancellationToken);
        return OperationResult<WalletResponse>.Success(wallet.ToResponse());
    }

    public async Task<IReadOnlyList<WalletResponse>> BulkActivateAsync(string tenantId, BulkActivateWalletsRequest request, CancellationToken cancellationToken = default)
    {
        var responses = new List<WalletResponse>();
        foreach (var studentId in request.StudentProfileIds.Distinct())
        {
            var result = await CreateOrActivateAsync(tenantId, new WalletCreateRequest(studentId, $"W-{studentId}", request.CurrencyCode, request.CreatedBy), cancellationToken);
            if (result.Succeeded && result.Value is not null) responses.Add(result.Value);
        }
        return responses;
    }

    public async Task<IReadOnlyList<WalletResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.StudentWallets.Where(x => x.TenantId == tenantId).OrderBy(x => x.WalletCode).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<WalletResponse?> GetAsync(string tenantId, Guid walletId, CancellationToken cancellationToken = default) =>
        await dbContext.StudentWallets.Where(x => x.TenantId == tenantId && x.Id == walletId).Select(x => x.ToResponse()).SingleOrDefaultAsync(cancellationToken);

    public async Task<OperationResult<WalletResponse>> ChangeStatusAsync(string tenantId, Guid walletId, WalletStatus status, string reason, string actor, CancellationToken cancellationToken = default)
    {
        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == walletId, cancellationToken);
        if (wallet is null) return OperationResult<WalletResponse>.Failure(new ValidationError("wallet_not_found", "Wallet was not found."));
        if (status == WalletStatus.Closed && !wallet.CanClose()) return OperationResult<WalletResponse>.Failure(new ValidationError("wallet_has_active_hold", "Wallet cannot close while holds, recovery, dispute, or reconciliation review is active."));
        if (status == WalletStatus.Restricted) wallet.Restrict(reason, actor);
        else if (status == WalletStatus.Active) wallet.Restore(actor);
        else { wallet.WalletStatus = status; wallet.RestrictionReason = reason; wallet.UpdatedBy = actor; wallet.UpdatedAt = DateTimeOffset.UtcNow; }
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "Wallet", EventType = $"wallet.wallets.{status.ToString().ToLowerInvariant()}", SubjectType = "StudentWallet", SubjectReference = wallet.Id.ToString(), ActorReference = actor, Reason = reason }, cancellationToken);
        return OperationResult<WalletResponse>.Success(wallet.ToResponse());
    }
}
