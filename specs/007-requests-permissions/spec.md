# Feature Specification: Phase 6 Requests & Permissions

**Feature Branch**: `007-requests-permissions`
**Created**: 2026-05-04
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for phase 6: Requests & Permissions ONLY."

## Clarifications

### Session 2026-05-04

- Q: When a request requires guardian consent and a student has multiple authorized guardians, whose consent should be enough by default? → A: Any one authorized guardian may satisfy consent by default; schools can require stricter consent per request type.

### Session 2026-05-05

- Q: For star-gated requests that spend stars, when should stars be reserved or consumed? → A: Reserve stars at submission, consume on final approval, and release on denial, withdrawal, or expiry.
- Q: For early leave pickup, what evidence should Phase 6 require by default before staff can treat an approved request as release-eligible? → A: Guardian-selected authorized pickup person plus staff verification note.
- Q: When an approval workflow step reaches its configured expiry time without a decision, what should Phase 6 do by default? → A: Route to manual review or configured escalation while keeping the request pending.
- Q: When a student already has an active request for the same request type and overlapping date/time window, what should Phase 6 do by default? → A: Block exact active duplicates and route overlapping non-identical requests to manual review.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 6: Requests & Permissions
- **Feature Module(s)**: Outing Request, Star-Based Permission Rules, Early Leave Request, Approval Workflow Engine
- **Tenant Scope**: All request records, workflow templates, workflow versions, approver assignments, guardian consent records, early leave evidence, outing details, star-rule snapshots, exception records, corrections, review summaries, and audit evidence belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Outing requests, early leave requests, star-based permission rules, approval workflow engine, request history, workflow configuration, and request review summaries must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, request managers, assigned staff approvers, homeroom or grade staff, guardians, students, attendance or gate staff, auditors, and reviewers must have explicit permissions for each Phase 6 action. Guardians can create, approve, withdraw, or view only requests for students linked to them through an approved active guardian relationship. Students can create, withdraw, or view only their own eligible requests when school rules allow student-initiated requests. Staff users can act only within their school account and assigned authority.
- **Offline/NFC Impact**: Phase 6 does not require NFC, QR, or offline scanning. Approved early leave or outing evidence may be viewed by authorized staff, but Phase 6 must not independently create attendance, campus access, transport, or gate scan outcomes. Any read access to approved release evidence must be tenant-scoped and audit-visible.
- **Observability**: The system must emit reviewable evidence for request creation, submission, withdrawal, approval, denial, information request, escalation, delegation, expiration, correction, reopening, star-rule evaluation, guardian decision, early leave release eligibility changes, workflow template changes, exception creation, manual review, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Submit and Track Permission Requests (Priority: P1)

As a student or guardian, I need to submit outing or permission requests and track their status so school approvals are structured, visible, and not handled through informal messages.

**Why this priority**: Request creation is the entry point for all Phase 6 workflows and must establish tenant scope, requester identity, student eligibility, required details, and status history.

**Independent Test**: Submit an outing or permission request for an active student in one school account, verify required details and requester eligibility are enforced, and confirm the requester and authorized staff can see the correct status without exposing another school account or unrelated student.

**Acceptance Scenarios**:

1. **Given** the request type is enabled and the student or guardian is authorized, **When** they submit an outing or permission request with all required details, **Then** the request is created in the correct school account, enters the configured approval workflow, and becomes visible to authorized participants.
2. **Given** the request is missing required details, the requester is not eligible, the guardian link is not approved and active, the student belongs to another school account, or the request is an exact active duplicate, **When** submission is attempted, **Then** the request is blocked with a reviewable reason and no workflow decision is created.
3. **Given** a request has not reached a final decision, **When** the original requester withdraws it, **Then** the request status changes to withdrawn, pending approval work stops, and the withdrawal reason remains audit-visible.

---

### User Story 2 - Review and Decide Requests with Approval Workflows (Priority: P1)

As an assigned approver, I need to approve, deny, request information, or escalate requests according to the configured workflow so decisions are consistent and traceable.

**Why this priority**: Structured approvals are the core business value of this phase and protect students, guardians, and staff from unclear decision ownership.

