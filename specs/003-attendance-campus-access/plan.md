# Implementation Plan: Phase 2 Attendance & Campus Access

**Branch**: `003-attendance-campus-access` | **Date**: 2026-05-03 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-attendance-campus-access/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 2 implements secure campus entry/exit scanning and attendance automation
for the School NFC platform. The implementation approach extends the existing
foundation and identity phases with an AttendanceAccess feature area for gate
configuration, NFC/QR gate scans, offline mobile scan queues, idempotent scan
sync, attendance generation, linked-guardian entry/exit notification records,
and anomaly review. All behavior is scoped to a school account, gated by
tenant feature configuration, protected by RBAC and permission checks, backed by
PostgreSQL persistence, and observable through audit evidence.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration
web surfaces, and Flutter/Dart for mobile scan capture where NFC, QR, and
offline operation are required. Exact package versions are pinned when runtime
manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess credential status snapshots and guardian links,
Next.js App Router, TanStack Query or typed server-driven data access, Flutter
NFC/QR platform integrations, mobile SQLite offline queues, and Spec Kit
planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned gates, scan points,
scan events, campus access decisions, attendance sessions, attendance records,
notification records, anomalies, review actions, and audit evidence. Every
tenant-owned table includes `tenant_id`, `created_at`, and `updated_at`, with
indexes for tenant boundaries, scan idempotency, credential references, student
attendance lookup, session/day lookup, anomaly status, guardian notification
lookup, and audit traceability.
**Testing**: Backend unit tests for scan validation, idempotency, attendance
rules, notification eligibility, anomaly detection, and correction state
transitions; integration and contract tests for `/api/v1/` attendance-access
routes; authorization, tenant-isolation, feature-flag, and audit tests; mobile
offline queue and sync tests; and web/guardian journey tests for review and
visibility flows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration web app, guardian-facing entry/exit visibility, and mobile gate
scan support for NFC/QR operations.
**Project Type**: Modular monolith SaaS with web, backend, and mobile clients
organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: record normal gate scans
in under 10 seconds, reconcile duplicate/retried/offline scans without duplicate
attendance records, generate attendance for 95% of accepted scans within 2
minutes of scan availability, create or expose 95% of eligible notification
records within 2 minutes, and let reviewers trace scan outcomes in under 60
seconds.
**Constraints**: Phase 2 only. Excludes bus boarding, route tracking, wallet
transactions, learning engagement, outing and early-leave workflows, medical
workflows, complaints, general messaging, broadcasts, document management,
search, broad admin dashboards, and physical gate hardware control.
**Scale/Scope**: Four Phase 2 modules from `PLAN.md`: Gate Scan Flow,
Attendance Generation, Entry/Exit Notification, and Attendance Anomaly
Detection.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 2:
  Attendance & Campus Access and maps to Gate Scan Flow, Attendance Generation,
  Entry/Exit Notification, and Attendance Anomaly Detection.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  records, capability keys, backend feature enforcement, and web/mobile feature
  gates are required before Phase 2 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, scan point authorization, feature
  availability checks, guardian-link visibility checks, and auditable access
  decisions are scoped for sensitive actions.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, and idempotent scan sync commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 2 defines mobile SQLite scan queues,
  credential snapshot use, scan source evidence, timestamp conflict handling,
  delayed sync reconciliation, duplicate detection, idempotent sync APIs, and
  audit events.
- **Observability and testing**: PASS. Structured logs, audit events, scan sync
  metrics, anomaly metrics, error reporting, backend tests, contract tests,
  authorization tests, tenant-isolation tests, mobile offline tests, and
  critical UI journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, extra storage
  products, physical gate control service, or broad CQRS patterns are
  introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 2 scope, keep tenant and permission
enforcement explicit, define offline scan idempotency, and introduce no
constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/003-attendance-campus-access/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── anomaly-detection.md
│   ├── attendance-generation.md
│   ├── entry-exit-notification.md
│   └── gate-scan-flow.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/AttendanceAccess/
│   │   ├── Gates/
│   │   ├── Scans/
│   │   ├── Attendance/
│   │   ├── Notifications/
│   │   ├── Anomalies/
│   │   ├── Sync/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/AttendanceAccess/
├── admin-web/
│   ├── src/app/(school)/attendance-access/
│   ├── src/features/attendance-access/
│   └── tests/attendance-access/
└── mobile/
    ├── lib/features/attendance_access/
    └── test/features/attendance_access/

tests/
├── contracts/attendance-access/
└── e2e/attendance-access/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own scan validation, attendance rules, notification
eligibility, anomaly detection, review actions, persistence, and audit events.
Web code owns school administration and reviewer workflows. Mobile code is
limited to NFC/QR gate scanning, offline scan storage, and sync behavior. The
current repository contains planning artifacts only, so these source paths are
the implementation target for the later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 1 dependency use, tenant and feature capability enforcement,
gate and scan point authorization, NFC/QR offline scan handling, sync
idempotency, attendance generation rules, guardian notification eligibility,
anomaly review, observability, testing strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for gate scan flow, attendance
generation, entry/exit notification records, and anomaly detection. These
artifacts define entities, state transitions, validation rules, `/api/v1/`
interface behavior, idempotency requirements, audit evidence, and verification
steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the four
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
offline or idempotency behavior where applicable, audit evidence, and validation
coverage where it touches those concerns.
