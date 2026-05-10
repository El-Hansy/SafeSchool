# Data Model: Phase 4 Wallet & Payments

This model defines runtime business entities for student wallets, ledger
entries, top-ups, payment confirmations, canteen merchants, POS terminals,
purchase transactions, offline POS sync, spending limits, transaction history,
corrections, reconciliation, anomalies, reviews, rule settings, retention, and
audit evidence. All tenant-owned entities include `tenant_id`, `created_at`,
and `updated_at`.

## Student Wallet

**Purpose**: A school-account stored-value account for one active student.

**Fields**:
- `student_wallet_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student from Phase 1.
- `wallet_code`: School-unique wallet reference.
- `currency_code`: School account configured currency.
- `available_balance_minor`: Spendable confirmed amount in minor units.
- `pending_balance_minor`: Amount waiting for confirmation or review.
- `held_balance_minor`: Amount restricted by holds, disputes, or reviews.
- `settled_balance_minor`: Posted amount included in closed reconciliation.
- `pending_recovery_minor`: Amount under chargeback recovery review.
- `wallet_status`: Draft, Active, Restricted, Suspended, Closed.
- `restriction_reason`: Optional reason for restricted or suspended state.
- `current_rule_setting_id`: Wallet rule setting version in effect.
- `created_by`: Actor that created the wallet.
- `updated_by`: Actor that last changed the wallet.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Student Profile from Phase 1.
- Has many Wallet Ledger Entries, Wallet Top-Ups, Canteen Purchase
  Transactions, Spending Limits, Wallet Anomalies, Manual Wallet Reviews, and
  Wallet Review Summaries.
- Produces Audit Events when created, activated, restricted, suspended, closed,
  or restored.

**Validation rules**:
- One active wallet per student per school account unless a later spec enables
  additional wallet types.
- Wallet activation requires an active student profile, enabled `wallet.ledger`
  capability, tenant access, and `wallet.wallets.manage`.
- Wallet operations are denied for inactive students, cross-school students,
  closed wallets, missing feature capability, or actors without permission.
- Balance fields are stored in minor units and cannot be mutated directly
  outside ledger posting logic.

**State transitions**:
- Draft -> Active when student and tenant eligibility pass.
- Active -> Restricted when chargeback recovery, hold, or financial review
  limits discretionary spending.
- Restricted -> Active when authorized review resolves the restriction.
- Active or Restricted -> Suspended during operational hold.
- Suspended -> Active or Restricted when restored by reviewer outcome.
- Active, Restricted, or Suspended -> Closed when the wallet is retired with no
  unresolved recovery, dispute, or reconciliation hold.

## Wallet Ledger Entry

**Purpose**: Append-only financial evidence that changes or explains wallet
balance.

**Fields**:
- `wallet_ledger_entry_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Wallet affected by the entry.
- `entry_type`: Credit, Debit, Hold, Release, Refund, Reversal, Chargeback,
  Recovery, Manual Adjustment, Settlement Mark.
- `entry_status`: Pending, Approved, Rejected, Reversed, Needs Review.
- `amount_minor`: Signed or directional amount in minor units.
- `currency_code`: School account configured currency.
- `balance_available_after_minor`: Available balance after approved posting.
- `balance_pending_after_minor`: Pending balance after approved posting.
- `balance_held_after_minor`: Held balance after approved posting.
- `source_type`: Top-Up, Payment Confirmation, Canteen Purchase, Refund,
  Reversal, Chargeback, Offline Sync, Manual Review, Reconciliation.
- `source_reference`: Stable source event reference.
- `original_entry_id`: Entry corrected or reversed when applicable.
- `idempotency_key`: Caller-stable identity for retry-safe posting.
- `rule_snapshot_reference`: Spending or wallet rule version used when
  applicable.
- `posted_at`: Time the entry affected wallet balances.
- `review_reason`: Reason when entry requires or records review.
- `created_by`: Actor or system source that created the entry.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Wallet.
- May reference Wallet Top-Up, Payment Confirmation, Canteen Purchase
  Transaction, Refund or Reversal, Settlement Reference, Wallet Anomaly, Manual
  Wallet Review, and Audit Event records.

