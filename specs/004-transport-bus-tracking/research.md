# Phase 3 Research: Transport & Bus Tracking

## Decision: Implement Phase 3 as runtime product behavior

**Rationale**: The Phase 3 spec defines operational route planning, student
transport assignments, active trips, boarding/drop scans, live location
visibility, ETA records, guardian notification records, anomaly review, and
retention. These are production workflows with measurable safety and visibility
outcomes.

**Alternatives considered**:
- Treat Phase 3 as documentation only: rejected because the spec requires
  active trip operations, guardian-visible outcomes, and scan/tracking evidence.
- Fold Phase 3 into attendance: rejected because campus attendance generation
  and campus entry/exit decisions are explicitly outside Phase 3.

## Decision: Use the constitution runtime baseline without adding new platforms

**Rationale**: The constitution already defines ASP.NET Core Web API for
backend, PostgreSQL for storage, Next.js/React/TypeScript for web, and
Flutter/Dart for mobile where native NFC/QR, offline capture, and location
updates are required. Phase 3 does not justify a new service, queue product,
dedicated tracking hardware service, or separate location database.

**Alternatives considered**:
- Create a transport microservice: rejected because the modular monolith is the
  default and no measured scale pressure exists.
- Add a broker for live location and scan events: rejected because idempotent
  commands and database-backed reconciliation are sufficient for the planning
  scope.
- Use a separate location store: rejected because 30-day detailed retention and
  school-account scoped summaries fit the single PostgreSQL baseline.

## Decision: Organize implementation around one Transport feature area

**Rationale**: Routes, vehicles, assignments, active trips, scans, live
tracking, ETA, notifications, anomalies, rules, and audit evidence share tenant,
feature, permission, student identity, guardian visibility, and trip-state
rules. A single Transport feature area with internal modules keeps ownership
cohesive while avoiding one oversized service.

**Alternatives considered**:
- Split route planning, scanning, and tracking into unrelated roots: rejected
  because scans, location, ETA, and notifications all depend on the same active
  trip and assignment context.
- Put all transport behavior into one service: rejected because route planning,
  assignments, trip lifecycle, scan sync, ETA, notifications, anomalies, and
  retention have separate responsibilities.

## Decision: Reuse Phase 1 identity, credential, and guardian evidence

**Rationale**: Phase 1 owns student profiles, guardian links, NFC credentials,
QR fallback credentials, credential status, roles, and permissions. Phase 3
should consume that evidence for transport decisions and guardian visibility,
not redefine identity or credential lifecycle.

**Alternatives considered**:
- Duplicate student, guardian, and credential records inside Transport:
  rejected because it creates inconsistent identity ownership.
- Accept transport scans without credential status evidence: rejected because
  expired, suspended, revoked, replaced, unknown, duplicated, and cross-school
  credentials must be denied or flagged.

## Decision: Keep Phase 2 attendance and campus access out of Phase 3 outcomes

**Rationale**: Phase 3 may reference Phase 2 context for separation and review
traceability, but boarding/drop scans must not generate campus attendance,
campus entry/exit decisions, or physical access outcomes.

**Alternatives considered**:
- Generate attendance from bus boarding: rejected because the Phase 3 spec
  explicitly excludes attendance generation and campus access decisions.
- Reuse gate scan entities for transport scans: rejected because transport
  scans require route, trip, stop, assignment, and onboard/drop states that
  differ from campus gate direction and attendance state.

## Decision: Treat school account as the tenant boundary and routes/stops as internal scope

**Rationale**: Phase 0 established School Account as the tenant boundary.
Routes, stops, buses, trips, and staff assignments are internal school account
scopes used for filtering and review; they must not replace the parent tenant
boundary.

**Alternatives considered**:
- Use route as a tenant boundary: rejected because school administrators and
  transport managers must review multiple routes within one school account.
- Allow global credential or trip lookup: rejected because it risks exposing
  cross-school student or credential existence.

## Decision: Enforce feature availability with explicit Phase 3 capability keys

**Rationale**: The constitution requires backend feature flag enforcement and UI
feature gates. Phase 3 workflows can be enabled independently because schools
may start with routes and assignments before live tracking, ETA, or guardian
notifications.

**Capability keys**:
- `transport.bus_assignment`
- `transport.route_stop_management`
- `transport.live_tracking`
- `transport.boarding_drop_scans`
- `transport.eta_calculation`
- `transport.notifications`

Supporting anomaly review and retention behavior are governed by the relevant
scan, tracking, ETA, notification, and rule-setting capabilities.

**Alternatives considered**:
- One `transport` flag for every workflow: rejected because schools may phase
  in route setup, scanning, tracking, ETA, and notifications separately.
