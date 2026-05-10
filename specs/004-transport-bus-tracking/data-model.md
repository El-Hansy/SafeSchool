# Data Model: Phase 3 Transport & Bus Tracking

This model defines runtime business entities for vehicles, routes, stops,
student assignments, active trips, boarding/drop scans, live tracking, ETA
records, notification records, anomalies, reviews, rule settings, retention,
and audit evidence. All tenant-owned entities include `tenant_id`,
`created_at`, and `updated_at`.

## Transport Vehicle

**Purpose**: A school-owned bus or transport vehicle used for route assignments
and active trips.

**Fields**:
- `transport_vehicle_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `vehicle_name`: Human-readable name or label.
- `vehicle_code`: School-unique vehicle code.
- `plate_reference`: Optional license or fleet reference.
- `capacity`: Optional student capacity.
- `vehicle_status`: Draft, Active, Suspended, Retired.
- `default_supervisor_reference`: Optional assigned supervisor.
- `default_driver_reference`: Optional assigned driver.
- `created_by`: Actor that created the vehicle.
- `updated_by`: Actor that last changed the vehicle.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May be assigned to many Student Transport Assignments.
- May be assigned to many Transport Trips over time.
- Produces Audit Events when configured or status changes.

**Validation rules**:
- `vehicle_code` must be unique within the school account while active.
- Active trips cannot use suspended or retired vehicles.
- Vehicle management requires `transport.vehicles.manage`.
- Cross-school vehicle lookup is denied unless an explicit platform-level
  review permission allows inspection.

**State transitions**:
- Draft -> Active when required vehicle details are valid.
- Active -> Suspended during maintenance, investigation, or operational hold.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Transport Route

**Purpose**: A school account route definition with service direction, active
state, planned timing, and review history.

**Fields**:
- `transport_route_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `route_name`: Human-readable route name.
- `route_code`: School-unique route code.
- `service_direction`: Pickup, Dropoff, or Combined.
- `campus_reference`: Optional internal campus or division reference.
- `planned_start_time`: Optional planned start time in school time context.
- `planned_end_time`: Optional planned end time in school time context.
- `route_status`: Draft, Active, Suspended, Retired.
- `route_version`: Reviewable version of the route plan.
- `created_by`: Actor that created the route.
- `updated_by`: Actor that last changed the route.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Route Stop Sequences.
- Has many Student Transport Assignments.
- Has many Transport Trips.
- Produces Audit Events when configured or status changes.

**Validation rules**:
- Active routes require at least one active stop sequence for each enabled
  direction.
- Route and stop management requires `transport.routes.manage`.
- Route codes must be unique within the school account while active.
- Suspended or retired routes cannot receive new normal assignments or start new
  active trips.

**State transitions**:
- Draft -> Active when route details and stop sequence are valid.
- Active -> Suspended during temporary operational hold.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Transport Stop

**Purpose**: A pickup or drop location that can be included in one or more
route plans within the school account.

**Fields**:
- `transport_stop_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `stop_name`: Human-readable stop name.
- `stop_code`: Optional school-unique stop code.
- `stop_reference`: Optional map, campus, or locality reference.
- `pickup_allowed`: Whether pickup is allowed at this stop.
- `drop_allowed`: Whether drop is allowed at this stop.
- `stop_status`: Draft, Active, Suspended, Retired.
- `created_by`: Actor that created the stop.
- `updated_by`: Actor that last changed the stop.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May appear in many Route Stop Sequences.
- May be used as pickup or drop stop in many Student Transport Assignments.
- May be referenced by Boarding/Drop Scan Events and ETA Records.

**Validation rules**:
- Active stops must belong to exactly one school account.
- A stop cannot be used for pickup or drop when that direction is not allowed.
- Similar names are allowed only when reviewers can distinguish stop references
  within the school account.

**State transitions**:
- Draft -> Active when required stop details are valid.
- Active -> Suspended during temporary removal.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Route Stop Sequence

**Purpose**: The ordered stop plan for a route and direction.

**Fields**:
- `route_stop_sequence_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `transport_route_id`: Parent route.
- `transport_stop_id`: Stop included in the route.
- `service_direction`: Pickup or Dropoff.
- `sequence_number`: Stop order within route and direction.
- `planned_arrival_offset`: Optional planned timing from trip start.
- `planned_departure_offset`: Optional planned departure from stop.
- `route_version`: Route version where this sequence applies.
- `sequence_status`: Active, Superseded, Suspended.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Transport Route.
- References one Transport Stop.
- May be referenced by Student Transport Assignments, Transport Trips,
  Boarding/Drop Scan Events, and ETA Records.

