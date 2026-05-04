# Feature Specification: Phase 7 Medical & Emergency

**Feature Branch**: `008-medical-emergency`  
**Created**: 2026-05-05  
**Status**: Draft  
**Input**: User description: "Read PLAN.md and create a specification for phase of Medical & Emergency ONLY."

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 7: Medical & Emergency
- **Feature Module(s)**: Medical Record, Emergency Access, Medical Incident Logging, Medical Notification
- **Tenant Scope**: All student medical profiles, medical conditions, allergy records, medication instructions, care plans, emergency contacts, guardian medical consent records, emergency access sessions, break-glass events, medical incidents, care actions, medical notification requests, contact attempts, acknowledgements, exceptions, manual reviews, rule settings, and audit evidence belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Medical records, emergency access, medical incidents, medical notifications, medical history, medical configuration, and medical review summaries must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, school nurses, clinic staff, medical coordinators, emergency-authorized staff, teachers with limited emergency view, transport or gate staff with emergency view where configured, guardians, students, auditors, and reviewers must have explicit permissions for each Phase 7 action. Students can access only allowed own medical summary details. Guardians can access only allowed records for students linked to them through an approved active guardian relationship. Staff users can act only within their school account and assigned medical, emergency, class, trip, gate, transport, reviewer, or administrative authority.
- **Offline/NFC Impact**: Phase 7 may consume identity evidence from prior NFC or QR identity capabilities to locate a student during an emergency, but it does not provision NFC cards, create scan events, generate attendance, decide campus access, or manage transport boarding. If a school enables emergency offline access, cached critical medical data must show freshness, be limited to minimum necessary details, and create reviewable access evidence that syncs when connectivity returns.
- **Observability**: The system must emit reviewable evidence for medical record creation, medical record update, guardian-submitted update, consent change, emergency access, break-glass access, emergency access denial, medical incident creation, severity escalation, care action logging, medication administration evidence, guardian or contact notification request, contact attempt, acknowledgement, failed contact, incident correction, incident closure, configuration changes, summary reads, manual review, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Manage Student Medical Records (Priority: P1)

As a school nurse or authorized medical staff member, I need to maintain student medical records, conditions, allergies, medication instructions, care plans, emergency contacts, and guardian consent so the school has accurate health information before incidents occur.

**Why this priority**: Medical records are the source of truth for emergency access, incident handling, notifications, and guardian visibility.

**Independent Test**: Create a medical profile for an active student in one school account, add condition, allergy, medication instruction, care plan, emergency contact, and guardian consent details, then verify authorized medical staff and linked guardians see only their allowed views while unrelated users and other school accounts cannot discover or open the record.

**Acceptance Scenarios**:

1. **Given** medical records are enabled and the actor has medical record management permission, **When** the actor creates or updates a student's medical profile with required medical summary, allergies, medication instructions, care plan, emergency contacts, consent state, and effective dates, **Then** the record is saved as tenant-scoped, permission-scoped medical evidence with audit history.
2. **Given** a guardian has an approved active link and school policy allows guardian medical visibility, **When** the guardian views the student's medical information, **Then** the guardian sees only allowed profile, condition, allergy, care plan, medication instruction, consent, and contact details without staff-only notes or cross-school data.
3. **Given** a medical record is expired, superseded, archived, disputed, disabled by feature configuration, or belongs to another school account, **When** a user attempts to view or update it, **Then** access is blocked, hidden, or routed to review according to role, consent, tenant scope, and school rules.

---

### User Story 2 - Access Critical Medical Data During Emergencies (Priority: P1)

As emergency-authorized school staff, I need rapid, accountable access to critical medical information during a health or safety emergency so I can follow school-approved care instructions and contact the right people.

**Why this priority**: Emergency access is the core safety workflow for this phase and must work even when normal medical staff are not immediately available.

**Independent Test**: Start an emergency access session for an active student, provide the required reason, view critical allergies, medication instructions, care plan, emergency contacts, and latest relevant incidents, and verify the session is time-bounded, minimum-necessary, tenant-scoped, and audit-visible.

**Acceptance Scenarios**:

1. **Given** emergency access is enabled and the actor has emergency access authority, **When** the actor opens a student's emergency profile with a reason and emergency context, **Then** the actor sees only critical medical details, emergency contacts, and active care instructions needed for immediate response.
2. **Given** an urgent emergency occurs and the actor lacks ordinary medical record permission but has configured break-glass authority, **When** the actor confirms emergency need and provides a reason, **Then** temporary access is granted, marked for mandatory review, and fully auditable.
3. **Given** the actor is unauthorized, the student is outside the school account, the guardian link is invalid, the emergency feature is disabled, the cached profile is stale, or no reason is provided, **When** emergency access is requested, **Then** access is denied or routed to the school's manual emergency protocol without exposing unrelated records.

