# Contract: Spending Limits

This contract defines guardian and school spending limits, strictest-rule
evaluation, rule versioning, purchase denial reasons, and spending limit
traceability for Phase 4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.spending_limits`
  - `wallet.canteen_pos` when limits are enforced against purchases
- Common permissions:
  - `wallet.limits.read`
  - `wallet.limits.manage`
  - `wallet.guardian_history.read`
  - `wallet.audit.read`

Spending limits are tenant-owned, versioned, and evaluated server-side. UI
feature gates cannot replace backend rule enforcement.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits` | Create a school-owned spending limit |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits` | List school spending limits with filters |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{spendingLimitId}` | Read spending limit detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{spendingLimitId}` | Update a draft or suspended spending limit |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{spendingLimitId}/activate` | Activate a spending limit version |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{spendingLimitId}/suspend` | Suspend a spending limit with reason |
| POST | `/api/v1/guardians/me/students/{studentProfileId}/wallet/spending-limits` | Create a guardian-owned limit for a linked student |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/wallet/spending-limits` | List guardian-visible limits for a linked student |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/student-wallets/{walletId}/spending-limits/effective` | Read effective active rules and strictest-rule result |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/spending-limits/{spendingLimitId}/trace` | Trace limit version to purchases, denials, reviews, and audit evidence |

## Spending Limit Request

```yaml
student_wallet_id: "wallet-reference"
limit_owner_type: "School"
limit_type: "Daily Amount"
amount_limit_minor: 2500
currency_code: "SAR"
merchant_reference: null
category_code: null
time_window: null
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: "YYYY-MM-DDTHH:MM:SSZ"
change_reason: "Daily canteen budget for lower school."
client_request_id: "request-unique-to-caller"
```

## Category Restriction Request

```yaml
student_wallet_id: "wallet-reference"
limit_owner_type: "Guardian"
limit_type: "Category"
category_code: "snack"
restriction_mode: "Deny"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
change_reason: "Guardian disabled snack purchases."
client_request_id: "request-unique-to-caller"
```

## Spending Limit Response

```yaml
spending_limit_id: "spending-limit-reference"
school_account_id: "school-account-reference"
student_wallet_id: "wallet-reference"
limit_owner_type: "School"
limit_type: "Daily Amount"
amount_limit_minor: 2500
currency_code: "SAR"
precedence_rank: 10
limit_status: "Active"
rule_version: "limit-version-reference"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Effective Rule Response

```yaml
student_wallet_id: "wallet-reference"
school_account_id: "school-account-reference"
evaluated_at: "YYYY-MM-DDTHH:MM:SSZ"
effective_rules:
  - spending_limit_id: "school-limit-reference"
    limit_type: "Daily Amount"
    remaining_amount_minor: 1300
    rule_version: "limit-version-reference"
  - spending_limit_id: "guardian-limit-reference"
    limit_type: "Category"
    category_code: "snack"
    restriction_mode: "Deny"
    rule_version: "limit-version-reference"
strictest_rule_summary: "Guardian category restriction denies snack purchases; school daily remaining amount is 1300."
```

## Acceptance Rules

- School limit management requires tenant access, enabled
  `wallet.spending_limits`, `wallet.limits.manage`, idempotent request identity,
  valid values, and audit evidence.
- Guardian limit management requires an approved active guardian link, linked
  student scope, enabled `wallet.spending_limits`, guardian limit permission,
  idempotent request identity, valid values, and audit evidence.
- Limits may be scoped by amount, time period, per-purchase amount, merchant,
  item category, time window, active date range, or wallet restriction.
- Purchase authorization evaluates all active applicable school, guardian, and
  review limits and enforces the strictest applicable rule.
- Guardian limits cannot loosen stricter school or financial review
  restrictions.
- A spending limit change affects eligible future purchases within 1 minute of
  becoming active during review testing.
- Historical purchases keep the rule version and decision reason used at the
  time of authorization.
- List responses must be paginated and support filters for student, owner,
  limit type, merchant, category, active date, status, and latest update time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny spending limit workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks limit permission | Deny mutation or read and record required permission |
| Guardian not actively linked | Deny guardian limit without exposing unrelated wallet existence |
| Wallet inactive, closed, or cross-school | Reject limit mutation |
| Invalid amount, currency, date range, merchant, or category | Reject with field-level validation details |
| Limit attempts to loosen stricter review restriction | Reject and show stricter-rule reason |
| Duplicate client request identity | Return existing outcome when request content matches |
| Purchase violates strictest rule | Deny or hold purchase and store rule snapshot reason |
| Audit write fails for sensitive outcome | Reject mutation rather than allowing unaudited limit change |
