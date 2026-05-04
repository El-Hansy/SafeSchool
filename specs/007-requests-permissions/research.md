# Phase 6 Research: Requests & Permissions

## Decision: Implement Phase 6 as runtime product behavior

**Rationale**: The Phase 6 spec defines operational student, guardian, and
staff request workflows with approvals, early leave safety checks, outing
requests, star-based rules, exception handling, review history, and audit
evidence. These are production workflows with measurable tenant isolation,
authorization, decision, and traceability outcomes.

**Alternatives considered**:
- Treat Phase 6 as documentation only: rejected because the spec requires
  request submission, decision routing, early leave release eligibility, and
  auditable review behavior.
- Limit Phase 6 to a generic form builder: rejected because workflow decisions,
  guardian consent, early leave pickup evidence, star rules, and duplicate
  handling are explicit Phase 6 requirements.

## Decision: Use the constitution runtime baseline without adding new platforms

**Rationale**: The constitution already defines ASP.NET Core Web API for
backend, PostgreSQL for storage, Next.js/React/TypeScript for web surfaces, and
Flutter/Dart only where mobile client surfaces are needed. Phase 6 does not
justify a workflow microservice, queue broker, document store, scan subsystem,
or separate approvals platform.

**Alternatives considered**:
- Add a dedicated workflow engine service: rejected because a versioned
  approval workflow module inside the modular monolith satisfies the current
  scope.
- Add a separate document store for pickup evidence: rejected because default
  evidence is an authorized pickup person plus staff verification note, not
  government ID image capture.
- Add offline scan infrastructure: rejected because Phase 6 has no NFC, QR, or
  offline scan requirement.

## Decision: Organize implementation around one Requests feature area

**Rationale**: Outing requests, early leave requests, request types, workflow
templates, decisions, guardian consent, star rules, exceptions, reviews, and
summaries share tenant, feature, permission, audit, and student/guardian
eligibility rules. A single Requests feature area with internal modules keeps
ownership cohesive while avoiding one oversized service.

**Alternatives considered**:
- Split outing, early leave, and approvals into unrelated roots: rejected
  because they share workflow and review behavior.
- Put all request behavior into one service: rejected because request creation,
  workflow evaluation, consent, star rules, early leave evidence, and reviews
  have separate responsibilities.

## Decision: Reuse Phase 0 tenant, feature, audit, and observability foundations

**Rationale**: Phase 0 owns school account tenant resolution, feature
configuration, audit/event logging, observability, and shared platform rules.
Phase 6 consumes those foundations instead of redefining tenant context,
feature flags, or audit semantics.

**Alternatives considered**:
- Create request-specific tenant resolution: rejected because it risks
  divergent authorization and cross-school exposure.
- Allow unaudited review corrections: rejected because request outcomes affect
  student permissions and release eligibility.

## Decision: Reuse Phase 1 identity, guardian, role, and permission evidence

**Rationale**: Phase 1 owns student profiles, guardian links, roles, and
permissions. Phase 6 uses that evidence for student eligibility, guardian
submission and consent, staff assignment, assigned approver authorization,
guardian history visibility, and reviewer authorization.

**Alternatives considered**:
- Duplicate student or guardian records inside Requests: rejected because it
  creates inconsistent identity ownership.
- Let workflow assignees decide based on UI visibility alone: rejected because
  backend permission checks must enforce each decision.

## Decision: Integrate with Phase 5 only for star evidence and star outcomes

**Rationale**: Phase 5 owns star and reward balances. Phase 6 can evaluate
star-based permission rules, preserve star rule snapshots, and record
reservation, consumption, release, pending, failed, or reviewed outcome
references, but it must not become the source of truth for star balances.

**Alternatives considered**:
- Store independent star balances in Requests: rejected because it duplicates
  Phase 5 ownership.
- Ignore star changes after submission entirely: rejected because the spec
  requires preserving evaluation evidence and handling failed, pending,
  reversed, or reviewed results.

## Decision: Treat school account as the tenant boundary for all request records

**Rationale**: Phase 0 established School Account as the tenant boundary.
Requests, workflow templates, decisions, guardian consent, star evaluations,
pickup evidence, exceptions, reviews, summaries, and audit evidence all belong
to one school account. Guardians see only linked-student request records
allowed by their active guardian relationship.

**Alternatives considered**:
- Use student identity as a global lookup boundary: rejected because it risks
  exposing cross-school student existence.
- Allow platform-wide request search by default: rejected because platform
  access must require explicit review authority.

## Decision: Enforce feature availability with explicit Phase 6 capability keys

**Rationale**: The constitution requires backend feature flag enforcement and UI
feature gates. Phase 6 capabilities can be enabled independently because a
school may enable basic requests before star rules, early leave, or workflow
configuration.

**Capability keys**:
- `requests.outing`
- `requests.early_leave`
- `requests.star_rules`
- `requests.approval_workflow`
- `requests.history`
- `requests.configuration`
- `requests.review_summaries`

Supporting exception, review, and audit behavior is governed by the relevant
request workflow capabilities and permissions.

**Alternatives considered**:
- One `requests` flag for every workflow: rejected because schools may phase in
  request types, early leave, star rules, and configuration separately.
- UI-only gating: rejected because backend enforcement must protect tenant data
  and student release workflows.

## Decision: Use explicit Phase 6 permissions with inherited RBAC

**Rationale**: Phase 1 establishes role and permission enforcement. Phase 6
adds concrete permission keys for school administrators, request managers,
assigned approvers, guardians, students, attendance or gate staff read-only
release evidence, auditors, reviewers, and platform reviewers while evaluating
permissions inside the active school account.

