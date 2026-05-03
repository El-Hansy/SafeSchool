# Phase 2 Research: Attendance & Campus Access

## Decision: Implement Phase 2 as runtime product behavior

**Rationale**: The Phase 2 spec defines operational campus entry/exit scans,
attendance generation, guardian entry/exit notification records, and anomaly
review. These are production workflows with measurable outcomes and must be
implemented as runtime behavior, unlike the Phase 0 foundation package.

**Alternatives considered**:
- Treat Phase 2 as documentation only: rejected because the spec requires
  operational scan capture, attendance outputs, and guardian-visible outcomes.
- Combine Phase 2 with transport boarding: rejected because transport behavior
  belongs to Phase 3 and has different route, bus, and stop concepts.

## Decision: Use the constitution runtime baseline without adding new platforms

**Rationale**: The constitution already defines the technology direction:
ASP.NET Core Web API for backend, PostgreSQL for storage, Next.js/React/
TypeScript for web, and Flutter/Dart for mobile where native NFC/QR and offline
capture are needed. Phase 2 needs offline mobile storage but does not justify a
new service, queue product, or separate database.

**Alternatives considered**:
- Create a campus access microservice: rejected because the modular monolith is
  the default and no measured scale pressure exists.
- Add a broker or external queue for scan events: rejected because idempotent
  sync commands and database-backed reconciliation are sufficient for Phase 2
  scope.
- Use a separate scan database: rejected because tenant-owned scans and
  attendance records fit the single PostgreSQL baseline.

## Decision: Organize implementation around one AttendanceAccess feature area

**Rationale**: Gate scans, campus access decisions, attendance generation,
notification eligibility, anomalies, and review history must share tenant
resolution, credential validation, idempotency, and audit rules. A single
AttendanceAccess feature area with internal modules keeps the workflow cohesive
while avoiding one oversized service.

**Alternatives considered**:
- Split campus access and attendance into unrelated feature roots: rejected
  because attendance status is directly generated from gate scan evidence.
- Put all behavior in one scan service: rejected because gate configuration,
  attendance generation, notification eligibility, and anomaly review have
  separate responsibilities.

## Decision: Reuse Phase 1 identity and credential evidence as the scan input

**Rationale**: Phase 1 defines student profiles, guardian links, NFC credentials,
QR fallback credentials, and credential status snapshots. Phase 2 should consume
that evidence and produce campus/attendance outcomes, not redefine identity or
credential lifecycle.

**Alternatives considered**:
- Rebuild student and credential records inside Phase 2: rejected because it
  duplicates Phase 1 ownership and creates inconsistent identity rules.
- Accept scans without credential status evidence: rejected because expired,
  suspended, revoked, replaced, unknown, and cross-school credentials must be
  denied or flagged.

## Decision: Treat school account as the tenant boundary and gate/campus as internal scope

**Rationale**: Phase 0 established School Account as the tenant boundary.
Gate, campus, group, and scan point references are internal school account
scopes used for filtering, assignment, and review, but they cannot replace the
parent tenant boundary.

**Alternatives considered**:
- Use campus as the top-level tenant for scans and attendance: rejected because
  students may move across campuses inside one school account and school-level
  administration must still review all records.
- Allow global scan lookup by credential reference: rejected because it risks
  exposing cross-school credential existence.

## Decision: Enforce feature availability with explicit Phase 2 capability keys

**Rationale**: The constitution requires backend feature flag enforcement and UI
feature gates. Phase 2 workflows can be enabled independently, because a school
may start with gate scans before automated attendance or guardian notifications.

**Capability keys**:
- `attendance_access.gate_scanning`
- `attendance_access.attendance_generation`
- `attendance_access.entry_exit_notifications`
- `attendance_access.anomaly_detection`

**Alternatives considered**:
- One `attendance_access` flag for every workflow: rejected because schools may
  phase in scanning, attendance, notifications, and anomaly review separately.
- UI-only gating: rejected because backend enforcement must protect tenant
  data and sensitive actions.

## Decision: Use explicit Phase 2 permissions with inherited RBAC

**Rationale**: Phase 1 establishes roles and permission enforcement. Phase 2
must add concrete permission keys for gate staff, attendance reviewers,
notification reviewers, guardian read access, and audit review while evaluating
all permissions inside the active school account.

**Common permission families**:
- `attendance_access.gates.read`
- `attendance_access.gates.manage`
- `attendance_access.scan_points.manage`
- `attendance_access.scans.record`
- `attendance_access.scans.sync`
- `attendance_access.scans.read`
- `attendance_access.attendance.read`
- `attendance_access.attendance.generate`
- `attendance_access.attendance.correct`
- `attendance_access.notifications.read`
- `attendance_access.guardian_entry_exit.read`
- `attendance_access.anomalies.read`
- `attendance_access.anomalies.resolve`
- `attendance_access.audit.read`

