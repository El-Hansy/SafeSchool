# Tasks: Phase 2 Attendance & Campus Access

**Input**: Design documents from `/specs/003-attendance-campus-access/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 2 plan require unit, integration, contract, authorization, tenant-isolation, audit, offline sync, mobile, and critical UI journey coverage. Write test tasks before implementation tasks in each user-story phase and confirm they fail for the missing behavior before completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a task is marked `[P]`. Do not implement Phase 3 transport, Phase 4 wallet, Phase 6 requests, Phase 9 general messaging, or broad admin dashboard behavior. Every sensitive action must resolve tenant context, check the Phase 2 feature capability, enforce permission, prevent cross-school visibility, record denied access decisions, and emit audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses described in [plan.md](./plan.md).

- [x] T001 Create or update API solution and AttendanceAccess source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/
- [x] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, and logging dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [x] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [x] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, and test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [x] T005 [P] Create or update mobile package manifest with Flutter test, SQLite local storage, NFC integration, QR scanning, and HTTP client dependencies in apps/mobile/pubspec.yaml
- [x] T006 [P] Create repository coding defaults and generated-file ignores for API, web, mobile, coverage, and local SQLite artifacts in .editorconfig and .gitignore
- [x] T007 Create API bootstrap with versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, and AttendanceAccess endpoint registration placeholders in apps/api/src/SafeSchool.Api/Program.cs
- [x] T008 [P] Create API configuration placeholders for connection strings, JWT, logging, audit, feature flags, scan sync limits, and offline clock drift tolerance in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [x] T009 [P] Create admin web shell route for the Attendance & Campus Access area in apps/admin-web/src/app/(school)/attendance-access/page.tsx and apps/admin-web/src/features/attendance-access/index.ts
- [x] T010 [P] Create mobile AttendanceAccess module export shell in apps/mobile/lib/features/attendance_access/attendance_access.dart
- [x] T011 [P] Create contract test documentation index linking the four Phase 2 contracts in tests/contracts/attendance-access/README.md
- [x] T012 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/attendance-access/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, credential evidence, idempotency, persistence, audit, and client foundations required by every Phase 2 user story.

**Critical**: No user-story implementation should start until this phase is complete.

- [x] T013 Create shared AttendanceAccess tenant-owned entity base, result types, validation error type, paged response type, and clock abstraction in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/OperationResults.cs
- [x] T014 [P] Create shared AttendanceAccess enums for directions, statuses, decisions, scan methods, anomaly types, severity, notification eligibility, and review actions from data-model.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/AttendanceAccessEnums.cs
- [x] T015 Create or extend SafeSchoolDbContext registration for AttendanceAccess entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [x] T016 Create AttendanceAccess model-builder extension for tenant metadata, timestamps, status indexes, idempotency indexes, and foreign key relationships in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/AttendanceAccessDbContextModelBuilderExtensions.cs
- [x] T017 Create Phase 2 capability constants, rule setting model, and feature gate adapter for attendance_access.gate_scanning, attendance_access.attendance_generation, attendance_access.entry_exit_notifications, and attendance_access.anomaly_detection in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/AttendanceAccessCapabilities.cs, apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/AttendanceAccessRuleSettings.cs, and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/AttendanceAccessFeatureGate.cs
- [x] T018 Create AttendanceAccess permission catalog entries for gates, scan points, scans, sync, attendance, notifications, guardian entry/exit, anomalies, and audit review in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/AttendanceAccessPermissionCatalog.cs
- [x] T019 Create AttendanceAccess permission guard that wraps tenant context, feature gate, permission evaluation, access decision writing, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/AttendanceAccessPermissionGuard.cs
- [x] T020 Create IdentityAccess credential snapshot adapter interface and test fake for active, expired, suspended, revoked, replaced, unknown, and cross-tenant credentials in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/IdentityCredentialEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Fixtures/FakeIdentityCredentialEvidenceProvider.cs
- [x] T021 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/GuardianLinkEligibilityProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Fixtures/FakeGuardianLinkEligibilityProvider.cs
- [x] T022 Create IdentityAccess student population adapter interface and test fake for expected attendance populations and cross-tenant student prevention in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/ExpectedStudentPopulationProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Fixtures/FakeExpectedStudentPopulationProvider.cs
- [x] T023 Create idempotency service for client_request_id, client_scan_id, and client_batch_id retry handling in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Common/Idempotency/IdempotencyService.cs
- [x] T024 Create tenant-scoped audit event entity and audit writer adapter for Phase 2 scan, attendance, notification, anomaly, sync, review, feature, and access events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Audit/AttendanceAccessAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Audit/AttendanceAccessAuditWriter.cs
- [x] T025 Create AttendanceAccess route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/attendance-access in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/AttendanceAccessEndpointRegistration.cs
- [x] T026 Create API test fixture for tenants, capabilities, permissions, credential evidence, guardian links, expected students, scan points, idempotency, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Fixtures/AttendanceAccessTestFixture.cs
- [x] T027 [P] Create unit tests for Phase 2 capability decisions and disabled-workflow denial in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Foundational/AttendanceAccessFeatureGateTests.cs
- [x] T028 [P] Create unit tests for permission guard tenant mismatch, missing permission, disabled capability, allowed access, denied access decision, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Foundational/AttendanceAccessPermissionGuardTests.cs
- [x] T029 [P] Create unit tests for idempotency service retry, conflict, and duplicate request behavior in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Foundational/IdempotencyServiceTests.cs
- [x] T030 Create initial EF migration for AttendanceAccess audit, idempotency, shared feature setting, and attendance access rule setting foundations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040001_AttendanceAccessFoundation.cs
- [x] T031 [P] Create admin web AttendanceAccess API client with tenant context, typed errors, pagination, and feature-disabled handling in apps/admin-web/src/features/attendance-access/api/client.ts
- [x] T032 [P] Create mobile AttendanceAccess API client shell with tenant context, auth headers, retry metadata, and offline error mapping in apps/mobile/lib/features/attendance_access/attendance_access_api.dart
- [x] T033 [P] Create mobile SQLite database wrapper and migration registry for offline scan queue tables in apps/mobile/lib/features/attendance_access/local/attendance_access_database.dart
- [x] T034 [P] Create shared API and mobile test data builders for tenants, gates, scan points, students, credentials, guardians, attendance sessions, attendance access rule settings, and anomalies in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Fixtures/AttendanceAccessTestData.cs and apps/mobile/test/features/attendance_access/attendance_access_test_data.dart

**Checkpoint**: Foundation ready. User-story work can start after T013-T034 are complete.

---

## Phase 3: User Story 1 - Record Campus Entry and Exit Scans (Priority: P1)

**Goal**: Authorized gate staff can record online and offline NFC/QR entry and exit scans at authorized gates, with allowed, denied, flagged, duplicate, and needs-review outcomes.

**Independent Test**: Create an active gate and scan point, scan an active credential for entry and exit, deny invalid credentials, submit delayed offline scans twice, and verify tenant-scoped scan evidence and audit traceability without generating attendance.

### Tests for User Story 1

- [x] T035 [P] [US1] Create Gate and ScanPoint lifecycle unit tests for Draft, Active, Suspended, Decommissioned, Pending, Retired, allowed directions, and offline_allowed rules in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Gates/GateAndScanPointTests.cs
- [x] T036 [P] [US1] Create scan validation unit tests for active credential, expired credential, suspended credential, revoked credential, replaced credential, unknown credential, cross-tenant credential, inactive gate, inactive scan point, and disallowed direction in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Scans/ScanValidationServiceTests.cs
- [x] T037 [P] [US1] Create offline sync and duplicate handling unit tests for client_scan_id retry, client_batch_id retry, repeated taps, delayed local time, received time, clock drift, and duplicate attendance prevention signal in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Sync/OfflineScanSyncServiceTests.cs
- [x] T038 [P] [US1] Create contract tests for gate create, gate update, scan point create, scan point update, online scan, offline sync, scan list, scan detail, and scan trace routes from contracts/gate-scan-flow.md in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Scans/GateScanFlowContractTests.cs
- [x] T039 [P] [US1] Create integration tests for tenant isolation, attendance_access.gate_scanning disabled capability, missing scan permission, scan point authorization, invalid credential denial, duplicate scan idempotency, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Scans/GateScanFlowIntegrationTests.cs
- [x] T040 [P] [US1] Create mobile offline queue tests for enqueue, read pending, retry after failure, mark synced, duplicate client_scan_id, and local time preservation in apps/mobile/test/features/attendance_access/offline_scan_queue_test.dart
- [x] T041 [P] [US1] Create mobile scan capture tests for NFC scan, QR scan, direction selection, offline mode, source metadata, and scan sync request mapping in apps/mobile/test/features/attendance_access/gate_scan_capture_test.dart
- [x] T042 [P] [US1] Create admin web journey tests for gate management, scan point management, scan event review, denied scan review, and trace view in apps/admin-web/tests/attendance-access/gate-scan-flow.spec.ts

### Implementation for User Story 1

- [x] T043 [P] [US1] Create Gate and ScanPoint domain models with validation helpers in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/Gate.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/ScanPoint.cs
- [x] T044 [P] [US1] Create Gate, ScanPoint, ScanEvent, OfflineSync, CampusAccessDecision, and ScanTrace DTOs matching contracts/gate-scan-flow.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/GateScanFlowDtos.cs
- [x] T045 [US1] Create EF configurations and migration for gates, scan points, scan events, offline sync batches, campus access decisions, tenant indexes, scan idempotency indexes, and scan trace indexes in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/GateScanEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040002_GateScanFlow.cs
- [x] T046 [US1] Implement GateService for create, update, list, read, status changes, allowed directions, and audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/GateService.cs
- [x] T047 [US1] Implement ScanPointService for register, update, suspend, retire, assigned actor, device reference, offline_allowed, and gate authorization behavior in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/ScanPointService.cs
- [x] T048 [US1] Implement ScanValidationService for tenant, feature, permission, gate, scan point, direction, credential evidence, QR fallback enablement, and cross-school denial rules in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanValidationService.cs
- [x] T049 [US1] Implement CampusAccessDecisionService for allowed, denied, flagged, needs-review decisions and campus_state_after calculation in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/CampusAccessDecisionService.cs
- [x] T050 [US1] Implement OfflineScanSyncService for client_batch_id idempotency, client_scan_id idempotency, delayed scan reconciliation, duplicate detection, local time preservation, received time preservation, and audit outcomes in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Sync/OfflineScanSyncService.cs
- [x] T051 [US1] Implement ScanTraceService that returns scan, campus access decision, attendance, notification, anomaly, manual review, and audit references when present in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanTraceService.cs
- [x] T052 [US1] Implement GatesController and ScanPointsController routes from contracts/gate-scan-flow.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/GatesController.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Gates/ScanPointsController.cs
- [x] T053 [US1] Implement ScanEventsController routes for online scan, offline sync, scan list, scan detail, and scan trace in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanEventsController.cs
- [x] T054 [US1] Wire gate, scan point, scan capture, denied scan, flagged scan, offline sync, duplicate scan, and trace audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/GateScanAuditAdapter.cs
- [x] T055 [P] [US1] Create admin web gate and scan API hooks in apps/admin-web/src/features/attendance-access/gates/gatesApi.ts and apps/admin-web/src/features/attendance-access/scans/scansApi.ts
- [x] T056 [P] [US1] Create admin web gate form, scan point form, scan event table, scan decision badge, and scan trace panel components in apps/admin-web/src/features/attendance-access/gates/GateForm.tsx and apps/admin-web/src/features/attendance-access/scans/ScanEventTable.tsx
- [x] T057 [US1] Create admin web gate and scan review routes in apps/admin-web/src/app/(school)/attendance-access/gates/page.tsx and apps/admin-web/src/app/(school)/attendance-access/scans/page.tsx
- [x] T058 [US1] Implement mobile scan event model, offline queue entity, and sync response model in apps/mobile/lib/features/attendance_access/scans/gate_scan_event.dart and apps/mobile/lib/features/attendance_access/scans/offline_scan_sync_models.dart
- [x] T059 [US1] Implement SQLite-backed offline scan queue with enqueue, pending list, mark synced, mark rejected, duplicate client_scan_id detection, and migration in apps/mobile/lib/features/attendance_access/scans/offline_scan_queue.dart
- [x] T060 [US1] Implement mobile NFC and QR reader adapter interfaces with fakeable implementations for tests in apps/mobile/lib/features/attendance_access/scans/scan_reader_adapter.dart
- [x] T061 [US1] Implement mobile GateScanRepository for online scan submission, offline fallback, batch sync, retry, and typed error mapping in apps/mobile/lib/features/attendance_access/scans/gate_scan_repository.dart
- [x] T062 [US1] Implement mobile gate scan screen with entry/exit selection, scan result state, offline indicator, retry action, and audit-safe display fields in apps/mobile/lib/features/attendance_access/scans/gate_scan_screen.dart
- [x] T063 [US1] Add OpenAPI examples for gate, scan point, online scan, offline sync, scan list, scan detail, trace, and error outcomes in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/GateScanOpenApiExamples.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T035-T063 pass.

---

## Phase 4: User Story 2 - Generate Attendance From Gate Evidence (Priority: P1)

**Goal**: Attendance reviewers can configure attendance sessions, generate attendance from accepted gate scans, review attendance statuses, correct records, and view summaries.

**Independent Test**: Seed accepted scan evidence for an attendance day, generate Present, Late, Absent, Early Exit, and Needs Review records, correct one record with a reason, re-run generation, and verify no duplicate current attendance record exists.

### Tests for User Story 2

- [x] T064 [P] [US2] Create AttendanceSession lifecycle and time-window unit tests for Draft, Active, Generated, Reopened, Closed, invalid window ordering, school time context, and expected population validation in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceSessionTests.cs
- [x] T065 [P] [US2] Create AttendanceRuleEvaluator unit tests for Present, Late, Absent, Early Exit, Needs Review, Excused correction, delayed offline scan handling, and denied scan exclusion in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceRuleEvaluatorTests.cs
- [x] T066 [P] [US2] Create attendance generation idempotency unit tests for repeated generation requests, same session/student uniqueness, rule version changes, and duplicate accepted scan evidence in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceGenerationServiceTests.cs
- [x] T067 [P] [US2] Create attendance correction unit tests for authorized correction, missing reason rejection, original scan-based decision preservation, and audit event creation in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceCorrectionServiceTests.cs
- [x] T068 [P] [US2] Create contract tests for attendance session create/update/list, generate, attendance record list/detail, correction, and summary routes from contracts/attendance-generation.md in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceGenerationContractTests.cs
- [x] T069 [P] [US2] Create integration tests for tenant isolation, attendance_access.attendance_generation disabled capability, missing generate permission, missing correct permission, denied scan exclusion, delayed offline scan inclusion, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceGenerationIntegrationTests.cs
- [x] T070 [P] [US2] Create admin web journey tests for session setup, generation, attendance review filters, correction, summary, and trace link back to scans in apps/admin-web/tests/attendance-access/attendance-generation.spec.ts

### Implementation for User Story 2

- [x] T071 [P] [US2] Create AttendanceSession and AttendanceRecord domain models with state transition helpers in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceSession.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceRecord.cs
- [x] T072 [P] [US2] Create attendance session, generation, record, correction, and summary DTOs matching contracts/attendance-generation.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceDtos.cs
- [x] T073 [US2] Create EF configurations and migration for attendance sessions, attendance records, session/student uniqueness, expected population references, status indexes, and scan evidence references in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040003_AttendanceGeneration.cs
- [x] T074 [US2] Implement AttendanceSessionService for create, update, list, read, reopen, close, window validation, active attendance access rule setting selection, and expected population validation in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceSessionService.cs
- [x] T075 [US2] Implement AttendanceRuleEvaluator for Present, Late, Absent, Early Exit, Needs Review, delayed offline inclusion, denied scan exclusion, and active rule setting application in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceRuleEvaluator.cs
- [x] T076 [US2] Implement AttendanceGenerationService for accepted scan lookup, expected population expansion, idempotent generation, current record upsert, and scan evidence preservation in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceGenerationService.cs
- [x] T077 [US2] Implement AttendanceCorrectionService for reviewer correction, reason validation, original scan-based decision preservation, and manual review linkage in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceCorrectionService.cs
- [x] T078 [US2] Implement AttendanceSummaryService for group, student, gate, attendance day, status, and anomaly-status summaries without cross-tenant exposure in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceSummaryService.cs
- [x] T079 [US2] Implement AttendanceSessionsController, AttendanceRecordsController, and AttendanceSummaryController routes from contracts/attendance-generation.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceControllers.cs
- [x] T080 [US2] Wire attendance session, generation, correction, denied generation, and summary audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceAuditAdapter.cs
- [x] T081 [P] [US2] Create admin web attendance API hooks for sessions, generation, records, corrections, summaries, and typed errors in apps/admin-web/src/features/attendance-access/attendance/attendanceApi.ts
- [x] T082 [P] [US2] Create admin web attendance session form, generation action panel, attendance record table, status badge, correction dialog, and summary cards in apps/admin-web/src/features/attendance-access/attendance/AttendanceSessionForm.tsx and apps/admin-web/src/features/attendance-access/attendance/AttendanceRecordTable.tsx
- [x] T083 [US2] Create admin web attendance route for sessions, generation, records, summaries, corrections, and scan trace links in apps/admin-web/src/app/(school)/attendance-access/attendance/page.tsx
- [x] T084 [US2] Extend ScanTraceService to include attendance record and correction references after US2 records exist in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanTraceService.cs
- [x] T085 [US2] Create attendance test data builder for sessions, expected populations, accepted scans, delayed offline scans, and correction cases in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Attendance/AttendanceTestData.cs

**Checkpoint**: User Story 2 can be demonstrated independently after T064-T085 pass.

---

## Phase 5: User Story 3 - Notify Guardians About Entry and Exit (Priority: P2)

**Goal**: Eligible guardians receive or can view linked-student entry/exit notification records, while ineligible guardians and unresolved scans are suppressed with reviewable reasons.

**Independent Test**: Capture an eligible entry scan for a student with an approved guardian link, create a visible notification record, suppress records for inactive or out-of-scope links, withdraw a corrected scan notification, and verify guardian read scope.

### Tests for User Story 3

- [x] T086 [P] [US3] Create notification eligibility unit tests for approved active link, pending link, suspended link, expired link, removed link, rejected link, out-of-scope link, disabled capability, denied scan, and needs-review scan in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Notifications/EntryExitNotificationEligibilityTests.cs
- [x] T087 [P] [US3] Create notification record lifecycle unit tests for Eligible, Suppressed, Visible, Attempted, Failed, Withdrawn, suppression reason, guardian visible time, and corrected scan updates in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Notifications/EntryExitNotificationRecordTests.cs
- [x] T088 [P] [US3] Create contract tests for notification list, notification detail, student notification review, guardian me list, and withdraw routes from contracts/entry-exit-notification.md in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Notifications/EntryExitNotificationContractTests.cs
- [x] T089 [P] [US3] Create integration tests for guardian link visibility scope, tenant isolation, guardian requesting another student, disabled notification capability, missing review permission, unresolved scan suppression, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Notifications/EntryExitNotificationIntegrationTests.cs
- [x] T090 [P] [US3] Create admin web notification review tests for filters, suppression reasons, detail view, withdrawal, and corrected scan visibility in apps/admin-web/tests/attendance-access/entry-exit-notification.spec.ts
- [x] T091 [P] [US3] Create guardian-facing entry/exit visibility tests for linked students only, visible records only, suppressed records hidden, and tenant isolation in apps/admin-web/tests/attendance-access/guardian-entry-exit.spec.ts

### Implementation for User Story 3

- [x] T092 [P] [US3] Create EntryExitNotificationRecord domain model with eligibility status transitions and suppression reason validation in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationRecord.cs
- [x] T093 [P] [US3] Create entry/exit notification DTOs matching contracts/entry-exit-notification.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationDtos.cs
- [x] T094 [US3] Create EF configuration and migration for entry/exit notification records, guardian link references, scan references, attendance references, visibility indexes, and suppression indexes in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040004_EntryExitNotifications.cs
- [x] T095 [US3] Implement EntryExitNotificationEligibilityService for guardian link scope, feature capability, scan decision, attendance outcome, active rule setting application, suppression reason, and guardian-visible time behavior in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationEligibilityService.cs
- [x] T096 [US3] Implement EntryExitNotificationService for creating visible or suppressed records from entry/exit scans, updating records after corrected scans, withdrawing records, and idempotent notification creation in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationService.cs
- [x] T097 [US3] Implement GuardianEntryExitVisibilityService for guardian me list, linked-student filtering, access scope enforcement, and no cross-student leakage in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/GuardianEntryExitVisibilityService.cs
- [x] T098 [US3] Implement NotificationRecordsController and GuardianEntryExitController routes from contracts/entry-exit-notification.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/NotificationRecordsController.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/GuardianEntryExitController.cs
- [x] T099 [US3] Wire notification eligibility, suppression, visibility, delivery attempt, withdrawal, corrected scan update, and access denial audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationAuditAdapter.cs
- [x] T100 [US3] Integrate EntryExitNotificationService with allowed entry/exit scan outcomes and attendance correction updates without adding general messaging behavior in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Notifications/EntryExitNotificationWorkflowHooks.cs
- [x] T101 [P] [US3] Create admin web notification API hooks and guardian entry/exit API hooks in apps/admin-web/src/features/attendance-access/notifications/notificationsApi.ts and apps/admin-web/src/features/attendance-access/guardian-entry-exit/guardianEntryExitApi.ts
- [x] T102 [P] [US3] Create admin web notification record table, suppression reason badge, notification detail panel, and withdraw dialog in apps/admin-web/src/features/attendance-access/notifications/NotificationRecordTable.tsx and apps/admin-web/src/features/attendance-access/notifications/NotificationDetailPanel.tsx
- [x] T103 [US3] Create admin web notification review route in apps/admin-web/src/app/(school)/attendance-access/notifications/page.tsx
- [x] T104 [P] [US3] Create guardian-facing entry/exit list, student filter, direction badge, and time display components in apps/admin-web/src/features/attendance-access/guardian-entry-exit/GuardianEntryExitList.tsx
- [x] T105 [US3] Create guardian-facing entry/exit route with linked-student-only visibility in apps/admin-web/src/app/(guardian)/entry-exit/page.tsx
- [x] T106 [US3] Extend ScanTraceService to include notification record and suppression references after US3 records exist in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanTraceService.cs

**Checkpoint**: User Story 3 can be demonstrated independently after T086-T106 pass.

---

## Phase 6: User Story 4 - Detect and Resolve Attendance Anomalies (Priority: P3)

**Goal**: Attendance reviewers can detect, assign, resolve, dismiss, reopen, and trace anomalies without silently changing attendance records.

**Independent Test**: Create duplicate, missing, invalid, out-of-order, conflicting, delayed offline, late, and early cases, run detection, resolve or dismiss anomalies with reasons, reopen one anomaly, and verify attendance correction remains a separate action.

### Tests for User Story 4

- [x] T107 [P] [US4] Create anomaly classification unit tests for missing entry, missing exit, duplicate scan, invalid credential, out-of-order scan, conflicting campus state, delayed offline conflict, late arrival, early exit, and manual-review-required scan in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AnomalyDetectionRuleTests.cs
- [x] T108 [P] [US4] Create anomaly lifecycle unit tests for New, Assigned, In Review, Resolved, Dismissed, Escalated, Reopened, required reviewer, required reason, and invalid transitions in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AttendanceAnomalyLifecycleTests.cs
- [x] T109 [P] [US4] Create anomaly run idempotency tests for duplicate detection runs, same evidence updates, resolved anomaly retry, and no duplicate open anomalies in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AnomalyDetectionRunTests.cs
- [x] T110 [P] [US4] Create contract tests for anomaly runs, anomaly list, anomaly detail, assign, resolve, dismiss, and reopen routes from contracts/anomaly-detection.md in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AnomalyDetectionContractTests.cs
- [x] T111 [P] [US4] Create integration tests for tenant isolation, disabled anomaly capability, missing anomaly permission, evidence from another tenant, resolution without attendance correction, audit evidence, and trace updates in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AnomalyDetectionIntegrationTests.cs
- [x] T112 [P] [US4] Create admin web anomaly review tests for detection run, filters, detail, assignment, resolution, dismissal, reopening, and correction handoff in apps/admin-web/tests/attendance-access/anomaly-detection.spec.ts

### Implementation for User Story 4

- [x] T113 [P] [US4] Create AttendanceAnomaly and ManualReview domain models with state transition helpers in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AttendanceAnomaly.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Reviews/ManualReview.cs
- [x] T114 [P] [US4] Create anomaly detection, anomaly record, assignment, resolution, dismissal, reopening, and manual review DTOs matching contracts/anomaly-detection.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyDtos.cs and apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Reviews/ManualReviewDtos.cs
- [x] T115 [US4] Create EF configurations and migration for attendance anomalies, manual reviews, evidence references, anomaly status indexes, reviewer indexes, and idempotent detection run records in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040005_AttendanceAnomalies.cs
- [x] T116 [US4] Implement AnomalyDetectionRuleSet for missing entry, missing exit, duplicate scan, invalid credential, out-of-order scan, conflicting campus state, delayed offline conflict, late arrival, early exit, manual-review-required scan detection, and active rule setting thresholds in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyDetectionRuleSet.cs
- [x] T117 [US4] Implement AnomalyDetectionService for scoped evidence loading, idempotent detection runs, severity assignment, existing anomaly updates, and audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyDetectionService.cs
- [x] T118 [US4] Implement AnomalyReviewService for assign, resolve, dismiss, reopen, reason validation, reviewer permission checks, and no-silent-attendance-correction enforcement in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyReviewService.cs
- [x] T119 [US4] Implement ManualReviewService for review actions on scan, attendance, notification, anomaly, and campus access decision targets with reason and audit history in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Reviews/ManualReviewService.cs
- [x] T120 [US4] Implement AnomalyRunsController and AnomaliesController routes from contracts/anomaly-detection.md in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomaliesController.cs
- [x] T121 [US4] Wire anomaly detection, assignment, resolution, dismissal, reopening, escalation, manual review, and denied action audit events in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Anomalies/AnomalyAuditAdapter.cs
- [x] T122 [US4] Extend AttendanceCorrectionService to accept optional anomaly/manual review linkage while preserving explicit correction behavior in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Attendance/AttendanceCorrectionService.cs
- [x] T123 [P] [US4] Create admin web anomaly API hooks and manual review API hooks in apps/admin-web/src/features/attendance-access/anomalies/anomaliesApi.ts and apps/admin-web/src/features/attendance-access/reviews/manualReviewsApi.ts
- [x] T124 [P] [US4] Create admin web anomaly run form, anomaly table, severity badge, status badge, anomaly detail panel, assign dialog, resolve dialog, dismiss dialog, and reopen action components in apps/admin-web/src/features/attendance-access/anomalies/AnomalyRunForm.tsx and apps/admin-web/src/features/attendance-access/anomalies/AnomalyTable.tsx
- [x] T125 [US4] Create admin web anomaly review route with filters, detail, assignment, resolution, dismissal, reopening, and correction handoff in apps/admin-web/src/app/(school)/attendance-access/anomalies/page.tsx
- [x] T126 [US4] Extend ScanTraceService to include anomaly and manual review references after US4 records exist in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/Scans/ScanTraceService.cs
- [x] T127 [US4] Create anomaly test data builder for missing, duplicate, invalid, out-of-order, conflicting, delayed offline, late, early, and correction-handoff cases in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Anomalies/AnomalyTestData.cs

**Checkpoint**: User Story 4 can be demonstrated independently after T107-T127 pass.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full Phase 2 implementation, harden security and observability, and preserve traceability for later phases.

- [x] T128 [P] Generate or update OpenAPI documentation for all AttendanceAccess routes in apps/api/src/SafeSchool.Api/Features/AttendanceAccess/AttendanceAccessOpenApi.md
- [x] T129 [P] Create end-to-end tenant isolation and feature-disabled scenarios across gates, scans, attendance, notifications, anomalies, manual reviews, audit, and access decisions in tests/e2e/attendance-access/attendance-access.e2e.spec.ts
- [x] T130 [P] Create security review checklist for tenant boundaries, feature gates, permissions, guardian visibility, credential evidence, offline sync idempotency, cross-tenant not-found behavior, and audit failure behavior in docs/attendance-access/security-review.md
- [x] T131 [P] Create observability review for scan sync metrics, scan denial metrics, attendance generation metrics, notification suppression metrics, anomaly metrics, audit event categories, logs, and centralized error reporting in docs/attendance-access/observability.md
- [x] T132 [P] Create Phase 2 traceability matrix mapping FR-001 through FR-021 and SC-001 through SC-008 to implemented tests and files in docs/attendance-access/traceability.md
- [x] T133 [P] Create offline scan operations runbook covering scan point setup, local queue retry, delayed sync review, duplicate scan review, clock drift review, and failed sync escalation in docs/attendance-access/offline-scan-runbook.md
- [x] T134 Run API test suite and record relevant AttendanceAccess output in docs/attendance-access/backend-test-results.md
- [x] T135 Run admin web test suite and record relevant AttendanceAccess output in docs/attendance-access/admin-web-test-results.md
- [x] T136 Run mobile test suite and record relevant AttendanceAccess output in docs/attendance-access/mobile-test-results.md
- [x] T137 Run contract and end-to-end validation for AttendanceAccess and record output in docs/attendance-access/contract-e2e-test-results.md
- [x] T138 Run quickstart.md validation scenarios end-to-end and record pass/fail evidence in docs/attendance-access/quickstart-validation.md
- [x] T139 Verify Phase 2 excludes transport, wallet, requests, medical, complaints, general messaging, documents, search, broad dashboards, and physical gate hardware control in docs/attendance-access/scope-boundary-review.md
- [x] T140 [P] Create SC-001 gate scan latency tests for online scan, offline fallback, and mobile scan capture under 10 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Performance/GateScanLatencyTests.cs and apps/mobile/test/features/attendance_access/gate_scan_latency_test.dart
- [x] T141 [P] Create SC-004 attendance generation latency tests for 95% of accepted entry scans producing or updating attendance records within 2 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Performance/AttendanceGenerationLatencyTests.cs
- [x] T142 [P] Create SC-005 entry/exit notification latency tests for 95% of eligible notification records becoming created or visible within 2 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Performance/EntryExitNotificationLatencyTests.cs
- [x] T143 [P] Create SC-007 scan trace latency tests for reviewers tracing scan outcomes in under 60 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/AttendanceAccess/Performance/ScanTraceLatencyTests.cs and apps/admin-web/tests/attendance-access/scan-trace-latency.spec.ts

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1 completion and blocks all user stories.
- **Phase 3 US1**: Depends on Phase 2. Delivers the MVP gate scan flow.
- **Phase 4 US2**: Depends on Phase 2 and consumes accepted scan evidence. It can be tested with seeded accepted scan events, but production delivery should follow US1.
- **Phase 5 US3**: Depends on Phase 2 and consumes scan outcomes plus Phase 1 guardian link evidence. It can be tested with seeded scan outcomes and guardian link fakes, but production delivery should follow US1.
- **Phase 6 US4**: Depends on Phase 2 and consumes scan and attendance evidence. It can be tested with seeded evidence, but production delivery is most useful after US1 and US2.
- **Phase 7 Polish**: Depends on all desired user stories being complete.

### User Story Dependencies

- **US1 Record Campus Entry and Exit Scans (P1)**: Can start after Phase 2. This is the suggested MVP because it creates trusted gate scan evidence.
- **US2 Generate Attendance From Gate Evidence (P1)**: Can start after Phase 2 with seeded accepted scan evidence, but real workflow value depends on US1 scan capture.
- **US3 Notify Guardians About Entry and Exit (P2)**: Can start after Phase 2 with seeded scan outcomes and Phase 1 guardian link adapters, but real workflow value depends on US1.
- **US4 Detect and Resolve Attendance Anomalies (P3)**: Can start after Phase 2 with seeded evidence, but full anomaly value depends on US1 and US2.

### Within Each User Story

- Create tests before implementation and confirm they fail for missing behavior.
- Create domain models before EF configuration.
- Create EF configuration and migrations before services that persist records.
- Create services before controllers.
- Create API hooks before UI pages.
- Wire tenant, capability, permission, idempotency, access-decision, and audit behavior before exposing UI or mobile actions.
- Keep out-of-scope modules out of Phase 2 unless the spec is amended.

---

## Parallel Execution Examples

### Setup

```text
Run together after T001:
- T002 API project manifest
- T003 API test project manifest
- T004 admin web manifests
- T005 mobile manifest
- T008 API configuration placeholders
- T009 admin web shell
- T010 mobile module shell
- T011 contract test documentation index
- T012 implementation README
```

### Foundational

```text
Run together after T013-T019 establish shared guards:
- T020 credential evidence provider fake
- T021 guardian link provider fake
- T022 expected student provider fake
- T027 feature gate tests
- T028 permission guard tests
- T029 idempotency tests
- T031 admin web API client
- T032 mobile API client shell
- T033 mobile SQLite wrapper
```

### User Story 1

```text
Run together after Phase 2:
- T035 Gate and ScanPoint lifecycle tests
- T036 scan validation tests
- T037 offline sync tests
- T038 contract tests
- T039 integration tests
- T040 mobile offline queue tests
- T041 mobile scan capture tests
- T042 admin web journey tests

