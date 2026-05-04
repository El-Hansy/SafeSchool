# Implementation Plan: Phase 7 Medical & Emergency

**Branch**: `008-medical-emergency` | **Date**: 2026-05-05 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/008-medical-emergency/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 7 implements secure medical and emergency workflows for the School NFC
platform. The implementation approach adds a Medical feature area for student
medical profiles, conditions, allergies, medication instructions, care plans,
emergency contacts, consent records, guardian update review, emergency access,
break-glass access, optional offline critical medical cache, medical incident
logging, care actions, medication administration evidence, medical notification
requests, contact attempts, acknowledgements, history, exceptions, manual
review, configuration, review summaries, status events, and audit evidence.
All behavior is scoped to a school account, gated by tenant feature
configuration, protected by RBAC and permission checks, backed by PostgreSQL
persistence, and observable through audit evidence. Phase 7 may consume prior
student identity evidence for emergency lookup and may expose medical status
events to later communication capabilities, but it does not create attendance,
campus access, transport, wallet, learning reward, request approval,
complaint, broad messaging, document, search, diagnosis, prescription, or broad
admin dashboard outcomes.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
medical staff, guardian, and limited student web surfaces, and Flutter/Dart
only where existing mobile clients host emergency access, medical profile, or
incident views. Exact package versions are pinned when runtime manifests are
created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
roles and permissions, tenant feature configuration, audit/event logging,
prior NFC or QR identity evidence for emergency lookup, Next.js App Router,
TanStack Query or typed server-driven data access, optional mobile offline
cache primitives for emergency essentials, and Spec Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned student medical
profiles, medical condition records, allergy records, medication instructions,
care plans, emergency contacts, medical consent records, guardian medical
update submissions, emergency access sessions, break-glass access events,
offline cache access evidence, medical incidents, care actions, medical
notification requests, medical contact attempts, medical exceptions, manual
medical reviews, medical rule settings, medical review summaries, medical
status events, feature settings, and audit evidence. Every tenant-owned table
includes `tenant_id`, `created_at`, and `updated_at`, with indexes for tenant
boundaries, student lookup, guardian visibility, emergency access lookup,
break-glass review queues, incident severity, medication instruction expiry,
consent state, notification acknowledgement state, contact failure state,
duplicate/conflict detection, exception status, review state, summary filters,
status event export, and audit traceability.
**Testing**: Backend unit tests for medical profile validation, guardian
update review, consent state, critical emergency view filtering, break-glass
authorization, 30-minute session expiry, 24-hour offline cache freshness,
incident lifecycle, care action preservation, medication administration
evidence, notification audience defaults, contact attempts, acknowledgement
state, duplicate and conflict handling, exception detection, correction and
reopen behavior, history filters, summary visibility, and audit emission;
integration and contract tests for `/api/v1/` medical routes; authorization,
tenant-isolation, feature-flag, offline-cache, and audit tests; and web/mobile
journey tests for nurse, clinic staff, emergency-authorized staff, guardian,
administrator, reviewer, and auditor workflows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration and medical staff web app, guardian-facing medical and incident
views, limited student medical summary views where enabled, and optional mobile
emergency access views. Phase 7 consumes student identity evidence but does not
implement scan capture workflows.
**Project Type**: Modular monolith SaaS with web, backend, and optional mobile
clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: complete medical profile
create or update in under 3 minutes, critical emergency profile open in under
30 seconds, complete medical incident logging in under 2 minutes,
high-severity notification requests available within 2 minutes, guardian
medical lookup in under 30 seconds, auditor lifecycle trace in under 60
seconds, and 95% of eligible medical status changes available to later
communication and notification capabilities within 2 minutes.
**Constraints**: Phase 7 only. Medical and emergency records are
tenant-scoped and feature-gated. Guardian-submitted updates always enter
pending school medical review before becoming school-verified evidence.
Emergency access sessions last 30 minutes and require re-confirmation after
expiry. Break-glass access is limited to school-configured pre-authorized
emergency roles and routes to mandatory review. Optional offline emergency
cache is usable only when synced within 24 hours; stale cache access requires
warning, reason, and review. High-severity medical incidents default
notification audience to approved guardians, emergency contacts, assigned
nurse or clinic staff, and the school emergency coordinator. Exact duplicates
are rejected or treated as already processed; conflicting non-identical records
route to manual review. Phase 7 must not diagnose, prescribe, replace school
emergency protocols, or create attendance, campus gate, scan, transport,
wallet, learning reward, request approval, complaint, broad messaging,
broadcast, document, search, or broad admin dashboard outcomes.
**Scale/Scope**: Four Phase 7 modules from `PLAN.md`: Medical Record,
Emergency Access, Medical Incident Logging, and Medical Notification.
Supporting medical history, exception handling, manual review, configuration,
review summaries, offline emergency essentials, audit, and
notification-eligibility status events are included only where required by the
Phase 7 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, clarifications, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 7:
  Medical & Emergency and maps to Medical Record, Emergency Access, Medical
  Incident Logging, and Medical Notification.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  medical records, capability keys, backend feature enforcement, and web/mobile
  feature gates are required before Phase 7 workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, approved guardian-link visibility,
  medical role authorization, pre-authorized emergency role break-glass access,
  reviewer authorization, feature availability checks, and auditable access
  decisions are scoped for sensitive health data.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, versioned medical rule records, and idempotent medical profile,
  guardian update, emergency access, incident, notification, contact,
  correction, and review commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 7 does not create NFC, QR, attendance,
  campus access, or transport scan records. Optional emergency offline access
  uses a minimal critical medical cache with 24-hour freshness, stale-cache
  review routing, syncable access evidence, and no scan outcome mutation.
