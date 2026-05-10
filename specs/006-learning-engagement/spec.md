# Feature Specification: Phase 5 Learning & Engagement

**Feature Branch**: `006-learning-engagement`
**Created**: 2026-05-05
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for phase 5: Learning & Engagement ONLY."

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 5: Learning & Engagement
- **Feature Module(s)**: Course & Content Delivery, Assignment Tracking, Quiz Engine, Star & Reward System, Behavior Logging
- **Tenant Scope**: All courses, learning groups, content items, assignments, submissions, quizzes, question sets, quiz attempts, progress events, star ledger entries, reward catalog records, reward redemptions, behavior events, exceptions, corrections, review summaries, rule settings, and audit evidence belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Course content delivery, assignment tracking, quiz engine, star and reward system, behavior logging, learning progress history, learning configuration, and learning review summaries must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, academic coordinators, teachers, teaching assistants, behavior reviewers, reward managers, students, guardians, auditors, and reviewers must have explicit permissions for each Phase 5 action. Students can access only their own assigned learning work, quiz attempts, stars, rewards, and allowed behavior history. Guardians can access only allowed learning and engagement records for students linked to them through an approved active guardian relationship. Staff users can act only within their school account and assigned course, group, class, reviewer, or reward authority.
- **Offline/NFC Impact**: Phase 5 does not require NFC, QR, or offline scan capture. Learning progress, quiz completion, behavior, and star evidence may be viewed or consumed by authorized later workflows, but Phase 5 must not create attendance, campus access, transport, wallet, request approval, medical, complaint, messaging, document storage, global search, or broad admin dashboard outcomes.
- **Observability**: The system must emit reviewable evidence for course creation, content publication, content withdrawal, assignment creation, assignment submission, resubmission, grading, quiz activation, quiz attempt start, quiz completion, quiz scoring, star award, star reversal, star reservation, star consumption, star release, reward redemption, reward fulfillment, behavior event creation, behavior acknowledgement, correction, reopening, exception creation, manual review, configuration changes, progress summary reads, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Publish and Consume Learning Content (Priority: P1)

As a teacher or academic coordinator, I need to publish courses and learning content to assigned students so coursework is available in a structured, tenant-scoped learning space.

**Why this priority**: Learning content is the foundation for assignments, quizzes, progress tracking, and engagement evidence.

**Independent Test**: Publish a course content item for an active learning group in one school account, verify assigned students and linked guardians can view allowed details, and confirm unrelated students, guardians, staff, and school accounts cannot discover or open it.

**Acceptance Scenarios**:

1. **Given** course content delivery is enabled and the staff actor is authorized for the course or group, **When** the actor publishes a content item with required title, description, schedule, target learners, and learning resource reference, **Then** assigned students can access it within the configured visibility window and the publication remains audit-visible.
2. **Given** content is draft, withdrawn, expired, assigned to another group, disabled by feature configuration, or owned by another school account, **When** a student, guardian, or staff user attempts to view it, **Then** access is blocked or hidden according to role and tenant scope without exposing cross-school existence.
3. **Given** a student opens, completes, or resumes a published content item, **When** progress is recorded, **Then** the student, authorized teacher, and allowed guardian views show the correct progress state without creating attendance, transport, wallet, or request outcomes.

---

### User Story 2 - Track Assignments and Submissions (Priority: P1)

As a teacher, I need to assign work, receive submissions, and record review outcomes so student learning evidence is organized and visible to the right participants.

**Why this priority**: Assignments are a core learning workflow and produce reviewable evidence for progress, grading, stars, and guardian visibility.

**Independent Test**: Create an assignment for a learning group, submit work as an assigned student, grade it as an authorized teacher, and verify submission status, feedback, visibility, late handling, and audit evidence.

**Acceptance Scenarios**:

