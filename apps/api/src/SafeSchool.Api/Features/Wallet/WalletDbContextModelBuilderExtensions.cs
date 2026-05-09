using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Anomalies;
using SafeSchool.Api.Features.Wallet.Audit;
using SafeSchool.Api.Features.Wallet.Canteen;
using SafeSchool.Api.Features.Wallet.Common.Idempotency;
using SafeSchool.Api.Features.Wallet.Corrections;
using SafeSchool.Api.Features.Wallet.History;
using SafeSchool.Api.Features.Wallet.Ledger;
using SafeSchool.Api.Features.Wallet.Limits;
using SafeSchool.Api.Features.Wallet.Payments;
using SafeSchool.Api.Features.Wallet.Pos;
using SafeSchool.Api.Features.Wallet.Reconciliation;
using SafeSchool.Api.Features.Wallet.Reviews;
using SafeSchool.Api.Features.Wallet.Rules;
using SafeSchool.Api.Features.Wallet.Sync;
using SafeSchool.Api.Features.Wallet.TopUps;
using SafeSchool.Api.Features.Wallet.Wallets;

namespace SafeSchool.Api.Features.Wallet;

public static class WalletDbContextModelBuilderExtensions
{
    public static ModelBuilder ApplyWalletModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentWallet>(entity =>
        {
            entity.ToTable("wallet_student_wallets");
            entity.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.WalletStatus });
            entity.HasIndex(x => new { x.TenantId, x.WalletCode }).IsUnique();
        });
        modelBuilder.Entity<WalletLedgerEntry>(entity =>
        {
            entity.ToTable("wallet_ledger_entries");
            entity.HasIndex(x => new { x.TenantId, x.StudentWalletId, x.PostedAt });
            entity.HasIndex(x => new { x.TenantId, x.IdempotencyKey }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.SourceType, x.SourceReference });
        });
        modelBuilder.Entity<WalletTopUp>(entity =>
        {
            entity.ToTable("wallet_top_ups");
            entity.HasIndex(x => new { x.TenantId, x.StudentWalletId, x.TopUpStatus });
            entity.HasIndex(x => new { x.TenantId, x.IdempotencyKey }).IsUnique();
        });
        modelBuilder.Entity<PaymentConfirmation>(entity =>
        {
            entity.ToTable("wallet_payment_confirmations");
            entity.HasIndex(x => new { x.TenantId, x.ProviderEventId }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.ProviderReference });
        });
        modelBuilder.Entity<CanteenMerchant>().ToTable("wallet_canteen_merchants").HasIndex(x => new { x.TenantId, x.MerchantCode }).IsUnique();
        modelBuilder.Entity<CanteenItemCategory>().ToTable("wallet_canteen_item_categories").HasIndex(x => new { x.TenantId, x.ItemCategoryCode }).IsUnique();
        modelBuilder.Entity<PurchaseEligibilityRule>().ToTable("wallet_purchase_eligibility_rules").HasIndex(x => new { x.TenantId, x.CanteenMerchantId, x.ItemCategoryCode });
        modelBuilder.Entity<POSTerminal>().ToTable("wallet_pos_terminals").HasIndex(x => new { x.TenantId, x.TerminalCode }).IsUnique();
        modelBuilder.Entity<OfflinePosSyncBatch>().ToTable("wallet_offline_pos_sync_batches").HasIndex(x => new { x.TenantId, x.ClientBatchId }).IsUnique();
        modelBuilder.Entity<CanteenPurchaseTransaction>().ToTable("wallet_canteen_purchase_transactions").HasIndex(x => new { x.TenantId, x.ClientPurchaseId }).IsUnique();
        modelBuilder.Entity<SpendingLimit>().ToTable("wallet_spending_limits").HasIndex(x => new { x.TenantId, x.StudentWalletId, x.Status });
        modelBuilder.Entity<RefundOrReversal>().ToTable("wallet_refunds_reversals").HasIndex(x => new { x.TenantId, x.ClientRequestId });
        modelBuilder.Entity<ManualWalletReview>().ToTable("wallet_manual_reviews").HasIndex(x => new { x.TenantId, x.ReviewScope, x.ScopeReference });
        modelBuilder.Entity<WalletAnomaly>().ToTable("wallet_anomalies").HasIndex(x => new { x.TenantId, x.Status, x.AnomalyType });
        modelBuilder.Entity<SettlementReference>().ToTable("wallet_settlement_references").HasIndex(x => new { x.TenantId, x.Status });
        modelBuilder.Entity<WalletReconciliationRun>().ToTable("wallet_reconciliation_runs").HasIndex(x => new { x.TenantId, x.Status, x.DateFrom, x.DateUntil });
        modelBuilder.Entity<WalletReconciliationMismatch>().ToTable("wallet_reconciliation_mismatches").HasIndex(x => new { x.TenantId, x.ReconciliationRunId });
        modelBuilder.Entity<WalletReviewSummary>().ToTable("wallet_review_summaries").HasIndex(x => new { x.TenantId, x.SummaryScope, x.ScopeReference });
        modelBuilder.Entity<WalletRuleSetting>().ToTable("wallet_rule_settings").HasIndex(x => new { x.TenantId, x.Status });
        modelBuilder.Entity<WalletAuditEvent>().ToTable("wallet_audit_events").HasIndex(x => new { x.TenantId, x.EventType, x.EventTime });
        modelBuilder.Entity<WalletIdempotencyRecord>().ToTable("wallet_idempotency_records").HasIndex(x => new { x.TenantId, x.IdempotencyKind, x.IdempotencyKey }).IsUnique();
        return modelBuilder;
    }
}
