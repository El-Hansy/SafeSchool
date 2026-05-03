# Feature Specification: Phase 3 Transport & Bus Tracking

**Feature Branch**: `004-transport-bus-tracking`
**Created**: 2026-05-04
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for phase 3: Transport & Bus Tracking ONLY."

## Clarifications

### Session 2026-05-04

- Q: What live tracking source should Phase 3 treat as authoritative for active bus location? → A: Authorized staff or vehicle mobile device during active trip.
- Q: How should Phase 3 handle boarding or drop scans for a student without an active assignment or on the wrong route or stop? → A: Record as needs-review anomaly and withhold normal status until reviewer approval.
- Q: How long should Phase 3 retain detailed active-trip location history? → A: 30 days, then summaries and audit evidence under school policy.
- Q: What active-trip overlap rule should Phase 3 enforce? → A: Multiple active trips per route allowed; one active trip per bus and tracking device.
- Q: What live-location visibility should guardians receive for a linked student's active trip? → A: Pickup ETA before boarding, exact live bus location after boarding, and drop status after drop-off.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 3: Transport & Bus Tracking
- **Feature Module(s)**: Bus Assignment, Route & Stop Management, Live Tracking, Boarding/Drop Scan, ETA Calculation, Transport Notification
- **Tenant Scope**: All buses, routes, stops, student transport assignments, active trips, boarding and drop scan events, location updates, ETA records, transport notification records, anomaly records, review actions, corrections, and transport rule settings belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Bus assignment, route and stop management, live tracking, boarding and drop scans, ETA calculation, and transport notifications must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, transport managers, bus supervisors, drivers, attendants, authorized staff, guardians, students, and reviewers must have explicit permissions for each Phase 3 action. Guardians can see only transport visibility and notifications for students linked to them through an approved active guardian relationship.
- **Offline/NFC Impact**: Boarding and drop scans using NFC or QR identity evidence must continue when connectivity is unavailable where transport continuity depends on scanning. Offline scans must preserve identity evidence, route, trip, stop, direction, actor or device source, local time, later received time, and reconciliation outcome so duplicate or conflicting scans do not create duplicate transport events.
- **Observability**: The system must emit reviewable evidence for route and stop changes, bus assignments, student transport assignments, trip start and end actions, authorized mobile location update acceptance or suppression, boarding and drop scans, ETA changes, notification attempts or suppression, anomaly detection, manual review, correction, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Manage Routes and Stops (Priority: P1)

As a transport manager, I need to define school bus routes, ordered stops, and service windows so daily transport operations have an approved plan that staff, guardians, and reviewers can trust.

**Why this priority**: Routes and stops are the foundation for assignments, active trips, ETAs, scans, notifications, and review history.

**Independent Test**: Create a route with ordered pickup and drop stops for a school account, activate it, update one stop, and confirm the active plan and prior values remain reviewable.

**Acceptance Scenarios**:

1. **Given** route and stop management is enabled for a school account and the transport manager is authorized, **When** they create a route with ordered stops, service direction, planned timing, and active status, **Then** the route is available only within that school account with review history.
2. **Given** an active route requires a stop sequence or timing change, **When** an authorized user updates the route, **Then** the current route plan reflects the approved change and the prior plan remains reviewable.
3. **Given** a route is inactive or outside the user's authorized school account, **When** a user attempts to assign students or start trips from it, **Then** the system blocks the action and records the reason.

---

### User Story 2 - Assign Students to Buses and Stops (Priority: P1)

As a school administrator or transport manager, I need to assign students to approved routes, buses, and pickup or drop stops so each student's transport plan is clear and guardian visibility is based on an authorized assignment.

**Why this priority**: Student transport assignments connect identity, guardian visibility, routes, and trip operations. Without assignments, boarding and drop scans cannot be evaluated reliably.

**Independent Test**: Assign an active student to an active route and stops for a date range, then confirm the assignment is tenant-scoped, visible to authorized reviewers, and unavailable to unrelated guardians or school accounts.

**Acceptance Scenarios**:

1. **Given** an active student profile, approved transport route, and enabled bus assignment capability, **When** an authorized user creates a student transport assignment, **Then** the assignment includes route, stop, direction, validity dates, visibility state, and review evidence.
2. **Given** a guardian has an approved active link to the assigned student, **When** the guardian views the student's transport plan and transport visibility is enabled, **Then** the guardian can see only the transport details allowed by the school account and link scope.
3. **Given** a student profile is inactive, the route is inactive, the assignment dates are invalid, or the target route belongs to another school account, **When** a user attempts the assignment, **Then** the system prevents activation and records the reason.