**Validation rules**:
- Approved ledger entries cannot be edited or deleted directly.
- Corrections must create reversal, refund, recovery, release, or adjustment
  entries that reference original evidence.
- `idempotency_key` must be unique for the tenant, wallet, source type, and
  source reference combination.
- A debit cannot reduce available balance below zero unless it is an approved
  bounded offline reserve outcome routed through review rules.
- Ledger posting requires tenant access, feature capability, permission, money
  precision validation, and audit evidence.

**State transitions**:
- Pending -> Approved when all posting rules and audit evidence pass.
- Pending -> Rejected when validation fails before posting.
- Approved -> Reversed through a new reversal entry.
- Approved -> Needs Review when reconciliation or anomaly rules flag the entry.

## Wallet Top-Up

**Purpose**: A guardian, cashier, or school-approved funding event for a
student wallet.

**Fields**:
- `wallet_top_up_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Wallet receiving funds.
- `student_profile_id`: Student receiving funds.
- `initiated_by_actor_id`: Guardian, cashier, or finance actor.
- `guardian_link_id`: Guardian relationship when guardian-initiated.
- `top_up_source`: Guardian Online Provider, Authorized Cashier.
- `amount_minor`: Requested top-up amount in minor units.
- `currency_code`: School account configured currency.
- `fee_minor`: Optional fee amount when supplied by funding source.
- `net_credit_minor`: Amount to credit after approved rules.
- `top_up_status`: Draft, Initiated, Awaiting Confirmation, Confirmed,
  Credited, Failed, Cancelled, Expired, Disputed, Charged Back, Needs Review.
- `payment_provider_reference`: Safe external provider reference when
  applicable.
- `cashier_reference`: Cashier evidence when applicable.
- `idempotency_key`: Caller-stable request identity.
- `initiated_at`: Top-up initiation time.
- `confirmed_at`: Successful confirmation time when applicable.
- `credited_at`: Ledger credit posting time when applicable.
- `review_reason`: Reason for failure, hold, dispute, or review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Wallet and School Account.
- Guardian-initiated top-ups reference an approved active Guardian Link from
  Phase 1.
- Has many Payment Confirmations.
- Produces one approved wallet credit ledger entry when confirmed and credited.
- May produce Wallet Anomalies, Manual Wallet Reviews, Audit Events, and
  Reconciliation evidence.

**Validation rules**:
- Guardian top-up requires active guardian link, enabled `wallet.top_up` and
  `wallet.payment_processing`, tenant access, linked student scope, and
  `wallet.topups.initiate`.
- Cashier top-up requires enabled `wallet.top_up`, tenant access, active wallet,
  cashier permission, source evidence, reason, and audit evidence.
- Failed, cancelled, expired, pending, disputed, duplicate, and cross-school
  top-ups do not create spendable credits.
- Top-up amount must satisfy school wallet rule settings for minimum, maximum,
  currency, and cashier threshold.

**State transitions**:
- Draft -> Initiated when request validation passes.
- Initiated -> Awaiting Confirmation for guardian online payment.
- Awaiting Confirmation -> Confirmed when provider success evidence is
  accepted idempotently.
- Confirmed -> Credited when one approved wallet credit ledger entry posts.
- Initiated or Awaiting Confirmation -> Failed, Cancelled, or Expired when
  source evidence says so.
- Confirmed or Credited -> Disputed or Charged Back when later provider
  evidence requires review.
- Any non-closed status -> Needs Review for duplicate, cross-school, amount,
  audit, or reconciliation conflicts.

## Payment Confirmation

**Purpose**: Safe normalized evidence from an approved external payment provider
or authorized funding source.

**Fields**:
- `payment_confirmation_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `wallet_top_up_id`: Related top-up when resolved.
- `provider_reference`: Safe provider payment reference.
- `provider_event_id`: Provider event identity for idempotency.
- `confirmation_status`: Successful, Failed, Cancelled, Expired, Disputed,
  Charged Back, Reversed, Duplicate, Needs Review.