**Common permission families**:
- `requests.requests.read`
- `requests.requests.create`
- `requests.requests.withdraw`
- `requests.requests.manage`
- `requests.outings.read`
- `requests.outings.manage`
- `requests.early_leave.read`
- `requests.early_leave.manage`
- `requests.early_leave.release_read`
- `requests.pickup.verify`
- `requests.workflows.read`
- `requests.workflows.manage`
- `requests.decisions.act`
- `requests.guardian_consent.act`
- `requests.star_rules.read`
- `requests.star_rules.manage`
- `requests.history.read`
- `requests.guardian_history.read`
- `requests.reviews.manage`
- `requests.summaries.read`
- `requests.audit.read`

**Alternatives considered**:
- Let all staff approve requests: rejected because approval steps must honor
  assigned roles and permissions.
- Let guardians see all request records for a school: rejected because guardian
  access is limited to approved active linked students.

## Decision: Use versioned request types and workflow templates

**Rationale**: Historical requests must retain the request type, required
fields, guardian consent rule, star rule, and workflow version that governed
them at submission time. New requests use the currently active version.
Migration or reopening of old requests requires authorized review with a
reason.

**Alternatives considered**:
- Mutate active workflow templates in place: rejected because it would make old
  decisions impossible to explain.
- Rebuild all pending requests whenever a template changes: rejected because it
  may invalidate already-made guardian or staff decisions without review.

## Decision: Default guardian consent to one authorized guardian with stricter request-type overrides

**Rationale**: The clarification selected any one authorized guardian as the
default consent rule while allowing schools to configure stricter request type
rules. This keeps everyday requests usable while supporting higher-sensitivity
workflows.

**Alternatives considered**:
- Require primary guardian only: rejected because it blocks legitimate
  decision-capable guardians unless explicitly delegated.
- Require all decision-capable guardians: rejected because common school
  requests would be delayed unnecessarily.
- Require explicit setup before enabling any requests: rejected because the
  default rule is safe enough when guardian authority is already controlled by
  Phase 1 guardian links.

## Decision: Keep expired workflow steps pending and route them to review or escalation

**Rationale**: Student safety workflows should not silently approve or deny a
request because a person missed an expiry window. Expired steps remain pending
and route to manual review or configured escalation with evidence.

**Alternatives considered**:
- Automatically approve expired steps: rejected because it risks unsafe release
  or permission outcomes.
- Automatically deny expired steps: rejected because it may incorrectly reject
  valid requests during closure, holiday, or staff absence.
- Leave expired steps pending without escalation: rejected because stalled
  requests would not be operationally visible.

## Decision: Require guardian-selected pickup person plus staff verification note for early leave

**Rationale**: The clarification selected guardian-selected authorized pickup
person plus staff verification note by default. This creates reviewable release
eligibility evidence without forcing Phase 6 into document capture,
government-ID image storage, NFC/QR scans, attendance generation, or gate
decisions.

**Alternatives considered**:
- Free-text pickup name only: rejected because it is too weak for student
  release eligibility.
- Government ID capture for every pickup: rejected because document capture is
  outside Phase 6 scope and adds privacy overhead.
- NFC or QR scan at release: rejected because scan flows belong to attendance,
  campus access, or later integration behavior.

## Decision: Reserve stars at submission, consume on approval, and release on denial, withdrawal, or expiry

**Rationale**: The clarification selected reservation at submission with final
consumption only on approval. This prevents overspending while a request is
pending, gives clean rollback behavior, and keeps Phase 5 as the source of
truth for star balances and star outcome references.

**Alternatives considered**:
- Consume immediately at submission: rejected because denied or withdrawn
  requests would require refunds and more correction paths.
- Check stars only at submission: rejected because students could overspend
  before approval.
- Check stars only at final approval: rejected because students could submit
  multiple pending requests against the same stars.

## Decision: Block exact active duplicates and review overlapping non-identical active requests

**Rationale**: The clarification selected blocking exact active duplicates for
the same student, request type, requested date, and requested time window, while
routing overlapping non-identical active requests to manual review. This
prevents duplicate approval work without hiding legitimate but conflicting
requests.

**Alternatives considered**:
- Allow all duplicates: rejected because approvers would receive noisy and
  potentially conflicting work.
- Merge duplicates automatically: rejected because merging can hide requester
  intent and evidence.
- Withdraw older requests automatically: rejected because it changes an
  existing request without requester or reviewer decision.

## Decision: Keep notification delivery outside Phase 6

**Rationale**: Phase 6 makes request status changes and reviewable events
available to later communication and notification capabilities, but Phase 9
owns messaging, broadcasts, and notification delivery. This preserves phase
boundaries while allowing later notification integration.

**Alternatives considered**:
- Implement full notifications in Phase 6: rejected because it duplicates the
  Phase 9 communication scope.
- Hide status changes from later notification capabilities: rejected because
  request updates must be eligible for later notification workflows.

## Decision: Keep attendance, campus access, transport, wallet, learning, medical, complaint, document, search, and broad dashboard outcomes out of Phase 6

**Rationale**: Approved request evidence may be read by authorized staff, but
Phase 6 must not create attendance records, gate events, scan events, transport
outcomes, wallet actions, learning content, star balance ownership, medical
flows, complaint escalation, document storage, global search, or broad admin
dashboard behavior.

**Alternatives considered**:
- Let early leave approval create attendance or gate events: rejected because
  those outcomes belong to Phase 2 and must remain separate.
- Let star-gated requests own star balances: rejected because Phase 5 owns star
  and reward records.
- Add broad dashboards or global search: rejected because those belong to later
  admin/audit/search phases.
