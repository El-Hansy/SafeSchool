# Contract: Attendance Anomaly Detection

This contract defines anomaly detection runs, anomaly records, assignment,
resolution, dismissal, reopening, and review traceability for Phase 2.

## Capabilities and Permissions

- Required capabilities:
  - `attendance_access.anomaly_detection`
- Common permissions:
  - `attendance_access.anomalies.read`
  - `attendance_access.anomalies.resolve`
  - `attendance_access.attendance.read`
  - `attendance_access.attendance.correct`
  - `attendance_access.scans.read`
  - `attendance_access.audit.read`

Anomaly resolution records reviewer decisions. Attendance changes caused by a
review must use the attendance correction contract and remain separately
auditable.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/anomaly-runs` | Detect anomalies for a date, session, or scan set |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies` | Review anomalies with filters |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies/{anomalyId}` | Read anomaly detail |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies/{anomalyId}/assign` | Assign anomaly to a reviewer |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies/{anomalyId}/resolve` | Resolve anomaly with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies/{anomalyId}/dismiss` | Dismiss anomaly with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/anomalies/{anomalyId}/reopen` | Reopen anomaly after new evidence |

## Run Detection Request

```yaml
attendance_session_id: "attendance-session-reference"
attendance_date: "YYYY-MM-DD"
include_scan_statuses:
  - "Reconciled"
  - "Needs Review"
include_anomaly_types:
  - "Missing Entry"
  - "Missing Exit"
  - "Duplicate Scan"
  - "Invalid Credential"
  - "Out Of Order Scan"
  - "Conflicting Campus State"
  - "Delayed Offline Conflict"
  - "Late Arrival"
  - "Early Exit"
requested_reason: "Daily anomaly review."
client_request_id: "request-unique-to-caller"
```

## Anomaly Record Response

```yaml
attendance_anomaly_id: "anomaly-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
attendance_session_id: "attendance-session-reference"
gate_scan_event_id: "scan-event-reference"
attendance_record_id: "attendance-record-reference"
anomaly_type: "Duplicate Scan"
severity: "Medium"
status: "Assigned"
assigned_to: "reviewer-actor-reference"
detected_at: "YYYY-MM-DDTHH:MM:SSZ"
resolution_reason: null
resolved_by: null
resolved_at: null
evidence:
  scan_event_ids:
    - "scan-event-reference"
  attendance_record_ids:
    - "attendance-record-reference"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Assign Request

```yaml
assigned_to: "reviewer-actor-reference"
review_reason: "Campus access reviewer owns duplicate scan review."
client_request_id: "request-unique-to-caller"
```

## Resolve Request

```yaml
resolution_reason: "Duplicate retry matched original scan; no attendance change required."
related_manual_review_id: "manual-review-reference"
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Anomaly detection requires `attendance_access.anomaly_detection` to be
  enabled.
- Detection runs must be scoped to one school account and must not inspect
  cross-tenant scans or attendance records.
- Supported anomaly types include missing entry, missing exit, duplicate scan,
  invalid credential, out-of-order scan, conflicting campus state, delayed
  offline conflict, late arrival, early exit, and manual-review-required scan.
- Each anomaly must identify affected student, evidence, type, severity, status,
  detection time, and review need.
- Assignment, resolution, dismissal, and reopening require
  `attendance_access.anomalies.resolve`, a reviewer actor, reason where status
  changes, and audit evidence.
- Resolving or dismissing an anomaly must not silently correct attendance.
  Attendance changes must use the attendance correction contract.
- Anomaly list responses must be paginated and support filtering by student,
  session, type, severity, status, assigned reviewer, scan evidence, and
  detection time.
- Re-running detection for the same evidence must update or return existing
  open anomalies rather than creating duplicates for the same issue.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny anomaly workflow and record feature capability reason |
| Actor lacks review permission | Deny assignment or resolution and record required permission |
| Evidence belongs to another tenant | Deny without exposing cross-tenant existence |
| Anomaly already resolved | Treat matching retry as same final state or reject invalid transition |
| Resolution lacks reason | Reject status change with validation details |
| Related attendance correction missing | Resolve anomaly only when no attendance change is needed; otherwise require correction |
| Duplicate detection run | Return existing anomaly outcomes when request identity and evidence match |
| Audit write fails for status change | Reject status change rather than allowing unaudited mutation |
