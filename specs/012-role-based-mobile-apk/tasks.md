# Tasks: Phase 12 Role-Based Mobile App & APK Release

**Input**: Design documents from `/specs/012-role-based-mobile-apk/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Tests are included because the feature spec and constitution require
unit, contract, integration, mobile journey, permission, tenant-isolation,
offline sync, APK release, support evidence, and Arabic/English RTL validation.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (US1, US2, US3, US4)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the mobile module, web control, mobile app, and test structure needed by Phase 12.

- [X] T001 Create Mobile API module directories in apps/api/src/SafeSchool.Api/Features/Mobile/
- [X] T002 [P] Create Mobile API test directories in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/
- [X] T003 [P] Create mobile admin web route directories in apps/admin-web/src/app/(school)/mobile/
- [X] T004 [P] Create mobile admin web feature directories in apps/admin-web/src/features/mobile/
- [X] T005 [P] Create Flutter core mobile directories in apps/mobile/lib/core/
- [X] T006 [P] Create Flutter role workspace directories in apps/mobile/lib/features/
- [X] T007 [P] Create Flutter mobile test directories in apps/mobile/test/features/mobile/
- [X] T008 [P] Create Flutter integration test directories in apps/mobile/integration_test/mobile/
- [X] T009 [P] Create contract and e2e test directories in tests/contracts/mobile/ and tests/e2e/mobile/
- [X] T010 Register Mobile feature configuration shell in apps/api/src/SafeSchool.Api/appsettings.json
- [X] T011 Register Mobile development configuration shell in apps/api/src/SafeSchool.Api/appsettings.Development.json
- [X] T012 Add APK artifact ignore rules in .gitignore
- [X] T013 Configure Flutter localization generation in apps/mobile/l10n.yaml
- [X] T014 Configure Android app version placeholders in apps/mobile/android/app/build.gradle.kts
- [X] T015 Add Phase 12 documentation index in docs/mobile/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core data, permission, tenant, localization, audit, and API foundations that must be complete before user stories.

**Critical**: No user story work can begin until this phase is complete.

- [X] T016 [P] Add Mobile domain model tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileDomainModelTests.cs
- [X] T017 [P] Add Mobile permission resolver tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobilePermissionResolverTests.cs
- [X] T018 [P] Add Mobile feature availability tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileFeatureAvailabilityTests.cs
- [X] T019 [P] Add Mobile localization tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileLocalizationTests.cs
- [X] T020 [P] Add Mobile audit event tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileAuditEventTests.cs
- [X] T021 [P] Add Flutter shared mobile shell tests in apps/mobile/test/features/mobile/mobile_shell_test.dart
- [X] T022 [P] Add Flutter Arabic/English localization baseline tests in apps/mobile/test/features/mobile/mobile_localization_test.dart
- [X] T023 [P] Add admin web mobile smoke tests in apps/admin-web/tests/mobile/mobileAdmin.spec.ts
- [X] T024 Define MobileUserProfile entity in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/MobileUserProfile.cs
- [X] T025 [P] Define RoleWorkspace and RoleWorkspaceAction entities in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/RoleWorkspace.cs
- [X] T026 [P] Define MobilePermissionGrant entity in apps/api/src/SafeSchool.Api/Features/Mobile/Permissions/MobilePermissionGrant.cs
- [X] T027 [P] Define TenantMobileFeatureAvailability entity in apps/api/src/SafeSchool.Api/Features/Mobile/Permissions/TenantMobileFeatureAvailability.cs
- [X] T028 [P] Define MobileLanguagePreference entity in apps/api/src/SafeSchool.Api/Features/Mobile/Localization/MobileLanguagePreference.cs
- [X] T029 [P] Define DeviceSession entity in apps/api/src/SafeSchool.Api/Features/Mobile/DeviceSessions/DeviceSession.cs
- [X] T030 [P] Define ApkRelease and ReleaseAudience entities in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ApkRelease.cs
- [X] T031 [P] Define InstallOrUpgradeEvent entity in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/InstallOrUpgradeEvent.cs
- [X] T032 [P] Define OfflineActionQueue entity in apps/api/src/SafeSchool.Api/Features/Mobile/OfflineSync/OfflineActionQueue.cs
- [X] T033 [P] Define MobileAuditEvent entity in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileAuditEvent.cs
- [X] T034 Map Mobile entities and indexes in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [X] T035 Create Mobile EF Core migration in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/
- [X] T036 Implement Mobile tenant-owned repository interfaces in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileRepositories.cs
- [X] T037 Implement Mobile DTOs and error codes in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileDtos.cs
- [X] T038 Implement MobilePermissionResolver service in apps/api/src/SafeSchool.Api/Features/Mobile/Permissions/MobilePermissionResolver.cs
- [X] T039 Implement MobileFeatureAvailabilityService in apps/api/src/SafeSchool.Api/Features/Mobile/Permissions/MobileFeatureAvailabilityService.cs
- [X] T040 Implement MobileLanguageService in apps/api/src/SafeSchool.Api/Features/Mobile/Localization/MobileLanguageService.cs
- [X] T041 Implement DeviceSessionService in apps/api/src/SafeSchool.Api/Features/Mobile/DeviceSessions/DeviceSessionService.cs
- [X] T042 Implement MobileAuditService and metrics hooks in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileAuditService.cs
- [X] T043 Implement source-domain action adapter interfaces in apps/api/src/SafeSchool.Api/Features/Mobile/Common/SourceDomainActionAdapters.cs
- [X] T044 Implement offline policy adapter interfaces in apps/api/src/SafeSchool.Api/Features/Mobile/OfflineSync/OfflinePolicyAdapters.cs
- [X] T045 Implement Mobile seed catalog for all role workspaces in apps/api/src/SafeSchool.Api/Features/Mobile/Seed/MobileSeedCatalog.cs
- [X] T046 Register Mobile services and endpoint group in apps/api/src/SafeSchool.Api/Features/Mobile/MobileModule.cs
- [X] T047 Wire MobileModule into app startup in apps/api/src/SafeSchool.Api/Program.cs
- [X] T048 Implement shared Flutter API client for mobile endpoints in apps/mobile/lib/core/api/mobile_api_client.dart
- [X] T049 Implement shared Flutter auth and tenant context store in apps/mobile/lib/core/auth/mobile_auth_context.dart
- [X] T050 Implement shared Flutter localization loader in apps/mobile/lib/core/localization/mobile_localizations.dart
- [X] T051 Implement shared Flutter offline queue store in apps/mobile/lib/core/offline/mobile_offline_store.dart
- [X] T052 Implement admin web mobile API client in apps/admin-web/src/features/mobile/api/mobileApi.ts
- [X] T053 Implement admin web mobile feature layout shell in apps/admin-web/src/features/mobile/components/MobileAdminLayout.tsx

**Checkpoint**: Foundation ready. User story implementation can now proceed.

---

## Phase 3: User Story 1 - Open One App With the Right Role Workspace (Priority: P1) MVP

**Goal**: A signed-in user sees only the tenant, role workspace, linked students, language, navigation, and actions allowed by their current role and permissions.

**Independent Test**: Sign in with guardian-only, student-only, staff-only, no-mobile-access, and multi-role accounts; verify correct role context, Arabic/English layout, denied access, and no unauthorized actions.

### Tests for User Story 1

- [X] T054 [P] [US1] Add profile resolver unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/ProfileResolverTests.cs
- [X] T055 [P] [US1] Add profile and context contract tests in tests/contracts/mobile/mobile-profile-context.contract.md
- [X] T056 [P] [US1] Add workspace visibility contract tests in tests/contracts/mobile/mobile-workspaces.contract.md
- [X] T057 [P] [US1] Add tenant-role-feature integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileContextIntegrationTests.cs
- [X] T058 [P] [US1] Add denied deep-link integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileDeniedAccessTests.cs
- [X] T059 [P] [US1] Add Flutter sign-in and role switcher tests in apps/mobile/test/features/mobile/sign_in_role_switcher_test.dart
- [X] T060 [P] [US1] Add Flutter Arabic RTL shell tests in apps/mobile/test/features/mobile/mobile_shell_rtl_test.dart
- [X] T061 [P] [US1] Add admin web role matrix tests in apps/admin-web/tests/mobile/mobileRoleMatrix.spec.ts

### Implementation for User Story 1

- [X] T062 [US1] Implement MobileProfileService in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/MobileProfileService.cs
- [X] T063 [US1] Implement GET /api/v1/mobile/profile endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/MobileProfileEndpoints.cs
- [X] T064 [US1] Implement POST /api/v1/mobile/context endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/MobileContextEndpoints.cs
- [X] T065 [US1] Implement RoleWorkspaceResolver in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/RoleWorkspaceResolver.cs
- [X] T066 [US1] Implement GET /api/v1/mobile/workspaces endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/RoleWorkspaceEndpoints.cs
- [X] T067 [US1] Implement denied access response handling in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileDeniedAccess.cs
- [X] T068 [US1] Implement device session creation during context selection in apps/api/src/SafeSchool.Api/Features/Mobile/DeviceSessions/DeviceSessionService.cs
- [X] T069 [US1] Implement mobile sign-in shell in apps/mobile/lib/features/mobile_shell/mobile_sign_in_shell.dart
- [X] T070 [US1] Implement active tenant selector in apps/mobile/lib/features/mobile_shell/tenant_selector.dart
- [X] T071 [US1] Implement active role switcher in apps/mobile/lib/features/mobile_shell/role_switcher.dart
- [X] T072 [US1] Implement role workspace registry in apps/mobile/lib/features/role_workspaces/role_workspace_registry.dart
- [X] T073 [US1] Implement mobile workspace dashboard in apps/mobile/lib/features/mobile_shell/workspace_dashboard.dart
- [X] T074 [US1] Implement denied access and blocked feature screens in apps/mobile/lib/features/mobile_shell/denied_access_screen.dart
- [X] T075 [US1] Implement shared sign-out and cache clearing in apps/mobile/lib/features/mobile_shell/shared_device_sign_out.dart
- [X] T076 [US1] Add English mobile strings in apps/mobile/lib/core/localization/app_en.arb
- [X] T077 [US1] Add Arabic mobile strings in apps/mobile/lib/core/localization/app_ar.arb
- [X] T078 [US1] Implement RTL-aware mobile theme in apps/mobile/lib/app/mobile_theme.dart
- [X] T079 [US1] Implement guardian workspace shell in apps/mobile/lib/features/guardian/guardian_workspace.dart
- [X] T080 [US1] Implement student workspace shell in apps/mobile/lib/features/student/student_workspace.dart
- [X] T081 [US1] Implement transport driver workspace shell in apps/mobile/lib/features/transport_driver/transport_driver_workspace.dart
- [X] T082 [US1] Implement gate/access workspace shell in apps/mobile/lib/features/gate_access/gate_access_workspace.dart
- [X] T083 [US1] Implement canteen cashier workspace shell in apps/mobile/lib/features/canteen_cashier/canteen_cashier_workspace.dart
- [X] T084 [US1] Implement teacher workspace shell in apps/mobile/lib/features/teacher/teacher_workspace.dart
- [X] T085 [US1] Implement medical staff workspace shell in apps/mobile/lib/features/medical_staff/medical_staff_workspace.dart
- [X] T086 [US1] Implement complaint handler workspace shell in apps/mobile/lib/features/complaint_handler/complaint_handler_workspace.dart
- [X] T087 [US1] Implement communication sender workspace shell in apps/mobile/lib/features/communication_sender/communication_sender_workspace.dart
- [X] T088 [US1] Implement document administrator workspace shell in apps/mobile/lib/features/document_admin/document_admin_workspace.dart
- [X] T089 [US1] Implement school administrator workspace shell in apps/mobile/lib/features/school_admin/school_admin_workspace.dart
- [X] T090 [US1] Implement platform support workspace shell in apps/mobile/lib/features/platform_support/platform_support_workspace.dart
- [X] T091 [US1] Implement admin mobile role matrix page in apps/admin-web/src/app/(school)/mobile/roles/page.tsx
- [X] T092 [US1] Implement mobile role matrix component in apps/admin-web/src/features/mobile/components/MobileRoleMatrix.tsx
- [X] T093 [US1] Implement mobile permission assignment form in apps/admin-web/src/features/mobile/components/MobilePermissionGrantForm.tsx
- [X] T094 [US1] Seed demo-safe role users and workspaces in apps/api/src/SafeSchool.Api/Features/Mobile/Seed/MobileDemoSeed.cs
- [X] T095 [US1] Emit mobile sign-in, role selection, denied access, and language audit events in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileAuditService.cs

**Checkpoint**: User Story 1 is independently testable as the MVP role-based app shell.

---

## Phase 4: User Story 2 - Guardian and Student Complete Mobile Journeys (Priority: P2)

**Goal**: Guardians and students can review enabled summaries and submit allowed actions without using the school admin web portal.

**Independent Test**: Sign in as guardian and student demo accounts; complete linked-student summaries, guardian request/complaint submission, student views, notifications, hidden-feature checks, and Arabic/English layout validation.

### Tests for User Story 2

- [X] T096 [P] [US2] Add guardian mobile summary contract tests in tests/contracts/mobile/guardian-mobile-summary.contract.md
- [X] T097 [P] [US2] Add student mobile summary contract tests in tests/contracts/mobile/student-mobile-summary.contract.md
- [X] T098 [P] [US2] Add guardian linked-student integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/GuardianMobileIntegrationTests.cs
- [X] T099 [P] [US2] Add student self-scope integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/StudentMobileIntegrationTests.cs
- [X] T100 [P] [US2] Add guardian request and complaint action tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/GuardianMobileActionTests.cs
- [X] T101 [P] [US2] Add Flutter guardian journey tests in apps/mobile/test/features/guardian/guardian_journey_test.dart
- [X] T102 [P] [US2] Add Flutter student journey tests in apps/mobile/test/features/student/student_journey_test.dart
- [X] T103 [P] [US2] Add Flutter guardian/student RTL tests in apps/mobile/test/features/mobile/guardian_student_rtl_test.dart

### Implementation for User Story 2

- [X] T104 [US2] Implement guardian mobile summary DTOs in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/GuardianMobileDtos.cs
- [X] T105 [US2] Implement student mobile summary DTOs in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/StudentMobileDtos.cs
- [X] T106 [US2] Implement GuardianMobileService with approved link checks in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/GuardianMobileService.cs
- [X] T107 [US2] Implement StudentMobileService with self-scope checks in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/StudentMobileService.cs
- [X] T108 [US2] Implement guardian mobile endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/GuardianMobileEndpoints.cs
- [X] T109 [US2] Implement student mobile endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/StudentMobileEndpoints.cs
- [X] T110 [US2] Implement mobile notification center endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Notifications/MobileNotificationEndpoints.cs
- [X] T111 [US2] Implement guardian/student source action routing in apps/api/src/SafeSchool.Api/Features/Mobile/Common/GuardianStudentActionRouter.cs
- [X] T112 [US2] Implement guardian repository client in apps/mobile/lib/features/guardian/guardian_repository.dart
- [X] T113 [US2] Implement guardian student selector in apps/mobile/lib/features/guardian/guardian_student_selector.dart
- [X] T114 [US2] Implement guardian summary cards for attendance, transport, wallet, learning, requests, complaints, communications, documents, and certificates in apps/mobile/lib/features/guardian/guardian_summary_cards.dart
- [X] T115 [US2] Implement guardian request submission screen in apps/mobile/lib/features/guardian/guardian_request_screen.dart
- [X] T116 [US2] Implement guardian complaint submission screen in apps/mobile/lib/features/guardian/guardian_complaint_screen.dart
- [X] T117 [US2] Implement guardian notification center in apps/mobile/lib/features/guardian/guardian_notifications_screen.dart
- [X] T118 [US2] Implement student repository client in apps/mobile/lib/features/student/student_repository.dart
- [X] T119 [US2] Implement student home summaries in apps/mobile/lib/features/student/student_home_screen.dart
- [X] T120 [US2] Implement student learning, communications, complaints, documents, and certificates views in apps/mobile/lib/features/student/student_module_views.dart
- [X] T121 [US2] Implement guardian/student blocked feature handling in apps/mobile/lib/features/mobile_shell/blocked_feature_handler.dart
- [X] T122 [US2] Implement cached guardian link revocation handling in apps/mobile/lib/features/guardian/guardian_link_revocation_handler.dart
- [X] T123 [US2] Implement localized guardian/student form validation in apps/mobile/lib/features/mobile_shell/localized_form_validation.dart
- [X] T124 [US2] Add guardian/student audit events in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileAuditService.cs
- [X] T125 [US2] Add guardian/student e2e validation script in tests/e2e/mobile/guardian-student-mobile.md

**Checkpoint**: Guardian and student journeys work independently on top of the shared mobile shell.

---

## Phase 5: User Story 3 - Staff Use Operational Mobile Duties (Priority: P2)

**Goal**: Every listed staff role can complete assigned mobile duties while unrelated role actions remain blocked.

**Independent Test**: Sign in as each configured staff role and complete one critical role-specific journey; verify source-domain authority, offline limits, denied cross-role access, and Arabic/English layout.

### Tests for User Story 3

- [X] T126 [P] [US3] Add transport driver mobile journey tests in apps/mobile/test/features/transport_driver/transport_driver_journey_test.dart
- [X] T127 [P] [US3] Add gate/access mobile journey tests in apps/mobile/test/features/gate_access/gate_access_journey_test.dart
- [X] T128 [P] [US3] Add canteen cashier mobile journey tests in apps/mobile/test/features/canteen_cashier/canteen_cashier_journey_test.dart
- [X] T129 [P] [US3] Add teacher mobile journey tests in apps/mobile/test/features/teacher/teacher_journey_test.dart
- [X] T130 [P] [US3] Add medical staff mobile journey tests in apps/mobile/test/features/medical_staff/medical_staff_journey_test.dart
- [X] T131 [P] [US3] Add complaint handler mobile journey tests in apps/mobile/test/features/complaint_handler/complaint_handler_journey_test.dart
- [X] T132 [P] [US3] Add communication sender mobile journey tests in apps/mobile/test/features/communication_sender/communication_sender_journey_test.dart
- [X] T133 [P] [US3] Add document administrator mobile journey tests in apps/mobile/test/features/document_admin/document_admin_journey_test.dart
- [X] T134 [P] [US3] Add school administrator mobile journey tests in apps/mobile/test/features/school_admin/school_admin_journey_test.dart
- [X] T135 [P] [US3] Add staff source-domain action contract tests in tests/contracts/mobile/staff-source-actions.contract.md
- [X] T136 [P] [US3] Add staff cross-role denial integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/StaffMobileAuthorizationTests.cs
- [X] T137 [P] [US3] Add offline queue integration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/MobileOfflineQueueTests.cs
- [X] T138 [P] [US3] Add staff Arabic/English layout tests in apps/mobile/test/features/mobile/staff_rtl_ltr_test.dart

### Implementation for User Story 3

- [X] T139 [US3] Implement staff mobile DTOs in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/StaffMobileDtos.cs
- [X] T140 [US3] Implement StaffWorkspaceService in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/StaffWorkspaceService.cs
- [X] T141 [US3] Implement staff workspace endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/StaffWorkspaceEndpoints.cs
- [X] T142 [US3] Implement StaffSourceActionRouter in apps/api/src/SafeSchool.Api/Features/Mobile/Common/StaffSourceActionRouter.cs
- [X] T143 [US3] Implement transport driver API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/TransportDriverMobileAdapter.cs
- [X] T144 [US3] Implement transport driver trip, boarding, drop, and location UI in apps/mobile/lib/features/transport_driver/transport_driver_trip_screen.dart
- [X] T145 [US3] Implement gate/access API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/GateAccessMobileAdapter.cs
- [X] T146 [US3] Implement gate/access NFC/QR scan UI in apps/mobile/lib/features/gate_access/gate_access_scan_screen.dart
- [X] T147 [US3] Implement canteen cashier API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/CanteenCashierMobileAdapter.cs
- [X] T148 [US3] Implement canteen cashier POS UI in apps/mobile/lib/features/canteen_cashier/canteen_pos_screen.dart
- [X] T149 [US3] Implement teacher API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/TeacherMobileAdapter.cs
- [X] T150 [US3] Implement teacher learning, attendance, and behavior UI in apps/mobile/lib/features/teacher/teacher_workspace_screen.dart
- [X] T151 [US3] Implement medical staff API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/MedicalStaffMobileAdapter.cs
- [X] T152 [US3] Implement medical emergency and student medical UI in apps/mobile/lib/features/medical_staff/medical_staff_workspace_screen.dart
- [X] T153 [US3] Implement complaint handler API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/ComplaintHandlerMobileAdapter.cs
- [X] T154 [US3] Implement complaint triage and escalation UI in apps/mobile/lib/features/complaint_handler/complaint_handler_workspace_screen.dart
- [X] T155 [US3] Implement communication sender API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/CommunicationSenderMobileAdapter.cs
- [X] T156 [US3] Implement broadcast and direct message UI in apps/mobile/lib/features/communication_sender/communication_sender_workspace_screen.dart
- [X] T157 [US3] Implement document administrator API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/DocumentAdministratorMobileAdapter.cs
- [X] T158 [US3] Implement document and certificate review UI in apps/mobile/lib/features/document_admin/document_admin_workspace_screen.dart
- [X] T159 [US3] Implement school administrator API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/SchoolAdministratorMobileAdapter.cs
- [X] T160 [US3] Implement school administrator mobile dashboard UI in apps/mobile/lib/features/school_admin/school_admin_dashboard_screen.dart
- [X] T161 [US3] Implement offline action endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/OfflineSync/MobileOfflineActionEndpoints.cs
- [X] T162 [US3] Implement offline action sync service in apps/api/src/SafeSchool.Api/Features/Mobile/OfflineSync/MobileOfflineActionSyncService.cs
- [X] T163 [US3] Implement Flutter offline queue repository in apps/mobile/lib/core/offline/mobile_offline_queue_repository.dart
- [X] T164 [US3] Implement Flutter offline sync status UI in apps/mobile/lib/core/offline/mobile_sync_status_banner.dart
- [X] T165 [US3] Implement staff denied cross-role UI in apps/mobile/lib/features/mobile_shell/staff_cross_role_denied_screen.dart
- [X] T166 [US3] Implement admin staff mobile assignment page in apps/admin-web/src/app/(school)/mobile/roles/staff/page.tsx
- [X] T167 [US3] Implement staff mobile assignment component in apps/admin-web/src/features/mobile/components/StaffMobileAssignmentPanel.tsx
- [X] T168 [US3] Add staff action audit events in apps/api/src/SafeSchool.Api/Features/Mobile/Common/MobileAuditService.cs
- [X] T169 [US3] Add staff e2e validation script in tests/e2e/mobile/staff-role-workspaces.md

**Checkpoint**: Staff operational role journeys work independently and respect source-domain boundaries.

---

## Phase 6: User Story 4 - Release and Support an Installable APK (Priority: P3)

**Goal**: Release operators and authorized administrators can manage controlled APK releases, verify installs, enforce update/rollback policy, and review support evidence.

**Independent Test**: Publish an approved APK release for a pilot audience, install with an approved user, block out-of-audience and obsolete versions, and find support evidence by user, device, version, tenant, or time range.

### Tests for User Story 4

- [X] T170 [P] [US4] Add APK release state transition tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/ApkReleaseStateTests.cs
- [X] T171 [P] [US4] Add release audience matching tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/ReleaseAudienceTests.cs
- [X] T172 [P] [US4] Add current release contract tests in tests/contracts/mobile/apk-current-release.contract.md
- [X] T173 [P] [US4] Add release create approve revoke contract tests in tests/contracts/mobile/apk-release-admin.contract.md
- [X] T174 [P] [US4] Add install event contract tests in tests/contracts/mobile/apk-install-events.contract.md
- [X] T175 [P] [US4] Add support observability contract tests in tests/contracts/mobile/mobile-support-observability.contract.md
- [X] T176 [P] [US4] Add admin web APK release tests in apps/admin-web/tests/mobile/mobileReleases.spec.ts
- [X] T177 [P] [US4] Add admin web mobile support tests in apps/admin-web/tests/mobile/mobileSupport.spec.ts
- [X] T178 [P] [US4] Add Flutter version check and update screen tests in apps/mobile/test/features/mobile/apk_version_check_test.dart
- [X] T179 [P] [US4] Add Android APK smoke test checklist in tests/e2e/mobile/apk-smoke-test.md

### Implementation for User Story 4

- [X] T180 [US4] Implement ApkReleaseRepository in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ApkReleaseRepository.cs
- [X] T181 [US4] Implement ApkReleaseService in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ApkReleaseService.cs
- [X] T182 [US4] Implement ReleaseAudienceService in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ReleaseAudienceService.cs
- [X] T183 [US4] Implement MobileVersionPolicyService in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/MobileVersionPolicyService.cs
- [X] T184 [US4] Implement GET /api/v1/mobile/releases/current endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/MobileReleaseLookupEndpoints.cs
- [X] T185 [US4] Implement POST /api/v1/mobile/releases endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/MobileReleaseAdminEndpoints.cs
- [X] T186 [US4] Implement release approve and revoke endpoints in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/MobileReleaseAdminEndpoints.cs
- [X] T187 [US4] Implement POST /api/v1/mobile/install-events endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/MobileInstallEventEndpoints.cs
- [X] T188 [US4] Implement APK artifact storage adapter in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ApkArtifactStorage.cs
- [X] T189 [US4] Implement release notes Arabic/English validation in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ReleaseNotesValidator.cs
- [X] T190 [US4] Implement mobile release list page in apps/admin-web/src/app/(school)/mobile/releases/page.tsx
- [X] T191 [US4] Implement mobile release detail page in apps/admin-web/src/app/(school)/mobile/releases/[releaseId]/page.tsx
- [X] T192 [US4] Implement mobile release create form in apps/admin-web/src/features/mobile/components/MobileReleaseForm.tsx
- [X] T193 [US4] Implement mobile release approve/revoke controls in apps/admin-web/src/features/mobile/components/MobileReleaseApprovalPanel.tsx
- [X] T194 [US4] Implement mobile release audience editor in apps/admin-web/src/features/mobile/components/MobileReleaseAudienceEditor.tsx
- [X] T195 [US4] Implement mobile release notes localization editor in apps/admin-web/src/features/mobile/components/MobileReleaseNotesEditor.tsx
- [X] T196 [US4] Implement Flutter release lookup client in apps/mobile/lib/core/release/mobile_release_client.dart
- [X] T197 [US4] Implement Flutter app version policy guard in apps/mobile/lib/core/release/mobile_version_guard.dart
- [X] T198 [US4] Implement Flutter update required screen in apps/mobile/lib/core/release/update_required_screen.dart
- [X] T199 [US4] Implement Flutter install and launch event reporter in apps/mobile/lib/core/release/install_event_reporter.dart
- [X] T200 [US4] Configure Android version code and version name wiring in apps/mobile/android/app/build.gradle.kts
- [X] T201 [US4] Add controlled APK build script in apps/mobile/tool/build_controlled_apk.sh
- [X] T202 [US4] Implement support device session endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportDeviceSessionEndpoints.cs
- [X] T203 [US4] Implement support audit event endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportAuditEventEndpoints.cs
- [X] T204 [US4] Implement support install event endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportInstallEventEndpoints.cs
- [X] T205 [US4] Implement support diagnostics endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportDiagnosticsEndpoints.cs
- [X] T206 [US4] Implement mobile support overview page in apps/admin-web/src/app/(school)/mobile/support/page.tsx
- [X] T207 [US4] Implement mobile device session support panel in apps/admin-web/src/features/mobile/components/MobileDeviceSessionPanel.tsx
- [X] T208 [US4] Implement mobile audit timeline panel in apps/admin-web/src/features/mobile/components/MobileAuditTimelinePanel.tsx
- [X] T209 [US4] Implement mobile install evidence panel in apps/admin-web/src/features/mobile/components/MobileInstallEvidencePanel.tsx
- [X] T210 [US4] Implement mobile diagnostics detail page in apps/admin-web/src/app/(school)/mobile/support/[correlationId]/page.tsx
- [X] T211 [US4] Add release and support metrics in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportMetrics.cs
- [X] T212 [US4] Seed demo APK release and pilot audience data in apps/api/src/SafeSchool.Api/Features/Mobile/Seed/MobileReleaseSeed.cs
- [X] T213 [US4] Add APK release e2e validation script in tests/e2e/mobile/apk-release-support.md

**Checkpoint**: APK release and support observability are independently testable.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Hardening, validation, documentation, and release readiness across all stories.

- [X] T214 [P] Update implementation documentation in docs/mobile/README.md
- [X] T215 [P] Update API contract documentation index in tests/contracts/mobile/README.md
- [X] T216 [P] Update mobile demo and pilot runbook in docs/mobile/mobile-demo-runbook.md
- [X] T217 [P] Add final Arabic copy review checklist in docs/mobile/arabic-english-copy-review.md
- [X] T218 Run backend Mobile tests with dotnet test in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [X] T219 Run admin web Mobile tests with npm test in apps/admin-web/
- [X] T220 Run admin web production build with npm run build in apps/admin-web/
- [X] T221 Run Flutter Mobile tests with flutter test in apps/mobile/
- [X] T222 Run Flutter integration tests for role journeys in apps/mobile/
- [X] T223 Build controlled Android APK in apps/mobile/
- [X] T224 Verify APK install on Android device/emulator using tests/e2e/mobile/apk-smoke-test.md
- [X] T225 Verify quickstart backend validation using specs/012-role-based-mobile-apk/quickstart.md
- [X] T226 Verify quickstart admin web validation using specs/012-role-based-mobile-apk/quickstart.md
- [X] T227 Verify quickstart mobile app validation using specs/012-role-based-mobile-apk/quickstart.md
- [X] T228 Verify quickstart offline workflow validation using specs/012-role-based-mobile-apk/quickstart.md
- [X] T229 Verify quickstart APK release validation using specs/012-role-based-mobile-apk/quickstart.md
- [X] T230 Complete tenant isolation and feature-flag audit in docs/mobile/tenant-feature-audit.md
- [X] T231 Complete role permission coverage matrix in docs/mobile/role-permission-coverage.md
- [X] T232 Complete source-domain boundary audit in docs/mobile/source-domain-boundary-audit.md
- [X] T233 Complete observability coverage audit in docs/mobile/mobile-observability-audit.md
- [X] T234 Complete accessibility and RTL layout audit in docs/mobile/mobile-accessibility-rtl-audit.md
- [X] T235 Complete release readiness checklist in docs/mobile/apk-release-readiness.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies and can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories.
- **User Story 1 (Phase 3)**: Depends on Foundational and is the MVP.
- **User Story 2 (Phase 4)**: Depends on Foundational and shared shell behavior from US1 for final integration.
- **User Story 3 (Phase 5)**: Depends on Foundational and shared shell behavior from US1 for final integration.
- **User Story 4 (Phase 6)**: Depends on Foundational and can proceed after shared device/session foundations are ready.
- **Polish (Phase 7)**: Depends on all desired stories being complete.

### User Story Dependencies

- **US1**: Required MVP and shared shell; no dependency on US2, US3, or US4.
- **US2**: Can start after Foundational, but final mobile integration uses US1 shell.
- **US3**: Can start after Foundational, but final mobile integration uses US1 shell.
- **US4**: Can start after Foundational; mobile version guard integrates with US1 shell.

### Within Each User Story

- Write tests first and confirm they fail before implementation.
- Domain models and DTOs before services.
- Services before endpoints.
- Backend tenant, feature flag, role, and permission enforcement before UI exposure.
- Mobile repositories before screens.
- Source-domain adapters before mobile action forms.
- Audit and metric events before story checkpoint sign-off.

---

## Parallel Opportunities

- Setup directory creation tasks T002 through T009 can run in parallel.
- Foundational model tasks T024 through T033 can run in parallel after test scaffolding.
- Foundational Flutter/admin client tasks T048 through T053 can run in parallel with backend service tasks T038 through T046.
- US1 tests T054 through T061 can run in parallel.
- US1 workspace shell tasks T079 through T090 can run in parallel after the workspace registry is available.
- US2 tests T096 through T103 can run in parallel.
- US2 guardian and student mobile implementation tracks T112 through T123 can run in parallel after API DTOs are ready.
- US3 role journey tests T126 through T138 can run in parallel.
- US3 role adapters and screens T143 through T160 can run in parallel by role after StaffWorkspaceService is available.
- US4 contract, admin web, and Flutter version tests T170 through T179 can run in parallel.
- US4 admin web release controls T190 through T195 can run in parallel with Flutter version guard tasks T196 through T201 and support endpoint tasks T202 through T205.
- Polish documentation tasks T214 through T217 can run in parallel.

---

## Parallel Example: User Story 1

```text
Task: "T054 Add profile resolver unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Mobile/ProfileResolverTests.cs"
Task: "T055 Add profile and context contract tests in tests/contracts/mobile/mobile-profile-context.contract.md"
Task: "T059 Add Flutter sign-in and role switcher tests in apps/mobile/test/features/mobile/sign_in_role_switcher_test.dart"
Task: "T061 Add admin web role matrix tests in apps/admin-web/tests/mobile/mobileRoleMatrix.spec.ts"
```

## Parallel Example: User Story 2

```text
Task: "T106 Implement GuardianMobileService with approved link checks in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/GuardianMobileService.cs"
Task: "T107 Implement StudentMobileService with self-scope checks in apps/api/src/SafeSchool.Api/Features/Mobile/Profiles/StudentMobileService.cs"
Task: "T112 Implement guardian repository client in apps/mobile/lib/features/guardian/guardian_repository.dart"
Task: "T118 Implement student repository client in apps/mobile/lib/features/student/student_repository.dart"
```

## Parallel Example: User Story 3

```text
Task: "T143 Implement transport driver API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/TransportDriverMobileAdapter.cs"
Task: "T145 Implement gate/access API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/GateAccessMobileAdapter.cs"
Task: "T147 Implement canteen cashier API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/CanteenCashierMobileAdapter.cs"
Task: "T149 Implement teacher API adapter in apps/api/src/SafeSchool.Api/Features/Mobile/RoleWorkspaces/TeacherMobileAdapter.cs"
```

## Parallel Example: User Story 4

```text
Task: "T181 Implement ApkReleaseService in apps/api/src/SafeSchool.Api/Features/Mobile/Releases/ApkReleaseService.cs"
Task: "T190 Implement mobile release list page in apps/admin-web/src/app/(school)/mobile/releases/page.tsx"
Task: "T196 Implement Flutter release lookup client in apps/mobile/lib/core/release/mobile_release_client.dart"
Task: "T202 Implement support device session endpoint in apps/api/src/SafeSchool.Api/Features/Mobile/Support/MobileSupportDeviceSessionEndpoints.cs"
```

---

## Implementation Strategy

### MVP First: User Story 1 Only

1. Complete Phase 1 setup.
2. Complete Phase 2 foundational backend, admin web, and Flutter infrastructure.
3. Complete Phase 3 User Story 1.
4. Stop and validate role-based shell, tenant/role context, Arabic/English RTL, no-mobile-access denial, and all role workspace shells.
5. Demo the one-app role model before adding full role actions.

### Incremental Delivery

1. Add US1 for one-app sign-in, tenant/role context, localization, workspace visibility, and permission denial.
2. Add US2 for guardian and student mobile journeys.
3. Add US3 for all staff operational role journeys.
4. Add US4 for controlled APK release and support observability.
5. Run Phase 7 hardening and release readiness.

### Parallel Team Strategy

1. One team completes Setup and Foundational phases together.
2. After Foundational:
   - Team A: US1 mobile shell and role workspace resolution.
   - Team B: US2 guardian/student journeys.
   - Team C: US3 staff operational roles split by role.
   - Team D: US4 APK release and support observability.
3. Integrate through shared permission, device session, localization, and audit services.

---

## Notes

- [P] tasks use different files and can be worked in parallel after their dependencies.
- [US1], [US2], [US3], and [US4] labels map to the user stories in spec.md.
- Every listed role must have a production-critical journey before Phase 12 is complete.
- Guardian access remains a role and permission in the same app, not a separate parent app.
- Arabic and English with right-to-left Arabic validation are required for all role-critical journeys.
- Mobile actions must preserve source-domain authority and audit boundaries.
