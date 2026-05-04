# Research: Phase 5 Learning & Engagement

## Decision: Keep Phase 5 bounded to learning and engagement evidence

**Rationale**: `PLAN.md` assigns Phase 5 to Course & Content Delivery,
Assignment Tracking, Quiz Engine, Star & Reward System, and Behavior Logging.
The spec also requires Phase 5 to exclude attendance, campus access, transport,
wallet, request approval, medical, complaint, messaging, document, search, and
broad dashboard outcomes. Keeping the boundary explicit prevents Phase 5 from
becoming a general LMS, wallet, document, notification, or discipline system.

**Alternatives considered**:
- Implement broad LMS/document storage in Phase 5: rejected because Phase 10
  owns document storage and global search.
- Implement messaging from learning events in Phase 5: rejected because Phase 9
  owns messaging and notifications.
- Implement request privilege approvals from star evidence in Phase 5:
  rejected because Phase 6 owns request workflows.

## Decision: Use the modular monolith Learning feature area

**Rationale**: The constitution favors a modular monolith and a single
PostgreSQL database unless measured pressure justifies extraction. Learning
features share tenant, identity, guardian visibility, audit, and configuration
infrastructure with prior phases, so a feature area under the same modular
monolith keeps cost and complexity low.

**Alternatives considered**:
- Separate LMS service: rejected because there is no current scale evidence
  requiring operational separation.
- Separate star ledger database: rejected because tenant, audit, and review
  behavior belong in the platform's shared data boundary.
- External LMS replacement: rejected because the phase requires school-owned
  learning workflows, not an integration replacement strategy.

## Decision: Depend on Phase 0 and Phase 1 foundations

**Rationale**: Phase 5 records are tenant-owned and student-centric. Course
visibility, student access, guardian views, staff course/group authority, and
audit behavior depend on Phase 0 tenant/capability/audit infrastructure and
Phase 1 student, guardian, role, and permission records.

**Alternatives considered**:
- Duplicate identity or guardian records inside Learning: rejected because it
  creates divergent authority and cross-tenant risk.
- Allow UI-only feature gates: rejected because the constitution requires
  backend feature enforcement.

## Decision: Treat learning groups as Phase 5 assignment scope

**Rationale**: Courses, content, assignments, quizzes, rewards, and behavior
visibility need a school-account scoped way to target students and assigned
staff. Learning groups provide this without pulling in attendance classes,
transport assignments, or external scheduling systems.

**Alternatives considered**:
- Target only individual students: rejected because teachers need class/group
  workflows.
- Reuse transport or attendance assignments: rejected because those are
  separate domains with different lifecycle and authorization rules.

## Decision: Store resource references, not broad document management

**Rationale**: Phase 5 content and submissions may need references to learning
resources or submitted evidence, but Phase 10 owns document storage,
certificates, search indexing, and file workflows. Phase 5 should reference
resource identifiers and preserve learning state without becoming a document
system.

**Alternatives considered**:
- Build a full file repository in Phase 5: rejected because it overlaps Phase
  10.
- Disallow resource references: rejected because assignments and lessons need
  reviewable evidence.

## Decision: Preserve assignment submission history

**Rationale**: The spec requires resubmission, grading, feedback, late/excused
states, correction history, and audit evidence without overwriting prior
evidence. Each submission attempt should be traceable and reviewable.

**Alternatives considered**:
- Mutate the latest submission in place: rejected because it hides original
  work and feedback.
- Store only final grade outcomes: rejected because teachers, guardians, and
  auditors need traceable review history.

## Decision: Model quiz attempts as controlled, idempotent records

**Rationale**: Quiz integrity depends on active quiz state, eligible student,
attempt limits, timing rules, scoring rules, and feedback visibility. Attempts
must preserve responses, timings, scores, and review outcomes without
corrupting prior attempts.

**Alternatives considered**:
- Store only quiz scores: rejected because scoring disputes, feedback, and
  audit review need response and timing evidence.
- Allow unlimited retries by default: rejected because the spec requires
  configured attempt limits.

## Decision: Make Phase 5 the source of truth for stars and rewards

**Rationale**: Phase 6 explicitly consumes Phase 5 star evidence. Phase 5 must
own star balances, reservations, consumption, releases, corrections, reward
redemptions, and source references. This keeps star authority centralized.

