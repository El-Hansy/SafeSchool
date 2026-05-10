# Contract: ETA Calculation

This contract defines ETA calculation, ETA freshness, confidence state,
guardian-facing ETA visibility, and stale-progress behavior for Phase 3.

## Capabilities and Permissions

- Required capabilities:
  - `transport.eta_calculation`
  - `transport.live_tracking`
- Common permissions:
  - `transport.eta.read`
  - `transport.eta.calculate`
  - `transport.tracking.read`
  - `transport.guardian_visibility.read`
  - `transport.audit.read`

ETA records are calculated only from approved route sequences, active trip
state, non-overlapping bus/device assignment, and sufficiently current trusted
trip progress.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/eta/recalculate` | Recalculate ETA records for an active trip |
| GET | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/eta` | Read staff-visible ETA records |
| GET | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/stops/{routeStopSequenceId}/eta` | Read ETA for one stop |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/transport/trips/{tripId}/eta` | Read guardian-visible ETA for linked student |
| GET | `/api/v1/schools/{schoolAccountId}/transport/eta-records/{etaRecordId}/trace` | Trace ETA to trip, stop, location, assignment, notification, anomaly, and audit outcomes |

## Recalculate Request

```yaml
include_route_stop_sequence_ids:
  - "route-stop-reference-1"
  - "route-stop-reference-2"
calculation_reason: "Location update received."
source_location_update_id: "location-update-reference"
client_request_id: "request-unique-to-caller"
```

## ETA Record Response

```yaml
eta_record_id: "eta-reference"
school_account_id: "school-account-reference"
transport_trip_id: "trip-reference"
transport_route_id: "route-reference"
route_stop_sequence_id: "route-stop-reference"
student_profile_id: "student-profile-reference"
estimated_arrival_time: "YYYY-MM-DDTHH:MM:SSZ"
eta_state: "Available"
confidence_state: "Medium"
freshness_status: "Current"
source_location_update_id: "location-update-reference"
calculated_at: "YYYY-MM-DDTHH:MM:SSZ"
review_status: "Not Required"
```

## Guardian ETA Response

```yaml
student_profile_id: "student-profile-reference"
transport_trip_id: "trip-reference"
route_stop_sequence_id: "route-stop-reference"
visibility_phase: "Before Boarding"
pickup_eta:
  eta_state: "Available"
  estimated_arrival_time: "YYYY-MM-DDTHH:MM:SSZ"
  confidence_state: "Medium"
  freshness_status: "Current"
exact_live_location_available: false
```

## Acceptance Rules

- ETA calculation requires `transport.eta_calculation` and
  `transport.live_tracking` to be enabled.
- ETA calculation requires tenant access, active trip, approved route, ordered
  stop sequence, non-overlapping bus/device assignment, current trusted trip
  progress, and `transport.eta.calculate` or authorized system evaluation.
- Stale, untrusted, missing, or inconsistent location evidence produces
  Unavailable, Stale, or Needs Review rather than unsupported ETA.
- ETA records must include trip, route, stop, affected student when applicable,
  estimated arrival state, freshness, confidence, source evidence, and review
  status.
- Material ETA changes may create eligible transport notification records when
  transport notifications are enabled.
- Staff-visible ETA lists require `transport.eta.read`.
- Guardian-visible ETA requires an approved active guardian link and transport
  visibility scope.
- ETA responses must be paginated where lists can grow and support filtering by
  trip, route, stop, student, state, confidence, freshness, and calculation time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny ETA workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks ETA permission | Deny and record required permission |
| Trip inactive or completed | Mark ETA unavailable or reject recalculation |
| Route sequence missing | Reject calculation with route readiness reason |
| Location stale or untrusted | Mark ETA stale, unavailable, or needs review |
| Guardian link inactive or out of scope | Suppress guardian ETA and record reason |
| Cross-school trip or student reference | Deny without exposing cross-tenant existence |
| Audit write fails for material ETA change | Reject mutation rather than allowing unaudited change |
