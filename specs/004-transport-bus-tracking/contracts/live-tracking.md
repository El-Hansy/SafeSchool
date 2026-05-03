# Contract: Live Tracking

This contract defines active trip lifecycle, authorized mobile location update
submission, staff trip progress visibility, guardian visibility boundaries, and
location retention for Phase 3.

## Capabilities and Permissions

- Required capabilities:
  - `transport.live_tracking`
- Common permissions:
  - `transport.trips.read`
  - `transport.trips.start`
  - `transport.trips.update`
  - `transport.trips.end`
  - `transport.location.submit`
  - `transport.tracking.read`
  - `transport.guardian_visibility.read`
  - `transport.audit.read`

Live tracking uses an authorized staff or vehicle mobile device associated with
the active trip. Dedicated bus hardware and physical vehicle control are out of
Phase 3 scope. Live tracking may share the TransportTripLifecycleService with
the scan-ready trip lifecycle from the Boarding/Drop Scan contract, but this
contract adds the full tracking route surface, location submission, progress
visibility, guardian live-location windows, and retention.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/transport/trips` | List trips with filters |
| POST | `/api/v1/schools/{schoolAccountId}/transport/trips` | Create or plan a trip |
| POST | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/start` | Start an active trip |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}` | Update trip staff, state, or review status |
| POST | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/end` | End an active trip |
| POST | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/location-updates` | Submit mobile location update |
| GET | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/progress` | Read staff-visible trip progress |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/transport/trips/{tripId}/progress` | Read guardian-visible linked-student trip progress |
| GET | `/api/v1/schools/{schoolAccountId}/transport/trips/{tripId}/trace` | Trace trip to route, assignments, scans, locations, ETAs, notifications, anomalies, reviews, and audit outcomes |

## Create Trip Request

```yaml
transport_route_id: "route-reference"
transport_vehicle_id: "vehicle-reference"
route_version: "2026-S1-v2"
service_direction: "Pickup"
trip_date: "YYYY-MM-DD"
planned_start_time: "YYYY-MM-DDTHH:MM:SSZ"
driver_reference: "driver-actor-reference"
attendant_reference: "attendant-actor-reference"
supervisor_reference: "supervisor-actor-reference"
tracking_device_reference: "registered-mobile-device-reference"
client_request_id: "request-unique-to-caller"
```

## Start Trip Request

```yaml
tracking_device_reference: "registered-mobile-device-reference"
started_by: "attendant-actor-reference"
actual_start_time: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "request-unique-to-caller"
```

## Location Update Request

```yaml
client_location_id: "location-unique-to-device"
tracking_device_reference: "registered-mobile-device-reference"
actor_reference: "attendant-actor-reference"
reported_at: "YYYY-MM-DDTHH:MM:SSZ"
location_reference: "non-sensitive-route-progress-reference"
progress_state: "En Route"
nearest_route_stop_sequence_id: "route-stop-reference"
```

## Trip Progress Response

```yaml
transport_trip_id: "trip-reference"
school_account_id: "school-account-reference"
transport_route_id: "route-reference"
transport_vehicle_id: "vehicle-reference"
trip_status: "Active"
service_direction: "Pickup"
actual_start_time: "YYYY-MM-DDTHH:MM:SSZ"
latest_location:
  transport_location_update_id: "location-reference"
  progress_state: "En Route"
  freshness_status: "Current"
  reported_at: "YYYY-MM-DDTHH:MM:SSZ"
  received_at: "YYYY-MM-DDTHH:MM:SSZ"
guardian_visibility_state: "Exact Location Available"
```

## Guardian Progress Response

```yaml
student_profile_id: "student-profile-reference"
transport_trip_id: "trip-reference"
visibility_phase: "Onboard"
pickup_eta: null
exact_live_location:
  location_reference: "guardian-allowed-location-reference"
  freshness_status: "Current"
  reported_at: "YYYY-MM-DDTHH:MM:SSZ"
drop_status: null
```

## Acceptance Rules

- Live tracking requires `transport.live_tracking` to be enabled.
- Starting a trip requires an active route, active vehicle, approved route
  version, authorized staff, authorized mobile tracking device, tenant access,
  and `transport.trips.start`.
- Multiple active trips may use the same route only when each active trip has a
  different vehicle and tracking device.
- A vehicle or tracking device cannot be attached to more than one active trip.
- Location submission requires `transport.location.submit` or an authorized
  mobile source assigned to the trip.
- Stale, untrusted, out-of-trip, cross-school, or inconsistent location updates
  are suppressed or routed to review with a reason.
- Staff-visible trip progress requires `transport.tracking.read`.
- Guardian progress requires an approved active guardian link and transport
  visibility scope.
- Guardian exact live bus location is available only after accepted boarding and
  before accepted or reviewed drop for the linked student. Before boarding,
  guardians see pickup ETA and trip status; after drop, guardians see drop
  status.
- Detailed location history is retained for 30 days, then reduced to trip
  summaries and audit evidence unless school account policy has an approved
  review hold.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny tracking workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks trip or tracking permission | Deny and record required permission |
| Route or vehicle inactive | Reject trip start with readiness reason |
| Bus or tracking device already active | Block overlapping active trip and record reason |
| Device not authorized for trip | Reject or suppress location update |
| Location stale or untrusted | Mark unavailable or suppress and record reason |
| Guardian not linked or out of visibility phase | Suppress exact location and record reason |
| Cross-school trip or student reference | Deny without exposing cross-tenant existence |
| Audit write fails for sensitive outcome | Reject mutation rather than allowing unaudited change |