**Independent Test**: Configure a simple approval workflow, submit a request, complete each required decision step with assigned approvers, and verify final status, decision history, authorization boundaries, and audit evidence.

**Acceptance Scenarios**:

1. **Given** a request is waiting on an assigned workflow step, **When** an authorized approver approves, denies, or requests information with the required reason or note, **Then** the decision is recorded, the next step or final status is calculated, and the full decision history remains reviewable.
2. **Given** an actor is not assigned to the active step, lacks permission, belongs to another school account, or tries to decide a final request, **When** they attempt a decision, **Then** the action is rejected or returned as already final without changing the approved decision history.
3. **Given** a workflow step expires without a decision, is delegated, or requires escalation, **When** the configured condition is reached, **Then** the request remains pending and is routed to manual review or configured escalation with reviewable evidence explaining why it changed.

---

### User Story 3 - Manage Early Leave Requests (Priority: P1)

As a guardian or authorized staff member, I need early leave requests with pickup and consent evidence so a student can be released only after the school approves the request.

**Why this priority**: Early leave affects student safety and must be controlled before lower-priority history, summaries, and advanced configuration work.

**Independent Test**: Submit an early leave request with student, date, release time, reason, guardian consent, guardian-selected authorized pickup person, and staff verification note; approve it through the workflow; and verify authorized staff can see release eligibility without creating an attendance or gate event.

**Acceptance Scenarios**:

1. **Given** early leave requests are enabled and a guardian or staff user is authorized, **When** they submit a request with date, release time, reason, guardian consent where required, guardian-selected authorized pickup person, and pickup evidence, **Then** the request enters the configured approval workflow.
2. **Given** an early leave request is approved and still valid, **When** authorized attendance, gate, or school staff view it after staff verification is recorded, **Then** they can see release eligibility and pickup evidence allowed by their role, but no attendance, entry, exit, or scan outcome is created by Phase 6.
3. **Given** the guardian is unlinked, the pickup person is not authorized, the staff verification note is missing, the approval has expired, or the request conflicts with school rules, **When** release eligibility is checked, **Then** the request is blocked or marked for review with a clear reason.

---

### User Story 4 - Enforce Star-Based Permission Rules (Priority: P2)

As a school administrator or request manager, I need permission rules based on student stars or rewards so eligible students can receive privileges while insufficient or unavailable star evidence is handled safely.

**Why this priority**: Star-based rules add automated eligibility decisions, but they depend on core request submission and workflow decision behavior.

**Independent Test**: Configure a request type that requires a star threshold or star cost, submit requests for students with sufficient, insufficient, and unavailable star evidence, and verify the correct rule outcome is recorded without inventing or silently changing star balances.

**Acceptance Scenarios**:

1. **Given** star-based permission rules are enabled and a request type requires a star threshold or cost, **When** a student with sufficient eligible star evidence submits the request, **Then** the request records the rule snapshot and proceeds according to the configured approval workflow.
2. **Given** the student has insufficient stars, missing Phase 5 star evidence, suspended reward eligibility, or a disabled star-rule capability, **When** the request is evaluated, **Then** the request is denied, blocked, or marked for manual review according to school rules and no invented star balance is used.
3. **Given** a star rule changes after a request is submitted, **When** the request is reviewed later, **Then** the historical request shows the rule version and star outcome used at submission time.

---

### User Story 5 - Review Request History, Exceptions, and Corrections (Priority: P2)

As a guardian, student, request manager, or auditor, I need request history and exception visibility so decisions can be explained, corrected, and reviewed without hiding the original record.

**Why this priority**: Trustworthy history and correction handling are required after users can submit and decide requests.

**Independent Test**: Filter request history by student, request type, status, date range, approver, and exception state; then correct or reopen an eligible request with a reason and verify both the original and corrective records remain visible to authorized users.

**Acceptance Scenarios**:

1. **Given** request history is enabled and the user is authorized, **When** they search by student, request type, status, date range, approver, star-rule outcome, or exception state, **Then** the results include only records they are allowed to view.
2. **Given** a guardian or student views request history, **When** they are linked to or own the student record, **Then** they can see allowed request details, status, decisions, and correction outcomes without staff-only assignment or internal review details.
3. **Given** an authorized reviewer corrects, reopens, or resolves an exception for a request, **When** they provide the required reason, **Then** the original decision remains preserved and the correction history explains the current outcome.

