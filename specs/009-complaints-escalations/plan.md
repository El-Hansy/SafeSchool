# Implementation Plan: Phase 8 Complaints & Escalations

**Branch**: `009-complaints-escalations` | **Date**: 2026-05-06 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/009-complaints-escalations/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 8 implements structured complaint intake, categorization, escalation, and
resolution workflows for the School NFC platform. The implementation approach
adds a Complaints feature area for complaint records, category configuration,
required intake fields, participants, assignments, investigation entries,
internal and complainant-visible responses, conflict-of-interest handling,
escalation rules, escalation events, resolution records, feedback, reopen
requests, exceptions, manual reviews, review summaries, status events, and audit
evidence. All behavior is scoped to a school account, gated by tenant feature
configuration, protected by RBAC and permission checks, backed by PostgreSQL
persistence, and observable through audit evidence. Phase 8 may reference prior
student, guardian, attendance, transport, wallet, learning, request, and medical
evidence where authorized, and may expose complaint status events to later
communication capabilities, but it does not create attendance, campus access,
scan, transport, wallet, learning reward, request approval, medical, emergency,
broad messaging, document, search, or broad admin dashboard outcomes.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
complaint manager, staff, guardian, and student web surfaces, and Flutter/Dart
only where existing mobile clients host complaint submission or tracking views.
Exact package versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
roles and permissions, tenant feature configuration, audit/event logging, prior
evidence references from attendance, transport, wallet, learning, requests, and
medical modules where authorized, Next.js App Router, TanStack Query or typed
server-driven data access, and Spec Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned complaints, complaint
categories, complaint participants, complaint assignments, investigation
entries, evidence references, escalation rules, escalation events, resolution
records, feedback, exceptions, manual reviews, review summaries, status events,
feature settings, and audit evidence. Every tenant-owned table includes
`tenant_id`, `created_at`, and `updated_at`, with indexes for tenant
boundaries, student lookup, guardian visibility, complainant lookup, category,
priority, confidentiality, status, owner queue, target timing, escalation
state, feedback state, duplicate detection, conflict-of-interest checks,
exception status, review state, status event export, and audit traceability.
**Testing**: Backend unit tests for complaint intake validation, category rule
evaluation, assignment routing, conflict-of-interest blocking, status
transitions, visible/internal detail separation, escalation triggers, target
expiry routing, duplicate handling, resolution closure, feedback and reopen
rules, exception detection, review correction, summary filters, and audit
emission; integration and contract tests for `/api/v1/` complaint routes;
authorization, tenant-isolation, feature-flag, status-event, and audit tests;
web journey tests for guardian, student, staff submitter, complaint manager,
investigator, resolver, escalation reviewer, administrator, reviewer, and
auditor workflows, plus conditional mobile journey tests for guardian and
student complaint surfaces where existing mobile clients host them.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration and complaint management web app, guardian-facing complaint
submission and tracking views, student-facing complaint submission and tracking
views where enabled, staff complaint queues, and optional mobile complaint
submission and tracking surfaces. Phase 8 consumes prior evidence references
only through authorized read boundaries.
**Project Type**: Modular monolith SaaS with web, backend, and optional mobile
clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: complete complaint
submission in under 2 minutes, categorize and assign a complaint in under 60
seconds, record next handling action in under 60 seconds, expose escalation
events to the configured owner within 1 minute for 95% of eligible cases, allow
complainants to find current status in under 30 seconds, allow feedback or
reopen requests in under 60 seconds, allow auditors to trace a complaint
lifecycle in under 60 seconds, and make 95% of eligible complaint status events
available to later communication capabilities within 2 minutes.
**Constraints**: Phase 8 only. Complaints and escalation records are
tenant-scoped and feature-gated. Complaint submissions are authenticated by
default. Student-initiated complaints require school-enabled categories.
Guardian complaint access requires an approved active guardian link unless the
guardian personally submitted the complaint and school rules allow that view.
Complaint subjects and conflicted users cannot assign, decide, close, or view
restricted complainant details unless an authorized reviewer records a limited
exception. Exact duplicate active complaints are blocked or return the existing
reference; overlapping non-identical complaints are linked, grouped, or routed
to manual review. Target expiry routes to escalation or manual review and never
silently closes or dismisses the complaint. Phase 8 must not create attendance,
campus gate, scan, transport, wallet, learning reward, request approval,
medical, emergency, broad messaging, broadcast, document, search, or broad
admin dashboard outcomes.
**Scale/Scope**: Four Phase 8 modules from `PLAN.md`: Complaint Submission,
Complaint Categorization, Escalation Workflow, and Feedback & Resolution.
Supporting complaint history, exception handling, manual review, configuration,
review summaries, audit, and communication-eligibility status events are
included only where required by the Phase 8 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  is marked Reviewed and defines user stories, acceptance scenarios,
  requirements, entities, edge cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 8:
  Complaints & Escalations and maps to Complaint Submission, Complaint
  Categorization, Escalation Workflow, and Feedback & Resolution.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  complaint records, capability keys, backend feature enforcement, and
  web/mobile feature gates are required before Phase 8 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, approved guardian-link visibility,
  student self-scope, complainant ownership, assignment authority,
  conflict-of-interest restrictions, reviewer authorization, feature
  availability checks, and auditable access decisions are scoped.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, versioned complaint category and escalation rule records, and
  idempotent complaint submission, categorization, assignment, escalation,
  resolution, feedback, reopen, correction, and review commands are planned.