1. **Given** assignment tracking is enabled and the teacher is authorized for the course or group, **When** the teacher creates an assignment with due date, instructions, required evidence, and grading or review settings, **Then** eligible students see it and can submit work during the allowed window.
2. **Given** a student is assigned and the submission window permits work, **When** the student submits or resubmits assignment evidence, **Then** the submission status, submitted time, attempt history, and allowed guardian visibility update without overwriting prior evidence.
3. **Given** a submission is late, missing, duplicate, withdrawn, excused, submitted by an ineligible student, or belongs to another school account, **When** the system evaluates it, **Then** it is blocked, marked late, excused, or routed to review according to school rules with a clear reason.

---

### User Story 3 - Deliver Quizzes and Record Results (Priority: P1)

As a teacher, I need quizzes with controlled attempts and reviewable scoring so student understanding can be measured consistently.

**Why this priority**: Quizzes are a distinct learning module with timing, attempt, scoring, and integrity rules that must be correct before engagement rewards depend on them.

**Independent Test**: Activate a quiz for an assigned group, complete an attempt as an eligible student, and verify scoring, attempt limits, feedback visibility, tenant isolation, and audit evidence.

**Acceptance Scenarios**:

1. **Given** the quiz engine is enabled and the staff actor is authorized, **When** the actor activates a quiz with questions, schedule, attempt limits, scoring rules, and feedback visibility, **Then** assigned students can start attempts only during the allowed window.
2. **Given** an eligible student completes a quiz attempt, **When** the attempt is submitted, **Then** the result preserves responses, score or review state, attempt number, allowed feedback, and progress evidence.
3. **Given** a quiz attempt is expired, duplicated, submitted after the window, beyond the allowed attempt count, missing required answers, interrupted, or attempted by an ineligible or cross-school student, **When** completion is processed, **Then** the attempt is blocked, marked for review, or finalized according to school rules without corrupting prior attempts.

---

### User Story 4 - Manage Stars and Rewards (Priority: P1)

As a school administrator, teacher, or reward manager, I need a star and reward system so positive learning, quiz, assignment, and behavior outcomes can produce reliable engagement evidence and safe reward redemption.

**Why this priority**: Stars and rewards are a required Phase 5 output and later Phase 6 request rules consume Phase 5 star evidence as the source of truth.

**Independent Test**: Configure a star rule, award stars from a qualifying learning or behavior event, redeem a reward as an eligible student, and verify the star ledger, balance, redemption status, reversal behavior, and evidence available to later permission rules.

**Acceptance Scenarios**:

1. **Given** the star and reward system is enabled and a star rule is active, **When** a qualifying assignment, quiz, content, or behavior event occurs, **Then** the system records an append-only star ledger entry, updates the student's available star evidence, and preserves the source event reference.
2. **Given** a student has enough available stars and a reward is active for that student's eligible group, **When** the student or authorized staff redeems the reward, **Then** the redemption records the star impact, status, fulfillment state, and audit evidence without creating wallet or payment activity.
3. **Given** the student lacks enough stars, the reward is unavailable, the rule is disabled, a duplicate award is attempted, a prior award is corrected, or a later workflow reserves stars, **When** the star or reward operation is evaluated, **Then** the ledger remains balanced, the action is blocked or routed to review, and historical evidence is preserved.

---

### User Story 5 - Log Behavior and Engagement Events (Priority: P2)

As an authorized staff member, I need to record positive, corrective, or neutral behavior and engagement events so student conduct and participation are visible, reviewable, and can support school-defined engagement rules.

**Why this priority**: Behavior logging is important for engagement insight and star rules, but it can follow after the core learning, assignment, quiz, and star evidence flows are defined.

**Independent Test**: Record a behavior event for an active student, verify guardian and student visibility according to school rules, correct the event as an authorized reviewer, and confirm the original event remains preserved.

**Acceptance Scenarios**:

1. **Given** behavior logging is enabled and the actor is authorized for the student or group, **When** the actor records a behavior or engagement event with category, severity, note, visibility, and source context, **Then** the event is attached to the correct student and school account with reviewable evidence.
2. **Given** the behavior event is configured to affect stars, **When** the event is accepted or reviewed, **Then** the related star rule is applied only under active school rules and the behavior event remains traceable from the star evidence.
3. **Given** a behavior event is sensitive, disputed, duplicated, recorded for the wrong student, recorded by an unauthorized actor, or assigned to another school account, **When** it is viewed or corrected, **Then** visibility is restricted and corrections preserve the original event, reason, actor, and resulting status.

