# Feature Specification: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

**Feature Branch**: `011-documents-search-admin-observability`  
**Created**: 2026-05-06  
**Status**: Reviewed  
**Input**: User description: "Read PLAN.md and create a specification for phase of Documents & Search and phase Admin, Audit & Observability."

## Review Status

- Product and engineering review gate: Passed for planning and task generation.
- Implementation may proceed from this specification unless later review changes are recorded.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 10: Documents & Search and Phase 11: Admin, Audit & Observability
- **Feature Module(s)**: Document Storage, Certificate Management, Search Indexing, Global Search, Audit Trail, Admin Dashboard, Tenant Feature Configuration, Metrics & Monitoring
- **Tenant Scope**: All documents, document versions, document categories, certificate records, certificate verification records, search index entries, search queries, search result access decisions, admin dashboard summaries, tenant feature settings, configuration changes, audit events, audit queries, metric observations, alert rules, incidents, retention policies, legal holds, exports, and operational evidence belong to one school account unless an explicit platform-level operator role is acting across schools with audited authority.
- **Feature Flag(s)**: Document storage, certificate management, global search, search indexing, admin dashboard, audit trail, audit export, tenant feature configuration, metrics monitoring, alerting, retention management, legal hold, and platform operations views must respect each school account's enabled capabilities before users can access, configure, search, export, monitor, or administer related workflows.
- **Security/Roles**: Platform owners, platform operators, school administrators, document managers, certificate issuers, authorized staff, teachers, finance staff, learning staff, transport staff, medical staff, complaint managers, communication managers, guardians, students, auditors, compliance reviewers, operations reviewers, and support reviewers must have explicit permissions for each Phase 10 and Phase 11 action. Guardians can view only documents, certificates, and search results connected to approved active guardian relationships or records addressed directly to them. Students can view only school-enabled student-visible records. Staff can view, manage, search, or export only records inside their school account and assignment, role, source-module, or review authority. Audit, search, dashboard, and observability surfaces must not reveal restricted medical, complaint, finance, staff, safety, student-welfare, or platform operations details to unauthorized users.
- **Offline/NFC Impact**: These phases do not require NFC, QR, or offline scan behavior. Documents, certificates, search, admin dashboards, audit review, and monitoring may reference prior records from identity, attendance, campus access, transport, wallet, learning, requests, medical, complaints, and communications, but they must not create scan events, attendance outcomes, campus access decisions, transport boarding decisions, wallet transactions, learning rewards, request approvals, medical incidents, emergency access sessions, complaint decisions, or communication deliveries.
- **Observability**: The system must emit reviewable evidence for document upload, document view, document download, document update, version creation, archive, restore, legal hold, retention action, certificate issue, certificate correction, certificate revocation, certificate verification, index request, index failure, search query, search result open, denied search result, dashboard read, feature configuration change, feature dependency rejection, audit query, audit export, metric threshold change, alert creation, alert acknowledgement, incident status change, operational exception, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Manage School Documents (Priority: P1)

As a school administrator, document manager, or authorized staff member, I need to store, classify, version, find, share, archive, and retain school documents so important records remain controlled, traceable, and visible only to the right audience.

**Why this priority**: Document storage is the primary value of Phase 10 and becomes the foundation for certificates, search, retention, audit review, and admin oversight.

**Independent Test**: Upload a tenant-scoped document for a student, staff process, or school context, classify it, assign visibility, create a replacement version, and verify only authorized actors can view, download, archive, or restore it.

**Acceptance Scenarios**:

1. **Given** document storage is enabled and the actor has document management permission, **When** the actor uploads a document with category, owner or subject context, visibility, retention rule, and optional source reference, **Then** the document is stored in the correct school account with metadata, status, version, access rules, retention state, and audit evidence.
2. **Given** a document has multiple versions, **When** an authorized user views the document history, **Then** the user can distinguish active, archived, superseded, restored, and legally held versions without losing the original evidence.
3. **Given** a guardian, student, or staff user lacks authority for the document category, student relationship, source module, or visibility level, **When** they attempt to view, download, search, export, archive, restore, or share the document, **Then** the action is denied and recorded without exposing restricted metadata or content.