---

### User Story 3 - Record Boarding and Drop Scans (Priority: P1)

As a bus attendant or authorized transport staff member, I need to record student boarding and drop scans during trips so the school has trusted evidence of each student's transport status.

**Why this priority**: Boarding and drop evidence is the primary safety record for Phase 3 and feeds guardian notifications, anomalies, and trip review.

**Independent Test**: Start an active trip, scan an assigned student's active NFC or QR identity credential at boarding and drop, and confirm accepted events are captured with trip, route, stop, student, direction, source, and review status without generating campus attendance.

**Acceptance Scenarios**:

1. **Given** boarding and drop scans are enabled, a trip is active, and the staff member is authorized for that trip, **When** a student presents an active NFC or QR identity credential for boarding, **Then** the system records an accepted boarding event with the student, route, trip, stop, direction, time evidence, and source.
2. **Given** a student presents an expired, suspended, revoked, replaced, unknown, duplicated, or cross-school credential, **When** transport staff scan it, **Then** the system denies or flags the scan, prevents it from becoming a normal transport event, and records the reason for review.
3. **Given** a student has no active assignment for the trip or is scanned on the wrong route or stop, **When** transport staff record the boarding or drop scan, **Then** the system records a needs-review anomaly, withholds normal transport status and guardian notification, and requires reviewer approval before the event can be treated as accepted.
4. **Given** connectivity is unavailable during the trip, **When** authorized staff record boarding or drop scans, **Then** the scans remain usable for transport continuity and are later reconciled without losing source, time, or identity evidence.

---

### User Story 4 - Track Active Bus Trips (Priority: P2)

As a transport manager or eligible guardian, I need visibility into active bus trip progress so I can understand whether a student route is running, delayed, stale, or complete.

**Why this priority**: Live tracking provides operational and guardian value after routes, assignments, and scan evidence are established.

**Independent Test**: Start a trip for an active route, accept current location updates from an authorized mobile tracking device, and confirm authorized staff see trip progress while guardians see only linked-student transport visibility.

**Acceptance Scenarios**:

1. **Given** live tracking is enabled and a trip is active, **When** current location updates are received from an authorized mobile tracking device, **Then** the system shows the trip's latest accepted progress to authorized viewers within their school account scope.
2. **Given** location updates are stale, outside the active trip, from an untrusted source, or outside the authorized school account, **When** a user requests trip progress, **Then** the system suppresses or marks the location as unavailable and records the reason.
3. **Given** a route already has an active trip, **When** an authorized transport user starts another trip for the same route with a different bus and tracking device, **Then** the system allows the second trip and keeps scan, location, ETA, notification, and anomaly evidence tied to the correct trip.
4. **Given** a bus or tracking device is already associated with an active trip, **When** a user attempts to start another active trip with the same bus or device, **Then** the system blocks the new trip and records the reason.
5. **Given** a guardian is linked to one student assigned to an active trip, **When** the student has not yet boarded, **Then** the guardian sees pickup ETA and trip status without exact live bus location.
6. **Given** the linked student has an accepted boarding event and no accepted or reviewed drop event, **When** the guardian views transport progress, **Then** the guardian sees the exact live bus location for that student's active trip and not other students or unrelated routes.
7. **Given** the linked student has an accepted or reviewed drop event, **When** the guardian views transport progress, **Then** the guardian sees drop status and no longer sees exact live bus location.

---

### User Story 5 - Calculate ETAs for Stops and Students (Priority: P2)

As a guardian or transport reviewer, I need estimated arrival information for upcoming route stops so delays and pickup or drop expectations are visible without manual calls.

**Why this priority**: ETA calculation improves transport transparency, but it depends on active routes, trip progress, and trusted location evidence.

**Independent Test**: Use an active trip with an approved route, upcoming stops, and current trip progress, then confirm ETA records are produced or marked unavailable with a clear reason.

**Acceptance Scenarios**:

1. **Given** ETA calculation is enabled, the route has ordered stops, and the active trip has current trusted progress, **When** the system evaluates upcoming stops, **Then** each eligible upcoming stop receives an ETA with freshness and confidence state.
2. **Given** location evidence is stale or route progress is inconsistent, **When** ETA is requested, **Then** the system marks ETA as unavailable or needing review rather than presenting an unsupported estimate.
3. **Given** a student is assigned to an upcoming stop and the guardian has approved visibility, **When** ETA changes materially, **Then** the guardian-facing ETA reflects the current reviewed or accepted state according to school account rules.

