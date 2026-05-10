# Contract: Gate Scan Flow

This contract defines gate configuration, scan point authorization, online scan
capture, offline scan sync, campus access decisions, and scan traceability for
Phase 2.

## Capabilities and Permissions

- Required capabilities:
  - `attendance_access.gate_scanning`
- Common permissions:
  - `attendance_access.gates.read`
  - `attendance_access.gates.manage`
  - `attendance_access.scan_points.manage`
  - `attendance_access.scans.record`
  - `attendance_access.scans.sync`
  - `attendance_access.scans.read`
  - `attendance_access.audit.read`

Gate scan commands must be idempotent and must not create duplicate scan or
attendance outcomes when a caller retries the same physical scan.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/gates` | List gates and scan readiness |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/gates` | Create a gate |
| PATCH | `/api/v1/schools/{schoolAccountId}/attendance-access/gates/{gateId}` | Update gate details or status |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/gates/{gateId}/scan-points` | Register a scan point |
| PATCH | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-points/{scanPointId}` | Update scan point details or status |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-events` | Record an online gate scan |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-events/sync` | Submit an offline scan batch |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-events` | Review scan events with filters |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-events/{scanEventId}` | Read scan event details |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/scan-events/{scanEventId}/trace` | Trace scan to access, attendance, notification, anomaly, and audit outcomes |

## Create Gate Request

```yaml
gate_name: "Main Entrance"
campus_reference: "north-campus"
allowed_directions:
  - "Entry"
  - "Exit"
timezone: "Asia/Riyadh"
gate_status: "Active"
client_request_id: "request-unique-to-caller"
```

## Register Scan Point Request

```yaml
scan_point_name: "Main Entrance Staff Device 1"
scan_point_type: "Staff Device"
device_reference: "registered-device-reference"
assigned_actor_reference: "staff-actor-reference"
allowed_directions:
  - "Entry"
  - "Exit"
offline_allowed: true
client_request_id: "request-unique-to-caller"
```

## Online Scan Request

```yaml
client_scan_id: "scan-unique-to-device"
credential_type: "NFC Card"
credential_reference: "non-secret-card-reference"
scan_method: "NFC"
scan_direction: "Entry"
gate_id: "gate-reference"
scan_point_id: "scan-point-reference"
actor_reference: "staff-actor-reference"
device_reference: "registered-device-reference"
local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
credential_snapshot_version: "snapshot-version-reference"
```

## Offline Sync Request

```yaml
client_batch_id: "batch-unique-to-device"
scan_point_id: "scan-point-reference"
device_reference: "registered-device-reference"
actor_reference: "staff-actor-reference"
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
scans:
  - client_scan_id: "scan-unique-to-device"
    credential_type: "NFC Card"
    credential_reference: "non-secret-card-reference"
    scan_method: "NFC"
    scan_direction: "Entry"
    local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
    credential_snapshot_version: "snapshot-version-reference"
```

## Scan Event Response

```yaml
gate_scan_event_id: "scan-event-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
identity_credential_id: "credential-reference"
scan_direction: "Entry"
gate_id: "gate-reference"
scan_point_id: "scan-point-reference"
local_scan_time: "YYYY-MM-DDTHH:MM:SSZ"
received_at: "YYYY-MM-DDTHH:MM:SSZ"
school_effective_time: "YYYY-MM-DDTHH:MM:SSZ"
offline_captured: false
sync_status: "Reconciled"
scan_decision: "Allowed"
decision_reason: "Credential active and scan point authorized."
campus_state_after: "On Campus"
```

## Offline Sync Response

```yaml
offline_scan_sync_batch_id: "sync-batch-reference"
school_account_id: "school-account-reference"
client_batch_id: "batch-unique-to-device"
batch_status: "Reconciled"
accepted_count: 20
duplicate_count: 1
rejected_count: 0
items:
  - client_scan_id: "scan-unique-to-device"
    gate_scan_event_id: "scan-event-reference"
    sync_status: "Reconciled"
    scan_decision: "Allowed"
    decision_reason: "Credential active and scan point authorized."
```

## Acceptance Rules

- Gate scan capture requires `attendance_access.gate_scanning` to be enabled.
- Gate and scan point management requires matching gate management permission.
- Recording scans requires tenant access, an active gate, an active scan point,
  allowed direction, credential evidence from Phase 1, and
  `attendance_access.scans.record` or an authorized device source.
- Offline sync requires `attendance_access.scans.sync`, an authorized scan
  point, and caller-stable `client_batch_id` and `client_scan_id` values.
- Expired, suspended, revoked, replaced, unknown, duplicated, and cross-school
  credentials must be denied or flagged without creating normal attendance.
- Repeated submissions with the same client scan identity must return the same
  scan outcome or identify a duplicate without creating a second physical scan.
- Scan event list responses must be paginated and support stable filtering by
  student, credential reference, gate, scan point, direction, decision, sync
  status, and time.
- Scan trace must show related campus access decision, attendance record,
  notification record, anomaly record, manual review, and audit evidence when
  those records exist.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny scan workflow, record feature capability reason, and emit audit evidence |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks scan permission | Deny and record required permission |
| Gate or scan point inactive | Reject or route scan to review with status reason |
| Direction not allowed | Reject scan and record direction mismatch |
| Credential invalid or cross-school | Deny or flag scan without exposing cross-tenant existence |
| Duplicate client scan identity | Return existing outcome or mark duplicate without duplicate attendance |
| Device clock drift detected | Accept with needs-review signal or route to anomaly according to school rules |
| Audit write fails for sensitive outcome | Reject sensitive mutation rather than allowing unaudited change |