**Validation rules**:
- `sequence_number` must be unique within route, direction, and route version.
- A route version must not contain suspended stops as normal active stops.
- Route updates preserve superseded sequences for review.

**State transitions**:
- Active -> Superseded when a new route version replaces it.
- Active -> Suspended when temporarily unavailable.
- Suspended -> Active when restored in the current route version.

## Student Transport Assignment

**Purpose**: The relationship between a student and a route, vehicle or planned
trip, pickup stop, drop stop, visibility state, and validity period.

**Fields**:
- `student_transport_assignment_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student from Phase 1.
- `transport_route_id`: Assigned route.
- `transport_vehicle_id`: Optional default assigned vehicle.
- `pickup_route_stop_sequence_id`: Optional pickup stop sequence.
- `drop_route_stop_sequence_id`: Optional drop stop sequence.
- `service_direction`: Pickup, Dropoff, or Both.
- `valid_from`: First date assignment may be used.
- `valid_to`: Optional last date assignment may be used.
- `visibility_state`: Guardian Visible, Staff Only, Suspended.
- `assignment_status`: Draft, Active, Suspended, Expired, Removed.
- `created_by`: Actor that created the assignment.
- `updated_by`: Actor that last changed the assignment.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Student Profile from Phase 1.
- References one Transport Route.
- May reference one Transport Vehicle.
- References pickup and drop Route Stop Sequences when applicable.
- May be used by many Transport Trips and Boarding/Drop Scan Events.

**Validation rules**:
- Active assignments require an active student profile, active route, valid stop
  sequences, valid date range, enabled capability, and `transport.assignments.manage`.
- Assignment activation is denied for inactive students, inactive routes,
  invalid dates, cross-school route/student combinations, or unavailable
  feature capability.
- Guardian visibility requires an approved active guardian link and transport
  visibility scope.
- Overlapping assignments for the same student, date range, direction, and
  route require explicit review.

**State transitions**:
- Draft -> Active when all eligibility rules pass.
- Active -> Suspended during temporary hold.
- Suspended -> Active when restored.
- Active or Suspended -> Expired when date range ends.
- Draft, Active, or Suspended -> Removed when revoked by authorized staff.

## Transport Trip

**Purpose**: A dated run of a route with assigned bus, assigned staff,
authorized mobile tracking device, planned stops, live state, start and end
evidence, and review status.

**Fields**:
- `transport_trip_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `transport_route_id`: Route being run.
- `route_version`: Route version used by the trip.
- `transport_vehicle_id`: Assigned vehicle.
- `tracking_device_reference`: Authorized staff or vehicle mobile device.
- `service_direction`: Pickup or Dropoff.
- `trip_date`: School date for the trip.
- `planned_start_time`: Planned start time.
- `actual_start_time`: Actual start time when available.
- `actual_end_time`: Actual end time when available.
- `driver_reference`: Assigned driver actor.
- `attendant_reference`: Assigned attendant actor.
- `supervisor_reference`: Assigned supervisor actor.
- `trip_status`: Planned, Active, Paused, Completed, Cancelled, Needs Review.
- `start_source`: Actor or device source that started the trip.
- `end_source`: Actor or device source that ended the trip.
- `review_status`: Not Required, Needs Review, In Review, Corrected, Closed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Transport Route and one Transport Vehicle.
- References planned Route Stop Sequences.
- Has many Boarding/Drop Scan Events, Transport Location Updates, ETA Records,
  Notification Records, Transport Anomalies, Manual Transport Reviews, and Audit
  Events.

**Validation rules**:
- Starting a trip requires active route, active vehicle, authorized staff,
  authorized mobile tracking device, enabled capability, and
  `transport.trips.start`.
- Multiple active trips may use the same route only when each active trip has a
  different vehicle and tracking device.
- A vehicle or tracking device cannot be attached to more than one active trip.
- Completed or cancelled trips cannot accept normal scans or location updates
  without manual review.

