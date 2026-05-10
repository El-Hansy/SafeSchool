# Tasks: Phase 1 Identity & Access

**Input**: Design documents from `/specs/002-identity-access/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and plan require unit, integration, contract, authorization, tenant-isolation, audit, and critical UI/mobile journey coverage. Write test tasks before implementation tasks in each user-story phase and confirm they fail for the missing behavior before completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a task is marked `[P]`. Do not implement Phase 2 or Phase 3 product outcomes such as attendance, campus access decisions, transport boarding, wallet, communications, documents, or search. Every sensitive action must resolve tenant context, check feature capability, enforce permission, record denied access decisions, and emit audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the runtime project skeleton and test harnesses described in [plan.md](./plan.md).

- [x] T001 Create API solution file and source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/
- [x] T002 [P] Create API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, and OpenAPI dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [x] T003 [P] Create API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [x] T004 [P] Create admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, and test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [x] T005 [P] Create mobile package manifest with Flutter test, SQLite local storage, and NFC/QR integration placeholders in apps/mobile/pubspec.yaml
- [x] T006 [P] Create repository coding defaults and generated-file ignores in .editorconfig and .gitignore
- [x] T007 Create API bootstrap with versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, and IdentityAccess endpoint registration placeholders in apps/api/src/SafeSchool.Api/Program.cs
- [x] T008 [P] Create API configuration placeholders for connection strings, JWT, logging, audit, and feature settings in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [x] T009 [P] Create admin web shell route for the Identity & Access area in apps/admin-web/src/app/layout.tsx and apps/admin-web/src/app/(school)/identity-access/page.tsx
- [x] T010 [P] Create mobile IdentityAccess module export shell in apps/mobile/lib/features/identity_access/identity_access.dart
- [x] T011 [P] Create contract test documentation index linking the four Phase 1 contracts in tests/contracts/identity-access/README.md
- [x] T012 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/identity-access/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, persistence, audit, and client foundations required by every user story.

**Critical**: No user-story implementation should start until this phase is complete.

- [x] T013 Create shared tenant-owned entity base, result types, validation error type, and paged response type in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/Common/OperationResults.cs
- [x] T014 [P] Create shared IdentityAccess lifecycle enums from data-model.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Common/IdentityAccessEnums.cs
- [x] T015 Create SafeSchoolDbContext and design-time factory for PostgreSQL in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContextFactory.cs
- [x] T016 Create IdentityAccess model-builder extension for tenant-owned metadata, timestamps, indexes, and feature modules in apps/api/src/SafeSchool.Api/Features/IdentityAccess/IdentityAccessDbContextModelBuilderExtensions.cs
- [x] T017 Create tenant context abstraction and tenant resolution middleware that rejects missing or mismatched school account context in apps/api/src/SafeSchool.Api/Infrastructure/Tenancy/TenantContext.cs and apps/api/src/SafeSchool.Api/Infrastructure/Tenancy/TenantResolutionMiddleware.cs
- [x] T018 Create Phase 1 capability constants and feature gate service for identity.student_profiles, identity.guardian_linking, identity.nfc_credentials, identity.qr_fallback, identity.role_administration, and identity.permission_enforcement in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/IdentityAccessCapabilities.cs and apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/FeatureGateService.cs
- [x] T019 Create permission requirement, seeded default permission catalog, school administrator role seed, and working permission guard for sensitive actions in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Authorization/PermissionGuard.cs, apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/PermissionCatalog.cs, and apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/SeededIdentityAccessRoles.cs
- [x] T020 Create AuditEvent entity and audit writer abstraction for tenant-scoped Phase 1 events in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Audit/AuditEvent.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/Audit/AuditWriter.cs
- [x] T021 Create AccessDecision entity and writer abstraction for allow/deny outcomes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/AccessDecision.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/AccessDecisionWriter.cs
- [x] T022 Create consistent API error response middleware for validation, disabled capability, missing permission, tenant mismatch, and not-found-without-cross-tenant-leakage cases in apps/api/src/SafeSchool.Api/Infrastructure/Errors/ApiErrorMiddleware.cs
- [x] T023 Create IdentityAccess endpoint group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/identity in apps/api/src/SafeSchool.Api/Features/IdentityAccess/IdentityAccessEndpointRegistration.cs
- [x] T024 Create API test fixture for tenants, capabilities, users, permissions, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Fixtures/IdentityAccessTestFixture.cs
- [x] T025 [P] Create unit tests for disabled and enabled Phase 1 feature capability decisions in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Foundational/FeatureGateServiceTests.cs
- [x] T026 [P] Create unit tests for tenant context resolution, tenant mismatch, and missing tenant behavior in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Foundational/TenantResolutionMiddlewareTests.cs
- [x] T027 [P] Create unit tests for audit writer, access decision writer, seeded school administrator permission guard, and default denial behavior in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Foundational/AuditAccessDecisionAndPermissionGuardTests.cs
- [x] T028 Create initial EF migration for shared IdentityAccess audit, access decision, and feature setting foundations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605030001_IdentityAccessFoundation.cs
- [x] T029 [P] Create admin web IdentityAccess API client with tenant header handling, typed errors, and feature-disabled handling in apps/admin-web/src/features/identity-access/api/client.ts
- [x] T030 [P] Create mobile IdentityAccess API client shell with tenant context and credential status fetch placeholder in apps/mobile/lib/features/identity_access/identity_access_api.dart

**Checkpoint**: Foundation ready. User-story work can start after T013-T030 are complete.

---

## Phase 3: User Story 1 - Maintain Student Identity Profile (Priority: P1)

**Goal**: School administrators can create, update, deactivate, review, and duplicate-check student identity profiles within one school account.

**Independent Test**: Create a student profile, block a duplicate active profile for the same configured identifiers, update controlled fields, and verify audit history inside one tenant without cross-tenant leakage.

### Tests for User Story 1

- [x] T031 [P] [US1] Create StudentProfile lifecycle unit tests for Draft, Active, Suspended, Deactivated, Archived, and invalid transitions in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/StudentProfiles/StudentProfileTests.cs
- [x] T032 [P] [US1] Create duplicate active identity detection unit tests for school_student_number and external identity references in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/StudentProfiles/DuplicateStudentProfileDetectorTests.cs
- [x] T033 [P] [US1] Create contract tests for student profile create, list, read, patch, deactivate, history, and duplicate-check routes in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/StudentProfiles/StudentProfilesContractTests.cs
- [x] T034 [P] [US1] Create integration tests for tenant isolation, disabled identity.student_profiles capability, missing permission, and audit evidence on student profile operations in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/StudentProfiles/StudentProfilesIntegrationTests.cs
- [x] T035 [P] [US1] Create admin web journey tests for create, duplicate rejection, update, deactivate, and history review in apps/admin-web/tests/identity-access/student-profile.spec.ts

### Implementation for User Story 1

- [x] T036 [P] [US1] Create StudentProfile domain model and status enums in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfile.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileStatus.cs
- [x] T037 [P] [US1] Create StudentProfile request/response DTOs matching contracts/student-profile.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileDtos.cs
- [x] T038 [US1] Create EF configuration and migration for StudentProfile unique tenant-scoped active identifiers and review fields in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605030002_StudentProfiles.cs
- [x] T039 [US1] Implement duplicate detection service with review-required output for ambiguous matches in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/DuplicateStudentProfileDetector.cs
- [x] T040 [US1] Implement StudentProfileService for create, update, deactivate, duplicate-check, list, read, and history behavior in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileService.cs
- [x] T041 [US1] Implement StudentProfilesController routes from contracts/student-profile.md under /api/v1/schools/{schoolAccountId}/identity/students in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfilesController.cs
- [x] T042 [US1] Wire profile create, update, duplicate conflict, and deactivate audit events plus access decisions in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileAuditAdapter.cs
- [x] T043 [US1] Add OpenAPI examples for student profile requests, responses, pagination, and error outcomes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/StudentProfiles/StudentProfileOpenApiExamples.cs
- [x] T044 [P] [US1] Create admin web student profile types and API hooks in apps/admin-web/src/features/identity-access/student-profiles/studentProfilesApi.ts
- [x] T045 [P] [US1] Create admin web student profile form, table, duplicate warning, and history timeline components in apps/admin-web/src/features/identity-access/student-profiles/StudentProfileForm.tsx and apps/admin-web/src/features/identity-access/student-profiles/StudentProfileTable.tsx
- [x] T046 [US1] Create admin web student profiles route that uses the components and API hooks in apps/admin-web/src/app/(school)/identity-access/students/page.tsx
- [x] T047 [US1] Create StudentProfile test data builder for all US1 tests in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/StudentProfiles/StudentProfileTestData.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T031-T047 pass.

---

## Phase 4: User Story 2 - Enforce Identity Permissions (Priority: P1)

**Goal**: Sensitive identity and access actions are allowed only for actors with active tenant-scoped roles, active permissions, enabled capabilities, and valid target ownership.

**Independent Test**: Execute representative identity actions as platform owner, school admin, staff, guardian, student, and reviewer actors, then verify allowed actions succeed and unauthorized actions produce access decision and audit evidence.

### Tests for User Story 2

- [x] T048 [P] [US2] Create PermissionEvaluator unit tests for tenant access, feature capability, active role assignment, required permission, and target ownership decisions in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/AccessControl/PermissionEvaluatorTests.cs
- [x] T049 [P] [US2] Create Role and ActorRoleAssignment lifecycle unit tests for Active, Suspended, Revoked, Expired, Deprecated, and invalid assignment cases in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/AccessControl/RoleAssignmentTests.cs
- [x] T050 [P] [US2] Create contract tests for paginated roles, permissions, role assignments, access decisions, and audit-events routes in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/AccessControl/PermissionEnforcementContractTests.cs
- [x] T051 [P] [US2] Create integration tests proving unauthorized profile, guardian, credential, role, and permission actions are denied before mutation and audited in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/AccessControl/PermissionEnforcementIntegrationTests.cs
- [x] T052 [P] [US2] Create admin web tests for role management, role assignment, access decision review, and audit review in apps/admin-web/tests/identity-access/access-control.spec.ts

### Implementation for User Story 2

- [x] T053 [P] [US2] Create Role, Permission, RolePermission, and ActorRoleAssignment domain models in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Role.cs, apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Permission.cs, apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/RolePermission.cs, and apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/ActorRoleAssignment.cs
- [x] T054 [P] [US2] Create RBAC request/response DTOs and paginated list DTOs matching contracts/permission-enforcement.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/AccessControlDtos.cs
- [x] T055 [US2] Create EF configurations and migration for roles, permissions, role permissions, actor role assignments, and access-decision indexes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/AccessControlEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605030003_AccessControl.cs
- [x] T056 [US2] Extend the seeded PermissionCatalog with role administration, permission administration, audit, and reviewer permission families in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/PermissionCatalog.cs
- [x] T057 [US2] Implement RolePermissionService for role create, update, permission replacement, assignment create, assignment state changes, and history behavior in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/RolePermissionService.cs
- [x] T058 [US2] Implement PermissionEvaluator that checks tenant, capability, assignment state, permission, and target ownership before business logic runs in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Authorization/PermissionEvaluator.cs
- [x] T059 [US2] Implement paginated RolesController, PermissionsController, and RoleAssignmentsController routes from contracts/permission-enforcement.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/AccessControlControllers.cs
- [x] T060 [US2] Implement paginated AccessDecisionsController and AuditEventsController with reviewer filters in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/ReviewControllers.cs
- [x] T061 [US2] Extend the foundational permission guard with role administration, assignment lifecycle, and reviewer access behavior in apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Authorization/PermissionEvaluator.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/AccessControl/Authorization/PermissionGuard.cs
- [x] T062 [P] [US2] Create admin web access-control types and API hooks in apps/admin-web/src/features/identity-access/access-control/accessControlApi.ts
- [x] T063 [P] [US2] Create admin web access-control route for roles, assignments, permissions, access decisions, and audit events in apps/admin-web/src/app/(school)/identity-access/access-control/page.tsx
- [x] T064 [US2] Create access decision table and audit event timeline components in apps/admin-web/src/features/identity-access/access-control/AccessDecisionTable.tsx and apps/admin-web/src/features/identity-access/access-control/AuditEventTimeline.tsx

**Checkpoint**: User Story 2 can be demonstrated independently after T048-T064 pass.

---

## Phase 5: User Story 3 - Link Guardians to Students (Priority: P2)

**Goal**: School administrators can create guardian records and manage approved, suspended, expired, rejected, or removed guardian links with explicit access scope.

**Independent Test**: Create a guardian record, create an approved guardian link to one student, verify guardian-visible students match link scope, then suspend/remove the link and verify access is denied with review evidence.

### Tests for User Story 3

- [x] T065 [P] [US3] Create GuardianRecord unit tests for Verified, Rejected, Suspended, Deactivated, and no-link-no-visibility behavior in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Guardians/GuardianRecordTests.cs
- [x] T066 [P] [US3] Create GuardianLink lifecycle unit tests for Pending, Approved, Suspended, Expired, Rejected, Removed, validity window, and explicit access scope rules in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Guardians/GuardianLinkTests.cs
- [x] T067 [P] [US3] Create contract tests for guardian records, guardian links, link suspend/remove, history, and guardian-visible students routes in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Guardians/GuardianLinkingContractTests.cs
- [x] T068 [P] [US3] Create integration tests for same-tenant guardian/student links, cross-tenant denial, disabled identity.guardian_linking capability, missing permission, and link-state access denial in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Guardians/GuardianLinkingIntegrationTests.cs
- [x] T069 [P] [US3] Create admin web guardian linking tests for create guardian, approve link, suspend link, remove link, and visibility review in apps/admin-web/tests/identity-access/guardian-linking.spec.ts

### Implementation for User Story 3

- [x] T070 [P] [US3] Create GuardianRecord, GuardianLink, relationship type, link status, and access scope domain models in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianRecord.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianLink.cs
- [x] T071 [P] [US3] Create guardian and guardian-link DTOs matching contracts/guardian-linking.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianDtos.cs
- [x] T072 [US3] Create EF configurations and migration for guardians, guardian links, tenant indexes, student/guardian relationships, and link state indexes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605030004_GuardianLinking.cs
- [x] T073 [US3] Implement GuardianService for guardian create, update, list, read, and identity review behavior in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianService.cs
- [x] T074 [US3] Implement GuardianLinkService for create, approve, update scope, suspend, remove, expire, history, and lifecycle validation in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianLinkService.cs
- [x] T075 [US3] Implement GuardianVisibilityService that calculates guardian-visible students per tenant, link state, validity window, access scope, and feature availability in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianVisibilityService.cs
- [x] T076 [US3] Implement GuardiansController and GuardianLinksController routes from contracts/guardian-linking.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardiansController.cs and apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianLinksController.cs
- [x] T077 [US3] Wire guardian record and guardian link audit events plus access decisions for create, update, approve, suspend, remove, and visibility-denied outcomes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Guardians/GuardianAuditAdapter.cs
- [x] T078 [P] [US3] Create admin web guardian and guardian-link types and API hooks in apps/admin-web/src/features/identity-access/guardians/guardiansApi.ts
- [x] T079 [P] [US3] Create guardian record form, guardian link form, link state badge, access scope editor, and guardian visibility table components in apps/admin-web/src/features/identity-access/guardians/GuardianForm.tsx and apps/admin-web/src/features/identity-access/guardians/GuardianLinkTable.tsx
- [x] T080 [US3] Create admin web guardian linking route in apps/admin-web/src/app/(school)/identity-access/guardians/page.tsx

**Checkpoint**: User Story 3 can be demonstrated independently after T065-T080 pass.

---

## Phase 6: User Story 4 - Manage NFC and QR Identity Credentials (Priority: P3)

**Goal**: Authorized staff can issue, suspend, restore, replace, expire, revoke, rotate, and review NFC card and QR fallback credentials for active student profiles.

**Independent Test**: Issue NFC and QR credentials for an active student, change lifecycle states, verify only active unexpired credentials appear as current identity evidence, and block QR workflows when identity.qr_fallback is disabled.

### Tests for User Story 4

- [x] T081 [P] [US4] Create IdentityCredential lifecycle unit tests for Proposed, Active, Suspended, Replaced, Expired, Revoked, validity window, replacement, and invalid transitions in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Credentials/IdentityCredentialTests.cs
- [x] T082 [P] [US4] Create QR fallback unit tests for required validity window, rotation sequence, disabled capability denial, and prior credential replacement in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Credentials/QrFallbackCredentialTests.cs
- [x] T083 [P] [US4] Create contract tests for NFC issue, credential suspend/restore/replace/revoke, QR create/rotate, credential list, history, and status snapshot routes in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Credentials/CredentialLifecycleContractTests.cs
- [x] T084 [P] [US4] Create integration tests for active student requirement, duplicate active NFC card reference, tenant isolation, disabled capability, missing permission, audit evidence, and retry-safe lifecycle commands in apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess/Credentials/CredentialLifecycleIntegrationTests.cs
- [x] T085 [P] [US4] Create admin web credential lifecycle tests for issue, suspend, restore, replace, revoke, create QR, rotate QR, and status review in apps/admin-web/tests/identity-access/credential-lifecycle.spec.ts
- [x] T086 [P] [US4] Create mobile credential status snapshot and QR fallback tests in apps/mobile/test/features/identity_access/credential_status_snapshot_test.dart
- [x] T087 [P] [US4] Create mobile SQLite cache tests for cached snapshot reads, expired snapshot handling, refresh failure fallback, and offline mode in apps/mobile/test/features/identity_access/credential_status_cache_test.dart

### Implementation for User Story 4

- [x] T088 [P] [US4] Create IdentityCredential, NFC Card Credential, QR Fallback Credential, and Credential Status Snapshot domain models in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/IdentityCredential.cs, apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/NfcCardCredential.cs, apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/QrFallbackCredential.cs, and apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialStatusSnapshot.cs
- [x] T089 [P] [US4] Create credential lifecycle DTOs matching contracts/credential-lifecycle.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialDtos.cs
- [x] T090 [US4] Create EF configurations and migration for identity credentials, NFC credentials, QR credentials, status snapshots, unique active card references, and status indexes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605030005_CredentialLifecycle.cs
- [x] T091 [US4] Implement CredentialLifecycleService for issue NFC, suspend, restore, replace, revoke, expire, list, history, and retry-safe client_request_id handling in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialLifecycleService.cs
- [x] T092 [US4] Implement QrFallbackCredentialService for QR create, rotate, validity window enforcement, capability checks, and prior credential replacement in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/QrFallbackCredentialService.cs
- [x] T093 [US4] Implement CredentialStatusSnapshotService that excludes suspended, replaced, expired, or revoked credentials from current identity evidence in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialStatusSnapshotService.cs
- [x] T094 [US4] Implement CredentialsController routes from contracts/credential-lifecycle.md in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialsController.cs
- [x] T095 [US4] Wire credential lifecycle audit events and access decisions for issue, suspend, restore, replace, revoke, expire, QR create, QR rotate, and status snapshot denial outcomes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/Credentials/CredentialAuditAdapter.cs
- [x] T096 [P] [US4] Create admin web credential types and API hooks in apps/admin-web/src/features/identity-access/credentials/credentialsApi.ts
- [x] T097 [P] [US4] Create admin web credential issue form, credential table, lifecycle action menu, QR rotation form, and status snapshot viewer components in apps/admin-web/src/features/identity-access/credentials/CredentialIssueForm.tsx and apps/admin-web/src/features/identity-access/credentials/CredentialTable.tsx
- [x] T098 [US4] Create admin web student credentials route in apps/admin-web/src/app/(school)/identity-access/credentials/page.tsx
- [x] T099 [US4] Implement mobile credential status snapshot client and local model parsing in apps/mobile/lib/features/identity_access/credential_status_snapshot.dart and apps/mobile/lib/features/identity_access/credential_status_repository.dart
- [x] T100 [US4] Implement SQLite-backed credential status snapshot cache with expiry, refresh metadata, and offline reads in apps/mobile/lib/features/identity_access/credential_status_cache.dart
- [x] T101 [US4] Create mobile credential status review screen using cached current identity evidence without attendance or transport outcomes in apps/mobile/lib/features/identity_access/credential_status_screen.dart

**Checkpoint**: User Story 4 can be demonstrated independently after T081-T101 pass.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full Phase 1 implementation, harden security and observability, and preserve traceability for future phases.

- [x] T102 [P] Generate or update OpenAPI documentation for all IdentityAccess routes in apps/api/src/SafeSchool.Api/Features/IdentityAccess/IdentityAccessOpenApi.md
- [x] T103 [P] Create end-to-end tenant isolation and feature-disabled scenarios across student, guardian, credential, role, permission, audit, and access decision workflows in tests/e2e/identity-access/identity-access.e2e.spec.ts
- [x] T104 [P] Create security review checklist for tenant boundaries, feature gates, permissions, audit evidence, raw credential exposure, and cross-tenant not-found behavior in docs/identity-access/security-review.md
- [x] T105 [P] Create observability review for logs, metrics, audit event categories, access-denial dashboards, and centralized error reporting in docs/identity-access/observability.md
- [x] T106 [P] Create Phase 1 traceability matrix mapping FR-001 through FR-018 and SC-001 through SC-007 to implemented tests and files in docs/identity-access/traceability.md
- [x] T107 Run API test suite and record relevant output for IdentityAccess in docs/identity-access/backend-test-results.md
- [x] T108 Run admin web test suite and record relevant output for IdentityAccess in docs/identity-access/admin-web-test-results.md
- [x] T109 Run mobile test suite and record relevant output for IdentityAccess in docs/identity-access/mobile-test-results.md
- [x] T110 Run quickstart.md validation scenarios end-to-end and record pass/fail evidence in docs/identity-access/quickstart-validation.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1 completion and blocks all user stories.
- **Phase 3 US1**: Depends on Phase 2.
- **Phase 4 US2**: Depends on Phase 2. US2 can run in parallel with US1, but production-safe delivery should complete US1 and US2 together because both are P1.
- **Phase 5 US3**: Depends on Phase 2 and uses StudentProfile references from US1 plus foundational permission guard behavior for full integration tests.
- **Phase 6 US4**: Depends on Phase 2 and uses active StudentProfile references from US1 plus foundational permission guard behavior for full integration tests.
- **Phase 7 Polish**: Depends on all desired user stories being complete.

### User Story Dependencies

- **US1 Maintain Student Identity Profile (P1)**: Can start after Phase 2. Delivers student profile MVP with tenant, capability, permission, and audit enforcement.
- **US2 Enforce Identity Permissions (P1)**: Can start after Phase 2. Extends the foundational permission guard with role/permission administration and reviewer workflows used by all stories.
- **US3 Link Guardians to Students (P2)**: Best started after US1 for real student references; it uses the Phase 2 foundational permission guard and gains richer reviewer workflows after US2.
- **US4 Manage NFC and QR Identity Credentials (P3)**: Best started after US1 for real active student references; it uses the Phase 2 foundational permission guard and gains richer reviewer workflows after US2.

### Within Each User Story

- Create tests before implementation and confirm they fail for missing behavior.
- Create domain models before EF configuration.
- Create EF configuration and migrations before services that persist records.
- Create services before controllers.
- Create API hooks before UI pages.
- Wire tenant, capability, permission, access-decision, and audit behavior before exposing UI actions.

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
- T009 admin shell
- T010 mobile shell
```

