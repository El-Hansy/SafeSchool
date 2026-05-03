<!--
Sync Impact Report
Version change: unversioned template -> 1.0.0
Modified principles:
- Placeholder principle 1 -> I. Spec-First Delivery
- Placeholder principle 2 -> II. Modular Multi-Tenant SaaS Architecture
- Placeholder principle 3 -> III. Tenant Configuration and Feature Flag Enforcement
- Placeholder principle 4 -> IV. Offline NFC Integrity and Idempotent Sync
- Placeholder principle 5 -> V. Security, Observability, and Testable Quality
Added sections:
- Platform Constraints
- Development Workflow & Quality Gates
Removed sections:
- Placeholder-only template examples and guidance comments
Templates requiring updates:
- ✅ .specify/templates/plan-template.md
- ✅ .specify/templates/spec-template.md
- ✅ .specify/templates/tasks-template.md
- ⚠ .specify/templates/commands/*.md not present in this checkout
- ✅ AGENTS.md
Follow-up TODOs:
- None
-->
# School NFC Platform Constitution

## Core Principles

### I. Spec-First Delivery
Every feature MUST begin as a reviewed specification before implementation.
Specs MUST define user stories, acceptance criteria, data entities, API
contracts, edge cases, assumptions, and validation approach. The approved spec
is the single source of truth for product and engineering until it is amended
through the same review process.

Rationale: The project plan defines "User Stories -> Specs -> Implementation ->
Validation" as the delivery model to reduce ambiguity and keep decisions
traceable.

### II. Modular Multi-Tenant SaaS Architecture
The system MUST be designed as a modular, multi-tenant SaaS platform. The
default architecture is a modular monolith organized by feature/domain modules,
with microservices or event-driven extraction allowed only after measured scale
or operational pressure justifies the added cost and complexity.

Rationale: A feature-based modular monolith keeps infrastructure cost low while
preserving a clear path to extract heavy modules such as transport or wallet
when production evidence requires it.

### III. Tenant Configuration and Feature Flag Enforcement
Every feature MUST be tenant-aware and feature-flag controlled. Backend
endpoints MUST resolve tenant context, validate tenant access, enforce role and
permission rules, and check feature availability before executing business
logic. UI feature gates are required for usability but MUST NOT be treated as an
authorization boundary.

Rationale: The platform serves multiple schools with configurable capabilities;
tenant isolation and feature gating are non-negotiable security and product
requirements.

### IV. Offline NFC Integrity and Idempotent Sync
NFC, QR fallback, attendance, campus access, and transport scan flows MUST work
offline where the user journey depends on scan continuity. Mobile clients MUST
store offline scans, cached data, and sync queues in SQLite. Backend APIs that
receive scan sync data MUST be idempotent and MUST define timestamp-based
conflict handling and audit events.

Rationale: School safety and transport workflows cannot depend on continuous
connectivity, and duplicate or conflicting scans must not corrupt attendance,
access, wallet, or transport state.

### V. Security, Observability, and Testable Quality
Security-sensitive behavior MUST be enforced server-side using JWT-based
authentication, role-based authorization, permission checks, tenant validation,
and feature authorization. Business logic MUST have unit tests. APIs,
authorization boundaries, data migrations, offline sync, and critical user
journeys MUST have integration or contract coverage. The system MUST emit
structured logs, audit events, basic metrics, and centralized error reports for
operationally important workflows.

Rationale: The platform manages student identity, campus access, payments,
medical data, and guardian notifications; correctness must be observable and
verifiable, not assumed.

## Platform Constraints

- Web applications MUST use Next.js, React, and TypeScript. New Next.js work
  MUST use the App Router, Server Components by default, and Client Components
  only when interactivity requires them.
- Web server state MUST use TanStack Query or typed server-driven data access.
  Local UI state MUST remain lightweight through React hooks or Zustand.
- Web UI MUST use a consistent design system, reusable module-level components,
  typed API clients, and a centralized API layer.
- Backend services MUST use the latest supported .NET stack with ASP.NET Core
  Web API, feature-based folders, lightweight Clean Architecture, and CQRS only
  when a feature has a concrete read/write separation need.
- Public APIs MUST be RESTful, versioned under `/api/v1/`, DTO-based, and
  paginated for list responses. Database models MUST NOT be exposed directly.
- Data storage MUST use a single PostgreSQL database for the modular monolith.
  Every tenant-owned table MUST include `tenant_id`, `created_at`, and
  `updated_at`. EF Core migrations MUST be version-controlled.
- Tenant identifiers, foreign keys, and frequently queried fields MUST be
  indexed where they affect correctness or performance.
- Mobile applications MUST use Flutter and Dart with Android Kotlin/Gradle and
  iOS Swift/CocoaPods integration where native layers are required. Mobile
  structure MUST align with backend feature modules.
- Deployment choices MUST favor low operating cost: Dockerized .NET API,
  managed PostgreSQL, Vercel for Next.js, and S3-compatible object storage
  unless a spec documents a justified alternative.

## Development Workflow & Quality Gates

- Features MUST follow the phase model in `PLAN.md`. Each spec MUST identify
  its implementation phase and feature module mapping before planning begins.
- The lifecycle is mandatory: spec creation, product and engineering review,
  task breakdown, implementation, and validation against acceptance criteria.
- Implementation plans MUST pass the Constitution Check before research and be
  re-checked after design. Any violation MUST be recorded in Complexity
  Tracking with the simpler alternative that was rejected.
- Tasks MUST be grouped by independently testable user story and MUST include
  foundational work for tenant resolution, feature flags, authorization,
  migrations, observability, and validation when the feature touches those
  concerns.
- Controllers MUST NOT contain business logic. UI code MUST NOT access the
  database directly. Feature code MUST be organized by module and MUST avoid
  oversized services that combine unrelated responsibilities.
- New infrastructure, microservices, queues, nonstandard storage, or broad CQRS
  patterns MUST be justified by concrete scale, reliability, or operational
  needs in the feature plan.

## Governance

This constitution supersedes conflicting local practices for feature planning,
implementation, and review. Amendments MUST update this file, include a Sync
Impact Report, and propagate affected guidance to spec-kit templates and runtime
guidance documents.

Versioning follows semantic versioning:
- MAJOR changes remove or redefine principles or governance in a backward
  incompatible way.
- MINOR changes add principles, sections, or materially expanded obligations.
- PATCH changes clarify wording, fix errors, or make non-semantic refinements.

Every spec, plan, task list, and implementation review MUST verify compliance
with the Core Principles. Approved exceptions MUST be documented in the
feature's Complexity Tracking section with the rationale, rejected simpler
alternative, and reviewer approval.

**Version**: 1.0.0 | **Ratified**: 2026-05-02 | **Last Amended**: 2026-05-02
