# Contract: Foundation Decision Review

This contract defines the minimum information required when Phase 0 records or
reviews a foundation decision.

## Purpose

Use this contract when documenting a decision in one of the six Phase 0
foundation areas:

- System Architecture
- Multi-Tenant Architecture
- Identity & Access Model
- NFC & QR Integration
- Event & Audit Logging
- Feature Flag / Tenant Configuration

## Required Record

```yaml
decision_id: "FD-001"
foundation_area: "Multi-Tenant Architecture"
decision_statement: "School Account is the default tenant boundary."
rationale: "Prevents cross-school data exposure and gives later specs a stable tenant model."
affected_entities:
  - School Account
  - Permission Rule
review_owner: "Product and Engineering Leadership"
status: "Accepted"
created_at: "YYYY-MM-DD"
last_reviewed_at: "YYYY-MM-DD"
supersedes: null
```

## Acceptance Rules

- `decision_id` MUST be unique within Phase 0.
- `foundation_area` MUST be one of the six approved Phase 0 areas.
- `decision_statement` MUST be written as a testable rule.
- `rationale` MUST explain why the decision exists.
- `review_owner` MUST identify who can approve changes.
- `status` MUST be Proposed, Accepted, Superseded, or Deprecated.

## Review Outcomes

- **Accepted**: Later specs may reference the decision.
- **Superseded**: Later specs must reference the replacement decision.
- **Deprecated**: Later specs must not introduce new dependency on the decision.
