# Feature Specification: Phase 9 Communication & Notifications

**Feature Branch**: `010-communication-notifications`  
**Created**: 2026-05-06  
**Status**: Reviewed  
**Input**: User description: "Read PLAN.md and create a specification for phase of Communication & Notifications ONLY."

## Review Status

- Product and engineering review gate: Passed for planning and task generation.
- Implementation may proceed from this specification unless later review changes are recorded.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 9: Communication & Notifications
- **Feature Module(s)**: Messaging System, Broadcast & Announcement, Notification System
- **Tenant Scope**: All conversations, message threads, message participants, message content, broadcast drafts, announcements, audience rules, recipient snapshots, notification rules, notification records, delivery attempts, read states, acknowledgements, preferences, templates, moderation decisions, exceptions, review summaries, configuration, and audit evidence belong to one school account and must not be visible, delivered, searched, replied to, or administered outside that school account unless an explicit platform-level review role permits it.
- **Feature Flag(s)**: Direct messaging, staff-to-guardian messaging, student messaging, broadcasts, announcements, notification center, external delivery channels, templates, delivery tracking, acknowledgements, communication preferences, communication history, moderation, and communication review summaries must respect each school account's enabled capabilities before users can access, send, receive, automate, or review related communication workflows.
- **Security/Roles**: Platform owners, school administrators, communication managers, authorized staff, teachers, transport coordinators, finance staff, learning staff, request approvers, medical staff, complaint managers, guardians, students, auditors, moderators, and reviewers must have explicit permissions for each Phase 9 action. Guardians can receive, view, reply to, or acknowledge communications only for students linked to them through an approved active guardian relationship or for communications addressed directly to them. Students can receive, view, reply to, or acknowledge communications only within school-enabled student communication rules. Staff users can communicate only within their school account and role-authorized audience. Restricted medical, complaint, safety, finance, and student-welfare details must be minimized unless the actor has explicit authority to view them.
- **Offline/NFC Impact**: Phase 9 does not require NFC, QR, or offline scan behavior. It may consume eligible status events from attendance, campus access, transport, wallet, learning, requests, medical, emergency, and complaints modules for communication purposes, but Phase 9 must not create scan events, attendance outcomes, campus access decisions, transport boarding decisions, wallet transactions, learning rewards, request approvals, medical incidents, emergency access sessions, complaint decisions, documents, global search records, or broad admin dashboard outcomes.
- **Observability**: The system must emit reviewable evidence for message draft, message send, message edit where allowed, message withdrawal, reply, recipient selection, recipient exclusion, broadcast draft, broadcast approval, broadcast publication, announcement update, notification generation, duplicate suppression, delivery attempt, delivery success, delivery failure, retry, read state change, acknowledgement, acknowledgement expiry, preference change, template change, moderation decision, exception creation, summary read, configuration change, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Receive and Review Notifications (Priority: P1)

As a guardian, student, or staff member, I need a permission-scoped notification center so I can quickly understand school updates that are relevant to me or to students I am allowed to support.

**Why this priority**: The notification center is the baseline value of the phase. It centralizes status events from previous phases without forcing each domain module to deliver messages directly.

**Independent Test**: Record an eligible school status event for one student, verify the authorized guardian and relevant staff receive a notification inside the correct school account, and confirm unrelated guardians, students, staff, and other school accounts cannot view it.

**Acceptance Scenarios**:

1. **Given** notification center is enabled and an eligible status event is available for a student, **When** the event is accepted for notification, **Then** authorized recipients receive a notification with category, priority, source summary, allowed action, read state, acknowledgement requirement where applicable, and audit evidence.
2. **Given** a guardian, student, or staff member has visible notifications, **When** they open and filter the notification center by student, category, priority, read state, acknowledgement state, and date range, **Then** only notifications within their authorized school, relationship, role, or assignment scope are shown.
3. **Given** a notification contains restricted medical, complaint, safety, finance, or staff details, **When** a recipient lacks authority for those details, **Then** the notification shows only the minimum permitted summary or is withheld with a reviewable reason.

---

### User Story 2 - Send Scoped Direct Messages (Priority: P1)

As an authorized staff member, guardian, or student where enabled, I need to send and reply to scoped messages so school communication happens in an accountable channel rather than informal external chats.

