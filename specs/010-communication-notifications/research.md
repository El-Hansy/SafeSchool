# Research: Phase 9 Communication & Notifications

## Decision: Keep Phase 9 bounded to communication and notification workflows

**Rationale**: `PLAN.md` assigns Phase 9 to Messaging System, Broadcast &
Announcement, and Notification System. The spec excludes attendance, campus
access, scans, transport, wallet, learning reward, request approval, medical,
emergency, complaint resolution, broad document storage, certificate
management, global search, and broad admin dashboards. Keeping the boundary
explicit prevents Phase 9 from becoming an operational workflow engine,
document platform, search platform, or broad analytics dashboard.

**Alternatives considered**:
- Let Phase 9 own follow-up actions in source modules: rejected because source
  modules remain the system of record for their own outcomes.
- Implement document attachments and searchable communication archives:
  rejected because Phase 10 owns broad document storage and search.
- Implement broad admin dashboards: rejected because Phase 11 owns broad
  operational dashboards and observability consoles.

## Decision: Use the modular monolith Communications feature area

**Rationale**: The constitution favors a modular monolith and single
PostgreSQL database unless measured pressure justifies extraction.
Communication workflows share tenant, identity, guardian visibility, role,
feature configuration, audit, and status event foundations with earlier
phases. A Communications feature area keeps cost low while isolating
notification, message, broadcast, delivery, preference, moderation, and review
logic by module.

**Alternatives considered**:
- Separate notification microservice: rejected because the current phase does
  not justify added operational cost, deployment complexity, or cross-service
  data synchronization.
- External communication platform as source of truth: rejected because tenant,
  guardian-link, student, permission, audit, and restricted-detail enforcement
  belong inside the school platform boundary.
- Separate communication database: rejected because communication records need
  the same tenant isolation, indexes, migrations, and audit conventions as the
  platform.

## Decision: Depend on Phase 0 and Phase 1 foundations

**Rationale**: Every communication is tenant-owned and often student-centric.
Recipient eligibility, guardian visibility, student self-scope, staff role
authority, feature capability enforcement, and audit behavior depend on Phase
0 tenant/capability/audit infrastructure and Phase 1 student, guardian, role,
and permission records.

**Alternatives considered**:
- Duplicate student, guardian, or staff records inside Communications:
  rejected because it creates divergent authority and cross-tenant risk.
- Use UI-only feature gates: rejected because backend feature enforcement is
  mandatory under the constitution.

## Decision: Treat prior phase events as read-only notification source events

**Rationale**: Attendance, transport, wallet, learning, requests, medical,
emergency, and complaints modules may expose eligible status events. Phase 9
accepts those events only as communication triggers and must not mutate source
records or become the source of truth for the originating workflow.

**Alternatives considered**:
- Let Phase 9 query and infer every source workflow state directly: rejected
  because that couples communications to unrelated domain internals.
- Copy full source records into notifications: rejected because copies can
  become stale and bypass source-domain permissions.
- Let source modules deliver messages directly: rejected because Phase 9 owns
  centralized delivery, preferences, duplicate suppression, templates, and
  review evidence.

## Decision: Use an in-app notification center as the baseline delivery surface

**Rationale**: The in-app notification center provides a reliable baseline
that does not depend on external providers. External channels can be enabled by
school configuration and represented as auditable delivery attempts, while the
platform still preserves recipient-visible notification records.

**Alternatives considered**:
- External channels only: rejected because provider failure would remove the
  platform's canonical notification record.
- In-app only forever: rejected because schools may configure external
  channels for operational needs.
- Treat provider delivery as guaranteed recipient attention: rejected because
  external status can be delayed, rejected, or unavailable.

## Decision: Use a PostgreSQL-backed communication outbox and delivery attempts

**Rationale**: Delivery needs queued, retried, failed, suppressed, and excluded
states, but the constitution favors low-cost modular monolith architecture.
PostgreSQL-backed outbox records and delivery attempts support idempotency,
auditability, retries, and later extraction if measured scale requires it.

**Alternatives considered**:
- Add a message broker in the baseline plan: rejected because no measured
  operational pressure justifies the added infrastructure yet.
- Send synchronously during user actions: rejected because external delivery
  attempts can be slow or fail independently of the communication record.
- Skip retry evidence: rejected because schools need to distinguish delivered,
  failed, excluded, and pending communications.

## Decision: Scope direct messaging through explicit participants and permissions

**Rationale**: Direct messaging must be accountable and tenant-scoped.
Conversation participants preserve sender, recipient, student context where
applicable, reply authority, read state, acknowledgement state, and visibility
rules.

**Alternatives considered**:
- Open chat between any platform users: rejected because it bypasses school
  authority, guardian links, student messaging rules, and moderation.
- Staff-only messages: rejected because guardians and students need structured
  communication where enabled.
- Anonymous or public messaging: rejected for baseline Phase 9 because it
  requires additional abuse prevention and moderation workflows.

## Decision: Preserve recipient snapshots at send or publication time

