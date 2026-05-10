# Implementation Plan: Phase 5 Learning & Engagement

**Branch**: `006-learning-engagement` | **Date**: 2026-05-05 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/006-learning-engagement/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 5 implements learning and engagement workflows for the School NFC
platform. The implementation approach adds a Learning feature area for courses,
learning groups, content delivery, assignment tracking, quiz attempts, progress
events, append-only star ledger evidence, reward catalog and redemption
workflows, behavior logging, learning history, exceptions, corrections, rule
settings, and review summaries. All behavior is scoped to a school account,
gated by tenant feature configuration, protected by RBAC and permission checks,
backed by PostgreSQL persistence, and observable through audit evidence. Phase
5 is the source of truth for star and reward evidence consumed by later Phase 6
permission rules, but it does not create request approvals, attendance, campus
access, transport, wallet, medical, complaint, messaging, document, search, or
broad admin dashboard outcomes.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
teacher, guardian, and student web surfaces, and Flutter/Dart only where
existing mobile clients host learning content, assignment, quiz, star, reward,
or behavior views. Exact package versions are pinned when runtime manifests are
created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
staff role and group assignment providers, tenant feature configuration,
audit/event logging, Next.js App Router, TanStack Query or typed server-driven
data access, optional mobile feature shells, and Spec Kit planning artifacts.
Phase 6 consumes Phase 5 star evidence through a bounded adapter contract after
Phase 5 star ledger behavior exists.
**Storage**: Single PostgreSQL database for tenant-owned courses, learning
groups, group memberships, content items, content revisions, progress events,
assignments, assignment submissions, quizzes, question sets, quiz attempts,
quiz responses, star rule settings, star ledger entries, star balance
snapshots, reward catalog items, reward redemptions, behavior categories,
behavior events, learning exceptions, manual learning reviews, learning rule
settings, review summaries, status event exports, feature settings, and audit
evidence. Every tenant-owned table includes `tenant_id`, `created_at`, and
`updated_at`, with indexes for tenant boundaries, student lookup, guardian
visibility, course and group lookup, assignment due queues, submission
idempotency, quiz attempt idempotency, star ledger source references, star
balance lookups, reward eligibility, behavior category filters, exception
status, review state, summary filters, and audit traceability.
**Testing**: Backend unit tests for course publication, content visibility,
progress recording, assignment required evidence and late policies, submission
idempotency, quiz activation and attempt lifecycle, scoring and review states,
star rule evaluation, append-only ledger posting, star reservation and release,
reward redemption, behavior visibility and star effects, exception detection,
correction and reopen behavior, history filters, summary visibility, and audit
emission; integration and contract tests for `/api/v1/` learning routes;
authorization, tenant-isolation, feature-flag, and audit tests; and web/mobile
journey tests for teacher, student, guardian, reward manager, behavior
reviewer, and auditor workflows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration and teacher web app, guardian-facing learning and engagement
views, student learning work views, and optional mobile learning/reward views.
Phase 5 has no NFC, QR, or offline scan capture requirement.
**Project Type**: Modular monolith SaaS with web, backend, and optional mobile
clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: content publication in
under 2 minutes, eligible assignment submission in under 3 minutes, teacher
submission lookup in under 60 seconds, quiz result visibility within 1 minute,
reward redemption in under 1 minute, guardian learning history lookup in under
30 seconds, 90-day learning search in under 30 seconds, auditor traceability in
under 60 seconds, 95% of eligible star evidence available to Phase 6 within 2
minutes, and 95% of eligible learning status changes available to later
notification capabilities within 2 minutes.
**Constraints**: Phase 5 only. Learning records are tenant-scoped and
feature-gated. Course content delivery references learning resources but does
not implement broad document storage, certificates, file management, global
search, or document workflows. Stars and rewards are engagement records, not
wallet funds or payment instruments. Behavior logging is limited to school
engagement and conduct evidence, not medical, emergency, complaint, or
disciplinary case-management workflows. Exact duplicate active submissions,
quiz attempts, star awards, reward redemptions, and behavior events are
rejected or treated as already processed; conflicting non-identical records
route to manual review. Phase 5 must not create attendance, campus gate, scan,
transport, wallet, request approval, medical, complaint, broad messaging,
broadcast, document, search, or broad admin dashboard outcomes.
**Scale/Scope**: Five Phase 5 modules from `PLAN.md`: Course & Content
Delivery, Assignment Tracking, Quiz Engine, Star & Reward System, and Behavior
Logging. Supporting learning progress history, exception handling, corrections,
configuration, review summaries, audit, Phase 6 star evidence export, and
notification-eligibility records are included only where required by the Phase
5 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 5:
  Learning & Engagement and maps to Course & Content Delivery, Assignment
  Tracking, Quiz Engine, Star & Reward System, and Behavior Logging.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  learning and engagement records, capability keys, backend feature
  enforcement, and web/mobile feature gates are required before Phase 5
  workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, student self-scope, guardian-link
  visibility, staff course or group assignment checks, reward manager and
  behavior reviewer authorization, feature availability checks, and auditable
  access decisions are scoped.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, versioned/revision-traceable learning rules, append-only star
  ledger records, and idempotent content, assignment, quiz, star, reward,
  behavior, correction, and review commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 5 has no NFC, QR, attendance, campus
  access, or transport scan capture requirement. Learning and star evidence is
  read-only to later workflows, and no mobile SQLite scan queue or offline scan
  sync is required for this phase.
