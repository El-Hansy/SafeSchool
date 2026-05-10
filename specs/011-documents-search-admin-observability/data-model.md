# Data Model: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

This model defines runtime business entities for document storage,
certificates, search, admin dashboards, tenant configuration, audit trail,
metrics, alerts, incidents, retention, legal holds, exports, operational
exceptions, and review evidence. All tenant-owned entities include
`tenant_id`, `created_at`, and `updated_at`.

## Document

**Purpose**: A tenant-owned managed record with classified metadata, source
context, visibility, retention, legal hold, and active version.

**Fields**:
- `document_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `document_reference`: Human-friendly reference.
- `document_category_id`: Category governing metadata, visibility, retention,
  export, and review rules.
- `title`: Document title.
- `description`: Optional permitted summary.
- `owner_actor_id`: Actor owner where applicable.
- `subject_type`: Student, Guardian, Staff, School, Finance, Medical,
  Complaint, Transport, Learning, Request, Communication, Platform.
- `subject_reference`: Student, staff, guardian link, source, or school record
  reference.
- `source_module`: Source module when document is attached to a prior workflow.
- `source_record_reference`: Source-domain record reference when applicable.
- `visibility_level`: Recipient Visible, Staff Visible, Restricted, Reviewer
  Only, Platform Operator.
- `document_status`: Draft, Pending Review, Active, Archived, Superseded,
  Restored, Deleted Pending Retention, Legal Hold, Review Required.
- `active_version_id`: Current active document version.
- `retention_policy_id`: Retention policy in effect.
- `legal_hold_state`: None, Hold Pending, On Hold, Released.
- `export_allowed`: Whether policy allows export.
- `created_by_actor_id`: Actor that created the document.
- `updated_by_actor_id`: Actor that last changed metadata.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and one Document Category.
- Has many Document Versions, Document Access Decisions, Search Index Entries,
  Audit Events, Legal Holds, Controlled Exports, and Operational Exceptions.
- May reference one source-domain record without mutating it.

**Validation rules**:
- Create, read, update, version, archive, restore, hold, export, retain, and
  delete actions require tenant access, enabled capability, actor permission,
  category rules, source visibility, restricted-detail policy, and audit
  evidence.
- Documents under active legal hold or retention protection cannot be deleted
  or hidden from authorized review.
- Document actions must not create outcomes in attendance, campus access,
  scans, transport, wallet, learning rewards, request approvals, medical,
  emergency, complaint resolution, or communication delivery.

## Document Version

**Purpose**: Preserved file and metadata evidence for one version of a
document.

**Fields**:
- `document_version_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `document_id`: Parent document.
- `version_number`: Monotonic document version.
- `object_storage_key`: Stored object reference.
- `file_name`: Original file name.
- `file_type`: Declared file type.
- `file_size_bytes`: Stored file size.
- `content_hash`: Evidence for duplicate and integrity checks.
- `validation_state`: Pending, Valid, Failed, Quarantined, Review Required.
- `version_status`: Draft, Active, Superseded, Archived, Restored, Withdrawn.
- `version_reason`: Reason for upload, correction, replacement, archive, or
  restore.
- `visibility_impact`: Summary of visibility change where applicable.
- `retention_impact`: Summary of retention change where applicable.
- `uploaded_by_actor_id`: Uploading actor.
- `activated_at`: Time this version became active.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- New versions preserve original versions and require a reason.
- Active version changes require category permission, retention validation,
  legal hold validation, and audit evidence.
- Quarantined or failed validation versions cannot be published or exported.

## Document Category

**Purpose**: A versioned school-account policy for document metadata,
visibility, retention, legal hold, export, and review behavior.

**Fields**:
- `document_category_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `category_code`: School-unique code.
- `category_name`: Display name.
- `allowed_actor_roles`: Roles allowed to create or manage this category.
- `allowed_subject_types`: Subject types permitted for this category.
- `required_metadata`: Required fields.
- `default_visibility_level`: Default visibility.
- `restricted_detail_policy`: Full, Summary Only, Withhold, Review Required.
- `retention_policy_id`: Default retention policy.
- `legal_hold_eligible`: Whether holds can be placed.
- `export_eligible`: Whether controlled export is allowed.
- `publication_review_required`: Whether review is required before active use.
- `category_status`: Draft, Active, Suspended, Archived.
- `version`: Category version.
- `effective_from`: Start of validity.
- `effective_to`: End of validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires valid required metadata, subject types, visibility,
  retention, export, legal hold, and dependent capabilities.
- Historical documents preserve the category version that governed each
  relevant action.

## Document Access Decision

**Purpose**: Evidence that a document access attempt was allowed, denied,
minimized, or routed to review.

**Fields**:
- `document_access_decision_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `document_id`: Target document.
- `actor_id`: Actor requesting access.
- `requested_action`: View, Download, Update, Archive, Restore, Hold, Export,
  Delete, Search Open.
