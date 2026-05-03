# Data Model: Phase 2 Attendance & Campus Access

This model defines runtime business entities for gates, scan points, gate scan
events, campus access decisions, attendance sessions, attendance records,
entry/exit notification records, anomalies, manual reviews, sync batches, and
audit evidence. All tenant-owned entities include `tenant_id`, `created_at`,
and `updated_at`.

## Gate

**Purpose**: A school account location where student campus entry or exit may
be scanned.

**Fields**:
- `gate_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `gate_name`: Human-readable gate name.
- `campus_reference`: Optional internal campus or division reference.
- `allowed_directions`: Entry, Exit, or Entry and Exit.
- `gate_status`: Draft, Active, Suspended, Decommissioned.
- `timezone`: School-approved time context for scan review.
- `created_by`: Actor that created the gate.
- `updated_by`: Actor that last changed the gate.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Scan Points.
- Has many Gate Scan Events through Scan Points.
- Produces Audit Events when configured or status changes.

**Validation rules**:
- Active gates must belong to exactly one school account.
- A suspended or decommissioned gate cannot accept normal scans.
- Gate configuration changes require `attendance_access.gates.manage`.
- Cross-school gate lookup is denied unless an explicit platform-level review
  permission allows inspection.

**State transitions**:
- Draft -> Active when required gate details are valid.
- Active -> Suspended during temporary closure or review.
- Suspended -> Active when restored.
- Active or Suspended -> Decommissioned when no longer used.

## Scan Point

**Purpose**: An authorized physical or staff-operated point associated with a
gate that records entry or exit scans.

**Fields**:
- `scan_point_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `gate_id`: Associated gate.
- `scan_point_name`: Human-readable name.
- `scan_point_type`: Staff Device, Gate Device, or Review Station.
- `device_reference`: Optional registered device reference.
- `assigned_actor_reference`: Optional staff actor assigned to the point.
- `allowed_directions`: Entry, Exit, or Entry and Exit.
- `offline_allowed`: Whether offline capture is allowed.
- `cached_policy_version`: Last policy or credential snapshot version known to
  the scan point.
- `last_seen_at`: Last known connectivity or sync time.
- `scan_point_status`: Pending, Active, Suspended, Retired.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Gate.
- Has many Gate Scan Events.
- May be included in Offline Scan Sync Batches.

**Validation rules**:
- Active scan points require an active gate in the same school account.
- A scan point cannot record a direction outside its allowed directions.
- Offline capture is allowed only when the school account and scan point allow
  it.
- Staff-operated scan points require an actor with
  `attendance_access.scans.record`.

**State transitions**:
- Pending -> Active when authorized for use.
- Active -> Suspended during device loss, policy issue, or operational hold.
- Suspended -> Active when restored.
- Active or Suspended -> Retired when no longer used.

## Offline Scan Sync Batch

**Purpose**: A retry-safe submission of offline scan events from one scan
source.

**Fields**:
- `offline_scan_sync_batch_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `client_batch_id`: Caller-stable batch identifier.
- `scan_point_id`: Source scan point.
- `device_reference`: Source device.
- `actor_reference`: Staff actor when applicable.
- `submitted_at`: Time the batch was received.
- `batch_status`: Received, Partially Reconciled, Reconciled, Rejected.
- `accepted_count`: Number of scan events accepted or matched as retries.
- `rejected_count`: Number of scan events rejected.
- `duplicate_count`: Number of scan events identified as duplicates.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Scan Point.
- Contains many Gate Scan Events.
- Produces Audit Events for reconciliation outcomes.

**Validation rules**:
- `client_batch_id` must be unique per tenant and scan source for idempotent
  retry handling.
- A retried batch with the same client identity must return the same accepted,
  duplicate, and rejected outcomes where the submitted contents match.
- A batch from a suspended scan point is rejected or routed to review.

## Gate Scan Event

**Purpose**: Captured evidence of an NFC or QR identity scan at a campus gate.

**Fields**:
- `gate_scan_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `client_scan_id`: Caller-stable scan identifier for idempotency.
- `offline_scan_sync_batch_id`: Related offline batch when applicable.
- `student_profile_id`: Student resolved from identity evidence when known.
- `identity_credential_id`: Credential resolved from Phase 1 evidence when
  known.
