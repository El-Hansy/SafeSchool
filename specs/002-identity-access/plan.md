# Implementation Plan: Phase 1 Identity & Access

**Branch**: `002-identity-access` | **Date**: 2026-05-03 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-identity-access/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 1 builds the core identity and access capability for the School NFC
platform: student profiles, guardian links, NFC card credentials, QR fallback
credentials, role-based access, and permission enforcement. The implementation
approach follows the constitution baseline: a modular monolith backend, a
school administration web surface, and a mobile identity credential surface only
where NFC/QR device interaction is required. All Phase 1 behavior is scoped to a
school account, gated by tenant feature configuration, protected by RBAC and
permission checks, and backed by audit evidence.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for web, and Flutter/Dart
for mobile credential workflows when native NFC or QR interaction is needed.
Exact package versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, Next.js App Router, TanStack Query or typed server-driven data
access, Flutter NFC/QR platform integrations, and Spec Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned identity and access
records. Every tenant-owned table includes `tenant_id`, `created_at`, and
`updated_at`, with indexes for tenant boundaries, active identity identifiers,
credential references, guardian links, role assignments, and audit review.
**Testing**: Backend unit tests for domain rules, integration and contract tests
for `/api/v1/` identity/access routes, authorization and tenant-isolation tests,
web UI journey tests for administrator workflows, mobile tests for credential
read/provisioning behavior when implemented, and audit evidence review tests.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration web app, and mobile identity credential support for NFC/QR
operations.
**Project Type**: Modular monolith SaaS with web, backend, and mobile clients
organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: complete student profile
creation with duplicate checks in under 5 minutes, determine credential status
in under 60 seconds, deny 100% of sampled unauthorized actions with a reviewable
reason, and provide audit evidence for 100% of sampled sensitive changes.
**Constraints**: Phase 1 only. Excludes attendance generation, campus entry/exit
decisions, transport boarding, wallet payments, learning, requests, medical,
complaints, communication delivery, documents, and search. NFC/QR credentials
are identity evidence only until later scan-flow phases consume them.
**Scale/Scope**: Six Phase 1 modules from `PLAN.md`: Student Profile, Guardian
Linking, NFC Card Provisioning, QR Identity Fallback, Role-Based Access, and
Permission Enforcement.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 1:
  Identity & Access and maps to Student Profile, Guardian Linking, NFC Card
  Provisioning, QR Identity Fallback, Role-Based Access, and Permission
  Enforcement.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  records, tenant feature capability keys, backend enforcement, and web/mobile
  feature gates are required before any Phase 1 workflow executes.
- **Security and authorization**: PASS. JWT authentication, role assignments,
  permission checks, tenant validation, feature availability checks, and
  auditable access decisions are scoped for all sensitive actions.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, and retry-safe lifecycle commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 1 does not define attendance or
  transport scan outcomes, but it does define NFC/QR credential state, validity,
  revocation evidence, offline credential snapshots, idempotent credential
  lifecycle commands, and audit events needed by later offline scan flows.
- **Observability and testing**: PASS. Structured audit events, access-denial
  evidence, metrics for lifecycle operations, centralized error reporting,
  backend tests, API contract tests, authorization tests, and critical
  web/mobile journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, extra storage
  products, or broad CQRS patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve the Phase 1 scope, keep tenant and permission
enforcement explicit, and introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/002-identity-access/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── credential-lifecycle.md
│   ├── guardian-linking.md
│   ├── permission-enforcement.md
│   └── student-profile.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/IdentityAccess/
│   │   ├── StudentProfiles/
│   │   ├── Guardians/
│   │   ├── Credentials/
│   │   ├── AccessControl/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/IdentityAccess/
├── admin-web/
│   ├── src/app/(school)/identity-access/
│   ├── src/features/identity-access/
│   └── tests/identity-access/
└── mobile/
    ├── lib/features/identity_access/
    └── test/features/identity_access/

tests/
├── contracts/identity-access/
└── e2e/identity-access/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own business rules and persistence boundaries; web code
owns school administration workflows; mobile code is limited to credential
provisioning or validation support where NFC/QR native interaction is required.
The current repository contains planning artifacts only, so these source paths
are the implementation target for the later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, tenant and feature capability enforcement, RBAC and permission
modeling, duplicate identity detection, guardian link lifecycle, credential
lifecycle, QR fallback behavior, offline credential evidence, audit events,
testing strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for student profiles, guardian
links, credential lifecycle, and permission enforcement. These artifacts define
entities, state transitions, validation rules, `/api/v1/` interface behavior,
audit evidence, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the four
independently testable user stories in [spec.md](./spec.md), and each story
must include tenant resolution, feature flag checks, permission enforcement,
audit evidence, and validation coverage where it touches those concerns.
