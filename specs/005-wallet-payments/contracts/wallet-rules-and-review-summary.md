# Contract: Wallet Rules and Review Summary

This contract defines school-account wallet rule setting management and
permission-scoped review summary access for Phase 4 wallet workflows.

## Capabilities and Permissions

- Required capabilities:
  - None beyond tenant access and permissions for rule management.
  - Related workflow capabilities apply when rule values are enforced or when
    summary filters expose workflow-specific data.
- Common permissions:
  - `wallet.rules.read`
  - `wallet.rules.manage`
  - `wallet.history.read`
  - `wallet.reconciliation.read`
  - `wallet.anomalies.read`
  - `wallet.guardian_history.read`
  - `wallet.audit.read`

Rule settings are tenant-owned and versioned. Review summaries are read models
computed from tenant-owned wallet evidence and must not expose records outside
the caller's authorized school account or guardian link scope.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/wallet/rule-settings/current` | Read active Phase 4 wallet rule settings |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/rule-settings` | Create a draft wallet rule setting |
| PATCH | `/api/v1/schools/{schoolAccountId}/wallet/rule-settings/{ruleSettingId}` | Update a draft or suspended rule setting |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/rule-settings/{ruleSettingId}/activate` | Activate a rule setting and supersede prior active version |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/rule-settings/{ruleSettingId}/suspend` | Suspend an active rule setting with a reason |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/review-summaries` | Read paginated wallet review summaries by scope and status filters |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/review-summaries/{summaryScope}/{scopeReference}` | Read one wallet review summary with trace links |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/wallet/review-summary` | Read guardian-visible linked-student wallet summary |

## Rule Setting Request

```yaml
currency_code: "SAR"
top_up_minimum_minor: 500
top_up_maximum_minor: 50000
cashier_adjustment_threshold_minor: 10000
offline_pos_enabled: true
offline_student_reserve_limit_minor: 2000
offline_terminal_reserve_limit_minor: 50000
spending_limit_precedence_policy: "Strictest Applicable"
refund_window_policy: "SchoolConfigured"
chargeback_handling_policy: "Restrict Wallet And Require Review"
anomaly_detection_policy:
  enabled_types:
    - "Duplicate Top-Up Confirmation"
    - "Duplicate POS Purchase"
    - "Negative Available Balance"
    - "Offline Overspend"
    - "Invalid Credential Purchase"
    - "Unmatched Settlement"
    - "Chargeback After Spend"
duplicate_retry_handling_policy: "Idempotent Client Identifiers"
reconciliation_review_threshold_minor: 1
financial_record_retention_policy:
  mode: "School Configured"
  detailed_record_retention_days: 365
  preserve_active_review_holds: true
change_reason: "Initial wallet policy for school year."
client_request_id: "request-unique-to-caller"
```

## Rule Setting Response

```yaml
wallet_rule_setting_id: "rule-setting-reference"
school_account_id: "school-account-reference"
currency_code: "SAR"
offline_pos_enabled: true
offline_student_reserve_limit_minor: 2000
offline_terminal_reserve_limit_minor: 50000
chargeback_handling_policy: "Restrict Wallet And Require Review"
financial_record_retention_policy:
  mode: "School Configured"
  detailed_record_retention_days: 365
  preserve_active_review_holds: true
rule_setting_status: "Active"
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Review Summary Response

```yaml
summary_scope: "Student Wallet"
scope_reference: "wallet-reference"
school_account_id: "school-account-reference"
student_wallet_id: "wallet-reference"
wallet_status_counts:
  active: 1
  restricted: 0
transaction_status_counts:
  credited: 4
  approved_purchases: 12
  refunds: 1
  chargebacks: 0
spending_limit_status_counts:
  active: 2
  suspended: 0
settlement_status_counts:
  pending: 2
  closed: 1
anomaly_status_counts:
  open: 0
  resolved: 1
review_status_counts:
  needs_review: 0
  closed: 1
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
trace_reference:
  wallet_trace: "wallet-reference"
  ledger_trace: "ledger-filter-reference"
```

## Guardian Review Summary Response

```yaml
student_profile_id: "linked-student-reference"
wallet_status: "Active"
available_balance_minor: 3800
currency_code: "SAR"
recent_transaction_counts:
  top_ups: 2
  purchases: 8
  refunds_or_corrections: 1
latest_visible_activity_at: "YYYY-MM-DDTHH:MM:SSZ"
open_guardian_visible_issue_count: 0
```

## Acceptance Rules

- Rule reads require tenant access and `wallet.rules.read` or
  `wallet.rules.manage`.
- Rule mutation requires tenant access, `wallet.rules.manage`, validation of
  every policy value, idempotent `client_request_id`, and audit evidence.
- Activating a rule setting supersedes the prior active version without
  changing historical event evidence evaluated under the prior version.
- Offline POS can be enabled only when both per-student and per-terminal
  reserve limits are valid.
- Chargeback handling policy must preserve the clarified behavior: restrict the
  wallet, record pending recovery, and require financial review before further
  discretionary spending after chargeback following spend.
- Detailed financial record retention is school-configured with no Phase 4
  platform-wide minimum, while records under active review, dispute, recovery,
  or reconciliation hold remain preserved.
- Review-summary reads require tenant access and the permission associated with
  the requested summary scope.
- Review-summary list responses must be paginated and support filters for
  student wallet, guardian, merchant, POS terminal, transaction type,
  transaction status, spending limit status, settlement status, anomaly status,
  review status, and latest evidence time.
- Guardian-scoped summaries must be limited to linked-student wallet evidence
  and must not include staff-only terminal, operator, settlement, or internal
  review assignment details.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks rule permission | Deny rule read or mutation and record required permission |
| Actor lacks summary permission | Deny summary read and record required permission |
| Invalid rule value | Reject with field-level validation details |
| Offline POS enabled without both reserve limits | Reject activation with offline reserve validation reason |
| Chargeback policy tries to skip review after spend | Reject activation with financial integrity reason |
| Retention rule would delete active holds | Reject activation with retention hold reason |
| Summary filter references another tenant | Deny without exposing cross-tenant existence |
| Guardian requests unrelated summary | Suppress summary and record guardian scope reason |
| Duplicate client request identity | Return existing outcome when request content matches |
| Audit write fails for rule mutation | Reject mutation rather than allowing unaudited change |