---

### User Story 2 - Issue and Verify Certificates (Priority: P1)

As a school administrator or certificate issuer, I need to issue, correct, revoke, and verify certificates so official school records can be trusted without changing the underlying academic, attendance, finance, or conduct records they summarize.

**Why this priority**: Certificate management is explicitly part of Phase 10 and requires strong provenance, versioning, verification, and revocation behavior.

**Independent Test**: Issue a certificate for an eligible student, verify it as an authorized recipient, correct it, revoke it, and confirm each state preserves issuer, reason, time, verification result, and audit evidence.

**Acceptance Scenarios**:

1. **Given** certificate management is enabled and the actor has issuer authority, **When** the actor issues a certificate with certificate type, student or recipient, source summary, issuer, validity period, language where applicable, and visibility, **Then** the certificate is created inside the school account with status, version, verification state, and audit evidence.
2. **Given** a certificate is corrected, superseded, expired, revoked, or reissued, **When** an authorized viewer opens it or verifies it, **Then** the current status and allowed historical evidence are shown without hiding the original issuance record.
3. **Given** certificate issuance would reference unauthorized source records, a cross-school student, stale source evidence, a disabled certificate type, missing required fields, or a conflicted issuer, **When** issuance is attempted, **Then** the request is blocked or routed to review with a clear reason.

---

### User Story 3 - Search Authorized Records (Priority: P1)

As a school administrator, staff member, guardian, student, auditor, or platform reviewer, I need to search across records I am allowed to access so I can quickly find documents, certificates, students, communications, complaints, requests, payments, transport records, learning records, medical summaries, and audit evidence without bypassing permissions.

**Why this priority**: Global search is a core Phase 10 module and must be useful only if it preserves tenant, role, relationship, source-module, and restricted-detail boundaries.

**Independent Test**: Search for a student-related term as a school administrator, guardian, student, and unrelated staff member, then verify each actor receives only permitted results and denied or restricted results are not leaked through titles, snippets, counts, filters, or previews.

**Acceptance Scenarios**:

1. **Given** global search is enabled and the actor is authenticated, **When** the actor searches by name, title, reference, category, date range, source module, owner, student context, certificate type, document category, or status, **Then** results include only records within the actor's school account, relationship, role, assignment, source-module, or review scope.
2. **Given** a search result references restricted medical, complaint, finance, staff, safety, or student-welfare context, **When** the actor lacks authority for the restricted details, **Then** the result is hidden, minimized, or routed to review according to the visibility policy.
3. **Given** search indexing is delayed, failed, stale, or missing for a record, **When** the actor searches or opens a result, **Then** the system shows a clear freshness or unavailable state and never opens a result without revalidating current access.

---

### User Story 4 - Operate the Admin Dashboard and Tenant Configuration (Priority: P1)

As a platform owner or school administrator, I need operational dashboards and tenant feature configuration so I can understand enabled capabilities, usage, exceptions, and risk while safely changing school-specific settings.

**Why this priority**: Phase 11 depends on administrators having a governed control surface for feature availability, operational summaries, exceptions, and configuration.

**Independent Test**: Open the admin dashboard for a school account, review enabled capabilities and operational summaries, change a tenant feature setting, reject an invalid dependency change, and verify the dashboard and audit trail reflect the outcome.

**Acceptance Scenarios**:

1. **Given** admin dashboard is enabled and the actor is an authorized administrator, **When** the actor opens the dashboard, **Then** they see permission-scoped summaries for enabled modules, usage, exceptions, pending review, feature status, operational health, and recent audit activity without restricted details outside their authority.
2. **Given** tenant feature configuration is enabled and the actor has configuration authority, **When** they enable, disable, schedule, or modify a feature setting, **Then** the change validates dependencies, records reason and actor, preserves the prior version, and applies only within the selected school account.
3. **Given** a configuration change would break a required dependency, expose restricted data, disable a mandatory safety capability, affect another school account, or conflict with existing active records, **When** the change is submitted, **Then** it is rejected or routed to review with preserved rationale.