---

### User Story 3 - Log Medical Incidents and Care Actions (Priority: P1)

As a nurse, clinic staff member, teacher, or emergency-authorized staff member, I need to log medical incidents, observed symptoms, care actions, medication administration evidence, and follow-up status so the school can preserve a complete and reviewable incident history.

**Why this priority**: Incident logging turns emergency activity into evidence for guardians, reviewers, school leadership, and future care decisions.

**Independent Test**: Log a medical incident for an active student, record severity, location, observed details, care actions, medication administration evidence, guardian contact attempt, follow-up requirement, and closure status, then verify the incident remains tenant-scoped and preserves all updates.

**Acceptance Scenarios**:

1. **Given** medical incident logging is enabled and the actor is authorized for the student or emergency context, **When** the actor logs an incident with required severity, location, observation, care action, and follow-up fields, **Then** the incident is attached to the correct student and school account with reviewable evidence.
2. **Given** an incident requires escalation, medication administration, emergency services handoff, guardian contact, or later follow-up, **When** those actions are recorded, **Then** each action is preserved in the incident timeline with actor, time, reason, and visibility rules.
3. **Given** an incident is duplicated, corrected, disputed, recorded for the wrong student, missing required evidence, logged by an unauthorized actor, or belongs to another school account, **When** it is submitted or reviewed, **Then** it is rejected, treated as already processed, corrected, or routed to manual review without losing original evidence.

---

### User Story 4 - Manage Medical Notifications and Acknowledgements (Priority: P2)

As authorized medical or emergency staff, I need medical notification requests, contact attempts, and acknowledgement tracking so guardians, emergency contacts, and required school staff can be informed according to urgency and school policy.

**Why this priority**: Notifications are critical for emergency response, but they depend on accurate medical records and incident evidence.

**Independent Test**: Create a medical notification request from a high-severity incident, verify the correct guardian, emergency contact, and school staff audience is selected, record contact attempts and acknowledgements, and confirm sensitive details are minimized and no broad messaging or broadcast workflow is created.

**Acceptance Scenarios**:

1. **Given** medical notifications are enabled and an incident or emergency access session requires contact, **When** authorized staff creates or confirms the notification request, **Then** the request records audience, urgency, privacy-safe content summary, contact priority, and acknowledgement requirements.
2. **Given** a notification cannot be delivered automatically or the recipient does not acknowledge, **When** staff records manual call, alternate contact, failed contact, or delayed acknowledgement, **Then** the contact history remains visible to authorized reviewers and emergency staff.
3. **Given** the notification contains sensitive medical information, targets an invalid guardian link, references a closed incident, duplicates an existing active notification, or attempts broad broadcast behavior, **When** it is evaluated, **Then** restricted details are minimized, invalid recipients are blocked, duplicates are handled idempotently, and broad messaging is prevented.

---

### User Story 5 - Review Medical History, Exceptions, and Access Audits (Priority: P3)

As a school administrator, medical coordinator, guardian, reviewer, or auditor, I need medical history, exception review, configuration, and access audit views so the school can explain outcomes and govern sensitive health workflows.

**Why this priority**: History and governance are essential for trust and compliance, but they are most useful after records, emergency access, incidents, and notification evidence exist.

**Independent Test**: Search medical history for a student, review emergency access sessions and incidents, correct an eligible incident with a reason, configure medical visibility rules, and verify all results remain permission-scoped and tenant-scoped.

**Acceptance Scenarios**:

1. **Given** medical history or review summaries are enabled and the user is authorized, **When** the user filters by student, condition, allergy, medication instruction, care plan, incident severity, emergency access event, notification state, date range, exception type, or review state, **Then** only records inside the user's authorized scope are returned.
2. **Given** a medical record, emergency access session, incident, care action, notification request, or acknowledgement needs correction or review, **When** an authorized reviewer provides the required reason, **Then** the original evidence remains preserved and the current outcome explains the correction.
3. **Given** a school administrator changes medical visibility, emergency access, break-glass, notification, consent, or incident rules, **When** old and new records are reviewed, **Then** records show the rule version or configuration that governed them at the time of the event.

---

### Edge Cases

