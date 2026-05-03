# Implementation Plan: Phase 3 Transport & Bus Tracking

**Branch**: `004-transport-bus-tracking` | **Date**: 2026-05-04 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/004-transport-bus-tracking/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 3 implements transport visibility and bus tracking for the School NFC
platform. The implementation approach adds a Transport feature area for bus and
vehicle records, route and stop plans, student transport assignments, active
trip lifecycle, NFC/QR boarding and drop scans, offline mobile scan queues,
authorized mobile live tracking, ETA calculation, linked-guardian transport
notification records, anomaly review, and transport rule settings. All behavior
is scoped to a school account, gated by tenant feature configuration, protected
by RBAC and permission checks, backed by PostgreSQL persistence, and observable
through audit evidence.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration
and guardian web surfaces, and Flutter/Dart for mobile NFC/QR transport scans,
offline queues, and authorized active-trip location updates. Exact package
versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, guardian links, credential
status snapshots, AttendanceAccess context for campus separation only, Next.js
App Router, TanStack Query or typed server-driven data access, Flutter NFC/QR
platform integrations, mobile SQLite offline queues, mobile location
capabilities, and Spec Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned vehicles, routes,
stops, route stop sequences, student transport assignments, active trips,
offline scan sync batches, boarding/drop scan events, location updates, ETA
records, notification records, anomalies, manual transport reviews, transport
rule settings, feature settings, and audit evidence. Every tenant-owned table
includes `tenant_id`, `created_at`, and `updated_at`, with indexes for tenant
boundaries, active route and trip lookup, student assignment lookup, trip
idempotency, scan idempotency, credential references, location freshness,
guardian visibility, anomaly status, notification status, retention jobs, and
audit traceability.
**Testing**: Backend unit tests for route validation, assignment eligibility,
active-trip overlap, scan validation, idempotent sync, offline reconciliation,
ETA rules, guardian visibility windows, notification eligibility, anomaly
detection, retention, and correction state transitions; integration and
contract tests for `/api/v1/` transport routes; authorization, tenant-isolation,
feature-flag, and audit tests; mobile offline queue, NFC/QR scan, and location
update tests; and web/guardian journey tests for transport management, review,
and visibility flows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration web app, guardian-facing transport visibility, and mobile
transport scan/tracking support for NFC/QR and offline operations.
**Project Type**: Modular monolith SaaS with web, backend, and mobile clients
organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: create one active route
with at least five stops and assign ten students in under 15 minutes, record
normal boarding/drop scans in under 10 seconds, reconcile duplicate/retried/
offline scans without duplicate transport outcomes, expose 95% of accepted
boarding/drop scan events within 2 minutes of scan availability, reflect 95% of
accepted active-trip location updates within 30 seconds of update availability,
produce 95% of eligible ETA records within 60 seconds of trusted trip progress,
create or expose 95% of eligible notification records within 2 minutes, and let
reviewers trace active-trip evidence in under 60 seconds.
**Constraints**: Phase 3 only. Live tracking uses authorized staff or vehicle
mobile devices associated with active trips; dedicated bus hardware and physical
vehicle control are outside scope. Guardians see pickup ETA before accepted
boarding, exact live bus location only between accepted boarding and accepted or
reviewed drop, and drop status after drop. Detailed active-trip location
history is retained for 30 days before summary-only retention unless school
policy places an approved review hold. Phase 3 excludes campus attendance
generation, campus entry/exit decisions, wallet, learning, request, medical,
complaint, general messaging, broadcast, document, search, and broad admin
dashboard behavior.
**Scale/Scope**: Six Phase 3 modules from `PLAN.md`: Bus Assignment, Route &
Stop Management, Live Tracking, Boarding/Drop Scan, ETA Calculation, and
Transport Notification. Supporting anomaly review, manual correction, rule
settings, audit, and retention behavior are included only where required by the
Phase 3 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, clarifications, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 3:
  Transport & Bus Tracking and maps to Bus Assignment, Route & Stop Management,
  Live Tracking, Boarding/Drop Scan, ETA Calculation, and Transport
  Notification.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  transport records, capability keys, backend feature enforcement, and
  web/mobile feature gates are required before Phase 3 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access, role
  assignments, permission checks, trip staff/device authorization, feature
  availability checks, guardian-link visibility checks, and auditable access
  decisions are scoped for sensitive actions.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, and idempotent trip, scan, sync, review, and notification commands
  are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 3 defines mobile SQLite scan queues,
  credential snapshot use, scan source evidence, local and received times,
  timestamp conflict handling, delayed sync reconciliation, duplicate
  detection, idempotent sync APIs, and audit events.
- **Observability and testing**: PASS. Structured logs, audit events, scan sync
  metrics, active-trip and location metrics, anomaly metrics, error reporting,
  backend tests, contract tests, authorization tests, tenant-isolation tests,
  mobile offline tests, and critical UI journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, dedicated
  tracking hardware service, separate location store, or broad CQRS patterns
  are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 3 scope, keep tenant and permission
enforcement explicit, define offline scan idempotency and mobile live tracking
constraints, and introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/004-transport-bus-tracking/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── boarding-drop-scan.md
│   ├── bus-assignment.md
│   ├── eta-calculation.md
│   ├── live-tracking.md
│   ├── route-stop-management.md
│   ├── transport-anomaly-review.md
│   └── transport-notification.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Transport/
│   │   ├── Vehicles/
│   │   ├── Routes/
│   │   ├── Assignments/
│   │   ├── Trips/
│   │   ├── Scans/
│   │   ├── Tracking/
│   │   ├── Eta/
│   │   ├── Notifications/
│   │   ├── Anomalies/
│   │   ├── Rules/
│   │   ├── Sync/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/Transport/
├── admin-web/
│   ├── src/app/(school)/transport/
│   ├── src/app/(guardian)/transport/
│   ├── src/features/transport/
│   └── tests/transport/
└── mobile/
    ├── lib/features/transport/
    └── test/features/transport/

tests/
├── contracts/transport/
└── e2e/transport/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own route configuration, assignment rules, trip
lifecycle, scan validation, sync reconciliation, live tracking, ETA rules,
notification eligibility, anomaly review, retention, persistence, and audit
events. Web code owns school administration, transport manager, reviewer, and
guardian-facing transport visibility workflows. Mobile code is limited to NFC/QR
boarding/drop scans, offline scan storage, retry-safe sync, and authorized
active-trip location updates. The current repository contains planning
artifacts only, so these source paths are the implementation target for the
later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 1 and Phase 2 dependency use, tenant and feature capability
enforcement, transport permissions, route and assignment validation, active-trip
overlap, mobile live tracking source, NFC/QR offline scan handling, sync
idempotency, ETA calculation, guardian visibility windows, notification
eligibility, anomaly review, retention, observability, testing strategy, and
implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for route and stop management,
bus assignment, boarding/drop scan capture and sync, live tracking, ETA
calculation, transport notification records, and anomaly review. These
artifacts define entities, state transitions, validation rules, `/api/v1/`
interface behavior, idempotency requirements, guardian visibility boundaries,
retention behavior, audit evidence, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the six
independently testable user stories in [spec.md](./spec.md), and each story must
include tenant resolution, feature flag checks, permission enforcement,
idempotency or offline behavior where applicable, audit evidence, and validation
coverage where it touches those concerns.