- **Observability and testing**: PASS. Structured logs, audit events, medical
  access metrics, break-glass review metrics, incident metrics, notification
  and acknowledgement metrics, exception metrics, error reporting, backend
  tests, contract tests, authorization tests, tenant-isolation tests,
  offline-cache tests, and critical UI journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, separate
  clinical system, diagnosis engine, prescription system, document store, scan
  subsystem, or broad CQRS patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 7 scope, keep tenant and permission
enforcement explicit, define break-glass restrictions, 30-minute emergency
access expiry, 24-hour offline cache freshness, notification audience defaults,
duplicate handling, privacy boundaries, and status event boundaries, and
introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/008-medical-emergency/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── emergency-access.md
│   ├── medical-configuration.md
│   ├── medical-history-review.md
│   ├── medical-incident-logging.md
│   ├── medical-notifications.md
│   └── medical-records.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Medical/
│   │   ├── Records/
│   │   ├── Guardians/
│   │   ├── EmergencyAccess/
│   │   ├── OfflineCache/
│   │   ├── Incidents/
│   │   ├── CareActions/
│   │   ├── Notifications/
│   │   ├── Contacts/
│   │   ├── Exceptions/
│   │   ├── Reviews/
│   │   ├── Configuration/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/Medical/
├── admin-web/
│   ├── src/app/(school)/medical/
│   ├── src/app/(guardian)/medical/
│   ├── src/app/(student)/medical/
│   ├── src/features/medical/
│   └── tests/medical/
└── mobile/
    ├── lib/features/medical/
    └── test/features/medical/

tests/
├── contracts/medical/
└── e2e/medical/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own medical profile management, guardian update
review, consent and visibility enforcement, emergency access, break-glass
access, optional offline critical medical cache access evidence, incident
logging, care actions, medication evidence, notification requests, contact
attempts, acknowledgements, exception review, rule settings, persistence, and
audit events. Web code owns school administrator, nurse, clinic staff,
medical coordinator, emergency staff, reviewer, guardian, and limited student
medical workflows. Mobile code is limited to existing mobile emergency
lookup, critical profile, and incident capture surfaces where needed; no
attendance, gate, transport, wallet, request, document, or broad messaging
workflow is introduced. The current repository contains planning artifacts
only, so these source paths are the implementation target for the later
`/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | No exception required | No simpler alternative rejected |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, tenant and feature capability
enforcement, medical privacy roles, guardian medical update review,
emergency access and break-glass rules, 30-minute session expiry, optional
offline cache freshness, incident lifecycle, care action evidence,
medication administration boundaries, medical notification audience defaults,
contact attempts, duplicate and conflict handling, exception review,
notification boundary, observability, testing strategy, and implementation
structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for medical records, emergency
access, medical incident logging, medical notifications, medical
configuration, medical history, exception review, corrections, and review
summaries. These artifacts define entities, state transitions, validation
rules, `/api/v1/` interface behavior, idempotency requirements, guardian
visibility boundaries, break-glass audit requirements, offline cache access
evidence, status events, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the five
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
medical privacy filtering, audit evidence, idempotency, no-side-effect
boundaries, and validation coverage where it touches those concerns.