- `amount_minor`: Provider-reported amount in minor units.
- `currency_code`: Provider-reported currency.
- `safe_payment_method_summary`: Non-sensitive method summary.
- `received_at`: Time the confirmation was received.
- `provider_event_time`: Source event time when supplied.
- `raw_payload_reference`: Secure internal reference to stored normalized
  evidence when allowed; never full external payment credentials.
- `idempotency_key`: Unique provider event key.
- `review_reason`: Reason when held or rejected.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Wallet Top-Up when matched.
- May create Ledger Entry, Wallet Anomaly, Manual Wallet Review,
  Reconciliation, and Audit Event records.

**Validation rules**:
- Provider event identity must be unique within the tenant and provider
  adapter.
- Full external payment credentials must never be stored or displayed.
- Confirmation amount, currency, tenant, top-up, and provider reference must
  match before a credit is posted.
- Duplicate confirmations return the existing outcome without duplicate wallet
  credits.

**State transitions**:
- Received -> Successful, Failed, Cancelled, Expired, Disputed, Charged Back,
  Reversed, Duplicate, or Needs Review after normalization and validation.
- Successful -> Applied when linked top-up is credited once.
- Disputed or Charged Back -> Recovery Review when credited funds were already
  spent.

## Canteen Merchant

**Purpose**: A school-authorized canteen operator or outlet that can accept
wallet purchases.

**Fields**:
- `canteen_merchant_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `merchant_name`: Human-readable name.
- `merchant_code`: School-unique merchant code.
- `merchant_status`: Draft, Active, Suspended, Retired.
- `allowed_category_codes`: Item categories this merchant may sell when
  restricted.
- `settlement_reference_policy`: Required settlement evidence for finance
  review.
- `created_by`: Actor that created the merchant.
- `updated_by`: Actor that last changed the merchant.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many POS Terminals and Canteen Purchase Transactions.
- May appear in Spending Limits, Settlement References, Wallet Anomalies, and
  Review Summaries.

**Validation rules**:
- `merchant_code` must be unique within the school account while active.
- Active merchants require enabled `wallet.canteen_pos` and
  `wallet.purchases.record` or merchant management permissions.
- Suspended or retired merchants cannot receive normal wallet purchases.

**State transitions**:
- Draft -> Active when required merchant details pass validation.
- Active -> Suspended during operational hold.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Canteen Item Category

**Purpose**: A tenant-owned item grouping used by canteen POS eligibility,
guardian transaction summaries, and spending limit evaluation.

**Fields**:
- `canteen_item_category_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `category_code`: School-unique category code.
- `category_name`: Human-readable category name.
- `guardian_summary_label`: Safe guardian-facing label.
- `category_status`: Draft, Active, Suspended, Retired.
- `created_by`: Actor that created the category.
- `updated_by`: Actor that last changed the category.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May be referenced by Purchase Eligibility Rules, Canteen Purchase
  Transactions, Spending Limits, Wallet Review Summaries, and Audit Events.

**Validation rules**:
- `category_code` must be unique within the school account while active.
- Purchases cannot use suspended, retired, or cross-school categories for normal
  wallet debits.
- Guardian-facing labels must not expose staff-only POS or settlement detail.

**State transitions**:
- Draft -> Active when category details pass validation.
- Active -> Suspended during temporary hold.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Purchase Eligibility Rule

**Purpose**: A baseline tenant-owned merchant/category rule evaluated before
normal canteen wallet debits.

