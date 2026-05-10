# Quickstart: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

Use this quickstart to validate that the Phase 10 and Phase 11 planning
package is complete before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for combined Phase 10 and Phase 11 scope, user
  stories, requirements, success criteria, edge cases, and assumptions.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, configuration, and storage
  foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 2 through Phase 9 artifacts only to preserve source-domain
  boundaries. Phase 10 and Phase 11 may reference allowed source evidence but
  must not create attendance, campus access, transport, wallet, learning,
  request, medical, emergency, complaint, or communication delivery outcomes.
- Confirm `.specify/feature.json` points to
  `specs/011-documents-search-admin-observability`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes documents, versions,
   categories, access decisions, certificates, certificate types,
   verification records, search index entries, search query logs, search
   access decisions, dashboard summaries, tenant feature settings,
   configuration changes, audit events, audit exports, metrics, alert rules,
   alerts, incidents, retention policies, legal holds, controlled exports, and
   operational exceptions.
3. Confirm [contracts/document-storage.md](./contracts/document-storage.md)
   covers document upload, metadata, versions, archive, restore, legal hold,
   retention, export, restricted-detail behavior, and audit expectations.
4. Confirm [contracts/certificate-management.md](./contracts/certificate-management.md)
   covers certificate types, issuance, correction, revocation, reissue,
   verification, export, and audit expectations.
5. Confirm [contracts/search.md](./contracts/search.md) covers global search,
   result-open access decisions, index freshness, reindex requests, exports,
   restricted result minimization, and audit expectations.
6. Confirm [contracts/admin-dashboard-configuration.md](./contracts/admin-dashboard-configuration.md)
   covers dashboard summaries, tenant feature settings, configuration change
   history, dependency validation, approvals, and audit behavior.
7. Confirm [contracts/audit-trail.md](./contracts/audit-trail.md) covers audit
   event filtering, event detail, payload minimization, controlled audit
   exports, append-only evidence, and access control.
8. Confirm [contracts/metrics-monitoring.md](./contracts/metrics-monitoring.md)
   covers metrics, alert rules, alerts, incidents, operational exceptions,
   data-quality states, and audit behavior.

## Implementation Order for Later Tasks

1. Establish shared tenant, feature capability, permission, guardian-link,
   student self-scope, staff assignment, source-module, reviewer, platform
   operator, export, retention, legal hold, audit, privacy, and visibility
   guards for Documents and Administration workflows.
2. Create document, document version, document category, document access
   decision, certificate, certificate type, certificate verification, search
   index, search query log, search result decision, dashboard summary, tenant
   feature setting, configuration change, audit event, audit export, metric,
   alert rule, alert, incident, retention policy, legal hold, controlled export,
   operational exception, and audit models with tenant indexes and migrations.
3. Create Document Storage behavior for upload intent, metadata validation,
   version creation, validation state, archive, restore, legal hold, retention,
   export, and restricted-detail minimization.
4. Create Certificate Management behavior for type configuration, issuance,
   verification, correction, revocation, expiration, supersession, reissue,
   controlled export, and source-evidence preservation.
5. Create Search behavior for source indexing eligibility, index freshness,
   reindex, scoped search filters, result listing, result-open access
   decisions, restricted metadata minimization, query logs, and exports.
6. Create Admin Dashboard behavior for permission-scoped summaries covering
   enabled modules, usage, exceptions, pending reviews, documents,
   certificates, search health, audit activity, alerts, incidents, and
   operational health.
7. Create Tenant Feature Configuration behavior for feature settings,
   dependency validation, effective windows, approval or rejection, version
   preservation, and no partial unsafe application.
8. Create Audit Trail behavior for audit event search, event detail, payload
   minimization, audit export, append-only correction evidence, export
   retention, and access-denial evidence.
9. Create Metrics, Alert, Incident, and Operational Exception behavior for
   metric observations, data-quality state, alert rule activation, alert
   lifecycle, incident lifecycle, exception review, and dashboard integration.
10. Create Retention, Legal Hold, and Controlled Export policies for documents,
    certificates, audit, search logs, configuration history, metrics, alerts,
    incidents, and operational evidence.
11. Expose eligible document, certificate, search, audit, configuration, alert,
    incident, and exception status events for later review or communication
    capabilities without delivering general messages directly.