- `decision`: Allowed, Denied, Minimized, Review Required.
- `decision_reason`: Reason for decision.
- `visibility_level_granted`: Detail level granted.
- `guardian_link_id`: Guardian relationship evidence when applicable.
- `student_profile_id`: Student context when applicable.
- `source_module`: Source module considered.
- `decided_at`: Decision time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Access decisions cannot grant visibility beyond tenant, guardian-link,
  student ownership, staff assignment, source-module, document ownership,
  audit, operations, review, or platform review authority.

## Certificate

**Purpose**: An official school record issued for a student, guardian, staff
member, or school context.

**Fields**:
- `certificate_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `certificate_reference`: Human-friendly reference.
- `certificate_type_id`: Governing certificate type.
- `recipient_actor_id`: Recipient actor when applicable.
- `subject_type`: Student, Staff, Guardian, School.
- `subject_reference`: Student, staff, guardian, or school reference.
- `source_summary`: Permitted summary of source evidence.
- `source_module`: Source module used for evidence.
- `source_record_reference`: Source-domain record reference when applicable.
- `document_id`: Rendered or attached certificate document where applicable.
- `issuer_actor_id`: Actor that issued the certificate.
- `language`: Language metadata.
- `certificate_status`: Draft, Issued, Corrected, Revoked, Expired,
  Superseded, Reissued, Review Required.
- `verification_state`: Valid, Invalid, Expired, Revoked, Superseded,
  Review Required.
- `valid_from`: Certificate validity start.
- `valid_to`: Certificate validity end.
- `correction_of_certificate_id`: Prior certificate when corrected.
- `revocation_reason`: Reason when revoked.
- `correction_reason`: Reason when corrected.
- `issued_at`: Issuance time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and one Certificate Type.
- May reference one Document and source-domain evidence.
- Has many Certificate Verification Records, Search Index Entries, Audit
  Events, Controlled Exports, and Operational Exceptions.

**Validation rules**:
- Issuance requires tenant access, enabled certificate capability, issuer
  authority, subject eligibility, certificate type availability, required
  fields, duplicate active certificate validation, source visibility, and audit
  evidence.
- Corrections, revocations, expirations, supersessions, and reissues preserve
  original issuance evidence and require reason capture.
- Certificates must not mutate academic, attendance, payment, complaint,
  medical, request, learning, transport, or communication outcomes.

## Certificate Type

**Purpose**: A versioned school-account certificate policy.

**Fields**:
- `certificate_type_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `type_code`: School-unique code.
- `type_name`: Display name.
- `allowed_subject_types`: Student, Staff, Guardian, School.
- `issuer_roles`: Roles allowed to issue.
- `required_fields`: Required certificate fields.
- `source_evidence_rules`: Required and allowed source evidence.
- `default_validity_period`: Default certificate validity.
- `visibility_level`: Default certificate visibility.
- `verification_policy`: Internal, Recipient, External Link, Reviewer Only.
- `duplicate_active_policy`: Reject, Supersede, Review Required.
- `revocation_policy`: Who can revoke and when.
- `type_status`: Draft, Active, Suspended, Archived.
- `version`: Certificate type version.
- `effective_from`: Start of validity.
- `effective_to`: End of validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires valid issuer roles, subject rules, source evidence rules,
  validity behavior, visibility, duplicate policy, and dependent capabilities.

## Certificate Verification Record

**Purpose**: Evidence that a certificate was verified or denied.

**Fields**:
- `certificate_verification_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `certificate_id`: Target certificate.
- `requester_actor_id`: Requesting actor when known.
- `verification_result`: Valid, Invalid, Expired, Revoked, Superseded, Denied,
  Review Required.
- `visible_status`: Status shown to requester.
- `visibility_level_granted`: Detail level granted.
- `denial_reason`: Reason when denied.
- `verified_at`: Verification time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Verification reveals only permitted status and summary for the actor's
  authority.
- Revoked, expired, or superseded certificates cannot verify as valid.

## Search Index Entry

**Purpose**: A tenant-scoped searchable representation of an eligible source
record.

**Fields**:
- `search_index_entry_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `record_type`: Document, Certificate, Student, Attendance, Transport,
  Wallet, Learning, Request, Medical, Complaint, Communication, Audit,
  Configuration, Alert, Incident.
