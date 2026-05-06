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

## Status Examples

```yaml
configuration_change_id: "TC-APPROVED-001"
school_account_id: "school-alpha"
change_type: "Feature Availability"
capability_key: "attendance.gate-scan"
requested_by: "School Administrator"
approved_by: "Platform Owner"
change_reason: "Enable gate scan pilot after operational readiness review."
previous_value_summary: "Disabled"
new_value_summary: "Enabled"
effective_at: "2026-05-06T09:00:00Z"
review_status: "Approved"
audit_event_id: "AE-APPROVED-001"
```

```yaml
configuration_change_id: "TC-REJECTED-001"
school_account_id: "school-alpha"
change_type: "Feature Availability"
capability_key: "wallet.payments"
requested_by: "School Administrator"
approved_by: "Reviewer"
change_reason: "Rejected because payment controls are not yet reviewed."
previous_value_summary: "Disabled"
new_value_summary: "Enabled"
effective_at: "2026-05-06T09:15:00Z"
review_status: "Rejected"
audit_event_id: "AE-REJECTED-001"
```

```yaml
configuration_change_id: "TC-APPLIED-001"
school_account_id: "school-alpha"
change_type: "Feature Availability"
capability_key: "identity.guardian-linking"
requested_by: "School Administrator"
approved_by: "Platform Owner"
change_reason: "Apply approved guardian linking availability."
previous_value_summary: "Proposed"
new_value_summary: "Enabled"
effective_at: "2026-05-06T09:30:00Z"
review_status: "Applied"
audit_event_id: "AE-APPLIED-001"
```

```yaml
configuration_change_id: "TC-ROLLED-BACK-001"
school_account_id: "school-alpha"
change_type: "Feature Availability"
capability_key: "transport.boarding-scan"
requested_by: "School Administrator"
approved_by: "Platform Owner"
change_reason: "Roll back after readiness issue found during review."
previous_value_summary: "Enabled"
new_value_summary: "Suspended"
effective_at: "2026-05-06T09:45:00Z"
review_status: "Rolled Back"
audit_event_id: "AE-ROLLED-BACK-001"
```