- **Observability and testing**: PASS. Structured logs, audit events, learning
  lifecycle metrics, assignment and quiz metrics, star ledger metrics, reward
  metrics, behavior metrics, exception metrics, error reporting, backend tests,
  contract tests, authorization tests, tenant-isolation tests, and critical UI
  journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, document
  storage subsystem, learning analytics warehouse, external LMS replacement,
  wallet subsystem, or broad CQRS patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 5 scope, keep tenant and permission
enforcement explicit, define append-only star ledger behavior, reward
boundaries, duplicate handling, Phase 6 star evidence export, and notification
boundaries, and introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/006-learning-engagement/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── assignment-tracking.md
│   ├── behavior-logging.md
│   ├── course-content-delivery.md
│   ├── learning-configuration.md
│   ├── learning-history-review.md
│   ├── quiz-engine.md
│   └── star-reward-system.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Learning/
│   │   ├── Courses/
│   │   ├── Content/
│   │   ├── Groups/
│   │   ├── Assignments/
│   │   ├── Quizzes/
│   │   ├── Progress/
│   │   ├── Stars/
│   │   ├── Rewards/
│   │   ├── Behavior/
│   │   ├── Exceptions/
│   │   ├── Reviews/
│   │   ├── Configuration/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/Learning/
├── admin-web/
│   ├── src/app/(school)/learning/
│   ├── src/app/(guardian)/learning/
│   ├── src/app/(student)/learning/
│   ├── src/features/learning/
│   └── tests/learning/
└── mobile/
    ├── lib/features/learning/
    └── test/features/learning/

tests/
├── contracts/learning/
└── e2e/learning/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own course publication, learning group membership,
content visibility, assignment submissions, quiz attempts, progress events,
star ledger posting, reward redemption, behavior logging, exception review,
rule settings, persistence, and audit events. Web code owns school
administrator, academic coordinator, teacher, reviewer, reward manager,
behavior reviewer, guardian, and student-facing learning workflows. Mobile code
is limited to existing mobile learning, assignment, quiz, star, reward, and
behavior views where needed; no offline scan storage is introduced. The
current repository contains planning artifacts only, so these source paths are
the implementation target for the later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, Phase 6 star evidence export,
tenant and feature capability enforcement, learning permissions, course and
content visibility, assignment submission rules, quiz attempt lifecycle, star
ledger ownership, reward redemption boundaries, behavior logging, duplicate and
conflict handling, exception review, notification boundary, observability,
testing strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for course content delivery,
assignment tracking, quiz attempts, star and reward workflows, behavior
logging, learning configuration, learning history, exception review,
corrections, and review summaries. These artifacts define entities, state
transitions, validation rules, `/api/v1/` interface behavior, idempotency
requirements, guardian visibility boundaries, Phase 6 star evidence export,
audit evidence, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the six
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
visibility boundaries, audit evidence, idempotency, and validation coverage
where it touches those concerns.
