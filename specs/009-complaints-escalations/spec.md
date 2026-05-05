# Feature Specification: Phase 8 Complaints & Escalations

**Feature Branch**: `009-complaints-escalations`  
**Created**: 2026-05-06  
**Status**: Reviewed  
**Input**: User description: "Read PLAN.md and create a specification for phase of Complaints & Escalations ONLY."

## Review Status

- Product and engineering review gate: Passed for planning and task generation.
- Implementation may proceed from this specification unless later review changes are recorded.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 8: Complaints & Escalations
- **Feature Module(s)**: Complaint Submission, Complaint Categorization, Escalation Workflow, Feedback & Resolution
- **Tenant Scope**: All complaint records, complaint categories, required intake fields, complaint participants, subject references, assignments, investigation notes, complainant-visible responses, escalation rules, escalation events, resolution records, feedback records, reopen requests, exceptions, review summaries, configuration, and audit evidence belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Complaint submission, complaint categorization, complaint assignment, escalation workflow, feedback and resolution, complaint history, complaint configuration, and complaint review summaries must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, complaint managers, assigned investigators, assigned resolvers, escalation reviewers, safeguarding or safety reviewers where configured, guardians, students, staff submitters, auditors, and reviewers must have explicit permissions for each Phase 8 action. Guardians can submit, view, respond to, or reopen complaints only for students linked to them through an approved active guardian relationship or for complaints they personally submitted. Students can submit or view only their own eligible complaints when school rules allow student submissions. Staff users can act only within their school account and assigned complaint authority. Users named as the complaint subject, involved party, or potential conflict of interest must not decide, close, or view restricted complainant details unless an authorized reviewer explicitly permits a limited review action.
- **Offline/NFC Impact**: Phase 8 does not require NFC, QR, or offline scan behavior. Complaints may reference prior student identity, attendance, transport, wallet, learning, request, or medical evidence where the actor is allowed to see that evidence, but Phase 8 must not create scan events, attendance outcomes, campus access decisions, transport boarding decisions, wallet transactions, learning rewards, request approvals, medical incidents, or emergency access sessions.
- **Observability**: The system must emit reviewable evidence for complaint creation, submission, withdrawal, categorization, reclassification, assignment, reassignment, priority change, confidentiality change, information request, complainant response, internal note, visible response, escalation, missed target, conflict-of-interest detection, resolution proposal, resolution closure, feedback submission, reopen request, dispute, exception creation, manual review, configuration change, summary read, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Submit and Track Complaints (Priority: P1)

As a guardian, student, or school staff member, I need to submit a structured complaint and track its status so concerns are captured through an accountable school process instead of informal messages.

**Why this priority**: Complaint submission is the entry point for the phase and establishes tenant scope, complainant identity, student relationship, category, confidentiality, status, and follow-up ownership.

**Independent Test**: Submit a complaint for an active student in one school account, verify required fields and complainant eligibility are enforced, and confirm the complainant and authorized complaint staff can see the correct status without exposing another school account or unrelated student.

**Acceptance Scenarios**:

1. **Given** complaint submission is enabled and the actor is an authorized guardian, student, or staff member, **When** the actor submits a complaint with the required category, description, involved student or context, urgency, and requested outcome where required, **Then** the complaint is recorded in the correct school account with a tracking reference, initial status, visibility rules, and audit evidence.
2. **Given** the complainant is allowed to track the complaint, **When** the complainant opens the complaint after submission, **Then** they see the current status, category, submitted details, visible responses, next expected school action, and resolution outcome when available without seeing restricted internal notes.
3. **Given** the actor is unauthorized, the guardian link is not approved and active, the student belongs to another school account, complaint submission is disabled, required fields are missing, or the submission is an exact active duplicate, **When** complaint submission is attempted, **Then** the complaint is rejected, blocked, or treated as already submitted with a reviewable reason.

---

### User Story 2 - Categorize, Prioritize, and Assign Complaints (Priority: P1)

As a complaint manager, I need to categorize, prioritize, and assign complaints so each issue follows the correct ownership, confidentiality, target timing, and escalation path.

**Why this priority**: Triage determines who is accountable for the complaint and whether safety, confidentiality, or escalation rules apply.

**Independent Test**: Configure or use an active complaint category, submit a complaint, classify it, assign an owner, and verify category rules determine required fields, priority, confidentiality, target timing, and escalation route.