---

### User Story 6 - Configure Request Rules and Workflow Templates (Priority: P3)

As a school administrator, I need configurable request types, required fields, approver chains, escalation windows, guardian consent requirements, and star rules so each school can operate its approval policies without changing the product.

**Why this priority**: Configuration is important for scale and flexibility, but it can follow after the core request, approval, early leave, and rule evaluation flows are defined.

**Independent Test**: Create a request type and workflow template with required fields, approver roles, escalation timing, guardian consent, and optional star rules; activate it; submit a new request; and verify the request uses the activated version while historical requests keep their original version.

**Acceptance Scenarios**:

1. **Given** workflow configuration is enabled and a school administrator is authorized, **When** they create or activate a request type and workflow template, **Then** future requests of that type use the active version for required fields, approvers, escalation, consent, and star rules.
2. **Given** a workflow template has no valid approver path, circular steps, impossible escalation timing, missing required field definitions, or a star rule without the required star capability, **When** activation is attempted, **Then** the template is rejected with a reviewable reason.
3. **Given** a request type, required field, workflow step, consent rule, or star rule is changed, **When** old and new requests are reviewed, **Then** each request shows the version that governed it at submission time.

---

### Edge Cases

- A student attempts to submit a request type that the school account allows only guardians or staff to initiate.
- A guardian link is pending, expired, suspended, removed, rejected, or belongs to another school account.
- A request is submitted for an inactive, graduated, transferred, duplicated, or cross-school student profile.
- A requester submits an exact active duplicate for the same student, request type, date, and time window, or submits a non-identical request with an overlapping date or time window.
- A request is withdrawn while an approver is viewing or deciding it.
- Two approvers attempt to decide the same active workflow step at nearly the same time.
- An approver is removed from a role, transferred, disabled, or delegated after a request has already reached their step.
- A workflow step expires during school closure, holiday, weekend, or outside configured working hours and must remain pending while routing to manual review or configured escalation.
- A workflow template changes while requests using the old version are still pending.
- A request requires guardian consent but the student has multiple linked guardians with different decision authority, and the request type has a stricter consent rule than the default one-authorized-guardian rule.
- An early leave request is approved after the requested release time has passed.
- An early leave pickup person is missing, duplicated, expired, blocked, no longer approved by the guardian or school, or lacks the required staff verification note.
- An approved outing request reaches the end of its time window without an expected return status or closure.
- Star evidence is delayed, unavailable, stale, corrected, or changed after request submission.
- A star-gated request with a star cost is denied, withdrawn, or expires after stars were reserved at submission and before final approval.
- A star-gated request is submitted before Phase 5 star or reward records are available to the school account.
- A school disables outing requests, early leave requests, star rules, or workflow configuration while related requests are pending.
- A reviewer reopens a final request after a guardian, student, or staff user has already viewed the prior outcome.
- A user attempts to use request status as authorization for attendance, campus gate, transport boarding, wallet purchase, medical access, complaint escalation, or broad notification delivery.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized students, guardians, and school staff to create, save, submit, view, withdraw, and track outing, early leave, and permission request records within a school account when the relevant request capability is enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, student status, requester identity, guardian link status, actor permission, request type eligibility, required field completion, workflow availability, and applicable rule settings before creating, submitting, deciding, withdrawing, correcting, or showing a request.
- **FR-003**: Each request record MUST capture the school account, student, requester, request type, source role, reason, requested date and time window, status, active workflow version, required field responses, related consent status, decision history, current assignee state, exception state, and audit evidence.
- **FR-004**: Outing requests MUST capture destination or purpose, requested departure and return window, supervision or transport expectation when required by the school account, guardian consent when required, approval status, and closure or return status when the request type requires it.
- **FR-005**: Early leave requests MUST capture requested release date and time, reason, guardian consent where required, guardian-selected authorized pickup person, staff verification note before release eligibility, approval status, expiry rules, and release eligibility status.
- **FR-006**: Phase 6 MUST expose approved early leave or outing eligibility to authorized school, attendance, or gate staff as read-only request evidence and MUST NOT directly create attendance records, gate entry or exit events, scan events, transport events, or student location outcomes.
- **FR-007**: The approval workflow engine MUST support configurable request types, required fields, ordered or conditional approval steps, assigned approver roles, guardian consent steps, school staff decision steps, escalation timing, delegation, expiration, final statuses, and manual review routing within a school account. By default, an expired workflow step MUST route the request to manual review or configured escalation while keeping the request pending, and MUST NOT automatically approve or deny the request.
- **FR-008**: Workflow decisions MUST require an authorized actor assigned to the active step or explicitly permitted reviewer role, preserve the decision, reason when required, actor, timestamp, resulting status, and next-step calculation, and prevent direct edits or deletion of approved decision history.
- **FR-009**: The system MUST reject, ignore as already processed, or route to review any duplicate, out-of-order, unauthorized, cross-school, final-state, withdrawn, expired, or concurrently conflicting workflow decision without corrupting the request outcome.
- **FR-010**: The system MUST allow authorized approvers to approve, deny, request more information, delegate, escalate, or mark a request for manual review when the active workflow step and school rules permit the action.
- **FR-011**: The system MUST allow original requesters to withdraw requests only before configured final states or restricted review states, and withdrawal MUST stop pending approval work while preserving all prior request and workflow history.
- **FR-012**: Star-based permission rules MUST be configurable by school account, request type, active date range, eligible student groups where allowed, required star threshold, optional star cost, manual review behavior, and failure behavior when star evidence is unavailable.
- **FR-013**: Star-rule evaluation MUST use Phase 5 star or reward evidence when available, capture the rule version and star outcome used at evaluation time, and MUST NOT invent, silently modify, or assume star balances when Phase 5 evidence is missing, unavailable, stale, or disabled.
- **FR-014**: If a star-gated request has a star cost under school rules, Phase 6 MUST reserve the required stars at submission, consume the reserved stars only on final approval, and release the reservation on denial, withdrawal, or expiry. The Phase 6 record MUST preserve the requested star impact, external star evidence or outcome reference, rule snapshot, reservation status, consumption status, release status, and any failed, pending, reversed, or reviewed result without becoming the source of truth for star balances.
- **FR-015**: Guardian consent requirements MUST distinguish between guardians who may view, submit, approve, deny, or withdraw requests for a linked student. By default, any one authorized guardian may satisfy guardian consent for a request, and schools MAY configure stricter consent rules per request type. Requests requiring guardian consent MUST not proceed past the configured consent point until the required guardian decision is satisfied or the workflow routes to review.
- **FR-016**: Staff and guardian visibility MUST be permission-scoped so guardians see only linked student request details allowed for their role, students see only their own eligible request details, staff see only school-account records allowed by assignment or role, and platform-level reviewers see only records permitted by explicit review authority.
- **FR-017**: Request history MUST be filterable by student, requester, guardian, request type, status, date range, requested time window, approver, current assignee, workflow version, star-rule outcome, exception type, and review status within the user's authorized scope.
- **FR-018**: The system MUST allow authorized reviewers to correct, reopen, close, resolve, or escalate request exceptions with a reason while preserving the original request, original decisions, correction actor, correction time, and resulting status.
- **FR-019**: The system MUST detect and record request exceptions, including missing required consent, invalid guardian link, expired approval, duplicate request, conflicting approvers, out-of-order decision, stale workflow version, missing star evidence, insufficient stars, disabled feature, invalid pickup evidence, cross-school access attempt, and manual-review-required condition.
- **FR-020**: Each exception record MUST include school account, affected request, affected student where applicable, exception type, severity, source evidence, current status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-021**: Request type and workflow configuration MUST be versioned so requests already submitted continue to use the version active at submission time unless an authorized reviewer explicitly migrates or reopens the request with a recorded reason.
- **FR-022**: The system MUST reject activation of request types or workflow templates that have no valid approver path, circular approval steps, impossible escalation timing, missing required fields, invalid guardian consent settings, disabled dependent features, or star rules that cannot be evaluated under the school account's enabled capabilities.
- **FR-023**: The system MUST respect school account feature configuration independently for outing requests, early leave requests, star-based permission rules, approval workflow engine, request history, workflow configuration, and request review summaries.
- **FR-024**: The system MUST make request status changes and reviewable events available to the later communication and notification capabilities, but Phase 6 MUST NOT implement general messaging, broadcasts, or notification delivery.
- **FR-025**: The system MUST record audit evidence for request creation, submission, withdrawal, approval, denial, information request, escalation, delegation, expiration, correction, reopening, star-rule evaluation, guardian decision, early leave release eligibility changes, workflow template changes, exception creation, manual review, and access denial.
- **FR-026**: The system MUST provide request review summaries by student, request type, status, current assignee, approver, guardian, date range, workflow version, exception state, and star-rule outcome without exposing records outside the authorized school account, guardian link, or student scope.
- **FR-027**: Phase 6 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet or payment actions, learning content delivery, star balance ownership, medical or emergency workflows, complaint escalation workflows, broad messaging or broadcasts, document storage workflows, global search, and broad admin dashboards from deliverable scope.
- **FR-028**: The system MUST block exact active duplicates for the same student, request type, requested date, and requested time window. The system MUST route overlapping non-identical active requests for the same student and request type to manual review instead of silently allowing or merging them.