---

### User Story 6 - Review Progress, Exceptions, and Learning Configuration (Priority: P3)

As a school administrator, academic coordinator, reviewer, guardian, or auditor, I need progress summaries, exception handling, and configurable learning rules so the school can operate learning workflows consistently and explain outcomes.

**Why this priority**: Summaries, exception review, and advanced configuration improve scale and governance after the core learning and engagement records exist.

**Independent Test**: Configure learning rules for a school account, search progress and exception history for a student, correct an eligible learning record with a reason, and verify all summary, historical, and audit evidence remains tenant-scoped.

**Acceptance Scenarios**:

1. **Given** learning history or review summaries are enabled and the user is authorized, **When** the user filters by student, course, assignment, quiz, star outcome, behavior category, date range, or review state, **Then** only records inside the user's authorized scope are returned.
2. **Given** a learning record, quiz attempt, star ledger entry, reward redemption, or behavior event needs correction, **When** an authorized reviewer provides the required reason, **Then** the original evidence remains preserved and the current outcome explains the correction.
3. **Given** a school administrator changes course, assignment, quiz, star, reward, behavior, or visibility rules, **When** old and new records are reviewed, **Then** records show the rule version or configuration that governed them at the time of the event.

---

### Edge Cases

- A student is added to, removed from, transferred between, or suspended from a learning group while content, assignments, quizzes, or rewards are active.
- A guardian link is pending, expired, suspended, removed, rejected, or belongs to another school account.
- A teacher or assistant is removed from a course, disabled, transferred, or assigned after learning records already exist.
- Course content is withdrawn after students have already viewed or completed it.
- A content resource reference becomes unavailable after publication.
- An assignment due date changes after students have submitted work.
- Two submissions or quiz attempts are sent for the same student and task at nearly the same time.
- A quiz timer expires during an active attempt or a student loses connectivity before final submission.
- A quiz question, answer key, or scoring rule is corrected after attempts are already submitted.
- A star rule changes after an assignment, quiz, content, or behavior event already produced star evidence.
- A star award is duplicated, reversed, corrected, reserved, consumed, or released after a related reward or permission workflow has read the earlier balance.
- A reward becomes unavailable, suspended, expired, or over-redeemed while a student is redeeming it.
- A behavior event is marked sensitive, disputed by a guardian, or later found to belong to a different student.
- A user attempts to use learning progress, behavior status, reward status, or stars as authorization for attendance, campus gate, transport boarding, wallet purchase, request approval, medical access, complaint escalation, or broad notification delivery.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized school staff to create, update, publish, withdraw, view, and review school-account courses, learning groups, content items, and learning resource references when course content delivery is enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, student status, guardian link status, staff assignment, actor permission, course or group eligibility, visibility rules, required evidence, and applicable learning rule settings before creating, submitting, grading, scoring, awarding, redeeming, correcting, or showing a Phase 5 record.
- **FR-003**: Course and content records MUST capture school account, course or group, title, description, learning resource reference, target learners, publication status, release and expiry windows, visibility rules, progress expectations, version or revision state, and audit evidence.
- **FR-004**: The system MUST allow eligible students to view assigned content, record progress, resume incomplete work, and see allowed completion states while allowing linked guardians to view only school-approved progress details.
- **FR-005**: The system MUST allow authorized staff to create, assign, update, close, review, grade, return, excuse, and archive assignments for eligible students or learning groups.
- **FR-006**: Assignment records MUST capture school account, course or group, target learners, instructions, required evidence, due date, submission window, late policy, rubric or review settings, submission status, grade or feedback state, attempt history, and audit evidence.
- **FR-007**: The system MUST allow eligible students to submit, resubmit when allowed, withdraw when allowed, and view assignment outcomes while preserving all prior submission evidence and reviewer feedback.
- **FR-008**: The quiz engine MUST allow authorized staff to create, activate, suspend, review, and retire quizzes with question sets, schedules, attempt limits, time limits, scoring rules, feedback visibility, and review rules.
- **FR-009**: Quiz attempts MUST require an eligible student, active quiz, enabled quiz capability, available attempt count, valid time window, required answers where applicable, and tenant-scoped identity before starting, completing, scoring, or showing a result.
- **FR-010**: Quiz result records MUST preserve student, quiz, attempt number, submitted responses, score or review status, feedback visibility, reviewer actions when applicable, timing evidence, and audit evidence without overwriting prior attempts.
- **FR-011**: The star and reward system MUST support school-account rule settings for earning stars from content, assignments, quizzes, behavior events, manual awards, corrections, expirations, group eligibility, award caps, reward eligibility, review behavior, and failure behavior when source evidence is unavailable.
- **FR-012**: Star ledger entries MUST be append-only and MUST record school account, student, source event, actor or system source, star amount, ledger direction, reason, available balance impact, reservation status, consumption status, release status, correction reference, and audit evidence.
- **FR-013**: Phase 5 MUST be the source of truth for star balances, star reservations, star consumption, star releases, star corrections, reward redemptions, and star evidence consumed by later Phase 6 permission rules.
- **FR-014**: Reward catalog records MUST capture school account, reward name, description, eligible student groups, star cost, availability window, inventory or fulfillment constraints when used, redemption limits, status, and audit evidence.
- **FR-015**: Reward redemptions MUST validate student eligibility, available stars, reward status, redemption limits, duplicate attempts, guardian or staff restrictions when configured, and fulfillment state before stars are consumed or released.
- **FR-016**: The system MUST allow authorized staff to record, view, correct, and review behavior or engagement events for students when behavior logging is enabled.
- **FR-017**: Behavior event records MUST capture school account, student, category, severity, positive/corrective/neutral classification, source context, staff note, guardian or student visibility setting, review state, related star impact when applicable, correction history, and audit evidence.
- **FR-018**: Behavior events that affect stars MUST apply only through active school-account star rules and MUST preserve the link between the behavior event, star ledger entry, reviewer decision, and any later correction.
- **FR-019**: Learning history MUST be filterable by student, guardian, course, learning group, content item, assignment, submission state, quiz, quiz result, star outcome, reward redemption, behavior category, date range, actor, exception type, and review state within the user's authorized scope.
- **FR-020**: The system MUST allow authorized reviewers to correct, reopen, close, resolve, or escalate learning exceptions with a reason while preserving the original learning record, original outcome, correction actor, correction time, and resulting status.
- **FR-021**: The system MUST detect and record learning exceptions, including invalid enrollment, inactive student, missing evidence, late submission, duplicate submission, duplicate quiz attempt, quiz scoring conflict, stale learning rule, missing source evidence, duplicate star award, insufficient stars, unavailable reward, invalid behavior record, disabled feature, cross-school access attempt, and manual-review-required condition.
- **FR-022**: Each learning exception record MUST include school account, affected student where applicable, affected learning record, exception type, severity, source evidence, current status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-023**: Course, assignment, quiz, star, reward, behavior, visibility, and review configuration MUST be versioned or revision-traceable so historical records show the rule or configuration that governed them when the event occurred.
- **FR-024**: Staff, student, guardian, reward manager, behavior reviewer, auditor, and platform reviewer visibility MUST be permission-scoped so users see only records allowed by school account, approved guardian link, student ownership, course assignment, group assignment, reviewer assignment, or explicit platform review authority.
- **FR-025**: The system MUST respect school account feature configuration independently for course content delivery, assignment tracking, quiz engine, star and reward system, behavior logging, learning history, learning configuration, and learning review summaries.
- **FR-026**: The system MUST provide learning review summaries by student, course, learning group, assignment status, quiz result, star balance state, reward redemption status, behavior category, exception state, date range, and reviewer assignment without exposing records outside the authorized scope.
- **FR-027**: The system MUST make learning progress changes, quiz outcomes, assignment status changes, star changes, reward changes, and reviewable events available to later communication and notification capabilities, but Phase 5 MUST NOT implement general messaging, broadcasts, or notification delivery.
- **FR-028**: The system MUST record audit evidence for course creation, content publication, assignment creation, assignment submission, grading, quiz activation, quiz attempt lifecycle, quiz scoring, star award, star correction, star reservation, star consumption, star release, reward redemption, reward fulfillment, behavior event creation, behavior correction, exception creation, manual review, summary read, configuration change, and access denial.
- **FR-029**: Phase 5 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet or payment actions, request approval workflows, medical or emergency workflows, complaint escalation workflows, broad messaging or broadcasts, document storage workflows, global search, and broad admin dashboards from deliverable scope.
- **FR-030**: Exact duplicate active submissions, quiz attempts, star awards, reward redemptions, and behavior events MUST be rejected or treated as already processed, while overlapping or conflicting non-identical records MUST be routed to manual review instead of being silently merged or discarded.