**Why this priority**: Direct messaging provides the core two-way communication flow and establishes participant scope, visibility, moderation, and audit behavior for the phase.

**Independent Test**: Send a message between an authorized staff member and an approved guardian for an active student, verify both participants can view and reply in the thread, and confirm unrelated users and other school accounts cannot access the conversation.

**Acceptance Scenarios**:

1. **Given** direct messaging is enabled and the actor has permission to contact the selected recipient or group, **When** the actor sends a message with subject, body, student context where applicable, priority, and allowed recipients, **Then** a tenant-scoped conversation is created or continued with participants, message content, delivery records, read state, and audit evidence.
2. **Given** a recipient is an authorized participant in a conversation, **When** they reply, mark messages read, or view thread history, **Then** the thread preserves message order, sender identity, participant visibility, read state, and any acknowledgement requirements.
3. **Given** the actor lacks permission, the guardian link is not approved and active, student messaging is disabled, the recipient is outside the school account, the thread is closed, or moderation is required, **When** message send or reply is attempted, **Then** the action is blocked or routed to moderation with a reviewable reason.

---

### User Story 3 - Publish Broadcasts and Announcements (Priority: P1)

As a school administrator or communication manager, I need to publish broadcasts and announcements to permitted audiences so school-wide and cohort-specific information reaches the correct people.

**Why this priority**: Broadcasts and announcements are explicitly part of Phase 9 and support one-to-many school communication without manual recipient handling.

**Independent Test**: Publish an announcement to a permitted grade, class, route, staff group, guardian group, or whole-school audience, verify the recipient snapshot is tenant-scoped and relationship-scoped, and confirm excluded or unauthorized recipients do not receive it.

**Acceptance Scenarios**:

1. **Given** broadcasts or announcements are enabled and the actor has publisher authority, **When** they create and publish a communication with title, body, priority, audience, effective window, and optional acknowledgement requirement, **Then** the announcement is delivered to the resolved authorized recipient snapshot and preserved with publication audit evidence.
2. **Given** a broadcast requires approval, moderation, translation, or scheduled release, **When** the actor submits it, **Then** it remains pending until the required condition is satisfied and no recipient receives it early.
3. **Given** the audience is empty, contains cross-school users, includes restricted recipients, conflicts with feature configuration, exceeds school policy limits, or would expose restricted student details, **When** publication is attempted, **Then** the broadcast is rejected or routed to review with a clear reason.

---

### User Story 4 - Track Delivery, Reads, and Acknowledgements (Priority: P1)

As a communication manager, school administrator, or authorized staff member, I need delivery, read, and acknowledgement tracking so urgent or required communications are not treated as sent without evidence.

**Why this priority**: A communication system is not operationally useful unless the school can distinguish created, delivered, failed, read, acknowledged, and overdue communications.

**Independent Test**: Send a required acknowledgement notification to an authorized audience, verify delivery attempts and read or acknowledgement states are recorded per recipient, and verify failed or overdue recipients appear in the correct review queue.

**Acceptance Scenarios**:

1. **Given** delivery tracking is enabled for a message, notification, broadcast, or announcement, **When** delivery is attempted through an enabled channel, **Then** each recipient has an attempt record with channel, status, time, failure reason where applicable, retry state, and audit evidence.
2. **Given** an acknowledgement is required, **When** a recipient reads or acknowledges the communication, **Then** the read and acknowledgement state updates only for that recipient and the sender or authorized reviewer can see aggregate and per-recipient status within their scope.
3. **Given** delivery fails, acknowledgement is overdue, the recipient becomes ineligible, or the channel is disabled after send, **When** tracking is reviewed, **Then** the item is marked failed, overdue, excluded, or review-required without silently claiming success.

---

### User Story 5 - Configure Communication Rules and Preferences (Priority: P2)

As a school administrator, I need configurable templates, audiences, channels, quiet hours, acknowledgement rules, moderation rules, and recipient preferences so each school can operate its communication policy without product changes.

**Why this priority**: Configuration is essential for scale and tenant variation, but it can follow after the core notification, messaging, broadcast, and tracking flows are defined.

