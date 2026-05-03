# Contract: Boarding/Drop Scan

This contract defines online scan capture, offline scan sync, scan decisions,
needs-review anomaly handling, and scan traceability for Phase 3.

## Capabilities and Permissions

- Required capabilities:
  - `transport.boarding_drop_scans`
- Common permissions:
  - `transport.scans.record`
  - `transport.scans.sync`
  - `transport.scans.read`
  - `transport.trips.read`
  - `transport.anomalies.read`
  - `transport.audit.read`

Boarding/drop scan commands must be idempotent and must not create duplicate
transport outcomes when a caller retries the same physical scan.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/transport/scan-events` | Record an online boarding or drop scan |
| POST | `/api/v1/schools/{schoolAccountId}/transport/scan-events/sync` | Submit an offline transport scan batch |
| GET | `/api/v1/schools/{schoolAccountId}/transport/scan-events` | Review scan events with filters |
| GET | `/api/v1/schools/{schoolAccountId}/transport/scan-events/{scanEventId}` | Read scan event details |
| GET | `/api/v1/schools/{schoolAccountId}/transport/scan-events/{scanEventId}/trace` | Trace scan to trip, assignment, ETA, notification, anomaly, review, and audit outcomes |

## Online Scan Request

```yaml
client_scan_id: "scan-unique-to-device"
transport_trip_id: "trip-reference"
route_stop_sequence_id: "route-stop-reference"
credential_type: "NFC Card"
credential_reference: "non-secret-card-reference"
scan_method: "NFC"
scan_direction: "Boarding"
actor_reference: "attendant-actor-reference"
tracking_device_reference: "registered-mobile-device-reference"
local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
credential_snapshot_version: "snapshot-version-reference"
```

## Offline Sync Request

```yaml
client_batch_id: "batch-unique-to-device"
transport_trip_id: "trip-reference"
tracking_device_reference: "registered-mobile-device-reference"
actor_reference: "attendant-actor-reference"
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
scans:
  - client_scan_id: "scan-unique-to-device"
    route_stop_sequence_id: "route-stop-reference"
    credential_type: "NFC Card"
    credential_reference: "non-secret-card-reference"
    scan_method: "NFC"
    scan_direction: "Boarding"
    local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
    credential_snapshot_version: "snapshot-version-reference"
```

## Scan Event Response

```yaml
boarding_drop_scan_event_id: "scan-event-reference"
school_account_id: "school-account-reference"
transport_trip_id: "trip-reference"
transport_route_id: "route-reference"
route_stop_sequence_id: "route-stop-reference"
student_profile_id: "student-profile-reference"
student_transport_assignment_id: "assignment-reference"
identity_credential_id: "credential-reference"
scan_direction: "Boarding"
local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
received_at: "YYYY-MM-DDTHH:MM:SSZ"
school_effective_time: "YYYY-MM-DDTHH:MM:SSZ"
offline_captured: false
sync_status: "Reconciled"
scan_decision: "Accepted"
decision_reason: "Credential active, trip active, and assignment matched."
transport_status_after: "Onboard"
review_status: "Not Required"
```

## Offline Sync Response

```yaml
offline_transport_scan_sync_batch_id: "sync-batch-reference"
school_account_id: "school-account-reference"
client_batch_id: "batch-unique-to-device"
batch_status: "Reconciled"
accepted_count: 20
needs_review_count: 1
duplicate_count: 1
rejected_count: 0
items:
  - client_scan_id: "scan-unique-to-device"
    boarding_drop_scan_event_id: "scan-event-reference"
    sync_status: "Reconciled"
    scan_decision: "Accepted"
    decision_reason: "Credential active, trip active, and assignment matched."
```

## Acceptance Rules

- Scan capture requires `transport.boarding_drop_scans` to be enabled.
- Recording scans requires tenant access, active trip, authorized trip staff or
  device source, active credential evidence, and `transport.scans.record` or
  authorized mobile source.
- Offline sync requires `transport.scans.sync`, active or reviewable trip
  context, authorized device, and caller-stable `client_batch_id` and
  `client_scan_id` values.
- Expired, suspended, revoked, replaced, unknown, duplicated, and cross-school
  credentials are denied or flagged without normal boarding/drop status.
- Students without active assignment or with route/stop mismatch are recorded as
  needs-review anomalies and normal status and guardian notification are
  withheld until reviewer approval.
- Repeated submissions with the same client scan identity return the same scan
  outcome or identify a duplicate without creating a second physical event.
- Scan event list responses must be paginated and support filtering by student,
  credential reference, trip, route, stop, direction, decision, sync status,
  anomaly status, and time.
- Scan trace must show related trip, route, stop, assignment, notification, ETA,
  anomaly, manual review, and audit evidence when those records exist.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny scan workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks scan permission | Deny and record required permission |
| Trip inactive or completed | Reject or route scan to review with status reason |
| Device not authorized for trip | Reject scan and record device authorization reason |
| Credential invalid or cross-school | Deny or flag scan without exposing cross-tenant existence |
| Assignment missing or mismatched | Record needs-review anomaly and withhold normal status |
| Duplicate client scan identity | Return existing outcome or mark duplicate without duplicate status |
| Device clock drift detected | Accept with needs-review signal or route to anomaly according to rules |
| Audit write fails for sensitive outcome | Reject sensitive mutation rather than allowing unaudited change |