### Key Entities *(include if feature involves data)*

- **Course**: A school-account learning container for curriculum, content items, assignments, quizzes, learning groups, progress records, and review history.
- **Learning Group**: A tenant-owned set of eligible students, staff assignments, visibility rules, and effective date ranges used to target learning content and activities.
- **Learning Content Item**: A published or draft lesson, resource reference, activity, or module assigned to learners with visibility windows, completion expectations, and progress evidence.
- **Assignment**: A teacher-created learning task with instructions, required evidence, due dates, submission rules, grading or review settings, and learner assignment.
- **Assignment Submission**: A student's submitted work evidence, attempt history, status, late or excused state, review outcome, feedback, and audit trail.
- **Quiz**: A controlled assessment with question sets, activation state, schedule, attempt rules, timing rules, scoring rules, feedback settings, and review history.
- **Quiz Attempt**: A student's attempt at a quiz, including timing evidence, responses, completion state, score or review state, feedback visibility, and prior attempt relationship.
- **Learning Progress Event**: Evidence that a student viewed, started, resumed, completed, or had progress corrected for content, assignments, quizzes, or related learning work.
- **Star Rule Setting**: A school-account configuration record defining when stars are awarded, reversed, reserved, consumed, released, capped, expired, or routed to review.
- **Star Ledger Entry**: Append-only evidence for a student's star credit, debit, reservation, consumption, release, correction, or expiry, tied to a source event and audit evidence.
- **Star Balance Snapshot**: A reviewable view of a student's available, reserved, consumed, released, corrected, or pending star state derived from ledger evidence.
- **Reward Catalog Item**: A tenant-owned reward definition with star cost, eligibility, availability, fulfillment constraints, redemption limits, and status.
- **Reward Redemption**: A student's request or staff-created record to spend stars on a reward, including validation outcome, fulfillment state, cancellation or release state, and audit history.
- **Behavior Event**: A positive, corrective, or neutral engagement record for a student, including category, severity, note, visibility, source context, star impact when configured, review state, and correction history.
- **Learning Exception**: A reviewable issue involving invalid enrollment, missing evidence, late or duplicate work, quiz scoring conflict, stale rules, duplicate stars, insufficient stars, unavailable reward, invalid behavior evidence, disabled feature, or access denial.
- **Manual Learning Review**: A reviewer action that corrects, reopens, resolves, escalates, dismisses, or documents a learning exception with reason and preserved history.
- **Learning Rule Setting**: A school-account configuration record for learning visibility, late policies, quiz attempts, feedback visibility, star rules, reward rules, behavior categories, correction permissions, and review routing.
- **Learning Review Summary**: A permission-scoped view of learning progress, assignment status, quiz outcomes, star balances, reward activity, behavior events, exceptions, and review outcomes.
- **Learning Status Event**: Tenant-scoped exportable evidence that a Phase 5 learning, assignment, quiz, star, reward, behavior, exception, or review status changed and may be consumed later by Phase 6 star evidence or Phase 9 notification capabilities without delivering messages or approving requests directly.
- **School Account Feature Setting**: A tenant capability setting that determines whether Phase 5 learning and engagement workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized staff can publish a complete course content item for assigned students in under 2 minutes during review testing.
- **SC-002**: 100% of sampled draft, withdrawn, expired, cross-school, unassigned, disabled-feature, and unauthorized content access attempts are blocked or hidden without exposing unrelated records.
- **SC-003**: Eligible students can submit a complete assignment in under 3 minutes, and authorized teachers can find the submission for review in under 60 seconds during review testing.
- **SC-004**: 100% of sampled assignment submissions preserve submission history, grade or review outcome, feedback visibility, and correction evidence after resubmission, return, excuse, or manual review.
- **SC-005**: Eligible students can start and complete an active quiz attempt during the allowed window, and allowed results are visible within 1 minute of completion during review testing.
- **SC-006**: 100% of sampled expired, duplicate, ineligible, over-limit, cross-school, and out-of-window quiz attempts do not corrupt prior attempts or create duplicate final scores.
- **SC-007**: 100% of sampled star balances match append-only ledger evidence after awards, reversals, reservations, consumption, releases, expirations, corrections, and duplicate retries.
- **SC-008**: Eligible students can redeem an available reward with sufficient stars in under 1 minute, and 100% of sampled insufficient-star, unavailable-reward, duplicate, and cross-school redemption attempts are blocked or routed to review.
- **SC-009**: 95% of eligible star evidence changes are available to later Phase 6 permission-rule consumers within 2 minutes without Phase 5 implementing request approval workflows.
- **SC-010**: 100% of sampled behavior events preserve original evidence, visibility setting, star impact when configured, correction reason, actor, and resulting status after correction or review.
- **SC-011**: Guardians can find allowed learning progress, assignment outcomes, quiz outcomes, star or reward state, and behavior records for linked students in under 30 seconds while 100% of sampled staff-only details remain hidden.
- **SC-012**: Authorized users can find a learning or engagement record from the last 90 days by student, course, assignment, quiz, star outcome, reward status, behavior category, date range, or exception state in under 30 seconds during review testing.
- **SC-013**: Auditors can trace a sampled student learning lifecycle from content publication through assignment, quiz, star, reward, behavior, exception, correction, and review summary in under 60 seconds during review testing.
- **SC-014**: 100% of sampled Phase 5 records are visible only within the authorized school account scope, approved guardian link scope, student ownership scope, staff assignment scope, reviewer scope, or explicit platform-level review scope.
- **SC-015**: 100% of sampled Phase 5 learning, star, reward, and behavior actions create no attendance, campus gate, transport, wallet, request approval, medical, complaint, document storage, global search, or broad admin dashboard outcome.
- **SC-016**: 95% of eligible learning status changes are available to later communication and notification capabilities within 2 minutes without Phase 5 delivering messages directly.

## Assumptions

- Phase 5 builds on Phase 0 tenant configuration, feature flag, audit, observability, and shared configuration foundations, and Phase 1 student identity, guardian linking, role, and permission capabilities.
- Course and learning group membership is school-account scoped. A student may belong to multiple groups, and school rules determine whether guardians see all or limited learning details.
- Learning content delivery may reference coursework resources needed for lessons and assignments, but broad document storage, certificates, document workflows, global search, and file management belong to Phase 10.
- Reward fulfillment is a school-managed engagement process and does not create wallet credits, payments, refunds, canteen POS actions, or financial records.
- Phase 5 is the source of truth for stars and rewards. Phase 6 may later consume star evidence for request permission rules, but Phase 5 does not implement request creation or approval workflows.
- Behavior logging in Phase 5 covers school engagement and conduct records only. Medical, emergency, complaint, escalation, and disciplinary case-management workflows are outside this phase unless later specs explicitly consume Phase 5 behavior evidence.
- General messaging, broadcasts, and notification delivery belong to Phase 9. Phase 5 only makes eligible learning, assignment, quiz, star, reward, and behavior events available to later notification capabilities.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 5 requirements.