**Fields**:
- `purchase_eligibility_rule_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `canteen_merchant_id`: Merchant scope.
- `category_code`: Item category scope.
- `eligibility_action`: Allow, Deny, Hold for Review.
- `rule_status`: Draft, Active, Suspended, Superseded, Expired.
- `valid_from`: First effective time.
- `valid_to`: Optional last effective time.
- `change_reason`: Required reason for rule changes.
- `created_by`: Actor that created the rule.
- `updated_by`: Actor that last changed the rule.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Canteen Merchant and one Canteen Item Category.
- May be snapshotted by Canteen Purchase Transactions and referenced by Wallet
  Anomalies, Manual Wallet Reviews, and Audit Events.

**Validation rules**:
- Active rules require active merchant and active item category within the same
  school account.
- Purchase authorization evaluates active merchant/category eligibility before
  creating a normal wallet debit.
- Deny or hold rules preserve reviewable reasons without silently changing
  balances.

**State transitions**:
- Draft -> Active when merchant, category, dates, and permissions pass.
- Active -> Suspended during temporary hold.
- Active -> Superseded when a new version replaces it.
- Active or Suspended -> Expired when `valid_to` passes.

## POS Terminal

**Purpose**: A school-authorized purchase source associated with a canteen
merchant.

**Fields**:
- `pos_terminal_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `canteen_merchant_id`: Merchant that owns or operates the terminal.
- `terminal_code`: School-unique terminal code.
- `terminal_label`: Human-readable label.
- `terminal_status`: Draft, Active, Offline Allowed, Suspended, Retired.
- `operator_actor_id`: Optional assigned operator.
- `device_reference`: Optional registered device identity.
- `offline_enabled`: Whether this terminal may capture offline purchases.
- `offline_terminal_reserve_limit_minor`: Maximum terminal exposure while
  offline.
- `last_sync_at`: Last accepted sync time.
- `created_by`: Actor that created the terminal.
- `updated_by`: Actor that last changed the terminal.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Canteen Merchant and School Account.
- Has many Canteen Purchase Transactions and Offline POS Sync Batches.
- May appear in Spending Limits, Settlement References, Wallet Anomalies,
  Manual Wallet Reviews, and Audit Events.

**Validation rules**:
- Active terminals require an active merchant and enabled `wallet.canteen_pos`.
- Offline capture requires school wallet rule settings that define both
  per-student and per-terminal reserve limits.
- A suspended or retired terminal cannot create normal online or offline
  purchases.
- Device and operator identity must match tenant, terminal, and permissions.

**State transitions**:
- Draft -> Active when terminal details and merchant status pass validation.
- Active -> Offline Allowed when offline POS is enabled and reserve rules are
  valid.
- Active or Offline Allowed -> Suspended during operational hold.
- Suspended -> Active or Offline Allowed when restored.
- Active, Offline Allowed, or Suspended -> Retired when no longer used.

## Offline POS Sync Batch

**Purpose**: A retry-safe submission of offline canteen purchase attempts from
one POS source.

**Fields**:
- `offline_pos_sync_batch_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `client_batch_id`: Caller-stable batch identifier.
- `canteen_merchant_id`: Merchant source.
- `pos_terminal_id`: Terminal source.
- `device_reference`: Registered device source.
- `operator_actor_id`: Operator when available.
- `submitted_at`: Time the batch was received.
- `batch_status`: Received, Partially Reconciled, Reconciled, Rejected, Needs
  Review.
- `accepted_count`: Purchases accepted or matched as retries.
- `held_count`: Purchases held for review.
- `rejected_count`: Purchases rejected.
- `duplicate_count`: Purchases identified as duplicates.
- `total_amount_minor`: Submitted total in minor units.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account, Canteen Merchant, and POS Terminal.
- Contains many Canteen Purchase Transactions.
- Produces Audit Events and Reconciliation evidence.

**Validation rules**:
- `client_batch_id` must be unique per tenant and POS source for idempotent
  retry handling.
- Retried batches with the same client identity return the same outcomes where
  submitted contents match.
- Batches from unauthorized, suspended, retired, or cross-school terminals are
  rejected or routed to review.

## Canteen Purchase Transaction

**Purpose**: A purchase attempt or approved wallet debit tied to student wallet,
identity evidence, merchant, terminal, items or categories, amount, decision,
and review status.

**Fields**:
- `canteen_purchase_transaction_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Wallet charged or evaluated.
- `student_profile_id`: Resolved student when known.
- `identity_credential_id`: Credential resolved from Phase 1 evidence when
  known.