---

### User Story 5 - Review Audit Trails and Exports (Priority: P1)

As an auditor, compliance reviewer, school administrator, or platform reviewer, I need audit trail search and controlled export so I can investigate who did what, when, why, and with what result across school workflows.

**Why this priority**: Audit trail is explicitly part of Phase 11 and is a security-critical governance requirement for the platform.

**Independent Test**: Perform sensitive actions across documents, certificates, search, tenant configuration, and another prior module; then verify an authorized auditor can filter, inspect, and export the permitted audit trail while unauthorized users cannot read or export it.

**Acceptance Scenarios**:

1. **Given** audit trail is enabled and the actor has audit authority, **When** the actor filters by school account, actor, role, student, source module, action, target record, result, denial reason, date range, risk level, or correlation reference, **Then** only audit entries inside the actor's authorized scope are shown.
2. **Given** an audit entry contains restricted or sensitive details, **When** a reviewer without full authority opens it, **Then** sensitive payload details are minimized while action, result, actor, time, and reviewable reason remain available according to policy.
3. **Given** an audit export is requested, **When** the actor has export permission and provides a reason, **Then** the export includes only permitted fields, records export evidence, and remains traceable as a sensitive audit action.

---

### User Story 6 - Monitor Metrics, Alerts, and Incidents (Priority: P2)

As a platform operator, school administrator, or operations reviewer, I need metrics, alerts, health summaries, and incident review so operational problems can be found, assigned, acknowledged, and resolved before they affect students, guardians, or school staff.

**Why this priority**: Monitoring is a Phase 11 requirement, but it can follow the core admin, audit, document, certificate, and search flows.

**Independent Test**: Configure a monitoring threshold for a school-visible workflow, trigger an alert condition, acknowledge the alert, link it to an incident, resolve the incident, and verify visibility, audit evidence, and dashboard summaries update within the authorized scope.

**Acceptance Scenarios**:

1. **Given** metrics and monitoring are enabled, **When** operational metrics for documents, certificates, search, audit, feature configuration, or prior modules cross a configured threshold, **Then** an alert is created with severity, affected school account or platform scope, current state, owner, and audit evidence.
2. **Given** an authorized operator reviews alerts or incidents, **When** they acknowledge, assign, update severity, resolve, or document an incident, **Then** the action updates the incident lifecycle and remains visible in audit and dashboard summaries.
3. **Given** metric data is missing, delayed, duplicated, noisy, or cross-school, **When** monitoring evaluates the condition, **Then** the system records a data-quality or review-required state instead of silently suppressing or misrouting the alert.

### Edge Cases