- `source_module`: Owning source module.
- `source_record_reference`: Source-domain record reference.
- `title`: Permitted title.
- `summary`: Permitted summary.
- `search_terms`: Searchable normalized terms.
- `student_profile_id`: Student context when applicable.
- `subject_reference`: Subject context when applicable.
- `category`: Search category.
- `record_status`: Source record status summary.
- `visibility_scope`: Actor, Guardian Link, Student Self, Role, Assignment,
  Reviewer, Platform Operator.
- `restricted_detail_policy`: Full, Summary Only, Withhold, Review Required.
- `index_state`: Pending, Indexed, Stale, Failed, Suppressed, Reindex Required,
  Review Required.
- `freshness_state`: Fresh, Possibly Stale, Stale, Unknown.
- `last_indexed_at`: Last successful indexing time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Entries can exist only for source records marked searchable for at least one
  authorized audience.
- Listing and opening results revalidate current tenant, role, relationship,
  source-module, feature, and restricted-detail authority.
- Restricted counts, facets, snippets, and previews cannot reveal hidden
  records.

## Search Query Log

**Purpose**: Audit-visible evidence of a search query and result-open follow-up.

**Fields**:
- `search_query_log_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `actor_id`: Searching actor.
- `query_text`: Search text or structured query summary.
- `filters`: Applied filters.
- `result_category_counts`: Permitted category counts only.
- `denied_result_count`: Count of denied results where safe to disclose to
  reviewers.
- `minimized_result_count`: Count of minimized results.
- `export_requested`: Whether the query led to export.
- `searched_at`: Query time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Query logs are visible only to actors with search audit authority.
- Broad or suspicious queries can create operational exceptions.

## Search Result Access Decision

**Purpose**: Per-result evidence that a search result was allowed, denied,
minimized, suppressed, or routed to review.

**Fields**:
- `search_result_access_decision_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `search_query_log_id`: Parent query.
- `search_index_entry_id`: Target result.
- `actor_id`: Requesting actor.
- `decision`: Listed, Opened, Denied, Minimized, Suppressed, Review Required.
- `decision_reason`: Reason for decision.
- `visible_fields`: Fields shown to the actor.
- `decided_at`: Decision time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Result decisions must be recomputed when the result is opened.
- Denied and suppressed results cannot leak restricted title, summary, count,
  snippet, preview, or source existence to unauthorized actors.

## Admin Dashboard Summary

**Purpose**: Permission-scoped aggregate view of module status, usage,
exceptions, configuration, audit, alerts, incidents, and operational health.

**Fields**:
- `admin_dashboard_summary_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope for authorized
  platform summaries.
- `summary_scope`: School, Module, Platform, Feature, Risk, Operations.
- `module_key`: Module summarized where applicable.
- `feature_status_counts`: Enabled, Disabled, Pending, Review Required counts.
- `usage_counts`: Permission-scoped usage counts.
- `exception_counts`: Open exception counts.
- `pending_review_counts`: Pending review counts.
- `document_certificate_counts`: Document and certificate state counts.
- `search_health_counts`: Search index freshness counts.
- `audit_activity_counts`: Audit event counts.
- `alert_incident_counts`: Alert and incident counts.
- `generated_for_actor_id`: Actor receiving summary.
- `generated_at`: Summary generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Summary counts cannot reveal restricted records or cross-school details
  outside actor authority.
- Dashboard reads create audit evidence.

## Tenant Feature Setting

**Purpose**: A school-account capability and policy setting controlling Phase
10, Phase 11, and related module availability.

**Fields**:
- `tenant_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `feature_key`: Feature capability key.
- `setting_state`: Enabled, Disabled, Scheduled, Suspended, Review Required.
- `setting_value`: Structured setting value.
- `dependency_state`: Valid, Missing Dependency, Conflicting Dependency,
  Review Required.
- `effective_from`: Start of setting validity.
- `effective_to`: End of setting validity.
- `version`: Setting version.
- `changed_by_actor_id`: Actor that last changed the setting.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Changes require tenant access, configuration authority, dependency
  validation, school policy validation, reason capture, and audit evidence.
- Disabling a feature cannot hide required audit, retention, legal hold, or
  review evidence.

## Feature Configuration Change