**Independent Test**: Configure a notification category and template with default audience, channel behavior, priority, quiet-hour handling, and acknowledgement requirement; trigger an eligible event; and verify future notifications use the active configuration while historical communications retain prior rules.

**Acceptance Scenarios**:

1. **Given** communication configuration is enabled and the actor is an authorized school administrator, **When** they create or activate a template, notification rule, channel rule, preference policy, or moderation rule, **Then** future communications use the active version and the change is audit-visible.
2. **Given** a guardian, student, or staff member updates allowed communication preferences, **When** future optional communications are generated, **Then** the preferences are respected while mandatory school safety, security, attendance, medical, emergency, and legal notices remain governed by school policy.
3. **Given** a template is missing required content, a rule has no valid audience, quiet-hour behavior is invalid, a mandatory category is made fully optional, or a channel is disabled, **When** activation is attempted, **Then** the configuration is rejected with a reviewable reason.

---

### User Story 6 - Review Communication History and Exceptions (Priority: P2)

As an auditor, communication reviewer, or school administrator, I need communication history, exception, moderation, and summary views so the school can explain what was sent, to whom, why, and with what result.

**Why this priority**: Review and audit improve governance after delivery and messaging flows are functioning, especially for sensitive school and student communications.

**Independent Test**: Send a direct message, publish a broadcast, generate a system notification, create a delivery failure, and verify an authorized reviewer can trace each lifecycle while unauthorized users cannot access restricted content.

**Acceptance Scenarios**:

1. **Given** communication history is enabled and the actor is authorized, **When** they filter by student, guardian, staff member, category, source module, message type, priority, delivery state, acknowledgement state, moderation state, exception type, and date range, **Then** only records inside the actor's authorized scope are returned.
2. **Given** a message, broadcast, notification, template, delivery attempt, or acknowledgement enters an exception or moderation state, **When** a reviewer resolves, rejects, corrects, republishes, or documents it, **Then** the original record and reviewer reason remain preserved.
3. **Given** an auditor reviews a communication lifecycle, **When** they open the trace, **Then** they can see the permitted chain from source event, template or author, audience resolution, delivery attempts, reads, acknowledgements, failures, moderation decisions, and audit evidence.

### Edge Cases