- `credential_type`: NFC Card, QR Fallback, or Manual Review Entry.
- `credential_reference`: Non-secret credential reference presented.
- `canteen_merchant_id`: Merchant source.
- `pos_terminal_id`: Terminal source.
- `offline_pos_sync_batch_id`: Related batch when captured offline.
- `client_purchase_id`: Caller-stable purchase identity for idempotency.
- `item_summary`: Item or category summary.
- `category_codes`: Categories used for limits and guardian history.
- `amount_minor`: Purchase amount in minor units.
- `currency_code`: School account configured currency.
- `purchase_mode`: Online, Offline, Manual Review Entry.
- `local_purchase_time`: POS local event time.
- `received_at`: Server received time.
- `rule_snapshot_reference`: Spending and wallet rule version used.
- `reserve_snapshot_reference`: Offline reserve snapshot when applicable.
- `purchase_decision`: Approved, Denied, Held for Review, Duplicate,
  Reversed, Refunded.
- `decision_reason`: Human-reviewable reason.
- `ledger_entry_id`: Approved debit entry when posted.
- `review_status`: Not Required, Needs Review, In Review, Corrected, Closed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Wallet, Canteen Merchant, POS Terminal, and School
  Account.
- May belong to an Offline POS Sync Batch.
- May reference Spending Limits, Wallet Ledger Entries, Refunds or Reversals,
  Wallet Anomalies, Manual Wallet Reviews, Settlement References, and Audit
  Events.

**Validation rules**:
- Online approval requires enabled `wallet.canteen_pos`, tenant access, active
  wallet, active credential, active merchant, active terminal, authorized actor
  or device, sufficient available balance, active spending rule satisfaction,
  idempotent purchase identity, and audit evidence.
- Offline capture is allowed only when offline POS is enabled and both
  per-student and per-terminal reserve snapshots permit the amount.
- Denied or held purchases do not create normal debit entries.
- Duplicate submissions return the existing purchase outcome without a second
  debit.

**State transitions**:
- Submitted -> Approved when all online authorization rules pass and a debit is
  posted.
- Submitted -> Denied when validation fails without review need.
- Submitted -> Held for Review when offline, stale, duplicate, reserve,
  credential, tenant, or rule conflicts require reviewer action.
- Approved -> Refunded through a refund entry.
- Approved -> Reversed through a reversal entry.
- Held for Review -> Approved, Denied, Corrected, or Closed through Manual
  Wallet Review.

## Spending Limit

**Purpose**: A school or guardian rule that restricts wallet purchases.

**Fields**:
- `spending_limit_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Wallet affected by the rule.
- `student_profile_id`: Student affected by the rule.
- `limit_owner_type`: School, Guardian, Finance Review.
- `guardian_link_id`: Guardian relationship when guardian-owned.
- `limit_type`: Daily Amount, Weekly Amount, Per Purchase Amount, Merchant,
  Category, Time Window, Active Date Range, Wallet Restriction.
- `amount_limit_minor`: Optional amount threshold in minor units.
- `merchant_reference`: Optional merchant scope.
- `category_code`: Optional category scope.
- `time_window`: Optional school local time window.
- `valid_from`: First effective time.
- `valid_to`: Optional last effective time.
- `precedence_rank`: Rank used by strictest-rule evaluation.
- `limit_status`: Draft, Active, Suspended, Superseded, Expired.
- `rule_version`: Reviewable version used in purchase snapshots.
- `created_by`: Actor that created the rule.
- `updated_by`: Actor that last changed the rule.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Wallet and School Account.
- Guardian-owned limits reference an approved active Guardian Link.
- May be referenced by Canteen Purchase Transactions and Review Summaries.

**Validation rules**:
- Limit management requires enabled `wallet.spending_limits`, tenant access,
  linked student scope for guardians, and `wallet.limits.manage`.
- Future purchases enforce the strictest active applicable rule.
- Historical purchase decisions reference the rule version used at decision
  time.
- Guardian limits cannot loosen stricter school or review restrictions.

**State transitions**:
- Draft -> Active when all fields and permissions pass.
- Active -> Suspended during temporary hold.
- Active -> Superseded when a new version replaces it.
- Active or Suspended -> Expired when `valid_to` passes.

## Refund or Reversal

**Purpose**: A corrective financial event that references an original top-up,
purchase, chargeback, or adjustment.

**Fields**:
- `refund_reversal_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Wallet affected by the correction.
- `correction_type`: Refund, Void, Reversal, Hold, Release, Chargeback
  Recovery, Manual Adjustment.