**Purpose**: Versioned evidence for a tenant feature configuration change.

**Fields**:
- `configuration_change_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `tenant_feature_setting_id`: Changed setting.
- `prior_setting_value`: Prior value.
- `new_setting_value`: New value.
- `change_reason`: Required reason.
- `dependency_decision`: Dependency validation result.
- `approval_state`: Not Required, Pending, Approved, Rejected, Review Required.
- `effective_from`: Effective start.
- `effective_to`: Effective end.
- `changed_by_actor_id`: Actor who submitted change.
- `approved_by_actor_id`: Approver when applicable.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Invalid or unsafe changes are rejected or routed to review and cannot apply
  partial effects.
- Historical records preserve the configuration version that governed them.

## Audit Event

**Purpose**: Preserved evidence for user, system, or platform actions.

**Fields**:
- `audit_event_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `actor_id`: Actor that performed the action.
- `actor_role`: Actor role at action time.
- `source_module`: Module that emitted the event.
- `action_name`: Action performed.
- `target_record_type`: Target record type.
- `target_record_reference`: Target record reference.
- `result`: Success, Denied, Failed, Suppressed, Review Required.
- `reason`: Reason where required.
- `risk_level`: Low, Normal, Elevated, High, Critical.
- `correlation_reference`: Related operation reference.
- `payload_visibility_policy`: Full, Minimized, Restricted, Reviewer Only,
  Platform Operator.
- `occurred_at`: Event time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Audit events are append-only from a reviewer perspective.
- Corrections or redactions create new review evidence and cannot erase the
  original event except through a separately authorized privacy masking
  process.

## Audit Export

**Purpose**: Controlled export of permitted audit evidence.

**Fields**:
- `audit_export_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `requested_by_actor_id`: Export requester.
- `export_scope`: School, Actor, Student, Source Module, Date Range, Target
  Record, Risk Level, Correlation.
- `filters`: Export filters.
- `included_fields`: Fields included.
- `export_reason`: Required reason.
- `export_status`: Requested, Generating, Complete, Failed, Expired,
  Revoked, Review Required.
- `retention_policy_id`: Retention policy for export artifact.
- `generated_at`: Export generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Export requires audit export capability, actor permission, validated scope,
  permitted fields, reason capture, retention policy, and audit evidence.

## Metric Observation

**Purpose**: Tenant or platform operational measurement for workflow health,
volume, latency, failures, denials, configuration, alerts, or incidents.

**Fields**:
- `metric_observation_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `metric_name`: Stable metric name.
- `metric_source`: Source module or platform component.
- `metric_scope`: School, Module, Feature, Platform, Actor, Record Type.
- `observed_value`: Numeric or categorical value.
- `threshold_context`: Threshold context at observation time.
- `data_quality_state`: Complete, Missing, Delayed, Duplicate, Noisy,
  Inconsistent, Review Required.
- `observed_at`: Observation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Missing, delayed, noisy, or inconsistent metrics create reviewable evidence
  instead of being silently ignored when they affect alert evaluation.

## Alert Rule

**Purpose**: A configured condition that evaluates metrics and creates alerts.

**Fields**:
- `alert_rule_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `rule_name`: Display name.
- `metric_name`: Metric evaluated.
- `condition`: Threshold or condition summary.
- `severity`: Low, Normal, Elevated, High, Critical.
- `owner_queue`: Queue or role receiving alerts.
- `suppression_policy`: Quiet, Suppress Duplicate, Require Review, None.
- `notification_eligible`: Whether later communication can notify owners.
- `rule_status`: Draft, Active, Suspended, Archived.
- `version`: Rule version.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires valid metric source, condition, severity, owner, scope,
  suppression behavior, and dependent capabilities.

## Alert

**Purpose**: Reviewable operational signal created by metric or exception
conditions.

**Fields**:
- `alert_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `alert_rule_id`: Triggering rule when applicable.
- `alert_source`: Metric, Exception, Manual, System.
- `affected_scope`: School, Module, Feature, Platform, Record.
- `severity`: Low, Normal, Elevated, High, Critical.
- `alert_status`: Open, Acknowledged, Assigned, Suppressed, Resolved,
  Reopened, Review Required.
- `owner_actor_id`: Assigned owner where applicable.
- `owner_queue`: Assigned queue where applicable.
- `triggered_at`: Trigger time.
- `acknowledged_at`: Acknowledgement time.
- `resolved_at`: Resolution time.
- `current_reason`: Current state reason.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Alert actions require operations authority, reason where required, and audit
  evidence.