- A document is uploaded without category, owner, retention rule, visibility, or required source context.
- A document contains restricted medical, complaint, finance, staff, safety, or student-welfare information but is assigned to a broad audience.
- A guardian link, student enrollment, staff role, or review authority changes after a document, certificate, search result, audit entry, export, alert, or dashboard summary is created.
- A document version is corrected, superseded, archived, restored, deleted, placed on legal hold, or reaches retention expiry while still linked from a certificate, complaint, communication, or audit investigation.
- A certificate is issued from stale, unauthorized, disputed, or incomplete source evidence.
- A certificate is corrected, revoked, expired, superseded, or verified after the student transfers, graduates, or becomes inactive.
- Search indexing is delayed, duplicated, stale, partially failed, or includes records from a disabled feature.
- A search query matches restricted content that the actor may not know exists.
- A search result is visible at query time but becomes unauthorized before the actor opens it.
- A search query is broad enough to create privacy risk, excessive export risk, or operational load.
- A school administrator disables a capability while active documents, certificates, audits, alerts, search results, or retention holds still depend on it.
- A feature configuration change has dependencies across communication, documents, search, audit, or metrics capabilities.
- An audit event references a record that has been archived, retained under legal hold, hidden from the current reviewer, or belongs to another school account.
- An audit export contains many sensitive records, is requested by a conflicted reviewer, or is repeated in a suspicious pattern.
- Metrics are missing, delayed, duplicated, noisy, or inconsistent across dashboard, audit, alert, and incident views.
- A platform operator needs cross-school visibility during an incident but school administrators must not see other schools' records.
- A user attempts to use document access, certificate status, search results, audit entries, admin dashboard counts, metrics, or alert state as authorization for attendance, campus gate, transport boarding, wallet payment, learning reward, request approval, medical access, complaint resolution, communication delivery, or emergency response.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized users to create, classify, view, update, version, archive, restore, place on hold, retain, delete where allowed, and export documents inside a school account when document storage is enabled.
- **FR-002**: The system MUST validate school account scope, feature availability, actor identity, actor permission, student or subject relationship, source record visibility, document category, certificate type, search scope, audit authority, configuration authority, monitoring authority, retention rule, and restricted-detail policy before creating, showing, searching, exporting, configuring, or reviewing any Phase 10 or Phase 11 record.
- **FR-003**: Each document record MUST capture school account, document category, title, description or summary, owner or subject context, source reference where applicable, visibility level, retention rule, hold state, current status, active version, created actor, updated actor, and audit evidence.
- **FR-004**: Document versions MUST preserve original file evidence, replacement file evidence, version reason, actor, time, status, visibility impact, retention impact, and relationship to prior and active versions.
- **FR-005**: Document categories MUST define school-account rules for allowed actors, required metadata, allowed subjects, visibility defaults, retention behavior, legal hold eligibility, export eligibility, restricted-detail handling, and whether review is required before publication.
- **FR-006**: Document access MUST be permission-scoped so users can view, download, update, archive, restore, hold, export, or delete only documents inside their school account, relationship, assignment, source-module, ownership, or review scope.
- **FR-007**: The system MUST separate recipient-visible, staff-visible, restricted, reviewer-only, and platform-operator document metadata and content so sensitive information is minimized or withheld when the actor lacks authority.
- **FR-008**: Certificate management MUST allow authorized issuers to create, issue, verify, correct, revoke, expire, supersede, and reissue certificates inside a school account when certificate management is enabled.
- **FR-009**: Each certificate record MUST capture school account, certificate type, recipient or subject, source summary, issuer, issuance time, validity window, language where applicable, status, verification state, correction or revocation reason where applicable, document reference where applicable, and audit evidence.
- **FR-010**: Certificate issuance MUST validate issuer authority, student or recipient eligibility, source evidence visibility, certificate type availability, required fields, duplicate active certificate rules, validity rules, and restricted-detail policy before issuance.
- **FR-011**: Certificate corrections, revocations, expirations, supersessions, and reissues MUST preserve the original certificate evidence, actor, time, reason, recipient impact, verification impact, and audit evidence.
- **FR-012**: Phase 10 certificates MUST summarize or reference allowed source evidence but MUST NOT create or modify academic results, attendance outcomes, payment state, complaint outcomes, medical records, request approvals, learning rewards, or transport decisions.
- **FR-013**: Search indexing MUST cover tenant-scoped eligible records from documents, certificates, identity, attendance, campus access, transport, wallet, learning, requests, medical, complaints, communications, admin configuration, audit, and observability only when the source record is marked searchable for at least one authorized audience.
- **FR-014**: Search results MUST enforce tenant, role, permission, relationship, assignment, source-module, review, restricted-detail, and current feature configuration checks both when results are listed and when a result is opened.
- **FR-015**: Global search MUST support filtering by record type, school account, student or subject context, source module, category, status, owner, date range, visibility level, certificate type, document category, audit action, alert state, and review state within the actor's authorized scope.
- **FR-016**: Search results MUST show only permitted title, summary, type, status, date, source module, and action hints, and MUST hide or minimize snippets, counts, facets, and previews that would reveal restricted records.
- **FR-017**: Search indexing state MUST be visible to authorized administrators and reviewers, including indexed, pending, stale, failed, suppressed, reindex-required, and review-required states with reason and last update time.
- **FR-018**: Search query and result-open events MUST be audit-visible, including actor, scope, filters, result categories, denied result attempts, reason where required, and export or download follow-up where applicable.
- **FR-019**: Admin dashboards MUST provide permission-scoped school and platform summaries for enabled modules, usage, open exceptions, pending reviews, active configuration, document and certificate state, search health, audit activity, alerts, incidents, and operational health without exposing restricted detail beyond the actor's authority.
- **FR-020**: Tenant feature configuration MUST allow authorized administrators to enable, disable, schedule, modify, review, and inspect capabilities for document storage, certificate management, global search, audit trail, admin dashboard, feature configuration, metrics monitoring, alerting, exports, retention management, and legal hold.
- **FR-021**: Tenant feature configuration changes MUST validate dependencies, school policy, required safety capabilities, active records, search visibility, audit obligations, and monitoring implications before the change is accepted.
- **FR-022**: Tenant feature configuration MUST preserve prior setting, new setting, actor, reason, time, effective window, dependency decisions, approval state where required, and version history.
- **FR-023**: Audit trail review MUST allow authorized auditors and reviewers to search, filter, inspect, and export permitted audit evidence by school account, actor, role, student or subject, source module, action, target record, result, denial reason, risk level, correlation reference, and date range.
- **FR-024**: Audit entries MUST preserve action, actor, actor role, school account, target record, source module, result, time, reason where required, correlation reference where applicable, risk level where applicable, and restricted payload visibility rules.
- **FR-025**: Audit entries and audit exports MUST be append-only from a reviewer perspective; corrections or redactions must create new reviewable evidence and must not erase the original event except where a separate legally required privacy process authorizes limited masking.
- **FR-026**: Audit trail access, audit exports, dashboard reads, configuration changes, document exports, certificate verification, search queries, alert actions, and denied access attempts MUST themselves create audit evidence.
- **FR-027**: Metrics and monitoring MUST provide authorized users with operational measures for document processing, certificate issuance, search freshness, search failures, audit event flow, feature configuration changes, access denials, alert volume, incident state, and platform health.
- **FR-028**: Alert rules MUST support school-account and platform scopes, metric source, threshold or condition, severity, owner or queue, notification eligibility, quieting or suppression reason, review requirement, and audit evidence.
- **FR-029**: Alert and incident handling MUST support creation, acknowledgement, assignment, severity change, status update, resolution, reopening, linked evidence, and review summaries while preserving actor, time, reason, current state, and audit evidence.
- **FR-030**: The system MUST detect and record operational exceptions, including document upload failure, document scan or validation failure, invalid document category, unauthorized document access, certificate source mismatch, duplicate active certificate, failed certificate verification, stale search index, failed search indexing, excessive search scope, audit event ingestion gap, suspicious audit export, invalid feature configuration, missing metric data, noisy alert, and manual-review-required condition.
- **FR-031**: Retention and legal hold rules MUST preserve required documents, certificates, audit entries, exports, search query logs, configuration changes, alerts, incidents, and operational evidence according to school policy, legal hold, and review state, and MUST prevent deletion while hold or retention protection applies.
- **FR-032**: The system MUST support controlled exports for documents, certificates, search results, audit trails, configuration history, metrics summaries, and incident records only when export capability is enabled, the actor has export permission, the export scope is validated, and an export reason is recorded.
- **FR-033**: Phase 10 and Phase 11 MUST make eligible document, certificate, search, audit, configuration, alert, incident, and exception status events available to later review or communication capabilities without directly creating general messages, attendance outcomes, payment actions, complaint resolutions, medical workflows, emergency workflows, or transport decisions.
- **FR-034**: Phase 10 and Phase 11 MUST explicitly exclude attendance generation, campus entry or exit decisions, NFC or QR scan processing, transport boarding or drop-off decisions, wallet refunds or payment actions, learning reward actions, request approval workflows, medical or emergency workflows, complaint resolution workflows, communication delivery management, and source-domain record mutation from deliverable scope.
- **FR-035**: Sensitive details MUST be hidden from users without explicit school account, guardian-link, student ownership, staff assignment, source-module, document ownership, certificate issuer, audit, operations, review, or platform review authority.