**Alternatives considered**:
- Let all staff scan at all gates: rejected because gate staff authorization is
  required by the spec and by tenant safety expectations.
- Allow guardians to read entry/exit records without guardian link scope:
  rejected because Phase 1 guardian visibility depends on an approved active
  link with explicit access scope.

## Decision: Use client scan identity and reconciliation state for idempotent sync

**Rationale**: Offline scans can be retried, delayed, or delivered out of order.
Each scan must carry a caller-stable scan identity and source metadata so the
backend can treat retries as the same physical event, preserve duplicates for
review, and prevent duplicate attendance records.

**Alternatives considered**:
- Deduplicate only by student and timestamp: rejected because device clock
  drift and repeated taps can produce false positives or false negatives.
- Let reviewers manually remove duplicate attendance later: rejected because
  common retries should resolve deterministically and safely.

## Decision: Preserve both local scan time and server received time

**Rationale**: Attendance decisions depend on school time rules, but offline
events may arrive late. Preserving local scan time, received time, scan point
time zone context, and reconciliation outcome allows reviewers to evaluate clock
drift, delayed sync, and conflicting scan order.

**Alternatives considered**:
- Use only server received time: rejected because offline scans would appear
  late or out of order.
- Trust local device time without review signals: rejected because device clock
  drift is an explicit edge case.

## Decision: Generate attendance from accepted scan evidence with reviewable rules

**Rationale**: The spec requires attendance statuses such as present, late,
absent, early exit, and needs review. Generation should apply school account
attendance windows and expected student populations, then preserve the scan
evidence and generation rule used for every status.

**Alternatives considered**:
- Require staff to manually create all attendance records: rejected because the
  Phase 2 objective is automated attendance.
- Generate attendance from denied or unresolved scans: rejected because invalid
  or cross-school credentials must not create normal attendance.

## Decision: Limit Phase 2 notifications to entry/exit notification records

**Rationale**: Phase 2 must notify guardians about entry and exit outcomes, but
the broader messaging and notification system belongs to Phase 9. Phase 2 will
create eligibility, visibility, suppression, and attempt records for entry/exit
events and rely on configured channels where available.

**Alternatives considered**:
- Build a general messaging system in Phase 2: rejected because it belongs to
  Phase 9 and would broaden scope.
- Omit guardian notification records entirely: rejected because Entry/Exit
  Notification is listed as a Phase 2 spec module.

## Decision: Detect anomalies as reviewable records, not automatic corrections

**Rationale**: Missing entry or exit evidence, duplicate scans, invalid
credentials, out-of-order scans, conflicting campus state, delayed offline
conflicts, late arrivals, early exits, and manual-review-required scans can
affect safety and attendance. The system should create anomaly records with
severity and reviewer workflow, while corrections remain explicit review
actions.

**Alternatives considered**:
- Automatically resolve every anomaly: rejected because many cases require
  human review and reason capture.
- Log anomalies without workflow state: rejected because the spec requires
  status, reviewer action, and resolution history.

## Decision: Emit audit events for every sensitive Phase 2 action

**Rationale**: Campus access and attendance affect student safety records.
Allowed, denied, flagged, synced, generated, corrected, suppressed, resolved,
and dismissed outcomes must produce reviewable audit evidence linked to the
actor, tenant, target, reason, and time.

**Alternatives considered**:
- Log only denied scans and errors: rejected because successful scans and
  attendance changes are also safety-relevant.
- Use attendance record history as the only audit source: rejected because
  reviewers need traceability across scans, notifications, anomalies, and
  access denials.

## Decision: Use layered validation and testing by user story

**Rationale**: Each Phase 2 story can be implemented independently while still
including shared tenant, capability, authorization, idempotency, and audit
coverage. Scan validation and attendance rules require lower-level tests, while
reviewer and guardian flows require contract and journey tests.

**Alternatives considered**:
- Test only final end-to-end attendance output: rejected because idempotency,
  offline sync, feature flags, and authorization failures need focused coverage.
- Delay mobile offline tests until transport: rejected because Phase 2 itself
  requires offline entry/exit scan continuity.

## Decision: No unresolved technical clarifications remain

**Rationale**: The Phase 2 spec, constitution, and Phase 0/1 artifacts define
enough scope for planning. Exact package versions, UI component names, and
device adapter choices are implementation details to pin when runtime manifests
and device integrations are created.

**Alternatives considered**:
- Add clarification markers for NFC hardware models: rejected because contracts
  can define scan and sync behavior without selecting a specific reader.
- Add clarification markers for notification channel vendors: rejected because
  Phase 2 is limited to entry/exit notification records and eligibility, while
  general channel management belongs to Phase 9.