- A guardian link is pending, expired, suspended, removed, rejected, restricted, or belongs to another school account when recipients are resolved.
- A student is inactive, graduated, transferred, duplicated, or belongs to another school account after a communication is drafted but before it is delivered.
- A staff member loses a role, is disabled, changes school account, or no longer has authority over the target audience after a message or broadcast is drafted.
- A recipient has opted out of optional communication, but the notification category is mandatory under school safety, security, attendance, medical, emergency, or legal policy.
- Quiet hours are active when an urgent or mandatory communication is generated.
- A source event from attendance, transport, wallet, learning, requests, medical, emergency, or complaints arrives late, is duplicated, or is superseded by a correction.
- A broadcast audience changes between draft, approval, scheduled release, and delivery.
- A broadcast is withdrawn, corrected, or republished after some recipients have already read or acknowledged it.
- A direct message thread includes a student or guardian who later becomes ineligible to participate.
- A message or announcement attempts to include broad document storage, file attachments, certificate records, or global search behavior that belongs to Phase 10.
- A user replies to a no-reply system notification or to a closed thread.
- A template contains missing variables, stale wording, restricted source details, unsupported language, or content that conflicts with the selected audience.
- External delivery channels are unavailable, disabled, rate-limited, rejected, or return delayed status updates.
- The same recipient is selected through multiple audience rules and would otherwise receive duplicate communications.
- A high-priority communication needs school safety action, medical response, emergency workflow, external authority contact, or manual protocol outside this phase.
- A user attempts to use message delivery, read state, or acknowledgement as authorization for attendance, campus gate, transport boarding, wallet payment, learning reward, request approval, medical access, complaint resolution, document access, global search, or broad admin dashboard outcomes.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized guardians, students, school staff, communication managers, and school administrators to create, send, receive, view, reply to, withdraw where allowed, and acknowledge communications inside a school account when the relevant communication capability is enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, actor identity, actor permission, recipient eligibility, student status, approved guardian link status, audience rule validity, moderation requirements, communication preferences, restricted detail visibility, and delivery policy before creating, sending, publishing, delivering, acknowledging, correcting, reviewing, or showing any communication.
- **FR-003**: The system MUST accept eligible notification source events from prior phase modules only when those events are tenant-scoped, permission-safe, feature-enabled, and marked as eligible for communication.
- **FR-004**: Each notification record MUST capture school account, source event reference, source module, category, priority, title or summary, permitted detail level, recipient, student or context where applicable, reply policy, support routing action where applicable, correction or withdrawal state, delivery state, read state, acknowledgement requirement, acknowledgement state, exception state, and audit evidence.
- **FR-005**: Direct messaging MUST support scoped conversations between authorized staff, guardians, students where enabled, and role-based school participants while preserving participants, student context where applicable, subject, message body, priority, visibility, thread state, read state, acknowledgement state, and audit evidence.
- **FR-006**: Conversation participants MUST be permission-scoped so users can view, reply to, mute, leave where allowed, or archive only conversations that remain inside their school, guardian-link, student ownership, assignment, role, or review scope.
- **FR-007**: Broadcasts and announcements MUST support draft, approval where required, scheduled publication, immediate publication, correction, withdrawal where allowed, expiration, recipient snapshot, acknowledgement requirement, and audit evidence.
- **FR-008**: Broadcast audience targeting MUST be limited to authorized school account audiences such as whole school, guardians, students, staff, grade, class, route, activity group, role group, manually selected permitted recipients, or source-event recipients.
- **FR-009**: Communication templates MUST define school-account rules for category, language, title, body, required variables, default priority, default audience, restricted-detail minimization, acknowledgement behavior, quiet-hour behavior, and channel behavior.
- **FR-010**: Communication preferences MUST allow eligible guardians, students, and staff to manage optional categories and enabled channels while preserving mandatory categories required by school policy, safety, security, attendance, medical, emergency, or legal obligations.
- **FR-011**: Quiet-hour rules MUST delay optional communications according to school configuration but MUST allow urgent or mandatory communications to bypass quiet hours when school policy requires immediate delivery.
- **FR-012**: Delivery tracking MUST record each enabled channel attempt, delivery result, retry state, failure reason, recipient exclusion reason, and final delivery state per recipient without marking undelivered communications as successful.
- **FR-013**: Read receipts and acknowledgements MUST be tracked per recipient and MUST support required acknowledgement deadlines, overdue state, reminder eligibility, and reviewer visibility without exposing recipients outside the actor's scope.
- **FR-014**: The system MUST suppress exact duplicate notification events, duplicate audience recipients, and repeated delivery attempts that use the same communication intent while preserving evidence that the duplicate was handled.
- **FR-015**: The system MUST detect and record communication exceptions, including invalid recipient, invalid guardian link, inactive student, disabled feature, disabled channel, missing template, missing audience, moderation required, cross-school recipient, restricted detail exposure risk, duplicate communication, delivery failure, acknowledgement overdue, and manual-review-required condition.
- **FR-016**: Moderation and approval rules MUST route selected messages, broadcasts, templates, or replies to authorized reviewers before delivery when required by school policy, student communication rules, restricted content, large audience size, or high-risk category.
- **FR-017**: Communication history MUST be filterable by student, guardian, staff member, sender, recipient, audience, source module, category, priority, message type, delivery state, read state, acknowledgement state, moderation state, exception type, and date range within the user's authorized scope.
- **FR-018**: Communication review summaries MUST show permission-scoped counts for sent, delivered, failed, read, acknowledged, overdue, moderated, withdrawn, corrected, exception, and pending communications without exposing records outside the authorized scope.
- **FR-019**: The system MUST separate internal, restricted, reviewer-only, and recipient-visible details so notification, message, and broadcast content never exposes medical, complaint, finance, staff, safety, or student-welfare details beyond the recipient's authority.
- **FR-020**: Communication templates, notification rules, preference policies, moderation rules, quiet-hour rules, delivery channel rules, and audience rules MUST be versioned or revision-traceable so historical communications show the rules that governed them when generated, published, delivered, or acknowledged.
- **FR-021**: The system MUST provide clear correction or withdrawal behavior for messages, broadcasts, announcements, and notifications while preserving the original content, corrected content, actor, time, reason, recipient impact, and audit evidence.
- **FR-022**: The system MUST make communication lifecycle events available for review and later operational monitoring, including created, queued, sent, delivered, failed, read, acknowledged, overdue, moderated, withdrawn, corrected, and access-denied states.
- **FR-023**: The system MUST record audit evidence for message draft, message send, message edit where allowed, message withdrawal, reply, recipient selection, recipient exclusion, broadcast draft, broadcast approval, broadcast publication, announcement update, notification generation, duplicate suppression, delivery attempt, delivery success, delivery failure, retry, read state change, acknowledgement, acknowledgement expiry, preference change, template change, moderation decision, exception creation, summary read, configuration change, and access denial.
- **FR-024**: Phase 9 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet refunds or payment actions, learning reward actions, request approval workflows, medical or emergency workflows, complaint resolution workflows, broad document storage, certificate management, file libraries, global search, and broad admin dashboards from deliverable scope.
- **FR-025**: Phase 9 MUST capture message text, announcement text, notification summaries, and allowed source references, but broad document storage, certificate management, file libraries, and global search belong to Phase 10.
- **FR-026**: The system MUST support language and localization metadata for templates, messages, broadcasts, and notifications, and MUST route missing or unsupported language variants to review or a configured fallback.
- **FR-027**: The system MUST apply school-configured bulk communication safeguards, including audience review, duplicate suppression, recipient count confirmation, restricted-detail checks, and rate or frequency policy checks before large broadcasts or repeated notifications are delivered.
- **FR-028**: The system MUST respect school account feature configuration independently for direct messaging, staff-to-guardian messaging, student messaging, broadcasts, announcements, notification center, external delivery channels, templates, delivery tracking, acknowledgements, preferences, history, moderation, and review summaries.
- **FR-029**: System-generated notifications MUST clearly identify their source module and whether replies are allowed; replies to no-reply or closed system notifications MUST be blocked or routed to the configured support workflow.
- **FR-030**: Sensitive communication details MUST be hidden from users without explicit school account, guardian-link, student ownership, sender, recipient, role, assignment, audit, moderation, review, or platform review authority.