- A student is transferred, suspended, withdrawn, graduated, or duplicated while active medical instructions or incidents exist.
- A guardian link is pending, expired, suspended, removed, rejected, restricted, or belongs to another school account.
- A nurse, teacher, transport supervisor, gate staff member, or emergency-authorized actor is removed, transferred, disabled, or assigned after records already exist.
- A medical condition, allergy, care plan, medication instruction, or consent record expires during an emergency.
- Guardian-provided medical details conflict with nurse-entered or school-verified records.
- A student is unconscious, unable to identify themselves, or does not have an available NFC or QR identity during an emergency.
- Emergency access is requested while the device is offline, has stale cached emergency data, or cannot sync the access audit immediately.
- Two staff members log the same medical incident or care action at nearly the same time.
- Medication administration is recorded with missing consent, expired instructions, wrong dosage evidence, or conflicting care plan guidance.
- Emergency contacts are unreachable, invalid, duplicated, out of order, or not authorized for medical details.
- A high-severity incident requires guardian contact, emergency services handoff, school leadership awareness, or later follow-up.
- A guardian disputes an incident, medication action, notification content, or emergency access event.
- A user attempts to use medical status as authorization for attendance, campus gate, transport boarding, wallet purchase, learning reward, request approval, complaint escalation, broad messaging, document storage, or global search.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized school medical staff to create, update, view, archive, and review student medical profiles when medical records are enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, student status, guardian link status, actor permission, medical role, emergency authority, visibility rules, consent state, and applicable medical rule settings before creating, changing, showing, notifying, or reviewing a Phase 7 record.
- **FR-003**: Medical profile records MUST capture school account, student, medical summary, conditions, allergies, medication instructions, care plans, emergency contacts, consent state, visibility rules, effective dates, review status, source evidence, and audit evidence.
- **FR-004**: The system MUST allow linked guardians to view school-approved medical details, submit medical update information when allowed, and see allowed incident or notification outcomes without exposing staff-only notes or unrelated student records.
- **FR-005**: Guardian-submitted medical updates MUST route to authorized school medical review before becoming school-verified medical evidence unless school policy explicitly allows direct guardian-maintained fields.
- **FR-006**: Emergency access sessions MUST require tenant scope, enabled emergency access capability, eligible student identity, actor authority or break-glass authority, emergency reason, access purpose, minimum-necessary data view, time-bounded access, and audit evidence.
- **FR-007**: Break-glass emergency access MUST require an explicit emergency confirmation and reason, preserve the actor, student, viewed data category, time, and context, and route the access event to mandatory review.
- **FR-008**: Critical emergency profile views MUST include only active and relevant allergies, medication instructions, care plans, restrictions, emergency contacts, and recent medical incident context allowed by school rules.
- **FR-009**: If emergency offline access is enabled, cached critical medical data MUST show freshness, limit visible data to emergency essentials, prevent unrelated record browsing, and create syncable access evidence when connectivity returns.
- **FR-010**: The system MUST allow authorized staff to log, view, update, correct, close, and review medical incidents when medical incident logging is enabled.
- **FR-011**: Medical incident records MUST capture school account, student, actor, incident type, severity, location or context, observed details, time, care actions, medication administration evidence where applicable, contact attempts, follow-up requirements, status, visibility rules, and audit evidence.
- **FR-012**: Care action records MUST preserve action type, actor, time, reason, medication or treatment evidence when applicable, guardian or contact involvement, follow-up state, correction history, and link to the parent incident.
- **FR-013**: Medication administration evidence MUST require an active medication instruction or authorized override reason and MUST NOT create diagnosis, prescription, pharmacy, wallet, or payment outcomes.
- **FR-014**: Medical notification requests MUST capture source incident or emergency access context, urgency, audience, contact priority, privacy-safe content summary, acknowledgement requirements, delivery or manual contact state, and audit evidence.
- **FR-015**: Medical notifications MUST minimize sensitive details for each recipient role and MUST NOT create broad messaging, broadcast, complaint, request approval, or general notification-management workflows.
- **FR-016**: Contact attempts and acknowledgements MUST preserve recipient, contact route category, attempt time, outcome, acknowledgement state, actor when manually recorded, failure reason when applicable, and source incident or emergency access reference.
- **FR-017**: The system MUST detect and record medical exceptions, including invalid student, inactive student, expired medical instruction, conflicting medical record, missing consent, missing emergency reason, duplicate incident, duplicate notification, failed contact, stale emergency cache, disabled feature, cross-school access attempt, and manual-review-required condition.
- **FR-018**: Each medical exception record MUST include school account, affected student where applicable, affected medical record, exception type, severity, source evidence, current status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-019**: Medical history MUST be filterable by student, guardian, condition, allergy, medication instruction, care plan, emergency access event, incident severity, care action, medication evidence, notification state, contact acknowledgement, date range, actor, exception type, and review state within the user's authorized scope.
- **FR-020**: The system MUST allow authorized reviewers to correct, reopen, close, resolve, dismiss, or escalate medical records, incidents, notification requests, emergency access reviews, and exceptions with a reason while preserving original evidence.
- **FR-021**: Medical visibility, emergency access, break-glass, consent, incident severity, notification audience, contact priority, and review configuration MUST be versioned or revision-traceable so historical records show the rule or configuration that governed them when the event occurred.
- **FR-022**: Staff, nurse, clinic staff, emergency-authorized staff, guardian, student, auditor, reviewer, and platform reviewer visibility MUST be permission-scoped so users see only records allowed by school account, approved guardian link, student ownership, medical assignment, emergency authority, reviewer assignment, or explicit platform review authority.
- **FR-023**: The system MUST respect school account feature configuration independently for medical records, emergency access, medical incidents, medical notifications, medical history, medical configuration, and medical review summaries.
- **FR-024**: The system MUST provide medical review summaries by student, condition, allergy, care plan, incident severity, emergency access state, notification acknowledgement state, exception state, date range, and reviewer assignment without exposing records outside the authorized scope.
- **FR-025**: The system MUST make eligible medical incident changes, emergency access events, notification requests, acknowledgement changes, and reviewable events available to later communication and notification capabilities, but Phase 7 MUST NOT implement general messaging, broadcasts, or notification delivery management.
- **FR-026**: The system MUST record audit evidence for medical profile creation, profile update, guardian update submission, consent change, emergency access, break-glass access, emergency access denial, medical incident creation, care action logging, medication administration evidence, notification request, contact attempt, acknowledgement, failed contact, incident correction, incident closure, exception creation, manual review, summary read, configuration change, and access denial.
- **FR-027**: Phase 7 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet or payment actions, learning reward actions, request approval workflows, complaint escalation workflows, broad messaging or broadcasts, document storage workflows, global search, and broad admin dashboards from deliverable scope.
- **FR-028**: Exact duplicate active medical update submissions, emergency access retries, medical incidents, care actions, notification requests, contact attempts, and review actions MUST be rejected or treated as already processed, while overlapping or conflicting non-identical records MUST be routed to manual review instead of being silently merged or discarded.
- **FR-029**: Phase 7 MUST NOT generate medical diagnoses, prescribe treatment, change medication instructions without authorized human input, or replace school emergency protocols; it records, exposes, and routes approved evidence and actions.
- **FR-030**: Sensitive medical details MUST be hidden from users without explicit medical, emergency, guardian, student, audit, or review authority, including staff-only notes, restricted guardian details, disputed records, and inactive or superseded instructions.

