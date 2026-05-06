# Research: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

## Decision: Keep Phase 10 and Phase 11 bounded to document, search, admin, audit, and observability workflows

**Rationale**: `PLAN.md` assigns Phase 10 to Document Storage, Certificate
Management, Search Indexing, and Global Search. Phase 11 is assigned to Audit
Trail, Admin Dashboard, Tenant Feature Configuration, and Metrics &
Monitoring. The spec excludes attendance, campus gate, scan, transport,
wallet, learning reward, request approval, medical, emergency, complaint
resolution, and communication delivery outcomes. Keeping the boundary explicit
prevents these phases from becoming a source-domain workflow engine.

**Alternatives considered**:
- Let document or certificate workflows update source records: rejected because
  source modules remain the system of record for their own outcomes.
- Let global search grant access to source records: rejected because search is
  discovery only and must revalidate permissions before listing or opening.
- Let admin dashboards operate source workflows directly: rejected because
  dashboards summarize and configure; they do not replace domain modules.

## Decision: Use two feature areas: Documents and Administration

**Rationale**: Document storage, certificates, search, retention, and document
exports share object metadata, source references, versioning, access
decisions, and restricted-detail rules. Admin dashboards, tenant
configuration, audit trail, metrics, alerts, and incidents share operational
review, platform authority, configuration history, and observability behavior.
Two feature areas keep module ownership clear while staying within the
modular monolith.

**Alternatives considered**:
- One broad Operations feature area: rejected because document/certificate
  records have different lifecycle and object-storage concerns from monitoring
  and audit review.
- Separate microservices for documents, search, audit, and metrics: rejected
  because no measured operational pressure justifies the added cost,
  deployment complexity, or cross-service consistency risk.
- Put document behavior inside each source module: rejected because document
  governance, retention, legal hold, certificates, and search need shared
  policies across modules.

## Decision: Use PostgreSQL for metadata and S3-compatible object storage for document content

**Rationale**: The constitution requires a single PostgreSQL database for the
modular monolith and allows S3-compatible object storage for deployment cost
control. Database metadata carries tenant ownership, category, version,
visibility, retention, legal hold, access decisions, source references, and
audit evidence. Object storage holds document binary content and is never used
as the authorization source of truth.

**Alternatives considered**:
- Store document binaries only in PostgreSQL: rejected because large file
  storage would increase database size, backup cost, and operational risk.
- Store metadata only in object storage: rejected because tenant queries,
  permissions, retention, legal hold, search freshness, and audit joins require
  structured records.
- Use a third-party document management platform as source of truth: rejected
  because tenant, guardian-link, student, role, retention, and audit policy
  must remain enforceable inside the school platform.

## Decision: Treat prior phase records as read-only source references

**Rationale**: Documents, certificates, search results, dashboards, audit
entries, metrics, and incidents may reference identity, attendance, campus
access, transport, wallet, learning, requests, medical, complaints, and
communications. These phases use source references and permitted summaries but
do not mutate source-domain records.

**Alternatives considered**:
- Copy full source records into documents or search entries: rejected because
  copies can become stale and may bypass source-domain permissions.
- Let certificates replace source academic, attendance, payment, or conduct
  records: rejected because certificates summarize or attest evidence but do
  not become the source of truth for that evidence.
- Let dashboards perform domain actions: rejected because operational control
  must remain in the owning feature module.

## Decision: Use document categories and certificate types as tenant-scoped policies

**Rationale**: Schools need different document categories, certificate types,
issuer roles, required metadata, visibility defaults, retention behavior,
export eligibility, legal hold behavior, and review requirements. Versioned
policies preserve the rule set that governed a document or certificate when it
was created, updated, issued, corrected, revoked, exported, or retained.

**Alternatives considered**:
- One global document policy: rejected because school policies vary by
  student, staff, finance, medical, complaint, and operational context.
- Update category/type policy in place with no version history: rejected
  because historical records would lose their governing policy context.
- Require product changes for every document or certificate rule adjustment:
  rejected because tenant configuration is a platform capability.

## Decision: Preserve document and certificate evidence through append-only versioning

**Rationale**: Documents and certificates can be corrected, superseded,
revoked, archived, restored, or retained under legal hold. Preserving original
evidence, actor, time, reason, visibility impact, recipient impact, and audit
evidence is necessary for compliance and school accountability.

**Alternatives considered**:
- Overwrite files or certificates in place: rejected because it hides the
  original record and makes audit review unreliable.
- Delete old versions after correction: rejected because corrections and
  revocations must remain explainable.
- Allow issuer-only private corrections: rejected because authorized reviewers
  need a complete chain of evidence.

## Decision: Revalidate search visibility when results are listed and opened

**Rationale**: Roles, guardian links, student enrollment, staff assignment,
source visibility, legal holds, and feature configuration can change after a
record is indexed. Search entries help discovery, but current access
decisions must be evaluated at list time and open time to prevent metadata,
snippet, facet, count, preview, or content leakage.

**Alternatives considered**:
- Trust index-time permissions only: rejected because permissions can become
  stale.
- Show hidden result counts for restricted records: rejected because even
  counts can reveal sensitive records.