- UI-only gating: rejected because backend enforcement must protect tenant data
  and sensitive transport actions.

## Decision: Use explicit Phase 3 permissions with inherited RBAC

**Rationale**: Phase 1 establishes role and permission enforcement. Phase 3 must
add concrete permission keys for transport managers, route administrators,
drivers, attendants, supervisors, reviewers, guardians, and audit users while
evaluating permissions inside the active school account.

**Common permission families**:
- `transport.vehicles.read`
- `transport.vehicles.manage`
- `transport.routes.read`
- `transport.routes.manage`
- `transport.assignments.read`
- `transport.assignments.manage`
- `transport.trips.read`
- `transport.trips.start`
- `transport.trips.update`
- `transport.trips.end`
- `transport.scans.record`
- `transport.scans.sync`
- `transport.scans.read`
- `transport.location.submit`
- `transport.tracking.read`
- `transport.eta.read`
- `transport.eta.calculate`
- `transport.notifications.read`
- `transport.guardian_visibility.read`
- `transport.anomalies.read`
- `transport.anomalies.resolve`
- `transport.rules.manage`
- `transport.audit.read`

**Alternatives considered**:
- Let all staff record scans for all trips: rejected because trip staff/device
  authorization is required for safety and tenant isolation.
- Allow guardians to read transport status without guardian link scope:
  rejected because Phase 1 guardian access requires an approved active link
  with explicit visibility scope.

## Decision: Live tracking uses authorized staff or vehicle mobile devices

**Rationale**: The Phase 3 clarification selected an authorized staff or
vehicle mobile device associated with the active trip as the authoritative live
tracking source. This matches the mobile NFC/offline platform already required
for scans and avoids dedicated bus hardware dependency.

**Alternatives considered**:
- Require dedicated GPS devices on every bus: rejected because it introduces
  hardware dependency and operational cost not required by the spec.
- Support both dedicated bus GPS and mobile devices in Phase 3: rejected
  because dedicated hardware can be planned later if measured operational need
  appears.
- Defer continuous location entirely: rejected because Live Tracking and ETA
  Calculation are Phase 3 modules.

## Decision: Allow route overlap but block bus and tracking device overlap

**Rationale**: The same route may need multiple active trips, such as parallel
buses or makeup runs. A bus or authorized tracking device must not be attached
to more than one active trip, or location and scan evidence could be assigned
to the wrong trip.

**Alternatives considered**:
- Allow only one active trip per route: rejected because schools may operate
  multiple buses on one route.
- Allow multiple active trips per bus with manual staff selection: rejected
  because it increases evidence mis-assignment risk.
- Make overlap rules fully tenant-configurable: rejected because the one-bus
  and one-device active trip rule is a safety boundary.

## Decision: Use caller-stable scan and batch identity for idempotent sync

**Rationale**: Offline transport scans can be retried, delayed, or delivered
out of order. Each scan and batch must carry caller-stable identity and source
metadata so retries are treated as the same physical event, duplicates remain
reviewable, and duplicate transport outcomes are prevented.

**Alternatives considered**:
- Deduplicate only by student, trip, and timestamp: rejected because clock
  drift and repeated taps can produce false matches.
- Let reviewers clean up duplicate transport status later: rejected because
  common retries should resolve deterministically and safely.

## Decision: Preserve invalid assignment scans as needs-review anomalies

**Rationale**: A scan for an unassigned student, wrong route, or wrong stop is
important safety evidence. It must be recorded as a needs-review anomaly while
normal boarding/drop status and guardian notifications are withheld until
reviewer approval.

**Alternatives considered**:
- Reject the scan outright: rejected because the school would lose evidence of
  a possible student safety event.
- Accept the scan as normal and create an anomaly: rejected because it could
  mislead guardians and staff before review.
- Make the behavior tenant-configurable: rejected because consistent safety
  review is required before normal status is granted.

## Decision: Preserve local event time, received time, and route context

**Rationale**: Transport decisions depend on route order, trip state, and school
time rules, but offline events may arrive late. Preserving local scan time,
received time, source, route, trip, stop, direction, and reconciliation outcome
allows reviewers to evaluate clock drift, delayed sync, and conflicting scan
order.

**Alternatives considered**:
- Use only received time: rejected because offline scans would appear late or
  out of order.
- Trust local device time without review signals: rejected because device clock
  drift is an explicit edge case.

## Decision: Calculate ETA from active trip progress and approved route sequence

**Rationale**: ETA records should exist only for active trips with approved
routes, ordered stops, non-overlapping bus/device assignment, and sufficiently
current trusted progress. Stale, untrusted, or inconsistent progress must mark
ETA as unavailable or needing review.