- `original_source_type`: Top-Up, Purchase, Ledger Entry, Settlement,
  Chargeback.
- `original_source_reference`: Original event reference.
- `amount_minor`: Correction amount in minor units.
- `currency_code`: School account configured currency.
- `correction_status`: Requested, Approved, Rejected, Posted, Needs Review.
- `reason`: Required correction reason.
- `reviewer_actor_id`: Actor approving or applying the correction.
- `ledger_entry_id`: Ledger entry posted by the correction when applicable.
- `client_request_id`: Caller-stable identity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Wallet.
- References original financial evidence and resulting Wallet Ledger Entry.
- May create Wallet Anomaly, Manual Wallet Review, Reconciliation, and Audit
  Event records.

**Validation rules**:
- Corrections require `wallet.corrections.manage` or more specific review
  permission, tenant access, active review reason, idempotent request identity,
  and audit evidence.
- Corrections never edit or delete original entries.
- Refunds and reversals after closed reconciliation create new review items
  rather than modifying closed evidence.

**State transitions**:
- Requested -> Approved when reviewer validation passes.
- Approved -> Posted when ledger correction entry posts.
- Requested -> Rejected when validation fails.
- Any non-posted status -> Needs Review when evidence is incomplete or
  conflicting.

## Settlement Reference

**Purpose**: A reconciliation record that groups payment confirmations, POS
batches, purchases, refunds, reversals, chargebacks, and mismatches.

**Fields**:
- `settlement_reference_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `settlement_scope`: Payment Provider, Merchant, POS Terminal, Date Range,
  Funding Source.
- `scope_reference`: Provider, merchant, terminal, or funding source reference.
- `period_start`: Start time for the settlement period.
- `period_end`: End time for the settlement period.
- `expected_total_minor`: Expected total from source evidence.
- `ledger_total_minor`: Total from wallet ledger entries.
- `difference_minor`: Difference between source and ledger totals.
- `settlement_status`: Draft, Matched, Mismatched, In Review, Closed,
  Reopened.
- `closed_by`: Actor closing the reconciliation when applicable.
- `closed_at`: Closure time when applicable.
- `review_reason`: Reason for mismatch or reopening.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References Payment Confirmations, POS batches, Purchase Transactions, Ledger
  Entries, Refunds, Reversals, Chargebacks, Wallet Anomalies, Manual Wallet
  Reviews, and Audit Events.

**Validation rules**:
- Reconciliation requires tenant access and `wallet.reconciliation.read` or
  `wallet.reconciliation.manage`.
- Mismatches do not silently alter balances.
- Closed settlement evidence remains reviewable; later corrections reopen or
  create new review items.

**State transitions**:
- Draft -> Matched when totals reconcile.
- Draft -> Mismatched when unresolved differences exist.
- Mismatched -> In Review when assigned to reviewer.
- Matched or In Review -> Closed when authorized finance review completes.
- Closed -> Reopened when later chargeback, refund, reversal, or correction
  changes the financial position.

## Wallet Anomaly

**Purpose**: A reviewable issue involving duplicate, conflicting, suspicious,
invalid, overspent, disputed, unmatched, or manual-review-required wallet
evidence.

**Fields**:
- `wallet_anomaly_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Affected wallet when applicable.
- `related_source_type`: Top-Up, Payment Confirmation, Purchase, Offline Sync,
  Spending Limit, Settlement, Ledger Entry, Manual Review.