**State transitions**:
- Planned -> Active when started by authorized staff or device.
- Active -> Paused during temporary operational hold.
- Paused -> Active when resumed.
- Active or Paused -> Completed when ended normally.
- Planned, Active, or Paused -> Cancelled when authorized staff cancel it.
- Any non-closed status -> Needs Review when conflicts or anomalies require
  review.

## Offline Transport Scan Sync Batch

**Purpose**: A retry-safe submission of offline boarding/drop scan events from
one transport scan source.

**Fields**:
- `offline_transport_scan_sync_batch_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `client_batch_id`: Caller-stable batch identifier.
- `transport_trip_id`: Trip associated with the submitted scans.
- `tracking_device_reference`: Source mobile device.
- `actor_reference`: Staff actor when applicable.
- `submitted_at`: Time the batch was received.
- `batch_status`: Received, Partially Reconciled, Reconciled, Rejected.
- `accepted_count`: Number of scan events accepted or matched as retries.
- `needs_review_count`: Number of scan events routed to review.
- `rejected_count`: Number of scan events rejected.
- `duplicate_count`: Number of scan events identified as duplicates.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Transport Trip.
- Contains many Boarding/Drop Scan Events.
- Produces Audit Events for reconciliation outcomes.

**Validation rules**:
- `client_batch_id` must be unique per tenant and scan source for idempotent
  retry handling.
- Retried batches with the same client identity return the same outcomes where
  submitted contents match.
- Batches from unauthorized devices or inactive trips are rejected or routed to
  review according to trip state.

## Boarding/Drop Scan Event

**Purpose**: Captured NFC or QR identity evidence for a student boarding or
dropping from a transport trip.

**Fields**:
- `boarding_drop_scan_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `client_scan_id`: Caller-stable scan identifier for idempotency.
- `offline_transport_scan_sync_batch_id`: Related offline batch when applicable.
- `transport_trip_id`: Trip where the scan occurred.
- `transport_route_id`: Route used by the trip.
- `route_stop_sequence_id`: Stop where scan occurred when known.
- `student_profile_id`: Student resolved from identity evidence when known.
- `student_transport_assignment_id`: Assignment matched when available.
- `identity_credential_id`: Credential resolved from Phase 1 evidence when
  known.
- `credential_type`: NFC Card or QR Fallback.
- `credential_reference`: Non-secret credential reference presented.
- `scan_method`: NFC, QR, or Manual Review Entry.
- `scan_direction`: Boarding or Drop.
- `actor_reference`: Staff actor when applicable.
- `tracking_device_reference`: Source mobile device.
- `local_scan_time`: Time recorded by the scan source.
- `received_at`: Time received by the backend.
- `school_effective_time`: Time used for transport decisions.
- `offline_captured`: Whether the scan was captured without connectivity.
- `sync_status`: Pending, Received, Reconciled, Duplicate, Rejected, Needs
  Review.
- `scan_decision`: Accepted, Denied, Flagged, Needs Review.
- `decision_reason`: Reason for current decision.
- `transport_status_after`: Waiting, Onboard, Dropped, Denied, Needs Review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Belongs to one Transport Trip.
- May belong to one Offline Transport Scan Sync Batch.
- May reference one Student Profile, Identity Credential, Student Transport
  Assignment, and Route Stop Sequence.
- May produce Notification Records, Transport Anomalies, Manual Transport
  Reviews, and Audit Events.

**Validation rules**:
- `client_scan_id` must be unique per tenant and scan source for physical scan
  idempotency.
- Accepted scans require active school account scope, active trip, authorized
  staff or device source, valid credential evidence, active assignment, route
  and stop match, enabled capability, and required permission.
- Expired, suspended, revoked, replaced, unknown, duplicated, or cross-school
  credentials are denied or flagged and cannot create normal boarding/drop
  status without review.
- Students without active assignment or with route/stop mismatch are recorded as
  needs-review anomalies and do not produce normal status or guardian
  notification until reviewer approval.
- Offline scans preserve both local scan time and received time.

## Transport Location Update

**Purpose**: Accepted or suppressed trip progress evidence tied to an active
trip and an authorized staff or vehicle mobile device.

