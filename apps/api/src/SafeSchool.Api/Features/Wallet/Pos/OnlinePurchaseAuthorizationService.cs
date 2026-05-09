using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.Wallets;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Pos;

public sealed class OnlinePurchaseAuthorizationService(SafeSchoolDbContext dbContext, WalletPermissionGuard guard, WalletPosCredentialValidationService credentialValidation, IWalletSpendingRuleEvaluationPort ruleEvaluation, WalletLedgerPostingService ledger)
{
    public async Task<OperationResult<PurchaseResponse>> RecordAsync(string tenantId, OnlinePurchaseRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, WalletCapabilities.CanteenPos, WalletPermissionCatalog.PurchasesRecord, actorPermissions: WalletPermissionCatalog.WalletAdministrator.Concat(WalletPermissionCatalog.PosOperator), targetType: "CanteenPurchase", targetReference: request.ClientPurchaseId, posDeviceReference: request.DeviceReference, requestedDeviceReference: request.DeviceReference, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<PurchaseResponse>.Failure(allowed.Errors.ToArray());
        var existing = await dbContext.CanteenPurchaseTransactions.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientPurchaseId == request.ClientPurchaseId, cancellationToken);
        if (existing is not null) return OperationResult<PurchaseResponse>.Success(existing.ToResponse());
        var wallet = await dbContext.StudentWallets.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.WalletId, cancellationToken);
        var terminal = await dbContext.PosTerminals.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.PosTerminalId, cancellationToken);
        var credential = await credentialValidation.ValidateAsync(tenantId, request.CredentialReference, cancellationToken);
        var decision = PosPurchaseDecision.Approved;
        var reason = "approved";
        if (wallet is null || wallet.WalletStatus != WalletStatus.Active) { decision = PosPurchaseDecision.Denied; reason = "wallet_not_active"; }
        else if (terminal is null || !terminal.IsReadyForOnlinePurchase()) { decision = PosPurchaseDecision.Denied; reason = "terminal_not_active"; }
        else if (!credential.Succeeded) { decision = PosPurchaseDecision.Denied; reason = "credential_not_active"; }
        else if (wallet.AvailableBalanceMinor < request.AmountMinor) { decision = PosPurchaseDecision.Denied; reason = "insufficient_funds"; }
        var rule = await ruleEvaluation.EvaluateAsync(tenantId, request.WalletId, request.ItemCategoryCode, request.MerchantId, request.AmountMinor, cancellationToken);
        if (!rule.Allowed) { decision = PosPurchaseDecision.Denied; reason = rule.Reason; }
        var purchase = new CanteenPurchaseTransaction { TenantId = tenantId, StudentWalletId = request.WalletId, StudentProfileId = wallet?.StudentProfileId ?? string.Empty, CanteenMerchantId = request.MerchantId, PosTerminalId = request.PosTerminalId, ClientPurchaseId = request.ClientPurchaseId, CredentialReference = request.CredentialReference, ItemCategoryCode = request.ItemCategoryCode, ItemSummary = request.ItemSummary, AmountMinor = request.AmountMinor, CurrencyCode = request.CurrencyCode, Decision = decision, DecisionReason = reason, RuleSnapshotReference = rule.RuleSnapshotReference, PurchaseMode = PosPurchaseMode.Online };
        dbContext.CanteenPurchaseTransactions.Add(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        if (decision == PosPurchaseDecision.Approved)
        {
            var posted = await ledger.PostAsync(tenantId, new PostLedgerRequest(request.WalletId, WalletLedgerEntryType.Debit, request.AmountMinor, request.CurrencyCode, "CanteenPurchase", purchase.Id.ToString(), $"purchase-{request.ClientPurchaseId}", request.OperatorReference, "approved canteen purchase"), cancellationToken);
            if (posted.Succeeded) { purchase.LedgerDebitEntryId = posted.Value!.LedgerEntryId; await dbContext.SaveChangesAsync(cancellationToken); }
        }
        return OperationResult<PurchaseResponse>.Success(purchase.ToResponse());
    }
}