---

### User Story 6 - Notify Guardians About Transport Events (Priority: P3)

As a guardian, I need transport notifications for my linked student's boarding, drop, delay, and ETA changes so I can respond to important transport events without seeing unrelated student information.

**Why this priority**: Guardian notification builds trust and reduces manual communication, but it must depend on verified assignments, accepted scans, guardian links, and school settings.

**Independent Test**: Capture eligible boarding, drop, delay, and ETA events for a student with an approved active guardian link, then confirm notification records are created for eligible guardians and suppressed for guardians without valid scope.

**Acceptance Scenarios**:

1. **Given** transport notifications are enabled and a guardian has an approved active link to the student, **When** the student has an accepted boarding event, drop event, delay, or material ETA change, **Then** the system creates or makes visible a transport notification record for that guardian.
2. **Given** a guardian link is pending, suspended, expired, removed, rejected, or outside the allowed access scope, **When** the linked student has a transport event, **Then** the system suppresses guardian notification and records why it was not eligible.
3. **Given** a transport event is later corrected, rejected, or changed after review, **When** the notification record is reviewed, **Then** the guardian-facing status reflects the corrected outcome according to school account rules.

---

### Edge Cases

- A route has multiple stops with similar names, shared pickup locations, or different pickup and drop sequences.
- A bus is substituted after a trip starts, or a driver, supervisor, or attendant changes during an active trip.
- A student boards the wrong bus, boards at the wrong stop, drops at the wrong stop, or attempts to board without an active assignment.
- The same credential is tapped repeatedly within a short period or retried after a failed confirmation.
- A credential is expired, suspended, revoked, replaced, unknown, or belongs to a different school account.
- A boarding or drop scan is captured while the transport device is offline and is received later than other events from the same trip.
- A delayed offline boarding scan arrives after a drop scan has already been recorded for the same student and trip.
- A trip starts late, ends early, skips a stop, returns to a prior stop, or deviates from the planned route.
- Location evidence becomes stale, unavailable, inconsistent with the route, or comes from an untrusted source.
- ETA is requested for a student without an active assignment, without an upcoming stop, or after the trip has ended.
- Guardian notification is disabled, guardian access is inactive, or the guardian link does not allow transport visibility.
- A Phase 3 capability is disabled for a school account while another Phase 3 capability remains enabled.
- A manual correction changes a boarding, drop, ETA, anomaly, or notification outcome after guardians have already viewed the prior status.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized users to create, view, update, deactivate, and review bus routes and ordered stops within a school account when route and stop management is enabled.
- **FR-002**: The system MUST allow authorized users to create, view, update, deactivate, and review buses or transport vehicles within a school account when bus assignment is enabled.
- **FR-003**: The system MUST allow authorized users to assign active students to approved routes, buses or planned trips, pickup stops, drop stops, service direction, visibility state, and validity dates.
- **FR-004**: The system MUST validate student profile status, guardian link status, identity credential status, route status, school account scope, actor permission, and feature availability before activating assignments, accepting scans, showing live tracking, calculating ETA, or creating transport notification records.
- **FR-005**: The system MUST allow authorized transport staff to start, update, and end active trips for an approved route, service direction, planned stops, assigned bus, assigned staff, and authorized mobile tracking device.
- **FR-006**: The system MUST allow multiple active trips for the same route only when each active trip has a different assigned bus and authorized mobile tracking device, and it MUST block overlapping active trips that reuse the same bus or tracking device.
- **FR-007**: Each boarding or drop scan MUST capture the school account, student identity evidence, credential type, trip, route, stop, direction, actor or device source, time evidence, scan decision, and review status.
- **FR-008**: The system MUST support offline boarding and drop scan capture where continuity is required, then reconcile delayed scans while preserving source evidence and preventing duplicate transport outcomes.
- **FR-009**: The system MUST deny or flag scans from expired, suspended, revoked, replaced, unknown, duplicated, or cross-school credentials and prevent them from creating normal boarding or drop outcomes without review.
- **FR-010**: The system MUST record scans for students without an active assignment or with a route or stop mismatch as needs-review transport anomalies, and it MUST withhold normal boarding or drop status and guardian notifications until reviewer approval.
- **FR-011**: The system MUST accept active-trip location updates only from an authorized staff or vehicle mobile device associated with the active trip and only for authorized school accounts and active trips.
- **FR-012**: The system MUST suppress or mark as unavailable any location visibility that is stale, untrusted, outside the active trip, outside the authorized school account, or inconsistent with the trip's review state.
- **FR-013**: Guardian-facing live-location visibility MUST be limited to pickup ETA and trip status before the linked student's accepted boarding event, exact live bus location after accepted boarding and before accepted or reviewed drop, and drop status after accepted or reviewed drop.
- **FR-014**: The system MUST calculate or update ETA records for upcoming route stops and linked-student transport visibility when ETA calculation is enabled and sufficient trusted trip progress exists.
- **FR-015**: ETA records MUST show the trip, route, stop, affected students when applicable, estimated arrival state, freshness, confidence, and review status.
- **FR-016**: The system MUST create transport notification records for eligible guardians when transport notifications are enabled and an accepted or reviewed transport event qualifies under school account rules.
- **FR-017**: The system MUST suppress guardian transport notifications when guardian access is inactive, outside scope, disabled by school account settings, based on a denied or unresolved event, or based on stale or untrusted tracking evidence, and it MUST record the suppression reason.
- **FR-018**: The system MUST detect transport anomalies, including missed boarding, missed drop, wrong route, wrong stop, duplicate scan, invalid credential, out-of-order scan, delayed offline conflict, route deviation, stale location, delayed trip, and manual-review-required events.
- **FR-019**: Each transport anomaly MUST include the affected student when applicable, school account, related route, trip, stop, scan or location evidence, anomaly type, severity, status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-020**: Authorized reviewers MUST be able to correct boarding, drop, trip, ETA, anomaly, and notification outcomes, record a reason, and preserve both the original evidence and corrected outcome.
- **FR-021**: The system MUST allow each school account to configure Phase 3 rule settings for assignment eligibility, pickup and drop windows, route deviation thresholds, location staleness, ETA change thresholds, notification eligibility, anomaly detection, scan clock drift tolerance, and retry handling, with tenant scope, permissions, and audit evidence.
- **FR-022**: The system MUST respect school account feature configuration independently for bus assignment, route and stop management, live tracking, boarding and drop scans, ETA calculation, and transport notifications.
- **FR-023**: The system MUST keep all Phase 3 records scoped to the school account and prevent cross-school visibility or action unless an explicit platform-level review role permits it.
- **FR-024**: The system MUST record audit evidence for route and stop changes, bus changes, assignment changes, trip lifecycle actions, scan capture, denied or flagged scans, offline reconciliation, location update acceptance or suppression, ETA changes, notification eligibility or suppression, anomaly creation, manual review, correction, and access denial.
- **FR-025**: The system MUST provide transport review summaries by student, route, bus, trip, stop, scan status, location status, ETA state, notification status, and anomaly status without exposing records outside the user's authorized school account or guardian link scope.
- **FR-026**: The system MUST retain detailed active-trip location history for 30 days, then keep only trip summaries and audit evidence according to school account policy.
- **FR-027**: The system MUST explicitly exclude campus attendance generation, campus entry or exit decisions, wallet transactions, learning engagement, outings and early leave requests, medical workflows, complaints, general messaging, broadcasts, document management, search, broad admin dashboards, and physical vehicle control from Phase 3 deliverable scope.

