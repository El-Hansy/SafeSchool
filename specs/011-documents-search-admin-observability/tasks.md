# Tasks: Phase 10 Documents & Search and Phase 11 Admin, Audit & Observability

**Input**: Design documents from `/specs/011-documents-search-admin-observability/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Include tests required by the constitution and Phase 10/11 plan. Business logic requires unit tests; API contracts, tenant/feature authorization, migrations, search authorization, retention/legal hold, exports, audit, metrics, alerts, incidents, and critical user journeys require integration or contract coverage.

**Organization**: Tasks are grouped by user story so each story can be implemented, tested, and reviewed independently after the shared foundation is complete.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label from spec.md. Setup, Foundational, and Polish tasks do not use story labels.
- Every task includes exact file paths. If a path does not exist yet, create it as part of that task.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the module directories, skeleton files, and test locations that all later tasks will use.

- [ ] T001 Create the Documents backend directory tree from the implementation plan in `apps/api/src/SafeSchool.Api/Features/Documents/`
- [ ] T002 [P] Create the Administration backend directory tree from the implementation plan in `apps/api/src/SafeSchool.Api/Features/Administration/`
- [ ] T003 [P] Create Documents backend test directory tree in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/`
- [ ] T004 [P] Create Administration backend test directory tree in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/`
- [ ] T005 [P] Create cross-service contract test directory tree in `tests/contracts/documents-admin/`
- [ ] T006 [P] Create end-to-end test directory tree in `tests/e2e/documents-admin/`
- [ ] T007 [P] Create Documents and Administration web feature directories in `apps/admin-web/src/features/documents/` and `apps/admin-web/src/features/administration/`
- [ ] T008 [P] Create school web route directories in `apps/admin-web/src/app/(school)/documents/`, `apps/admin-web/src/app/(school)/certificates/`, `apps/admin-web/src/app/(school)/search/`, and `apps/admin-web/src/app/(school)/admin/`
- [ ] T009 [P] Create guardian and student web route directories in `apps/admin-web/src/app/(guardian)/documents/`, `apps/admin-web/src/app/(guardian)/certificates/`, `apps/admin-web/src/app/(student)/documents/`, and `apps/admin-web/src/app/(student)/certificates/`
- [ ] T010 [P] Create Documents and Administration web test directory tree in `apps/admin-web/tests/documents-admin/`
- [ ] T011 [P] If `apps/mobile/` exists or mobile document surfaces are enabled, create mobile feature and test directory trees in `apps/mobile/lib/features/documents/` and `apps/mobile/test/features/documents/`
- [ ] T012 Create Documents backend module registration skeleton in `apps/api/src/SafeSchool.Api/Features/Documents/DocumentsModule.cs`
- [ ] T013 Create Administration backend module registration skeleton in `apps/api/src/SafeSchool.Api/Features/Administration/AdministrationModule.cs`
- [ ] T014 [P] Create Documents module implementation notes for lower-cost LLMs in `apps/api/src/SafeSchool.Api/Features/Documents/README.md`
- [ ] T015 [P] Create Administration module implementation notes for lower-cost LLMs in `apps/api/src/SafeSchool.Api/Features/Administration/README.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Implement shared constants, guards, persistence entities, audit, metrics, object-storage abstractions, search abstractions, DTOs, fixtures, and migrations that every user story depends on.

**Critical**: No user story implementation should begin until this phase is complete.