**Fields**:
- `transport_location_update_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `transport_trip_id`: Active trip receiving the update.
- `tracking_device_reference`: Authorized mobile device.
- `actor_reference`: Staff actor when applicable.
- `client_location_id`: Caller-stable location identity for retry handling.
- `reported_at`: Time reported by the mobile source.
- `received_at`: Time received by the backend.
- `location_reference`: Non-sensitive route progress or map reference.
- `progress_state`: At Start, En Route, Approaching Stop, At Stop, Delayed,
  Stale, Deviated, Completed, Needs Review.
- `nearest_route_stop_sequence_id`: Optional nearest planned stop.
- `freshness_status`: Current, Stale, Untrusted, Suppressed.
- `acceptance_status`: Accepted, Suppressed, Needs Review, Rejected.
- `suppression_reason`: Reason when not accepted.
- `retention_state`: Detailed, Summary Only, Review Hold.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Transport Trip.
- May feed many ETA Records.
- May produce Transport Anomalies and Audit Events.

**Validation rules**:
- Location updates are accepted only for active trips and authorized tracking
  devices associated with that trip.
- Stale, untrusted, cross-school, out-of-trip, or inconsistent updates are
  suppressed or marked unavailable with a reason.
- Guardian-facing exact location is limited to the interval after accepted
  boarding and before accepted or reviewed drop for the linked student.
- Detailed location records older than 30 days become summary-only unless an
  approved review hold applies.

## ETA Record

**Purpose**: Estimated arrival state for a trip stop or linked-student transport
view.

**Fields**:
- `eta_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `transport_trip_id`: Trip being evaluated.
- `transport_route_id`: Route used by the trip.
- `route_stop_sequence_id`: Stop receiving ETA.
- `student_profile_id`: Optional linked student when ETA is student-specific.
- `student_transport_assignment_id`: Optional assignment used for visibility.
- `estimated_arrival_time`: Estimated arrival time when available.
- `eta_state`: Available, Delayed, Unavailable, Stale, Needs Review.
- `confidence_state`: High, Medium, Low, Unsupported.
- `freshness_status`: Current, Stale, Suppressed.
- `source_location_update_id`: Location update used when applicable.
- `calculated_at`: Time ETA was calculated.
- `review_status`: Not Required, Needs Review, Corrected, Closed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Transport Trip and Route Stop Sequence.
- May reference one Student Transport Assignment and Student Profile.
- May create Transport Notification Records when materially changed.
- May produce Transport Anomalies and Audit Events.

**Validation rules**:
- ETA calculation requires enabled capability, active trip, approved route
  sequence, non-overlapping bus/device assignment, and sufficiently current
  trusted trip progress.
- Stale or inconsistent progress results in Unavailable, Stale, or Needs Review
  rather than unsupported estimates.
- Guardian-facing ETA requires approved guardian link scope and transport
  visibility.

## Transport Notification Record

**Purpose**: A guardian-facing notification event or suppression record tied to
an eligible transport event.

**Fields**:
- `transport_notification_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `guardian_record_id`: Guardian from Phase 1 when applicable.
- `student_profile_id`: Linked student.
- `guardian_link_id`: Guardian link used for eligibility.
- `transport_trip_id`: Related trip.
- `event_type`: Boarding, Drop, Delay, ETA Changed, Route Changed, Review
  Corrected, Suppressed.
- `source_event_reference`: Related scan, ETA, anomaly, trip, or review record.
- `notification_status`: Eligible, Created, Visible, Suppressed, Withdrawn,
  Failed.
- `suppression_reason`: Reason when suppressed.
- `visible_status`: Guardian-facing status text/category.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Guardian Link and Student Profile from Phase 1.
- References one Transport Trip.
- May reference Boarding/Drop Scan Events, ETA Records, Transport Anomalies, or
  Manual Transport Reviews.
- Produces Audit Events for eligibility, suppression, creation, withdrawal, and
  visibility changes.

**Validation rules**:
- Notification records require enabled capability, accepted or reviewed
  transport event, approved active guardian link, and allowed visibility scope.
- Denied, unresolved, stale, untrusted, or suppressed evidence cannot create a
  normal guardian notification.
- Guardian visibility must not expose other students, routes, or trips outside
  the approved link scope.

## Transport Anomaly

**Purpose**: A reviewable issue involving missing, duplicate, invalid,
conflicting, delayed, stale, deviated, or manual-review-required transport
evidence.

**Fields**:
- `transport_anomaly_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `transport_trip_id`: Related trip.
- `student_profile_id`: Affected student when applicable.
- `transport_route_id`: Related route when applicable.
- `route_stop_sequence_id`: Related stop when applicable.
- `source_event_reference`: Scan, location, ETA, notification, trip, or
  assignment record that triggered the anomaly.