### Key Entities *(include if feature involves data)*

- **Bus or Transport Vehicle**: A school-owned transport resource used for approved routes and active trips, including lifecycle state and review history.
- **Transport Route**: A school account route definition with service direction, active state, planned timing, and review history.
- **Transport Stop**: A pickup or drop location that can be included in one or more route plans within the school account.
- **Route Stop Sequence**: The ordered stop plan for a route and direction, including planned timing and active status.
- **Student Transport Assignment**: The relationship between a student and a route, bus or planned trip, pickup stop, drop stop, visibility state, and validity period.
- **Transport Trip**: A dated run of a route with assigned bus, assigned staff, authorized mobile tracking device, planned stops, live state, start and end evidence, and review status.
- **Boarding/Drop Scan Event**: Captured NFC or QR identity evidence for a student boarding or dropping from a trip, including decision, source, timing, offline status, and review status.
- **Transport Location Update**: Accepted or suppressed trip progress evidence tied to an active trip and an authorized staff or vehicle mobile device, with guardian-facing exact location limited to the interval after accepted boarding and before accepted or reviewed drop for the linked student.
- **ETA Record**: An estimated arrival state for a trip stop or linked-student transport view, including freshness, confidence, and review status.
- **Transport Notification Record**: A guardian-facing notification event or suppression record tied to an eligible transport event.
- **Transport Anomaly**: A reviewable issue involving missing, duplicate, invalid, conflicting, delayed, stale, deviated, or manual-review-required transport evidence.
- **Manual Transport Review**: A reviewer action that resolves, dismisses, or corrects transport scan, trip, ETA, notification, or anomaly outcomes with a reason and history.
- **Transport Rule Setting**: A school account configuration record that defines Phase 3 assignment, scan, tracking, ETA, notification, anomaly, clock drift, and retry rules.
- **School Account Feature Setting**: A school account capability setting that determines whether Phase 3 workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized transport managers can create one active route with at least five ordered stops and assign ten students to it in under 15 minutes during review testing.
- **SC-002**: 100% of sampled inactive routes, inactive students, invalid assignment dates, unauthorized users, and cross-school route or student combinations are prevented from activating transport assignments.
- **SC-003**: Authorized transport staff can record a student boarding or drop scan in under 10 seconds during normal operating conditions.
- **SC-004**: 100% of sampled expired, suspended, revoked, replaced, unknown, duplicated, and cross-school credentials are denied or flagged without creating normal boarding or drop outcomes.
- **SC-005**: 100% of sampled duplicate, retried, and delayed offline boarding or drop scans are reconciled without creating duplicate transport outcomes.
- **SC-006**: 95% of accepted boarding and drop scan events are visible to authorized reviewers and eligible guardians within 2 minutes of scan availability.
- **SC-007**: 95% of accepted active-trip location updates are visible or reflected as current progress to authorized viewers within 30 seconds of update availability, while stale or untrusted updates are marked unavailable or suppressed.
- **SC-008**: 95% of eligible active trips with approved routes, non-overlapping bus and tracking device assignment, and current trusted progress have ETA records available for upcoming stops within 60 seconds of trip progress availability.
- **SC-009**: 95% of eligible guardian transport notification records are created or made visible within 2 minutes of the accepted or reviewed transport event.
- **SC-010**: 100% of sampled transport anomaly records show anomaly type, affected student or trip, related evidence, status, reviewer action when required, and resolution history.
- **SC-011**: Reviewers can trace a sampled active trip to its route, stops, student assignments, boarding and drop scan events, location visibility, ETA records, notification records, anomaly records, and correction history in under 60 seconds.
- **SC-012**: 100% of sampled Phase 3 records are visible only within the authorized school account scope or approved guardian link scope unless an explicit platform-level review role permits access.
- **SC-013**: 100% of sampled guardian transport views show no exact live bus location before accepted boarding or after accepted or reviewed drop for the linked student.
- **SC-014**: 100% of sampled active-trip location records older than 30 days retain only trip summaries and audit evidence unless school account policy requires a longer approved review hold.

