using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.TopUps;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Payments;

public sealed class PaymentConfirmationService(SafeSchoolDbContext dbContext, WalletPermissionGuard guard, IPaymentProviderAdapter adapter, WalletLedgerPostingService ledger, IWalletAuditWriter auditWriter)
{
    public async Task<OperationResult<PaymentConfirmationResponse>> ReceiveAsync(string tenantId, PaymentConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.PaymentProcessing, WalletPermissionCatalog.PaymentsConfirm, targetType: "PaymentConfirmation", targetReference: request.ProviderEventId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<PaymentConfirmationResponse>.Failure(allowed.Errors.ToArray());
        var normalized = adapter.Normalize(request);
        var existing = await dbContext.PaymentConfirmations.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ProviderEventId == normalized.ProviderEventId, cancellationToken);
        if (existing is not null) return OperationResult<PaymentConfirmationResponse>.Success(existing.ToResponse());
        var confirmation = new PaymentConfirmation { TenantId = tenantId, WalletTopUpId = request.WalletTopUpId, ProviderReference = normalized.ProviderReference, ProviderEventId = normalized.ProviderEventId, ConfirmationStatus = normalized.Status, AmountMinor = normalized.AmountMinor, CurrencyCode = normalized.CurrencyCode, SafePaymentMethodSummary = normalized.SafePaymentMethodSummary, RawPayloadReference = normalized.RawPayloadReference, IdempotencyKey = normalized.ProviderEventId };
        dbContext.PaymentConfirmations.Add(confirmation);
        if (request.WalletTopUpId is Guid topUpId)
        {
            var topUp = await dbContext.WalletTopUps.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == topUpId, cancellationToken);
            if (topUp is not null && normalized.Status == PaymentConfirmationStatus.Successful && topUp.TopUpStatus != WalletTopUpStatus.Credited)
            {
                topUp.TopUpStatus = WalletTopUpStatus.Confirmed;
                topUp.ConfirmedAt = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync(cancellationToken);
                var posted = await ledger.PostAsync(tenantId, new PostLedgerRequest(topUp.StudentWalletId, WalletLedgerEntryType.Credit, topUp.NetCreditMinor, topUp.CurrencyCode, "PaymentConfirmation", confirmation.Id.ToString(), $"payment-{normalized.ProviderEventId}", "payment-provider", "confirmed guardian payment"), cancellationToken);
                if (posted.Succeeded) { topUp.TopUpStatus = WalletTopUpStatus.Credited; topUp.CreditedAt = DateTimeOffset.UtcNow; confirmation.ConfirmationStatus = PaymentConfirmationStatus.Applied; }
            }
            else if (topUp is not null && normalized.Status is PaymentConfirmationStatus.Disputed or PaymentConfirmationStatus.ChargedBack)
            {
                topUp.TopUpStatus = WalletTopUpStatus.NeedsReview;
                topUp.ReviewReason = normalized.Status.ToString();
            }
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new WalletAuditEvent { TenantId = tenantId, EventCategory = "Payment", EventType = "wallet.payments.confirmation_received", SubjectType = "PaymentConfirmation", SubjectReference = confirmation.Id.ToString(), ActorReference = "payment-provider", Reason = normalized.Status.ToString() }, cancellationToken);
        return OperationResult<PaymentConfirmationResponse>.Success(confirmation.ToResponse());
    }
}

public static class PaymentConfirmationMappingExtensions
{
    public static PaymentConfirmationResponse ToResponse(this PaymentConfirmation confirmation) => new(confirmation.Id, confirmation.WalletTopUpId, confirmation.ProviderReference, confirmation.ProviderEventId, confirmation.ConfirmationStatus, confirmation.AmountMinor, confirmation.CurrencyCode, confirmation.SafePaymentMethodSummary, confirmation.ReviewReason);
}
