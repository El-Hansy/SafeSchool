# Contract: Wallet Reconciliation

This contract defines finance reconciliation for wallet ledger totals, guardian
top-ups, payment confirmations, POS batches, canteen purchases, refunds,
reversals, chargebacks, settlement references, mismatches, and review actions
for Phase 4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.reconciliation`
  - Related wallet capabilities for records included in the reconciliation.
- Common permissions:
  - `wallet.reconciliation.read`
  - `wallet.reconciliation.manage`
  - `wallet.ledger.read`
  - `wallet.anomalies.read`
  - `wallet.anomalies.resolve`
  - `wallet.audit.read`

Reconciliation reads and mutations are tenant-scoped and must never silently
alter wallet balances. Financial corrections are created through explicit
correction or review workflows.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs` | Start a reconciliation run for a date range, merchant, terminal, provider, or funding source |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs` | List reconciliation runs |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs/{reconciliationRunId}` | Read reconciliation run detail |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs/{reconciliationRunId}/mismatches` | Read unmatched or suspicious records |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs/{reconciliationRunId}/close` | Close a matched or reviewed reconciliation |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs/{reconciliationRunId}/reopen` | Reopen reconciliation after later financial evidence |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/settlement-references` | List settlement references |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/settlement-references/{settlementReferenceId}` | Read settlement reference detail |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reconciliation-runs/{reconciliationRunId}/trace` | Trace reconciliation to ledger, payments, POS batches, corrections, anomalies, reviews, and audit evidence |

## Reconciliation Run Request

```yaml
settlement_scope: "Date Range"
scope_reference: "school-day-YYYY-MM-DD"
period_start: "YYYY-MM-DDT00:00:00Z"
period_end: "YYYY-MM-DDT23:59:59Z"
filters:
  merchant_id: "merchant-reference"
  pos_terminal_id: null
  payment_provider: null
  funding_source: null
client_request_id: "request-unique-to-caller"
```

## Reconciliation Summary Response

```yaml
reconciliation_run_id: "reconciliation-run-reference"
school_account_id: "school-account-reference"
period_start: "YYYY-MM-DDT00:00:00Z"
period_end: "YYYY-MM-DDT23:59:59Z"
settlement_status: "Mismatched"
top_up_totals:
  confirmed_minor: 75000
  credited_minor: 75000
purchase_totals:
  approved_minor: 62000
  offline_held_minor: 1500
refund_reversal_totals:
  posted_minor: 3000
chargeback_totals:
  pending_recovery_minor: 5000
ledger_total_minor: 10000
source_total_minor: 8500
difference_minor: 1500
mismatch_count: 2
anomaly_count: 2
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Mismatch Response

```yaml
items:
  - mismatch_id: "mismatch-reference"
    mismatch_type: "Offline Overspend"
    related_source_type: "Offline POS Sync"
    related_source_reference: "offline-sync-batch-reference"
    student_wallet_id: "wallet-reference"
    amount_minor: 1500
    currency_code: "SAR"
    review_status: "Needs Review"
    anomaly_id: "wallet-anomaly-reference"
    detected_at: "YYYY-MM-DDTHH:MM:SSZ"
page:
  size: 25
  next_cursor: "cursor-reference"
```

## Close Request

```yaml
close_reason: "Finance reviewed mismatches and linked corrections."
manual_wallet_review_id: "manual-review-reference"
client_request_id: "request-unique-to-caller"
```

## Reopen Request

```yaml
reopen_reason: "Provider chargeback arrived after reconciliation close."
related_source_type: "Payment Confirmation"
related_source_reference: "payment-confirmation-reference"
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Reconciliation reads require tenant access, enabled `wallet.reconciliation`,
  and `wallet.reconciliation.read`.
- Reconciliation runs, closes, and reopens require tenant access,
  enabled `wallet.reconciliation`, `wallet.reconciliation.manage`, idempotent
  request identity, and audit evidence.
- Reconciliation must compare wallet ledger totals to top-up confirmations, POS
  batches, purchase records, refunds, reversals, chargebacks, and settlement
  references for the selected scope.
- Matching totals can be closed with review evidence.
- Mismatches, duplicate confirmations, duplicate purchases, unmatched
  settlements, offline overspend, chargebacks after spend, and suspicious
  repeated attempts create or link wallet anomalies for review.
- Reconciliation must not silently alter balances. Corrections are separate
  append-only ledger events with reasons.
- Closed reconciliation evidence remains reviewable. Later chargebacks,
  refunds, reversals, or corrections reopen the run or create a linked review
  item while preserving prior evidence.
- List responses must be paginated and support filters for date range, merchant,
  POS terminal, funding source, payment provider, status, mismatch type,
  anomaly status, reviewer, and latest evidence time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks reconciliation read permission | Deny summary read and record required permission |
| Actor lacks reconciliation manage permission | Deny run, close, or reopen and record required permission |
| Scope references another tenant | Deny without exposing cross-tenant existence |
| Source evidence unavailable | Create reviewable mismatch instead of closing silently |
| Run already closed | Require reopen before additional close or correction linkage |
| Later chargeback changes closed period | Reopen or create linked review item with preserved prior close evidence |
| Duplicate client request identity | Return existing outcome when request content matches |
| Audit write fails for sensitive outcome | Reject mutation rather than allowing unaudited reconciliation change |