### User Story 1

```text
Run together after Phase 2:
- T031 StudentProfile lifecycle tests
- T032 duplicate detection tests
- T033 contract tests
- T034 integration tests
- T035 admin web journey tests

Run together after T031-T035 are committed:
- T036 StudentProfile domain model
- T037 StudentProfile DTOs
- T044 admin web API hooks
- T045 admin web components
```

### User Story 2

```text
Run together after Phase 2:
- T048 PermissionEvaluator tests
- T049 RoleAssignment tests
- T050 contract tests
- T051 integration tests
- T052 admin web tests

Run together after test files exist:
- T053 RBAC domain models
- T054 RBAC DTOs
- T062 admin web API hooks
- T063 admin web route
```

### User Story 3

```text
Run together after Phase 2 and preferably after US1/US2:
- T065 GuardianRecord tests
- T066 GuardianLink tests
- T067 contract tests
- T068 integration tests
- T069 admin web tests

Run together after test files exist:
- T070 Guardian domain models
- T071 Guardian DTOs
- T078 admin web API hooks
- T079 admin web components
```

### User Story 4

```text
Run together after Phase 2 and preferably after US1/US2:
- T081 IdentityCredential tests
- T082 QR fallback tests
- T083 contract tests
- T084 integration tests
- T085 admin web tests
- T086 mobile tests

Run together after test files exist:
- T088 credential domain models
- T089 credential DTOs
- T096 admin web API hooks
- T097 admin web components
```