- [ ] T016 Create Phase 10/11 capability constants for documents, certificates, search, exports, retention, legal hold, dashboard, tenant configuration, audit trail, metrics, alerting, incidents, and operational exceptions in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentCapabilities.cs`
- [ ] T017 Create Phase 10/11 permission constants for all roles and actions in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentPermissions.cs`
- [ ] T018 Create shared enum definitions for document status, version status, certificate status, verification state, search index state, access decision, feature setting state, audit result, metric quality, alert state, incident state, export state, retention state, legal hold state, and exception type in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentEnums.cs`
- [ ] T019 Create stable error codes and validation messages for Documents and Administration workflows in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentErrors.cs`
- [ ] T020 Create audit event names for all Phase 10/11 actions required by spec.md FR-026 in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AdminDocumentAuditEvents.cs`
- [ ] T021 Create a tenant, capability, and school-account guard for Documents and Administration workflows in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentTenantFeatureGuard.cs`
- [ ] T022 Create a role, permission, guardian-link, student self-scope, staff assignment, source-module, reviewer, platform-operator, export, and operations authorization guard in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentAuthorizationGuard.cs`
- [ ] T023 Create a restricted-detail visibility policy for documents, certificates, search, dashboards, audit payloads, metrics, alerts, incidents, and exports in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentVisibilityPolicy.cs`
- [ ] T024 Create a source-reference validation service that verifies prior phase record visibility without mutating source workflows in `apps/api/src/SafeSchool.Api/Features/Administration/Common/SourceReferenceValidationService.cs`
- [ ] T025 Create idempotency helpers for document, certificate, reindex, configuration, export, alert, incident, and review commands in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentIdempotencyService.cs`
- [ ] T026 Create retention and legal-hold policy helpers shared by documents, certificates, audit, exports, search logs, configuration, alerts, incidents, and metrics in `apps/api/src/SafeSchool.Api/Features/Documents/Retention/RetentionLegalHoldPolicy.cs`
- [ ] T027 Create controlled export policy helpers for field minimization, export scope validation, reason capture, and export retention in `apps/api/src/SafeSchool.Api/Features/Documents/Exports/ControlledExportPolicy.cs`
- [ ] T028 Create object storage abstraction interfaces and object metadata validation for document content in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentObjectStorage.cs`
- [ ] T029 Create search indexing abstraction interfaces for source eligibility, index freshness, result access decisions, and reindex commands in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchIndexingAbstractions.cs`
- [ ] T030 Create metric and alert evaluation abstraction interfaces for observations, thresholds, alert creation, and incident linking in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/MonitoringAbstractions.cs`
- [ ] T031 [P] Create the Document entity with all data-model fields and tenant timestamps in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/Document.cs`
- [ ] T032 [P] Create the DocumentVersion entity with object reference, validation, version, visibility, retention, and audit fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/DocumentVersion.cs`
- [ ] T033 [P] Create the DocumentCategory entity with actor, metadata, subject, visibility, retention, legal hold, export, review, status, and version fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/DocumentCategory.cs`
- [ ] T034 [P] Create the DocumentAccessDecision entity with action, decision, reason, granted visibility, relationship, source module, and timing fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/DocumentAccessDecision.cs`
- [ ] T035 [P] Create the Certificate and CertificateType entities with issuance, verification, correction, revocation, subject, source evidence, validity, status, and version fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/Certificate.cs` and `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/CertificateType.cs`
- [ ] T036 [P] Create the CertificateVerificationRecord entity with requester, result, visible status, denial reason, visibility, and timing fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/CertificateVerificationRecord.cs`
- [ ] T037 [P] Create the SearchIndexEntry, SearchQueryLog, and SearchResultAccessDecision entities with source, visibility, freshness, query, and per-result decision fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/SearchIndexEntry.cs`, `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/SearchQueryLog.cs`, and `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/SearchResultAccessDecision.cs`
- [ ] T038 [P] Create the AdminDashboardSummary entity with feature, usage, exception, pending review, search health, audit, alert, and incident summary fields in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/AdminDashboardSummary.cs`
- [ ] T039 [P] Create the TenantFeatureSetting and FeatureConfigurationChange entities with dependency, approval, effective window, reason, and version fields in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/TenantFeatureSetting.cs` and `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/FeatureConfigurationChange.cs`
- [ ] T040 [P] Create the AuditEvent and AuditExport entities with action, actor, target, risk, payload visibility, export scope, included fields, retention, and evidence fields in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/AuditEvent.cs` and `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/AuditExport.cs`
- [ ] T041 [P] Create the MetricObservation, AlertRule, Alert, and Incident entities with metric, data quality, threshold, severity, owner, lifecycle, and evidence fields in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/MetricObservation.cs`, `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/AlertRule.cs`, `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/Alert.cs`, and `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/Incident.cs`
- [ ] T042 [P] Create the RetentionPolicy, LegalHold, ControlledExport, and OperationalException entities with protection, export, review, severity, and resolution fields in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/RetentionPolicy.cs`, `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/LegalHold.cs`, `apps/api/src/SafeSchool.Api/Features/Documents/Data/Entities/ControlledExport.cs`, and `apps/api/src/SafeSchool.Api/Features/Administration/Data/Entities/OperationalException.cs`
- [ ] T043 Create EF Core DbContext extensions and DbSet registration for all Phase 10/11 entities in `apps/api/src/SafeSchool.Api/Features/Administration/Data/AdminDocumentsDbContextExtensions.cs`
- [ ] T044 [P] Create EF Core mappings and tenant indexes for Document, DocumentVersion, DocumentCategory, DocumentAccessDecision, RetentionPolicy, LegalHold, and ControlledExport in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Configurations/DocumentEntityConfigurations.cs`
- [ ] T045 [P] Create EF Core mappings and tenant indexes for Certificate, CertificateType, and CertificateVerificationRecord in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Configurations/CertificateEntityConfigurations.cs`
- [ ] T046 [P] Create EF Core mappings and tenant indexes for SearchIndexEntry, SearchQueryLog, and SearchResultAccessDecision in `apps/api/src/SafeSchool.Api/Features/Documents/Data/Configurations/SearchEntityConfigurations.cs`
- [ ] T047 [P] Create EF Core mappings and tenant indexes for AdminDashboardSummary, TenantFeatureSetting, FeatureConfigurationChange, AuditEvent, AuditExport, MetricObservation, AlertRule, Alert, Incident, and OperationalException in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Configurations/AdministrationEntityConfigurations.cs`
- [ ] T048 Create Phase 10/11 EF Core migration for all document, certificate, search, admin, audit, metric, alert, incident, retention, legal hold, export, and operational exception tables in `apps/api/src/SafeSchool.Api/Features/Administration/Data/Migrations/202605060003_AddDocumentsSearchAdminObservability.cs`
- [ ] T049 [P] Create seed data for baseline capabilities, permissions, document categories, certificate types, retention policies, feature settings, alert rules, dashboard scopes, and review queues in `apps/api/src/SafeSchool.Api/Features/Administration/Seed/AdminDocumentSeedData.cs`
- [ ] T050 [P] Create shared DTOs for actor context, source references, visibility summaries, pagination, access decisions, export status, review responses, and error responses in `apps/api/src/SafeSchool.Api/Features/Administration/Common/AdminDocumentCommonDtos.cs`
- [ ] T051 [P] Create audit writer for actor, tenant, source module, target record, result, reason, risk, payload visibility, and correlation evidence in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AdminDocumentAuditWriter.cs`
- [ ] T052 [P] Create metric writer and operational exception writer for Phase 10/11 lifecycle evidence in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/AdminDocumentMetricExceptionWriters.cs`
- [ ] T053 Create shared backend test fixtures for tenants, users, roles, permissions, guardian links, students, feature flags, source records, object storage stubs, audit capture, metrics, and idempotency in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentTestFixture.cs`
- [ ] T054 [P] Create contract test fixture utilities for route authentication, tenant headers, feature flags, idempotency, pagination, export, and response assertions in `tests/contracts/documents-admin/AdminDocumentContractTestFixture.cs`
- [ ] T055 [P] Create web test fixtures for school administrators, document managers, certificate issuers, guardians, students, auditors, compliance reviewers, operations reviewers, and platform operators in `apps/admin-web/tests/documents-admin/adminDocumentTestFixtures.ts`
- [ ] T056 [P] If `apps/mobile/` exists or mobile document surfaces are enabled, create mobile test fixtures for document, certificate, and search flows in `apps/mobile/test/features/documents/document_test_fixtures.dart`
- [ ] T057 Create foundational tests for tenant isolation, feature-flag denial, permission denial, source-reference read-only boundaries, restricted-detail minimization, export scope, legal hold protection, audit emission, and no-side-effect boundaries in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentFoundationTests.cs`
- [ ] T058 Create migration tests that verify all Phase 10/11 tenant-owned tables include `tenant_id`, `created_at`, `updated_at`, required indexes, object references, source references, retention indexes, search indexes, audit indexes, alert indexes, and incident indexes in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentMigrationTests.cs`

**Checkpoint**: Foundation ready. User story phases may now proceed independently or in parallel.

---

## Phase 3: User Story 1 - Manage School Documents (Priority: P1) MVP

**Goal**: Authorized users can store, classify, version, access, archive, restore, hold, retain, and export documents without exposing restricted content or mutating source workflows.

**Independent Test**: Upload a tenant-scoped document for a student, staff process, or school context, classify it, assign visibility, create a replacement version, and verify only authorized actors can view, download, archive, restore, hold, or export it.

### Tests for User Story 1

- [ ] T059 [P] [US1] Create unit tests for document category activation, required metadata, subject type rules, visibility defaults, retention rules, legal hold eligibility, export eligibility, and review requirements in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Storage/DocumentCategoryPolicyTests.cs`
- [ ] T060 [P] [US1] Create unit tests for document access decisions across guardian links, student self-scope, staff assignments, source modules, ownership, reviewer scope, and platform authority in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Storage/DocumentAccessDecisionTests.cs`
- [ ] T061 [P] [US1] Create unit tests for document version state transitions, object metadata validation, archive, restore, validation failure, retention protection, and legal hold protection in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Storage/DocumentVersionRetentionTests.cs`
- [ ] T062 [P] [US1] Create contract tests for document-storage.md document, version, category, legal hold, and export endpoints in `tests/contracts/documents-admin/DocumentStorageContractTests.cs`
- [ ] T063 [P] [US1] Create integration tests for document upload metadata, object storage stub use, tenant isolation, feature flags, source-reference validation, restricted-detail minimization, legal hold, retention, export scope, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Storage/DocumentStorageIntegrationTests.cs`
- [ ] T064 [P] [US1] Create web journey tests for school administrator and document manager document list, upload, detail, version history, archive, restore, legal hold, and export flows in `apps/admin-web/tests/documents-admin/documentStorage.spec.ts`
- [ ] T065 [P] [US1] Create guardian and student document visibility journey tests for permitted documents, hidden restricted metadata, denied downloads, and relationship changes in `apps/admin-web/tests/documents-admin/documentAudienceVisibility.spec.ts`
- [ ] T066 [P] [US1] If `apps/mobile/` exists or mobile document surfaces are enabled, create mobile journey tests for document list, detail, and download denial in `apps/mobile/test/features/documents/document_storage_flow_test.dart`

### Implementation for User Story 1