Run together after test files exist:
- T043 Gate and ScanPoint domain models
- T044 Gate Scan Flow DTOs
- T055 admin web gate and scan API hooks
- T056 admin web gate and scan components
- T058 mobile scan models
```

### User Story 2

```text
Run together after Phase 2:
- T064 AttendanceSession lifecycle tests
- T065 AttendanceRuleEvaluator tests
- T066 generation idempotency tests
- T067 correction tests
- T068 contract tests
- T069 integration tests
- T070 admin web journey tests

Run together after test files exist:
- T071 AttendanceSession and AttendanceRecord domain models
- T072 attendance DTOs
- T081 admin web attendance API hooks
- T082 admin web attendance components
```

### User Story 3

```text
Run together after Phase 2:
- T086 notification eligibility tests
- T087 notification record lifecycle tests
- T088 contract tests
- T089 integration tests
- T090 admin web notification review tests
- T091 guardian-facing visibility tests

Run together after test files exist:
- T092 EntryExitNotificationRecord model
- T093 notification DTOs
- T101 admin and guardian API hooks
- T102 notification review components
- T104 guardian-facing components
```

### User Story 4

```text
Run together after Phase 2:
- T107 anomaly classification tests
- T108 anomaly lifecycle tests
- T109 anomaly run idempotency tests
- T110 contract tests
- T111 integration tests
- T112 admin web anomaly review tests

