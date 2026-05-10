# Implementation Plan: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

**Branch**: `011-documents-search-admin-observability` | **Date**: 2026-05-06 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/011-documents-search-admin-observability/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 10 and Phase 11 add document management, certificate management,
authorized search, audit review, admin configuration, dashboards, metrics,
alerts, incident review, retention, legal hold, export controls, and
operational evidence for the School NFC platform. The implementation approach
adds a Documents feature area for managed documents, document versions,
document categories, access decisions, certificates, certificate types,
certificate verification records, search index entries, search query logs,
search result access decisions, retention policies, legal holds, and export
evidence. It also adds an Administration feature area for tenant feature
settings, configuration changes, admin dashboard summaries, audit events,
audit exports, metric observations, alert rules, alerts, incidents, and
operational exceptions. All behavior is tenant-scoped, feature-gated,
permission-scoped, auditable, backed by PostgreSQL persistence and
S3-compatible object storage, and designed to reference prior phase records
without mutating those source-domain workflows.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration,
staff, guardian, student, audit, and platform operations web surfaces, and
Flutter/Dart only where existing mobile clients expose document, certificate,
or search views. Exact package versions are pinned when runtime manifests are
created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL,
S3-compatible object storage, JWT-based authentication, IdentityAccess student
profiles, approved guardian links, roles and permissions, tenant feature
configuration, audit/event logging foundations, prior phase source references,
document validation/scanning adapters where configured, search indexing within
the platform boundary, metrics/error reporting foundations, Next.js App
Router, TanStack Query or typed server-driven data access, and Spec Kit
planning artifacts.
**Storage**: Single PostgreSQL database for document metadata, document
versions, document categories, document access decisions, certificates,
certificate types, certificate verification records, search index metadata,
search query logs, search result access decisions, tenant feature settings,
configuration changes, audit events, audit exports, metric observations, alert
rules, alerts, incidents, retention policies, legal holds, operational
exceptions, and audit evidence. Document binary content is stored in
S3-compatible object storage with database metadata, tenant ownership,
versioning, retention, legal hold, and access decisions. Every tenant-owned
table includes `tenant_id`, `created_at`, and `updated_at`, with indexes for
tenant boundaries, actor lookup, student or subject lookup, source module,
source reference, category, document status, certificate status, visibility,
retention state, legal hold state, search freshness, audit action, target
record, correlation reference, metric source, alert state, incident state, and
review state.
**Testing**: Backend unit tests for document category validation, document
access decisions, version transitions, retention and legal hold policy,
certificate type validation, certificate status transitions, search
visibility, search freshness, tenant feature dependency validation, audit
filtering, audit export scope, metric threshold evaluation, alert and incident
state transitions, operational exception detection, and no-side-effect
boundaries; integration and contract tests for `/api/v1/` document,
certificate, search, admin configuration, audit, dashboard, monitoring, alert,
and incident routes; authorization, tenant-isolation, feature-flag, export,
retention, legal hold, idempotency, audit, performance, and critical UI
journey tests.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration web app, guardian and student web views where enabled,
auditor/compliance review surfaces, platform operator views, and optional
mobile document or certificate surfaces where existing clients support them.
Phase 10 and Phase 11 consume source references and audit/metric/status
events from prior modules through authorized read boundaries.
**Project Type**: Modular monolith SaaS with web, backend, object storage, and
optional mobile clients organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: document upload and
classification under 2 minutes, permitted document or certificate search
under 30 seconds, 95% of eligible document and certificate changes searchable
within 5 minutes, certificate issuance under 2 minutes, 95% of scoped searches
returning in under 10 seconds, admin dashboard summaries in under 30 seconds,
audit trace lookup in under 60 seconds, and 95% of configured metric threshold
breaches visible to alert or incident owner views within 5 minutes.
**Constraints**: Phase 10 and Phase 11 only. Document, certificate, search,
admin, audit, metrics, alert, incident, export, retention, and legal hold
records are tenant-scoped, feature-gated, authenticated by default, and
permission-scoped. Search result visibility is revalidated when listed and
opened. Audit events are append-only from a reviewer perspective. Object
content cannot be deleted while retention or legal hold protection applies.
Prior phase source records remain read-only references and must not be mutated
by these phases. These phases must not create attendance, campus gate, scan,
transport, wallet, learning reward, request approval, medical, emergency,
complaint resolution, or communication delivery outcomes.
**Scale/Scope**: Eight modules from `PLAN.md`: Document Storage, Certificate
Management, Search Indexing, Global Search, Audit Trail, Admin Dashboard,
Tenant Feature Configuration, and Metrics & Monitoring. Supporting retention,
legal hold, controlled exports, operational exceptions, alerts, incidents,
review summaries, and status events are included only where required by the
combined Phase 10 and Phase 11 spec.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  is marked Reviewed and defines user stories, acceptance scenarios,
  requirements, entities, edge cases, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 10:
  Documents & Search and Phase 11: Admin, Audit & Observability, and maps to
  Document Storage, Certificate Management, Search Indexing, Global Search,
  Audit Trail, Admin Dashboard, Tenant Feature Configuration, and Metrics &
  Monitoring.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  records, school-account capability keys, backend feature enforcement, and
  web/mobile feature gates are required before document, certificate, search,
  admin, audit, export, monitoring, alert, or incident workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  role assignments, permission checks, approved guardian-link visibility,
  student self-scope, source-module authority, document ownership, certificate
  issuer authority, audit authority, operations authority, platform review
  authority, feature availability checks, and auditable access decisions are
  scoped.
