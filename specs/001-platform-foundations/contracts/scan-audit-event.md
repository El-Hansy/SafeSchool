# Contract: Scan and Audit Event Evidence

This contract defines the minimum evidence expected for NFC/QR scan events and
audit events used by later attendance, access, transport, and administrative
review specs.

## Scan Event Record

```yaml
scan_event_id: "SE-001"
school_account_id: "school-account-reference"
capture_method: "NFC"
capture_source: "Gate Device"
identity_evidence_id: "IE-001"
captured_at: "YYYY-MM-DDTHH:MM:SSZ"
received_at: "YYYY-MM-DDTHH:MM:SSZ"
offline_indicator: true
sync_status: "Pending"
duplicate_reference: null
review_outcome: "Needs Review"
```

## Audit Event Record

```yaml
audit_event_id: "AE-001"
school_account_id: "school-account-reference"
event_category: "Scan Reconciliation"
actor_category_key: "Reviewer"
subject_reference: "SE-001"
event_time: "YYYY-MM-DDTHH:MM:SSZ"
reason: "Offline scan reconciled after connectivity returned."
review_status: "Recorded"
```

## Acceptance Rules

- Scan events MUST include school account, capture method, source, capture time,
  received time, sync status, and review outcome.
- QR fallback events MUST satisfy the same evidence requirements as NFC events.
- Offline scan events MUST preserve enough source and timing evidence for later
  duplicate detection and conflict review.
- Audit events MUST identify who acted, which school account was affected when
  tenant-scoped, what changed, when it happened, and why it matters.
- Later specs MUST define their business outcome separately. Phase 0 does not
  decide attendance, access, transport, or payment effects.