## Assumptions

- Phase 3 builds on Phase 0 foundation rules, Phase 1 identity, credential, guardian link, role, permission, feature configuration, and audit capabilities, and may reference Phase 2 campus context without generating attendance or campus access outcomes.
- The school account is the default ownership boundary for buses, routes, stops, assignments, trips, scans, location evidence, ETAs, notifications, anomalies, and review actions.
- School administrators and transport managers configure buses, routes, stops, assignments, transport rules, and guardian transport visibility unless the school account grants specific staff permissions.
- NFC credentials are the primary scan method for boarding and drop evidence, and QR fallback may be used only when the school account enables it and the credential is valid under Phase 1 rules.
- Live tracking depends on an authorized staff or vehicle mobile device associated with an active trip; dedicated bus hardware and physical vehicle control are outside Phase 3.
- ETA is calculated only for active trips with an approved route, planned stops, and sufficiently current trusted trip progress; otherwise ETA is unavailable or marked for review.
- Guardian-facing exact live bus location is available only between the linked student's accepted boarding event and accepted or reviewed drop event; before boarding, guardians see pickup ETA and trip status, and after drop-off, guardians see drop status.
- Transport notifications in Phase 3 are limited to route, trip, boarding, drop, delay, ETA, and reviewed transport outcomes for linked guardians; general messaging and broadcasts belong to the later communication phase.
- Manual transport correction is allowed only for authorized reviewers and must preserve the original evidence.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 3 requirements.
