# Contract: Tenant Configuration Change

This contract defines the minimum information required to request, approve, or
review a tenant configuration or feature capability change.

## Required Record

```yaml
configuration_change_id: "TC-001"
school_account_id: "school-account-reference"
change_type: "Feature Availability"
capability_key: "attendance.gate-scan"
requested_by: "School Administrator"
approved_by: "Platform Owner"
change_reason: "Enable capability for a school account after review."
previous_value_summary: "Disabled"
new_value_summary: "Enabled"
effective_at: "YYYY-MM-DDTHH:MM:SSZ"
review_status: "Approved"
audit_event_id: "AE-001"
```

## Acceptance Rules

- `school_account_id` MUST be present for tenant-scoped changes.
- `change_type` MUST identify the business area affected.
- Feature capability changes MUST include `capability_key`.
- `requested_by`, `approved_by`, `change_reason`, and `effective_at` MUST be
  present before a change is applied.
- `review_status` MUST be Proposed, Approved, Rejected, Applied, or Rolled Back.
- Approved, applied, rejected, or rolled back changes MUST reference an Audit
  Event.

## Later Spec Responsibilities

Later feature specs that depend on tenant configuration MUST define:

- The capability key they introduce or require.
- The school account boundary affected.
- The actor categories allowed to request and approve changes.
- The user-facing behavior when the capability is unavailable.