**Rationale**: Broadcast and notification audiences can change after draft,
approval, scheduled release, or delivery. Recipient snapshots explain who was
included, excluded, and why at the time of send or publication.

**Alternatives considered**:
- Recalculate recipients every time a communication is viewed: rejected
  because it hides historical delivery decisions.
- Store only audience rule names: rejected because auditors need the actual
  recipient set and relationship evidence.
- Allow cross-school expansion from broad audiences: rejected because tenant
  isolation is mandatory.

## Decision: Version communication templates and rules

**Rationale**: Templates, notification rules, audience rules, preference
policies, quiet-hour rules, channel behavior, acknowledgement behavior, and
moderation rules affect what recipients see and when. Versioning preserves the
rule set that governed each communication.

**Alternatives considered**:
- Update rules in place: rejected because historical communications would lose
  their governing policy context.
- Use one global template and policy: rejected because schools need tenant
  variation.
- Require product changes for every policy adjustment: rejected because Phase
  9 must support school-configured communication behavior.

## Decision: Mandatory categories can override optional preferences and quiet hours

**Rationale**: Guardians, students, and staff should control optional
communication preferences where allowed, but school safety, security,
attendance, medical, emergency, and legal notices may be mandatory under school
policy. Quiet hours delay optional messages but must not suppress urgent or
mandatory notices.

**Alternatives considered**:
- Let users opt out of every category: rejected because schools may be
  obligated to deliver critical notices.
- Ignore preferences for all messages: rejected because optional categories
  need user control and reduced noise.
- Delay urgent notices during quiet hours: rejected because urgent school
  communication may require immediate delivery.

## Decision: Track delivery, read state, and acknowledgement per recipient

**Rationale**: One communication can have many recipients with different
channels, delivery outcomes, read states, acknowledgement deadlines, and
eligibility changes. Per-recipient tracking makes failed, overdue, excluded,
and review-required states visible.

**Alternatives considered**:
- Track only aggregate delivery state: rejected because staff need to know
  which recipients require follow-up.
- Treat read as acknowledgement: rejected because some communications require
  explicit acknowledgement evidence.
- Delete failed attempts after retry: rejected because failure evidence is
  operationally important.

## Decision: Suppress duplicate source events, recipients, and delivery intents

**Rationale**: Prior phases can retry or correct status events, and audience
rules can select the same recipient multiple ways. Duplicate suppression
prevents communication spam while preserving evidence that a duplicate was
handled.

**Alternatives considered**:
- Send every source event and audience match: rejected because duplicate
  notifications reduce trust and create noise.
- Silently discard duplicates: rejected because reviewers need traceability.
- Merge unrelated overlapping events: rejected because distinct source events
  may require separate notices.

## Decision: Route high-risk content, large audiences, and restricted details through moderation where configured

**Rationale**: Schools may require approval or moderation for student
communications, large broadcasts, high-risk categories, restricted content, or
template changes. Moderation preserves review decisions before delivery.

**Alternatives considered**:
- Always deliver immediately: rejected because policy and safeguarding review
  may be required.
- Require moderation for every communication: rejected because routine
  notifications and low-risk messages would become slow.
- Allow moderators to overwrite history: rejected because original content and
  reviewer reasons must remain preserved.

## Decision: Minimize restricted source details in communication content

**Rationale**: Notifications and messages may reference medical, complaint,
finance, staff, safety, or student-welfare context. Recipients should receive
only details they are authorized to see, with withheld or summarized content
where required.

**Alternatives considered**:
- Copy full source detail into communications: rejected because it can expose
  sensitive data outside source-domain permissions.
- Hide every sensitive event entirely: rejected because authorized recipients
  still need actionable summaries.
- Rely on frontend filtering only: rejected because backend visibility
  enforcement is mandatory.

## Decision: Exclude broad document storage, attachments, and global search

**Rationale**: Phase 9 captures message text, announcement text, notification
summaries, and allowed source references. File libraries, certificates,
document storage, and global search are explicitly Phase 10 capabilities.

**Alternatives considered**:
- Add attachment storage to messages: rejected because broad document storage
  and file governance belong to Phase 10.
- Index all communication content globally: rejected because Phase 10 owns
  global search and broader search authorization.
- Include certificate delivery in announcements: rejected because certificate
  management belongs to Phase 10.

## Decision: Testing strategy follows sensitive communication boundaries

**Rationale**: Phase 9 touches student communications, guardian relationships,
restricted source details, bulk audiences, delivery evidence, acknowledgements,
moderation, and audit records. Unit, integration, contract, authorization,
tenant-isolation, feature-flag, idempotency, delivery, acknowledgement,
moderation, audit, performance, and critical UI journey tests are required.

**Alternatives considered**:
- UI-only testing: rejected because tenant, permission, recipient, and
  restricted-detail boundaries are security-critical.
- Unit tests only: rejected because route contracts, authorization, delivery
  state, and lifecycle traceability require integration and contract coverage.
- Manual delivery verification only: rejected because delivery, read, and
  acknowledgement evidence must be repeatable and auditable.