**Acceptance Scenarios**:

1. **Given** complaint categorization is enabled and the actor is an authorized complaint manager, **When** the actor categorizes or reclassifies a complaint, **Then** the complaint records the category, subcategory where used, priority, confidentiality level, required owner group, target response timing, and reason for any reclassification.
2. **Given** a complaint category requires restricted handling, safety review, safeguarding review, finance review, transport review, or medical privacy review, **When** the complaint is categorized, **Then** visibility and assignment are limited to the allowed role group while preserving a complainant-visible status.
3. **Given** a category is disabled, missing a valid owner group, missing escalation rules, conflicts with feature configuration, or creates a conflict of interest with an assigned user, **When** assignment is attempted, **Then** the complaint is routed to manual review instead of being silently assigned or exposed.

---

### User Story 3 - Investigate, Respond, and Resolve Complaints (Priority: P1)

As an assigned investigator or resolver, I need to review complaint details, request information, record investigation notes, provide complainant-visible responses, and close complaints with resolution evidence so outcomes are traceable and understandable.

**Why this priority**: Resolution is the core business outcome of the phase; a complaint process is incomplete unless authorized staff can act on the complaint and explain the result.

**Independent Test**: Assign a complaint to a resolver, add allowed investigation details, request more information from the complainant, record a visible response, close the complaint with a resolution reason, and verify the complainant sees only the appropriate outcome summary.

**Acceptance Scenarios**:

1. **Given** a complaint is assigned and the actor has resolver authority, **When** the actor records investigation details, requests information, changes status, or adds a complainant-visible response, **Then** each action is preserved with actor, time, visibility level, reason when required, and complaint status impact.
2. **Given** the complaint is ready for closure, **When** the resolver records the resolution type, outcome summary, corrective action where applicable, and closure reason, **Then** the complaint moves to a resolved or closed state and the complainant can view the allowed resolution summary.
3. **Given** the actor is not assigned, lacks permission, is named as the complaint subject, attempts to edit closed evidence, attempts to expose restricted details, or acts across school accounts, **When** they try to investigate or resolve the complaint, **Then** the action is blocked or routed to authorized review without changing preserved complaint evidence.

---

### User Story 4 - Escalate High-Risk, Overdue, or Conflicted Complaints (Priority: P1)

As a school administrator, escalation reviewer, or complaint manager, I need complaints to escalate when they are high-risk, overdue, disputed, unresolved, or conflicted so serious issues are not lost in routine queues.

**Why this priority**: Escalation is explicitly part of Phase 8 and protects students, guardians, staff, and the school from stalled or mishandled complaints.

**Independent Test**: Submit complaints that match high-priority, overdue, disputed, and conflict-of-interest conditions, then verify each complaint reaches the configured escalation owner with preserved reason, timing, and visibility boundaries.

**Acceptance Scenarios**:

1. **Given** escalation workflow is enabled and a complaint meets a configured escalation condition, **When** the condition is reached, **Then** the complaint is escalated to the configured role or reviewer with escalation reason, prior owner, target timing, and audit evidence.
2. **Given** a complaint is marked high-risk, safety-related, safeguarding-related, severe misconduct-related, or externally reportable under school rules, **When** the complaint is triaged or updated, **Then** the complaint is prioritized and routed to the configured urgent review path without exposing restricted details to ordinary staff.
3. **Given** the escalation route is missing, the assigned reviewer is disabled, the complaint involves the assigned reviewer, the complaint is already final, or escalation would expose cross-school data, **When** escalation is evaluated, **Then** the complaint is routed to manual review with a clear exception reason.

---

### User Story 5 - Capture Feedback, Reopen Disputes, and Review History (Priority: P2)

As a complainant, complaint manager, reviewer, or auditor, I need feedback, dispute, reopen, history, and review summary views so the school can measure satisfaction, correct outcomes, and explain the complaint lifecycle.

**Why this priority**: Feedback and review improve trust and governance after submission, categorization, resolution, and escalation are functioning.

**Independent Test**: Resolve a complaint, submit complainant feedback, reopen or dispute an eligible outcome, filter complaint history by status and category, and verify all history remains tenant-scoped and permission-scoped.

**Acceptance Scenarios**:

1. **Given** feedback and resolution are enabled and a complaint has a visible outcome, **When** the complainant submits feedback, acceptance, dissatisfaction, or a reopen request within the configured rules, **Then** the feedback is attached to the complaint and routed to the appropriate owner or reviewer.
2. **Given** complaint history or review summaries are enabled and the actor is authorized, **When** the actor filters by student, complainant, category, priority, status, owner, escalation state, date range, feedback state, exception type, or review state, **Then** only records inside the actor's authorized scope are returned.
3. **Given** an authorized reviewer corrects, reopens, dismisses, or escalates a complaint after closure, **When** they provide the required reason, **Then** the original complaint, original resolution, feedback, corrective action, and current state remain reviewable.

---

### User Story 6 - Configure Complaint Rules (Priority: P3)

As a school administrator, I need configurable complaint categories, required fields, confidentiality rules, assignment groups, target timings, escalation paths, and feedback rules so each school can operate its complaint policy without product changes.

**Why this priority**: Configuration is important for scale and tenant variation, but it can follow after the core complaint, resolution, escalation, and review flows are defined.

**Independent Test**: Create or update a complaint category with required fields, confidentiality, default priority, owner group, target timing, escalation route, and feedback behavior; activate it; submit a complaint; and verify the complaint uses the active configuration while historical complaints retain prior rules.

**Acceptance Scenarios**:

1. **Given** complaint configuration is enabled and the actor is an authorized school administrator, **When** they create or activate a complaint category and escalation rule, **Then** future complaints in that category use the active required fields, confidentiality, assignment, target timing, escalation, and feedback behavior.
2. **Given** a category has no valid owner group, impossible target timing, missing required fields, invalid confidentiality settings, circular escalation, disabled dependent capability, or conflict-of-interest risk that cannot be reviewed, **When** activation is attempted, **Then** the configuration is rejected with a reviewable reason.
3. **Given** complaint rules change after complaints are submitted, **When** old and new complaints are reviewed, **Then** each complaint shows the category and escalation rule version that governed it at the time of the relevant action.

### Edge Cases

