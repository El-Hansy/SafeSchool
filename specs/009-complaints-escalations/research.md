# Research: Phase 8 Complaints & Escalations

## Decision: Keep Phase 8 bounded to complaint and escalation workflows

**Rationale**: `PLAN.md` assigns Phase 8 to Complaint Submission, Complaint
Categorization, Escalation Workflow, and Feedback & Resolution. The spec also
excludes attendance, campus access, scans, transport, wallet, learning reward,
request approval, medical, emergency, broad messaging, document storage, global
search, and broad dashboards. Keeping the boundary explicit prevents Phase 8
from becoming a messaging platform, document platform, operational dashboard,
refund workflow, discipline system, or emergency workflow.

**Alternatives considered**:
- Implement general helpdesk or ticketing for all school workflows: rejected
  because the phase is limited to complaint intake, handling, escalation, and
  feedback.
- Implement notification delivery directly in complaints: rejected because
  Phase 9 owns broad messaging and notification delivery.
- Implement document libraries or file evidence management: rejected because
  Phase 10 owns broad document storage and search.

## Decision: Use the modular monolith Complaints feature area

**Rationale**: The constitution favors a modular monolith and single PostgreSQL
database unless measured pressure justifies extraction. Complaint workflows
share tenant, identity, guardian visibility, role, feature configuration,
audit, and status event foundations with earlier phases. A Complaints feature
area keeps cost low while isolating sensitive complaint logic by module.

**Alternatives considered**:
- Separate case-management microservice: rejected because the current phase
  does not justify added operational cost or data synchronization complexity.
- External complaint system as source of truth: rejected because tenant,
  guardian-link, student, permission, and audit enforcement belong inside the
  school platform boundary.
- Store complaints in a separate database: rejected because complaint data
  requires the same tenant isolation and audit conventions as the platform.

## Decision: Depend on Phase 0 and Phase 1 foundations

**Rationale**: Complaint records are tenant-owned and often student-centric.
Submission eligibility, guardian visibility, student self-scope, staff role
authority, feature capability enforcement, and audit behavior depend on Phase 0
tenant/capability/audit infrastructure and Phase 1 student, guardian, role, and
permission records.

**Alternatives considered**:
- Duplicate student or guardian records inside Complaints: rejected because it
  creates divergent authority and cross-tenant risk.
- Allow UI-only feature gates: rejected because backend feature enforcement is
  mandatory under the constitution.

## Decision: Treat prior phase evidence as read-only references

**Rationale**: Complaints may mention attendance, transport, wallet, learning,
request, or medical records, but Phase 8 must not become the source of truth
for those domains. Evidence references let authorized complaint handlers view
context while preserving ownership and privacy boundaries.

**Alternatives considered**:
- Copy source records into the complaint: rejected because copies can become
  stale and bypass source-domain permissions.
- Allow complaints to modify referenced source records: rejected because that
  would leak Phase 8 into other phases.
- Disallow evidence references entirely: rejected because complaint handlers
  need context for fair review.

## Decision: Use authenticated complaint intake by default

**Rationale**: The spec assumes authenticated guardians, students, and staff.
Authenticated intake supports tenant resolution, guardian-link checks, student
self-scope, duplicate detection, feedback, reopen, and audit evidence.

**Alternatives considered**:
- Anonymous public intake: rejected for baseline Phase 8 because it requires
  extra abuse prevention, identity review, safety routing, and privacy rules.
- Staff-only intake: rejected because guardians and students need structured
  complaint channels.

## Decision: Use category-driven triage and versioned category rules

**Rationale**: Complaint category rules determine who may submit, required
fields, priority, confidentiality, owner group, target response timing,
escalation path, restricted handling, feedback behavior, and reopen windows.
Versioning preserves the rule that governed each complaint action.

**Alternatives considered**:
- Free-form categories without configured rules: rejected because ownership,
  target timing, and escalation become inconsistent.
- Global categories shared across all school accounts: rejected because schools
  need tenant-specific policies and owner groups.
- Rewrite historical complaints when rules change: rejected because it breaks
  auditability.

## Decision: Enforce conflict-of-interest restrictions before assignment and closure

**Rationale**: Users named as complaint subjects, involved parties, or potential
conflicts must not self-assign, decide, close, or view restricted complainant
details. This protects complainants and preserves trust in escalation outcomes.

**Alternatives considered**:
- Let administrators resolve conflicts manually after assignment: rejected
  because restricted details could already be exposed.
- Only prevent final closure by conflicted users: rejected because triage and
  investigation access can still bias the process.