- [ ] T067 [P] [US1] Create document create, update, query, response, version, archive, restore, legal hold, category, and export DTOs matching document-storage.md in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentStorageDtos.cs`
- [ ] T068 [US1] Implement document category policy and activation service with required metadata, subject types, visibility, retention, legal hold, export, review, status, and version validation in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentCategoryPolicyService.cs`
- [ ] T069 [US1] Implement document metadata validator for tenant scope, category, title, subject context, source reference, visibility, retention, legal hold, export, feature flags, and idempotency in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentMetadataValidator.cs`
- [ ] T070 [US1] Implement document object storage service using object metadata, content hash validation, validation state, quarantine state, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentObjectStorageService.cs`
- [ ] T071 [US1] Implement document service for create, detail, list, metadata update, status transitions, source-reference preservation, active version selection, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentService.cs`
- [ ] T072 [US1] Implement document version service for upload intent, replacement version, activation, validation failure, archive evidence, restore evidence, and original version preservation in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentVersionService.cs`
- [ ] T073 [US1] Implement document access decision service for view, download, update, archive, restore, hold, export, and search-open decisions with restricted-detail minimization in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentAccessDecisionService.cs`
- [ ] T074 [US1] Implement document retention and legal hold service for retention protection, hold placement, hold release, deletion prevention, and reviewer evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Retention/DocumentRetentionLegalHoldService.cs`
- [ ] T075 [US1] Implement document controlled export service with scope validation, included fields, reason capture, retention, object artifact reference, audit, and lifecycle status in `apps/api/src/SafeSchool.Api/Features/Documents/Exports/DocumentControlledExportService.cs`
- [ ] T076 [US1] Implement document query service with filters for category, subject, source module, owner, status, visibility, retention state, legal hold state, date range, and actor scope in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentQueryService.cs`
- [ ] T077 [US1] Implement school document, version, archive, restore, legal hold, download, and export endpoints in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/SchoolDocumentController.cs`
- [ ] T078 [US1] Implement document category endpoints for list, draft create, update, activate, archive, and version history in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentCategoryController.cs`
- [ ] T079 [US1] Implement guardian document list, detail, and download endpoints with approved guardian-link visibility in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/GuardianDocumentController.cs`
- [ ] T080 [US1] Implement student document list, detail, and download endpoints where student document visibility is enabled in `apps/api/src/SafeSchool.Api/Features/Documents/Storage/StudentDocumentController.cs`
- [ ] T081 [US1] Add document storage, category, version, legal hold, and export routes to Documents module registration in `apps/api/src/SafeSchool.Api/Features/Documents/DocumentsModule.cs`
- [ ] T082 [US1] Implement typed web API methods for document list, upload, detail, update, versions, archive, restore, legal hold, download, category, and export operations in `apps/admin-web/src/features/documents/api/documentStorageApi.ts`
- [ ] T083 [US1] Implement document list and filter component with category, subject, status, visibility, retention, legal hold, date range, and denied state display in `apps/admin-web/src/features/documents/components/DocumentList.tsx`
- [ ] T084 [US1] Implement document upload and metadata form with category, title, subject, source reference, visibility, retention, legal hold, object metadata, and validation errors in `apps/admin-web/src/features/documents/components/DocumentUploadForm.tsx`
- [ ] T085 [US1] Implement document detail, version history, access decision, archive, restore, legal hold, and export controls in `apps/admin-web/src/features/documents/components/DocumentDetailPanel.tsx`
- [ ] T086 [US1] Implement school document pages for list, upload, detail, categories, and category detail in `apps/admin-web/src/app/(school)/documents/page.tsx`, `apps/admin-web/src/app/(school)/documents/upload/page.tsx`, `apps/admin-web/src/app/(school)/documents/[documentId]/page.tsx`, `apps/admin-web/src/app/(school)/documents/categories/page.tsx`, and `apps/admin-web/src/app/(school)/documents/categories/[categoryId]/page.tsx`
- [ ] T087 [US1] Implement guardian document pages for list and detail in `apps/admin-web/src/app/(guardian)/documents/page.tsx` and `apps/admin-web/src/app/(guardian)/documents/[documentId]/page.tsx`
- [ ] T088 [US1] Implement student document pages for list and detail in `apps/admin-web/src/app/(student)/documents/page.tsx` and `apps/admin-web/src/app/(student)/documents/[documentId]/page.tsx`
- [ ] T089 [US1] If `apps/mobile/` exists or mobile document surfaces are enabled, implement mobile document list and detail screen in `apps/mobile/lib/features/documents/document_storage_screen.dart`

**Checkpoint**: US1 is complete when document upload, classification, versioning, access decisions, archive, restore, retention, legal hold, export, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 4: User Story 2 - Issue and Verify Certificates (Priority: P1)

**Goal**: Authorized certificate issuers can configure certificate types, issue, verify, correct, revoke, expire, supersede, reissue, and export certificates without mutating source records.

**Independent Test**: Issue a certificate for an eligible student, verify it as an authorized recipient, correct it, revoke it, and confirm each state preserves issuer, reason, time, verification result, and audit evidence.

### Tests for User Story 2

- [ ] T090 [P] [US2] Create unit tests for certificate type activation, issuer roles, subject rules, required fields, source evidence rules, duplicate active policy, validity behavior, and revocation policy in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Certificates/CertificateTypePolicyTests.cs`
- [ ] T091 [P] [US2] Create unit tests for certificate issuance, correction, revocation, expiration, supersession, reissue, verification state, recipient impact, and source evidence preservation in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Certificates/CertificateLifecycleServiceTests.cs`
- [ ] T092 [P] [US2] Create unit tests for certificate source evidence visibility, stale evidence rejection, cross-school subject rejection, conflicted issuer rejection, and restricted-detail minimization in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Certificates/CertificateSourceEvidenceTests.cs`
- [ ] T093 [P] [US2] Create contract tests for certificate-management.md type, issuance, correction, revocation, reissue, verification, guardian, student, and export endpoints in `tests/contracts/documents-admin/CertificateManagementContractTests.cs`
- [ ] T094 [P] [US2] Create integration tests for certificate issuance, tenant isolation, feature flags, source-reference validation, duplicate active certificate handling, correction, revocation, verification, export scope, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Certificates/CertificateManagementIntegrationTests.cs`
- [ ] T095 [P] [US2] Create web journey tests for certificate issuer type configuration, issuance, detail, correction, revocation, reissue, verification, and export flows in `apps/admin-web/tests/documents-admin/certificateManagement.spec.ts`
- [ ] T096 [P] [US2] Create guardian and student certificate journey tests for visible certificates, denied restricted details, verification status, and relationship changes in `apps/admin-web/tests/documents-admin/certificateAudienceVisibility.spec.ts`
- [ ] T097 [P] [US2] If `apps/mobile/` exists or mobile certificate surfaces are enabled, create mobile journey tests for certificate list, detail, and verification state in `apps/mobile/test/features/documents/certificate_management_flow_test.dart`

### Implementation for User Story 2