- `anomaly_type`: Missed Boarding, Missed Drop, Wrong Route, Wrong Stop,
  Duplicate Scan, Invalid Credential, Out Of Order Scan, Delayed Offline
  Conflict, Route Deviation, Stale Location, Delayed Trip, Manual Review
  Required.
- `severity`: Low, Medium, High, Critical.
- `status`: Open, Assigned, In Review, Resolved, Dismissed, Reopened.
- `assigned_to`: Reviewer actor when applicable.
- `detected_at`: Detection time.
- `resolution_reason`: Reason when resolved or dismissed.
- `resolved_by`: Reviewer actor when applicable.
- `resolved_at`: Resolution time when applicable.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and usually one Transport Trip.
- May reference Boarding/Drop Scan Events, Location Updates, ETA Records,
  Notification Records, Assignments, and Manual Transport Reviews.
- Produces Audit Events for detection and status changes.

**Validation rules**:
- Anomaly records require affected evidence, type, severity, status, and review
  need.
- Assignment, resolution, dismissal, and reopening require reviewer permission,
  reason where status changes, and audit evidence.
- Resolving an anomaly must not silently change scan, trip, ETA, or notification
  outcomes; corrections require Manual Transport Review.
- Re-running detection for the same evidence updates or returns existing open
  anomalies rather than creating duplicate anomalies.

**State transitions**:
- Open -> Assigned when routed to a reviewer.
- Open or Assigned -> In Review when review starts.
- Open, Assigned, or In Review -> Resolved when accepted with a reason.
- Open, Assigned, or In Review -> Dismissed when rejected with a reason.
- Resolved or Dismissed -> Reopened when new evidence requires review.

## Manual Transport Review

**Purpose**: A reviewer action that resolves, dismisses, or corrects transport
scan, trip, ETA, notification, or anomaly outcomes.

**Fields**:
- `manual_transport_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `reviewer_reference`: Actor performing the review.
- `transport_trip_id`: Related trip when applicable.
- `student_profile_id`: Affected student when applicable.
- `source_record_type`: Scan, Trip, ETA, Notification, Anomaly, Assignment, or
  Location.
- `source_record_reference`: Record being reviewed.
- `review_action`: Approve, Correct, Resolve, Dismiss, Withdraw, Reopen.
- `original_status`: Status before review.
- `corrected_status`: Status after review when applicable.
- `review_reason`: Required business reason.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May update Boarding/Drop Scan Events, Transport Trips, ETA Records,
  Notification Records, and Transport Anomalies.
- Produces Audit Events.

**Validation rules**:
- Review actions require explicit reviewer permission and tenant scope.
- Corrections preserve original evidence and current corrected outcome.
- Review cannot create campus attendance or campus access outcomes.
- Audit write failure rejects sensitive review mutation.

## Transport Rule Setting

**Purpose**: School account configuration for assignment, scan, tracking, ETA,
notification, anomaly, clock drift, retry, and retention behavior.

**Fields**:
- `transport_rule_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `assignment_eligibility_policy`: Current assignment eligibility policy.
- `pickup_window`: Allowed pickup window.
- `drop_window`: Allowed drop window.
- `route_deviation_threshold`: Threshold for route deviation anomalies.
- `location_staleness_threshold`: Threshold for stale location status.
- `eta_change_threshold`: Threshold for material ETA change.
- `notification_eligibility_policy`: Guardian notification eligibility policy.
- `anomaly_detection_policy`: Enabled anomaly types and severity rules.
- `scan_clock_drift_tolerance`: Allowed source clock drift.
- `retry_handling_policy`: Duplicate and retry handling policy.
- `location_detail_retention_days`: Detailed location retention, fixed at 30
  days unless an approved review hold requires longer retention.
- `rule_status`: Draft, Active, Superseded, Suspended.
- `created_by`: Actor that created the rule setting.
- `updated_by`: Actor that last changed the rule setting.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Used by Transport Assignments, Trips, Scans, Location Updates, ETA Records,
  Notifications, Anomalies, and Retention behavior.