- `related_source_reference`: Source event reference.
- `anomaly_type`: Duplicate Top-Up Confirmation, Duplicate POS Purchase,
  Negative Available Balance, Offline Overspend, Invalid Credential Purchase,
  Spending Limit Bypass, Unmatched Settlement, Chargeback After Spend,
  Suspicious Repeated Attempt, Manual Review Required.
- `severity`: Low, Medium, High, Critical.
- `anomaly_status`: Open, Assigned, In Review, Resolved, Dismissed, Reopened.
- `reviewer_actor_id`: Assigned reviewer when applicable.
- `resolution_reason`: Resolution or dismissal reason.
- `detected_at`: Detection time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and may belong to one Student Wallet.
- References related Wallet Top-Up, Payment Confirmation, Purchase, Ledger
  Entry, Settlement Reference, Manual Wallet Review, and Audit Event records.

**Validation rules**:
- Anomaly access requires tenant access and `wallet.anomalies.read`.
- Resolution requires `wallet.anomalies.resolve`, reason, preserved original
  evidence, and audit evidence.
- Resolving an anomaly does not mutate ledger entries except through explicit
  correction workflows.

**State transitions**:
- Open -> Assigned when reviewer assignment is recorded.
- Open or Assigned -> In Review when investigation starts.
- In Review -> Resolved or Dismissed with a reason.
- Resolved or Dismissed -> Reopened when new evidence arrives.

## Manual Wallet Review

**Purpose**: A reviewer action that resolves, dismisses, corrects, refunds,
reverses, holds, releases, restricts, records recovery, or escalates wallet
activity with a reason and history.

**Fields**:
- `manual_wallet_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_wallet_id`: Affected wallet when applicable.
- `review_scope`: Wallet, Ledger Entry, Top-Up, Payment Confirmation,
  Purchase, Spending Limit, Settlement, Anomaly.
- `scope_reference`: Reviewed entity reference.
- `review_action`: Assign, Resolve, Dismiss, Correct, Refund, Reverse, Hold,
  Release, Restrict, Record Recovery, Escalate, Close.
- `review_status`: Requested, In Review, Applied, Rejected, Closed.
- `review_reason`: Required reason.
- `reviewer_actor_id`: Reviewer actor.
- `resulting_ledger_entry_id`: Ledger entry created by review when applicable.
- `client_request_id`: Caller-stable identity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and may belong to one Student Wallet.
- May reference Wallet Anomaly, Refund or Reversal, Ledger Entry, Settlement
  Reference, Wallet Rule Setting, and Audit Event records.

**Validation rules**:
- Review actions require tenant access, specific review permission, reason, and
  audit evidence.
- Review outcomes preserve original source evidence.
- Wallet restoration after chargeback recovery requires a review action that
  resolves pending recovery and restriction state.

**State transitions**:
- Requested -> In Review when reviewer accepts assignment.
- In Review -> Applied when the review action is executed.
- In Review -> Rejected when the action is denied.
- Applied or Rejected -> Closed when no follow-up remains.

## Wallet Rule Setting

**Purpose**: A school account configuration record for wallet money, top-up,
offline POS, spending, refund, chargeback, anomaly, reconciliation, and
retention rules.

