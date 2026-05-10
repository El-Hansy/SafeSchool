# Implementation Plan: Phase 9 Communication & Notifications

**Branch**: `010-communication-notifications` | **Date**: 2026-05-06 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/010-communication-notifications/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 9 implements centralized communication and notification workflows for
the School NFC platform. The implementation approach adds a Communications
feature area for notification source events, notification records, direct
message conversations, conversation participants, messages, broadcast and
announcement records, audience rules, recipient snapshots, communication
templates, delivery attempts, read states, acknowledgements, preferences,
moderation reviews, communication exceptions, communication review summaries,
configuration, lifecycle events, and audit evidence. All behavior is scoped to
a school account, gated by tenant feature configuration, protected by RBAC and
permission checks, backed by PostgreSQL persistence, and observable through
audit evidence. Phase 9 consumes eligible status events from prior phases and
delivers in-app notifications plus configured external channel attempts, but it
does not create attendance, campus access, scan, transport, wallet, learning
reward, request approval, medical, emergency, complaint resolution, document,
search, or broad admin dashboard outcomes.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
staff, guardian, and student web surfaces, and Flutter/Dart only where existing
mobile clients host notification, messaging, or acknowledgement views. Exact
package versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
roles and permissions, tenant feature configuration, audit/event logging,
eligible status events from attendance, campus access, transport, wallet,
learning, requests, medical, emergency, and complaints modules, provider
adapters for school-enabled external channels, Next.js App Router, TanStack
Query or typed server-driven data access, and Spec Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned conversations,
conversation participants, messages, broadcasts, announcements, audience
rules, recipient snapshots, notification source events, notification records,
communication templates, delivery attempts, communication preferences,
acknowledgement records, moderation reviews, communication exceptions,
communication review summaries, communication feature settings, communication
lifecycle events, and audit evidence. Every tenant-owned table includes
`tenant_id`, `created_at`, and `updated_at`, with indexes for tenant
boundaries, recipient lookup, guardian visibility, student lookup, staff
lookup, source event lookup, category, priority, conversation status,
publication status, delivery state, read state, acknowledgement state,
moderation state, exception status, template version, recipient snapshot,
duplicate suppression, review state, lifecycle export, and audit traceability.
**Testing**: Backend unit tests for source event intake, notification routing,
recipient resolution, guardian-link visibility, student self-scope, direct
message authorization, conversation state transitions, broadcast audience
rules, publication approval, template validation, quiet-hour behavior,
preference rules, mandatory category overrides, delivery state transitions,
duplicate suppression, acknowledgement deadlines, moderation routing,
exception detection, correction and withdrawal, history filters, summary
visibility, and audit emission; integration and contract tests for `/api/v1/`
communication routes; authorization, tenant-isolation, feature-flag,
idempotency, delivery, read-state, acknowledgement, moderation, and audit
tests; and web/mobile journey tests for guardian, student, staff,
communication manager, administrator, moderator, reviewer, and auditor
workflows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration and communication management web app, guardian-facing
notification and message views, student-facing notification and message views
where enabled, staff message and broadcast surfaces, and optional mobile
notification, message, and acknowledgement surfaces. Phase 9 consumes source
events from prior modules only through authorized read boundaries.
**Project Type**: Modular monolith SaaS with web, backend, and optional mobile
clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: show notification lists
and unread counts in under 30 seconds, create notification records for 95% of
eligible status events within 2 minutes, send scoped direct messages in under
60 seconds, show replies in under 30 seconds, publish complete broadcasts in
under 2 minutes, record 95% of enabled delivery channel attempt outcomes within
5 minutes, identify failed or overdue communications in under 60 seconds,
apply optional preference changes within 2 minutes, trace communication
lifecycles in under 60 seconds, and preserve rule versions for 100% of sampled
configuration changes.
**Constraints**: Phase 9 only. Communication records are tenant-scoped,
feature-gated, authenticated by default, and permission-scoped. Source events
from earlier phases are read-only triggers and must not be mutated by Phase 9.
In-app notification center behavior is the baseline delivery surface. External
channels are school-configured and represented by auditable delivery attempts,
not guaranteed recipient attention. Mandatory categories may override optional
preferences and quiet hours according to school policy. Recipient snapshots are
preserved at send or publication time. Exact duplicate source events,
duplicate recipients, and repeated delivery intents are suppressed or merged
with evidence. Phase 9 must not create attendance, campus gate, scan,
transport, wallet, learning reward, request approval, medical, emergency,
complaint resolution, document storage, global search, or broad admin
dashboard outcomes.
**Scale/Scope**: Three Phase 9 modules from `PLAN.md`: Messaging System,
Broadcast & Announcement, and Notification System. Supporting communication
history, exception handling, moderation review, configuration, preferences,
review summaries, delivery evidence, audit, and lifecycle events are included
only where required by the Phase 9 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  is marked Reviewed and defines user stories, acceptance scenarios,
  requirements, entities, edge cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 9:
  Communication & Notifications and maps to Messaging System, Broadcast &
  Announcement, and Notification System.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  communication records, capability keys, backend feature enforcement, and
  web/mobile feature gates are required before Phase 9 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, approved guardian-link visibility,
  student self-scope, sender authority, recipient eligibility, audience
  authority, moderation authority, reviewer authorization, feature
  availability checks, and auditable access decisions are scoped.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, versioned template/rule records, idempotent source event,
  notification, message, broadcast, delivery retry, acknowledgement,
  preference, correction, moderation, and review commands are planned where
  applicable.