- `credential_type`: NFC Card or QR Fallback.
- `credential_reference`: Non-secret credential reference presented.
- `scan_method`: NFC, QR, or Manual Review Entry.
- `scan_direction`: Entry or Exit.
- `gate_id`: Gate where the scan occurred.
- `scan_point_id`: Scan point that captured the scan.
- `actor_reference`: Staff actor when applicable.
- `device_reference`: Source device when applicable.
- `local_scan_time`: Time recorded by the scan source.
- `received_at`: Time received by the backend.
- `school_effective_time`: Time used for school attendance decisions.
- `offline_captured`: Whether the scan was captured without connectivity.
- `sync_status`: Pending, Received, Reconciled, Duplicate, Rejected, Needs
  Review.
- `scan_decision`: Allowed, Denied, Flagged, Needs Review.
- `decision_reason`: Reason for current decision.
- `campus_state_after`: On Campus, Off Campus, Unknown, or Needs Review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May belong to one Offline Scan Sync Batch.
- References one Gate and one Scan Point.
- May reference one Student Profile and one Identity Credential from Phase 1.
- Produces one Campus Access Decision.
- May produce Attendance Records, Notification Records, Anomalies, Manual
  Reviews, and Audit Events.

**Validation rules**:
- `client_scan_id` must be unique per tenant and scan source for physical scan
  idempotency.
- Allowed scans require active school account scope, active scan point, allowed
  direction, valid credential evidence, enabled capability, and authorized
  actor or device source.
- Expired, suspended, revoked, replaced, unknown, duplicated, or cross-school
  credentials are denied or flagged and cannot create normal attendance without
  review.
- Offline scans preserve both local scan time and received time.
- Duplicate scans cannot create duplicate attendance records.

## Campus Access Decision

**Purpose**: The allowed, denied, flagged, or needs-review outcome produced from
a gate scan.

**Fields**:
- `campus_access_decision_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `gate_scan_event_id`: Related scan event.
- `decision`: Allowed, Denied, Flagged, Needs Review.
- `decision_reason`: Explanation of the decision.
- `feature_capability_key`: Capability checked when applicable.
- `permission_key`: Permission checked when applicable.
- `credential_status_used`: Credential status considered.
- `decided_at`: Decision time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Gate Scan Event.
- May be referenced by Attendance Records, Notification Records, Anomalies, and
  Audit Events.

**Validation rules**:
- Denied or flagged decisions require a decision reason.
- A decision must not expose cross-tenant credential or student existence to an
  unauthorized actor.
- Allowed decisions require traceability to tenant, feature, credential, scan
  point, and permission checks.

## Attendance Day or Session

**Purpose**: A school-defined period for evaluating attendance from gate scan
evidence.

**Fields**:
- `attendance_session_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `session_name`: Human-readable name.
- `attendance_date`: School attendance date.
- `campus_reference`: Optional internal campus or division scope.
- `student_group_reference`: Optional expected population scope.
- `entry_window_start`: Earliest normal entry time.
- `entry_window_end`: Latest accepted entry time for normal present status.
- `late_after`: Time after which accepted entry becomes late.
- `early_exit_before`: Time before which accepted exit becomes early exit.
- `expected_population_rule`: Rule or group reference for expected students.
- `generation_status`: Draft, Active, Generated, Reopened, Closed.
- `created_by`: Actor that created the session.
- `updated_by`: Actor that last changed the session.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Attendance Records.
- May be referenced by Attendance Anomalies and Audit Events.

**Validation rules**:
- Attendance generation requires an active session and
  `attendance_access.attendance_generation`.
- Time windows must be ordered and use school-approved time context.
- Expected population rules must not include students outside the school
  account.

**State transitions**:
- Draft -> Active when attendance rules are valid.
- Active -> Generated after generation runs.
- Generated -> Reopened when authorized review allows correction.
- Generated or Reopened -> Closed when review is complete.

## Attendance Record

**Purpose**: The student's attendance outcome for an attendance day or session.