- A student attempts to submit a complaint type that the school account allows only guardians or staff to initiate.
- A guardian link is pending, expired, suspended, removed, rejected, restricted, or belongs to another school account.
- A complaint is submitted for an inactive, graduated, transferred, duplicated, or cross-school student profile.
- A complainant submits an exact active duplicate for the same student, category, description, and event window, or submits a non-identical complaint about an overlapping issue.
- A complaint references a transport route, wallet transaction, attendance event, request, medical incident, learning activity, or other prior record that the actor is not allowed to view.
- A complaint category is disabled while related complaints remain open.
- A complaint is submitted without enough information for triage but appears to involve safety, safeguarding, medical, staff misconduct, bullying, transport risk, or financial dispute.
- A complaint names a staff member, resolver, complaint manager, guardian, or student who would normally have access to the complaint queue.
- A complaint involves multiple students, multiple guardians, multiple staff members, or multiple school units with different visibility permissions.
- A complainant withdraws a complaint while investigation, escalation, or resolution review is in progress.
- A resolver is removed from a role, transferred, disabled, or becomes conflicted after assignment.
- Two complaint managers categorize, assign, resolve, or escalate the same complaint at nearly the same time.
- A target response or resolution window expires during school closure, holiday, weekend, or outside configured working hours.
- An urgent complaint requires immediate school safety action, medical response, emergency workflow, external authority contact, or manual protocol outside this phase.
- Complainant feedback disputes a resolution after closure or after the configured reopen window.
- A school administrator changes category, confidentiality, target timing, feedback, or escalation rules while complaints using the old version are still open.
- A user attempts to use complaint status as authorization for attendance, campus gate, transport boarding, wallet refunds, learning rewards, request approval, medical access, broad messaging, document storage, global search, or broad admin dashboard outcomes.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized guardians, students, and school staff to create, submit, view, track, withdraw when allowed, and respond to complaint records within a school account when the relevant complaint capability is enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, actor identity, actor permission, student status, approved guardian link status, complaint category availability, required field completion, confidentiality rules, assignment authority, conflict-of-interest rules, and visibility rules before creating, submitting, categorizing, assigning, responding to, escalating, resolving, reopening, correcting, or showing a complaint.
- **FR-003**: Each complaint record MUST capture school account, complainant, complainant role, primary affected student or context where applicable, participant records for additional students, guardians, staff, or school units where applicable, involved parties, category, subcategory where used, priority, confidentiality level, description, event date or window where applicable, requested outcome, status, owner or queue, target response timing, escalation state, feedback state, source references, exception state, and audit evidence.
- **FR-004**: Complaint submission MUST support guardian-submitted complaints for approved linked students, student-submitted complaints when school rules allow student initiation, and staff-submitted complaints within the staff member's school account and role authority.
- **FR-005**: Complaint categories MUST define school-account rules for who may submit, required intake fields, default priority, confidentiality level, owner group, target response timing, escalation route, feedback behavior, and whether restricted handling or safety review applies.
- **FR-006**: Authorized complaint managers MUST be able to categorize, reclassify, prioritize, assign, and reassign complaints with a recorded reason when the action changes category, priority, confidentiality, owner, target timing, or escalation path.
- **FR-007**: Complaint assignment MUST route work only to authorized owner groups or named users inside the school account and MUST preserve assignment history, current owner, prior owner, assignment reason, and assignment time.
- **FR-008**: Users named as complaint subjects, involved parties, or potential conflicts of interest MUST be prevented from deciding, closing, self-assigning, or viewing restricted complainant details unless an authorized reviewer records a limited exception.
- **FR-009**: Complaint status MUST support at least submitted, received, triage, assigned, in review, waiting for information, escalated, resolved, closed, withdrawn, reopened, dismissed, and manual-review-required states with allowed transitions governed by school rules.
- **FR-010**: The system MUST allow authorized investigators and resolvers to record investigation notes, complainant-visible responses, requested information, received responses, status changes, corrective actions, and resolution proposals while preserving actor, time, visibility level, and reason where required.
- **FR-011**: Internal investigation notes, restricted details, conflict-of-interest review notes, and staff-only corrective action notes MUST be separated from complainant-visible complaint details and outcome summaries.
- **FR-012**: Complaint resolution MUST capture resolution type, outcome summary, corrective action where applicable, closure reason, responsible actor, closure time, complainant visibility, feedback eligibility, reopen eligibility, and preserved complaint history.
- **FR-013**: Feedback and reopen rules MUST allow eligible complainants to submit acceptance, dissatisfaction, satisfaction rating, comment, or reopen request according to the active school-account configuration while preserving the original resolution.
- **FR-014**: Escalation workflow MUST support rule-based and manual escalation for high-priority, safety-related, safeguarding-related, severe misconduct-related, overdue, disputed, unresolved, repeatedly reopened, externally reportable, or conflicted complaints.
- **FR-015**: Escalation events MUST capture escalation reason, trigger source, prior owner, new owner or reviewer, target timing, priority, visibility constraints, outcome, and audit evidence.
- **FR-016**: If a target response or resolution time expires, the system MUST route the complaint to the configured escalation path or manual review and MUST NOT silently close, dismiss, downgrade, or hide the complaint.
- **FR-017**: High-risk complaints that appear to require immediate safety, medical, emergency, safeguarding, or external authority action MUST be routed to the configured urgent review path, but Phase 8 MUST NOT create medical incidents, emergency access sessions, external authority reports, or replace school emergency protocols.
- **FR-018**: The system MUST reject exact active duplicate complaint submissions or return the existing active complaint reference, while overlapping non-identical complaints MUST be linked, grouped, or routed to manual review according to school rules instead of being silently merged or discarded.
- **FR-019**: The system MUST detect and record complaint exceptions, including invalid student, invalid guardian link, missing required field, disabled feature, disabled category, missing owner group, missing escalation route, conflicted owner, stale configuration, duplicate complaint, cross-school access attempt, restricted evidence reference, missed target timing, and manual-review-required condition.
- **FR-020**: Each complaint exception record MUST include school account, affected complaint where applicable, affected student or context where applicable, exception type, severity, source evidence, current status, reviewer assignment where applicable, resolution reason, and resolution history.
- **FR-021**: Complaint history MUST be filterable by student, complainant, guardian, staff submitter, category, priority, confidentiality level, status, owner, escalation state, date range, target timing state, feedback state, exception type, and review state within the user's authorized scope.
- **FR-022**: Complaint category, confidentiality, target timing, assignment, escalation, feedback, and reopen configuration MUST be versioned or revision-traceable so historical complaints show the rules that governed them when submitted, categorized, escalated, resolved, or reopened.
- **FR-023**: Guardian, student, staff, complaint manager, resolver, escalation reviewer, auditor, reviewer, and platform reviewer visibility MUST be permission-scoped so users see only complaint details allowed by school account, approved guardian link, student ownership, complainant ownership, assignment, review authority, or explicit platform-level review authority.
- **FR-024**: The system MUST respect school account feature configuration independently for complaint submission, complaint categorization, complaint assignment, escalation workflow, feedback and resolution, complaint history, complaint configuration, and complaint review summaries.
- **FR-025**: The system MUST provide complaint review summaries by category, priority, status, owner, target timing state, escalation state, feedback state, exception state, date range, student, guardian, staff submitter, and reviewer assignment without exposing records outside the authorized scope.
- **FR-026**: The system MUST make eligible complaint status changes, escalation events, feedback submissions, reopen requests, and reviewable events available to later communication and notification capabilities, but Phase 8 MUST NOT implement general messaging, broadcasts, or notification delivery management.
- **FR-027**: Phase 8 MUST capture complaint statements, structured details, and allowed evidence references, but broad document storage, certificate management, file libraries, and global search belong to later document and search capabilities.
- **FR-028**: The system MUST record audit evidence for complaint creation, submission, withdrawal, categorization, reclassification, assignment, reassignment, priority change, confidentiality change, information request, complainant response, internal note, visible response, escalation, missed target, conflict-of-interest detection, resolution proposal, closure, feedback submission, reopen request, dispute, correction, exception creation, manual review, summary read, configuration change, and access denial.
- **FR-029**: Phase 8 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet refunds or payment actions, learning reward actions, request approval workflows, medical or emergency workflows, broad messaging or broadcasts, document storage workflows, global search, and broad admin dashboards from deliverable scope.
- **FR-030**: Sensitive complaint details MUST be hidden from users without explicit complaint, guardian, student, staff submitter, assignment, audit, review, or platform review authority, including restricted complainant details, internal notes, safety review notes, conflicted-party details, disputed evidence, and inactive or superseded category rules.

