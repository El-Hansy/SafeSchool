# Tasks: Phase 7 Medical & Emergency

**Input**: Design documents from `/specs/008-medical-emergency/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 7 plan require unit,
integration, contract, authorization, tenant-isolation, offline-cache, audit,
performance, and critical UI journey coverage. Write test tasks before
implementation tasks in each user-story phase and confirm they fail for the
missing behavior before completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a
task is marked `[P]`. Do not implement attendance generation, campus entry or
exit decisions, NFC or QR scan processing, transport boarding/drop decisions,
wallet/payment actions, learning reward actions, request approval workflows,
complaint escalation, broad messaging, broadcasts, document storage, global
search, diagnosis, prescription, pharmacy, emergency services dispatch,
insurance, hospital exchange, or broad admin dashboards. Every Phase 7 action
must resolve tenant context, check the required medical capability, enforce
permission, prevent cross-school visibility, minimize sensitive medical data,
preserve original evidence, record denied access decisions, and emit audit
evidence. Tasks that start with "If an existing mobile client is present" are
conditional; skip them when the repository has no mobile client.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no
  dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses
described in [plan.md](./plan.md).

- [ ] T001 Create or update API solution and Medical source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/Medical/
- [ ] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, logging, and time provider dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [ ] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, authorization fakes, time provider fakes, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [ ] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, route tests, and component test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [ ] T005 [P] If an existing mobile client is present, create or update mobile package manifest with Flutter test, HTTP client, secure storage, local cache, and medical feature dependencies in apps/mobile/pubspec.yaml
- [ ] T006 [P] Create Medical admin, guardian, and student route placeholders plus feature exports in apps/admin-web/src/app/(school)/medical/page.tsx, apps/admin-web/src/app/(guardian)/medical/page.tsx, apps/admin-web/src/app/(student)/medical/page.tsx, and apps/admin-web/src/features/medical/index.ts
- [ ] T007 [P] Create Medical records, emergency, incidents, notifications, history, configuration, and reviews route placeholders in apps/admin-web/src/app/(school)/medical/records/page.tsx, apps/admin-web/src/app/(school)/medical/emergency/page.tsx, apps/admin-web/src/app/(school)/medical/incidents/page.tsx, apps/admin-web/src/app/(school)/medical/notifications/page.tsx, apps/admin-web/src/app/(school)/medical/history/page.tsx, apps/admin-web/src/app/(school)/medical/configuration/page.tsx, and apps/admin-web/src/app/(school)/medical/reviews/page.tsx
- [ ] T008 [P] If an existing mobile client is present, create mobile Medical module export shell in apps/mobile/lib/features/medical/medical.dart
- [ ] T009 Create API bootstrap additions for Medical endpoint registration, versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, and emergency session expiry registration in apps/api/src/SafeSchool.Api/Program.cs
- [ ] T010 [P] Create API configuration placeholders for medical capabilities, emergency access expiry, offline cache freshness, break-glass review, notification status export, audit, and duplicate detection thresholds in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [ ] T011 [P] Create contract test documentation index linking the six Phase 7 contracts in tests/contracts/medical/README.md
- [ ] T012 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/medical/README.md
- [ ] T013 [P] Create repository generated-file ignores for Medical API, web, mobile, coverage, contract fixture, offline cache fixture, and seed data artifacts in .gitignore

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, identity,
privacy, idempotency, persistence, audit, API, web, and conditional mobile foundations
required by every Phase 7 user story.

**Critical**: No user-story implementation should start until this phase is
complete. Foundation must include the minimal exception, manual review, and
effective medical rule support needed by P1 workflows; US5 extends those
foundations with full history, summaries, and configuration UI.

- [ ] T014 Create shared Medical tenant-owned entity base, operation result type, validation error type, paged response type, time provider abstraction, review reason type, source metadata type, privacy redaction marker, and minimal exception/review/rule reference records in apps/api/src/SafeSchool.Api/Features/Medical/Common/TenantOwnedMedicalEntity.cs and apps/api/src/SafeSchool.Api/Features/Medical/Common/MedicalOperationResults.cs
- [ ] T015 [P] Create shared Medical enums for capability keys, permissions, profile status, source type, visibility policy, review state, severity, effective status, consent state, emergency access mode, emergency session status, break-glass status, cache status, incident status, care action type, notification status, contact outcome, acknowledgement state, exception type, exception severity, and review action in apps/api/src/SafeSchool.Api/Features/Medical/Common/MedicalEnums.cs
- [ ] T016 Create or extend SafeSchoolDbContext registration for Medical entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T017 Create Medical model-builder extension shell for tenant metadata, timestamps, indexes, duplicate lookups, status filters, guardian visibility, emergency access lookup, incident severity, notification acknowledgement, minimal exception queues, minimal manual review records, effective rule snapshots, summaries, and audit traceability in apps/api/src/SafeSchool.Api/Features/Medical/Persistence/MedicalDbContextModelBuilderExtensions.cs
- [ ] T018 Create Phase 7 capability constants for medical.records, medical.emergency_access, medical.offline_emergency_cache, medical.incidents, medical.notifications, medical.history, medical.configuration, and medical.review_summaries in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/MedicalCapabilities.cs
- [ ] T019 Create Medical permission catalog entries for records manage/read, guardian update submit/review, guardian history, student summary, emergency read, break-glass, emergency review, incident create/read/review, care action create, notification create/read/contact/review, configuration read/manage, history read, review manage, summary read, audit read, and platform review in apps/api/src/SafeSchool.Api/Features/Medical/Common/MedicalPermissionCatalog.cs
- [ ] T020 Create Medical permission guard that wraps tenant context, feature gate, permission evaluation, student self scope, guardian link scope, medical assignment scope, emergency role scope, reviewer scope, platform review scope, denied access audit, and typed errors in apps/api/src/SafeSchool.Api/Features/Medical/Common/MedicalPermissionGuard.cs
- [ ] T021 Create Medical feature gate and effective rule reader services that enforce Phase 7 capability keys server-side, load active medical rule settings for protected actions, and return typed disabled-capability or unsafe-rule errors in apps/api/src/SafeSchool.Api/Features/Medical/Common/MedicalFeatureGate.cs and apps/api/src/SafeSchool.Api/Features/Medical/Common/Rules/MedicalEffectiveRuleProvider.cs
- [ ] T022 Create IdentityAccess student profile adapter interface and test fake for active, inactive, transferred, suspended, withdrawn, graduated, duplicated, missing, and cross-tenant student profiles in apps/api/src/SafeSchool.Api/Features/Medical/Common/Identity/MedicalStudentProfileProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/FakeMedicalStudentProfileProvider.cs
- [ ] T023 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, restricted, primary, decision-capable, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/Medical/Common/Identity/MedicalGuardianLinkProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/FakeMedicalGuardianLinkProvider.cs
- [ ] T024 Create IdentityAccess staff role and medical assignment adapter interface and test fake for nurse, clinic staff, medical coordinator, emergency-authorized staff, teacher limited emergency view, transport emergency view, gate emergency view, disabled actor, transferred actor, and missing permission cases in apps/api/src/SafeSchool.Api/Features/Medical/Common/Identity/MedicalActorAuthorityProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/FakeMedicalActorAuthorityProvider.cs
- [ ] T025 Create read-only prior identity evidence adapter interface and test fake for NFC or QR identity lookup success, unavailable identity, stale identity, ambiguous identity, cross-tenant identity, and no scan mutation in apps/api/src/SafeSchool.Api/Features/Medical/Common/Identity/MedicalIdentityEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/FakeMedicalIdentityEvidenceProvider.cs
- [ ] T026 Create AttendanceAccess, CampusAccess, Transport, Wallet, Learning, Requests, Complaints, Documents, Search, Dashboard, and Messaging boundary guard that prevents Phase 7 side effects outside medical status event export in apps/api/src/SafeSchool.Api/Features/Medical/Common/Boundaries/MedicalPhaseBoundaryGuard.cs
- [ ] T027 Create Medical idempotency service for client_request_id, guardian update submissions, update reviews, emergency access retries, break-glass retries, offline cache access evidence, incident creation, care action creation, notification requests, contact attempts, acknowledgements, manual reviews, and configuration changes in apps/api/src/SafeSchool.Api/Features/Medical/Common/Idempotency/MedicalIdempotencyService.cs
- [ ] T028 Create Medical audit event entity and audit writer adapter for profile creation, profile update, guardian update submission, consent change, emergency access, break-glass access, emergency denial, incident creation, care action logging, medication evidence, notification request, contact attempt, acknowledgement, failed contact, correction, closure, exception creation, manual review, summary read, configuration change, and access denial events in apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalAuditWriter.cs
- [ ] T029 Create Medical status event entity and status event writer for Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalStatusEvent.cs and apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalStatusEventWriter.cs
- [ ] T030 Create Medical trace reference type and minimal exception/review routing interfaces for P1 review routing across profile, condition, allergy, medication instruction, care plan, consent, guardian update, emergency access, break-glass, offline cache, incident, care action, notification, contact, exception, review, summary, configuration, status event, and audit links in apps/api/src/SafeSchool.Api/Features/Medical/Common/Trace/MedicalTraceReference.cs, apps/api/src/SafeSchool.Api/Features/Medical/Common/Reviews/MedicalExceptionRouter.cs, and apps/api/src/SafeSchool.Api/Features/Medical/Common/Reviews/ManualMedicalReviewRouter.cs
- [ ] T031 Create Medical privacy and visibility service for staff, guardian, student, emergency, reviewer, auditor, and platform reviewer redaction rules in apps/api/src/SafeSchool.Api/Features/Medical/Common/Privacy/MedicalPrivacyService.cs
- [ ] T032 Create Medical route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/medical, /api/v1/guardians/me/students/{studentProfileId}/medical, and /api/v1/students/me/medical in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T033 Create Medical OpenAPI tag registration and shared response/error examples for tenant mismatch, disabled capability, missing permission, invalid guardian link, invalid student, stale cache, missing emergency reason, duplicate command, conflict review, privacy redaction, and audit failure in apps/api/src/SafeSchool.Api/Features/Medical/MedicalOpenApiExamples.cs
- [ ] T034 Create API test fixture for tenants, capabilities, permissions, students, guardians, medical staff, emergency roles, profiles, incidents, notifications, exceptions, reviews, idempotency, boundary assertions, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/MedicalTestFixture.cs
- [ ] T035 [P] Create shared API test data builders for profiles, conditions, allergies, medication instructions, care plans, contacts, consents, guardian updates, emergency sessions, offline cache access, incidents, care actions, notifications, contacts attempts, exceptions, reviews, rule settings, summaries, status events, feature settings, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Fixtures/MedicalTestData.cs
- [ ] T036 [P] Create unit tests for MedicalFeatureGate and MedicalEffectiveRuleProvider capability decisions, disabled capability denial, independent capability keys, active rule lookup, dependent capability validation, unsafe-rule denial, and backend enforcement behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalFeatureGateTests.cs
- [ ] T037 [P] Create unit tests for MedicalPermissionGuard tenant mismatch, guardian scope mismatch, student self scope mismatch, medical assignment mismatch, emergency role mismatch, reviewer scope mismatch, platform review allowance, denied access audit, and allowed access in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalPermissionGuardTests.cs
- [ ] T038 [P] Create unit tests for MedicalIdempotencyService retry, duplicate profile command, duplicate guardian update, duplicate emergency access, duplicate incident, duplicate care action, duplicate notification, duplicate contact attempt, duplicate review, and conflict behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalIdempotencyServiceTests.cs
- [ ] T039 [P] Create unit tests for MedicalPhaseBoundaryGuard preventing attendance, campus gate, scan, transport, wallet, learning, request approval, complaint, document, search, dashboard, direct messaging, diagnosis, prescription, pharmacy, payment, and dispatch side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalPhaseBoundaryGuardTests.cs
- [ ] T040 [P] Create unit tests for MedicalPrivacyService staff, guardian, student, emergency, reviewer, auditor, platform reviewer, staff-only note, restricted guardian, disputed record, inactive instruction, and superseded record filtering in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalPrivacyServiceTests.cs
- [ ] T041 [P] Create unit tests for MedicalAuditWriter sensitive mutation failure behavior, denied access evidence, status-event audit payloads, summary-read audit, and configuration-change audit in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalAuditWriterTests.cs
- [ ] T042 [P] Create school web Medical API client with tenant context, typed errors, pagination, capability-disabled handling, records, emergency, incidents, notifications, history, configuration, reviews, summaries, and trace methods in apps/admin-web/src/features/medical/api/medicalApi.ts
- [ ] T043 [P] Create guardian web Medical API client with linked-student scope, typed errors, pagination, profile, updates, incidents, notification acknowledgement, history, and trace methods in apps/admin-web/src/features/medical/api/guardianMedicalApi.ts
- [ ] T044 [P] Create student web Medical API client with self-scope, typed errors, profile summary, history, and access-denied mapping in apps/admin-web/src/features/medical/api/studentMedicalApi.ts
- [ ] T045 [P] If an existing mobile client is present, create mobile Medical API client shell with auth headers, tenant context, emergency profile, offline cache access evidence, incident capture, and typed denial errors in apps/mobile/lib/features/medical/medical_api.dart
- [ ] T046 [P] Create shared web test data builders for medical profiles, guardian updates, emergency sessions, incidents, notifications, contacts, exceptions, reviews, rule settings, summaries, and audit traces in apps/admin-web/tests/medical/medicalTestData.ts
- [ ] T047 [P] Create contract fixture documentation for common medical errors, tenant isolation, disabled capability, missing permission, privacy redaction, idempotency, stale cache, duplicate handling, and trace payloads in tests/contracts/medical/common-fixtures.md
- [ ] T048 Create Medical observability constants for logs, audit event names, metrics, stale-cache counters, break-glass review counters, incident counters, notification acknowledgement counters, exception counters, and performance timers in apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalObservability.cs
- [ ] T049 [P] Create Medical seed data harness for local tenants, capabilities, roles, students, guardian links, medical staff, emergency roles, and sample records without sensitive real data in apps/api/src/SafeSchool.Api/Features/Medical/Seed/MedicalSeedData.cs
- [ ] T050 [P] If an existing mobile client is present, create mobile offline cache abstraction and test fake for 24-hour freshness, stale warning state, reason capture, pending sync, sync failure, and no unrelated browsing in apps/mobile/lib/features/medical/offline/medical_offline_cache.dart and apps/mobile/test/features/medical/medical_offline_cache_test.dart

**Checkpoint**: Foundation ready. User-story work can start after T014-T050 are complete.

---

## Phase 3: User Story 1 - Manage Student Medical Records (Priority: P1) MVP

**Goal**: Authorized school medical staff can maintain student medical
profiles, conditions, allergies, medication instructions, care plans,
emergency contacts, consent, guardian updates, and permitted guardian/student
views.

**Independent Test**: Create a medical profile for an active student in one
school account, add condition, allergy, medication instruction, care plan,
emergency contact, and guardian consent details, then verify authorized
medical staff and linked guardians see only allowed views while unrelated
users and other school accounts cannot discover or open the record.

### Tests for User Story 1

- [ ] T051 [P] [US1] Create StudentMedicalProfile domain tests for one active profile per tenant/student, required summaries, effective dates, profile status transitions, archive reason, review state, tenant ownership, and audit references in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/StudentMedicalProfileDomainTests.cs
- [ ] T052 [P] [US1] Create medical record item domain tests for condition, allergy, medication instruction, care plan, emergency contact, and consent validation including effective dates, emergency relevance, guardian visibility, staff-only notes, expired instruction, and superseded evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordItemDomainTests.cs
- [ ] T053 [P] [US1] Create GuardianMedicalUpdateSubmission domain tests for pending review default, accepted, rejected, partially accepted, needs information, withdrawn, exact duplicate, conflicting non-identical update, and preserved source evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/GuardianMedicalUpdateDomainTests.cs
- [ ] T054 [P] [US1] Create MedicalProfileService unit tests for active student, inactive student denial, disabled medical.records denial, missing permission denial, cross-school denial, duplicate profile command, conflict review route, archive behavior, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalProfileServiceTests.cs
- [ ] T055 [P] [US1] Create MedicalGuardianVisibilityService unit tests for approved guardian, pending guardian denial, expired guardian denial, restricted guardian, staff-only notes hidden, disputed record hidden, inactive instruction hidden, and student summary filtering in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalGuardianVisibilityServiceTests.cs
- [ ] T056 [P] [US1] Create GuardianMedicalUpdateReviewService unit tests for accept, reject, partially accept, request information, reviewer permission, duplicate review retry, conflict handling, status event creation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/GuardianMedicalUpdateReviewServiceTests.cs
- [ ] T057 [P] [US1] Create contract tests for medical profile create/read/archive, guardian profile read, student profile read, guardian update submit/review, and profile trace routes from contracts/medical-records.md in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordsContractTests.cs
- [ ] T058 [P] [US1] Create integration tests for tenant isolation, medical.records disabled capability, missing records manage permission, approved guardian visibility, unrelated guardian denial, student self scope, duplicate profile command, conflicting guardian update review, profile trace, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordsIntegrationTests.cs
- [ ] T059 [P] [US1] Create web journey tests for school nurse profile create/edit/archive, guardian update review, restricted visibility, disabled capability, access-denied state, and audit trace link in apps/admin-web/tests/medical/medical-records.spec.ts
- [ ] T060 [P] [US1] Create guardian and student web journey tests for guardian profile read, guardian update submit, pending review state, staff-only note redaction, student summary read, unrelated student denial, and disabled capability state in apps/admin-web/tests/medical/guardian-student-medical-records.spec.ts
- [ ] T061 [P] [US1] Create API performance tests verifying complete profile create/update is under 3 minutes and guardian medical profile lookup is under 30 seconds using seeded data in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordsPerformanceTests.cs

### Implementation for User Story 1

- [ ] T062 [P] [US1] Create medical profile, condition, allergy, medication instruction, care plan, emergency contact, consent, guardian update, review, visibility, list, detail, and trace DTOs matching contracts/medical-records.md in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalRecordsDtos.cs
- [ ] T063 [P] [US1] Create StudentMedicalProfile domain entity with tenant metadata, student reference, summaries, visibility policies, profile status, verification fields, effective dates, review state, and audit references in apps/api/src/SafeSchool.Api/Features/Medical/Records/Domain/StudentMedicalProfile.cs
- [ ] T064 [P] [US1] Create condition, allergy, medication instruction, care plan, emergency contact, and medical consent domain entities with validation fields from data-model.md in apps/api/src/SafeSchool.Api/Features/Medical/Records/Domain/MedicalRecordItems.cs
- [ ] T065 [P] [US1] Create GuardianMedicalUpdateSubmission domain entity with submitted area, payload reference, pending review status, reviewer fields, review reason, duplicate identity, and preserved source evidence in apps/api/src/SafeSchool.Api/Features/Medical/Records/Domain/GuardianMedicalUpdateSubmission.cs
- [ ] T066 [US1] Create Medical Records entity configurations and EF migration for profile, condition, allergy, medication instruction, care plan, emergency contact, consent, guardian update, duplicate indexes, guardian visibility indexes, effective-date indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Medical/Records/Persistence/MedicalRecordsEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050010_MedicalRecords.cs
- [ ] T067 [US1] Implement MedicalProfileValidator for required profile fields, active student, one active profile per tenant/student, effective date range, consent consistency, expired record handling, and restricted visibility errors in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalProfileValidator.cs
- [ ] T068 [US1] Implement MedicalProfileService for create, update, archive, duplicate command return, conflict review route, tenant check, feature check, permission check, persistence, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalProfileService.cs
- [ ] T069 [US1] Implement MedicalProfileQueryService for staff-visible profile read, guardian-visible profile read, student summary read, tenant filters, effective record filtering, privacy redaction, and paged profile lookup in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalProfileQueryService.cs
- [ ] T070 [US1] Implement GuardianMedicalUpdateService for guardian update submission, approved guardian link check, pending review default, duplicate submission handling, conflict exception creation, withdrawal, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Records/GuardianMedicalUpdateService.cs
- [ ] T071 [US1] Implement GuardianMedicalUpdateReviewService for accept, reject, partial accept, request information, reviewer authorization, verified evidence application, original submission preservation, idempotency, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Records/GuardianMedicalUpdateReviewService.cs
- [ ] T072 [US1] Implement MedicalProfileTraceService for profile, record item, guardian update, review, status event, exception, and audit trace references in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalProfileTraceService.cs
- [ ] T073 [US1] Implement MedicalRecordsController routes for school profile create/read/archive, guardian profile read, student profile read, guardian update submit, update review, and profile trace in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalRecordsController.cs
- [ ] T074 [US1] Wire profile creation, profile update, archive, guardian update submission, consent change, update review, profile read denial, guardian visibility denial, student summary read, status event, and trace read audit events in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalRecordsAuditAdapter.cs
- [ ] T075 [US1] Update Medical endpoint registration to map school profile, guardian profile, student profile, guardian update, update review, archive, and trace routes in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T076 [US1] Create OpenAPI examples for profile create/update, archive, guardian read, student read, guardian update submit, update review accept/reject, duplicate result, conflict review, disabled capability, missing permission, guardian denial, and trace in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalRecordsOpenApiExamples.cs
- [ ] T077 [P] [US1] Create school web Medical Records API hooks for profile create/update/archive/read, guardian update review, trace read, typed errors, and privacy redaction in apps/admin-web/src/features/medical/records/medicalRecordsApi.ts
- [ ] T078 [P] [US1] Create medical profile editor, condition editor, allergy editor, medication instruction editor, care plan editor, emergency contact editor, consent editor, and validation summary components in apps/admin-web/src/features/medical/records/MedicalProfileEditor.tsx and apps/admin-web/src/features/medical/records/MedicalRecordItemEditors.tsx
- [ ] T079 [P] [US1] Create guardian update review queue, review decision panel, duplicate/conflict banner, source evidence panel, and profile trace panel in apps/admin-web/src/features/medical/records/GuardianMedicalUpdateReview.tsx and apps/admin-web/src/features/medical/records/MedicalProfileTracePanel.tsx
- [ ] T080 [US1] Implement school Medical Records route with student lookup, profile editor, guardian update queue, permission-denied state, disabled capability state, save/archive actions, and trace links in apps/admin-web/src/app/(school)/medical/records/page.tsx
- [ ] T081 [P] [US1] Create guardian and student medical profile components for allowed details, pending update status, staff-only redaction, disputed record redaction, and disabled capability state in apps/admin-web/src/features/medical/records/GuardianMedicalProfile.tsx and apps/admin-web/src/features/medical/records/StudentMedicalSummary.tsx
- [ ] T082 [US1] Implement guardian and student Medical routes with linked-student scope, own-student scope, guardian update form, pending review state, access-denied state, and no-side-effect messaging in apps/admin-web/src/app/(guardian)/medical/page.tsx and apps/admin-web/src/app/(student)/medical/page.tsx
- [ ] T083 [P] [US1] If an existing mobile client is present, create mobile medical profile models and repository methods for allowed profile summary, guardian update submission, pending review state, typed denial errors, and no offline mutation of verified records in apps/mobile/lib/features/medical/records/medical_records_models.dart and apps/mobile/lib/features/medical/records/medical_records_repository.dart
- [ ] T084 [US1] If an existing mobile client is present, create mobile medical profile screen shell for optional guardian/student medical profile display with redaction, pending review, disabled capability, and access-denied states in apps/mobile/lib/features/medical/records/medical_profile_screen.dart
- [ ] T085 [US1] Create Medical Records test data builder for active profile, archived profile, disputed profile, critical allergy, expired medication instruction, care plan, emergency contact, consent, linked guardian, restricted guardian, guardian update, conflict update, duplicate command, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordsTestData.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T051-T085 pass.

---

## Phase 4: User Story 2 - Access Critical Medical Data During Emergencies (Priority: P1)

**Goal**: Emergency-authorized staff can open minimum-necessary critical
medical data, use pre-authorized break-glass access, and record optional
offline cache access evidence with 30-minute sessions and 24-hour cache
freshness.

**Independent Test**: Start an emergency access session for an active student,
provide the required reason, view critical allergies, medication instructions,
care plan, emergency contacts, and latest relevant incidents, and verify the
session is time-bounded, minimum-necessary, tenant-scoped, and audit-visible.

### Tests for User Story 2

- [ ] T086 [P] [US2] Create EmergencyAccessSession domain tests for required reason, viewed categories, 30-minute expiry, re-confirmation, close, denied status, review state, tenant ownership, and trace references in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessSessionDomainTests.cs
- [ ] T087 [P] [US2] Create BreakGlassAccessEvent domain tests for pre-authorized emergency role, explicit confirmation, mandatory review, non-authorized staff denial, review due time, and audit references in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/BreakGlassAccessEventDomainTests.cs
- [ ] T088 [P] [US2] Create EmergencyOfflineCacheAccess domain tests for 24-hour freshness, stale warning acknowledgement, required stale reason, sync status, no unrelated browsing, and review routing in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyOfflineCacheAccessDomainTests.cs
- [ ] T089 [P] [US2] Create CriticalEmergencyProfileProjection unit tests for active emergency-relevant allergies, medication instructions, care plans, restrictions, emergency contacts, latest relevant incidents, staff-only redaction, expired record exclusion, and disputed record filtering in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/CriticalEmergencyProfileProjectionTests.cs
- [ ] T090 [P] [US2] Create EmergencyAccessService unit tests for emergency authority, missing reason denial, disabled capability denial, invalid student denial, cross-school denial, duplicate access retry, session expiry, re-confirmation, close, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessServiceTests.cs
- [ ] T091 [P] [US2] Create BreakGlassAccessService unit tests for pre-authorized role allow, ordinary medical permission absence, class/gate/trip/bus assignment without role denial, mandatory review creation, duplicate retry, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/BreakGlassAccessServiceTests.cs
- [ ] T092 [P] [US2] Create OfflineEmergencyCacheService unit tests for fresh cache allow, stale cache warning, stale reason required, stale review route, pending sync evidence, sync failure, disabled cache capability, and no scan side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/OfflineEmergencyCacheServiceTests.cs
- [ ] T093 [P] [US2] Create contract tests for emergency access start/reconfirm/close, break-glass, offline cache access evidence, and emergency access trace routes from contracts/emergency-access.md in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessContractTests.cs
- [ ] T094 [P] [US2] Create integration tests for tenant isolation, emergency access disabled capability, offline cache disabled capability, missing emergency permission, break-glass role denial, stale cache review, 30-minute expiry, re-confirmation, no scan mutation, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessIntegrationTests.cs
- [ ] T095 [P] [US2] Create web emergency journey tests and, if an existing mobile client is present, matching mobile tests for emergency profile open, break-glass confirmation, stale cache warning, re-confirmation prompt, access-denied state, mandatory review marker, and trace link in apps/admin-web/tests/medical/emergency-access.spec.ts and apps/mobile/test/features/medical/emergency_access_test.dart
- [ ] T096 [P] [US2] Create API performance tests verifying critical emergency profile opens in under 30 seconds and emergency trace preserves actor, reason, categories, expiry, re-confirmation, and review state in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessPerformanceTests.cs

### Implementation for User Story 2

- [ ] T097 [P] [US2] Create emergency access, break-glass, reconfirm, close, offline cache access, emergency profile, trace, and denial DTOs matching contracts/emergency-access.md in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessDtos.cs
- [ ] T098 [P] [US2] Create EmergencyAccessSession, BreakGlassAccessEvent, and EmergencyOfflineCacheAccess domain entities with fields and state transitions from data-model.md in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/Domain/EmergencyAccessEntities.cs
- [ ] T099 [US2] Create Emergency Access entity configurations and EF migration for sessions, break-glass events, offline cache access evidence, emergency lookup indexes, break-glass review queue indexes, expiry indexes, cache freshness indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/Persistence/EmergencyAccessEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050020_MedicalEmergencyAccess.cs
- [ ] T100 [US2] Implement CriticalEmergencyProfileProjection for minimum-necessary filtering of active allergies, medication instructions, care plans, restrictions, emergency contacts, and recent relevant incidents with privacy redaction in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/CriticalEmergencyProfileProjection.cs
- [ ] T101 [US2] Implement EmergencyAccessPolicy for 30-minute session duration, re-confirmation requirement, required reason, emergency data categories, allowed access modes, denied states, and configuration validation in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessPolicy.cs
- [ ] T102 [US2] Implement EmergencyAccessService for session start, duplicate retry handling, authority validation, critical profile response, expiry calculation, session persistence, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessService.cs
- [ ] T103 [US2] Implement EmergencySessionLifecycleService for re-confirmation, close, expiry detection, active session lookup, denied final state behavior, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencySessionLifecycleService.cs
- [ ] T104 [US2] Implement BreakGlassAccessService for pre-authorized role validation, explicit confirmation, ordinary permission bypass only for emergency role, mandatory review event creation, duplicate retry handling, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/BreakGlassAccessService.cs
- [ ] T105 [US2] Implement OfflineEmergencyCacheService for cache freshness evaluation, 24-hour usable window, stale warning acknowledgement, stale reason validation, review route, syncable access evidence, and no unrelated browsing in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/OfflineEmergencyCacheService.cs
- [ ] T106 [US2] Implement EmergencyAccessTraceService for session, break-glass, offline cache, profile snapshot, viewed categories, incidents, exceptions, reviews, status events, and audit trace references in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessTraceService.cs
- [ ] T107 [US2] Implement EmergencyAccessController routes for session start, re-confirmation, close, break-glass, offline cache access evidence, and trace in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessController.cs
- [ ] T108 [US2] Wire emergency access, break-glass access, emergency denial, stale cache access, stale cache denial, re-confirmation, session close, expiry, mandatory review, and trace read audit events in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessAuditAdapter.cs
- [ ] T109 [US2] Update Medical endpoint registration to map emergency access, break-glass, re-confirmation, close, offline cache access, and emergency trace routes in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T110 [US2] Create OpenAPI examples for emergency access start, break-glass, re-confirmation, close, fresh offline cache, stale offline cache, missing reason denial, unauthorized role denial, disabled capability, no scan side effect, and trace in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencyAccessOpenApiExamples.cs
- [ ] T111 [P] [US2] Create school web Emergency Access API hooks for start, break-glass, re-confirmation, close, offline cache evidence, trace, stale cache errors, and typed denial errors in apps/admin-web/src/features/medical/emergency/emergencyAccessApi.ts
- [ ] T112 [P] [US2] Create emergency student lookup, critical profile panel, allergy alert list, medication instruction panel, care plan panel, contact panel, recent incident panel, and emergency trace panel in apps/admin-web/src/features/medical/emergency/EmergencyProfilePanel.tsx and apps/admin-web/src/features/medical/emergency/EmergencyTracePanel.tsx
- [ ] T113 [P] [US2] Create break-glass confirmation modal, stale cache warning panel, session expiry timer, re-confirmation prompt, mandatory review badge, and access-denied panel in apps/admin-web/src/features/medical/emergency/EmergencyAccessControls.tsx
- [ ] T114 [US2] Implement school Emergency Access route with reason capture, critical profile view, break-glass flow, stale cache display, expiry/re-confirmation handling, no-side-effect boundaries, disabled capability state, and trace links in apps/admin-web/src/app/(school)/medical/emergency/page.tsx
- [ ] T115 [P] [US2] If an existing mobile client is present, create mobile emergency access models, offline cache snapshot model, stale cache state model, access evidence model, and repository methods in apps/mobile/lib/features/medical/emergency/emergency_access_models.dart and apps/mobile/lib/features/medical/emergency/emergency_access_repository.dart
- [ ] T116 [US2] If an existing mobile client is present, implement mobile critical emergency profile screen with reason capture, freshness display, stale warning, break-glass indicator, offline evidence queue, re-confirmation prompt, and access-denied state in apps/mobile/lib/features/medical/emergency/emergency_profile_screen.dart
- [ ] T117 [US2] If an existing mobile client is present, update mobile offline cache abstraction to store only emergency essentials, enforce 24-hour freshness, block unrelated browsing, require stale reason, and queue syncable access evidence in apps/mobile/lib/features/medical/offline/medical_offline_cache.dart
- [ ] T118 [US2] Create emergency session expiry scanner that marks expired sessions, emits metrics, preserves review state, and never extends access automatically in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/EmergencySessionExpiryScanner.cs
- [ ] T119 [US2] Create Emergency Access test data builder for active emergency profile, expired instructions, stale cache, fresh cache, break-glass role, non-authorized staff, missing reason, duplicate retry, cross-school student, and no-scan side-effect cases in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessTestData.cs
- [ ] T120 [US2] Update MedicalPhaseBoundaryGuard tests and implementation to assert emergency access and offline cache access never create NFC, QR, attendance, gate, transport, wallet, request, complaint, document, search, or messaging outcomes in apps/api/src/SafeSchool.Api/Features/Medical/Common/Boundaries/MedicalPhaseBoundaryGuard.cs

**Checkpoint**: User Story 2 can be demonstrated independently after T086-T120 pass.

---

## Phase 5: User Story 3 - Log Medical Incidents and Care Actions (Priority: P1)

**Goal**: Authorized staff can log medical incidents, append care actions,
record medication administration evidence, handle duplicates/conflicts, close
or correct incidents, and preserve a complete tenant-scoped timeline.

**Independent Test**: Log a medical incident for an active student, record
severity, location, observed details, care actions, medication administration
evidence, guardian contact evidence as a care action, follow-up requirement, and closure status,
then verify the incident remains tenant-scoped and preserves all updates.

### Tests for User Story 3

- [ ] T121 [P] [US3] Create MedicalIncident domain tests for required severity, location context, observed details, occurrence time, status transitions, duplicate identity, conflicting evidence, tenant ownership, and audit references in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentDomainTests.cs
- [ ] T122 [P] [US3] Create CareAction domain tests for observation, first aid, medication administration, emergency services handoff evidence, guardian contact, follow-up, append-only corrections, performed time, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/CareActionDomainTests.cs
- [ ] T123 [P] [US3] Create MedicationAdministrationEvidence unit tests for active instruction allow, expired instruction review, missing consent review, override reason required, wrong student denial, wrong dosage evidence review, no diagnosis, no prescription, no pharmacy, and no payment side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicationAdministrationEvidenceTests.cs
- [ ] T124 [P] [US3] Create MedicalIncidentService unit tests for incident create, severity escalation, contact required state, follow-up required state, duplicate return, conflict manual review, inactive student denial, disabled capability denial, missing permission denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentServiceTests.cs
- [ ] T125 [P] [US3] Create CareActionService unit tests for append action, medication evidence, handoff evidence, follow-up completion, duplicate action retry, conflicting care evidence review, unauthorized actor denial, cross-school denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/CareActionServiceTests.cs
- [ ] T126 [P] [US3] Create IncidentReviewService unit tests for close, correct, dispute, reopen, resolve, escalate, reason required, original evidence preserved, duplicate review retry, guardian visibility update, status event creation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/IncidentReviewServiceTests.cs
- [ ] T127 [P] [US3] Create contract tests for incident create/list/detail, care action add, close, review, and incident trace routes from contracts/medical-incident-logging.md in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentContractTests.cs
- [ ] T128 [P] [US3] Create integration tests for tenant isolation, medical.incidents disabled capability, missing incident permission, emergency-authorized actor, duplicate incident, concurrent care action conflict, expired instruction, missing consent, no wallet/payment side effects, trace read, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentIntegrationTests.cs
- [ ] T129 [P] [US3] Create web journey tests for incident create, care action timeline, medication evidence, severity escalation, follow-up, close, correction, duplicate retry display, disabled capability, access-denied state, and trace link in apps/admin-web/tests/medical/medical-incidents.spec.ts
- [ ] T130 [P] [US3] If an existing mobile client is present, create mobile incident capture tests for incident draft, required fields, offline submit guard, medication evidence fields, follow-up state, duplicate error display, and no wallet/payment side effects in apps/mobile/test/features/medical/medical_incident_capture_test.dart
- [ ] T131 [P] [US3] Create API performance tests verifying complete incident logging is under 2 minutes and incident trace preserves original and corrected evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentPerformanceTests.cs

### Implementation for User Story 3

- [ ] T132 [P] [US3] Create incident create, incident list filter, incident detail, care action, medication evidence, close, review, trace, and duplicate/conflict DTOs matching contracts/medical-incident-logging.md in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentDtos.cs
- [ ] T133 [P] [US3] Create MedicalIncident and CareAction domain entities with fields, status transitions, duplicate identity, correction state, medication evidence fields, and audit references from data-model.md in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/Domain/MedicalIncidentEntities.cs
- [ ] T134 [US3] Create Medical Incident entity configurations and EF migration for incidents, care actions, incident severity indexes, medication instruction lookups, duplicate indexes, follow-up indexes, status indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/Persistence/MedicalIncidentEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050030_MedicalIncidents.cs
- [ ] T135 [US3] Implement MedicalIncidentValidator for required severity, location, observed details, active student, actor authority, feature capability, medical profile context, duplicate identity, and no-side-effect boundary validation in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentValidator.cs
- [ ] T136 [US3] Implement MedicalIncidentService for create, list, detail, severity escalation, status changes, duplicate return, conflict exception creation, contact required state, follow-up state, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentService.cs
- [ ] T137 [US3] Implement MedicationAdministrationEvidencePolicy for active instruction, consent state, override reason, expired instruction review, wrong student denial, dosage evidence review, and no diagnosis/prescription/pharmacy/payment outcomes in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicationAdministrationEvidencePolicy.cs
- [ ] T138 [US3] Implement CareActionService for append-only observation, first aid, medication administration, emergency services handoff evidence, guardian contact evidence, follow-up actions, duplicate retry, conflict review route, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/CareActionService.cs
- [ ] T139 [US3] Implement IncidentReviewService for close, correct, dispute, reopen, resolve, escalate, reason required, preserved original evidence, guardian/staff visibility updates, exception resolution, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/IncidentReviewService.cs
- [ ] T140 [US3] Implement MedicalIncidentQueryService for school incident list/detail filters by student, severity, status, location, care action, medication evidence, date range, follow-up state, exception state, actor, and pagination in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentQueryService.cs
- [ ] T141 [US3] Implement MedicalIncidentTraceService for incident, care action, medication instruction, consent, notification request, contact attempt, exception, review, status event, and audit references in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentTraceService.cs
- [ ] T142 [US3] Implement MedicalIncidentController routes for incident create/list/detail, care action add, close, review, and trace in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentController.cs
- [ ] T143 [US3] Wire incident creation, severity escalation, care action logging, medication evidence, handoff evidence, guardian contact action, follow-up, duplicate return, correction, closure, access denial, and trace read audit events in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentAuditAdapter.cs
- [ ] T144 [US3] Update Medical endpoint registration to map incident create/list/detail, care action, close, review, and trace routes in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T145 [US3] Create OpenAPI examples for incident create, list, detail, care action, medication evidence, close, review, duplicate result, conflict review, expired instruction, missing consent, no payment side effect, disabled capability, missing permission, and trace in apps/api/src/SafeSchool.Api/Features/Medical/Incidents/MedicalIncidentOpenApiExamples.cs
- [ ] T146 [P] [US3] Create school web Medical Incident API hooks for create, list, detail, care action, close, review, trace, typed errors, duplicate result, conflict review, and permission denial in apps/admin-web/src/features/medical/incidents/medicalIncidentsApi.ts
- [ ] T147 [P] [US3] Create incident form, severity selector, location/context selector, observation editor, required field summary, duplicate warning, and conflict review banner in apps/admin-web/src/features/medical/incidents/MedicalIncidentForm.tsx
- [ ] T148 [P] [US3] Create care action timeline, medication evidence panel, handoff evidence panel, follow-up panel, correction panel, closure panel, and incident trace panel in apps/admin-web/src/features/medical/incidents/CareActionTimeline.tsx and apps/admin-web/src/features/medical/incidents/MedicalIncidentTracePanel.tsx
- [ ] T149 [US3] Implement school Medical Incidents route with create/list/detail, care action timeline, close/review actions, duplicate retry display, disabled capability state, access-denied state, and trace links in apps/admin-web/src/app/(school)/medical/incidents/page.tsx
- [ ] T150 [P] [US3] If an existing mobile client is present, create mobile medical incident models and repository methods for incident create, draft validation, care action append, medication evidence, duplicate/conflict errors, and typed denial errors in apps/mobile/lib/features/medical/incidents/medical_incident_models.dart and apps/mobile/lib/features/medical/incidents/medical_incident_repository.dart
- [ ] T151 [US3] If an existing mobile client is present, implement mobile medical incident capture screen with severity, location, observation, care action, medication evidence, follow-up, guardian contact note, submit result, duplicate warning, disabled capability, and access-denied states in apps/mobile/lib/features/medical/incidents/medical_incident_capture_screen.dart
- [ ] T152 [US3] Update MedicalPhaseBoundaryGuard tests and implementation to assert incidents and care actions never create diagnosis, prescription, pharmacy, wallet, payment, attendance, gate, transport, request, complaint, document, search, or broad messaging outcomes in apps/api/src/SafeSchool.Api/Features/Medical/Common/Boundaries/MedicalPhaseBoundaryGuard.cs
- [ ] T153 [US3] Create Medical Incident test data builder for active incident, high-severity incident, duplicate incident, conflicting incident, care actions, medication instruction, expired instruction, missing consent, override reason, wrong student, follow-up required, closed incident, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentTestData.cs
- [ ] T154 [US3] Update CriticalEmergencyProfileProjection to include latest relevant active incidents without exposing full incident history in apps/api/src/SafeSchool.Api/Features/Medical/EmergencyAccess/CriticalEmergencyProfileProjection.cs
- [ ] T155 [US3] Update MedicalProfileTraceService to include incident and care action references without changing profile evidence in apps/api/src/SafeSchool.Api/Features/Medical/Records/MedicalProfileTraceService.cs

**Checkpoint**: User Story 3 can be demonstrated independently after T121-T155 pass.

---

## Phase 6: User Story 4 - Manage Medical Notifications and Acknowledgements (Priority: P2)

**Goal**: Authorized staff can create medical notification requests, apply
high-severity default audience rules, record contact attempts and
acknowledgements, and expose status events for Phase 9 without delivering
general messages.

**Independent Test**: Create a medical notification request from a
high-severity incident, verify approved guardians, emergency contacts,
assigned nurse or clinic staff, and the school emergency coordinator are
selected by default, record contact attempts and acknowledgements, and confirm
sensitive details are minimized and no broad messaging or broadcast workflow is
created.

### Tests for User Story 4

- [ ] T156 [P] [US4] Create MedicalNotificationRequest domain tests for source incident, source emergency access, urgency, default audience applied, privacy summary, acknowledgement required, status transitions, duplicate identity, tenant ownership, and audit references in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationRequestDomainTests.cs
- [ ] T157 [P] [US4] Create MedicalContactAttempt domain tests for recipient role, emergency contact, contact route category, attempt outcome, acknowledgement state, failure reason, manual actor, attempted time, acknowledged time, invalid contact, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalContactAttemptDomainTests.cs
- [ ] T158 [P] [US4] Create MedicalNotificationAudienceService unit tests for high-severity default audience, approved guardian selection, emergency contact priority, assigned nurse/clinic staff selection, emergency coordinator selection, invalid guardian block, expired contact skip, and restricted detail minimization in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationAudienceServiceTests.cs
- [ ] T159 [P] [US4] Create MedicalNotificationService unit tests for create, confirm, duplicate active notification, closed incident denial, source emergency access allow, disabled capability denial, missing permission denial, broad broadcast block, status event creation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationServiceTests.cs
- [ ] T160 [P] [US4] Create MedicalContactAttemptService unit tests for manual call, alternate contact, failed contact, delayed acknowledgement, acknowledgement update, duplicate attempt retry, invalid recipient review, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalContactAttemptServiceTests.cs
- [ ] T161 [P] [US4] Create contract tests for notification create/list/detail, contact attempt, acknowledgement, notification review, and notification trace routes from contracts/medical-notifications.md in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationsContractTests.cs
- [ ] T162 [P] [US4] Create integration tests for high-severity default audience, sensitive detail minimization, invalid guardian link block, expired contact skip, duplicate notification idempotency, closed incident block, no broadcast delivery, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationsIntegrationTests.cs
- [ ] T163 [P] [US4] Create web journey tests for notification request creation, default audience review, manual contact attempt, failed contact, acknowledgement, delayed acknowledgement, duplicate notification display, disabled capability, access-denied state, and trace link in apps/admin-web/tests/medical/medical-notifications.spec.ts
- [ ] T164 [P] [US4] Create API performance tests verifying high-severity notification requests are available within 2 minutes and status events are eligible for Phase 9 consumption without delivery in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationsPerformanceTests.cs

### Implementation for User Story 4

- [ ] T165 [P] [US4] Create notification request, audience, contact attempt, acknowledgement, review, list filter, detail, trace, and status event DTOs matching contracts/medical-notifications.md in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationDtos.cs
- [ ] T166 [P] [US4] Create MedicalNotificationRequest and MedicalContactAttempt domain entities with fields, status transitions, acknowledgement states, duplicate identity, and audit references from data-model.md in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/Domain/MedicalNotificationEntities.cs
- [ ] T167 [US4] Create Medical Notification entity configurations and EF migration for notification requests, contact attempts, audience indexes, acknowledgement indexes, contact failure indexes, duplicate indexes, status event indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/Persistence/MedicalNotificationEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050040_MedicalNotifications.cs
- [ ] T168 [US4] Implement MedicalNotificationAudienceService for high-severity default audience, guardian link filtering, emergency contact priority, nurse/clinic staff selection, emergency coordinator selection, required staff selection, invalid recipient review, and privacy minimization in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationAudienceService.cs
- [ ] T169 [US4] Implement MedicalNotificationService for create, confirm, source validation, urgency evaluation, duplicate active notification idempotency, closed incident block, broad broadcast prevention, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationService.cs
- [ ] T170 [US4] Implement MedicalContactAttemptService for manual contact, delivery evidence import, failed contact, alternate contact, delayed acknowledgement, acknowledgement update, duplicate retry, invalid contact review, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalContactAttemptService.cs
- [ ] T171 [US4] Implement MedicalNotificationQueryService for notification list/detail filters by student, source, urgency, status, audience, acknowledgement state, failed contact, date range, and pagination in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationQueryService.cs
- [ ] T172 [US4] Implement MedicalNotificationReviewService for correct, cancel, reopen, review, reason required, preserved original request/contact evidence, exception resolution, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationReviewService.cs
- [ ] T173 [US4] Implement MedicalNotificationTraceService for source incident/access, audience, contacts, acknowledgements, exceptions, reviews, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationTraceService.cs
- [ ] T174 [US4] Implement MedicalNotificationController routes for create/list/detail, contact attempts, acknowledgements, review, and trace in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationController.cs
- [ ] T175 [US4] Wire notification request, contact attempt, acknowledgement, failed contact, duplicate notification, invalid recipient, review, status event export, no delivery side effect, and trace read audit events in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationAuditAdapter.cs
- [ ] T176 [US4] Update Medical endpoint registration to map notification create/list/detail, contact attempt, acknowledgement, review, and trace routes in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T177 [US4] Create OpenAPI examples for notification create, high-severity default audience, contact attempt, acknowledgement, failed contact, duplicate result, invalid guardian denial, closed incident denial, broad broadcast block, status event export, and trace in apps/api/src/SafeSchool.Api/Features/Medical/Notifications/MedicalNotificationOpenApiExamples.cs
- [ ] T178 [P] [US4] Create school web Medical Notifications API hooks for create, list, detail, contact attempt, acknowledgement, review, trace, default audience preview, and typed denial errors in apps/admin-web/src/features/medical/notifications/medicalNotificationsApi.ts
- [ ] T179 [P] [US4] Create notification request panel, default audience panel, privacy summary editor, contact priority list, contact attempt form, acknowledgement tracker, failed contact panel, and notification trace panel in apps/admin-web/src/features/medical/notifications/MedicalNotificationPanel.tsx and apps/admin-web/src/features/medical/notifications/MedicalNotificationTracePanel.tsx
- [ ] T180 [US4] Implement school Medical Notifications route with create/list/detail, default audience preview, contact attempts, acknowledgement tracking, duplicate display, disabled capability state, access-denied state, and trace links in apps/admin-web/src/app/(school)/medical/notifications/page.tsx
- [ ] T181 [P] [US4] Create guardian web notification acknowledgement components for allowed incident/notification summary, acknowledgement action, privacy redaction, disabled capability, and access-denied states in apps/admin-web/src/features/medical/notifications/GuardianMedicalAcknowledgement.tsx
- [ ] T182 [US4] Update guardian Medical route to show allowed medical notification requests and acknowledgement actions without broad messaging delivery controls in apps/admin-web/src/app/(guardian)/medical/page.tsx
- [ ] T183 [US4] Create Medical Notifications test data builder for high-severity incident, emergency access source, approved guardians, emergency contacts, assigned nurse, emergency coordinator, invalid guardian, expired contact, duplicate notification, failed contact, acknowledgement, closed incident, and broad broadcast attempt cases in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationsTestData.cs

**Checkpoint**: User Story 4 can be demonstrated independently after T156-T183 pass.

---

## Phase 7: User Story 5 - Review Medical History, Exceptions, and Access Audits (Priority: P3)

**Goal**: Authorized administrators, medical coordinators, guardians,
reviewers, and auditors can search medical history, review exceptions, correct
or resolve records, configure medical rules, and trace lifecycle evidence with
tenant and privacy boundaries.

**Independent Test**: Search medical history for a student, review emergency
access sessions and incidents, correct an eligible incident with a reason,
configure medical visibility rules, and verify all results remain
permission-scoped and tenant-scoped.

### Tests for User Story 5

- [ ] T184 [P] [US5] Create full-history extension tests for foundational MedicalException behavior covering invalid student, inactive student, expired instruction, conflicting record, missing consent, missing emergency reason, duplicate incident, duplicate notification, failed contact, stale cache, disabled feature, cross-school access, manual review required, status transitions, and preserved evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalExceptionDomainTests.cs
- [ ] T185 [P] [US5] Create full-review extension tests for foundational ManualMedicalReview behavior covering correct, reopen, close, resolve, dismiss, escalate, migrate rule version, reason required, original status, resulting status, reviewer actor, reviewed time, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/ManualMedicalReviewDomainTests.cs
- [ ] T186 [P] [US5] Create configuration extension tests for foundational MedicalRuleSetting behavior covering visibility, emergency access, break-glass, consent, medication evidence, incident severity, notification audience, acknowledgement, review routing, rule version, activation, suspension, retirement, and historical reference behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Configuration/MedicalRuleSettingDomainTests.cs
- [ ] T187 [P] [US5] Create MedicalHistoryQueryService unit tests for filters by student, guardian, condition, allergy, medication instruction, care plan, emergency access, incident severity, care action, medication evidence, notification state, acknowledgement, date range, actor, exception type, review state, tenant, and visibility in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/History/MedicalHistoryQueryServiceTests.cs
- [ ] T188 [P] [US5] Create extension unit tests for foundational MedicalExceptionService covering exception search, assignment, resolve, dismiss, escalate, reopen, duplicate exception identity, reviewer permission, resolution reason, status event creation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalExceptionServiceTests.cs
- [ ] T189 [P] [US5] Create extension unit tests for foundational ManualMedicalReviewService covering correction, reopen, close, resolve, dismiss, escalate, rule migration, preserved original evidence, duplicate review retry, privacy impact, status event creation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/ManualMedicalReviewServiceTests.cs
- [ ] T190 [P] [US5] Create extension unit tests for foundational effective rule support and MedicalConfigurationService covering feature settings read, draft create, activate, suspend, invalid break-glass role, invalid emergency duration, offline freshness greater than 24 hours, unsafe visibility, disabled dependent capability, versioning, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Configuration/MedicalConfigurationServiceTests.cs
- [ ] T191 [P] [US5] Create MedicalReviewSummaryService unit tests for summary by student, condition, allergy, care plan, incident severity, emergency access state, notification acknowledgement state, exception state, date range, reviewer assignment, guardian redaction, student redaction, and platform reviewer visibility in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalReviewSummaryServiceTests.cs
- [ ] T192 [P] [US5] Create contract tests for medical history, exceptions, manual reviews, review summaries, lifecycle trace, feature settings, and rule settings routes from contracts/medical-history-review.md and contracts/medical-configuration.md in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalHistoryReviewContractTests.cs
- [ ] T193 [P] [US5] Create integration tests for tenant isolation, medical.history disabled capability, medical.configuration disabled capability, reviewer permission denial, guardian redaction, student redaction, rule version snapshot, correction preservation, summary read audit, trace under 60 seconds, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalHistoryReviewIntegrationTests.cs
- [ ] T194 [P] [US5] Create web journey tests for history search, exception queue, manual review, incident correction, rule configuration, rule version display, review summary, guardian history redaction, auditor trace, disabled capability, and access-denied state in apps/admin-web/tests/medical/medical-history-review.spec.ts
- [ ] T195 [P] [US5] Create API performance tests verifying auditor lifecycle trace is under 60 seconds and review summaries remain permission-scoped under seeded data volume in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalHistoryReviewPerformanceTests.cs

### Implementation for User Story 5

- [ ] T196 [P] [US5] Create history query, history item, exception, manual review, rule setting, feature setting, review summary, lifecycle trace, configuration trace, and audit reference DTOs matching contracts/medical-history-review.md and contracts/medical-configuration.md in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalHistoryReviewDtos.cs
- [ ] T197 [P] [US5] Extend foundational MedicalException, ManualMedicalReview, MedicalRuleSetting, MedicalReviewSummary, and SchoolAccountFeatureSetting domain entities with full history, configuration, summary, and validation fields from data-model.md in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/Domain/MedicalReviewEntities.cs
- [ ] T198 [US5] Extend foundational Medical Review and Configuration entity configurations and EF migration for full exception history, manual reviews, rule settings, review summaries, feature settings, exception status indexes, reviewer assignment indexes, summary filter indexes, rule version indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/Persistence/MedicalReviewEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050050_MedicalHistoryReviews.cs
- [ ] T199 [US5] Implement MedicalHistoryQueryService for school, guardian, and student history filters, tenant scope, permission scope, guardian link scope, student self scope, reviewer scope, platform review scope, pagination, and privacy redaction in apps/api/src/SafeSchool.Api/Features/Medical/History/MedicalHistoryQueryService.cs
- [ ] T200 [US5] Extend foundational MedicalExceptionService for exception search, assignment, resolve, dismiss, escalate, reopen, resolution reason, reviewer authorization, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalExceptionService.cs
- [ ] T201 [US5] Extend foundational ManualMedicalReviewService for correct, reopen, close, resolve, dismiss, escalate, migrate rule version, original evidence preservation, resulting status, duplicate review retry, privacy impact, status event creation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/ManualMedicalReviewService.cs
- [ ] T202 [US5] Extend foundational effective rule support with MedicalConfigurationService for feature settings read, rule setting draft/create/update/activate/suspend, 30-minute emergency duration validation, 24-hour offline cache validation, break-glass role validation, unsafe visibility rejection, dependent capability validation, versioning, and audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Configuration/MedicalConfigurationService.cs
- [ ] T203 [US5] Implement MedicalReviewSummaryService for student, profile, incident, emergency access, notification, reviewer, guardian, student, auditor, and platform reviewer summaries with privacy filtering and summary-read audit evidence in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalReviewSummaryService.cs
- [ ] T204 [US5] Implement MedicalLifecycleTraceService for profile, guardian update, emergency access, offline cache, incident, care action, notification, contact, acknowledgement, exception, review, summary, configuration, status event, and audit trace references in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalLifecycleTraceService.cs
- [ ] T205 [US5] Implement MedicalHistoryController, MedicalExceptionController, ManualMedicalReviewController, MedicalReviewSummaryController, MedicalConfigurationController, and MedicalLifecycleTraceController routes in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalHistoryReviewControllers.cs
- [ ] T206 [US5] Wire history read, exception creation, exception resolution, manual review, correction, reopen, closure, escalation, rule activation, rule suspension, summary read, lifecycle trace read, denied access, and configuration change audit events in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalHistoryReviewAuditAdapter.cs
- [ ] T207 [US5] Update Medical endpoint registration to map school history, guardian history, student history, exceptions, reviews, review summaries, feature settings, rule settings, rule activation/suspension, rule trace, and lifecycle trace routes in apps/api/src/SafeSchool.Api/Features/Medical/MedicalEndpointRegistration.cs
- [ ] T208 [US5] Create OpenAPI examples for history search, guardian history redaction, student history redaction, exception list/detail, manual review, correction, review summary, feature settings, rule create/activate/suspend, invalid break-glass role, invalid cache freshness, lifecycle trace, disabled capability, and access denial in apps/api/src/SafeSchool.Api/Features/Medical/Reviews/MedicalHistoryReviewOpenApiExamples.cs
- [ ] T209 [P] [US5] Create school web Medical History and Review API hooks for history search, exception list/detail, manual review, summaries, feature settings, rule settings, rule trace, lifecycle trace, typed errors, and pagination in apps/admin-web/src/features/medical/reviews/medicalHistoryReviewApi.ts
- [ ] T210 [P] [US5] Create history search filters, history result table, exception queue, exception detail panel, manual review form, correction reason panel, and lifecycle trace panel in apps/admin-web/src/features/medical/reviews/MedicalHistoryReviewPanels.tsx
- [ ] T211 [P] [US5] Create medical configuration panels for capability settings, visibility rules, emergency access rules, break-glass roles, offline cache freshness, incident severity, notification audience, acknowledgement, review routing, rule version display, and unsafe-rule errors in apps/admin-web/src/features/medical/configuration/MedicalConfigurationPanels.tsx
- [ ] T212 [P] [US5] Create medical review summary cards and audit trace summary components for student, incident, emergency access, notification acknowledgement, exception, reviewer, guardian, student, auditor, and platform reviewer scopes in apps/admin-web/src/features/medical/reviews/MedicalReviewSummaryCards.tsx
- [ ] T213 [US5] Implement school Medical History route with filters, result table, exception queue, manual review action, correction preservation display, disabled capability state, access-denied state, and trace links in apps/admin-web/src/app/(school)/medical/history/page.tsx
- [ ] T214 [US5] Implement school Medical Configuration route with rule create/update/activate/suspend, break-glass role validation, 30-minute duration display, 24-hour cache display, rule version trace, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/medical/configuration/page.tsx
- [ ] T215 [US5] Implement school Medical Reviews route with assigned review queue, exception detail, manual review actions, review summaries, lifecycle trace, summary-read audit indicator, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/medical/reviews/page.tsx
- [ ] T216 [US5] Update guardian and student Medical routes to expose allowed history and notification acknowledgement outcomes with staff-only note redaction, disputed record redaction, internal review assignment hiding, and access-denied states in apps/admin-web/src/app/(guardian)/medical/page.tsx and apps/admin-web/src/app/(student)/medical/page.tsx
- [ ] T217 [US5] Create Medical Review test data builder for exceptions, manual reviews, rule settings, review summaries, guardian history, student history, auditor trace, unsafe visibility rule, invalid break-glass role, stale cache exception, duplicate notification exception, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalHistoryReviewTestData.cs
- [ ] T218 [US5] Update MedicalStatusEventWriter to mark eligible medical profile, emergency access, incident, notification, acknowledgement, exception, review, and configuration status changes for later Phase 9 consumption without direct message delivery in apps/api/src/SafeSchool.Api/Features/Medical/Audit/MedicalStatusEventWriter.cs

**Checkpoint**: User Story 5 can be demonstrated independently after T184-T218 pass.

---

## Phase 8: Polish & Cross-Cutting Concerns

**Purpose**: Harden Phase 7 across all stories after the selected story scope
is implemented.

- [ ] T219 [P] Update Medical developer documentation with module boundaries, capability keys, permission names, no-side-effect boundaries, emergency access rules, offline cache rules, notification boundary, and runbook links in docs/medical/README.md
- [ ] T220 [P] Update contract fixture examples for all Phase 7 routes, common errors, idempotency keys, redacted responses, stale cache responses, duplicate responses, conflict review responses, and lifecycle traces in tests/contracts/medical/common-fixtures.md
- [ ] T221 Run backend formatting, linting, unit tests, integration tests, contract tests, migration validation, and coverage checks for Medical features in apps/api/
- [ ] T222 Run admin web linting, type checks, component tests, and Medical route journey tests in apps/admin-web/
- [ ] T223 If an existing mobile client is present, run mobile formatting, analyzer, unit tests, offline cache tests, emergency profile tests, and incident capture tests in apps/mobile/
- [ ] T224 [P] Create end-to-end validation tests for quickstart Medical Records, Emergency Access, Medical Incidents, Medical Notifications, History/Reviews, boundaries, and audit scenarios in tests/e2e/medical/medical-phase7.e2e.ts
- [ ] T225 [P] Create security review checklist for medical privacy, guardian visibility, student visibility, break-glass access, stale cache, audit evidence, tenant isolation, and data minimization in docs/medical/security-review.md
- [ ] T226 Create observability dashboard documentation for emergency access counts, break-glass reviews, stale cache access, incident severity, notification acknowledgements, failed contacts, exceptions, review queues, and denied access in docs/medical/observability.md
- [ ] T227 Create data retention and audit retention notes for medical profiles, incidents, emergency access sessions, offline cache evidence, notifications, contact attempts, exceptions, reviews, summaries, and status events in docs/medical/data-retention.md
- [ ] T228 Verify all Phase 7 records include tenant_id, created_at, updated_at, indexes, idempotency identity where needed, privacy filters, status events, and audit events in apps/api/src/SafeSchool.Api/Features/Medical/
- [ ] T229 Verify Phase 7 creates no attendance, campus gate, NFC/QR scan, transport, wallet, learning reward, request approval, complaint, broad messaging, document, search, diagnosis, prescription, pharmacy, payment, dispatch, or broad dashboard side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Foundational/MedicalPhaseBoundaryGuardTests.cs
- [ ] T230 Verify quickstart validation scenarios for records, emergency access, incidents, notifications, history, exceptions, reviews, boundaries, and audit evidence against specs/008-medical-emergency/quickstart.md
- [ ] T231 Remove temporary seed-only switches, fake providers, debug routes, console logs, unredacted medical fixture payloads, and test-only capability bypasses from apps/api/src/SafeSchool.Api/Features/Medical/, apps/admin-web/src/features/medical/, and apps/mobile/lib/features/medical/ when present
- [ ] T232 Create final Phase 7 implementation summary with completed stories, validation commands, known limitations, deployment notes, and follow-up recommendations in docs/medical/phase7-implementation-summary.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion; blocks all user stories.
- **User Stories (Phase 3+)**: All depend on Foundational completion.
- **Polish (Phase 8)**: Depends on all user stories selected for delivery.

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational; provides verified medical profile source of truth.
- **User Story 2 (P1)**: Can start after Foundational, but is most valuable after US1 profile records exist; remains independently testable with seeded profiles.
- **User Story 3 (P1)**: Can start after Foundational, but uses US1 medication and consent evidence when medication administration is recorded; remains independently testable with seeded records.
- **User Story 4 (P2)**: Depends on US3 for high-severity incident source and may use US2 emergency access source; implement after at least one source is available.
- **User Story 5 (P3)**: Depends on evidence from US1-US4 for full history, review, summary, and trace value; can be partially implemented with seeded records.

### Within Each User Story

- Tests required by the constitution must be written and fail before implementation.
- Domain entities and DTOs before services.
- Services before controllers and route registration.
- Tenant, feature flag, permission, privacy, idempotency, no-side-effect boundary, and audit enforcement before UI exposure.
- Backend routes before web API hooks and any conditional mobile API hooks.
- API hooks before UI pages.
- Story complete before moving to the next priority unless staffing allows parallel work on independent files.

### Parallel Opportunities

- Setup tasks marked `[P]` can run in parallel after T001.
- Foundational adapter, test, web client, conditional mobile client, and fixture tasks marked `[P]` can run in parallel after the common contracts from T014-T021 are agreed.
- User story test tasks marked `[P]` can run in parallel at the start of each story phase.
- Domain entity, DTO, web component, conditional mobile model, and test data builder tasks marked `[P]` can run in parallel inside the same story when they touch separate files.
- US2 and US3 can proceed in parallel after Foundation using seeded US1 records, then integrate with US1 once records are available.
- US4 can proceed in parallel after US3 source contracts stabilize using seeded high-severity incidents.
- US5 can proceed with seeded records but should receive final trace integrations after US1-US4 complete.

---

## Parallel Example: User Story 1

```bash
Task: "T051 Create StudentMedicalProfile domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/StudentMedicalProfileDomainTests.cs"
Task: "T052 Create medical record item domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordItemDomainTests.cs"
Task: "T057 Create contract tests for medical profile routes in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Records/MedicalRecordsContractTests.cs"
Task: "T077 Create school web Medical Records API hooks in apps/admin-web/src/features/medical/records/medicalRecordsApi.ts"
```

## Parallel Example: User Story 2

```bash
Task: "T086 Create EmergencyAccessSession domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessSessionDomainTests.cs"
Task: "T088 Create EmergencyOfflineCacheAccess domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyOfflineCacheAccessDomainTests.cs"
Task: "T093 Create contract tests for emergency access routes in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/EmergencyAccess/EmergencyAccessContractTests.cs"
Task: "T115 If an existing mobile client is present, create mobile emergency access models in apps/mobile/lib/features/medical/emergency/emergency_access_models.dart"
```

## Parallel Example: User Story 3

```bash
Task: "T121 Create MedicalIncident domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentDomainTests.cs"
Task: "T122 Create CareAction domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/CareActionDomainTests.cs"
Task: "T127 Create contract tests for incident routes in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Incidents/MedicalIncidentContractTests.cs"
Task: "T146 Create school web Medical Incident API hooks in apps/admin-web/src/features/medical/incidents/medicalIncidentsApi.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T156 Create MedicalNotificationRequest domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationRequestDomainTests.cs"
Task: "T157 Create MedicalContactAttempt domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalContactAttemptDomainTests.cs"
Task: "T161 Create contract tests for notification routes in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Notifications/MedicalNotificationsContractTests.cs"
Task: "T178 Create school web Medical Notifications API hooks in apps/admin-web/src/features/medical/notifications/medicalNotificationsApi.ts"
```

## Parallel Example: User Story 5

```bash
Task: "T184 Create MedicalException domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalExceptionDomainTests.cs"
Task: "T186 Create MedicalRuleSetting domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Configuration/MedicalRuleSettingDomainTests.cs"
Task: "T192 Create contract tests for history and configuration routes in apps/api/tests/SafeSchool.Api.Tests/Features/Medical/Reviews/MedicalHistoryReviewContractTests.cs"
Task: "T209 Create school web Medical History and Review API hooks in apps/admin-web/src/features/medical/reviews/medicalHistoryReviewApi.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate Medical Records independently with tests and quickstart
   Medical Records scenarios.