- [ ] T098 [P] [US2] Create certificate type, issue, correction, revocation, reissue, verification, export, query, and response DTOs matching certificate-management.md in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateManagementDtos.cs`
- [ ] T099 [US2] Implement certificate type policy and activation service with issuer roles, subject rules, required fields, source evidence rules, validity, visibility, duplicate active policy, revocation policy, and version validation in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateTypePolicyService.cs`
- [ ] T100 [US2] Implement certificate source evidence validator for issuer authority, subject eligibility, source visibility, stale evidence, cross-school subjects, conflicted issuers, and restricted-detail policy in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateSourceEvidenceValidator.cs`
- [ ] T101 [US2] Implement certificate issuance service with type validation, duplicate active handling, source summary preservation, document reference creation where applicable, idempotency, audit, and lifecycle evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateIssuanceService.cs`
- [ ] T102 [US2] Implement certificate lifecycle service for correction, revocation, expiration, supersession, reissue, reason capture, original evidence preservation, recipient impact, verification impact, audit, and lifecycle evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateLifecycleService.cs`
- [ ] T103 [US2] Implement certificate verification service with actor visibility checks, visible status, denial reason, verification evidence, and audit capture in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateVerificationService.cs`
- [ ] T104 [US2] Implement certificate query service with filters for type, subject, recipient, status, verification state, issuer, source module, date range, and actor scope in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateQueryService.cs`
- [ ] T105 [US2] Implement certificate controlled export service with export scope validation, included fields, reason capture, retention, audit, and lifecycle status in `apps/api/src/SafeSchool.Api/Features/Documents/Exports/CertificateControlledExportService.cs`
- [ ] T106 [US2] Implement school certificate, correction, revocation, reissue, verification, and export endpoints in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/SchoolCertificateController.cs`
- [ ] T107 [US2] Implement certificate type endpoints for list, draft create, update, activate, archive, and version history in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateTypeController.cs`
- [ ] T108 [US2] Implement guardian certificate list, detail, and verification endpoints in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/GuardianCertificateController.cs`
- [ ] T109 [US2] Implement student certificate list, detail, and verification endpoints where student certificate visibility is enabled in `apps/api/src/SafeSchool.Api/Features/Documents/Certificates/StudentCertificateController.cs`
- [ ] T110 [US2] Add certificate management and certificate type routes to Documents module registration in `apps/api/src/SafeSchool.Api/Features/Documents/DocumentsModule.cs`
- [ ] T111 [US2] Implement typed web API methods for certificate types, certificates, issuance, correction, revocation, reissue, verification, and export operations in `apps/admin-web/src/features/documents/api/certificateManagementApi.ts`
- [ ] T112 [US2] Implement certificate list and filters component with type, subject, recipient, status, verification state, issuer, source module, and date range in `apps/admin-web/src/features/documents/components/CertificateList.tsx`
- [ ] T113 [US2] Implement certificate issue and type configuration forms with source evidence, issuer role, subject, validity, language, visibility, duplicate policy, and activation errors in `apps/admin-web/src/features/documents/components/CertificateForms.tsx`
- [ ] T114 [US2] Implement certificate detail panel with issuance evidence, verification state, correction, revocation, reissue, export, and audit summary controls in `apps/admin-web/src/features/documents/components/CertificateDetailPanel.tsx`
- [ ] T115 [US2] Implement school certificate pages for list, issue, detail, types, and type detail in `apps/admin-web/src/app/(school)/certificates/page.tsx`, `apps/admin-web/src/app/(school)/certificates/issue/page.tsx`, `apps/admin-web/src/app/(school)/certificates/[certificateId]/page.tsx`, `apps/admin-web/src/app/(school)/certificates/types/page.tsx`, and `apps/admin-web/src/app/(school)/certificates/types/[typeId]/page.tsx`
- [ ] T116 [US2] Implement guardian certificate pages for list and detail in `apps/admin-web/src/app/(guardian)/certificates/page.tsx` and `apps/admin-web/src/app/(guardian)/certificates/[certificateId]/page.tsx`
- [ ] T117 [US2] Implement student certificate pages for list and detail in `apps/admin-web/src/app/(student)/certificates/page.tsx` and `apps/admin-web/src/app/(student)/certificates/[certificateId]/page.tsx`
- [ ] T118 [US2] If `apps/mobile/` exists or mobile certificate surfaces are enabled, implement mobile certificate list and detail screen in `apps/mobile/lib/features/documents/certificate_management_screen.dart`

**Checkpoint**: US2 is complete when certificate type configuration, issuance, verification, correction, revocation, reissue, tenant isolation, feature gating, source-boundary preservation, audit, and no-side-effect assertions pass independently.

---

## Phase 5: User Story 3 - Search Authorized Records (Priority: P1)

**Goal**: Authorized users can search permitted records across documents, certificates, prior modules, audit, configuration, alerts, and incidents without leaking restricted results.

**Independent Test**: Search for a student-related term as a school administrator, guardian, student, and unrelated staff member, then verify each actor receives only permitted results and denied or restricted results are not leaked through titles, snippets, counts, filters, or previews.

### Tests for User Story 3

- [ ] T119 [P] [US3] Create unit tests for search indexing eligibility, searchable audience rules, source module boundaries, feature configuration, and source reference preservation in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchIndexEligibilityTests.cs`
- [ ] T120 [P] [US3] Create unit tests for search result access decisions across tenant, guardian link, student ownership, staff assignment, source module, restricted-detail policy, reviewer scope, and platform authority in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchResultAccessDecisionTests.cs`
- [ ] T121 [P] [US3] Create unit tests for search freshness, stale index handling, failed index handling, suppressed entries, reindex eligibility, and review-required states in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchFreshnessReindexTests.cs`
- [ ] T122 [P] [US3] Create unit tests for broad query privacy risk, hidden count suppression, snippet minimization, facet minimization, preview minimization, and suspicious query exceptions in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchPrivacyTests.cs`
- [ ] T123 [P] [US3] Create contract tests for search.md query, result open, index-state, reindex, export, guardian, and student endpoints in `tests/contracts/documents-admin/SearchContractTests.cs`
- [ ] T124 [P] [US3] Create integration tests for scoped search, result-open revalidation, stale index visibility, reindex requests, denied result attempts, restricted metadata minimization, export scope, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchIntegrationTests.cs`
- [ ] T125 [P] [US3] Create web journey tests for school administrator search, filters, result open, stale state, denied results, reindex request, and search export flows in `apps/admin-web/tests/documents-admin/search.spec.ts`
- [ ] T126 [P] [US3] Create guardian and student search journey tests for approved scope, self-scope, hidden restricted records, minimized metadata, and denied result opens in `apps/admin-web/tests/documents-admin/searchAudienceVisibility.spec.ts`

### Implementation for User Story 3

- [ ] T127 [P] [US3] Create search query, search response, result open, index state, reindex, export, access decision, and freshness DTOs matching search.md in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchDtos.cs`
- [ ] T128 [US3] Implement search indexing eligibility service for source modules, searchable audience rules, tenant scope, feature availability, restricted-detail policy, source record status, and suppression reasons in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchIndexEligibilityService.cs`
- [ ] T129 [US3] Implement search index writer service for pending, indexed, stale, failed, suppressed, reindex-required, and review-required states with source references and audit evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchIndexWriterService.cs`
- [ ] T130 [US3] Implement search query parser and filter validator for text, record type, source module, student context, category, status, owner, date range, visibility, certificate type, document category, audit action, alert state, and review state in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchQueryFilterService.cs`
- [ ] T131 [US3] Implement search query service that returns only permitted result fields, visible counts, freshness state, pagination, query log, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchQueryService.cs`
- [ ] T132 [US3] Implement search result access decision service that revalidates current access on result listing and result open while suppressing restricted metadata leaks in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchResultAccessDecisionService.cs`
- [ ] T133 [US3] Implement search freshness and reindex service for stale, failed, suppressed, pending, and review-required entries with reviewer reasons and idempotency in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchFreshnessReindexService.cs`
- [ ] T134 [US3] Implement search controlled export service with query log validation, permitted fields, export reason, retention, audit, and export status in `apps/api/src/SafeSchool.Api/Features/Documents/Exports/SearchControlledExportService.cs`
- [ ] T135 [US3] Implement school search, result open, index-state, reindex, and export endpoints in `apps/api/src/SafeSchool.Api/Features/Documents/Search/SchoolSearchController.cs`
- [ ] T136 [US3] Implement guardian search and result open endpoints with approved guardian-link scope in `apps/api/src/SafeSchool.Api/Features/Documents/Search/GuardianSearchController.cs`
- [ ] T137 [US3] Implement student search and result open endpoints where student search is enabled in `apps/api/src/SafeSchool.Api/Features/Documents/Search/StudentSearchController.cs`
- [ ] T138 [US3] Add search, result-open, index-state, reindex, and search export routes to Documents module registration in `apps/api/src/SafeSchool.Api/Features/Documents/DocumentsModule.cs`
- [ ] T139 [US3] Implement typed web API methods for search query, result open, index state, reindex, and search export operations in `apps/admin-web/src/features/documents/api/searchApi.ts`
- [ ] T140 [US3] Implement global search form and filters with record type, source module, student, category, status, owner, date range, visibility, certificate type, document category, audit action, alert state, and review state in `apps/admin-web/src/features/documents/components/GlobalSearchForm.tsx`
- [ ] T141 [US3] Implement search results component with freshness state, minimized fields, denied state, action hints, pagination, and result-open revalidation handling in `apps/admin-web/src/features/documents/components/SearchResults.tsx`
- [ ] T142 [US3] Implement search index health component with pending, indexed, stale, failed, suppressed, reindex-required, and review-required states in `apps/admin-web/src/features/documents/components/SearchIndexHealthPanel.tsx`
- [ ] T143 [US3] Implement school search pages for query, result detail, and index health in `apps/admin-web/src/app/(school)/search/page.tsx`, `apps/admin-web/src/app/(school)/search/results/[entryId]/page.tsx`, and `apps/admin-web/src/app/(school)/search/index-health/page.tsx`
- [ ] T144 [US3] If guardian search is enabled, implement guardian search page in `apps/admin-web/src/app/(guardian)/documents/search/page.tsx`
- [ ] T145 [US3] If student search is enabled, implement student search page in `apps/admin-web/src/app/(student)/documents/search/page.tsx`