### Key Entities *(include if feature involves data)*

- **Permission Request**: A tenant-owned request for outing, early leave, or another school-defined permission type, including requester, student, reason, requested date and time window, status, workflow version, duplicate or overlap review state, and review history.
- **Outing Request Detail**: Request-specific data for a student leaving campus or participating in an outing, including destination or purpose, departure and return window, supervision or transport expectation, consent needs, and closure status.
- **Early Leave Detail**: Request-specific data for releasing a student before the normal end time, including release time, reason, guardian consent, pickup evidence, expiry, and release eligibility status.
- **Request Type**: A school account configuration record that defines who may initiate a request, required fields, applicable workflow, consent requirements, star rules, expiry behavior, and closure expectations.
- **Workflow Template**: A versioned school account approval definition containing ordered or conditional approval steps, assigned roles, escalation timing, delegation rules, final states, and manual review routes.
- **Workflow Step**: A specific approval, consent, information request, escalation, or review point inside a workflow version.
- **Workflow Decision**: An approval, denial, information request, consent decision, escalation, delegation, or review outcome recorded by an authorized actor for a request step.
- **Guardian Consent Record**: Evidence that an eligible guardian approved, denied, or was required to decide a student request under the default one-authorized-guardian consent rule or a stricter school-configured request type rule.
- **Star Permission Rule**: A versioned rule that determines whether a request needs a star threshold, star cost, manual review, or failure behavior based on Phase 5 star or reward evidence.
- **Star Rule Evaluation**: The captured outcome of applying a star permission rule to a request, including rule version, available evidence, sufficiency, pending or failed state, reservation status, consumption status, release status, and any requested star reservation or consumption outcome reference.
- **Pickup Evidence**: Guardian-selected authorized pickup person, relationship, authorization status, and staff verification note needed before an approved early leave request is treated as release-eligible.
- **Request Exception**: A reviewable issue involving missing consent, invalid guardian link, duplicate request, expired approval, conflicting decision, stale workflow, insufficient stars, unavailable star evidence, invalid pickup evidence, disabled feature, or access denial.
- **Manual Request Review**: A reviewer action that corrects, reopens, resolves, escalates, closes, or documents an exception with reason and history.
- **Request Review Summary**: A permission-scoped view of request counts, pending assignments, final outcomes, exception states, and star-rule outcomes.
- **School Account Feature Setting**: A tenant capability setting that determines whether Phase 6 workflows and request types are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized students or guardians can submit a complete eligible outing or permission request in under 2 minutes during review testing.
- **SC-002**: 100% of sampled inactive students, unauthorized actors, unlinked guardians, missing required fields, disabled request capabilities, and cross-school student combinations are prevented from creating, deciding, viewing, or withdrawing requests.
- **SC-003**: Assigned approvers can find and decide a pending request in under 60 seconds during review testing after the request reaches their active workflow step.
- **SC-004**: 100% of sampled duplicate, out-of-order, unauthorized, withdrawn, expired, concurrent, and final-state workflow decision attempts do not create duplicate final outcomes or corrupt decision history.
- **SC-005**: Approved early leave requests become visible as release eligibility evidence to authorized staff within 1 minute of final approval during review testing, and 100% of sampled approved early leave records create no attendance, gate, or scan event as a Phase 6 side effect.
- **SC-006**: 100% of sampled early leave requests with unlinked guardians, unauthorized pickup evidence, missing staff verification note, expired approvals, invalid students, or disabled capability are blocked or routed to review with a clear reason.
- **SC-007**: Star-based permission outcomes match the configured school rule and available Phase 5 star evidence in 100% of sampled sufficient, insufficient, unavailable, disabled, and corrected star-evidence scenarios.
- **SC-008**: 100% of sampled star-gated requests preserve the rule version and star outcome used at evaluation time, even after the rule or star evidence later changes.
- **SC-009**: Authorized users can find a request from the last 90 days by student, request type, status, date range, approver, workflow version, exception state, or star-rule outcome in under 30 seconds during review testing.
- **SC-010**: 100% of sampled corrections, reopenings, escalations, delegations, and exception resolutions preserve the original request and decision history while showing the corrective actor, reason, time, and resulting status.
- **SC-011**: 100% of sampled request type and workflow template changes are versioned so historical requests show the configuration version that governed them at submission time.
- **SC-012**: 100% of sampled Phase 6 records are visible only within the authorized school account scope, approved guardian link scope, student ownership scope, assigned approver scope, or explicit platform-level review scope.
- **SC-013**: Auditors can trace a sampled request from creation through final decision, exception handling, correction, and review summary in under 60 seconds during review testing.
- **SC-014**: 95% of eligible request status changes are available to later notification capabilities within 2 minutes of the status change without requiring Phase 6 to deliver messages directly.
- **SC-015**: 100% of sampled expired workflow steps route to manual review or configured escalation while keeping the request pending and creating no automatic approval or denial.
- **SC-016**: 100% of sampled exact active duplicate requests are blocked, and 100% of sampled overlapping non-identical active requests are routed to manual review without being silently merged, approved, or withdrawn.