### Key Entities *(include if feature involves data)*

- **Conversation**: A tenant-owned direct message thread with participants, student or context reference where applicable, subject, status, priority, visibility rules, latest activity, and audit history.
- **Conversation Participant**: A user, role, guardian, student, staff member, reviewer, or system participant associated with a conversation and governed by send, reply, read, mute, archive, and visibility rules.
- **Message**: A preserved communication entry in a conversation, including sender, body, priority, visibility, correction or withdrawal state, read state, acknowledgement state, and audit evidence.
- **Broadcast or Announcement**: A one-to-many school communication with title, body, priority, audience rule, effective window, publication state, correction or withdrawal state, acknowledgement requirement, and audit evidence.
- **Audience Rule**: A school-account rule or recipient selection that resolves permitted recipients by whole school, guardians, students, staff, grade, class, route, activity group, role group, source event, or manually selected recipients.
- **Recipient Snapshot**: The resolved set of recipients for a message, notification, broadcast, or announcement at send or publication time, including inclusion reason, exclusion reason, relationship evidence, and delivery eligibility.
- **Notification Source Event**: A tenant-scoped event from another phase that is eligible to generate a notification without making Phase 9 the source of truth for the originating workflow.
- **Notification Record**: A recipient-visible or staff-visible notification item with category, source summary, priority, reply policy, support routing action where applicable, correction or withdrawal state, read state, acknowledgement state, delivery state, and exception state.
- **Communication Template**: A versioned school-account template defining category, language, required variables, default audience, detail minimization, priority, acknowledgement behavior, quiet-hour behavior, and channel behavior.
- **Delivery Attempt**: A per-recipient, per-channel attempt to deliver a communication, including attempt status, failure reason, retry state, and final result.
- **Communication Preference**: A user or school-account preference record for optional categories, enabled channels, quiet-hour behavior where permitted, and mandatory category constraints.
- **Acknowledgement Record**: Per-recipient evidence that a required communication was read or acknowledged, including time, actor, deadline, reminder state, and overdue state.
- **Communication Exception**: A reviewable issue involving invalid recipients, disabled features, missing templates, moderation, restricted details, duplicate communication, delivery failure, overdue acknowledgement, or manual review requirements.
- **Moderation Review**: A reviewer action that approves, rejects, corrects, withdraws, or documents a message, broadcast, template, or exception with a reason and preserved history.
- **Communication Review Summary**: A permission-scoped view of communication counts, delivery states, read states, acknowledgement states, exceptions, moderation outcomes, and aging.
- **Communication Feature Setting**: A tenant capability and policy setting that determines which Phase 9 communication and notification workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized guardians, students, and staff can find their current notification list and unread count in under 30 seconds during review testing.
- **SC-002**: 95% of eligible school status events produce recipient-visible notification records within 2 minutes of the event becoming available for communication.
- **SC-003**: 100% of sampled unauthorized actors, invalid guardian links, inactive students, disabled communication capabilities, disabled channels, cross-school recipients, and restricted-detail combinations are prevented from sending, receiving, viewing, replying to, acknowledging, or reviewing communications.
- **SC-004**: Authorized staff can send a scoped direct message to a permitted guardian, student, staff member, or role group in under 60 seconds during review testing.
- **SC-005**: Authorized recipients can reply to an eligible direct message and see the reply in the correct thread in under 30 seconds during review testing.
- **SC-006**: Authorized communication managers can publish a complete broadcast or announcement to a permitted audience in under 2 minutes during review testing.
- **SC-007**: 100% of sampled audience rules resolve only recipients inside the authorized school account and approved relationship, role, student ownership, or assignment scope.
- **SC-008**: 95% of enabled delivery channel attempts record success, failure, exclusion, or retry-needed state within 5 minutes of the delivery attempt.
- **SC-009**: Communication managers can identify failed, overdue, unacknowledged, and review-required communications in under 60 seconds during review testing.
- **SC-010**: 100% of sampled required acknowledgements preserve recipient, time, deadline, reminder state, overdue state, and reviewer-visible evidence.
- **SC-011**: 100% of sampled exact duplicate notification events and duplicate audience recipient selections are suppressed or merged without sending duplicate communications to the same recipient for the same intent.
- **SC-012**: 100% of sampled optional communication preference changes affect newly generated optional communications within 2 minutes while mandatory categories remain governed by school policy.
- **SC-013**: 100% of sampled communications that contain medical, complaint, finance, staff, safety, or student-welfare context show only the permitted summary to each recipient role.
- **SC-014**: Auditors can trace a sampled communication from source event or author through template, audience resolution, delivery attempts, read state, acknowledgement, moderation, exception, and correction history in under 60 seconds during review testing.
- **SC-015**: 100% of sampled Phase 9 communication and notification actions create no attendance, campus gate, scan, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, document storage, global search, or broad admin dashboard outcome.
- **SC-016**: 100% of sampled communication configuration changes preserve the template, audience, channel, preference, quiet-hour, acknowledgement, and moderation rule version that governed affected communications at the time of generation or delivery.