**Checkpoint**: US3 is complete when scoped search, index freshness, result-open revalidation, restricted metadata minimization, reindex, search export, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 6: User Story 4 - Operate the Admin Dashboard and Tenant Configuration (Priority: P1)

**Goal**: Authorized administrators can review operational summaries and safely manage tenant feature settings with dependency validation and versioned audit evidence.

**Independent Test**: Open the admin dashboard for a school account, review enabled capabilities and operational summaries, change a tenant feature setting, reject an invalid dependency change, and verify the dashboard and audit trail reflect the outcome.

### Tests for User Story 4

- [ ] T146 [P] [US4] Create unit tests for admin dashboard summary scoping, restricted count minimization, module status counts, exception counts, pending review counts, search health, audit activity, alerts, incidents, and platform-scope denial in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Dashboard/AdminDashboardSummaryTests.cs`
- [ ] T147 [P] [US4] Create unit tests for tenant feature dependency validation, mandatory safety protections, active record conflicts, search visibility implications, audit obligations, monitoring implications, and partial-change rejection in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Configuration/TenantFeatureDependencyValidationTests.cs`
- [ ] T148 [P] [US4] Create unit tests for configuration change versioning, effective windows, approval state, rejection state, prior and new value preservation, reason capture, and audit evidence in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Configuration/FeatureConfigurationChangeTests.cs`
- [ ] T149 [P] [US4] Create contract tests for admin-dashboard-configuration.md dashboard, feature setting, history, approve, reject, and platform dashboard endpoints in `tests/contracts/documents-admin/AdminDashboardConfigurationContractTests.cs`
- [ ] T150 [P] [US4] Create integration tests for dashboard reads, tenant configuration changes, dependency rejection, approval workflow, tenant isolation, feature flags, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/DashboardConfigurationIntegrationTests.cs`
- [ ] T151 [P] [US4] Create web journey tests for school admin dashboard summaries, feature settings, dependency errors, approvals, rejected changes, version history, and platform dashboard denial in `apps/admin-web/tests/documents-admin/adminDashboardConfiguration.spec.ts`

### Implementation for User Story 4