5. Demo or deploy only if tenant, feature, permission, privacy, idempotency,
   and audit checks pass.

### Safety-First P1 Delivery

1. Complete Setup and Foundation.
2. Add US1 Medical Records as the verified source of truth.
3. Add US2 Emergency Access with 30-minute sessions, break-glass review, and
   24-hour offline cache rules.
4. Add US3 Medical Incidents and Care Actions with append-only evidence.
5. Validate P1 safety flows before adding notifications or broad review views.

### Incremental Delivery

1. US1 Medical Records - test independently and deploy if ready.
2. US2 Emergency Access - test independently with seeded and real profiles.
3. US3 Medical Incidents - test independently with profile and medication
   evidence.
4. US4 Medical Notifications - test independently with high-severity incident
   sources and no Phase 9 delivery.
5. US5 History, Reviews, and Configuration - test complete lifecycle trace and
   governance views.

### Parallel Team Strategy

1. Team completes Setup and Foundation together.
2. Developer A owns US1 files under Records.
3. Developer B owns US2 files under EmergencyAccess and optional mobile emergency when a mobile client exists.
4. Developer C owns US3 files under Incidents and optional mobile incident capture when a mobile client exists.
5. Developer D owns US4 files under Notifications after US3 source contracts
   stabilize.
6. Developer E owns US5 files under Reviews, History, and Configuration after
   source trace contracts stabilize.

## Notes

- `[P]` tasks touch separate files and can run in parallel when their phase
  prerequisites are met.
- `[US#]` labels map directly to the five user stories in [spec.md](./spec.md).
- Every user story must remain independently testable with seeded data.
- Keep Phase 7 medical notification as request, contact, acknowledgement, and
  status evidence only; do not implement general messaging delivery.
- Commit after each task or logical group.
