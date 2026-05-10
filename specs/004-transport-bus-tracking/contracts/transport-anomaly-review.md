# Contract: Transport Anomaly Review

This contract defines transport anomaly detection, assignment, resolution,
dismissal, reopening, manual corrections, and review traceability for Phase 3.

## Capabilities and Permissions

- Required capabilities:
  - `transport.boarding_drop_scans`
  - `transport.live_tracking`
- Common permissions:
  - `transport.anomalies.read`
  - `transport.anomalies.resolve`
  - `transport.scans.read`
  - `transport.trips.read`
  - `transport.eta.read`
  - `transport.notifications.read`
  - `transport.audit.read`

Anomaly review is supporting behavior for scan, tracking, ETA, and notification
workflows. Corrections must be explicit manual review actions and remain
auditable.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/transport/anomaly-runs` | Detect anomalies for a trip, route, date, or evidence set |
| GET | `/api/v1/schools/{schoolAccountId}/transport/anomalies` | Review anomalies with filters |
| GET | `/api/v1/schools/{schoolAccountId}/transport/anomalies/{anomalyId}` | Read anomaly detail |
| POST | `/api/v1/schools/{schoolAccountId}/transport/anomalies/{anomalyId}/assign` | Assign anomaly to a reviewer |
| POST | `/api/v1/schools/{schoolAccountId}/transport/anomalies/{anomalyId}/resolve` | Resolve anomaly with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/transport/anomalies/{anomalyId}/dismiss` | Dismiss anomaly with a reason |
| POST | `/api/v1/schools/{schoolAccountId}/transport/anomalies/{anomalyId}/reopen` | Reopen anomaly after new evidence |
| POST | `/api/v1/schools/{schoolAccountId}/transport/manual-reviews` | Record manual transport review or correction |
| GET | `/api/v1/schools/{schoolAccountId}/transport/manual-reviews/{manualReviewId}` | Read manual review detail |

## Run Detection Request

```yaml
transport_trip_id: "trip-reference"
transport_route_id: "route-reference"
trip_date: "YYYY-MM-DD"
include_anomaly_types:
  - "Missed Boarding"
  - "Wrong Route"
  - "Wrong Stop"
  - "Duplicate Scan"
  - "Invalid Credential"
  - "Out Of Order Scan"
  - "Delayed Offline Conflict"
  - "Route Deviation"
  - "Stale Location"
  - "Delayed Trip"
requested_reason: "Daily transport review."
client_request_id: "request-unique-to-caller"
```

## Anomaly Record Response

```yaml
transport_anomaly_id: "anomaly-reference"
school_account_id: "school-account-reference"
transport_trip_id: "trip-reference"
student_profile_id: "student-profile-reference"
transport_route_id: "route-reference"
route_stop_sequence_id: "route-stop-reference"
source_event_reference: "scan-event-reference"
anomaly_type: "Wrong Stop"
severity: "High"
status: "Assigned"
assigned_to: "reviewer-actor-reference"
detected_at: "YYYY-MM-DDTHH:MM:SSZ"
resolution_reason: null
resolved_by: null
resolved_at: null
evidence:
  scan_event_ids:
    - "scan-event-reference"
  location_update_ids: []
  eta_record_ids: []
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Manual Review Request

```yaml
source_record_type: "Scan"
source_record_reference: "scan-event-reference"
review_action: "Correct"
corrected_status: "Accepted"
review_reason: "Reviewer confirmed student was assigned to emergency replacement stop."
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Anomaly detection requires the related scan, tracking, ETA, or notification
  capability to be enabled.
- Detection runs must be scoped to one school account and must not inspect
  cross-tenant trips, scans, location updates, assignments, or notifications.
- Supported anomaly types include missed boarding, missed drop, wrong route,
  wrong stop, duplicate scan, invalid credential, out-of-order scan, delayed
  offline conflict, route deviation, stale location, delayed trip, and
  manual-review-required events.
- Each anomaly must identify affected student or trip, evidence, type, severity,
  status, detection time, and review need.
- Assignment, resolution, dismissal, reopening, and manual correction require
  `transport.anomalies.resolve`, reviewer actor, reason where status changes,
  and audit evidence.
- Resolving or dismissing an anomaly must not silently change scan, trip, ETA,
  or notification outcomes. Such changes must use Manual Transport Review.
- Anomaly list responses must be paginated and support filtering by student,
  trip, route, stop, type, severity, status, assigned reviewer, source evidence,
  and detection time.
- Re-running detection for the same evidence must update or return existing
  open anomalies rather than creating duplicates for the same issue.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Related capability disabled | Deny anomaly workflow and record feature capability reason |
| Actor lacks review permission | Deny assignment, resolution, or correction and record required permission |
| Evidence belongs to another tenant | Deny without exposing cross-tenant existence |
| Anomaly already resolved | Treat matching retry as same final state or reject invalid transition |
| Resolution lacks reason | Reject status change with validation details |
| Correction would create attendance or campus access | Reject as out of Phase 3 scope |
| Duplicate detection run | Return existing anomaly outcomes when request identity and evidence match |
| Audit write fails for status change | Reject status change rather than allowing unaudited mutation |
