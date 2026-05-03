# Contract: Canteen POS Purchase

This contract defines canteen merchant and POS purchase capture, wallet debit
authorization, NFC/QR identity evidence, online purchase idempotency, bounded
offline POS sync, denial evidence, and purchase traceability for Phase 4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.canteen_pos`
  - `wallet.ledger`
- Common permissions:
  - `wallet.purchases.record`
  - `wallet.purchases.sync`
  - `wallet.purchases.read`
  - `wallet.ledger.read`
  - `wallet.anomalies.read`
  - `wallet.audit.read`

Purchase commands must be idempotent and must not create duplicate wallet
debits when a POS terminal retries the same physical purchase.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/merchants` | Create a canteen merchant |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/canteen/merchants/{merchantId}` | Update merchant details or status |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/item-categories` | Create a canteen item category for POS eligibility and guardian summaries |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/canteen/item-categories/{itemCategoryId}` | Update item category details or status |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchase-eligibility-rules` | Create a baseline merchant/category purchase eligibility rule |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchase-eligibility-rules/{purchaseEligibilityRuleId}` | Update purchase eligibility rule details or status |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/terminals` | Create a POS terminal |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/canteen/terminals/{terminalId}` | Update terminal details or status |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases` | Record an online canteen wallet purchase attempt |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases/sync` | Submit an offline POS purchase batch |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases` | Review purchases with filters |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases/{purchaseId}` | Read purchase details |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases/{purchaseId}/trace` | Trace purchase to wallet, credential, ledger, limit, anomaly, review, reconciliation, and audit outcomes |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/canteen/purchases/{purchaseId}/review-outcome` | Approve, deny, correct, refund, or reverse a held purchase |

## Merchant Request

```yaml
merchant_name: "Main Canteen"
merchant_code: "MAIN-CANTEEN"
allowed_category_codes:
  - "meal"
  - "snack"
merchant_status: "Active"
client_request_id: "request-unique-to-caller"
```

## Item Category Request

```yaml
category_code: "meal"
category_name: "Meals"
category_status: "Active"
guardian_summary_label: "Meal"
client_request_id: "request-unique-to-caller"
```

## Purchase Eligibility Rule Request

```yaml
canteen_merchant_id: "merchant-reference"
category_code: "meal"
rule_status: "Active"
eligibility_action: "Allow"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
change_reason: "Main canteen may sell meal items through wallet POS."
client_request_id: "request-unique-to-caller"
```

## POS Terminal Request

```yaml
canteen_merchant_id: "merchant-reference"
terminal_code: "POS-01"
terminal_label: "Main counter"
device_reference: "registered-device-reference"
operator_actor_id: "operator-actor-reference"
offline_enabled: true
offline_terminal_reserve_limit_minor: 50000
client_request_id: "request-unique-to-caller"
```

## Online Purchase Request

```yaml
client_purchase_id: "purchase-unique-to-terminal"
student_wallet_id: "wallet-reference"
credential_type: "NFC Card"
credential_reference: "non-secret-card-reference"
canteen_merchant_id: "merchant-reference"
pos_terminal_id: "terminal-reference"
operator_actor_id: "operator-actor-reference"
device_reference: "registered-device-reference"
item_summary: "Lunch combo"
category_codes:
  - "meal"
amount_minor: 1200
currency_code: "SAR"
local_purchase_time: "YYYY-MM-DDTHH:MM:SSZ"
credential_snapshot_version: "snapshot-version-reference"
```

## Offline Sync Request

```yaml
client_batch_id: "batch-unique-to-terminal"
canteen_merchant_id: "merchant-reference"
pos_terminal_id: "terminal-reference"
device_reference: "registered-device-reference"
operator_actor_id: "operator-actor-reference"
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
purchases:
  - client_purchase_id: "purchase-unique-to-terminal"
    student_wallet_id: "wallet-reference"
    credential_type: "NFC Card"
    credential_reference: "non-secret-card-reference"
    item_summary: "Snack"
    category_codes:
      - "snack"
    amount_minor: 500
    currency_code: "SAR"
    local_purchase_time: "YYYY-MM-DDTHH:MM:SSZ"
    credential_snapshot_version: "snapshot-version-reference"
    spending_rule_snapshot_reference: "rule-snapshot-reference"
    offline_student_reserve_snapshot_minor: 2000
    offline_terminal_reserve_snapshot_minor: 50000