- Block all access for conflicted users forever: rejected because an authorized
  reviewer may need to permit a limited, auditable review action.

## Decision: Separate internal notes from complainant-visible responses

**Rationale**: Complaint handling often requires internal investigation notes,
staff-only corrective action details, conflict review, and safety information.
Complainants still need clear status and outcome summaries without restricted
details. Separate visibility levels make this testable and auditable.

**Alternatives considered**:
- Make every entry visible to complainants: rejected because it can expose
  staff-only, student, guardian, safety, or medical privacy details.
- Hide all investigation details from complainants: rejected because complaint
  status and resolution outcomes must be understandable.

## Decision: Escalation covers high-risk, overdue, disputed, unresolved, and conflicted complaints

**Rationale**: Phase 8 includes escalation workflow. Escalation rules must
handle urgent and stalled complaints, including safety, safeguarding, severe
misconduct, externally reportable, overdue, disputed, repeatedly reopened, and
conflicted cases.

**Alternatives considered**:
- Only manual escalation: rejected because overdue and high-risk complaints can
  be missed.
- Only time-based escalation: rejected because severity and conflict-of-interest
  require immediate route changes.
- Auto-close overdue complaints: rejected because the spec requires escalation
  or manual review, never silent closure.

## Decision: Route urgent safety, medical, emergency, or external authority cases to review without creating those workflows

**Rationale**: Complaints can reveal urgent safety or medical concerns, but
Phase 8 does not own emergency protocols, medical incidents, or external
authority reporting. The correct behavior is urgent routing and audit evidence,
not side-effect creation in excluded domains.

**Alternatives considered**:
- Create medical incidents or emergency sessions from complaints: rejected
  because Phase 7 owns medical and emergency workflows.
- Create external authority reports directly: rejected because that requires a
  separate policy and legal workflow.
- Ignore urgent signals until routine triage: rejected because high-risk
  complaints require prompt review.

## Decision: Preserve complaint timelines as append-only evidence

**Rationale**: Complaints may be disputed or reopened. Original complaint
details, category changes, assignments, internal notes, visible responses,
escalations, resolutions, feedback, reopen requests, and reviews must remain
traceable.

**Alternatives considered**:
- Edit the latest complaint record in place: rejected because it hides original
  evidence.
- Keep only final resolution status: rejected because reviewers and auditors
  need explainable histories.

## Decision: Duplicate complaints use idempotent exact-match handling and review for overlap

**Rationale**: Exact active duplicates should return an existing complaint
reference or be blocked. Overlapping non-identical complaints may represent
related but distinct concerns and must be linked, grouped, or reviewed instead
of silently merged or discarded.

**Alternatives considered**:
- Always accept duplicates: rejected because it fragments ownership and target
  timing.
- Always merge overlapping complaints: rejected because distinct complainants
  and facts can be lost.
- Always block overlaps: rejected because legitimate related complaints may
  need independent handling.

## Decision: Feedback and reopen behavior is configurable and versioned

**Rationale**: Schools need configurable acceptance, dissatisfaction, ratings,
comments, reopen windows, and routing. Versioning preserves which feedback and
reopen rules governed each resolution.

**Alternatives considered**:
- No feedback after resolution: rejected because Phase 8 explicitly includes
  Feedback & Resolution.
- Unlimited reopen at any time: rejected because schools need finality and
  escalation controls.
- One global reopen rule: rejected because category risk and policy vary.

## Decision: Complaint status events feed later communication capabilities

**Rationale**: Phase 8 must make eligible status changes, escalations,
feedback, reopen requests, and reviewable events available to later Phase 9
communication and notification capabilities. It must not implement delivery,
templates, channels, retries, or broadcasts.

**Alternatives considered**:
- Send messages directly from Complaints: rejected because Phase 9 owns
  notification delivery.
- Omit status events: rejected because later communication capabilities need
  reliable complaint lifecycle triggers.

## Decision: Testing strategy follows sensitive complaint and escalation boundaries

**Rationale**: Phase 8 touches student concerns, guardian relationships,
confidential details, conflict-of-interest rules, high-risk routing, and audit
evidence. Unit, integration, contract, authorization, tenant-isolation,
feature-flag, audit, status-event, performance, and critical UI journey tests
are required.

**Alternatives considered**:
- UI-only testing: rejected because tenant and permission boundaries are
  security-critical.
- Unit tests only: rejected because route contracts, authorization, and
  lifecycle traceability require integration and contract coverage.
