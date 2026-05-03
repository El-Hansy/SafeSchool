# Contract: Route & Stop Management

This contract defines route, stop, and route stop sequence management for Phase
3 Transport & Bus Tracking.

## Capabilities and Permissions

- Required capabilities:
  - `transport.route_stop_management`
- Common permissions:
  - `transport.routes.read`
  - `transport.routes.manage`
  - `transport.audit.read`

Route and stop changes are tenant-scoped, versioned for review, and must not
affect already completed trip evidence.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/transport/routes` | List routes with filters and readiness |
| POST | `/api/v1/schools/{schoolAccountId}/transport/routes` | Create a route |
| GET | `/api/v1/schools/{schoolAccountId}/transport/routes/{routeId}` | Read route details |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/routes/{routeId}` | Update route details or status |
| GET | `/api/v1/schools/{schoolAccountId}/transport/stops` | List reusable transport stops |
| POST | `/api/v1/schools/{schoolAccountId}/transport/stops` | Create a stop |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/stops/{stopId}` | Update stop details or status |
| POST | `/api/v1/schools/{schoolAccountId}/transport/routes/{routeId}/stop-sequences` | Replace or create a route stop sequence version |
| GET | `/api/v1/schools/{schoolAccountId}/transport/routes/{routeId}/versions/{routeVersion}` | Read a route version for review |
| GET | `/api/v1/schools/{schoolAccountId}/transport/routes/{routeId}/trace` | Trace route to assignments, trips, scans, ETAs, notifications, anomalies, and audit outcomes |

## Create Route Request

```yaml
route_name: "North Morning Route"
route_code: "NORTH-AM"
service_direction: "Pickup"
campus_reference: "north-campus"
planned_start_time: "06:30"
planned_end_time: "07:45"
route_status: "Draft"
client_request_id: "request-unique-to-caller"
```

## Create Stop Request

```yaml
stop_name: "Gate 4 Community Stop"
stop_code: "G4-COMMUNITY"
stop_reference: "school-approved-stop-reference"
pickup_allowed: true
drop_allowed: true
stop_status: "Active"
client_request_id: "request-unique-to-caller"
```

## Replace Route Stop Sequence Request

```yaml
route_version_reason: "New semester route plan."
service_direction: "Pickup"
stops:
  - transport_stop_id: "stop-reference-1"
    sequence_number: 1
    planned_arrival_offset: "00:00:00"
    planned_departure_offset: "00:03:00"
  - transport_stop_id: "stop-reference-2"
    sequence_number: 2
    planned_arrival_offset: "00:12:00"
    planned_departure_offset: "00:15:00"
client_request_id: "request-unique-to-caller"
```

## Route Detail Response

```yaml
transport_route_id: "route-reference"
school_account_id: "school-account-reference"
route_name: "North Morning Route"
route_code: "NORTH-AM"
service_direction: "Pickup"
route_status: "Active"
route_version: "2026-S1-v2"
planned_start_time: "06:30"
planned_end_time: "07:45"
stops:
  - route_stop_sequence_id: "route-stop-reference"
    transport_stop_id: "stop-reference"
    stop_name: "Gate 4 Community Stop"
    sequence_number: 1
    planned_arrival_offset: "00:00:00"
    sequence_status: "Active"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Route and stop management requires `transport.route_stop_management` to be
  enabled.
- Route and stop creation or update requires tenant access and
  `transport.routes.manage`.
- Active routes require valid route details and at least one active stop
  sequence for each enabled direction.
- A route version must preserve previous route stop sequence values for review
  after changes.
- Stops cannot be used for pickup or drop when that direction is disabled.
- Route and stop list responses must be paginated and support filtering by
  status, direction, campus reference, route code, stop code, and update time.
- Route trace must show related assignments, trips, scans, ETA records,
  notifications, anomalies, manual reviews, and audit evidence when those
  records exist.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny route workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks route permission | Deny and record required permission |
| Route code duplicate | Reject activation with duplicate code reason |
| Stop direction mismatch | Reject stop sequence or assignment use with direction reason |
| Route has no active stops | Prevent activation and record readiness reason |
| Route belongs to another tenant | Deny without exposing cross-tenant existence |
| Audit write fails for sensitive change | Reject mutation rather than allowing unaudited change |