- Disable global search for sensitive modules entirely: rejected because
  authorized auditors and reviewers still need scoped discovery.

## Decision: Keep baseline search indexing inside the modular monolith

**Rationale**: The plan needs global search behavior, but the constitution
requires low-cost defaults and no extra infrastructure without measured
pressure. A platform-managed search index table can satisfy scoped search,
freshness state, source references, and access decisions while preserving a
future extraction path if real usage requires specialized search services.

**Alternatives considered**:
- Add a separate search cluster in the baseline plan: rejected because it adds
  operational cost before measured need.
- Query every source module live for every search: rejected because it creates
  slow, inconsistent, and tightly coupled search behavior.
- Make search unaudited: rejected because search queries and result opens can
  reveal sensitive information and must be reviewable.

## Decision: Use tenant feature configuration as the control plane

**Rationale**: Document storage, certificates, search, audit trail, dashboards,
exports, retention, legal hold, metrics, alerts, and incidents may be enabled
or constrained differently per school. Configuration changes need dependency
validation, effective windows, prior and new values, reasons, approval state
where required, and audit evidence.

**Alternatives considered**:
- UI-only feature toggles: rejected because backend feature enforcement is
  mandatory.
- Global on/off flags only: rejected because schools need tenant-specific
  policy and dependency control.
- Apply invalid dependency changes partially: rejected because partial
  configuration can expose data or break required workflows.

## Decision: Treat audit events as append-only reviewer evidence

**Rationale**: Audit evidence must explain sensitive actions, denied attempts,
exports, dashboard reads, search queries, configuration changes, alert
handling, incidents, retention actions, and document/certificate workflows.
Corrections or redactions must append new evidence and preserve the original
event unless a separately authorized privacy process requires limited masking.

**Alternatives considered**:
- Let administrators edit audit events directly: rejected because it defeats
  audit purpose.
- Hide denied attempts from audit: rejected because denied access is security
  evidence.
- Export full audit payloads by default: rejected because audit records can
  contain restricted data and exports need field-level minimization.

## Decision: Include controlled export as a sensitive workflow

**Rationale**: Documents, certificates, search results, audit trails,
configuration history, metrics summaries, and incident records can leave the
interactive platform through exports. Export scope, permission, reason,
included fields, generated time, retention state, and audit evidence must be
preserved.

**Alternatives considered**:
- Allow unrestricted exports for administrators: rejected because export can
  bypass ordinary screen-level visibility controls.
- Disable all exports: rejected because schools and auditors need governed
  records for review and compliance.
- Treat exports as ordinary reads: rejected because exports have higher data
  leakage risk and need explicit evidence.

## Decision: Enforce retention and legal hold before deletion or hiding

**Rationale**: Documents, certificates, audit entries, exports, search query
logs, configuration changes, alerts, incidents, and operational evidence may
be subject to retention or legal hold. Records under protection cannot be
deleted or hidden from authorized review.

**Alternatives considered**:
- Immediate delete on user request: rejected because school policy and legal
  hold can require preservation.
- Keep all records forever without policy: rejected because retention must be
  explicit and manageable.
- Apply retention only to files: rejected because audit, search, export,
  configuration, and incident evidence may also require retention.

## Decision: Use metrics, alerts, and incidents as operational review workflows

**Rationale**: Metrics indicate operational state but do not by themselves
prove source-domain business outcomes. Alerts and incidents create reviewable
workflows with severity, owner, status, acknowledgement, resolution, evidence,
and audit history.

**Alternatives considered**:
- Dashboard metrics only with no alerts: rejected because threshold breaches
  need assignment and acknowledgement.
- Let alerts mutate source workflows: rejected because the owning domain must
  make its own business decisions.
- Suppress noisy or missing metrics silently: rejected because data quality
  issues are operational evidence.

## Decision: Minimize restricted details across documents, search, dashboards, audit, and monitoring

**Rationale**: Phase 10 and Phase 11 can surface sensitive medical,
complaint, finance, staff, safety, student-welfare, audit, and platform
operations context. Backend visibility policy must minimize, withhold, or
route restricted details to review when the actor lacks authority.

**Alternatives considered**:
- Trust frontend filtering only: rejected because backend enforcement is
  mandatory.
- Show all metadata but hide file content: rejected because metadata can reveal
  sensitive facts.
- Hide every sensitive record from all dashboards: rejected because authorized
  reviewers and platform operators still need scoped operational visibility.

## Decision: Testing strategy follows sensitive document, search, audit, and operations boundaries

**Rationale**: These phases touch files, certificates, search discovery,
exports, tenant configuration, audit evidence, legal hold, retention, alerts,
incidents, and cross-module summaries. Unit, integration, contract,
authorization, tenant-isolation, feature-flag, export, retention/legal hold,
audit, performance, and critical UI journey tests are required.

**Alternatives considered**:
- UI-only testing: rejected because authorization, search result leakage,
  exports, retention, and audit behavior are security-critical.
- Unit tests only: rejected because route contracts, authorization, object
  storage metadata, search freshness, audit exports, and dashboard summaries
  require integration and contract coverage.
- Manual audit or monitoring verification only: rejected because evidence and
  alert behavior must be repeatable and auditable.
