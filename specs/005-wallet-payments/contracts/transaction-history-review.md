# Contract: Transaction History and Review

This contract defines school and guardian transaction history, privacy-limited
guardian detail, refunds, voids, reversals, holds, releases, chargeback
recovery, manual wallet reviews, and correction traceability for Phase 4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.transaction_history`
  - Related workflow capabilities apply when history includes top-up, payment,
    POS, limit, or reconciliation records.
- Common permissions:
  - `wallet.history.read`
  - `wallet.guardian_history.read`
  - `wallet.corrections.manage`
  - `wallet.chargebacks.review`
  - `wallet.anomalies.resolve`
  - `wallet.audit.read`

Transaction history must be permission-scoped. Guardians see amount, merchant,
item or category summary, status, and corrections for linked students only, and
must not see staff-only POS terminal, operator, settlement, or internal review
assignment details.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/wallet/transactions` | Read school-scoped wallet transaction history |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/transactions` | Read one wallet's transaction history |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/wallet/transactions` | Read guardian-visible linked-student history |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/transactions/{transactionId}` | Read transaction detail by normalized transaction reference |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/reviews` | Create or apply manual wallet review action |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reviews` | List manual wallet reviews |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/reviews/{manualWalletReviewId}` | Read manual wallet review detail |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/refunds-reversals` | Create refund, void, reversal, hold, release, recovery, or manual adjustment |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/refunds-reversals/{refundReversalId}` | Read correction detail |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/transactions/{transactionId}/trace` | Trace transaction to original event, correction, ledger, anomaly, review, reconciliation, and audit evidence |

## School Transaction History Response

```yaml
items:
  - transaction_id: "normalized-transaction-reference"
    school_account_id: "school-account-reference"
    student_wallet_id: "wallet-reference"
    student_profile_id: "student-profile-reference"
    transaction_type: "Canteen Purchase"
    amount_minor: 1200
    currency_code: "SAR"
    merchant_name: "Main Canteen"
    pos_terminal_id: "terminal-reference"
    operator_actor_id: "operator-reference"
    item_summary: "Lunch combo"
    transaction_status: "Approved"
    correction_status: "None"
    settlement_status: "Pending"
    review_status: "Not Required"
    occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
page:
  size: 25
  next_cursor: "cursor-reference"
```

## Guardian Transaction History Response

```yaml
items:
  - transaction_id: "guardian-visible-transaction-reference"
    student_profile_id: "linked-student-reference"
    transaction_type: "Canteen Purchase"
    amount_minor: 1200
    currency_code: "SAR"
    merchant_name: "Main Canteen"
    item_or_category_summary: "Lunch combo"
    transaction_status: "Approved"
    correction_summary: null
    occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
page:
  size: 25
  next_cursor: "cursor-reference"
```

## Manual Review Request

```yaml
review_scope: "Purchase"
scope_reference: "purchase-reference"
review_action: "Correct"
review_reason: "Duplicate tap confirmed by POS operator and finance reviewer."
student_wallet_id: "wallet-reference"
resulting_action:
  correction_type: "Reversal"
  amount_minor: 1200
  currency_code: "SAR"
client_request_id: "request-unique-to-caller"
```

## Refund or Reversal Request

```yaml
student_wallet_id: "wallet-reference"
correction_type: "Refund"
original_source_type: "Canteen Purchase"
original_source_reference: "purchase-reference"
amount_minor: 1200
currency_code: "SAR"
reason: "Item returned before reconciliation close."
client_request_id: "request-unique-to-caller"
```

## Correction Response

```yaml
refund_reversal_id: "correction-reference"
student_wallet_id: "wallet-reference"
correction_type: "Refund"
correction_status: "Posted"
original_source_type: "Canteen Purchase"
original_source_reference: "purchase-reference"
amount_minor: 1200
currency_code: "SAR"
wallet_ledger_entry_id: "ledger-entry-reference"
review_reason: "Item returned before reconciliation close."
created_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- School transaction history requires tenant access, enabled
  `wallet.transaction_history`, and `wallet.history.read`.
- Guardian transaction history requires approved active guardian link, linked
  student scope, enabled `wallet.transaction_history`, and
  `wallet.guardian_history.read`.
- Guardian history shows amount, merchant, item or category summary, status,
  and corrections, while hiding staff-only terminal, operator, settlement, and
  internal review assignment details.
- History filters must support student, wallet, guardian link, date range,
  transaction type, merchant, POS terminal, funding source, status, amount, and
  review state without exposing cross-school data.
- Refunds, voids, reversals, holds, releases, restrictions, recoveries, and
  disputes require specific correction or chargeback review permission,
  preserved original evidence, required reason, idempotent request identity,
  resulting ledger entry when applicable, and audit evidence.
- Corrections do not edit or delete original events.
- Corrections after a closed reconciliation period create new review evidence
  and preserve the prior closed reconciliation.
- Chargeback after spend keeps original top-up and spending evidence, restricts
  the wallet, records pending recovery, and requires authorized financial review
  before discretionary spending is restored.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Transaction history capability disabled | Deny history workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks history permission | Deny history read and record required permission |
| Guardian not actively linked | Deny without exposing unrelated wallet existence |
| Correction actor lacks review permission | Deny correction and record required permission |
| Original event missing, cross-school, or hidden | Deny or hold correction without exposing cross-tenant existence |
| Amount exceeds refundable or reversible amount | Reject with correction validation details |
| Attempt to edit original event | Reject because corrections require append-only entries |
| Duplicate client request identity | Return existing correction outcome when request content matches |
| Audit write fails for sensitive outcome | Reject sensitive mutation rather than allowing unaudited correction |