---

## Implementation Strategy

### Production-Safe MVP

1. Complete Phase 1 Setup.
2. Complete Phase 2 Foundational.
3. Complete Phase 3 US1 Maintain Student Identity Profile.
4. Complete Phase 4 US2 Enforce Identity Permissions.
5. Stop and validate: student profile creation, duplicate detection, tenant isolation, feature-disabled denial, permission denial, and audit evidence.

US1 alone can be demoed after Phase 3, but production-safe delivery should include US2 because permission enforcement is also Priority P1 and protects every sensitive workflow.

### Incremental Delivery

1. Deliver US1 + US2 as the secure identity MVP.
2. Add US3 Guardian Linking and validate guardian visibility by link state and scope.
3. Add US4 NFC/QR Credential Lifecycle and validate status snapshots without attendance, campus access, or transport outcomes.
4. Complete Phase 7 polish and quickstart validation.

### Handoff Rules for Cheaper LLM Executors

- Work one task at a time unless the task is explicitly marked `[P]`.
- Do not combine unrelated tasks or refactor outside the listed paths.
- For every backend endpoint, implement tenant resolution, feature capability check, permission guard, validation, error outcome, audit event, and tests in the same story phase.
- For every UI action, handle disabled capability, missing permission, validation errors, loading state, empty state, and audit/history visibility where the story requires it.
- Never expose raw NFC or QR secrets in UI, logs, audit events, errors, or tests.
- Do not implement attendance, campus entry/exit decisions, bus boarding, wallet payments, notifications, documents, or search in Phase 1.