- [ ] T152 [P] [US4] Create dashboard query, dashboard response, feature setting, configuration change, approval, rejection, history, and platform summary DTOs matching admin-dashboard-configuration.md in `apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/AdminDashboardConfigurationDtos.cs`
- [ ] T153 [US4] Implement admin dashboard summary service for enabled modules, usage, open exceptions, pending reviews, feature state, document state, certificate state, search health, audit activity, alerts, incidents, and operational health with permission-scoped counts in `apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/AdminDashboardSummaryService.cs`
- [ ] T154 [US4] Implement dashboard visibility service that minimizes restricted summaries, cross-school counts, hidden search results, audit details, and platform operations details based on actor authority in `apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/AdminDashboardVisibilityService.cs`
- [ ] T155 [US4] Implement tenant feature dependency validation service for Phase 10/11 capabilities, required dependencies, mandatory safety protections, active records, retention, audit, search visibility, and monitoring implications in `apps/api/src/SafeSchool.Api/Features/Administration/Configuration/TenantFeatureDependencyValidator.cs`
- [ ] T156 [US4] Implement tenant feature setting service for read, update, schedule, suspend, inspect, version preservation, effective windows, reason capture, idempotency, audit, and lifecycle evidence in `apps/api/src/SafeSchool.Api/Features/Administration/Configuration/TenantFeatureSettingService.cs`
- [ ] T157 [US4] Implement feature configuration approval service for pending, approved, rejected, review-required, dependency decision, no partial application, and version history behavior in `apps/api/src/SafeSchool.Api/Features/Administration/Configuration/FeatureConfigurationApprovalService.cs`
- [ ] T158 [US4] Implement configuration history query service with filters for feature key, actor, state, dependency decision, approval state, effective window, date range, and actor scope in `apps/api/src/SafeSchool.Api/Features/Administration/Configuration/FeatureConfigurationHistoryService.cs`
- [ ] T159 [US4] Implement school admin dashboard endpoint in `apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/SchoolAdminDashboardController.cs`
- [ ] T160 [US4] Implement tenant feature setting, update, history, approve, and reject endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/Configuration/TenantFeatureConfigurationController.cs`
- [ ] T161 [US4] Implement platform dashboard endpoint with explicit platform authority and cross-school minimization in `apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/PlatformDashboardController.cs`
- [ ] T162 [US4] Add dashboard and tenant configuration routes to Administration module registration in `apps/api/src/SafeSchool.Api/Features/Administration/AdministrationModule.cs`
- [ ] T163 [US4] Implement typed web API methods for dashboard summaries, feature settings, configuration updates, approvals, rejections, history, and platform dashboard reads in `apps/admin-web/src/features/administration/api/adminDashboardConfigurationApi.ts`
- [ ] T164 [US4] Implement admin dashboard summary component with module status, usage, exceptions, pending reviews, document state, certificate state, search health, audit activity, alerts, incidents, and operational health in `apps/admin-web/src/features/administration/components/AdminDashboardSummary.tsx`
- [ ] T165 [US4] Implement tenant feature settings editor with dependency validation results, effective windows, reason capture, approval state, rejection state, and version history in `apps/admin-web/src/features/administration/components/TenantFeatureSettingsEditor.tsx`
- [ ] T166 [US4] Implement configuration change history table with feature key, actor, prior value, new value, dependency decision, approval state, effective window, and audit indicators in `apps/admin-web/src/features/administration/components/ConfigurationChangeHistory.tsx`
- [ ] T167 [US4] Implement school admin dashboard, feature settings, and configuration history pages in `apps/admin-web/src/app/(school)/admin/page.tsx`, `apps/admin-web/src/app/(school)/admin/features/page.tsx`, and `apps/admin-web/src/app/(school)/admin/features/history/page.tsx`

**Checkpoint**: US4 is complete when admin dashboard summaries, tenant feature configuration, dependency validation, approval or rejection, version preservation, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 7: User Story 5 - Review Audit Trails and Exports (Priority: P1)

**Goal**: Authorized auditors and reviewers can search, inspect, minimize, and export audit evidence while preserving append-only audit behavior.

**Independent Test**: Perform sensitive actions across documents, certificates, search, tenant configuration, and another prior module; then verify an authorized auditor can filter, inspect, and export the permitted audit trail while unauthorized users cannot read or export it.

### Tests for User Story 5

- [ ] T168 [P] [US5] Create unit tests for audit event filtering by actor, role, student, subject, source module, action, target, result, denial reason, risk, correlation, date range, and actor scope in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AuditTrail/AuditEventQueryTests.cs`
- [ ] T169 [P] [US5] Create unit tests for restricted audit payload minimization, reviewer-only payload denial, platform-only payload denial, denied attempt visibility, and field-level export minimization in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AuditTrail/AuditPayloadVisibilityTests.cs`
- [ ] T170 [P] [US5] Create unit tests for audit export scope validation, included fields, reason capture, export retention, append-only correction evidence, redaction evidence, and suspicious export exception creation in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AuditTrail/AuditExportServiceTests.cs`
- [ ] T171 [P] [US5] Create contract tests for audit-trail.md audit event search, event detail, export request, export list, export status, and platform audit endpoints in `tests/contracts/documents-admin/AuditTrailContractTests.cs`
- [ ] T172 [P] [US5] Create integration tests for audit event search, restricted payload minimization, export creation, export status, platform authority, tenant isolation, feature flags, export audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AuditTrail/AuditTrailIntegrationTests.cs`
- [ ] T173 [P] [US5] Create web journey tests for auditor audit search, event detail, payload minimization, export request, export status, denied export, and platform audit denial in `apps/admin-web/tests/documents-admin/auditTrail.spec.ts`

### Implementation for User Story 5

- [ ] T174 [P] [US5] Create audit query, audit event response, audit export request, audit export response, payload visibility, append-only correction, and platform audit DTOs matching audit-trail.md in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditTrailDtos.cs`
- [ ] T175 [US5] Implement audit event query service with filters for school account, actor, role, student, subject, source module, action, target record, result, denial reason, risk, correlation reference, date range, and actor scope in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditEventQueryService.cs`
- [ ] T176 [US5] Implement audit payload visibility service for full, minimized, restricted, reviewer-only, platform-operator, denied attempt, and export field visibility decisions in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditPayloadVisibilityService.cs`
- [ ] T177 [US5] Implement audit export service with scope validation, included fields, reason capture, retention policy, export artifact reference, status transitions, export audit event creation, and suspicious export exception creation in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditExportService.cs`
- [ ] T178 [US5] Implement append-only audit correction and redaction evidence service that records correction or privacy masking evidence without erasing original events in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditAppendOnlyCorrectionService.cs`
- [ ] T179 [US5] Implement school audit event search and detail endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/SchoolAuditTrailController.cs`
- [ ] T180 [US5] Implement audit export request, list, and status endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditExportController.cs`
- [ ] T181 [US5] Implement platform audit event search endpoint with explicit platform authority and cross-school minimization in `apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/PlatformAuditTrailController.cs`
- [ ] T182 [US5] Add audit trail and audit export routes to Administration module registration in `apps/api/src/SafeSchool.Api/Features/Administration/AdministrationModule.cs`
- [ ] T183 [US5] Implement typed web API methods for audit event search, event detail, export request, export list, export status, and platform audit reads in `apps/admin-web/src/features/administration/api/auditTrailApi.ts`
- [ ] T184 [US5] Implement audit search table with actor, role, source module, action, target, result, risk, correlation, date range, minimized payload, and denied state indicators in `apps/admin-web/src/features/administration/components/AuditEventTable.tsx`
- [ ] T185 [US5] Implement audit event detail and payload minimization panel with restricted payload labels, permitted fields, audit chain, and denied export reasons in `apps/admin-web/src/features/administration/components/AuditEventDetailPanel.tsx`
- [ ] T186 [US5] Implement audit export request and status component with filters, included fields, reason capture, retention, status, and generated artifact state in `apps/admin-web/src/features/administration/components/AuditExportPanel.tsx`
- [ ] T187 [US5] Implement school audit event and audit export pages in `apps/admin-web/src/app/(school)/admin/audit/page.tsx`, `apps/admin-web/src/app/(school)/admin/audit/[auditEventId]/page.tsx`, and `apps/admin-web/src/app/(school)/admin/audit/exports/page.tsx`

**Checkpoint**: US5 is complete when audit search, event detail, payload minimization, controlled exports, append-only correction evidence, platform authority, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 8: User Story 6 - Monitor Metrics, Alerts, and Incidents (Priority: P2)

**Goal**: Authorized operators can review metrics, configure alert rules, handle alerts, manage incidents, and resolve operational exceptions with audit evidence.

**Independent Test**: Configure a monitoring threshold for a school-visible workflow, trigger an alert condition, acknowledge the alert, link it to an incident, resolve the incident, and verify visibility, audit evidence, and dashboard summaries update within the authorized scope.

### Tests for User Story 6

- [ ] T188 [P] [US6] Create unit tests for metric observation validation, data-quality states, missing metrics, delayed metrics, duplicate metrics, noisy metrics, inconsistent metrics, and review-required behavior in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/MetricObservationServiceTests.cs`
- [ ] T189 [P] [US6] Create unit tests for alert rule activation, threshold evaluation, owner queue validation, suppression policy, notification eligibility, review requirement, and versioning in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/AlertRuleEvaluationTests.cs`
- [ ] T190 [P] [US6] Create unit tests for alert lifecycle transitions, acknowledgement, assignment, suppression, resolution, reopening, reason requirements, and audit evidence in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/AlertLifecycleServiceTests.cs`
- [ ] T191 [P] [US6] Create unit tests for incident lifecycle transitions, severity changes, linked alerts, linked evidence, owner changes, resolution summaries, platform-scope visibility, and audit evidence in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/IncidentLifecycleServiceTests.cs`
- [ ] T192 [P] [US6] Create unit tests for operational exception detection and review across document upload failure, certificate mismatch, stale search index, audit gap, suspicious export, invalid configuration, missing metric, noisy alert, retention conflict, and manual review cases in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/OperationalExceptionServiceTests.cs`
- [ ] T193 [P] [US6] Create contract tests for metrics-monitoring.md metric, alert rule, alert, incident, operational exception, and platform monitoring endpoints in `tests/contracts/documents-admin/MetricsMonitoringContractTests.cs`
- [ ] T194 [P] [US6] Create integration tests for metric observations, alert rule activation, threshold breach, alert acknowledgement, incident creation, incident resolution, exception review, dashboard integration, tenant isolation, feature flags, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/MetricsMonitoringIntegrationTests.cs`
- [ ] T195 [P] [US6] Create web journey tests for operations reviewer metrics, alert rule configuration, alert acknowledgement, incident update, exception review, platform monitoring denial, and dashboard updates in `apps/admin-web/tests/documents-admin/metricsMonitoring.spec.ts`

### Implementation for User Story 6