**Fields**:
- `attendance_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `attendance_session_id`: Related attendance session.
- `student_profile_id`: Student.
- `current_status`: Present, Late, Absent, Early Exit, Needs Review, Excused.
- `entry_scan_event_id`: Accepted entry scan used when applicable.
- `exit_scan_event_id`: Accepted exit scan used when applicable.
- `generation_source`: Scan Generated, Manual Correction, Review Resolution.
- `generation_rule_version`: School rule version used.
- `generated_at`: Time current generated status was produced.
- `review_status`: Not Required, Needs Review, In Review, Corrected, Closed.
- `corrected_by`: Reviewer that last corrected the record when applicable.
- `correction_reason`: Business reason for the latest correction.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Attendance Day or Session.
- References zero, one, or two Gate Scan Events.
- May have many Attendance Anomalies.
- May have many Manual Reviews.
- May produce Entry/Exit Notification Records and Audit Events.

**Validation rules**:
- One current attendance record exists per tenant, attendance session, and
  student.
- Normal present, late, and early exit statuses require accepted scan evidence
  unless explicitly corrected by an authorized reviewer.
- Absent status requires membership in the expected population and no accepted
  entry evidence, unless school rules route it to Needs Review.
- Corrections require `attendance_access.attendance.correct`, a reason, and
  audit evidence.

**State transitions**:
- Not Created -> Present, Late, Absent, Early Exit, or Needs Review after
  generation.
- Any generated status -> Corrected when an authorized reviewer changes it.
- Needs Review -> Present, Late, Absent, Early Exit, Excused, or Closed after
  review.

## Entry/Exit Notification Record

**Purpose**: A guardian-facing notification event or suppression record tied to
an entry or exit outcome.

**Fields**:
- `entry_exit_notification_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student whose entry or exit was evaluated.
- `guardian_id`: Guardian evaluated for visibility.
- `guardian_link_id`: Guardian link used for eligibility.
- `gate_scan_event_id`: Related scan event.
- `campus_access_decision_id`: Related access decision.
- `attendance_record_id`: Related attendance record when applicable.
- `direction`: Entry or Exit.
- `eligibility_status`: Eligible, Suppressed, Visible, Attempted, Failed,
  Withdrawn.
- `suppression_reason`: Reason when not eligible.
- `guardian_visible_time`: Time shown to the guardian when visible.
- `channel_reference`: Configured delivery or visibility channel when used.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Profile.
- Belongs to one Guardian Link.
- References a Gate Scan Event and Campus Access Decision.
- May reference an Attendance Record.
- Produces Audit Events for eligibility, suppression, delivery attempt, and
  withdrawal.

**Validation rules**:
- Eligible records require `attendance_access.entry_exit_notifications`, an
  approved active Guardian Link, and access scope that allows entry/exit
  visibility.
- Denied, unresolved, or needs-review scans suppress notification until school
  account rules allow visibility.
- A corrected or rejected scan outcome must update guardian-visible status
  according to school account rules.

## Attendance Anomaly

**Purpose**: A reviewable issue involving scan, campus state, attendance, or
notification evidence.

**Fields**:
- `attendance_anomaly_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Affected student.
- `attendance_session_id`: Related attendance session when applicable.
- `gate_scan_event_id`: Primary related scan event when applicable.
- `attendance_record_id`: Related attendance record when applicable.
- `anomaly_type`: Missing Entry, Missing Exit, Duplicate Scan, Invalid
  Credential, Out Of Order Scan, Conflicting Campus State, Delayed Offline
  Conflict, Late Arrival, Early Exit, Manual Review Required.
- `severity`: Low, Medium, High, Critical.
- `status`: New, Assigned, In Review, Resolved, Dismissed, Escalated, Reopened.
- `assigned_to`: Reviewer assigned when applicable.
- `detected_at`: Detection time.
- `resolution_reason`: Reason supplied at resolution or dismissal.
- `resolved_by`: Reviewer that resolved or dismissed the anomaly.
- `resolved_at`: Resolution time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May reference scan events, attendance records, notification records, and
  manual reviews.
- Produces Audit Events for creation, assignment, resolution, dismissal, and
  reopening.

**Validation rules**:
- Anomalies require enough evidence to identify affected student, tenant,
  anomaly type, severity, and review need.
- Resolved or dismissed anomalies require a reviewer and reason.
- Resolution must not silently change attendance; corrections are separate
  Manual Reviews.

**State transitions**:
- New -> Assigned when routed to a reviewer.
- New or Assigned -> In Review when review starts.
- In Review -> Resolved with a reason.
- New, Assigned, or In Review -> Dismissed with a reason.
- Resolved or Dismissed -> Reopened when later evidence changes the outcome.
- Any open state -> Escalated when school rules require escalation.

## Manual Review

**Purpose**: A reviewer action that resolves, dismisses, corrects, reopens, or
annotates Phase 2 outcomes.

**Fields**:
- `manual_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `target_type`: Gate Scan Event, Attendance Record, Notification Record,
  Attendance Anomaly, or Campus Access Decision.