- Suppressed alerts preserve suppression reason and evidence.

## Incident

**Purpose**: Managed operational issue linked to one or more alerts, metrics,
records, or exceptions.

**Fields**:
- `incident_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `incident_reference`: Human-friendly reference.
- `incident_title`: Incident title.
- `incident_summary`: Permitted summary.
- `severity`: Low, Normal, Elevated, High, Critical.
- `incident_status`: Open, Investigating, Mitigating, Resolved, Reopened,
  Closed, Review Required.
- `owner_actor_id`: Assigned owner where applicable.
- `owner_queue`: Assigned queue where applicable.
- `linked_alert_ids`: Linked alerts.
- `linked_record_references`: Related records.
- `resolution_summary`: Resolution evidence.
- `opened_at`: Open time.
- `resolved_at`: Resolve time.
- `closed_at`: Close time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Incident lifecycle changes require operations authority, reason where
  required, and audit evidence.
- Platform-scope incidents can be visible across schools only to explicit
  platform roles.

## Retention Policy

**Purpose**: Policy defining retention, archive, deletion eligibility, legal
hold behavior, export retention, and review constraints.

**Fields**:
- `retention_policy_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `policy_name`: Display name.
- `record_types`: Document, Certificate, Audit, Export, Search Log,
  Configuration, Alert, Incident, Metric.
- `retention_period`: Required retention period.
- `archive_behavior`: None, Archive After Period, Review Before Archive.
- `deletion_behavior`: Not Allowed, Delete After Retention, Review Required.
- `legal_hold_behavior`: Hold Eligible, Hold Required, Hold Not Allowed.
- `policy_status`: Draft, Active, Suspended, Archived.
- `version`: Policy version.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Retention policy activation must not conflict with legal hold or audit
  obligations.

## Legal Hold

**Purpose**: Preservation control that blocks deletion or hiding of protected
records.

**Fields**:
- `legal_hold_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `hold_name`: Display name.
- `target_record_type`: Protected record type.
- `target_record_reference`: Protected record reference.
- `hold_reason`: Required reason.
- `hold_status`: Pending, Active, Released, Rejected.
- `placed_by_actor_id`: Actor who placed hold.
- `released_by_actor_id`: Actor who released hold.
- `placed_at`: Placement time.
- `released_at`: Release time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Active holds prevent deletion, permanent hiding, and retention expiry
  destruction while hold applies.
- Hold placement and release require explicit authority and audit evidence.

## Controlled Export

**Purpose**: Export evidence for documents, certificates, search results,
configuration history, metrics summaries, and incident records.

**Fields**:
- `controlled_export_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `export_kind`: Document, Certificate, Search Results, Configuration,
  Metrics, Incident, Mixed.
- `requested_by_actor_id`: Export requester.
- `export_scope`: Validated export scope.
- `filters`: Export filters.
- `included_fields`: Fields included.
- `export_reason`: Required reason.
- `export_status`: Requested, Generating, Complete, Failed, Expired, Revoked,
  Review Required.
- `artifact_reference`: Export artifact reference when generated.
- `retention_policy_id`: Retention policy.
- `generated_at`: Generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Export cannot include records or fields outside the actor's export scope.
- Export artifacts follow retention and legal hold policy.

## Operational Exception

**Purpose**: Reviewable issue involving document, certificate, search, audit,
configuration, metrics, alerting, retention, export, cross-school, stale data,
or manual review conditions.

**Fields**:
- `operational_exception_id`: Stable identifier.
- `tenant_id`: Owning school account, or platform scope where authorized.
- `exception_type`: Document Upload Failure, Validation Failure, Invalid
  Category, Unauthorized Access, Certificate Source Mismatch, Duplicate
  Certificate, Verification Failure, Stale Search Index, Indexing Failure,
  Excessive Search Scope, Audit Gap, Suspicious Export, Invalid
  Configuration, Missing Metric, Noisy Alert, Retention Conflict, Manual
  Review Required.
- `severity`: Low, Normal, Elevated, High, Critical.
- `source_module`: Source module.
- `target_record_reference`: Related record.
- `source_evidence`: Evidence summary.
- `exception_status`: Open, Under Review, Resolved, Dismissed, Escalated.
- `reviewer_actor_id`: Assigned reviewer where applicable.
- `resolution_reason`: Resolution reason.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Exceptions preserve source evidence and cannot be deleted to hide an issue.
- Resolution requires reviewer authority, reason, and audit evidence.