- [ ] T196 [P] [US6] Create metric query, alert rule, alert action, incident create, incident update, operational exception review, monitoring response, and platform monitoring DTOs matching metrics-monitoring.md in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/MetricsMonitoringDtos.cs`
- [ ] T197 [US6] Implement metric observation service for ingestion, query, scope validation, data-quality state, missing metric, delayed metric, duplicate metric, noisy metric, inconsistent metric, review-required evidence, and audit capture in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/MetricObservationService.cs`
- [ ] T198 [US6] Implement alert rule configuration service for draft, activation, metric source validation, threshold condition validation, severity, owner queue, suppression behavior, notification eligibility, review requirement, and versioning in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/AlertRuleConfigurationService.cs`
- [ ] T199 [US6] Implement alert evaluation service for metric threshold breaches, suppression, duplicate alert handling, owner routing, severity assignment, dashboard summary update hooks, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/AlertEvaluationService.cs`
- [ ] T200 [US6] Implement alert lifecycle service for acknowledgement, assignment, severity change, suppression, resolution, reopening, reason capture, linked evidence, and audit history in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/AlertLifecycleService.cs`
- [ ] T201 [US6] Implement incident lifecycle service for create, assign, update severity, investigate, mitigate, resolve, reopen, close, link alerts, link records, preserve resolution summary, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Administration/Incidents/IncidentLifecycleService.cs`
- [ ] T202 [US6] Implement operational exception service for detection, query, resolve, dismiss, escalate, reviewer assignment, source evidence preservation, resolution reason, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/OperationalExceptionService.cs`
- [ ] T203 [US6] Implement metric observation and alert rule endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/MetricAlertRuleController.cs`
- [ ] T204 [US6] Implement alert search, acknowledge, assign, resolve, suppress, and reopen endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/AlertController.cs`
- [ ] T205 [US6] Implement incident search, create, update, resolve, reopen, close, and linked evidence endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/Incidents/IncidentController.cs`
- [ ] T206 [US6] Implement operational exception search and review endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/OperationalExceptionController.cs`
- [ ] T207 [US6] Implement platform alert and incident endpoints with explicit platform authority and cross-school minimization in `apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/PlatformMonitoringController.cs`
- [ ] T208 [US6] Add metrics, alert rule, alert, incident, and operational exception routes to Administration module registration in `apps/api/src/SafeSchool.Api/Features/Administration/AdministrationModule.cs`
- [ ] T209 [US6] Implement typed web API methods for metrics, alert rules, alerts, incidents, operational exceptions, and platform monitoring reads in `apps/admin-web/src/features/administration/api/metricsMonitoringApi.ts`
- [ ] T210 [US6] Implement metrics dashboard component with metric filters, data-quality state, threshold context, source module, scope, and date range in `apps/admin-web/src/features/administration/components/MetricsDashboard.tsx`
- [ ] T211 [US6] Implement alert rule editor and alert list components with threshold, severity, owner, suppression, acknowledgement, assignment, resolution, and reopening controls in `apps/admin-web/src/features/administration/components/AlertManagement.tsx`
- [ ] T212 [US6] Implement incident board component with status, severity, owner, linked alerts, linked records, timeline, resolution summary, reopen, and close controls in `apps/admin-web/src/features/administration/components/IncidentBoard.tsx`
- [ ] T213 [US6] Implement operational exception review component with exception type, severity, source evidence, reviewer assignment, resolve, dismiss, escalate, and audit indicators in `apps/admin-web/src/features/administration/components/OperationalExceptionReview.tsx`
- [ ] T214 [US6] Implement school monitoring pages for metrics, alert rules, alerts, incidents, and exceptions in `apps/admin-web/src/app/(school)/admin/monitoring/page.tsx`, `apps/admin-web/src/app/(school)/admin/monitoring/alert-rules/page.tsx`, `apps/admin-web/src/app/(school)/admin/monitoring/alerts/page.tsx`, `apps/admin-web/src/app/(school)/admin/monitoring/incidents/page.tsx`, and `apps/admin-web/src/app/(school)/admin/monitoring/exceptions/page.tsx`

**Checkpoint**: US6 is complete when metric observations, alert rules, alert lifecycle, incident lifecycle, operational exceptions, platform monitoring scope, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Tighten quality, performance, security, documentation, observability, privacy, and validation across completed user stories.

- [ ] T215 [P] Add OpenAPI or route documentation for all Phase 10/11 `/api/v1/` endpoints in `apps/api/src/SafeSchool.Api/Features/Administration/AdminDocumentsOpenApi.cs`
- [ ] T216 [P] Add a web and mobile API client contract snapshot for Documents and Administration features in `apps/admin-web/src/features/administration/api/adminDocuments.contract.ts`
- [ ] T217 [P] Add backend performance tests for document upload under 2 minutes, document or certificate search under 30 seconds, indexing freshness within 5 minutes, certificate issue under 2 minutes, scoped search under 10 seconds, dashboard summary under 30 seconds, audit trace under 60 seconds, and alert owner visibility within 5 minutes in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentPerformanceTests.cs`
- [ ] T218 [P] Add security regression tests for restricted documents, medical context, complaint context, finance context, staff-only details, safety notes, student-welfare details, hidden search result counts, audit payload minimization, cross-school data, guardian link restrictions, student self-scope, platform operator scope, export scope, retention, and legal hold in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentSecurityRegressionTests.cs`
- [ ] T219 Add audit coverage test that verifies every audit event required by spec.md FR-026 is emitted by at least one Phase 10/11 workflow in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentAuditCoverageTests.cs`
- [ ] T220 Add no-side-effect regression test proving Phase 10/11 creates no attendance, campus gate, scan, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, communication delivery, or source-domain mutation outcomes in `apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AdminDocumentNoSideEffectTests.cs`
- [ ] T221 [P] Add end-to-end smoke tests for document storage, certificate management, search, admin dashboard, configuration, audit trail, metrics, alerts, incidents, and operational exceptions in `tests/e2e/documents-admin/documents-admin.e2e.spec.ts`
- [ ] T222 [P] Add user-facing accessibility checks for document lists, document forms, certificate forms, search results, dashboard summaries, feature settings, audit tables, export forms, metrics dashboards, alert lists, incident boards, and exception reviews in `apps/admin-web/tests/documents-admin/documentsAdminAccessibility.spec.ts`
- [ ] T223 [P] If `apps/mobile/` exists or mobile document surfaces are enabled, add mobile smoke tests for document, certificate, and search screens in `apps/mobile/test/features/documents/documents_smoke_test.dart`
- [ ] T224 Update Phase 10/11 quickstart validation results and implementation notes in `specs/011-documents-search-admin-observability/quickstart.md`
- [ ] T225 Update the Documents module README with commands, role matrix, capability matrix, route map, document states, certificate states, search freshness states, retention states, legal hold behavior, export behavior, and no-side-effect boundaries in `apps/api/src/SafeSchool.Api/Features/Documents/README.md`
- [ ] T226 Update the Administration module README with commands, role matrix, capability matrix, route map, feature setting states, audit payload visibility states, metric quality states, alert states, incident states, and operational exception boundaries in `apps/api/src/SafeSchool.Api/Features/Administration/README.md`
- [ ] T227 Add a privacy, restricted-detail, export, retention, and legal-hold implementation checklist for reviewers in `apps/api/src/SafeSchool.Api/Features/Administration/PrivacyRetentionReview.md`
- [ ] T228 Add operational metric names and alert guidance for documents, certificates, search freshness, audit flow, configuration changes, access denials, exports, retention, legal holds, alerts, incidents, and operational exceptions in `apps/api/src/SafeSchool.Api/Features/Administration/AdminDocumentMetrics.md`
- [ ] T229 Run all backend, contract, web, enabled mobile, and e2e Phase 10/11 tests and record the command set in `specs/011-documents-search-admin-observability/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1; blocks all user story work.
- **Phase 3 US1**: Depends on Phase 2; MVP and first independently useful increment.
- **Phase 4 US2**: Depends on Phase 2; can run after or in parallel with US1, but certificate document rendering may reuse US1 document storage when enabled.
- **Phase 5 US3**: Depends on Phase 2; can run after or in parallel with US1 and US2 using seeded documents, certificates, and source records.
- **Phase 6 US4**: Depends on Phase 2; can run with seeded module summaries but final validation should rerun after US1 through US3.
- **Phase 7 US5**: Depends on Phase 2; can run with seeded audit events but final validation should rerun after each implemented workflow emits audit evidence.
- **Phase 8 US6**: Depends on Phase 2; can run with seeded metrics, alerts, incidents, and exceptions, but dashboard validation improves after US4.
- **Phase 9 Polish**: Depends on all desired user stories being complete.

### User Story Dependency Graph

```text
Setup -> Foundation -> US1 -> MVP validation
Setup -> Foundation -> US2 -> certificate validation
Setup -> Foundation -> US3 -> search validation
Setup -> Foundation -> US4 -> dashboard/config validation
Setup -> Foundation -> US5 -> audit/export validation
Setup -> Foundation -> US6 -> monitoring validation