**Alternatives considered**:
- Let Phase 6 own star balances for permission requests: rejected because it
  creates competing sources of truth.
- Store stars as mutable counters only: rejected because corrections,
  reservations, and audit traceability require ledger evidence.

## Decision: Use append-only star ledger entries with derived balance snapshots

**Rationale**: Star awards, reversals, reservations, consumption, releases, and
corrections must be auditable and retry-safe. Append-only ledger entries
preserve source evidence, while balance snapshots support fast reads and later
Phase 6 evidence checks.

**Alternatives considered**:
- Directly mutate balance fields without ledger entries: rejected because it
  cannot prove historical correctness.
- Recalculate all balances on every read: rejected because it is expensive for
  common student and guardian views.

## Decision: Keep rewards separate from wallet and payments

**Rationale**: Rewards spend stars, not money. Reward fulfillment is a school
engagement process and must not create wallet credits, purchases, refunds,
payment confirmations, or canteen POS activity.

**Alternatives considered**:
- Convert stars to wallet value: rejected because wallet/payment behavior is
  Phase 4 and has financial controls.
- Fulfill rewards as requests: rejected because Phase 6 owns approval
  workflows and Phase 5 rewards can be simpler engagement redemptions.

## Decision: Behavior logging is engagement evidence, not case management

**Rationale**: Phase 5 behavior events support learning engagement, visibility,
review, corrections, and optional star effects. Medical emergencies,
complaints, escalations, and disciplinary case-management workflows belong to
later phases.

**Alternatives considered**:
- Include full discipline workflow: rejected because it overlaps complaint and
  escalation phases.
- Include medical/emergency behavior categories: rejected because Phase 7 owns
  medical and emergency workflows.

## Decision: Route conflicting non-identical records to manual review

**Rationale**: The spec requires exact duplicates to be rejected or treated as
already processed, while conflicting non-identical submissions, attempts, star
awards, rewards, or behavior events must not be silently merged or discarded.
Manual review preserves safety and auditability.

**Alternatives considered**:
- Always accept the newest record: rejected because it may hide prior evidence.
- Always block all overlapping records: rejected because legitimate corrections
  or retries may need reviewer resolution.

## Decision: Expose eligible star and status events without delivering messages

**Rationale**: Phase 5 must make star evidence available to Phase 6 and learning
status events available to Phase 9. It should export reviewable status/evidence
records but not implement request approvals or notification delivery.

**Alternatives considered**:
- Send notifications directly from Phase 5: rejected because Phase 9 owns
  notification delivery.
- Hide star events from Phase 6 until request implementation: rejected because
  Phase 6 needs Phase 5 as the star source of truth.

## Decision: Use permission-scoped summaries and history

**Rationale**: Teachers, students, guardians, reviewers, reward managers, and
auditors need different views of learning progress, stars, rewards, behavior,
exceptions, and corrections. Summary and history reads must respect tenant,
guardian, student, course/group assignment, reviewer, and platform review
scope.

**Alternatives considered**:
- Provide broad admin dashboards in Phase 5: rejected because broad dashboard
  work is outside the phase.
- Show all behavior and staff notes to guardians: rejected because the spec
  requires visibility settings and staff-only detail protection.

## Decision: Use focused contract coverage per learning module

**Rationale**: Content delivery, assignments, quizzes, stars/rewards, behavior,
configuration, and history/review each have different routes, validation rules,
and authorization boundaries. Separate contracts keep tasks implementable and
testable by story.

**Alternatives considered**:
- One large learning contract: rejected because it would be hard for lower-cost
  implementation models to execute safely.
- No explicit contracts: rejected because the constitution requires data and
  API contracts for feature behavior.

## Decision: Testing strategy follows critical user journeys and boundaries

**Rationale**: Phase 5 touches students, guardians, staff, rewards, and later
Phase 6 star evidence, so unit, integration, contract, authorization,
tenant-isolation, audit, and critical UI journey tests are required. Tests must
cover duplicate handling, visibility, star ledger integrity, reward boundaries,
and no side effects outside Phase 5.

**Alternatives considered**:
- UI-only testing: rejected because backend tenant and permission boundaries
  are security-critical.
- Unit tests only: rejected because cross-module contracts and authorization
  boundaries require integration and contract coverage.
