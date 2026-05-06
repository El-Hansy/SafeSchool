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

## Concrete Examples

```yaml
decision_id: "FD-001"
foundation_area: "System Architecture"
decision_statement: "Phase 0 is a documentation and governance package and does not create runtime product behavior."
rationale: "Later specs need a stable foundation without prematurely introducing source code or infrastructure."
affected_entities:
  - Foundation Decision
  - Audit Event
review_owner: "Product and Engineering Leadership"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```

```yaml
decision_id: "FD-002"
foundation_area: "Multi-Tenant Architecture"
decision_statement: "School Account is the default tenant boundary for tenant-owned records and actions."
rationale: "Prevents cross-school exposure while allowing later internal campus scoping."
affected_entities:
  - School Account
  - Permission Rule
  - Audit Event
review_owner: "Security Reviewer"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```

```yaml
decision_id: "FD-003"
foundation_area: "Identity & Access Model"
decision_statement: "Later specs must use the canonical actor categories before adding detailed roles."
rationale: "Stable actor vocabulary keeps access reviews consistent across phases."
affected_entities:
  - Actor Category
  - Permission Rule
review_owner: "Security Reviewer"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```

```yaml
decision_id: "FD-004"
foundation_area: "NFC & QR Integration"
decision_statement: "NFC and QR scan events must preserve equivalent identity, tenant, timing, sync, duplicate, conflict, and review evidence."
rationale: "Scan continuity depends on evidence quality before later attendance, access, or transport outcomes are defined."
affected_entities:
  - Identity Evidence
  - Scan Event
  - Audit Event
review_owner: "Operations Reviewer"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```

```yaml
decision_id: "FD-005"
foundation_area: "Event & Audit Logging"
decision_statement: "Sensitive identity, access, configuration, feature availability, scan, reconciliation, and administrative review actions must create audit evidence."
rationale: "Auditability is required for platform safety and later review."
affected_entities:
  - Audit Event
  - Configuration Change
review_owner: "Compliance Reviewer"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```

```yaml
decision_id: "FD-006"
foundation_area: "Feature Flag / Tenant Configuration"
decision_statement: "Feature capabilities are tenant-scoped records changed through traceable configuration changes."
rationale: "Every later feature must respect school-specific capability availability."
affected_entities:
  - Feature Capability
  - Configuration Change
  - Audit Event
review_owner: "Product and Security Reviewers"
status: "Accepted"
created_at: "2026-05-03"
last_reviewed_at: "2026-05-06"
supersedes: null
```