**Fields**:
- `wallet_rule_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `currency_code`: Configured wallet currency.
- `top_up_minimum_minor`: Minimum allowed top-up.
- `top_up_maximum_minor`: Maximum allowed top-up.
- `cashier_adjustment_threshold_minor`: Amount requiring finance review.
- `offline_pos_enabled`: Whether offline POS capture is allowed.
- `offline_student_reserve_limit_minor`: Per-student offline exposure limit.
- `offline_terminal_reserve_limit_minor`: Default per-terminal exposure limit.
- `spending_limit_precedence_policy`: Strictest Applicable or approved policy.
- `refund_window_policy`: School refund window setting.
- `chargeback_handling_policy`: Restrict Wallet and Require Review.
- `anomaly_detection_policy`: Enabled anomaly types and thresholds.
- `duplicate_retry_handling_policy`: Idempotent Client Identifiers.
- `reconciliation_review_threshold_minor`: Difference requiring review.
- `financial_record_retention_policy`: School-configured detailed record
  retention rule.
- `rule_setting_status`: Draft, Active, Suspended, Superseded.
- `created_by`: Actor that created the setting.
- `updated_by`: Actor that last changed the setting.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Versioned and referenced by wallets, top-ups, purchases, spending limits,
  reviews, reconciliation records, and audit events.

**Validation rules**:
- Rule management requires tenant access and `wallet.rules.manage`.
- Active rules require valid currency, top-up ranges, offline reserve values,
  chargeback policy, anomaly policy, duplicate retry policy, reconciliation
  thresholds, retention policy, and audit evidence.
- Activating a new setting supersedes the prior active setting without changing
  historical event evidence.
- Retention policy may have no platform-wide minimum, but active review,
  dispute, recovery, and reconciliation holds are preserved.

**State transitions**:
- Draft -> Active when all values pass validation.
- Active -> Suspended during policy hold.
- Active -> Superseded when a newer active setting is approved.
- Suspended -> Active when restored.

## Wallet Review Summary

**Purpose**: A permission-scoped view of wallet status, transaction status,
merchant activity, settlement status, anomalies, and review outcomes.

**Fields**:
- `wallet_review_summary_id`: Stable identifier or computed read-model key.
- `tenant_id`: Owning school account.
- `summary_scope`: Student Wallet, Guardian, Merchant, POS Terminal, Date
  Range, Settlement, Anomaly.
- `scope_reference`: Scope identifier.
- `wallet_status_counts`: Counts by wallet state.
- `transaction_status_counts`: Counts by top-up, purchase, refund, reversal,
  chargeback, and pending status.
- `spending_limit_status_counts`: Counts by limit state.
- `settlement_status_counts`: Counts by reconciliation state.
- `anomaly_status_counts`: Counts by anomaly state.
- `review_status_counts`: Counts by review state.
- `latest_evidence_at`: Most recent source evidence time.
- `trace_reference`: Links to ledger, transaction, settlement, anomaly, and
  audit traces.
- `created_at`: Creation time when persisted.
- `updated_at`: Last update time when persisted.

**Relationships**:
- Computed from Student Wallet, Ledger Entry, Top-Up, Payment Confirmation,
  Canteen Purchase, Spending Limit, Settlement, Wallet Anomaly, Manual Wallet
  Review, and Audit Event records.

**Validation rules**:
- Summary reads require tenant access and `wallet.history.read`,
  `wallet.reconciliation.read`, `wallet.anomalies.read`, or
  `wallet.guardian_history.read` depending on scope.
- Guardian-scoped summaries are limited to linked-student data and hide
  staff-only terminal, operator, settlement, and internal review assignment
  details.
- Summary filters must not expose cross-school existence.

## School Account Feature Setting

**Purpose**: A school account capability setting that determines whether Phase
4 workflows are available.

**Fields**:
- `school_account_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `capability_key`: Wallet capability key.
- `capability_status`: Enabled, Disabled, Suspended.
- `effective_from`: Start time.
- `effective_to`: Optional end time.
- `changed_by`: Actor that changed the setting.
- `change_reason`: Required reason for sensitive capability changes.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Governs Student Wallet, Top-Up, Payment, Spending Limit, History, and Canteen
  POS workflows.
- Produces Audit Events when capability changes.

**Validation rules**:
- Backend APIs must check capability availability before wallet workflow
  execution.
- UI gates improve usability but are not an authorization boundary.
- Disabling one Phase 4 capability must not bypass audit, retention, or review
  obligations for existing records.
