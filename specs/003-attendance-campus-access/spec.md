# Feature Specification: Phase 2 Attendance & Campus Access

**Feature Branch**: `003-attendance-campus-access`
**Created**: 2026-05-03
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for phase 2: Attendance & Campus Access ONLY."

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 2: Attendance & Campus Access
- **Feature Module(s)**: Gate Scan Flow, Attendance Generation, Entry/Exit Notification, Attendance Anomaly Detection
- **Tenant Scope**: All gates, scan points, scan events, campus access decisions, attendance records, anomaly records, review actions, correction history, and entry/exit notification records belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Gate scanning, attendance generation, entry/exit notifications, and attendance anomaly detection must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, authorized gate staff, attendance reviewers, guardians, and students must have explicit permissions for each Phase 2 action; gate staff can record scans only for authorized school accounts, reviewers can resolve anomalies only within their scope, and guardians can see only approved linked-student entry/exit outcomes.
- **Offline/NFC Impact**: Gate scans using NFC or QR identity evidence must continue when connectivity is unavailable where campus entry or exit continuity depends on scanning. Offline scans must preserve identity evidence, scan direction, scan point, actor or device source, local time, later received time, and reconciliation outcome so duplicate or conflicting scans do not create duplicate attendance.
- **Observability**: The system must emit reviewable evidence for gate scans, denied or flagged access, offline reconciliation, attendance generation, attendance changes, entry/exit notification attempts or suppression, anomaly detection, manual reviews, corrections, and access denials.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Record Campus Entry and Exit Scans (Priority: P1)

As an authorized gate staff member, I need to record student entry and exit
scans at campus gates so the school has trusted evidence of who entered or left
campus and when.

**Why this priority**: Gate scan evidence is the source for secure campus access,
attendance generation, guardian notifications, and anomaly detection.

**Independent Test**: Scan an active student credential at an authorized gate for
entry and exit, then confirm the scan decision, direction, student identity,
school account, scan point, and review evidence are captured without generating
unrelated transport or wallet outcomes.

**Acceptance Scenarios**:

1. **Given** gate scanning is enabled for the school account and the gate staff member is authorized for the scan point, **When** a student presents an active NFC or QR identity credential for entry, **Then** the system records an allowed entry scan with the student, school account, scan point, scan direction, time evidence, and actor or device source.
2. **Given** a student presents an expired, suspended, revoked, replaced, unknown, or cross-school credential, **When** the gate staff member scans it, **Then** the system denies or flags the scan, prevents it from becoming a normal attendance event, and records the reason for review.
3. **Given** connectivity is unavailable at the scan point, **When** authorized gate staff record entry or exit scans, **Then** the scans remain usable for campus access continuity and are later reconciled without losing source, time, or identity evidence.

---

### User Story 2 - Generate Attendance From Gate Evidence (Priority: P1)

As an attendance reviewer, I need attendance to be generated from accepted gate
scan evidence so daily attendance can be reviewed with less manual entry and a
clear chain from scan to attendance status.

**Why this priority**: Automated attendance is the primary operational value of
Phase 2 and depends directly on accepted entry and exit evidence.

**Independent Test**: Capture accepted entry and exit scans for students in a
configured attendance day, then confirm attendance records are produced with
expected statuses and review links back to the scan evidence.

**Acceptance Scenarios**:

1. **Given** attendance generation is enabled and a student has an accepted entry scan within the configured attendance window, **When** attendance is evaluated, **Then** the system creates or updates that student's attendance record with a status such as present or late according to school account rules.
2. **Given** a student has no accepted entry scan for the configured attendance day, **When** attendance is evaluated for the expected student population, **Then** the system marks the student as absent or needing review according to school account rules.
3. **Given** a reviewer corrects an attendance status after validating the evidence, **When** the correction is saved, **Then** the current attendance record shows the corrected status while the original scan-based decision, reviewer, time, and reason remain reviewable.

---

### User Story 3 - Notify Guardians About Entry and Exit (Priority: P2)

As a guardian, I need to receive or view entry and exit updates for my linked
student so I know when the student has arrived at or left campus.

**Why this priority**: Guardian visibility is important for trust and safety, but
it must depend on reliable scans, identity links, and school notification
settings.

**Independent Test**: Capture an eligible entry or exit scan for a student with
an approved active guardian link, then confirm a notification record is created
for the guardian and suppressed for guardians without approved access.

**Acceptance Scenarios**:

1. **Given** entry/exit notifications are enabled and a guardian has an approved active link to the student, **When** the student has an accepted entry scan, **Then** the guardian receives or can view an entry notification with the school account, student, direction, and time.
2. **Given** a guardian link is pending, suspended, expired, removed, rejected, or outside the allowed access scope, **When** the linked student has an entry or exit scan, **Then** the system suppresses the guardian notification and records why it was not eligible.
3. **Given** a scan is later corrected, rejected, or changed after review, **When** the entry/exit notification record is reviewed, **Then** the guardian-facing status reflects the corrected outcome according to school account rules.

---

### User Story 4 - Detect and Resolve Attendance Anomalies (Priority: P3)

As an attendance reviewer, I need the system to identify attendance and campus
access anomalies so unusual or conflicting scan patterns can be reviewed before
they affect student safety records.

**Why this priority**: Anomaly detection improves reliability and auditability,
but it builds on captured scans and generated attendance.

**Independent Test**: Create representative duplicate, missing, invalid,
out-of-order, and conflicting scan cases, then confirm anomaly records are
created with reviewer workflow and resolution history.

**Acceptance Scenarios**:

1. **Given** a student has conflicting entry and exit evidence, **When** anomaly detection runs, **Then** the system creates an anomaly record that identifies the affected student, scan evidence, attendance impact, severity, and required reviewer action.
2. **Given** an anomaly is assigned to a reviewer, **When** the reviewer resolves or dismisses it with a reason, **Then** the anomaly status changes and the resolution remains linked to the attendance record and scan evidence.
3. **Given** a duplicate or retried offline scan is reconciled, **When** the system compares it to existing evidence, **Then** it prevents duplicate attendance while preserving the duplicate scan for review.

---

### Edge Cases

- A gate scan is captured while the scan point is offline and is received later than other scans from the same student.
- The same credential is tapped repeatedly within a short period or retried after a failed confirmation.
- A credential is expired, suspended, revoked, replaced, unknown, or belongs to a different school account.
- A student enters through one authorized gate and exits through another gate or campus in the same school account.
- A scan direction is missing, selected incorrectly, or conflicts with the student's latest known campus state.
- A device or scan point reports a local time that differs from school account time expectations.
- A student has accepted entry evidence but no matching exit evidence, or exit evidence without accepted entry evidence.
- A student arrives late, exits early, or returns after leaving during the same attendance day.
- Attendance generation runs before delayed offline scans are reconciled.
- A manual correction changes the attendance status after guardian notification records were created.
- Guardian notification is disabled, guardian access is inactive, or the guardian link does not allow entry/exit visibility.
- A Phase 2 capability is disabled for a school account while another Phase 2 capability remains enabled.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized users or authorized scan points to record student entry and exit scans for a school account when gate scanning is enabled.
- **FR-002**: Each gate scan MUST capture the school account, student identity evidence, credential type, scan direction, scan point, actor or device source, time evidence, scan decision, and review status.
- **FR-003**: The system MUST validate school account scope, scan point authorization, actor permission, credential status, and feature availability before treating a scan as allowed campus access evidence.
- **FR-004**: The system MUST deny or flag scans from expired, suspended, revoked, replaced, unknown, duplicated, or cross-school credentials and prevent them from creating normal attendance records without review.
- **FR-005**: The system MUST support offline scan capture for entry and exit where continuity is required, then reconcile delayed scans while preserving source evidence and preventing duplicate attendance outcomes.
- **FR-006**: The system MUST detect repeated taps, retries, and delayed duplicate scans so the same physical event does not create multiple attendance records or conflicting campus states.
- **FR-007**: The system MUST generate attendance records from accepted gate scan evidence for configured attendance days and student populations when attendance generation is enabled.
- **FR-008**: Attendance records MUST show the student, school account, attendance day or session, current status, scan evidence used, generation source, review status, and correction history.
- **FR-009**: The system MUST support school account attendance statuses such as present, late, absent, early exit, and needs review according to configured attendance rules.
- **FR-010**: Authorized reviewers MUST be able to correct attendance records, record a reason, and preserve both the original scan-based decision and the corrected outcome.
- **FR-011**: The system MUST create entry and exit notification records for eligible guardians when entry/exit notifications are enabled and the guardian has an approved active link with the required access scope.
- **FR-012**: The system MUST suppress guardian notifications when guardian access is inactive, outside scope, disabled by school account settings, or based on a denied or unresolved scan, and it MUST record the suppression reason.
- **FR-013**: The system MUST detect attendance and campus access anomalies, including missing entry or exit evidence, duplicate scans, invalid credentials, out-of-order scans, conflicting campus states, delayed offline conflicts, late arrivals, early exits, and manual-review-required scans.
- **FR-014**: Each anomaly record MUST include the affected student, school account, related scan or attendance evidence, anomaly type, severity, status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-015**: The system MUST provide reviewers with chronological traceability from a gate scan to any campus access decision, attendance record, guardian notification record, anomaly, review action, and correction.
- **FR-016**: The system MUST respect school account feature configuration independently for gate scanning, attendance generation, entry/exit notifications, and anomaly detection.
- **FR-017**: The system MUST keep all Phase 2 records scoped to the school account and prevent cross-school visibility or action unless an explicit platform-level review role permits it.
- **FR-018**: The system MUST record audit evidence for scan capture, denied or flagged access, offline reconciliation, attendance generation, notification eligibility or suppression, anomaly creation, manual review, correction, and access denial.
- **FR-019**: The system MUST provide attendance and scan review summaries by student, group, gate, attendance day, and anomaly status without exposing data outside the user's authorized school account scope.
- **FR-020**: The system MUST explicitly exclude bus boarding, route tracking, wallet transactions, learning engagement, outings and early leave requests, medical workflows, complaints, general messaging, broadcasts, document management, search, and broad admin dashboards from Phase 2 deliverable scope.
- **FR-021**: The system MUST allow each school account to configure Phase 2 rule settings for attendance windows, late and early-exit thresholds, notification eligibility timing, anomaly detection thresholds, scan clock drift tolerance, and retry handling, with tenant scope, permissions, and audit evidence.