### Key Entities *(include if feature involves data)*

- **Student Medical Profile**: A school-account record containing the student's medical summary, visibility rules, active care context, guardian visibility, review state, and audit history.
- **Medical Condition Record**: A condition, diagnosis label, health concern, or care-relevant status recorded by authorized sources with effective dates, severity, visibility, and review status.
- **Allergy Record**: A care-critical allergy or sensitivity record with severity, reaction notes, exposure guidance, effective dates, and emergency visibility.
- **Medication Instruction**: A school-held medication or administration instruction with dosage evidence, schedule, authorization state, expiry, override rules, and audit history.
- **Care Plan**: A school-approved plan describing care instructions, restrictions, emergency steps, staff notes, guardian-visible summary, and effective period.
- **Emergency Contact**: A guardian, emergency contact, or authorized contact route with priority, relationship, visibility permissions, and active status.
- **Medical Consent Record**: Evidence of guardian, school, or authorized consent for medical visibility, medication administration, emergency care steps, or information sharing.
- **Emergency Access Session**: A time-bounded access event that exposes critical medical data during an emergency and records actor, reason, viewed categories, context, and review state.
- **Break-Glass Access Event**: An emergency access event granted under configured break-glass rules that requires mandatory review.
- **Medical Incident**: A health, injury, medication, emergency, or care event logged for a student with severity, context, care actions, notification state, status, and audit evidence.
- **Care Action**: A recorded action taken during or after a medical incident, such as first aid, observation, medication administration evidence, emergency services handoff, guardian contact, or follow-up.
- **Medical Notification Request**: A tenant-owned request to notify guardians, emergency contacts, or required staff about a medical incident or emergency access event with urgency, privacy-safe content, and acknowledgement needs.
- **Medical Contact Attempt**: Evidence that a recipient was contacted or contact was attempted, including outcome, acknowledgement state, failure reason, and actor when manually recorded.
- **Medical Exception**: A reviewable issue involving invalid student, missing consent, expired instruction, duplicate incident, failed contact, stale cache, disabled feature, cross-school access, or manual review requirement.
- **Manual Medical Review**: A reviewer action that corrects, reopens, resolves, dismisses, escalates, or documents a medical record, incident, notification, access event, or exception with reason and preserved history.
- **Medical Rule Setting**: A school-account configuration record for medical visibility, emergency access, break-glass access, medication evidence, incident severity, notification audience, acknowledgement, and review routing.
- **Medical Review Summary**: A permission-scoped view of medical records, incident status, emergency access, notification acknowledgement, exceptions, and review outcomes.
- **Medical Status Event**: Tenant-scoped exportable evidence that a medical profile, incident, emergency access, notification, acknowledgement, exception, or review status changed and may be consumed later by communication or notification capabilities.
- **School Account Feature Setting**: A tenant capability setting that determines whether Phase 7 medical and emergency workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized medical staff can create or update a complete student medical profile with condition, allergy, medication instruction, care plan, emergency contact, and consent details in under 3 minutes during review testing.
- **SC-002**: 100% of sampled unauthorized, cross-school, expired guardian link, disabled-feature, and restricted-visibility medical record access attempts are blocked or hidden without exposing unrelated records.
- **SC-003**: Emergency-authorized staff can open a student's critical emergency profile in under 30 seconds during review testing when the student is in their authorized school scope.
- **SC-004**: 100% of sampled emergency access and break-glass sessions preserve actor, reason, student, viewed data category, time, and review state.
- **SC-005**: Authorized staff can log a complete medical incident with severity, observation, care action, contact requirement, and follow-up state in under 2 minutes during review testing.
- **SC-006**: 100% of sampled incident corrections preserve original incident evidence, correction reason, actor, time, resulting status, and guardian or staff visibility rules.
- **SC-007**: 95% of high-severity medical incident notification requests are available to responsible staff or later notification capabilities within 2 minutes of incident classification.
- **SC-008**: Guardians can find allowed medical profile, incident, notification, and acknowledgement information for linked students in under 30 seconds while 100% of sampled staff-only details remain hidden.
- **SC-009**: 100% of sampled expired medication instructions, expired care plans, missing consent, stale emergency cache, duplicate incidents, duplicate notifications, and failed contacts are blocked, flagged, or routed to review with a clear reason.
- **SC-010**: Auditors can trace a sampled medical lifecycle from profile update through emergency access, incident logging, care action, notification request, acknowledgement, correction, and review summary in under 60 seconds during review testing.
- **SC-011**: 100% of sampled Phase 7 records are visible only within the authorized school account scope, approved guardian link scope, student ownership scope, medical assignment scope, emergency authority scope, reviewer scope, or explicit platform-level review scope.
- **SC-012**: 100% of sampled Phase 7 medical and emergency actions create no attendance, campus gate, transport, wallet, learning reward, request approval, complaint escalation, document storage, global search, broad messaging, or broad admin dashboard outcome.
- **SC-013**: 95% of eligible medical status changes are available to later communication and notification capabilities within 2 minutes without Phase 7 delivering general messages directly.
- **SC-014**: 100% of sampled medical configuration changes preserve the rule version or configuration that governed affected records at the time of the event.

## Assumptions

- Phase 7 builds on Phase 0 tenant configuration, feature flag, audit, observability, and shared configuration foundations, and Phase 1 student identity, guardian linking, role, and permission capabilities.
- Phase 7 may consume student identity evidence from prior NFC or QR identity flows, but it does not create scan records, attendance outcomes, campus gate outcomes, or transport outcomes.
- Guardian medical visibility is school-configurable and may be restricted for staff-only notes, disputed records, sensitive care details, or records under review.
- Medical notification in Phase 7 means medical notification requests, contact priority, manual contact evidence, acknowledgement state, and status events. General messaging, broadcasts, and notification platform delivery belong to Phase 9.
- Medical profiles and incidents may reference supporting evidence identifiers, but broad document storage, certificates, file management, and global search belong to Phase 10.
- The system records and routes medical evidence and emergency workflow actions. It does not diagnose, prescribe, replace licensed medical judgement, or replace the school's emergency procedures.
- Medication administration is recorded only when authorized by school policy, guardian consent, and assigned staff authority, or when an emergency override reason is explicitly recorded for review.
- Emergency services integration, ambulance dispatch, insurance claims, pharmacy fulfillment, and hospital record exchange are outside Phase 7 unless later specs explicitly add them.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 7 requirements.
