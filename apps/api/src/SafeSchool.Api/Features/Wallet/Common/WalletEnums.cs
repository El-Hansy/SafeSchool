namespace SafeSchool.Api.Features.Wallet.Common;

public enum WalletStatus { Draft, Active, Restricted, Suspended, Closed }
public enum WalletLedgerEntryType { Credit, Debit, Hold, Release, Refund, Reversal, Chargeback, Recovery, ManualAdjustment, SettlementMark }
public enum WalletLedgerEntryStatus { Pending, Approved, Rejected, Reversed, NeedsReview }
public enum WalletTopUpSource { GuardianOnlineProvider, AuthorizedCashier }
public enum WalletTopUpStatus { Draft, Initiated, AwaitingConfirmation, Confirmed, Credited, Failed, Cancelled, Expired, Disputed, ChargedBack, NeedsReview }
public enum PaymentConfirmationStatus { Received, Successful, Failed, Cancelled, Expired, Disputed, ChargedBack, Reversed, Duplicate, NeedsReview, Applied }
public enum MerchantStatus { Draft, Active, Suspended, Retired }
public enum CanteenItemCategoryStatus { Draft, Active, Suspended, Retired }
public enum PurchaseEligibilityRuleStatus { Draft, Active, Suspended, Retired }
public enum PosTerminalStatus { Draft, Active, OfflineAllowed, Suspended, Retired }
public enum PosPurchaseMode { Online, Offline }
public enum PosPurchaseDecision { Submitted, Approved, Denied, HeldForReview, Duplicate, Refunded, Reversed }
public enum OfflinePosSyncStatus { Pending, Accepted, PartiallyAccepted, HeldForReview, Rejected, Duplicate }
public enum SpendingLimitOwnerType { School, Guardian, FinanceReview }
public enum SpendingLimitType { DailyAmount, WeeklyAmount, PerPurchaseAmount, Merchant, Category, TimeWindow, ActiveDateRange, WalletRestriction }
public enum SpendingLimitStatus { Draft, Active, Suspended, Superseded, Expired }
public enum CorrectionType { Refund, Void, Reversal, Hold, Release, ChargebackRecovery, ManualAdjustment }
public enum CorrectionStatus { Requested, Approved, Rejected, Posted, NeedsReview }
public enum SettlementStatus { Open, Matched, Mismatched, Closed, Reopened, NeedsReview }
public enum ReconciliationStatus { Draft, Running, Matched, Mismatched, Closed, Reopened, NeedsReview }
public enum WalletAnomalyType { DuplicateConfirmation, DuplicatePurchase, NegativeAvailableBalance, OfflineOverspend, InvalidCredentialPurchase, SpendingLimitBypass, UnmatchedSettlement, ChargebackAfterSpend, SuspiciousRepeatedAttempt, ManualReviewRequired }
public enum WalletAnomalySeverity { Low, Medium, High, Critical }
public enum WalletAnomalyStatus { New, Assigned, InReview, Resolved, Dismissed, Reopened }
public enum ManualWalletReviewStatus { Requested, InReview, Applied, Rejected, Closed, Escalated }
public enum ManualWalletReviewAction { Assign, Resolve, Dismiss, Correct, Refund, Reverse, Hold, Release, Restrict, RecordRecovery, Escalate, Close, Reopen }
public enum WalletRuleSettingStatus { Draft, Active, Suspended, Superseded }
public enum WalletFeatureStatus { Proposed, Enabled, Disabled, Suspended, Retired }
public enum WalletRetentionState { Detailed, Reduced, ActiveReviewHold, DisputeHold, RecoveryHold, ReconciliationHold }
public enum StudentEligibilityStatus { Active, Inactive, Missing, CrossTenant }
public enum WalletCredentialEvidenceStatus { Active, Expired, Suspended, Revoked, Replaced, Unknown, Duplicated, CrossTenant }
public enum WalletGuardianLinkStatus { Approved, Pending, Suspended, Expired, Removed, Rejected, OutOfScope }
public enum ReviewSummaryStatus { Current, NeedsReview, Closed, RetentionReduced }