### Key Entities *(include if feature involves data)*

- **Document**: A tenant-owned managed record with category, title, owner or subject context, source reference, visibility, retention rule, legal hold state, status, active version, and audit history.
- **Document Version**: A preserved version of a document, including original or replacement evidence, version reason, active or superseded state, actor, time, visibility impact, and retention impact.
- **Document Category**: A school-account rule set defining allowed actors, required metadata, subject types, visibility defaults, retention behavior, legal hold behavior, export eligibility, and review requirements.
- **Document Access Decision**: A recorded decision that allows, denies, minimizes, or routes document access to review based on actor scope, relationship, role, assignment, source module, and restricted-detail policy.
- **Certificate**: A tenant-owned official school record issued for a student, guardian, staff member, or school context, including certificate type, issuer, source summary, validity, status, verification state, correction or revocation state, and audit evidence.
- **Certificate Type**: A configurable school-account certificate definition with required fields, issuer roles, allowed subjects, source evidence rules, validity behavior, visibility, verification rules, and revocation behavior.
- **Certificate Verification Record**: Evidence of a certificate verification attempt, including requester where known, certificate status shown, verification result, time, restricted visibility, and audit evidence.
- **Search Index Entry**: A tenant-scoped searchable representation of an eligible source record with record type, source module, permitted summary, visibility scope, index state, freshness state, and source reference.
- **Search Query Log**: A record of a search query, including actor, scope, filters, result categories, denied or minimized result evidence, export or result-open follow-up, and audit evidence.
- **Search Result Access Decision**: A per-result decision that allows, denies, minimizes, suppresses, or routes a result to review when a search result is listed or opened.
- **Admin Dashboard Summary**: A permission-scoped aggregate view of module status, usage, exceptions, pending reviews, feature state, document and certificate state, search health, audit activity, alerts, incidents, and operational health.
- **Tenant Feature Setting**: A school-account capability and policy setting that controls whether a Phase 10, Phase 11, or prior module workflow is available and under what constraints.
- **Feature Configuration Change**: A versioned change record for a tenant feature setting, including prior value, new value, actor, reason, effective window, dependency decision, approval state, and audit evidence.
- **Audit Event**: A preserved evidence record for a user, system, or platform action, including actor, action, target, result, school account, source module, time, reason, risk level, correlation reference, and restricted payload policy.
- **Audit Export**: A controlled export of permitted audit evidence with actor, reason, scope, filters, generated time, included fields, retention state, and audit evidence.
- **Metric Observation**: A tenant or platform operational measurement for workflow health, volume, latency, failures, denials, configuration change, alert state, or incident state.
- **Alert Rule**: A configured condition that evaluates metric observations and creates alerts with severity, owner or queue, scope, suppression behavior, and review requirements.
- **Alert**: A reviewable operational signal with affected scope, severity, owner, current state, evidence, acknowledgement, and audit history.
- **Incident**: A managed operational issue linked to one or more alerts, metrics, records, or exceptions, including status, severity, owner, timeline, resolution, and audit evidence.
- **Retention Policy**: A tenant or platform policy that defines retention, archive, deletion eligibility, legal hold behavior, export retention, and review constraints for Phase 10 and Phase 11 records.
- **Operational Exception**: A reviewable issue involving document, certificate, search, audit, configuration, metrics, alerting, retention, export, cross-school, stale data, or manual review conditions.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized staff can upload, classify, and make a document available to its permitted audience in under 2 minutes during review testing.
- **SC-002**: 100% of sampled unauthorized actors, inactive relationships, disabled document capabilities, cross-school contexts, restricted categories, and missing required metadata combinations are prevented from viewing, downloading, exporting, updating, archiving, restoring, or deleting documents.
- **SC-003**: Authorized users can find and open a permitted document or certificate through search in under 30 seconds during review testing.
- **SC-004**: 95% of eligible document and certificate changes become searchable for authorized users within 5 minutes while stale or failed indexing states remain reviewable.
- **SC-005**: Authorized certificate issuers can issue a complete eligible certificate in under 2 minutes during review testing.
- **SC-006**: 100% of sampled certificate corrections, revocations, expirations, supersessions, and reissues preserve original issuance evidence, actor, time, reason, verification impact, and audit evidence.
- **SC-007**: 100% of sampled search results are visible only within the authorized school account, guardian link, student ownership, staff assignment, source-module, document ownership, audit, operations, review, or platform review scope.
- **SC-008**: 95% of scoped searches return permitted result lists in under 10 seconds during review testing.
- **SC-009**: Authorized school administrators can open the admin dashboard and see enabled feature status, open exceptions, pending reviews, and operational health summaries in under 30 seconds during review testing.
- **SC-010**: 100% of sampled tenant feature configuration changes preserve prior setting, new setting, actor, reason, dependency decision, effective window, and audit evidence.
- **SC-011**: 100% of sampled invalid or unsafe tenant feature configuration changes are rejected or routed to review without applying partial changes.
- **SC-012**: Authorized auditors can trace a sampled document, certificate, search query, feature configuration change, alert, or access denial from action through audit evidence in under 60 seconds during review testing.
- **SC-013**: 100% of sampled audit exports include only permitted fields and record actor, reason, scope, filters, generated time, and export evidence.
- **SC-014**: 95% of configured metric threshold breaches create or update the correct alert or incident owner view within 5 minutes of the condition becoming visible for review.
- **SC-015**: 100% of sampled legal hold and retention-protected records cannot be deleted or hidden from authorized review while protection applies.
- **SC-016**: 100% of sampled Phase 10 and Phase 11 actions create no attendance, campus gate, scan, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, communication delivery, or source-domain mutation outcome.
- **SC-017**: 100% of sampled records containing medical, complaint, finance, staff, safety, student-welfare, audit, or platform operations context show only permitted summaries, minimized metadata, or no result to unauthorized actors.
- **SC-018**: 90% of school administrators and auditors in review testing can complete their primary document search, audit lookup, dashboard review, or configuration check without assistance after one orientation session.

## Assumptions

- Phase 10 and Phase 11 build on Phase 0 tenant configuration, feature flag, audit, observability, and shared configuration foundations, plus Phase 1 identity, guardian linking, role, and permission capabilities.
- Prior phases expose eligible source references, summaries, audit events, and status events for document, search, dashboard, audit, and monitoring use without making Phase 10 or Phase 11 the source of truth for the originating workflow.
- Documents may reference records from earlier phases, but source-domain records remain owned by their original modules.
- Certificates summarize or reference allowed school evidence and do not replace the source records that justify them.
- Search is authenticated by default and must re-check current access when results are listed and opened.
- Tenant feature configuration is school-account scoped; platform-level cross-school configuration requires explicit platform authority and audit evidence.
- Audit events from prior phases are available as reviewable evidence even if some older events contain minimized payloads.
- Metrics and alerts are operational signals for review and response; they do not guarantee that the originating business workflow completed successfully unless the source module provides that evidence.
- External document exports, audit exports, and certificate verification views are controlled capabilities and are disabled unless explicitly enabled for the school account or platform role.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 10 or Phase 11 requirements.