- **Offline NFC integrity**: PASS. Phase 9 does not create NFC, QR, attendance,
  campus access, or transport scan records. It consumes eligible source events
  from prior modules and preserves delivery evidence without mutating scan or
  attendance outcomes.
- **Observability and testing**: PASS. Structured logs, audit events,
  communication lifecycle metrics, delivery metrics, acknowledgement metrics,
  moderation metrics, exception metrics, error reporting, backend tests,
  contract tests, authorization tests, tenant-isolation tests, feature-flag
  tests, idempotency tests, delivery tests, and critical UI journey tests are
  scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. A PostgreSQL-backed delivery outbox and attempt
  records are used inside the existing platform boundary. No new microservice,
  broker, separate communication database, broad document store, global search
  system, or broad CQRS pattern is introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 9 scope, keep tenant and permission
enforcement explicit, define source event boundaries, recipient snapshots,
template/rule versioning, preference and quiet-hour behavior, moderation,
delivery attempts, acknowledgement tracking, duplicate suppression,
restricted-detail minimization, no-side-effect boundaries, and introduce no
constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/010-communication-notifications/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── broadcast-announcement.md
│   ├── communication-configuration.md
│   ├── communication-history-review.md
│   ├── delivery-acknowledgement.md
│   ├── direct-messaging.md
│   └── notification-center.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Communications/
│   │   ├── Common/
│   │   ├── Notifications/
│   │   ├── Messaging/
│   │   ├── Broadcasts/
│   │   ├── Delivery/
│   │   ├── Preferences/
│   │   ├── History/
│   │   ├── Moderation/
│   │   ├── Configuration/
│   │   ├── Audit/
│   │   └── Seed/
│   └── tests/SafeSchool.Api.Tests/Features/Communications/
├── admin-web/
│   ├── src/app/(school)/communications/
│   ├── src/app/(guardian)/communications/
│   ├── src/app/(student)/communications/
│   ├── src/features/communications/
│   └── tests/communications/
└── mobile/
    ├── lib/features/communications/
    └── test/features/communications/

tests/
├── contracts/communications/
└── e2e/communications/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own source event intake, notification generation,
recipient resolution, direct messaging, broadcasts, announcements, delivery
attempts, read state, acknowledgements, preferences, quiet-hour handling,
template and rule settings, moderation, exception review, persistence, and
audit events. Web code owns school administrator, communication manager,
moderator, staff, guardian, student, reviewer, and auditor workflows. Mobile
code is limited to existing mobile notification, message, and acknowledgement
surfaces where needed; no NFC, attendance, transport, wallet, request, medical,
complaint resolution, document, search, or broad dashboard workflow is
introduced. The current repository contains planning artifacts only, so these
source paths are the implementation target for the later `/speckit.tasks`
output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | No exception required | No simpler alternative rejected |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, prior phase source event
boundaries, notification center baseline, external channel treatment,
direct-message scope, recipient snapshots, template and rule versioning,
preferences, quiet-hour behavior, moderation, delivery attempts,
acknowledgements, duplicate suppression, restricted-detail minimization,
document/search boundaries, observability, testing strategy, and
implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for notification center, direct
messaging, broadcast and announcement publication, delivery and
acknowledgement tracking, communication configuration, and communication
history/review workflows. These artifacts define tenant-owned entities,
state transitions, validation rules, `/api/v1/` route contracts, idempotency
expectations, pagination, audit behavior, restricted-detail boundaries, and
no-side-effect constraints for Phase 9.