Run together after test files exist:
- T113 AttendanceAnomaly and ManualReview domain models
- T114 anomaly and review DTOs
- T123 anomaly and manual review API hooks
- T124 anomaly review components
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate online scan, denied scan, offline sync retry, duplicate scan, tenant isolation, feature gate denial, permission denial, audit evidence, mobile offline queue, and admin scan review independently.
5. Demo gate scan evidence before adding attendance generation.

### Incremental Delivery

1. Setup plus Foundational creates the shared AttendanceAccess platform.
2. Add US1 to record trusted gate scan evidence.
3. Add US2 to generate attendance from accepted scan evidence.
4. Add US3 to expose eligible guardian entry/exit notification records.
5. Add US4 to detect and resolve anomalies across scan and attendance evidence.
6. Run Phase 7 polish after each selected delivery slice, then fully after all stories.

### Parallel Team Strategy

With multiple implementers:

1. Complete Setup and Foundational together.
2. Assign US1 and US2 to separate implementers only after agreeing on accepted scan evidence contracts.
3. Assign US3 after guardian link adapter behavior is stable.
4. Assign US4 after scan and attendance evidence builders are stable.
5. Keep API, web, and mobile changes for the same story coordinated through that story's contract tests.

---

## Notes

- `[P]` tasks touch different files and can run in parallel after their phase prerequisites are complete.
- `[US1]`, `[US2]`, `[US3]`, and `[US4]` labels map directly to the four user stories in [spec.md](./spec.md).
- Every user story is independently testable using either real prior-story output or seeded/fake evidence from the foundational test fixtures.
- Every controller must remain thin; put business rules in services under apps/api/src/SafeSchool.Api/Features/AttendanceAccess/.
- Every list route must be tenant-scoped, paginated, and filtered without exposing cross-tenant existence.
- Every retry-safe command must use caller-stable request, scan, or batch identity before writing records.
- Latency validation tasks T140-T143 must be completed before claiming SC-001, SC-004, SC-005, or SC-007.
- Commit after each task or small logical group when implementing.