- `target_reference`: Target entity.
- `review_action`: Resolve, Dismiss, Correct, Reopen, Assign, Annotate.
- `previous_status`: Previous business status when applicable.
- `new_status`: New business status when applicable.
- `reviewer_reference`: Reviewer actor.
- `review_reason`: Business reason.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- May reference any Phase 2 review target.
- Produces Audit Events.

**Validation rules**:
- Review actions require the matching Phase 2 permission.
- Status-changing reviews require a non-empty reason.
- Reviews cannot change records outside the active school account.

## Phase 2 Audit Event

**Purpose**: Reviewable evidence for important Phase 2 activity.

**Fields**:
- `audit_event_id`: Stable identifier.
- `tenant_id`: School account affected.
- `event_category`: Gate, Scan Point, Gate Scan, Campus Access Decision,
  Attendance, Notification, Anomaly, Manual Review, Sync, Feature Capability,
  or Access Decision.
- `event_type`: Specific action name.
- `actor_reference`: Actor responsible for the event when applicable.
- `source_reference`: Device or scan point when applicable.
- `subject_type`: Entity type affected.
- `subject_reference`: Entity affected.
- `previous_value_summary`: Summary of prior state when applicable.
- `new_value_summary`: Summary of new state when applicable.
- `reason`: Business reason or review note.
- `access_decision_reference`: Related permission/access decision when
  applicable.
- `event_time`: Event time.
- `review_status`: Recorded, Reviewed, Escalated, Corrected.

**Relationships**:
- May reference any Phase 2 entity.
- Supports quickstart and compliance review scenarios.

**Validation rules**:
- Sensitive scan, attendance, notification, anomaly, sync, and review events
  must produce audit evidence.
- Audit Events must preserve enough context to identify who acted, what
  changed, which school account was affected, and why.

## School Account Feature Setting

**Purpose**: Tenant capability setting inherited from Phase 0 and used to gate
Phase 2 workflows.

**Fields**:
- `feature_capability_key`: One of `attendance_access.gate_scanning`,
  `attendance_access.attendance_generation`,
  `attendance_access.entry_exit_notifications`, or
  `attendance_access.anomaly_detection`.
- `tenant_id`: Owning school account.
- `availability_status`: Enabled, Disabled, Suspended, or Review Required.
- `effective_from`: When the capability state begins.
- `effective_until`: Optional end time.
- `changed_by`: Actor that changed the setting.
- `change_reason`: Business reason.

**Relationships**:
- Belongs to one School Account.
- Is checked by Phase 2 workflows before business logic executes.
- Produces Audit Events when changed.

**Validation rules**:
- Disabled capabilities deny their related workflow even when the actor has
  permission.
- Capability decisions must be recorded for sensitive denials.
- Feature settings do not grant permissions by themselves.

## Attendance Access Rule Setting

**Purpose**: School account configuration for attendance, notification,
anomaly, scan clock drift, and retry behavior used by Phase 2 workflows.

**Fields**:
- `attendance_access_rule_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `rule_set_name`: Human-readable rule set name.
- `rule_set_status`: Draft, Active, Suspended, Archived.
- `effective_from`: When the rule set begins.
- `effective_until`: Optional end time.
- `attendance_window_rules`: Entry window, late threshold, early-exit threshold,
  and expected session defaults.
- `notification_rules`: Eligibility timing, suppression behavior, corrected
  scan visibility behavior, and guardian-visible time policy.
- `anomaly_rules`: Enabled anomaly types, severity thresholds, escalation
  triggers, and duplicate scan matching window.
- `clock_drift_tolerance`: Maximum tolerated difference between local scan time
  and school account time before review is required.
- `retry_rules`: Idempotency retention, duplicate handling, and retry conflict
  behavior.
- `changed_by`: Actor that changed the setting.
- `change_reason`: Business reason.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Is referenced by Gate Scan Events, Attendance Sessions, Notification Records,
  and Attendance Anomalies when rule decisions are made.
- Produces Audit Events when created, activated, suspended, or archived.

**Validation rules**:
- Only one active rule setting can apply to the same school account and
  effective period.
- Active rule settings require tenant scope, feature availability, permission
  checks, and audit evidence.
- Attendance, notification, anomaly, clock drift, and retry services must record
  the rule version or rule setting used for reviewable decisions.