### Key Entities *(include if feature involves data)*

- **Gate**: A school account location where student campus entry or exit may be scanned.
- **Scan Point**: An authorized physical or staff-operated point associated with a gate that records entry or exit scans.
- **Gate Scan Event**: The captured evidence of an NFC or QR identity scan, including direction, time evidence, scan decision, source, offline status, and review status.
- **Campus Access Decision**: The allowed, denied, or needs-review outcome produced from a gate scan.
- **Attendance Day or Session**: The school-defined period for evaluating student attendance from gate scan evidence.
- **Attendance Record**: The student's attendance outcome for an attendance day or session, including current status, evidence, generation source, and correction history.
- **Entry/Exit Notification Record**: A guardian-facing notification event or suppression record tied to an accepted or reviewed entry or exit outcome.
- **Attendance Anomaly**: A reviewable issue involving missing, duplicate, conflicting, invalid, late, early, or delayed scan and attendance evidence.
- **Manual Review**: A reviewer action that resolves, dismisses, or corrects scan, attendance, notification, or anomaly outcomes with a reason and history.
- **Attendance Access Rule Setting**: A school account configuration record that defines attendance, notification, anomaly, clock drift, and retry rules used by Phase 2 workflows.
- **School Account Feature Setting**: A school account capability setting that determines whether Phase 2 workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized gate staff can record a student entry or exit scan in under 10 seconds during normal operating conditions.
- **SC-002**: 100% of sampled expired, suspended, revoked, replaced, unknown, duplicated, and cross-school credentials are denied or flagged without creating normal attendance records.
- **SC-003**: 100% of sampled duplicate, retried, and delayed offline scans are reconciled without creating duplicate attendance records.
- **SC-004**: 95% of accepted entry scans produce or update the expected attendance record within 2 minutes of scan availability.
- **SC-005**: 95% of eligible guardian entry/exit notification records are created or made visible within 2 minutes of the accepted or reviewed campus access outcome.
- **SC-006**: 100% of sampled anomaly records show anomaly type, affected student, related evidence, status, reviewer action when required, and resolution history.
- **SC-007**: Reviewers can trace a sampled scan to its campus access decision, attendance status, guardian notification record, anomaly record, and correction history in under 60 seconds.
- **SC-008**: 100% of sampled Phase 2 records are visible only within the authorized school account scope unless an explicit platform-level review role permits access.

## Assumptions

- Phase 2 builds on Phase 0 foundation rules and Phase 1 identity, credential, guardian link, role, permission, feature configuration, and audit capabilities.
- The school account is the default ownership boundary for gates, scan points, scans, attendance, anomalies, notifications, and review actions.
- School administrators configure attendance days or sessions, gate availability, scan directions, notification eligibility, and anomaly rules for their school account.
- NFC credentials are the primary scan method, and QR fallback may be used only when the school account enables it and the credential is valid under Phase 1 rules.
- Entry/exit notifications in Phase 2 are limited to scan and attendance outcomes for linked guardians; general messaging, broadcasts, and unrelated notification management belong to the later communication phase.
- Manual attendance correction is allowed only for authorized reviewers and must preserve the original scan-based decision.
- Physical gate hardware control is outside Phase 2 unless a later approved spec explicitly adds it; Phase 2 records campus access decisions and evidence.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 2 requirements.