## Assumptions

- Phase 9 builds on Phase 0 tenant configuration, feature flag, audit, observability, and shared configuration foundations, and Phase 1 student identity, guardian linking, role, and permission capabilities.
- Earlier phases may expose eligible status events for communication. Phase 9 consumes those events and does not change the originating workflow or become its source of truth.
- In-app notification center behavior is the baseline delivery surface. External delivery channels are school-configured and may be enabled or disabled independently.
- Direct messaging is authenticated by default. Anonymous or public messaging is outside the baseline Phase 9 scope unless a later specification adds it with explicit abuse prevention, moderation, and review rules.
- Students may participate in messaging only when the school account enables student messaging for the selected communication category or audience.
- Guardian visibility and delivery require an approved active guardian link unless a communication is addressed directly to the guardian outside a student context.
- Mandatory communication categories are determined by school policy and may override optional recipient preferences and quiet hours.
- Message attachments, broad document storage, certificate management, file libraries, and global search belong to Phase 10. Phase 9 captures message text, announcement text, notification summaries, and permitted source references only.
- Broad operational dashboards and platform observability consoles belong to Phase 11. Phase 9 includes only communication-specific review summaries needed to operate this phase.
- External delivery channels may fail or be delayed. Phase 9 records attempts, failures, retries, exclusions, and acknowledgement evidence rather than guaranteeing recipient attention.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 9 requirements.
