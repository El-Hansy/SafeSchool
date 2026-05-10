# Tasks: Phase 3 Transport & Bus Tracking

**Input**: Design documents from `/specs/004-transport-bus-tracking/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 3 plan require unit, integration, contract, authorization, tenant-isolation, audit, offline sync, mobile, retention, and critical UI journey coverage. Write test tasks before implementation tasks in each user-story phase and confirm they fail for the missing behavior before completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a task is marked `[P]`. Do not implement Phase 2 attendance or campus access outcomes, Phase 4 wallet, Phase 6 requests, Phase 9 general messaging, Phase 10 document/search, Phase 11 broad dashboards, dedicated bus GPS hardware, or physical vehicle control. Every sensitive action must resolve tenant context, check the required Phase 3 capability, enforce permission, prevent cross-school visibility, record denied access decisions, and emit audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses described in [plan.md](./plan.md).

- [X] T001 Create or update API solution and Transport source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/Transport/
- [X] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, logging, and background worker dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [X] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, time provider fakes, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [X] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, test, and route test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [X] T005 [P] Create or update mobile package manifest with Flutter test, SQLite local storage, NFC integration, QR scanning, HTTP client, location, and permission dependencies in apps/mobile/pubspec.yaml
- [X] T006 [P] Create repository coding defaults and generated-file ignores for API, web, mobile, coverage, local SQLite, and location fixture artifacts in .editorconfig and .gitignore
- [X] T007 Create API bootstrap with versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, background worker registration, and Transport endpoint registration placeholders in apps/api/src/SafeSchool.Api/Program.cs
- [X] T008 [P] Create API configuration placeholders for connection strings, JWT, logging, audit, feature flags, scan sync limits, mobile tracking limits, location staleness, retention, and clock drift tolerance in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [X] T009 [P] Create admin web school transport shell route, settings route placeholder, review-summary route placeholder, and feature export in apps/admin-web/src/app/(school)/transport/page.tsx, apps/admin-web/src/app/(school)/transport/settings/page.tsx, apps/admin-web/src/app/(school)/transport/review/page.tsx, and apps/admin-web/src/features/transport/index.ts
- [X] T010 [P] Create guardian transport shell route and feature export in apps/admin-web/src/app/(guardian)/transport/page.tsx and apps/admin-web/src/features/guardian-transport/index.ts
- [X] T011 [P] Create mobile Transport module export shell in apps/mobile/lib/features/transport/transport.dart
- [X] T012 [P] Create contract test documentation index linking the eight Phase 3 contracts in tests/contracts/transport/README.md
- [X] T013 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/transport/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, identity evidence, guardian visibility, idempotency, persistence, audit, retention, and client foundations required by every Phase 3 user story.

**Critical**: No user-story implementation should start until this phase is complete.

- [X] T014 Create shared Transport tenant-owned entity base, result types, validation error type, paged response type, time provider abstraction, and source metadata type in apps/api/src/SafeSchool.Api/Features/Transport/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/Transport/Common/OperationResults.cs
- [X] T015 [P] Create shared Transport enums for vehicle, route, stop, assignment, trip, scan, sync, location, ETA, notification, anomaly, review, retention, and feature states from data-model.md in apps/api/src/SafeSchool.Api/Features/Transport/Common/TransportEnums.cs
- [X] T016 Create or extend SafeSchoolDbContext registration for Transport entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [X] T017 Create Transport model-builder extension for tenant metadata, timestamps, idempotency indexes, active-trip indexes, guardian visibility indexes, retention indexes, and foreign key relationships in apps/api/src/SafeSchool.Api/Features/Transport/TransportDbContextModelBuilderExtensions.cs
- [X] T018 Create Phase 3 capability constants for transport.bus_assignment, transport.route_stop_management, transport.live_tracking, transport.boarding_drop_scans, transport.eta_calculation, and transport.notifications in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/TransportCapabilities.cs
- [X] T019 Create Transport rule setting model, rule setting DTOs, and feature gate adapter for assignment, pickup/drop windows, route deviation, location staleness, ETA change, notification, anomaly, clock drift, retry, and 30-day retention rules in apps/api/src/SafeSchool.Api/Features/Transport/Rules/TransportRuleSetting.cs, apps/api/src/SafeSchool.Api/Features/Transport/Rules/TransportRuleSettingDtos.cs, and apps/api/src/SafeSchool.Api/Features/Transport/Common/TransportFeatureGate.cs
- [X] T020 Create Transport permission catalog entries for vehicles, routes, assignments, trips, scans, scan review, sync, location submit, tracking read, ETA, notifications, guardian visibility, anomalies, rules, review summaries, and audit review in apps/api/src/SafeSchool.Api/Features/Transport/Common/TransportPermissionCatalog.cs
- [X] T021 Create Transport permission guard that wraps tenant context, feature gate, permission evaluation, guardian scope, access decision writing, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/Transport/Common/TransportPermissionGuard.cs
- [X] T022 Create IdentityAccess student profile adapter interface and test fake for active, inactive, missing, and cross-tenant student profiles in apps/api/src/SafeSchool.Api/Features/Transport/Common/Identity/StudentProfileEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/FakeStudentProfileEvidenceProvider.cs
- [X] T023 Create IdentityAccess credential snapshot adapter interface and test fake for active, expired, suspended, revoked, replaced, unknown, duplicated, and cross-tenant NFC/QR credentials in apps/api/src/SafeSchool.Api/Features/Transport/Common/Identity/TransportCredentialEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/FakeTransportCredentialEvidenceProvider.cs
- [X] T024 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/Transport/Common/Identity/TransportGuardianLinkProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/FakeTransportGuardianLinkProvider.cs
- [X] T025 Create AttendanceAccess boundary adapter interface that exposes campus context for separation only and blocks attendance/campus-access mutation from Transport in apps/api/src/SafeSchool.Api/Features/Transport/Common/Boundaries/AttendanceAccessBoundary.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/FakeAttendanceAccessBoundary.cs
- [X] T026 Create Transport idempotency service for client_request_id, client_scan_id, client_batch_id, client_location_id, anomaly run, review, and notification retry handling in apps/api/src/SafeSchool.Api/Features/Transport/Common/Idempotency/TransportIdempotencyService.cs
- [X] T027 Create Transport audit event entity and audit writer adapter for route, vehicle, assignment, trip, scan, sync, location, ETA, notification, anomaly, review, retention, feature, and access events in apps/api/src/SafeSchool.Api/Features/Transport/Audit/TransportAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/Transport/Audit/TransportAuditWriter.cs
- [X] T028 Create Transport trace reference type for route, assignment, trip, scan, location, ETA, notification, anomaly, review, and audit links in apps/api/src/SafeSchool.Api/Features/Transport/Common/Trace/TransportTraceReference.cs
- [X] T029 Create Transport route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/transport and guardian /api/v1/guardians/me/students/{studentProfileId}/transport routes in apps/api/src/SafeSchool.Api/Features/Transport/TransportEndpointRegistration.cs
- [X] T030 Create API test fixture for tenants, capabilities, permissions, students, credentials, guardian links, route plans, vehicles, trips, devices, idempotency, rule settings, review summaries, retention, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/TransportTestFixture.cs
- [X] T031 [P] Create unit tests for Phase 3 capability decisions and disabled-workflow denial in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Foundational/TransportFeatureGateTests.cs
- [X] T032 [P] Create unit tests for TransportPermissionGuard tenant mismatch, guardian scope mismatch, missing permission, disabled capability, allowed access, denied access decision, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Foundational/TransportPermissionGuardTests.cs
- [X] T033 [P] Create unit tests for TransportIdempotencyService retry, conflict, duplicate request, duplicate scan, duplicate location, and duplicate anomaly run behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Foundational/TransportIdempotencyServiceTests.cs
- [X] T034 [P] Create unit tests for TransportRuleSetting validation and management including pickup/drop windows, stale location threshold, ETA change threshold, clock drift, retry policy, anomaly policy, draft activation, supersede behavior, permission denial, audit requirement, and 30-day location retention in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Foundational/TransportRuleSettingTests.cs
- [X] T035 Create initial EF migration for Transport audit, idempotency, feature setting, rule setting, and retained summary foundations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040006_TransportFoundation.cs
- [X] T036 [P] Create admin web Transport API client with tenant context, typed errors, pagination, rule-setting endpoints, review-summary endpoints, feature-disabled handling, and guardian-scope error mapping in apps/admin-web/src/features/transport/api/client.ts
- [X] T037 [P] Create guardian Transport API client with linked-student scope, typed errors, pagination, and feature-disabled handling in apps/admin-web/src/features/guardian-transport/api/client.ts
- [X] T038 [P] Create mobile Transport API client shell with tenant context, auth headers, retry metadata, source metadata, and offline error mapping in apps/mobile/lib/features/transport/transport_api.dart
- [X] T039 [P] Create mobile SQLite database wrapper and migration registry for transport scan queue, location retry queue, and local trip cache tables in apps/mobile/lib/features/transport/local/transport_database.dart
- [X] T040 [P] Create shared API and mobile test data builders for tenants, capabilities, permissions, students, guardians, credentials, vehicles, routes, stops, assignments, trips, scans, locations, ETAs, notifications, anomalies, and reviews in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Fixtures/TransportTestData.cs and apps/mobile/test/features/transport/transport_test_data.dart
- [X] T041 [P] Create shared web test data builders for route, assignment, trip, scan, ETA, notification, anomaly, and guardian visibility states in apps/admin-web/tests/transport/transportTestData.ts
- [X] T042 Create Transport OpenAPI tag registration and shared response/error examples for tenant mismatch, disabled capability, missing permission, rule-setting validation, review-summary filtering, audit failure, and cross-school reference in apps/api/src/SafeSchool.Api/Features/Transport/TransportOpenApiExamples.cs

**Checkpoint**: Foundation ready. User-story work can start after T014-T042 are complete.

---

## Phase 3: User Story 1 - Manage Routes and Stops (Priority: P1)

**Goal**: Transport managers can create, activate, update, version, review, list, and trace tenant-scoped bus routes and ordered stops.

**Independent Test**: Create a route with five ordered pickup stops, activate it, update one stop sequence, confirm the prior route version remains reviewable, and verify disabled capability, missing permission, duplicate code, inactive stop, and cross-school route cases are denied or recorded.

### Tests for User Story 1

- [X] T043 [P] [US1] Create TransportRoute, TransportStop, and RouteStopSequence lifecycle unit tests for Draft, Active, Suspended, Retired, Superseded, allowed directions, duplicate route_code, duplicate sequence_number, inactive stop, and route version preservation in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopDomainTests.cs
- [X] T044 [P] [US1] Create RouteStopSequenceValidator unit tests for active route readiness, pickup/drop direction mismatch, missing stops, similar stop names, invalid planned offsets, and superseded sequence review behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopSequenceValidatorTests.cs
- [X] T045 [P] [US1] Create contract tests for route list, create, detail, patch, stop list, stop create, stop patch, stop sequence replace, route version read, and route trace routes from contracts/route-stop-management.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopManagementContractTests.cs
- [X] T046 [P] [US1] Create integration tests for tenant isolation, transport.route_stop_management disabled capability, missing transport.routes.manage permission, duplicate route code, no active stops, cross-school stop reference, and route audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopManagementIntegrationTests.cs
- [X] T047 [P] [US1] Create admin web route and stop management journey tests for route create, stop create, sequence replace, route version review, disabled capability state, and trace link display in apps/admin-web/tests/transport/route-stop-management.spec.ts

### Implementation for User Story 1

- [X] T048 [P] [US1] Create TransportRoute, TransportStop, and RouteStopSequence domain models with validation helpers in apps/api/src/SafeSchool.Api/Features/Transport/Routes/TransportRoute.cs, apps/api/src/SafeSchool.Api/Features/Transport/Routes/TransportStop.cs, and apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopSequence.cs
- [X] T049 [P] [US1] Create route, stop, route version, route stop sequence, trace, and error DTOs matching contracts/route-stop-management.md in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopDtos.cs
- [X] T050 [US1] Create EF configurations and migration for routes, stops, route stop sequences, route version indexes, tenant indexes, status indexes, and route_code uniqueness in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040007_TransportRoutesAndStops.cs
- [X] T051 [US1] Implement TransportRouteService for route create, update, status changes, activation readiness, duplicate code validation, version preservation, list filtering, detail read, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Routes/TransportRouteService.cs
- [X] T052 [US1] Implement TransportStopService for stop create, update, suspend, retire, direction rules, similar-name review metadata, list filtering, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Routes/TransportStopService.cs
- [X] T053 [US1] Implement RouteStopSequenceService for sequence replacement, route_version creation, sequence_number validation, inactive stop rejection, superseded sequence review, and trace references in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopSequenceService.cs
- [X] T054 [US1] Implement RouteTraceService for route to assignments, trips, scans, ETAs, notifications, anomalies, manual reviews, and audit references in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteTraceService.cs
- [X] T055 [US1] Implement RoutesController and StopsController routes from contracts/route-stop-management.md in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RoutesController.cs and apps/api/src/SafeSchool.Api/Features/Transport/Routes/StopsController.cs
- [X] T056 [US1] Wire route create, route update, route activation denied, stop create, stop update, sequence replacement, route version read, route trace, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopAuditAdapter.cs
- [X] T057 [P] [US1] Create admin web route and stop API hooks for list, create, patch, sequence replace, version read, and trace in apps/admin-web/src/features/transport/routes/routesApi.ts
- [X] T058 [P] [US1] Create admin web route form, stop form, route stop sequence editor, route status badge, route version selector, and route trace panel in apps/admin-web/src/features/transport/routes/RouteForm.tsx and apps/admin-web/src/features/transport/routes/RouteStopSequenceEditor.tsx
- [X] T059 [US1] Create admin web route management route with filters, route list, stop list, create/update dialogs, sequence editor, version review, and trace links in apps/admin-web/src/app/(school)/transport/routes/page.tsx
- [X] T060 [US1] Add OpenAPI examples for route create, stop create, stop sequence replacement, route version read, route trace, disabled capability, missing permission, duplicate route code, no active stops, and cross-school reference in apps/api/src/SafeSchool.Api/Features/Transport/Routes/RouteStopOpenApiExamples.cs
- [X] T061 [US1] Create route and stop test data builder for active route with five ordered stops, superseded route version, duplicate code, inactive stop, and cross-school stop cases in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopTestData.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T043-T061 pass.

---

## Phase 4: User Story 2 - Assign Students to Buses and Stops (Priority: P1)

**Goal**: School administrators and transport managers can create and review vehicles and assign active students to approved routes, stops, and optional vehicles with guardian-scoped visibility.

**Independent Test**: Create an active vehicle, assign an active student to an active route with pickup and drop stops for a valid date range, show the staff and guardian transport plan for an approved guardian link, and reject inactive students, inactive routes, suspended vehicles, invalid dates, and cross-school references.

### Tests for User Story 2

- [X] T062 [P] [US2] Create TransportVehicle lifecycle unit tests for Draft, Active, Suspended, Retired, vehicle_code uniqueness, capacity validation, and active-trip usage prevention for suspended vehicles in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/TransportVehicleTests.cs
- [X] T063 [P] [US2] Create StudentTransportAssignment lifecycle unit tests for Draft, Active, Suspended, Expired, Removed, valid_from/valid_to validation, service direction, guardian visibility state, and overlapping assignment review in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/StudentTransportAssignmentTests.cs
- [X] T064 [P] [US2] Create assignment eligibility unit tests for active student, inactive student, active route, inactive route, pickup/drop stop mismatch, suspended vehicle, cross-school route, cross-school student, and disabled capability in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/AssignmentEligibilityServiceTests.cs
- [X] T065 [P] [US2] Create guardian transport plan visibility unit tests for approved active link, pending link, suspended link, expired link, removed link, rejected link, out-of-scope link, staff-only assignment, and disabled transport visibility in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/GuardianTransportPlanVisibilityTests.cs
- [X] T066 [P] [US2] Create contract tests for vehicle list, vehicle create, vehicle patch, assignment list, assignment create, assignment detail, assignment patch, staff student plan, guardian plan, and assignment trace routes from contracts/bus-assignment.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/BusAssignmentContractTests.cs
- [X] T067 [P] [US2] Create integration tests for tenant isolation, transport.bus_assignment disabled capability, missing assignment permission, guardian link scope, cross-school route/student reference, assignment activation denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/BusAssignmentIntegrationTests.cs
- [X] T068 [P] [US2] Create admin web vehicle and assignment journey tests for vehicle create/update, assignment create/update, invalid assignment denial, staff student plan, assignment trace, and guardian plan preview in apps/admin-web/tests/transport/bus-assignment.spec.ts
- [X] T069 [P] [US2] Create guardian transport plan journey tests for linked-student-only plan visibility, staff-only suppression, inactive guardian link suppression, and no unrelated route leakage in apps/admin-web/tests/transport/guardian-transport-plan.spec.ts

### Implementation for User Story 2

- [X] T070 [P] [US2] Create TransportVehicle and StudentTransportAssignment domain models with validation helpers in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/TransportVehicle.cs and apps/api/src/SafeSchool.Api/Features/Transport/Assignments/StudentTransportAssignment.cs
- [X] T071 [P] [US2] Create vehicle, assignment, student transport plan, guardian plan, assignment trace, and error DTOs matching contracts/bus-assignment.md in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/BusAssignmentDtos.cs
- [X] T072 [US2] Create EF configurations and migration for vehicles, assignments, assignment validity indexes, vehicle_code uniqueness, student/route/stop indexes, guardian visibility indexes, and assignment trace indexes in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/AssignmentEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040008_TransportVehiclesAndAssignments.cs
- [X] T073 [US2] Implement TransportVehicleService for vehicle create, update, suspend, retire, list filters, capacity validation, duplicate code validation, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/TransportVehicleService.cs
- [X] T074 [US2] Implement AssignmentEligibilityService for active student, active route, stop direction, valid dates, active vehicle, guardian visibility scope, overlapping assignment review, capability, permission, and cross-tenant checks in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/AssignmentEligibilityService.cs
- [X] T075 [US2] Implement StudentTransportAssignmentService for assignment create, update, suspend, expire, remove, list filters, detail read, review history, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/StudentTransportAssignmentService.cs
- [X] T076 [US2] Implement TransportPlanVisibilityService for staff-visible student transport plan and guardian-visible transport plan with linked-student-only filtering in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/TransportPlanVisibilityService.cs
- [X] T077 [US2] Implement AssignmentTraceService for assignment to trips, scans, notifications, anomalies, manual reviews, and audit references in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/AssignmentTraceService.cs
- [X] T078 [US2] Implement VehiclesController, AssignmentsController, StaffTransportPlanController, and GuardianTransportPlanController routes from contracts/bus-assignment.md in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/BusAssignmentControllers.cs
- [X] T079 [US2] Wire vehicle create/update, assignment create/update/denial, guardian plan visibility/suppression, assignment trace, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/BusAssignmentAuditAdapter.cs
- [X] T080 [P] [US2] Create admin web vehicle and assignment API hooks for list, create, patch, detail, staff student plan, guardian plan preview, and trace in apps/admin-web/src/features/transport/assignments/assignmentsApi.ts
- [X] T081 [P] [US2] Create admin web vehicle form, assignment form, validity date editor, visibility selector, assignment table, student plan panel, guardian plan preview, and assignment trace panel in apps/admin-web/src/features/transport/assignments/VehicleForm.tsx and apps/admin-web/src/features/transport/assignments/AssignmentForm.tsx
- [X] T082 [US2] Create admin web vehicle and assignment route with filters, assignment validation errors, staff student plan, guardian visibility preview, and trace links in apps/admin-web/src/app/(school)/transport/assignments/page.tsx
- [X] T083 [P] [US2] Create guardian transport plan API hooks and components for linked-student route, pickup stop, drop stop, visibility state, and suppression messaging in apps/admin-web/src/features/guardian-transport/plan/guardianTransportPlanApi.ts and apps/admin-web/src/features/guardian-transport/plan/GuardianTransportPlan.tsx
- [X] T084 [US2] Create guardian transport plan route with linked-student-only visibility and disabled-capability state in apps/admin-web/src/app/(guardian)/transport/plan/page.tsx
- [X] T085 [US2] Add OpenAPI examples for vehicle create, assignment create, staff plan, guardian plan, assignment trace, inactive student, inactive route, suspended vehicle, invalid date, guardian suppression, and cross-school reference in apps/api/src/SafeSchool.Api/Features/Transport/Assignments/BusAssignmentOpenApiExamples.cs
- [X] T086 [US2] Create assignment test data builder for active vehicle, suspended vehicle, active student, inactive student, active assignment, expired assignment, guardian-visible assignment, staff-only assignment, and cross-school assignment cases in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/BusAssignmentTestData.cs

**Checkpoint**: User Story 2 can be demonstrated independently after T062-T086 pass.

---

## Phase 5: User Story 3 - Record Boarding and Drop Scans (Priority: P1)

**Goal**: Authorized transport staff can create or start a scan-ready active trip context and record online or offline NFC/QR boarding/drop scans with accepted, denied, flagged, duplicate, and needs-review outcomes.

**Independent Test**: Use implemented APIs to create and start a scan-ready active trip for an active route, vehicle, authorized staff, and mobile device; scan an assigned student's active credential for boarding and drop; deny invalid credentials; record unassigned or wrong-stop scans as needs-review anomalies; approve one needs-review scan through reviewer action; submit delayed offline sync twice; end the trip; and verify no campus attendance or campus access outcome is created.

### Tests for User Story 3

- [X] T087 [P] [US3] Create TransportTrip scan-context and scan-ready lifecycle unit tests for create, start, end, Planned, Active, Paused, Completed, Cancelled, Needs Review, authorized staff, authorized mobile device, inactive trip scan rejection, and completed trip scan review routing in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/TransportTripScanContextTests.cs
- [X] T088 [P] [US3] Create boarding/drop scan validation unit tests for active credential, expired credential, suspended credential, revoked credential, replaced credential, unknown credential, duplicated credential, cross-tenant credential, active assignment, missing assignment, wrong route, wrong stop, disabled capability, and unauthorized device in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/BoardingDropScanValidationServiceTests.cs
- [X] T089 [P] [US3] Create offline transport sync unit tests for client_batch_id retry, client_scan_id retry, repeated taps, delayed local time, received time, clock drift, duplicate transport status prevention, and out-of-order boarding/drop evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Sync/OfflineTransportScanSyncServiceTests.cs
- [X] T090 [P] [US3] Create needs-review anomaly and scan-review unit tests for unassigned student, wrong route, wrong stop, invalid credential, delayed offline conflict, duplicate scan, normal-status withholding, reviewer approval, reviewer rejection, and preserved original evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/ScanAnomalyCreationTests.cs
- [X] T091 [P] [US3] Create contract tests for scan-ready trip create/start/end, online scan, offline sync, scan list, scan detail, scan trace, and scan review outcome routes from contracts/boarding-drop-scan.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/BoardingDropScanContractTests.cs
- [X] T092 [P] [US3] Create integration tests for tenant isolation, transport.boarding_drop_scans disabled capability, missing scan or trip permission, scan-ready trip start/end, unauthorized trip device, invalid credential denial, assignment mismatch anomaly, reviewer approval, duplicate scan idempotency, audit evidence, and no AttendanceAccess mutation in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/BoardingDropScanIntegrationTests.cs
- [X] T093 [P] [US3] Create mobile offline queue tests for enqueue boarding/drop scans, read pending, retry after failure, mark synced, mark rejected, duplicate client_scan_id, local time preservation, and source metadata in apps/mobile/test/features/transport/scans/offline_transport_scan_queue_test.dart
- [X] T094 [P] [US3] Create mobile scan capture tests for NFC scan, QR scan, boarding/drop selection, trip selection, stop selection, offline mode, result state, and sync request mapping in apps/mobile/test/features/transport/scans/boarding_drop_scan_capture_test.dart
- [X] T095 [P] [US3] Create admin web scan review journey tests for scan filters, denied scans, needs-review anomalies, offline sync batches, trace view, and no attendance outcome display in apps/admin-web/tests/transport/boarding-drop-scan.spec.ts

### Implementation for User Story 3

- [X] T096 [P] [US3] Create TransportTrip minimal domain model for scan-ready create/start/end, scan context, route version capture, assigned staff/device evidence, and lifecycle status checks in apps/api/src/SafeSchool.Api/Features/Transport/Trips/TransportTrip.cs
- [X] T097 [P] [US3] Create OfflineTransportScanSyncBatch and BoardingDropScanEvent domain models with idempotency and decision helpers in apps/api/src/SafeSchool.Api/Features/Transport/Scans/OfflineTransportScanSyncBatch.cs and apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanEvent.cs
- [X] T098 [P] [US3] Create TransportAnomaly and ManualTransportReview base domain models for needs-review scan outcomes, reviewer approval or rejection, preserved original evidence, and future review actions in apps/api/src/SafeSchool.Api/Features/Transport/Anomalies/TransportAnomaly.cs and apps/api/src/SafeSchool.Api/Features/Transport/Reviews/ManualTransportReview.cs
- [X] T099 [P] [US3] Create boarding/drop scan, offline sync, scan-ready trip lifecycle, scan review outcome, scan trace, trip scan context, anomaly reference, and error DTOs matching contracts/boarding-drop-scan.md in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanDtos.cs
- [X] T100 [US3] Create EF configurations and migration for transport trips, scan-ready trip lifecycle columns, offline scan sync batches, boarding/drop scan events, scan idempotency indexes, trip indexes, assignment indexes, credential indexes, anomaly references, scan review outcome fields, and manual review base tables in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040009_TransportBoardingDropScans.cs
- [X] T101 [US3] Implement minimal TransportTripLifecycleService and TransportTripScanContextService for scan-ready create/start/end, active trip lookup, route version validation, assigned staff validation, authorized mobile tracking device validation, completed/cancelled trip rejection, idempotent request handling, and cross-tenant denial in apps/api/src/SafeSchool.Api/Features/Transport/Trips/TransportTripLifecycleService.cs and apps/api/src/SafeSchool.Api/Features/Transport/Trips/TransportTripScanContextService.cs
- [X] T102 [US3] Implement BoardingDropScanValidationService for tenant, feature, permission, trip, credential, assignment, route, stop, direction, QR fallback, and cross-school denial rules in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanValidationService.cs
- [X] T103 [US3] Implement TransportStatusDecisionService for accepted boarding, accepted drop, denied credential, flagged credential, needs-review assignment mismatch, duplicate scan, reviewed approval or rejection, and transport_status_after calculation in apps/api/src/SafeSchool.Api/Features/Transport/Scans/TransportStatusDecisionService.cs
- [X] T104 [US3] Implement ScanAnomalyCreationService for missing assignment, wrong route, wrong stop, invalid credential, delayed offline conflict, duplicate scan, normal-status withholding, guardian-notification withholding, reviewer approval gate, and manual review evidence linkage in apps/api/src/SafeSchool.Api/Features/Transport/Scans/ScanAnomalyCreationService.cs
- [X] T105 [US3] Implement OfflineTransportScanSyncService for client_batch_id idempotency, client_scan_id idempotency, delayed scan reconciliation, duplicate detection, local time preservation, received time preservation, clock drift, and audit outcomes in apps/api/src/SafeSchool.Api/Features/Transport/Sync/OfflineTransportScanSyncService.cs
- [X] T106 [US3] Implement BoardingDropScanTraceService that returns trip, route, stop, assignment, ETA, notification, anomaly, manual review, and audit references when present in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanTraceService.cs
- [X] T107 [US3] Implement BoardingDropScanEventsController, ScanTripLifecycleController, and ScanReviewOutcomesController routes for scan-ready trip create/start/end, online scan, offline sync, scan list, scan detail, scan trace, and needs-review scan approval in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanEventsController.cs
- [X] T108 [US3] Wire scan-ready trip lifecycle, scan capture, denied scan, flagged scan, needs-review anomaly, reviewer approval or rejection, offline sync, duplicate scan, trace, no-campus-outcome check, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanAuditAdapter.cs
- [X] T109 [P] [US3] Create admin web scan API hooks for scan-ready trip create/start/end, online scan review, offline sync batch review, scan detail, scan trace, denied scans, needs-review anomalies, and scan review outcomes in apps/admin-web/src/features/transport/scans/scansApi.ts
- [X] T110 [P] [US3] Create admin web scan event table, scan decision badge, sync status badge, anomaly link, review outcome dialog, scan-ready trip controls, scan trace panel, and no-campus-outcome notice in apps/admin-web/src/features/transport/scans/ScanEventTable.tsx and apps/admin-web/src/features/transport/scans/ScanTracePanel.tsx
- [X] T111 [US3] Create admin web scan review route with filters by student, credential, trip, route, stop, direction, decision, sync status, anomaly status, review status, and time in apps/admin-web/src/app/(school)/transport/scans/page.tsx
- [X] T112 [US3] Implement mobile scan event model, offline sync batch model, and sync response model in apps/mobile/lib/features/transport/scans/boarding_drop_scan_event.dart and apps/mobile/lib/features/transport/scans/offline_transport_scan_sync_models.dart
- [X] T113 [US3] Implement SQLite-backed offline transport scan queue with enqueue, pending list, mark synced, mark rejected, duplicate client_scan_id detection, and migration in apps/mobile/lib/features/transport/scans/offline_transport_scan_queue.dart
- [X] T114 [US3] Implement mobile NFC and QR reader adapter interfaces with fakeable implementations for tests in apps/mobile/lib/features/transport/scans/transport_scan_reader_adapter.dart
- [X] T115 [US3] Implement mobile BoardingDropScanRepository for scan-ready trip start/end, online scan submission, offline fallback, batch sync, retry, and typed error mapping in apps/mobile/lib/features/transport/scans/boarding_drop_scan_repository.dart
- [X] T116 [US3] Implement mobile boarding/drop scan screen with active trip start/end controls, trip selector, stop selector, boarding/drop mode, scan result state, offline indicator, retry action, and audit-safe display fields in apps/mobile/lib/features/transport/scans/boarding_drop_scan_screen.dart
- [X] T117 [US3] Add OpenAPI examples for scan-ready trip create/start/end, online scan, offline sync, scan review outcome, scan list, scan detail, scan trace, invalid credential, assignment mismatch anomaly, duplicate scan, unauthorized device, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Transport/Scans/BoardingDropScanOpenApiExamples.cs
- [X] T118 [US3] Create boarding/drop scan test data builder for scan-ready trip create/start/end, assigned student, unassigned student, wrong route, wrong stop, invalid credential, duplicate scan, delayed offline scan, reviewer approval, and cross-school credential cases in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/BoardingDropScanTestData.cs

**Checkpoint**: User Story 3 can be demonstrated independently after T087-T118 pass.

---

## Phase 6: User Story 4 - Track Active Bus Trips (Priority: P2)

**Goal**: Transport managers and eligible guardians can view active trip progress from authorized mobile location updates, with route overlap allowed and bus/device overlap blocked.

**Independent Test**: Start two trips for the same route using different buses and tracking devices, block a third trip using an already active bus or device, submit current and stale location updates, show staff progress, show guardian pickup ETA before boarding, exact live location only while onboard, drop status after drop, and apply 30-day detailed location retention.

### Tests for User Story 4

- [X] T119 [P] [US4] Create extended TransportTrip lifecycle and overlap unit tests for pause, resume, patch staff, cancel, Needs Review, same-route parallel trips, same-bus overlap denial, same-device overlap denial, staff changes, and route version capture beyond the US3 scan-ready lifecycle in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/TransportTripLifecycleTests.cs
- [X] T120 [P] [US4] Create LocationUpdateValidationService unit tests for authorized mobile device, active trip, current update, stale update, untrusted update, out-of-trip update, cross-school update, duplicate client_location_id, and suppression reason in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/LocationUpdateValidationServiceTests.cs
- [X] T121 [P] [US4] Create GuardianTripProgressVisibility unit tests for before boarding pickup ETA only, onboard exact live location, after drop drop status only, inactive guardian link, unrelated student, stale location, and disabled capability in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/GuardianTripProgressVisibilityTests.cs
- [X] T122 [P] [US4] Create location retention unit tests for 30-day detailed retention, summary-only conversion, review hold exemption, audit event, and guardian exact-location suppression after retention in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/LocationRetentionServiceTests.cs
- [X] T123 [P] [US4] Create contract tests for extended trip list, trip patch, location update, staff progress, guardian progress, trip trace, and full live-tracking behavior that extends the scan-ready lifecycle from contracts/live-tracking.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/LiveTrackingContractTests.cs
- [X] T124 [P] [US4] Create integration tests for tenant isolation, transport.live_tracking disabled capability, missing trip/tracking/location permission, unauthorized device, route overlap allowed, bus/device overlap blocked, stale location suppression, guardian visibility windows, retention, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/LiveTrackingIntegrationTests.cs
- [X] T125 [P] [US4] Create mobile location queue tests for enqueue location update, retry after failure, duplicate client_location_id, mark synced, stale update flag, and local trip cache use in apps/mobile/test/features/transport/tracking/location_update_queue_test.dart
- [X] T126 [P] [US4] Create mobile trip tracking tests for trip start, location permission denied, authorized device reference, location update submission, offline retry, trip end, and stale indicator in apps/mobile/test/features/transport/tracking/trip_tracking_test.dart
- [X] T127 [P] [US4] Create admin web trip tracking journey tests for trip create/start/end, route overlap, bus/device overlap denial, staff progress, stale location display, trip trace, and retention status in apps/admin-web/tests/transport/live-tracking.spec.ts
- [X] T128 [P] [US4] Create guardian transport progress journey tests for pickup ETA before boarding, exact live location while onboard, drop status after drop, no unrelated route leakage, and stale-location suppression in apps/admin-web/tests/transport/guardian-transport-progress.spec.ts

### Implementation for User Story 4

- [X] T129 [P] [US4] Extend TransportTrip domain model with active-trip overlap helpers, staff change helpers, tracking_device_reference rules, and route_version capture in apps/api/src/SafeSchool.Api/Features/Transport/Trips/TransportTrip.cs
- [X] T130 [P] [US4] Create TransportLocationUpdate domain model with freshness, acceptance, suppression, progress, and retention helpers in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/TransportLocationUpdate.cs
- [X] T131 [P] [US4] Create trip lifecycle, location update, staff progress, guardian progress, retention, and trip trace DTOs matching contracts/live-tracking.md in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingDtos.cs
- [X] T132 [US4] Create EF configurations and migration for location updates, client_location_id idempotency, active-trip progress indexes, location freshness indexes, guardian visibility indexes, retention indexes, and live-tracking extensions to scan-ready trip records in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040010_TransportLiveTracking.cs
- [X] T133 [US4] Extend TransportTripLifecycleService for pause, resume, patch staff, cancel, same-route parallel trip support, same-bus/device overlap denial hardening, live-tracking state transitions, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Trips/TransportTripLifecycleService.cs
- [X] T134 [US4] Implement LocationUpdateValidationService for active trip, authorized mobile device, tenant, feature, permission, stale, untrusted, out-of-trip, cross-school, duplicate client_location_id, and suppression reason behavior in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LocationUpdateValidationService.cs
- [X] T135 [US4] Implement TransportLocationUpdateService for accepting, suppressing, listing latest progress, preserving reported_at/received_at, linking nearest stop, emitting metrics, and audit evidence in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/TransportLocationUpdateService.cs
- [X] T136 [US4] Implement GuardianTripProgressService for pickup ETA before boarding, exact live bus location after accepted boarding and before accepted/reviewed drop, drop status after drop, stale suppression, and linked-student filtering in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/GuardianTripProgressService.cs
- [X] T137 [US4] Implement LocationRetentionService for 30-day detailed retention, summary-only conversion, approved review hold, retention audit events, and scheduled execution entrypoint in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LocationRetentionService.cs
- [X] T138 [US4] Implement TripTraceService for route, assignments, scans, locations, ETAs, notifications, anomalies, reviews, retention, and audit references in apps/api/src/SafeSchool.Api/Features/Transport/Trips/TripTraceService.cs
- [X] T139 [US4] Implement TripsController, LocationUpdatesController, StaffTripProgressController, GuardianTripProgressController, and TripTraceController routes from contracts/live-tracking.md in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingControllers.cs
- [X] T140 [US4] Wire trip create/start/update/end, overlap denial, location accepted, location suppressed, guardian visibility suppression, retention conversion, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingAuditAdapter.cs
- [X] T141 [P] [US4] Create admin web trip and tracking API hooks for list, create, start, patch, end, location progress, trip trace, and retention status in apps/admin-web/src/features/transport/tracking/trackingApi.ts
- [X] T142 [P] [US4] Create admin web trip form, start trip dialog, trip status badge, active-trip conflict panel, staff progress map placeholder, stale location badge, retention badge, and trip trace panel in apps/admin-web/src/features/transport/tracking/TripForm.tsx and apps/admin-web/src/features/transport/tracking/TripProgressPanel.tsx
- [X] T143 [US4] Create admin web active trip route with filters, trip lifecycle actions, current progress, conflict messages, retention status, and trace links in apps/admin-web/src/app/(school)/transport/trips/page.tsx
- [X] T144 [P] [US4] Create guardian transport progress API hooks and components for visibility phase, pickup ETA, exact live location, stale state, and drop status in apps/admin-web/src/features/guardian-transport/progress/guardianTripProgressApi.ts and apps/admin-web/src/features/guardian-transport/progress/GuardianTripProgress.tsx
- [X] T145 [US4] Create guardian transport progress route with linked-student-only visibility and exact-location window enforcement in apps/admin-web/src/app/(guardian)/transport/progress/page.tsx
- [X] T146 [US4] Implement mobile trip tracking models, location update model, local trip cache model, and retention-safe response model in apps/mobile/lib/features/transport/tracking/transport_trip.dart and apps/mobile/lib/features/transport/tracking/transport_location_update.dart
- [X] T147 [US4] Implement SQLite-backed mobile location retry queue and local active trip cache in apps/mobile/lib/features/transport/tracking/location_update_queue.dart and apps/mobile/lib/features/transport/tracking/local_trip_cache.dart
- [X] T148 [US4] Implement mobile location provider adapter with fakeable permission, current location, stale location, and untrusted source outputs in apps/mobile/lib/features/transport/tracking/location_provider_adapter.dart
- [X] T149 [US4] Implement mobile TripTrackingRepository for trip start, location update submission, offline retry, stale location mapping, and trip end in apps/mobile/lib/features/transport/tracking/trip_tracking_repository.dart
- [X] T150 [US4] Implement mobile trip tracking screen with active trip selection, start/end actions, location permission state, sync status, stale indicator, and no dedicated GPS dependency in apps/mobile/lib/features/transport/tracking/trip_tracking_screen.dart
- [X] T151 [US4] Add OpenAPI examples for trip create, trip start, location update, staff progress, guardian progress, trip trace, overlap denial, stale suppression, review hold, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Transport/Tracking/LiveTrackingOpenApiExamples.cs
- [X] T152 [US4] Create live tracking test data builder for active trip, same-route parallel trips, same-bus conflict, same-device conflict, current location, stale location, onboard guardian, before-boarding guardian, after-drop guardian, and retention cases in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/LiveTrackingTestData.cs

**Checkpoint**: User Story 4 can be demonstrated independently after T119-T152 pass.

---

## Phase 7: User Story 5 - Calculate ETAs for Stops and Students (Priority: P2)

**Goal**: Guardians and transport reviewers can see ETA records for upcoming route stops when current trusted trip progress exists, and unavailable/stale states when progress cannot support an estimate.

**Independent Test**: Use an active trip with approved route stops and current trusted progress, recalculate ETA for upcoming stops, show guardian pickup ETA for a linked student, mark stale or inconsistent progress as unavailable/needs-review, and create eligible material ETA change events for notification handling.

### Tests for User Story 5

- [X] T153 [P] [US5] Create ETA calculation unit tests for active trip, approved route sequence, non-overlapping bus/device assignment, current trusted progress, upcoming stops, linked student assignment, and calculated freshness/confidence states in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaCalculationServiceTests.cs
- [X] T154 [P] [US5] Create ETA stale/unavailable unit tests for stale location, untrusted location, missing route sequence, completed trip, missing assignment, inconsistent progress, and unsupported estimate prevention in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaUnavailableStateTests.cs
- [X] T155 [P] [US5] Create material ETA change unit tests for threshold comparison, notification eligibility event creation, no-notification below threshold, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/MaterialEtaChangeTests.cs
- [X] T156 [P] [US5] Create contract tests for ETA recalculate, staff ETA list, stop ETA, guardian ETA, and ETA trace routes from contracts/eta-calculation.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaCalculationContractTests.cs
- [X] T157 [P] [US5] Create integration tests for tenant isolation, transport.eta_calculation disabled capability, transport.live_tracking disabled capability, missing ETA permission, guardian scope, stale progress, material ETA notification hook, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaCalculationIntegrationTests.cs
- [X] T158 [P] [US5] Create admin web ETA review journey tests for trip ETA list, stop ETA detail, stale state, confidence state, trace link, and material-change marker in apps/admin-web/tests/transport/eta-calculation.spec.ts
- [X] T159 [P] [US5] Create guardian ETA journey tests for linked-student pickup ETA, unavailable ETA, stale ETA, no unrelated student ETA, and exact live location unavailable before boarding in apps/admin-web/tests/transport/guardian-transport-eta.spec.ts

### Implementation for User Story 5

- [X] T160 [P] [US5] Create ETARecord domain model with freshness, confidence, unavailable, stale, needs-review, and material-change helper methods in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaRecord.cs
- [X] T161 [P] [US5] Create ETA recalculation, staff ETA list, stop ETA, guardian ETA, ETA trace, and error DTOs matching contracts/eta-calculation.md in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaDtos.cs
- [X] T162 [US5] Create EF configurations and migration for ETA records, trip/route/stop/student indexes, freshness indexes, confidence indexes, material change indexes, and trace indexes in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040011_TransportEtaRecords.cs
- [X] T163 [US5] Implement EtaCalculationService for upcoming stop selection, active trip validation, route sequence validation, current trusted progress use, estimated arrival state, confidence state, freshness state, and unsupported estimate prevention in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaCalculationService.cs
- [X] T164 [US5] Implement GuardianEtaVisibilityService for linked-student pickup ETA before boarding, guardian scope, unavailable/stale status, and no exact live location before boarding in apps/api/src/SafeSchool.Api/Features/Transport/Eta/GuardianEtaVisibilityService.cs
- [X] T165 [US5] Implement MaterialEtaChangeService for ETA threshold comparison, notification eligibility event creation, deduplication, and audit evidence in apps/api/src/SafeSchool.Api/Features/Transport/Eta/MaterialEtaChangeService.cs
- [X] T166 [US5] Implement EtaTraceService for trip, stop, location, assignment, notification, anomaly, review, and audit references in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaTraceService.cs
- [X] T167 [US5] Implement EtaController and GuardianEtaController routes from contracts/eta-calculation.md in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaControllers.cs
- [X] T168 [US5] Wire ETA calculated, ETA unavailable, ETA stale, material ETA change, guardian ETA suppression, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaAuditAdapter.cs
- [X] T169 [P] [US5] Create admin web ETA API hooks for recalculate, ETA list, stop ETA, ETA trace, stale state, confidence state, and material change state in apps/admin-web/src/features/transport/eta/etaApi.ts
- [X] T170 [P] [US5] Create admin web ETA table, ETA status badge, confidence badge, freshness badge, stop ETA panel, material change badge, and ETA trace panel in apps/admin-web/src/features/transport/eta/EtaTable.tsx and apps/admin-web/src/features/transport/eta/EtaTracePanel.tsx
- [X] T171 [US5] Create admin web ETA route with trip selector, recalculation action, ETA list, stale/unavailable states, material change markers, and trace links in apps/admin-web/src/app/(school)/transport/eta/page.tsx
- [X] T172 [P] [US5] Create guardian ETA API hooks and components for pickup ETA, stale/unavailable state, freshness, confidence, and linked-student filtering in apps/admin-web/src/features/guardian-transport/eta/guardianEtaApi.ts and apps/admin-web/src/features/guardian-transport/eta/GuardianEtaPanel.tsx
- [X] T173 [US5] Create guardian ETA route with linked-student-only pickup ETA and unavailable/stale messaging in apps/admin-web/src/app/(guardian)/transport/eta/page.tsx
- [X] T174 [US5] Add OpenAPI examples for ETA recalculate, staff ETA list, stop ETA, guardian ETA, ETA trace, stale progress, route sequence missing, guardian scope denial, material change, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Transport/Eta/EtaOpenApiExamples.cs
- [X] T175 [US5] Create ETA test data builder for current trip progress, stale trip progress, route sequence, upcoming stops, linked-student assignment, material ETA change, and guardian scope cases in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaTestData.cs

**Checkpoint**: User Story 5 can be demonstrated independently after T153-T175 pass.

---

## Phase 8: User Story 6 - Notify Guardians About Transport Events (Priority: P3)

**Goal**: Eligible guardians receive or can view transport notification records for accepted or reviewed boarding, drop, delay, material ETA change, route change, and corrected transport outcomes while ineligible or unresolved events are suppressed with reasons.

**Independent Test**: Capture eligible boarding, drop, delay, and material ETA events for a student with an approved guardian link, create visible notification records, suppress notifications for inactive links, denied scans, unresolved anomalies, stale tracking evidence, and disabled capability, withdraw a corrected event, and verify guardian read scope.

### Tests for User Story 6

- [X] T176 [P] [US6] Create transport notification eligibility unit tests for accepted boarding, accepted drop, delay, material ETA change, route change, reviewed correction, approved guardian link, pending link, suspended link, expired link, removed link, rejected link, out-of-scope link, denied event, unresolved anomaly, stale location, and disabled capability in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationEligibilityTests.cs
- [X] T177 [P] [US6] Create transport notification record lifecycle unit tests for Eligible, Created, Visible, Suppressed, Withdrawn, Failed, suppression reason, visible status, corrected status, and idempotent event creation in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationRecordTests.cs
- [X] T178 [P] [US6] Create transport notification withdrawal unit tests for reviewer permission, withdrawal reason, replacement visible status, guardian-facing correction, audit evidence, and invalid withdrawal transitions in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationWithdrawalTests.cs
- [X] T179 [P] [US6] Create contract tests for notification record list, detail, guardian notification list, withdraw, and notification trace routes from contracts/transport-notification.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationContractTests.cs
- [X] T180 [P] [US6] Create integration tests for tenant isolation, transport.notifications disabled capability, guardian scope, guardian requesting another student, unresolved event suppression, stale location suppression, missing review permission, withdrawal audit, and no Phase 9 messaging behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationIntegrationTests.cs
- [X] T181 [P] [US6] Create admin web notification review journey tests for filters, detail view, suppression reasons, withdrawal, corrected event display, notification trace, and no general messaging controls in apps/admin-web/tests/transport/transport-notification.spec.ts
- [X] T182 [P] [US6] Create guardian transport notification journey tests for linked-student-only records, boarding/drop/delay/ETA statuses, suppressed records hidden, withdrawn records corrected, and no unrelated route leakage in apps/admin-web/tests/transport/guardian-transport-notification.spec.ts

### Implementation for User Story 6

- [X] T183 [P] [US6] Create TransportNotificationRecord domain model with eligibility status transitions, suppression reason validation, visible status helpers, withdrawal helpers, and idempotency identity in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationRecord.cs
- [X] T184 [P] [US6] Create transport notification list, detail, guardian list, withdrawal, notification trace, and error DTOs matching contracts/transport-notification.md in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationDtos.cs
- [X] T185 [US6] Create EF configurations and migration for transport notification records, guardian link references, source event references, status indexes, suppression indexes, guardian visibility indexes, and notification trace indexes in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040012_TransportNotifications.cs
- [X] T186 [US6] Implement TransportNotificationEligibilityService for guardian link scope, feature capability, accepted/reviewed source evidence, denied event suppression, unresolved anomaly suppression, stale location suppression, material ETA event handling, and suppression reason output in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationEligibilityService.cs
- [X] T187 [US6] Implement TransportNotificationService for creating visible or suppressed records from boarding, drop, delay, material ETA, route change, and reviewed correction events with idempotent creation in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationService.cs
- [X] T188 [US6] Implement GuardianTransportNotificationVisibilityService for guardian me list, linked-student filtering, visible records only, withdrawal/correction display, and no cross-student leakage in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/GuardianTransportNotificationVisibilityService.cs
- [X] T189 [US6] Implement TransportNotificationWithdrawalService for reviewer withdrawal, replacement visible status, reason validation, invalid transition prevention, and audit evidence in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationWithdrawalService.cs
- [X] T190 [US6] Implement TransportNotificationTraceService for source event, guardian link, trip, anomaly, manual review, and audit references in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationTraceService.cs
- [X] T191 [US6] Implement TransportNotificationRecordsController and GuardianTransportNotificationsController routes from contracts/transport-notification.md in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationControllers.cs
- [X] T192 [US6] Wire notification eligibility, notification creation, suppression, withdrawal, correction visibility, guardian access denial, and audit events in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationAuditAdapter.cs
- [X] T193 [US6] Integrate TransportNotificationService with accepted boarding/drop scan outcomes, material ETA change events, delayed trip events, route change events, and manual review corrections without implementing general messaging channels in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationWorkflowHooks.cs
- [X] T194 [P] [US6] Create admin web notification API hooks for records, detail, filters, withdrawal, trace, suppression reasons, and typed errors in apps/admin-web/src/features/transport/notifications/notificationsApi.ts
- [X] T195 [P] [US6] Create admin web notification table, event type badge, notification status badge, suppression reason badge, detail panel, withdrawal dialog, and trace panel in apps/admin-web/src/features/transport/notifications/NotificationRecordTable.tsx and apps/admin-web/src/features/transport/notifications/NotificationDetailPanel.tsx
- [X] T196 [US6] Create admin web transport notification review route with filters, detail, withdrawal, corrected status, trace links, and no general messaging controls in apps/admin-web/src/app/(school)/transport/notifications/page.tsx
- [X] T197 [P] [US6] Create guardian transport notification API hooks and list components for linked-student transport notifications in apps/admin-web/src/features/guardian-transport/notifications/guardianTransportNotificationsApi.ts and apps/admin-web/src/features/guardian-transport/notifications/GuardianTransportNotificationList.tsx
- [X] T198 [US6] Create guardian transport notification route with linked-student-only visibility, event type filtering, withdrawn/corrected statuses, and disabled-capability state in apps/admin-web/src/app/(guardian)/transport/notifications/page.tsx
- [X] T199 [US6] Add OpenAPI examples for notification record list, detail, guardian list, withdrawal, notification trace, guardian link suppression, source event suppression, stale location suppression, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Transport/Notifications/TransportNotificationOpenApiExamples.cs
- [X] T200 [US6] Create notification test data builder for accepted boarding, accepted drop, delay, material ETA change, route change, reviewed correction, inactive guardian links, unresolved anomalies, stale locations, suppressed records, and withdrawn records in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationTestData.cs

**Checkpoint**: User Story 6 can be demonstrated independently after T176-T200 pass.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full Phase 3 implementation, complete anomaly review, rule-setting UI, review-summary coverage, harden security and observability, and preserve traceability for later phases.

- [X] T201 [P] Create transport anomaly, rule-setting, and review-summary contract tests for anomaly runs, list, detail, assign, resolve, dismiss, reopen, manual review create, manual review detail, rule setting read/create/update/activate/suspend, and review summary list/detail routes from contracts/transport-anomaly-review.md and contracts/transport-rules-and-review-summary.md in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Anomalies/TransportAnomalyReviewContractTests.cs
- [X] T202 [P] Create transport anomaly, rule-setting, and review-summary integration tests for tenant isolation, disabled related capability, missing review permission, missing rule permission, duplicate detection run, evidence from another tenant, manual correction, review summary filtering, no attendance/campus access mutation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Anomalies/TransportAnomalyReviewIntegrationTests.cs
- [X] T203 Implement TransportAnomalyDetectionService for missed boarding, missed drop, wrong route, wrong stop, duplicate scan, invalid credential, out-of-order scan, delayed offline conflict, route deviation, stale location, delayed trip, and manual-review-required cases in apps/api/src/SafeSchool.Api/Features/Transport/Anomalies/TransportAnomalyDetectionService.cs
- [X] T204 Implement TransportAnomalyReviewService, ManualTransportReviewService, TransportRuleSettingService, and TransportReviewSummaryService for assign, resolve, dismiss, reopen, correct, withdraw, reason validation, rule read/create/update/activate/suspend, review summary aggregation, no silent outcome mutation, and no campus attendance/access creation in apps/api/src/SafeSchool.Api/Features/Transport/Anomalies/TransportAnomalyReviewService.cs, apps/api/src/SafeSchool.Api/Features/Transport/Reviews/ManualTransportReviewService.cs, apps/api/src/SafeSchool.Api/Features/Transport/Rules/TransportRuleSettingService.cs, and apps/api/src/SafeSchool.Api/Features/Transport/Reviews/TransportReviewSummaryService.cs
- [X] T205 Implement TransportAnomaliesController, ManualTransportReviewsController, TransportRuleSettingsController, and TransportReviewSummariesController routes from contracts/transport-anomaly-review.md and contracts/transport-rules-and-review-summary.md in apps/api/src/SafeSchool.Api/Features/Transport/Anomalies/TransportAnomalyControllers.cs, apps/api/src/SafeSchool.Api/Features/Transport/Reviews/ManualTransportReviewsController.cs, apps/api/src/SafeSchool.Api/Features/Transport/Rules/TransportRuleSettingsController.cs, and apps/api/src/SafeSchool.Api/Features/Transport/Reviews/TransportReviewSummariesController.cs
- [X] T206 [P] Create admin web anomaly, rule-setting, and review-summary API hooks, anomaly table, anomaly detail panel, assign dialog, resolve dialog, dismiss dialog, reopen action, manual review correction dialog, transport rule settings form, and review summary table in apps/admin-web/src/features/transport/anomalies/anomaliesApi.ts, apps/admin-web/src/features/transport/anomalies/TransportAnomalyTable.tsx, apps/admin-web/src/features/transport/settings/TransportRuleSettingsForm.tsx, and apps/admin-web/src/features/transport/review/TransportReviewSummaryTable.tsx
- [X] T207 Create admin web transport anomaly review, transport settings, and transport review summary routes with filters, detection run action, assignment, resolution, dismissal, reopening, correction handoff, rule activation/suspension, summary filters, and trace links in apps/admin-web/src/app/(school)/transport/anomalies/page.tsx, apps/admin-web/src/app/(school)/transport/settings/page.tsx, and apps/admin-web/src/app/(school)/transport/review/page.tsx
- [X] T208 [P] Generate or update OpenAPI documentation for all Transport routes in apps/api/src/SafeSchool.Api/Features/Transport/TransportOpenApi.md
- [X] T209 [P] Create end-to-end tenant isolation and feature-disabled scenarios across routes, vehicles, assignments, trips, scans, tracking, ETA, notifications, anomalies, manual reviews, rule settings, review summaries, retention, audit, and access decisions in tests/e2e/transport/transport.e2e.spec.ts
- [X] T210 [P] Create security review checklist for tenant boundaries, feature gates, permissions, guardian visibility, credential evidence, offline sync idempotency, mobile location authorization, retention, cross-tenant not-found behavior, and audit failure behavior in docs/transport/security-review.md
- [X] T211 [P] Create observability review for scan sync metrics, invalid credential metrics, assignment mismatch metrics, active trip metrics, location suppression metrics, ETA metrics, notification suppression metrics, anomaly metrics, retention metrics, audit event categories, logs, and centralized error reporting in docs/transport/observability.md
- [X] T212 [P] Create Phase 3 traceability matrix mapping FR-001 through FR-027 and SC-001 through SC-014 to implemented tests and files, including rule-setting management and review-summary coverage, in docs/transport/traceability.md
- [X] T213 [P] Create offline transport scan operations runbook covering route/trip setup, mobile scan queue retry, delayed sync review, duplicate scan review, clock drift review, wrong-route/wrong-stop review, and failed sync escalation in docs/transport/offline-transport-scan-runbook.md
- [X] T214 [P] Create live tracking operations runbook covering authorized mobile tracking device setup, location permission denial, stale location review, bus/device overlap denial, route deviation review, 30-day retention, and review hold escalation in docs/transport/live-tracking-runbook.md
- [X] T215 Run API test suite and record relevant Transport output in docs/transport/backend-test-results.md
- [X] T216 Run admin web test suite and record relevant Transport output in docs/transport/admin-web-test-results.md
- [X] T217 Run mobile test suite and record relevant Transport output in docs/transport/mobile-test-results.md
- [X] T218 Run contract and end-to-end validation for Transport and record output in docs/transport/contract-e2e-test-results.md
- [X] T219 Run quickstart.md validation scenarios end-to-end and record pass/fail evidence in docs/transport/quickstart-validation.md
- [X] T220 Verify Phase 3 excludes campus attendance generation, campus access decisions, wallet, learning, requests, medical, complaints, general messaging, broadcasts, documents, search, broad dashboards, dedicated GPS hardware, and physical vehicle control in docs/transport/scope-boundary-review.md
- [X] T221 [P] Create SC-001 route and assignment workflow timing tests for one active route with five stops and ten assignments under 15 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/RouteAssignmentWorkflowTimingTests.cs and apps/admin-web/tests/transport/route-assignment-timing.spec.ts
- [X] T222 [P] Create SC-003 boarding/drop scan latency tests for online scan, offline fallback, and mobile scan capture under 10 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/BoardingDropScanLatencyTests.cs and apps/mobile/test/features/transport/scans/boarding_drop_scan_latency_test.dart
- [X] T223 [P] Create SC-006 boarding/drop visibility latency tests for 95% of accepted scan events visible to authorized reviewers and eligible guardians within 2 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/ScanVisibilityLatencyTests.cs
- [X] T224 [P] Create SC-007 active-trip location latency tests for 95% of accepted location updates visible or reflected within 30 seconds and stale/untrusted updates suppressed in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/LocationUpdateLatencyTests.cs and apps/admin-web/tests/transport/location-update-latency.spec.ts
- [X] T225 [P] Create SC-008 ETA latency tests for 95% of eligible active trips producing ETA records within 60 seconds of trusted trip progress in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/EtaLatencyTests.cs
- [X] T226 [P] Create SC-009 notification latency tests for 95% of eligible transport notification records becoming created or visible within 2 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/TransportNotificationLatencyTests.cs
- [X] T227 [P] Create SC-011 trip trace latency tests for reviewers tracing active-trip evidence in under 60 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Performance/TripTraceLatencyTests.cs and apps/admin-web/tests/transport/trip-trace-latency.spec.ts
- [X] T228 [P] Create SC-013 guardian exact-location boundary tests proving exact live bus location is hidden before accepted boarding and after accepted or reviewed drop in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Security/GuardianExactLocationBoundaryTests.cs and apps/admin-web/tests/transport/guardian-exact-location-boundary.spec.ts
- [X] T229 [P] Create SC-014 location retention tests proving detailed active-trip location records older than 30 days retain only summaries and audit evidence unless approved review hold applies in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Retention/LocationRetentionComplianceTests.cs

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1 completion and blocks all user stories.
- **Phase 3 US1**: Depends on Phase 2. Delivers the MVP route and stop plan foundation.
- **Phase 4 US2**: Depends on Phase 2 and consumes active routes/stops. It can be tested with seeded route/stop records, but production delivery should follow US1.
- **Phase 5 US3**: Depends on Phase 2 and consumes route, assignment, credential, vehicle, staff, and scan-ready trip lifecycle context. Production delivery should follow US1 and US2 so US3 can create and start active trips through implemented APIs.
- **Phase 6 US4**: Depends on Phase 2 and consumes route, vehicle, assignment, trip, scan, and guardian evidence. It extends the US3 scan-ready trip lifecycle with live tracking, location progress, guardian progress, and retention behavior.
- **Phase 7 US5**: Depends on Phase 2 and consumes active trip progress and route stop sequence. It can be tested with seeded trip/location evidence, but production delivery should follow US4.
- **Phase 8 US6**: Depends on Phase 2 and consumes accepted/reviewed scan, ETA, route, trip, and guardian evidence. It can be tested with seeded source events, but production delivery should follow US3 and US5.
- **Phase 9 Polish**: Depends on all desired user stories being complete.

### User Story Dependencies

- **US1 Manage Routes and Stops (P1)**: Can start after Foundational. No dependency on other user stories.
- **US2 Assign Students to Buses and Stops (P1)**: Can start after Foundational with seeded routes/stops, but final integration depends on US1 route/stop behavior.
- **US3 Record Boarding and Drop Scans (P1)**: Can start after Foundational with seeded route/assignment prerequisites, but production integration depends on US1 and US2 and includes scan-ready trip create/start/end.
- **US4 Track Active Bus Trips (P2)**: Can start after Foundational with seeded route/vehicle/trip context, extends the US3 trip lifecycle, and guardian onboard visibility depends on US3 scan outcomes.
- **US5 Calculate ETAs for Stops and Students (P2)**: Can start after Foundational with seeded trip/location context, but final integration depends on US4 tracking.
- **US6 Notify Guardians About Transport Events (P3)**: Can start after Foundational with seeded source events, but final integration depends on US3 scan outcomes and US5 ETA events.

### Within Each User Story

- Tests required by the constitution MUST be written and fail before implementation.
- Domain models before EF configurations and migrations.
- EF configurations and migrations before services that persist records.
- Services before controllers.
- Tenant, capability, permission, guardian visibility, idempotency, and audit enforcement before UI exposure.
- Backend contracts before web/mobile UI completion.
- Core implementation before integration, performance, and quickstart validation.

### Parallel Opportunities

- Setup tasks T002-T006 and T009-T013 can run in parallel.
- Foundational adapter, test, web client, mobile client, and test-data tasks marked `[P]` can run in parallel after T014-T021 are understood.
- Once Foundational phase completes, US1, US2, and US3 can start in parallel if seeded dependencies are used.
- US4, US5, and US6 can start in parallel with seeded evidence after Foundational, but production integration is simpler in priority order.
- Tests marked `[P]` inside a user story can run in parallel because they target separate files.
- Web, mobile, OpenAPI examples, and test-data builder tasks marked `[P]` can run in parallel after the story's backend DTOs and contracts are understood.

---

## Parallel Example: User Story 1

```bash
Task: "T043 Create route/stop lifecycle unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopDomainTests.cs"
Task: "T045 Create route/stop contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Routes/RouteStopManagementContractTests.cs"
Task: "T047 Create admin web route and stop management journey tests in apps/admin-web/tests/transport/route-stop-management.spec.ts"
```

## Parallel Example: User Story 2

```bash
Task: "T062 Create vehicle lifecycle tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/TransportVehicleTests.cs"
Task: "T065 Create guardian plan visibility tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Assignments/GuardianTransportPlanVisibilityTests.cs"
Task: "T069 Create guardian transport plan journey tests in apps/admin-web/tests/transport/guardian-transport-plan.spec.ts"
```

## Parallel Example: User Story 3

```bash
Task: "T087 Create scan-ready trip lifecycle and scan context tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Scans/TransportTripScanContextTests.cs"
Task: "T093 Create mobile offline queue tests in apps/mobile/test/features/transport/scans/offline_transport_scan_queue_test.dart"
Task: "T095 Create admin web scan review journey tests in apps/admin-web/tests/transport/boarding-drop-scan.spec.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T119 Create trip lifecycle and overlap tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/TransportTripLifecycleTests.cs"
Task: "T121 Create guardian trip progress visibility tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Tracking/GuardianTripProgressVisibilityTests.cs"
Task: "T126 Create mobile trip tracking tests in apps/mobile/test/features/transport/tracking/trip_tracking_test.dart"
```

## Parallel Example: User Story 5

```bash
Task: "T153 Create ETA calculation tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaCalculationServiceTests.cs"
Task: "T156 Create ETA contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Eta/EtaCalculationContractTests.cs"
Task: "T159 Create guardian ETA journey tests in apps/admin-web/tests/transport/guardian-transport-eta.spec.ts"
```

## Parallel Example: User Story 6

```bash
Task: "T176 Create transport notification eligibility tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationEligibilityTests.cs"
Task: "T179 Create notification contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Transport/Notifications/TransportNotificationContractTests.cs"
Task: "T182 Create guardian notification journey tests in apps/admin-web/tests/transport/guardian-transport-notification.spec.ts"
```

---

## Implementation Strategy

### MVP First

1. Complete Phase 1 Setup.
2. Complete Phase 2 Foundational.
3. Complete Phase 3 US1 Manage Routes and Stops.
4. Complete Phase 4 US2 Assign Students to Buses and Stops.
5. Complete Phase 5 US3 Record Boarding and Drop Scans.
6. Stop and validate route setup, assignment setup, scan-ready trip create/start/end, online scan, offline sync, invalid credential denial, wrong route/stop anomaly review approval, and no campus attendance/access outcome.

### Incremental Delivery

1. Setup + Foundation -> shared Transport capability, tenant, permission, identity, guardian, audit, idempotency, and client scaffolding.
2. US1 -> route and stop management MVP.
3. US2 -> vehicle and student assignment management.
4. US3 -> scan-ready active trip lifecycle, boarding/drop scan safety evidence, scan review approval, and offline sync.
5. US4 -> extended active trip tracking and guardian live-location boundaries.
6. US5 -> ETA calculation and ETA visibility.
7. US6 -> guardian notification records.
8. Polish -> anomaly review completion, performance, retention, security, observability, docs, and quickstart validation.

### Parallel Team Strategy

With multiple implementers:

1. Team completes Setup and Foundational together.
2. Developer A: US1 route/stop management.
3. Developer B: US2 vehicle/assignment management using seeded US1 records until US1 lands.
4. Developer C: US3 scan-ready trip lifecycle, scan capture, scan review approval, and offline sync using seeded route/assignment records until US1/US2 land.
5. Developer D: US4 live tracking using US3 trip/scan records or seeded equivalents, then integrates with US3.
6. Developer E: US5 ETA using seeded location records, then integrates with US4.
7. Developer F: US6 notification records using seeded source events, then integrates with US3/US5.

## Notes

- `[P]` tasks use different files and can run in parallel after their dependencies are understood.
- `[US#]` labels map tasks to specific user stories for traceability.
- Each user story is independently testable with seeded prerequisite data, even when production integration is delivered in priority order.
- Tests must fail before implementation, then pass after the implementation task group is complete.
- Commit after each task or logical group and do not include unrelated refactors.
