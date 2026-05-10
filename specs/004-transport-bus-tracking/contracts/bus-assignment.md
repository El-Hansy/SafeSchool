# Contract: Bus Assignment

This contract defines vehicle management and student transport assignment
behavior for Phase 3.

## Capabilities and Permissions

- Required capabilities:
  - `transport.bus_assignment`
  - `transport.route_stop_management`
- Common permissions:
  - `transport.vehicles.read`
  - `transport.vehicles.manage`
  - `transport.assignments.read`
  - `transport.assignments.manage`
  - `transport.guardian_visibility.read`
  - `transport.audit.read`

Assignments consume Phase 1 student profile, guardian link, and credential
evidence. Assignment behavior does not create attendance or campus access
outcomes.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/transport/vehicles` | List vehicles with filters |
| POST | `/api/v1/schools/{schoolAccountId}/transport/vehicles` | Create a vehicle |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/vehicles/{vehicleId}` | Update vehicle details or status |
| GET | `/api/v1/schools/{schoolAccountId}/transport/assignments` | Review assignments with filters |
| POST | `/api/v1/schools/{schoolAccountId}/transport/assignments` | Create a student transport assignment |
| GET | `/api/v1/schools/{schoolAccountId}/transport/assignments/{assignmentId}` | Read assignment details |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/assignments/{assignmentId}` | Update assignment details or status |
| GET | `/api/v1/schools/{schoolAccountId}/transport/students/{studentProfileId}/plan` | Read staff-visible student transport plan |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/transport/plan` | Read guardian-visible transport plan |
| GET | `/api/v1/schools/{schoolAccountId}/transport/assignments/{assignmentId}/trace` | Trace assignment to trips, scans, notifications, anomalies, and audit outcomes |

## Create Vehicle Request

```yaml
vehicle_name: "Bus 12"
vehicle_code: "BUS-12"
plate_reference: "fleet-plate-reference"
capacity: 40
vehicle_status: "Active"
default_driver_reference: "driver-actor-reference"
default_supervisor_reference: "supervisor-actor-reference"
client_request_id: "request-unique-to-caller"
```

## Create Assignment Request

```yaml
student_profile_id: "student-profile-reference"
transport_route_id: "route-reference"
transport_vehicle_id: "vehicle-reference"
pickup_route_stop_sequence_id: "pickup-stop-reference"
drop_route_stop_sequence_id: "drop-stop-reference"
service_direction: "Both"
valid_from: "YYYY-MM-DD"
valid_to: "YYYY-MM-DD"
visibility_state: "Guardian Visible"
assignment_status: "Active"
client_request_id: "request-unique-to-caller"
```

## Assignment Response

```yaml
student_transport_assignment_id: "assignment-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
transport_route_id: "route-reference"
transport_vehicle_id: "vehicle-reference"
pickup_route_stop_sequence_id: "pickup-stop-reference"
drop_route_stop_sequence_id: "drop-stop-reference"
service_direction: "Both"
valid_from: "YYYY-MM-DD"
valid_to: "YYYY-MM-DD"
visibility_state: "Guardian Visible"
assignment_status: "Active"
review_status: "Not Required"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Vehicle and assignment workflows require `transport.bus_assignment` to be
  enabled.
- Assignment activation requires an active student profile, active route, valid
  route stop sequences, valid date range, active vehicle when provided, tenant
  scope, required permission, and audit evidence.
- Guardian-visible assignments require an approved active guardian link and
  transport visibility scope.
- Assignment changes must preserve prior values and reason for review.
- Inactive students, inactive routes, invalid date ranges, suspended vehicles,
  and cross-school route/student combinations are prevented from activation.
- Assignment list responses must be paginated and support filtering by student,
  route, vehicle, stop, status, visibility, validity dates, and update time.
- Guardian transport plan responses must show only records allowed by the
  approved guardian link and school account feature settings.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny assignment workflow and record feature capability reason |
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks assignment permission | Deny and record required permission |
| Student inactive or missing | Reject activation with student eligibility reason |
| Route or stop inactive | Reject activation with route readiness reason |
| Vehicle suspended or retired | Reject activation with vehicle status reason |
| Date range invalid | Reject assignment with validation details |
| Guardian link inactive or out of scope | Suppress guardian visibility and record reason |
| Cross-school reference | Deny without exposing cross-tenant existence |
| Audit write fails for sensitive change | Reject mutation rather than allowing unaudited change |