Recommended delivery order for one implementer:
US1 -> US2 -> US3 -> US4 -> US5 -> US6 -> Polish
```

### Within Each User Story

- Write the listed tests first and confirm they fail before implementation.
- Implement DTOs before validators and services.
- Implement validators and guards before endpoint exposure.
- Implement services before controllers.
- Implement backend endpoints before web, mobile, and e2e journeys.
- Complete tenant resolution, feature flag checks, authorization, idempotency, restricted-detail filtering, retention, legal hold, export scope, audit, metrics, and no-side-effect checks before marking a story complete.

## Parallel Opportunities

- T003 through T011 can run in parallel after T001 and T002 create the main source roots.
- T031 through T042 can run in parallel after T016 through T030 define shared constants, guards, and abstractions.
- T044 through T047 can run in parallel after T031 through T042 create entity files.
- T053 through T056 can run in parallel after foundational paths exist.
- Test tasks at the start of each user story can run in parallel because they touch separate files.
- Web tasks and conditional mobile tasks in each user story can run after that story's backend API contract is stable.
- US3 search work can run with seeded source records after Phase 2, but final access validation should rerun after US1 and US2.
- US4, US5, and US6 can run with seeded summaries, audit events, metrics, alerts, and incidents after Phase 2, but final polish should validate real workflow integration.

## Parallel Example: User Story 1

```bash
Task: "T059 create document category policy tests in apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Storage/DocumentCategoryPolicyTests.cs"
Task: "T062 create document storage contract tests in tests/contracts/documents-admin/DocumentStorageContractTests.cs"
Task: "T067 create document storage DTOs in apps/api/src/SafeSchool.Api/Features/Documents/Storage/DocumentStorageDtos.cs"
Task: "T082 create web API methods in apps/admin-web/src/features/documents/api/documentStorageApi.ts"
```

## Parallel Example: User Story 2

```bash
Task: "T090 create certificate type policy tests in apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Certificates/CertificateTypePolicyTests.cs"
Task: "T093 create certificate contract tests in tests/contracts/documents-admin/CertificateManagementContractTests.cs"
Task: "T098 create certificate DTOs in apps/api/src/SafeSchool.Api/Features/Documents/Certificates/CertificateManagementDtos.cs"
Task: "T111 create web API methods in apps/admin-web/src/features/documents/api/certificateManagementApi.ts"
```

## Parallel Example: User Story 3

```bash
Task: "T119 create search indexing eligibility tests in apps/api/tests/SafeSchool.Api.Tests/Features/Documents/Search/SearchIndexEligibilityTests.cs"
Task: "T123 create search contract tests in tests/contracts/documents-admin/SearchContractTests.cs"
Task: "T127 create search DTOs in apps/api/src/SafeSchool.Api/Features/Documents/Search/SearchDtos.cs"
Task: "T139 create web API methods in apps/admin-web/src/features/documents/api/searchApi.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T146 create dashboard summary tests in apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Dashboard/AdminDashboardSummaryTests.cs"
Task: "T149 create admin dashboard contract tests in tests/contracts/documents-admin/AdminDashboardConfigurationContractTests.cs"
Task: "T152 create dashboard and configuration DTOs in apps/api/src/SafeSchool.Api/Features/Administration/Dashboard/AdminDashboardConfigurationDtos.cs"
Task: "T163 create web API methods in apps/admin-web/src/features/administration/api/adminDashboardConfigurationApi.ts"
```

## Parallel Example: User Story 5

```bash
Task: "T168 create audit event query tests in apps/api/tests/SafeSchool.Api.Tests/Features/Administration/AuditTrail/AuditEventQueryTests.cs"
Task: "T171 create audit trail contract tests in tests/contracts/documents-admin/AuditTrailContractTests.cs"
Task: "T174 create audit trail DTOs in apps/api/src/SafeSchool.Api/Features/Administration/AuditTrail/AuditTrailDtos.cs"
Task: "T183 create web API methods in apps/admin-web/src/features/administration/api/auditTrailApi.ts"
```

## Parallel Example: User Story 6

```bash
Task: "T188 create metric observation tests in apps/api/tests/SafeSchool.Api.Tests/Features/Administration/Monitoring/MetricObservationServiceTests.cs"
Task: "T193 create monitoring contract tests in tests/contracts/documents-admin/MetricsMonitoringContractTests.cs"
Task: "T196 create monitoring DTOs in apps/api/src/SafeSchool.Api/Features/Administration/Monitoring/MetricsMonitoringDtos.cs"
Task: "T209 create web API methods in apps/admin-web/src/features/administration/api/metricsMonitoringApi.ts"
```

## Implementation Strategy

### MVP First

1. Complete Phase 1 Setup.
2. Complete Phase 2 Foundational.
3. Complete Phase 3 User Story 1.
4. Stop and validate US1 with T059 through T089 plus foundational tests.
5. Demo document upload, classification, versioning, access decisions, legal hold, retention, and export behavior before adding certificates, search, dashboard, audit review, or monitoring.

### Incremental Delivery

1. Add US1 to establish document storage, access, versioning, retention, and export controls.
2. Add US2 to issue, verify, correct, revoke, reissue, and export certificates.
3. Add US3 to search authorized records with index freshness and result-open access decisions.
4. Add US4 to expose admin dashboard summaries and tenant feature configuration.
5. Add US5 to provide audit trail review and controlled audit exports.
6. Add US6 to provide metrics, alert rules, alerts, incidents, and operational exception review.
7. Finish Phase 10/11 polish and run quickstart validation.

### Notes for Lower-Cost LLM Implementation

- Read `specs/011-documents-search-admin-observability/spec.md`, `plan.md`, `data-model.md`, and the matching contract file before starting each story phase.
- Do not implement attendance, campus gate, NFC/QR scan processing, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, communication delivery, or source-domain mutation outcomes.
- Keep every mutation tenant-scoped, feature-gated, permission-checked, idempotent by `client_request_id` where applicable, audit-visible, metric-aware where applicable, and lifecycle-event-aware.
- Preserve original document, certificate, audit, export, alert, incident, and configuration evidence; implement corrections, revocations, redactions, legal holds, retention, reviews, and exports as appended records.
- Keep restricted source details separate from recipient-visible summaries in backend DTOs and UI components.
- Revalidate current access when search results are listed and again when results are opened.
- Treat mobile tasks as conditional: implement them only when `apps/mobile/` exists or mobile document/certificate/search surfaces are enabled for Phase 10/11.