**Alternatives considered**:
- Calculate ETA from planned stop times only: rejected because live tracking is
  intended to reflect real trip progress and delays.
- Always show an estimate even when location is stale: rejected because the spec
  requires stale or unsupported estimates to be unavailable or reviewable.

## Decision: Limit guardian exact live location to the onboard interval

**Rationale**: The Phase 3 clarification selected pickup ETA and trip status
before accepted boarding, exact live bus location after accepted boarding and
before accepted or reviewed drop, and drop status after drop. This gives
guardians useful visibility when their linked student is onboard while reducing
exposure before and after that interval.

**Alternatives considered**:
- Show no exact live location to guardians: rejected because it weakens the
  Live Tracking value for linked-student transport visibility.
- Show exact live location for the full trip: rejected because guardians should
  not see exact bus movement before their linked student boards or after drop.
- Make exact location visibility fully configurable: rejected because the
  clarified default is a privacy boundary that downstream tasks can validate.

## Decision: Retain detailed location history for 30 days

**Rationale**: Detailed active-trip location history is useful for incident
investigation but creates student movement privacy exposure. Thirty days gives
schools a review window while retaining only trip summaries and audit evidence
afterward unless school policy places an approved review hold.

**Alternatives considered**:
- Keep detailed location only during active trips: rejected because incident
  investigations may occur after the trip.
- Retain detailed location for 90 days or an academic year: rejected because it
  increases privacy exposure without a Phase 3 requirement.

## Decision: Limit Phase 3 notifications to transport notification records

**Rationale**: Phase 3 must notify guardians about transport events, but the
broader messaging and notification system belongs to Phase 9. Phase 3 will
create eligibility, visibility, suppression, and attempt records for boarding,
drop, delay, ETA, and reviewed transport outcomes and rely on configured
channels where available.

**Alternatives considered**:
- Build a general messaging system in Phase 3: rejected because it belongs to
  Phase 9 and would broaden scope.
- Omit guardian notification records entirely: rejected because Transport
  Notification is listed as a Phase 3 module.

## Decision: Detect transport anomalies as reviewable records

**Rationale**: Missed boarding, missed drop, wrong route, wrong stop, duplicate
scan, invalid credential, out-of-order scan, delayed offline conflict, route
deviation, stale location, delayed trip, and manual-review-required events can
affect student safety. The system should create anomaly records with severity
and workflow state, while corrections remain explicit review actions.

**Alternatives considered**:
- Automatically resolve every anomaly: rejected because many cases require
  human review and reason capture.
- Log anomalies without workflow state: rejected because the spec requires
  status, reviewer action, and resolution history.

## Decision: Emit audit events for every sensitive Phase 3 action

**Rationale**: Transport operations affect student safety and guardian trust.
Route changes, assignments, trip lifecycle, accepted/denied scans, sync
reconciliation, location acceptance or suppression, ETA changes, notification
eligibility or suppression, anomaly review, manual correction, retention, and
access denial must produce audit evidence linked to actor, tenant, target,
reason, and time.

**Alternatives considered**:
- Log only denied scans and errors: rejected because successful transport
  events are also safety-relevant.
- Use trip history as the only audit source: rejected because reviewers need
  traceability across assignments, scans, locations, ETAs, notifications,
  anomalies, corrections, and access denials.

## Decision: Use layered validation and testing by user story

**Rationale**: Each Phase 3 story can be implemented independently while still
including shared tenant, capability, authorization, idempotency, guardian
visibility, retention, and audit coverage. Route and assignment rules require
focused tests, while mobile scan/tracking and guardian flows require contract
and journey tests.

**Alternatives considered**:
- Test only full end-to-end transport flow: rejected because overlap rules,
  idempotency, offline sync, feature flags, guardian visibility, and
  authorization failures need focused coverage.
- Delay mobile offline tests until later phases: rejected because Phase 3
  itself requires offline boarding/drop scan continuity.

## Decision: No unresolved technical clarifications remain

**Rationale**: The Phase 3 spec, clarifications, constitution, and prior phase
artifacts define enough scope for planning. Exact package versions, UI component
names, map display choices, notification channel vendors, and device adapter
choices are implementation details to pin when runtime manifests and device
integrations are created.

**Alternatives considered**:
- Add clarification markers for map provider selection: rejected because
  contracts can define visibility, freshness, and privacy behavior without
  selecting a map provider.
- Add clarification markers for notification channel vendors: rejected because
  Phase 3 is limited to transport notification records and eligibility, while
  general channel management belongs to Phase 9.
