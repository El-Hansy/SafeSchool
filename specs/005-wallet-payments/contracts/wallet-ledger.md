# Contract: Wallet Ledger

This contract defines student wallet lifecycle, append-only ledger posting,
balance traceability, wallet restrictions, and wallet audit behavior for Phase
4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.ledger`
- Common permissions:
  - `wallet.wallets.read`
  - `wallet.wallets.manage`
  - `wallet.ledger.read`
  - `wallet.ledger.post`
  - `wallet.corrections.manage`
  - `wallet.audit.read`

Wallet and ledger commands must be tenant-scoped, idempotent where they mutate
financial state, and auditable. Approved ledger entries must not be edited or
deleted directly.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets` | Create or activate one student wallet |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/bulk-activate` | Create or activate wallets for eligible students |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets` | List wallets with student, status, balance, and review filters |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}` | Read wallet detail |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/restrict` | Restrict wallet spending with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/restore` | Restore wallet spending after authorized review |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/suspend` | Suspend a wallet with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/close` | Close a wallet when no active holds remain |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/ledger-entries` | Read paginated ledger entries |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/ledger-entries/{ledgerEntryId}` | Read one ledger entry |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/trace` | Trace wallet balance to ledger, top-up, purchase, correction, reconciliation, anomaly, review, and audit evidence |

## Wallet Create Request

```yaml
student_profile_id: "student-profile-reference"
wallet_code: "school-unique-wallet-code"
currency_code: "SAR"
activation_reason: "Initial wallet activation."
client_request_id: "request-unique-to-caller"
```

## Bulk Activate Request

```yaml
student_profile_ids:
  - "student-profile-reference-1"
  - "student-profile-reference-2"
currency_code: "SAR"
activation_reason: "New term wallet rollout."
client_request_id: "request-unique-to-caller"
```

## Wallet Response

```yaml
student_wallet_id: "wallet-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
wallet_code: "school-unique-wallet-code"
currency_code: "SAR"
available_balance_minor: 2500
pending_balance_minor: 0
held_balance_minor: 0
settled_balance_minor: 2000
pending_recovery_minor: 0
wallet_status: "Active"
restriction_reason: null
current_rule_setting_id: "wallet-rule-setting-reference"
latest_ledger_entry_id: "ledger-entry-reference"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Ledger Entry Response

```yaml
wallet_ledger_entry_id: "ledger-entry-reference"
school_account_id: "school-account-reference"
student_wallet_id: "wallet-reference"
entry_type: "Debit"
entry_status: "Approved"
amount_minor: 750
currency_code: "SAR"
balance_available_after_minor: 1750
balance_pending_after_minor: 0
balance_held_after_minor: 0
source_type: "Canteen Purchase"
source_reference: "purchase-reference"
original_entry_id: null
posted_at: "YYYY-MM-DDTHH:MM:SSZ"
review_reason: null
```

## Wallet Restriction Request

```yaml
restriction_reason: "Chargeback after spend requires pending recovery review."
pending_recovery_minor: 1500
related_source_type: "Payment Confirmation"
related_source_reference: "payment-confirmation-reference"
client_request_id: "request-unique-to-caller"
```

## Wallet Restore Request

```yaml
review_reason: "Recovery resolved and discretionary spending restored."
manual_wallet_review_id: "manual-review-reference"
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Every endpoint requires tenant access and enabled `wallet.ledger` unless the
  endpoint is only reading audit evidence allowed to platform-level reviewers.
- Wallet creation and lifecycle mutation require `wallet.wallets.manage`,
  active student profile, school-account scope, valid currency, idempotent
  `client_request_id`, and audit evidence.
- Ledger entry reads require `wallet.ledger.read` or a guardian-scoped history
  route that filters to linked students.
- Balance fields must be derived from approved ledger entries; API callers
  cannot patch balances directly.
- Approved ledger entries are append-only. Corrections require a new reversal,
  refund, hold, release, recovery, or manual adjustment entry with reason and
  audit evidence.
- Wallet restriction prevents further discretionary spending until authorized
  financial review restores it.
- Wallet close is rejected when pending, held, recovery, dispute, or
  reconciliation records remain active.
- List responses must be paginated and support filters for student, status,
  balance state, pending recovery, latest activity time, anomaly status, and
  review status.
- Wallet trace must expose links to related ledger, top-up, payment, purchase,
  correction, settlement, anomaly, manual review, rule, and audit records when
  the caller is authorized to see them.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny wallet workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks wallet permission | Deny mutation and record required permission |
| Actor lacks ledger read permission | Deny ledger detail and record required permission |
| Student inactive or cross-school | Reject without exposing cross-tenant existence |
| Wallet already active for student | Return existing wallet for idempotent activation or reject conflicting request |
| Invalid currency or amount precision | Reject with field-level validation details |
| Direct balance mutation requested | Reject because balances are ledger-derived |
| Wallet has unresolved holds | Reject close or restore until review resolves holds |
| Duplicate client request identity | Return existing outcome when request content matches |
| Audit write fails for sensitive outcome | Reject mutation rather than allowing unaudited financial change |