## Assumptions

- Phase 6 builds on Phase 0 tenant configuration, feature flag, audit, and observability capabilities, and Phase 1 student identity, guardian linking, role, and permission capabilities.
- Star-based permission rules depend on Phase 5 star and reward evidence when the school account enables them. If Phase 5 star evidence is unavailable, Phase 6 records the request as blocked, denied, or manual-review-required according to school rules rather than creating its own star balance.
- Star-gated requests with a star cost reserve stars at submission, consume reserved stars only on final approval, and release reservations when requests are denied, withdrawn, or expired.
- Students may initiate requests only for request types where the school account explicitly allows student initiation; otherwise guardians or staff must initiate.
- Guardian consent defaults to any one authorized guardian satisfying consent for a request. School accounts can configure stricter consent rules per request type, and all guardian actions require an approved active guardian link to the student.
- Early leave release eligibility requires a guardian-selected authorized pickup person plus a staff verification note by default; this does not require Phase 6 to capture government ID images or process NFC or QR release scans.
- Expired workflow steps do not automatically approve or deny requests by default; they keep requests pending and route them to manual review or configured escalation.
- Exact active duplicates are requests for the same student, request type, requested date, and requested time window while the earlier request is still pending, under review, approved but not expired, or otherwise not final for duplicate-checking purposes. Overlapping non-identical active requests are routed to manual review by default.
- Early leave and outing request approval is evidence for staff review only in this phase; attendance, gate, transport, scan, and notification systems may consume the status in later phases but are not implemented by Phase 6.
- Request workflow configuration is school-account scoped and versioned, with historical requests retaining the version active at submission time.
- General messaging, broadcasts, and notification delivery belong to Phase 9. Phase 6 only makes status changes and reviewable events available to those later capabilities.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 6 requirements.