```

## Purchase Response

```yaml
canteen_purchase_transaction_id: "purchase-reference"
school_account_id: "school-account-reference"
student_wallet_id: "wallet-reference"
student_profile_id: "student-profile-reference"
identity_credential_id: "credential-reference"
canteen_merchant_id: "merchant-reference"
pos_terminal_id: "terminal-reference"
amount_minor: 1200
currency_code: "SAR"
purchase_mode: "Online"
purchase_decision: "Approved"
decision_reason: "Wallet active, credential active, funds available, and limits satisfied."
wallet_ledger_entry_id: "ledger-entry-reference"
available_balance_after_minor: 3800
review_status: "Not Required"
received_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Offline Sync Response

```yaml
offline_pos_sync_batch_id: "offline-pos-sync-batch-reference"
school_account_id: "school-account-reference"
client_batch_id: "batch-unique-to-terminal"
batch_status: "Partially Reconciled"
accepted_count: 18
held_count: 2
duplicate_count: 1
rejected_count: 0
items:
  - client_purchase_id: "purchase-unique-to-terminal"
    canteen_purchase_transaction_id: "purchase-reference"
    purchase_decision: "Approved"
    decision_reason: "Offline reserves and later wallet reconciliation accepted."
```

## Review Outcome Request

```yaml
review_action: "Approve"
corrected_decision: "Approved"
review_reason: "Reviewer confirmed reserve snapshot and active credential."
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Merchant, item category, purchase eligibility rule, and terminal management
  require tenant access, enabled `wallet.canteen_pos`, management permission,
  idempotent request identity, and audit evidence.
- Item categories must be tenant-owned, status-controlled, and safe for guardian
  item/category summaries.
- Purchase eligibility rules must be active, tenant-owned, merchant/category
  scoped, and evaluated before a normal purchase debit is posted.
- Online purchase approval requires active wallet, active credential, active
  merchant, active item category, active purchase eligibility rule when one is
  required for the merchant/category pair, active terminal, authorized actor or
  device, sufficient available balance, active spending limit satisfaction,
  enabled `wallet.canteen_pos`, enabled `wallet.ledger`, idempotent
  `client_purchase_id`, and audit evidence.
- Insufficient balance, invalid credential, unauthorized merchant, suspended
  terminal, disabled capability, spending limit violation, and cross-school
  wallet attempts are denied or held without creating a normal debit.
- Offline POS capture is allowed only when school wallet rules enable offline
  spending and both per-student and per-terminal reserve snapshots allow the
  purchase amount.
- Offline sync requires `wallet.purchases.sync`, authorized terminal or device,
  caller-stable `client_batch_id`, caller-stable `client_purchase_id`, local
  time, received time, credential evidence, spending rule snapshot, reserve
  snapshot, and reconciliation outcome.
- Duplicate purchase submissions return the existing outcome or mark duplicate
  without a second wallet debit.
- Held purchase review requires a reason, preserved original evidence, manual
  review record, and audit evidence before a debit, denial, correction, refund,
  or reversal can be applied.
- Purchase list responses must be paginated and support filtering by student,
  wallet, credential, merchant, terminal, operator, purchase mode, amount,
  category, decision, sync status, anomaly status, review status, and time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny POS workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor or device lacks purchase permission | Deny and record required permission |
| Merchant inactive or cross-school | Reject purchase without exposing cross-tenant existence |
| Item category inactive or cross-school | Reject purchase without exposing cross-tenant existence |
| Purchase eligibility rule denies merchant/category | Deny or hold purchase with rule reason |
| Terminal inactive, suspended, or cross-school | Reject or hold purchase with terminal reason |
| Wallet inactive, restricted, or cross-school | Deny or hold according to wallet status |
| Credential invalid, revoked, replaced, duplicated, or cross-school | Deny or hold without normal wallet debit |
| Insufficient available funds | Deny without creating normal debit |
| Spending limit violated | Deny or hold with rule snapshot reason |
| Offline reserves exceeded | Hold for review without normal debit |
| Duplicate client purchase identity | Return existing outcome without duplicate debit |
| Device clock drift detected | Accept with review signal or hold according to rules |
| Audit write fails for sensitive outcome | Reject sensitive mutation rather than allowing unaudited purchase |