### Key Entities *(include if feature involves data)*

- **Complaint**: A tenant-owned issue report submitted by a guardian, student, or staff member, including complainant, primary affected student or context, participant records for additional involved students, guardians, staff, or school units, category, priority, confidentiality, description, status, owner, escalation state, feedback state, and audit history.
- **Complaint Category**: A school-account rule set defining submission eligibility, required fields, default priority, confidentiality, owner group, target timings, escalation path, feedback behavior, and restricted handling.
- **Complaint Participant**: A complainant, affected student, guardian, staff member, school unit, involved party, complaint subject, investigator, resolver, reviewer, or auditor associated with a complaint and governed by visibility rules.
- **Complaint Assignment**: The current and historical ownership record for a complaint, including assigned queue or user, reason, prior owner, assigned time, and conflict-of-interest state.
- **Complaint Investigation Entry**: A preserved note, requested information item, response, visible update, internal finding, corrective action, or status change recorded during complaint handling.
- **Complaint Evidence Reference**: A permitted reference to prior school evidence, such as student identity, attendance, transport, wallet, learning, request, or medical evidence, without making Phase 8 the source of truth for that referenced workflow.
- **Escalation Rule**: A versioned school-account rule describing escalation triggers, target roles, timing, priority changes, confidentiality constraints, and manual review routing.
- **Escalation Event**: A recorded escalation of a complaint, including trigger, reason, prior owner, target owner or reviewer, time, current outcome, and audit evidence.
- **Resolution Record**: The outcome of complaint handling, including resolution type, summary, corrective action where applicable, closure reason, visibility, responsible actor, and feedback eligibility.
- **Complaint Feedback**: A complainant response to a resolution, such as acceptance, dissatisfaction, satisfaction rating, comment, dispute, or reopen request.
- **Complaint Exception**: A reviewable issue involving invalid relationships, missing fields, disabled features, disabled categories, missing owner route, conflicted owner, restricted evidence, duplicates, missed targets, cross-school access, or manual review requirements.
- **Manual Complaint Review**: A reviewer action that corrects, reopens, dismisses, escalates, resolves, or documents a complaint or complaint exception with a reason and preserved history.
- **Complaint Review Summary**: A permission-scoped view of complaint counts, aging, categories, owners, escalation state, feedback state, exceptions, and resolution outcomes.
- **Complaint Status Event**: Tenant-scoped evidence that a complaint status, assignment, escalation, resolution, feedback, or review state changed and may be consumed later by communication or notification capabilities.
- **School Account Feature Setting**: A tenant capability setting that determines whether Phase 8 complaint and escalation workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized guardians, students, or staff can submit a complete eligible complaint in under 2 minutes during review testing.
- **SC-002**: 100% of sampled unauthorized actors, inactive students, unlinked guardians, disabled complaint capabilities, disabled categories, missing required fields, and cross-school student combinations are prevented from creating, viewing, assigning, resolving, escalating, or reopening complaints.
- **SC-003**: Authorized complaint managers can categorize, prioritize, and assign a newly submitted complaint in under 60 seconds during review testing.
- **SC-004**: 100% of sampled complaint category rules correctly determine required fields, confidentiality level, default priority, owner group, target timing, escalation route, and feedback behavior.
- **SC-005**: Assigned investigators or resolvers can find an assigned active complaint and record the next handling action in under 60 seconds during review testing.
- **SC-006**: 100% of sampled conflict-of-interest scenarios prevent conflicted users from self-assigning, deciding, closing, or viewing restricted complainant details.
- **SC-007**: 95% of high-priority, overdue, disputed, unresolved, or conflicted complaint escalation events are visible to the configured escalation owner or reviewer within 1 minute of the escalation condition being recorded.
- **SC-008**: Complainants can find the current status, latest visible response, and allowed resolution outcome for their complaint in under 30 seconds during review testing.
- **SC-009**: 100% of sampled complaint resolutions preserve original complaint details, investigation history, visible response, closure reason, actor, time, feedback eligibility, and current status.
- **SC-010**: Eligible complainants can submit feedback, dissatisfaction, or a reopen request for a resolved complaint in under 60 seconds during review testing.
- **SC-011**: 100% of sampled exact active duplicate complaints are blocked or return the existing active complaint reference, and 100% of sampled overlapping non-identical complaints are linked, grouped, or routed to manual review without being silently merged or discarded.
- **SC-012**: Auditors can trace a sampled complaint from submission through categorization, assignment, investigation, escalation, resolution, feedback, reopen, correction, and review summary in under 60 seconds during review testing.
- **SC-013**: 100% of sampled complaint records are visible only within the authorized school account scope, approved guardian link scope, student ownership scope, complainant ownership scope, assignment scope, review scope, or explicit platform-level review scope.
- **SC-014**: 100% of sampled Phase 8 complaint and escalation actions create no attendance, campus gate, scan, transport, wallet, learning reward, request approval, medical, emergency, document storage, global search, broad messaging, or broad admin dashboard outcome.
- **SC-015**: 95% of eligible complaint status changes, escalation events, feedback submissions, and reopen requests are available to later communication and notification capabilities within 2 minutes without Phase 8 delivering general messages directly.
- **SC-016**: 100% of sampled complaint configuration changes preserve the category, confidentiality, assignment, escalation, feedback, and reopen rule version that governed affected complaints at the time of each relevant action.