- **Offline NFC integrity**: PASS. Phase 8 does not create NFC, QR, attendance,
  campus access, or transport scan records. Prior evidence references are
  read-only and permission-scoped, and no scan outcome mutation is introduced.
- **Observability and testing**: PASS. Structured logs, audit events, complaint
  queue metrics, escalation aging metrics, feedback metrics, exception metrics,
  error reporting, backend tests, contract tests, authorization tests,
  tenant-isolation tests, status-event tests, and critical UI journey tests are
  scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, separate case
  management system, document store, scan subsystem, broad dashboard platform,
  or broad CQRS patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 8 scope, keep tenant and permission
enforcement explicit, define authenticated intake, category-driven triage,
conflict-of-interest restrictions, escalation routing, target expiry behavior,
duplicate handling, privacy boundaries, status event boundaries, and introduce
no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/009-complaints-escalations/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── complaint-categorization.md
│   ├── complaint-configuration.md
│   ├── complaint-history-review.md
│   ├── complaint-submission.md
│   ├── escalation-workflow.md
│   └── feedback-resolution.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)
```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Complaints/
│   │   ├── Common/
│   │   ├── Submission/
│   │   ├── Categorization/
│   │   ├── Escalations/
│   │   ├── Resolutions/
│   │   ├── History/
│   │   ├── Reviews/
│   │   ├── Configuration/
│   │   ├── Audit/
│   │   └── Seed/
│   └── tests/SafeSchool.Api.Tests/Features/Complaints/
├── admin-web/
│   ├── src/app/(school)/complaints/
│   ├── src/app/(guardian)/complaints/
│   ├── src/app/(student)/complaints/
│   ├── src/features/complaints/
│   └── tests/complaints/
└── mobile/
    ├── lib/features/complaints/
    └── test/features/complaints/

tests/
├── contracts/complaints/
└── e2e/complaints/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own complaint intake, category rule evaluation,
assignment, conflict-of-interest checks, investigation entries, escalation
routing, target expiry handling, resolution, feedback, reopen, exception
review, rule settings, persistence, and audit events. Web code owns school
administrator, complaint manager, investigator, resolver, escalation reviewer,
guardian, student, staff submitter, reviewer, and auditor workflows. Mobile
code is limited to existing mobile complaint submission and status tracking
surfaces where needed; no attendance, gate, transport, wallet, request,
medical, document, search, or broad messaging workflow is introduced. The
current repository contains planning artifacts only, so these source paths are
the implementation target for the later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | No exception required | No simpler alternative rejected |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, prior evidence reference
boundaries, tenant and feature capability enforcement, authenticated intake,
category-driven triage, conflict-of-interest restrictions, visible versus
internal details, escalation routing, target expiry, high-risk complaint
handling, feedback and reopen rules, duplicate handling, exception review,
notification boundary, document/search boundary, observability, testing
strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for complaint submission,
complaint categorization, escalation workflow, feedback and resolution,
complaint configuration, complaint history, exception review, corrections, and
review summaries. These artifacts define entities, state transitions,
validation rules, `/api/v1/` interface behavior, idempotency requirements,
guardian and student visibility boundaries, conflict-of-interest audit
requirements, status events, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the six
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
complaint privacy filtering, conflict-of-interest controls, audit evidence,
idempotency, no-side-effect boundaries, and validation coverage where it touches
those concerns.
