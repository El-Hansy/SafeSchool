using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Common.Identity;
using SafeSchool.Api.Features.Wallet.Common.Idempotency;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.TopUps;

public sealed class GuardianTopUpService(SafeSchoolDbContext dbContext, WalletPermissionGuard guard, IWalletGuardianLinkProvider guardianLinks, WalletIdempotencyService idempotency, IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<TopUpResponse>> InitiateAsync(string tenantId, string studentProfileId, GuardianTopUpRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.TopUp, WalletPermissionCatalog.TopUpsInitiate, actorPermissions: WalletPermissionCatalog.WalletAdministrator.Concat(WalletPermissionCatalog.Guardian), targetType: "WalletTopUp", targetReference: request.WalletId.ToString(), guardianScopedStudentId: studentProfileId, requestedStudentId: studentProfileId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<TopUpResponse>.Failure(allowed.Errors.ToArray());
        var link = await guardianLinks.GetAsync(tenantId, request.GuardianActorId, studentProfileId, cancellationToken);
        if (link.Status != WalletGuardianLinkStatus.Approved) return OperationResult<TopUpResponse>.Failure(new ValidationError("guardian_scope_mismatch", "Guardian top-up requires an approved linked student."));
        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.WalletId && x.StudentProfileId == studentProfileId, cancellationToken);
        if (wallet is null || wallet.WalletStatus != WalletStatus.Active) return OperationResult<TopUpResponse>.Failure(new ValidationError("wallet_not_active", "Top-up requires an active wallet."));
        var reserved = await idempotency.ReserveAsync(tenantId, "guardian_top_up", request.ClientRequestId, request.WalletId.ToString(), cancellationToken: cancellationToken);
        if (!reserved.Succeeded) return OperationResult<TopUpResponse>.Failure(reserved.Errors.ToArray());
        var existing = await dbContext.WalletTopUps.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.IdempotencyKey == request.ClientRequestId, cancellationToken);
        if (existing is not null) return OperationResult<TopUpResponse>.Success(existing.ToResponse());
        var topUp = new WalletTopUp { TenantId = tenantId, StudentWalletId = wallet.Id, StudentProfileId = wallet.StudentProfileId, InitiatedByActorId = request.GuardianActorId, GuardianLinkId = link.GuardianLinkId, TopUpSource = WalletTopUpSource.GuardianOnlineProvider, AmountMinor = request.AmountMinor, NetCreditMinor = request.AmountMinor, CurrencyCode = request.CurrencyCode, TopUpStatus = WalletTopUpStatus.AwaitingConfirmation, PaymentProviderReference = $"{request.PaymentProvider}:{request.ClientRequestId}", IdempotencyKey = request.ClientRequestId };
        dbContext.WalletTopUps.Add(topUp);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "TopUp", EventType = "wallet.topups.guardian_initiated", SubjectType = "WalletTopUp", SubjectReference = topUp.Id.ToString(), ActorReference = request.GuardianActorId, Reason = "awaiting payment confirmation" }, cancellationToken);
        return OperationResult<TopUpResponse>.Success(topUp.ToResponse());
    }
}
