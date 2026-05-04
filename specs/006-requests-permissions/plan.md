# Implementation Plan: Phase 6 Requests & Permissions

**Branch**: `007-requests-permissions` | **Date**: 2026-05-05 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/006-requests-permissions/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 6 implements structured request and permission workflows for the School
NFC platform. The implementation approach adds a Requests feature area for
outing requests, early leave requests, configurable request types, versioned
approval workflow templates, assigned workflow steps, guardian consent, staff
decisions, star-based permission rules, request history, exceptions, manual
review, corrections, and review summaries. All behavior is scoped to a school
account, gated by tenant feature configuration, protected by RBAC and
permission checks, backed by PostgreSQL persistence, and observable through
audit evidence. Phase 6 exposes approved request evidence to authorized users
but does not create attendance, campus access, transport, wallet, learning,
medical, complaint, messaging, document, search, or broad admin dashboard
outcomes.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
guardian, and student web surfaces, and Flutter/Dart only where existing mobile
clients host request submission, status tracking, or approval views. Exact
package versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
roles and permissions, tenant feature configuration, audit/event logging,
Phase 5 star and reward evidence for star-gated requests, Next.js App Router,
TanStack Query or typed server-driven data access, and Spec Kit planning
artifacts.
**Storage**: Single PostgreSQL database for tenant-owned permission requests,
outing details, early leave details, request types, workflow templates,
workflow template versions, workflow steps, workflow decisions, guardian
consent records, star permission rules, star rule evaluations, pickup evidence,
request exceptions, manual request reviews, request review summaries, feature
settings, and audit evidence. Every tenant-owned table includes `tenant_id`,
`created_at`, and `updated_at`, with indexes for tenant boundaries, student
lookup, requester lookup, guardian visibility, request type lookup, status and
assignee queues, workflow version lookup, duplicate and overlap detection,
approval expiry routing, star rule evaluation, exception status, review state,
summary filters, and audit traceability.
**Testing**: Backend unit tests for request creation, required field
validation, duplicate and overlap handling, workflow template validation,
workflow step state transitions, decision idempotency, expiry escalation,
guardian consent, early leave release eligibility, star reservation and
release behavior, exception detection, correction and reopen behavior, request
history filters, summary visibility, and audit emission; integration and
contract tests for `/api/v1/` request routes; authorization, tenant-isolation,
feature-flag, and audit tests; and web/mobile journey tests for student,
guardian, approver, request manager, and reviewer workflows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration web app, guardian-facing request views, student request views
where enabled, and optional mobile request views. Phase 6 has no NFC, QR, or
offline scan capture requirement.
**Project Type**: Modular monolith SaaS with web, backend, and optional mobile
clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: eligible request
submission in under 2 minutes, assigned approver decision in under 60 seconds,
approved early leave release evidence visible within 1 minute, request search
over the last 90 days in under 30 seconds, auditor traceability in under 60
seconds, and 95% of eligible request status changes available to later
notification capabilities within 2 minutes.
**Constraints**: Phase 6 only. Requests are tenant-scoped and feature-gated.
Guardian consent defaults to any one authorized guardian unless a request type
requires stricter consent. Early leave release eligibility requires a
guardian-selected authorized pickup person plus staff verification note.
Workflow step expiry routes to manual review or configured escalation while
keeping the request pending. Exact active duplicates are blocked and
overlapping non-identical active requests route to manual review. Star-gated
requests reserve stars at submission, consume on final approval, and release on
denial, withdrawal, or expiry while Phase 5 remains the star source of truth.
Phase 6 must not create attendance, campus gate, scan, transport, wallet,
learning content, medical, complaint, broad messaging, broadcast, document,
search, or broad admin dashboard outcomes.
**Scale/Scope**: Four Phase 6 modules from `PLAN.md`: Outing Request,
Star-Based Permission Rules, Early Leave Request, and Approval Workflow Engine.
Supporting request history, exception handling, corrections, configuration,
review summaries, audit, and notification-eligibility records are included only
where required by the Phase 6 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, clarifications, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 6:
  Requests & Permissions and maps to Outing Request, Star-Based Permission
  Rules, Early Leave Request, and Approval Workflow Engine.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  request records, capability keys, backend feature enforcement, and web/mobile
  feature gates are required before Phase 6 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, guardian-link submission and visibility
  checks, assigned approver authorization, reviewer authorization, feature
  availability checks, and auditable access decisions are scoped for sensitive
  student release and permission actions.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, versioned workflow and request type records, and idempotent request,
  decision, consent, star reservation, pickup verification, correction, and
  review commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 6 has no NFC, QR, attendance, campus
  access, or transport scan capture requirement. Approved request evidence is
  read-only to later scan/access workflows, and no mobile SQLite scan queue or
  offline scan sync is required for this phase.
- **Observability and testing**: PASS. Structured logs, audit events, request
  lifecycle metrics, workflow queue metrics, expiry and escalation metrics,
  star-rule metrics, exception metrics, error reporting, backend tests,
  contract tests, authorization tests, tenant-isolation tests, and critical UI
  journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, separate
  workflow engine service, document store, scan subsystem, or broad CQRS
  patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 6 scope, keep tenant and permission
enforcement explicit, define workflow versioning, star reservation boundaries,
duplicate handling, and early leave release evidence, and introduce no
constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/006-requests-permissions/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── approval-workflow.md
│   ├── early-leave.md
│   ├── outing-permission-requests.md
│   ├── request-configuration.md
│   ├── request-history-review.md
│   └── star-permission-rules.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Requests/
│   │   ├── Requests/
│   │   ├── Outings/
│   │   ├── EarlyLeave/
│   │   ├── Workflows/
│   │   ├── Decisions/
│   │   ├── GuardianConsent/
│   │   ├── StarRules/
│   │   ├── Exceptions/
│   │   ├── Reviews/
│   │   ├── Configuration/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/Requests/
├── admin-web/
│   ├── src/app/(school)/requests/
│   ├── src/app/(guardian)/requests/
│   ├── src/app/(student)/requests/
│   ├── src/features/requests/
│   └── tests/requests/
└── mobile/
    ├── lib/features/requests/
    └── test/features/requests/

tests/
├── contracts/requests/
└── e2e/requests/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own request creation, outing and early leave details,
workflow templates, workflow versioning, step assignment, decision validation,
guardian consent, star rule evaluation, pickup verification, duplicate and
overlap handling, exception review, persistence, and audit events. Web code
owns school administrator, request manager, staff approver, reviewer, guardian,
and student-facing request workflows. Mobile code is limited to existing
mobile request submission, status, and approval surfaces where needed; no
offline scan storage is introduced. The current repository contains planning
artifacts only, so these source paths are the implementation target for the
later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0, Phase 1, and Phase 5 dependency use, tenant and feature
capability enforcement, request permissions, request lifecycle states,
workflow versioning, guardian consent defaults, workflow expiry behavior,
early leave pickup evidence, star reservation and consumption behavior,
duplicate and overlap handling, notification boundary, observability, testing
strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for outing and permission
requests, approval workflow decisions, early leave release eligibility,
star-based permission rules, request configuration, request history, exception
review, corrections, and review summaries. These artifacts define entities,
state transitions, validation rules, `/api/v1/` interface behavior,
idempotency requirements, guardian visibility boundaries, audit evidence, and
verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the six
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
workflow version handling, audit evidence, and validation coverage where it
touches those concerns.