12. Complete unit, integration, contract, authorization, tenant-isolation,
    feature-flag, export, retention/legal hold, audit, performance, and
    critical UI journey tests.

## Validation Scenarios

### Document Storage

- Upload, classify, and make a document available to its permitted audience in
  under 2 minutes during review testing.
- Add a replacement version and confirm original file evidence, new file
  evidence, actor, time, reason, visibility impact, retention impact, and audit
  evidence remain preserved.
- Place a legal hold on a document and confirm deletion, permanent hiding, and
  retention expiry destruction are blocked while hold applies.
- Attempt unauthorized document view, download, update, archive, restore,
  export, and delete flows across cross-school, inactive relationship,
  restricted category, and missing metadata cases.

### Certificate Management

- Issue a complete eligible certificate in under 2 minutes during review
  testing.
- Verify a certificate as an authorized recipient and confirm only permitted
  certificate status and summary are shown.
- Correct, revoke, expire, supersede, and reissue sampled certificates and
  confirm original issuance evidence, actor, time, reason, recipient impact,
  verification impact, and audit evidence remain preserved.
- Attempt issuance with stale source evidence, unauthorized source evidence,
  cross-school subject, disabled certificate type, missing required fields, and
  duplicate active certificate conflicts.

### Search

- Search for a permitted document or certificate and open it in under 30
  seconds during review testing.
- Confirm 95% of eligible document and certificate changes become searchable
  within 5 minutes or show stale or failed indexing state to authorized
  reviewers.
- Confirm 100% of sampled search results are visible only inside authorized
  tenant, guardian link, student ownership, staff assignment, source-module,
  document ownership, audit, operations, review, or platform review scope.
- Search for restricted medical, complaint, finance, staff, safety,
  student-welfare, audit, and platform operations terms and confirm
  unauthorized users see no leaked titles, counts, snippets, facets, previews,
  or source existence.

### Admin Dashboard and Tenant Configuration

- Open the admin dashboard and see enabled feature status, open exceptions,
  pending reviews, document and certificate state, search health, audit
  activity, alerts, incidents, and operational health summaries in under 30
  seconds.
- Change a tenant feature setting and confirm prior setting, new setting,
  actor, reason, dependency decision, effective window, approval state where
  required, version, and audit evidence are preserved.
- Submit invalid dependency-breaking, cross-school, unsafe, or mandatory safety
  configuration changes and confirm they are rejected or routed to review
  without partial application.

### Audit Trail

- Search audit events by actor, role, student or subject, source module,
  action, target record, result, denial reason, risk level, correlation
  reference, and date range.
- Trace a sampled document, certificate, search query, feature configuration
  change, alert, or access denial through audit evidence in under 60 seconds.
- Export an audit trail with a required reason and confirm only permitted
  fields are included and export evidence is itself auditable.
- Confirm audit events are append-only from a reviewer perspective and
  corrections or redactions create new evidence.

### Metrics, Alerts, Incidents, and Exceptions

- Configure an alert rule for a document, certificate, search, audit,
  configuration, access denial, or platform health metric.
- Trigger a configured threshold breach and confirm 95% of sampled breaches
  create or update the correct alert or incident owner view within 5 minutes.
- Acknowledge, assign, resolve, reopen, and document an alert or incident and
  confirm actor, time, reason, current state, linked evidence, and audit
  history remain preserved.
- Submit missing, delayed, duplicated, noisy, or inconsistent metric data and
  confirm data-quality or review-required evidence is recorded rather than
  silently suppressed.

### Retention, Export, Boundaries, and Privacy

- Confirm 100% of sampled legal hold and retention-protected records cannot be
  deleted or hidden from authorized review while protection applies.
- Export documents, certificates, search results, configuration history,
  metrics summaries, and incident records only after export capability,
  permission, scope, included fields, reason, retention, and audit checks pass.
- Confirm sampled records containing medical, complaint, finance, staff,
  safety, student-welfare, audit, or platform operations context show only
  permitted summaries, minimized metadata, or no result to unauthorized actors.
- Confirm Phase 10 and Phase 11 create no attendance, campus gate, NFC/QR scan,
  transport, wallet, learning reward, request approval, medical, emergency,
  complaint resolution, communication delivery, or source-domain mutation
  outcome.