- Produces Audit Events when changed.

**Validation rules**:
- Active rule settings require `transport.rules.manage`.
- Detailed location retention is 30 days; longer retention requires approved
  review hold or school policy.
- Superseded rules remain reviewable for events evaluated under that rule
  version.

**State transitions**:
- Draft -> Active when required policy values pass validation.
- Active -> Superseded when a newer active version replaces it.
- Active -> Suspended during policy hold.
- Suspended -> Active when restored.

## Transport Review Summary

**Purpose**: Tenant-scoped read model for reviewers to inspect Phase 3 status
across students, routes, buses, trips, stops, scans, location progress, ETAs,
notifications, anomalies, manual reviews, and audit traces.

**Fields**:
- `school_account_id`: Owning school account scope.
- `summary_scope`: Student, Route, Bus, Trip, Stop, or School Account.
- `scope_reference`: Identifier for the selected summary scope.
- `student_profile_id`: Optional student reference when the summary is student-scoped.
- `transport_route_id`: Optional route reference.
- `transport_vehicle_id`: Optional bus or vehicle reference.
- `transport_trip_id`: Optional trip reference.
- `route_stop_sequence_id`: Optional stop sequence reference.
- `scan_status_counts`: Counts by boarding/drop scan decision and review status.
- `location_status_counts`: Counts by current, stale, suppressed, or unavailable location state.
- `eta_state_counts`: Counts by ETA freshness, confidence, unavailable, or needs-review state.
- `notification_status_counts`: Counts by eligible, visible, suppressed, withdrawn, or failed state.
- `anomaly_status_counts`: Counts by anomaly type, severity, and status.
- `latest_evidence_at`: Most recent source evidence timestamp included in the summary.
- `trace_reference`: Link set for drill-down to source records and audit evidence.

**Relationships**:
- Computed from tenant-owned Transport Routes, Vehicles, Assignments, Trips,
  Scan Events, Location Updates, ETA Records, Notification Records, Anomalies,
  Manual Reviews, and Audit Events.
- May be cached only if the cache includes `tenant_id`, freshness metadata, and
  invalidation rules.

**Validation rules**:
- Staff summaries require tenant access and `transport.review.read`.
- Guardian-facing summaries are limited to linked-student transport visibility
  and must not expose unrelated route, bus, trip, or student records.
- Filters must preserve tenant scope and must not reveal cross-school existence.

**State transitions**:
- Not a mutable business record; regenerated or refreshed from source evidence.

## School Account Feature Setting

**Purpose**: School account capability setting that determines whether Phase 3
workflows are available.

**Fields**:
- `school_account_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `capability_key`: Transport capability key.
- `enabled`: Whether capability is available.
- `effective_from`: Optional activation time.
- `effective_to`: Optional expiration time.
- `updated_by`: Actor that changed the capability.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Evaluated by every Phase 3 workflow before sensitive actions.
- Produces Audit Events when changed.

**Validation rules**:
- Disabled capabilities block backend actions and hide unavailable workflows in
  web or mobile surfaces.
- Capability checks are not an authorization substitute; permissions and tenant
  checks still apply.

## Audit Event

**Purpose**: Reviewable evidence for sensitive Phase 3 actions.

**Fields**:
- `audit_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `actor_reference`: Actor or device responsible for the event.
- `event_type`: Route Changed, Vehicle Changed, Assignment Changed, Trip
  Started, Trip Ended, Scan Captured, Scan Denied, Sync Reconciled, Location
  Accepted, Location Suppressed, ETA Changed, Notification Created,
  Notification Suppressed, Anomaly Created, Review Corrected, Access Denied,
  Retention Applied.
- `target_record_type`: Type of affected record.
- `target_record_reference`: Affected record identifier.
- `reason`: Business or system reason.
- `occurred_at`: Event time.
- `metadata_reference`: Optional non-secret metadata reference.
- `created_at`: Creation time.

**Relationships**:
- Belongs to one School Account.
- May reference any Phase 3 entity.

**Validation rules**:
- Sensitive Phase 3 mutations fail when required audit evidence cannot be
  recorded.
- Audit records must not expose cross-school student, guardian, credential, or
  trip existence to unauthorized actors.
- Audit records are retained according to school policy and platform audit
  governance.