- **Data and API contracts**: PASS. PostgreSQL metadata storage, S3-compatible
  object storage references, EF Core migrations, DTO-based `/api/v1/`
  contracts, paginated list responses, tenant and lookup indexes, versioned
  category/type/setting/rule records, idempotent document, certificate,
  configuration, export, alert, incident, and review commands are planned
  where applicable.
- **Offline NFC integrity**: PASS. Phase 10 and Phase 11 do not create NFC, QR,
  attendance, campus access, or transport scan records. They may reference
  source records and audit/metric/status events from prior modules without
  mutating scan, attendance, campus access, transport, wallet, learning,
  request, medical, emergency, complaint, or communication outcomes.
- **Observability and testing**: PASS. Structured logs, audit events,
  document metrics, certificate metrics, search freshness metrics, audit flow
  metrics, configuration metrics, alert metrics, incident metrics, error
  reporting, backend tests, contract tests, authorization tests,
  tenant-isolation tests, feature-flag tests, export tests, retention/legal
  hold tests, and critical UI journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. S3-compatible object storage is used only for
  document binary content as allowed by the constitution. Search indexing,
  metrics, alerts, and incidents remain inside the platform boundary; no new
  microservice, separate search cluster, separate audit database, or broad CQRS
  pattern is introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 10 and Phase 11 scope, keep tenant and
permission enforcement explicit, define prior-phase source reference
boundaries, object storage metadata boundaries, search authorization,
configuration dependency validation, audit append-only behavior, retention
and legal hold rules, monitoring and incident workflows, no-side-effect
boundaries, and introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/011-documents-search-admin-observability/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── admin-dashboard-configuration.md
│   ├── audit-trail.md
│   ├── certificate-management.md
│   ├── document-storage.md
│   ├── metrics-monitoring.md
│   └── search.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Documents/
│   │   ├── Common/
│   │   ├── Storage/
│   │   ├── Certificates/
│   │   ├── Search/
│   │   ├── Retention/
│   │   ├── Exports/
│   │   ├── Audit/
│   │   └── Seed/
│   ├── src/SafeSchool.Api/Features/Administration/
│   │   ├── Common/
│   │   ├── Dashboard/
│   │   ├── Configuration/
│   │   ├── AuditTrail/
│   │   ├── Monitoring/
│   │   ├── Alerts/
│   │   ├── Incidents/
│   │   └── Seed/
│   └── tests/SafeSchool.Api.Tests/Features/
│       ├── Documents/
│       └── Administration/
├── admin-web/
│   ├── src/app/(school)/documents/
│   ├── src/app/(school)/certificates/
│   ├── src/app/(school)/search/
│   ├── src/app/(school)/admin/
│   ├── src/app/(guardian)/documents/
│   ├── src/app/(guardian)/certificates/
│   ├── src/app/(student)/documents/
│   ├── src/app/(student)/certificates/
│   ├── src/features/documents/
│   ├── src/features/administration/
│   └── tests/documents-admin/
└── mobile/
    ├── lib/features/documents/
    └── test/features/documents/

tests/
├── contracts/documents-admin/
└── e2e/documents-admin/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders separate document and certificate behavior from
administrative, audit, dashboard, monitoring, alert, and incident behavior.
Documents code owns object metadata, document versions, document categories,
document access decisions, certificate records, search indexing metadata,
search result access decisions, retention, legal hold, and controlled exports.
Administration code owns tenant feature configuration, dashboard summaries,
audit trail review, audit exports, metrics, alert rules, alerts, incidents,
and operational exceptions. Web code owns school administrator, document
manager, certificate issuer, guardian, student, auditor, compliance reviewer,
operations reviewer, and platform operator workflows. Mobile code is limited
to existing mobile document, certificate, or search surfaces where needed; no
NFC, attendance, transport, wallet, request, medical, complaint resolution, or
communication delivery workflow is introduced.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | No exception required | No simpler alternative rejected |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, prior phase source reference
boundaries, object storage treatment, document versioning, certificate
provenance, search authorization, search freshness, dashboard boundaries,
tenant feature configuration dependencies, audit append-only behavior,
retention and legal hold, controlled exports, metrics, alerts, incidents,
operational exceptions, no-side-effect boundaries, testing strategy, and
implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for document storage,
certificate management, global search, admin dashboard and configuration,
audit trail, and metrics/monitoring workflows. These artifacts define
tenant-owned entities, state transitions, validation rules, `/api/v1/` route
contracts, idempotency expectations, pagination, audit behavior,
restricted-detail boundaries, retention and legal hold behavior, export
controls, operational monitoring behavior, and no-side-effect constraints for
Phase 10 and Phase 11.
