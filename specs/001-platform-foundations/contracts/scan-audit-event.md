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

## Evidence Examples

```yaml
scan_event_id: "SE-OFFLINE-NFC-001"
school_account_id: "school-alpha"
capture_method: "NFC"
capture_source: "Gate Device A"
identity_evidence_id: "IE-STUDENT-CARD-001"
captured_at: "2026-05-06T06:30:00Z"
received_at: "2026-05-06T06:45:00Z"
offline_indicator: true
sync_status: "Synced"
duplicate_reference: null
review_outcome: "Accepted"
```

```yaml
scan_event_id: "SE-QR-FALLBACK-001"
school_account_id: "school-alpha"
capture_method: "QR"
capture_source: "Gate Device B"
identity_evidence_id: "IE-STUDENT-QR-001"
captured_at: "2026-05-06T06:35:00Z"
received_at: "2026-05-06T06:35:05Z"
offline_indicator: false
sync_status: "Synced"
duplicate_reference: null
review_outcome: "Accepted"
```

```yaml
scan_event_id: "SE-DUPLICATE-001"
school_account_id: "school-alpha"
capture_method: "NFC"
capture_source: "Gate Device A"
identity_evidence_id: "IE-STUDENT-CARD-001"
captured_at: "2026-05-06T06:30:02Z"
received_at: "2026-05-06T06:45:02Z"
offline_indicator: true
sync_status: "Duplicate"
duplicate_reference: "SE-OFFLINE-NFC-001"
review_outcome: "Rejected"
```

```yaml
scan_event_id: "SE-CONFLICT-001"
school_account_id: "school-alpha"
capture_method: "QR"
capture_source: "Bus Device 3"
identity_evidence_id: "IE-STUDENT-QR-002"
captured_at: "2026-05-06T12:00:00Z"
received_at: "2026-05-06T12:05:00Z"
offline_indicator: true
sync_status: "Conflict"
duplicate_reference: null
review_outcome: "Needs Review"
```

```yaml
audit_event_id: "AE-CONFLICT-REVIEW-001"
school_account_id: "school-alpha"
event_category: "Scan Reconciliation"
actor_category_key: "Reviewer"
subject_reference: "SE-CONFLICT-001"
event_time: "2026-05-06T12:10:00Z"
reason: "QR fallback scan conflicts with expected school account assignment."
review_status: "Escalated"
```