## Assumptions

- Phase 8 builds on Phase 0 tenant configuration, feature flag, audit, observability, and shared configuration foundations, and Phase 1 student identity, guardian linking, role, and permission capabilities.
- Complaint submissions are authenticated by default. Anonymous or public intake is outside the baseline Phase 8 scope unless a later specification adds it with explicit safety, abuse prevention, and review rules.
- Students may submit complaints only when the school account allows student-initiated complaints for the selected category; otherwise guardians or staff must initiate.
- Guardian access to student-related complaints requires an approved active guardian link, except where a confidential or safeguarding category intentionally restricts guardian visibility pending authorized school review.
- Complaint categories, owner groups, target response timings, escalation paths, feedback rules, and reopen windows are school-account scoped and versioned.
- Escalation timers use school-configured working calendars when available; otherwise they use calendar time and route exceptions to review when timing cannot be evaluated.
- Complaints may reference prior evidence from other phases only as allowed references. Phase 8 does not alter the source record or become the system of record for attendance, transport, wallet, learning, request, medical, or emergency workflows.
- File uploads, broad document storage, certificate management, and global search belong to Phase 10. Phase 8 captures complaint text, structured details, and permitted references to existing evidence.
- General messaging, broadcasts, and notification delivery belong to Phase 9. Phase 8 records status changes and makes eligible events available to those later capabilities.
- Broad operational dashboards and platform observability consoles belong to Phase 11. Phase 8 includes only complaint-specific review summaries needed to operate this phase.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 8 requirements.
