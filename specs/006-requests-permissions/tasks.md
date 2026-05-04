# Tasks: Phase 6 Requests & Permissions

**Input**: Design documents from `/specs/006-requests-permissions/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 6 plan require unit,
integration, contract, authorization, tenant-isolation, audit, and critical UI
journey coverage. Write test tasks before implementation tasks in each
user-story phase and confirm they fail for the missing behavior before
completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a
task is marked `[P]`. Do not implement attendance generation, campus entry or
exit decisions, NFC or QR scan processing, transport boarding/drop decisions,
wallet/payment actions, learning content delivery, star balance ownership,
medical/emergency workflows, complaint escalation, broad messaging,
broadcasts, document storage, global search, or broad admin dashboards. Every
request action must resolve tenant context, check the required Phase 6
capability, enforce permission, prevent cross-school visibility, preserve
versioned workflow/request evidence, record denied access decisions, and emit
audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no
  dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses
described in [plan.md](./plan.md).

- [ ] T001 Create or update API solution and Requests source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/Requests/
- [ ] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, logging, and background worker dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [ ] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, time provider fakes, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [ ] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, route tests, and component test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [ ] T005 [P] Create or update mobile package manifest with Flutter test, HTTP client, auth header, and request surface dependencies in apps/mobile/pubspec.yaml
- [ ] T006 [P] Create Requests admin, guardian, and student route placeholders plus feature exports in apps/admin-web/src/app/(school)/requests/page.tsx, apps/admin-web/src/app/(guardian)/requests/page.tsx, apps/admin-web/src/app/(student)/requests/page.tsx, and apps/admin-web/src/features/requests/index.ts
- [ ] T007 [P] Create Requests configuration, early leave, workflow queue, and review route placeholders in apps/admin-web/src/app/(school)/requests/configuration/page.tsx, apps/admin-web/src/app/(school)/requests/early-leave/page.tsx, apps/admin-web/src/app/(school)/requests/workflow/page.tsx, and apps/admin-web/src/app/(school)/requests/review/page.tsx
- [ ] T008 [P] Create mobile Requests module export shell in apps/mobile/lib/features/requests/requests.dart
- [ ] T009 Create API bootstrap additions for Requests endpoint registration, versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, and background expiry registration in apps/api/src/SafeSchool.Api/Program.cs
- [ ] T010 [P] Create API configuration placeholders for request capabilities, workflow expiry scanning, notification event export, star evidence adapter, audit, and duplicate overlap thresholds in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [ ] T011 [P] Create contract test documentation index linking the six Phase 6 contracts in tests/contracts/requests/README.md
- [ ] T012 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/requests/README.md
- [ ] T013 [P] Create repository generated-file ignores for Requests API, web, mobile, coverage, contract fixture, and seed data artifacts in .gitignore

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, identity,
guardian, star evidence, persistence, idempotency, audit, API, web, and mobile
foundations required by every Phase 6 user story.

**Critical**: No user-story implementation should start until this phase is
complete.

- [ ] T014 Create shared Requests tenant-owned entity base, result types, validation error type, paged response type, time provider abstraction, review reason type, and source metadata type in apps/api/src/SafeSchool.Api/Features/Requests/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestOperationResults.cs
- [ ] T015 [P] Create shared Requests enums for request category, request status, requester role, duplicate review state, workflow template status, workflow step type, decision type, decision status, consent policy, consent decision, star evaluation status, pickup verification status, exception type, exception severity, review action, and capability keys in apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestEnums.cs
- [ ] T016 Create or extend SafeSchoolDbContext registration for Requests entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T017 Create Requests model-builder extension for tenant metadata, timestamps, version fields, status indexes, assignee queues, guardian visibility, duplicate/overlap lookups, workflow expiry routing, star evaluation lookups, exception status, review summaries, and audit traceability in apps/api/src/SafeSchool.Api/Features/Requests/RequestsDbContextModelBuilderExtensions.cs
- [ ] T018 Create Phase 6 capability constants for requests.outing, requests.early_leave, requests.star_rules, requests.approval_workflow, requests.history, requests.configuration, and requests.review_summaries in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/RequestCapabilities.cs
- [ ] T019 Create Requests permission catalog entries for create, read, withdraw, manage, outing, early leave, release read, pickup verify, workflow manage, decision act, guardian consent, star rules, history, guardian history, reviews, summaries, audit, and platform review in apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestPermissionCatalog.cs
- [ ] T020 Create Requests permission guard that wraps tenant context, feature gate, permission evaluation, guardian link scope, student self scope, assigned approver scope, release-read scope, access decision writing, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestPermissionGuard.cs
- [ ] T021 Create Requests feature gate service that enforces Phase 6 capability keys server-side and returns typed disabled-capability errors in apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestFeatureGate.cs
- [ ] T022 Create IdentityAccess student profile adapter interface and test fake for active, inactive, graduated, transferred, duplicated, missing, and cross-tenant student profiles in apps/api/src/SafeSchool.Api/Features/Requests/Common/Identity/RequestStudentProfileProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/FakeRequestStudentProfileProvider.cs
- [ ] T023 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, primary, decision-capable, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/Requests/Common/Identity/RequestGuardianLinkProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/FakeRequestGuardianLinkProvider.cs
- [ ] T024 Create IdentityAccess role and approver assignment adapter interface and test fake for staff role membership, explicit actor assignment, reviewer queue assignment, disabled actor, transferred actor, and missing permission cases in apps/api/src/SafeSchool.Api/Features/Requests/Common/Identity/RequestApproverAssignmentProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/FakeRequestApproverAssignmentProvider.cs
- [ ] T025 Create Phase 5 star evidence adapter interface and test fake for sufficient, insufficient, unavailable, stale, disabled, reservation success, reservation failure, consumption success, release success, and corrected evidence cases in apps/api/src/SafeSchool.Api/Features/Requests/Common/Star/Phase5StarEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/FakePhase5StarEvidenceProvider.cs
- [ ] T026 Create AttendanceAccess and CampusAccess boundary adapters that expose read-only release evidence hooks and block attendance, entry, exit, gate event, NFC, and QR mutation from Requests in apps/api/src/SafeSchool.Api/Features/Requests/Common/Boundaries/AttendanceCampusBoundary.cs
- [ ] T027 Create Transport, Wallet, Learning, Medical, Complaint, Document, Search, and Messaging boundary guard that prevents Phase 6 side effects outside request status event export in apps/api/src/SafeSchool.Api/Features/Requests/Common/Boundaries/RequestPhaseBoundaryGuard.cs
- [ ] T028 Create Requests idempotency service for client_request_id, workflow decision identity, guardian consent identity, pickup verification identity, star evaluation retries, review actions, and configuration changes in apps/api/src/SafeSchool.Api/Features/Requests/Common/Idempotency/RequestIdempotencyService.cs
- [ ] T029 Create Requests audit event entity and audit writer adapter for request creation, submission, withdrawal, approval, denial, information request, delegation, escalation, expiration, guardian consent, star evaluation, pickup verification, correction, reopen, exception, configuration, summary read, and access denial events in apps/api/src/SafeSchool.Api/Features/Requests/Audit/RequestAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/Requests/Audit/RequestAuditWriter.cs
- [ ] T030 Create Requests trace reference type for request, workflow, decision, consent, early leave, outing, pickup, star, exception, review, summary, notification event, and audit links in apps/api/src/SafeSchool.Api/Features/Requests/Common/Trace/RequestTraceReference.cs
- [ ] T031 Create core domain entity files in apps/api/src/SafeSchool.Api/Features/Requests/Domain/PermissionRequest.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/OutingRequestDetail.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/EarlyLeaveDetail.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/RequestType.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/WorkflowTemplate.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/WorkflowTemplateVersion.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/WorkflowStep.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/WorkflowDecision.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/GuardianConsentRecord.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/StarPermissionRule.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/StarRuleEvaluation.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/PickupEvidence.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/RequestException.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/ManualRequestReview.cs, apps/api/src/SafeSchool.Api/Features/Requests/Domain/RequestReviewSummary.cs, and apps/api/src/SafeSchool.Api/Features/Requests/Domain/SchoolAccountFeatureSetting.cs
- [ ] T032 Create entity configurations and initial EF migration for all Phase 6 tenant-owned tables, foreign keys, version indexes, duplicate/overlap indexes, workflow queue indexes, star indexes, exception indexes, summary indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Requests/Persistence/RequestEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050001_RequestsFoundation.cs
- [ ] T033 Create request DTO primitives for paging, typed errors, request status, workflow status, consent status, star status, release eligibility, trace links, and audit references in apps/api/src/SafeSchool.Api/Features/Requests/Common/RequestDtos.cs
- [ ] T034 Create Requests route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/requests, /api/v1/guardians/me/students/{studentProfileId}/requests, /api/v1/students/me/requests, and early-leave routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T035 Create Requests OpenAPI tag registration and shared response/error examples for tenant mismatch, disabled capability, missing permission, invalid guardian link, invalid student, duplicate request, overlap review, stale workflow, missing star evidence, invalid pickup evidence, and audit failure in apps/api/src/SafeSchool.Api/Features/Requests/RequestOpenApiExamples.cs
- [ ] T036 Create API test fixture for tenants, capabilities, permissions, students, guardians, approvers, workflow templates, request types, requests, decisions, consent, pickup evidence, star evidence, exceptions, reviews, summaries, idempotency, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/RequestTestFixture.cs
- [ ] T037 [P] Create shared API test data builders for request types, workflows, steps, permission requests, outing details, early leave details, decisions, guardian consent, star rules, pickup evidence, exceptions, reviews, summaries, feature settings, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Fixtures/RequestTestData.cs
- [ ] T038 [P] Create unit tests for RequestFeatureGate capability decisions, disabled workflow denial, independent capability keys, and backend enforcement behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Foundational/RequestFeatureGateTests.cs
- [ ] T039 [P] Create unit tests for RequestPermissionGuard tenant mismatch, guardian scope mismatch, student self scope mismatch, assigned approver mismatch, release-read permission denial, platform review allowance, denied access audit, and allowed access in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Foundational/RequestPermissionGuardTests.cs
- [ ] T040 [P] Create unit tests for RequestIdempotencyService retry, conflict, duplicate request create, duplicate decision, duplicate consent, duplicate pickup verification, duplicate star retry, and duplicate manual review behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Foundational/RequestIdempotencyServiceTests.cs
- [ ] T041 [P] Create unit tests for RequestPhaseBoundaryGuard preventing attendance, campus gate, scan, transport, wallet, learning, medical, complaint, document, search, dashboard, and direct notification delivery side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Foundational/RequestPhaseBoundaryGuardTests.cs
- [ ] T042 [P] Create unit tests for RequestAuditWriter sensitive mutation failure behavior, denied access evidence, and status-event audit payloads in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Foundational/RequestAuditWriterTests.cs
- [ ] T043 Create Requests status event export model for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Requests/Audit/RequestStatusEvent.cs
- [ ] T044 Create background workflow expiry scanner shell that only invokes configured Requests services and has no automatic approve/deny behavior in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowExpiryScanner.cs
- [ ] T045 [P] Create admin web Requests API client with tenant context, typed errors, pagination, capability-disabled handling, request trace links, workflow queue, early leave, star, configuration, and review routes in apps/admin-web/src/features/requests/api/requestsApi.ts
- [ ] T046 [P] Create guardian web Requests API client with linked-student scope, typed errors, pagination, submit, withdraw, consent, early leave, history, and trace routes in apps/admin-web/src/features/requests/api/guardianRequestsApi.ts
- [ ] T047 [P] Create student web Requests API client with self-scope, typed errors, pagination, submit, withdraw, history, and status routes in apps/admin-web/src/features/requests/api/studentRequestsApi.ts
- [ ] T048 [P] Create mobile Requests API client shell with auth headers, tenant context, linked-student context, typed errors, submit, withdraw, decision, consent, early leave, and history methods in apps/mobile/lib/features/requests/request_api.dart
- [ ] T049 [P] Create shared web test data builders for request types, workflows, requests, decisions, early leave, pickup evidence, star evaluations, exceptions, reviews, and summaries in apps/admin-web/tests/requests/requestTestData.ts
- [ ] T050 [P] Create contract fixture documentation for common request errors, tenant isolation, feature disabled, missing permission, idempotency, and trace payloads in tests/contracts/requests/common-fixtures.md

**Checkpoint**: Foundation ready. User-story work can start after T014-T050 are complete.

---

## Phase 3: User Story 1 - Submit and Track Permission Requests (Priority: P1) MVP

**Goal**: Students or guardians can submit outing or permission requests and
track status while tenant, requester, guardian link, required fields, duplicate
rules, and visibility are enforced.

**Independent Test**: Submit an outing or permission request for an active
student in one school account, verify required details and requester
eligibility are enforced, and confirm the requester and authorized staff can
see the correct status without exposing another school account or unrelated
student.

### Tests for User Story 1

- [ ] T051 [P] [US1] Create PermissionRequest domain tests for Draft, Submitted, Pending Consent, Pending Approval, Withdrawn, exact active duplicate blocked, overlap routed to review, required fields, student initiator allowed, guardian initiator allowed, staff initiator allowed, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/PermissionRequestDomainTests.cs
- [ ] T052 [P] [US1] Create OutingRequestDetail domain tests for destination, purpose, departure window, return window, supervision expectation, transport expectation, closure requirement, overdue closure state, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Outings/OutingRequestDetailDomainTests.cs
- [ ] T053 [P] [US1] Create RequestSubmissionService unit tests for active student, inactive student denial, unlinked guardian denial, student-initiator disabled denial, missing required fields denial, disabled capability denial, duplicate block, overlap manual review, workflow starter, withdrawal, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/RequestSubmissionServiceTests.cs
- [ ] T054 [P] [US1] Create contract tests for staff request create/list/detail/withdraw/trace, guardian request create/list/withdraw, and student request create/list/withdraw routes from contracts/outing-permission-requests.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/OutingPermissionRequestsContractTests.cs
- [ ] T055 [P] [US1] Create integration tests for tenant isolation, requests.outing disabled capability, missing requests.requests.create permission, guardian link states, student self scope, exact duplicate active request, overlap review, withdrawal before final state, trace read, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/RequestSubmissionIntegrationTests.cs
- [ ] T056 [P] [US1] Create web journey tests for guardian request submission, student request submission, school request list, status tracking, duplicate error, overlap review state, withdrawal, disabled capability state, and access-denied state in apps/admin-web/tests/requests/request-submission.spec.ts
- [ ] T057 [P] [US1] Create API performance test verifying eligible request submission completes in under 2 minutes during review testing with seeded tenant, request type, workflow, active student, and active guardian link in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/RequestSubmissionPerformanceTests.cs

### Implementation for User Story 1

- [ ] T058 [P] [US1] Create request submission, outing detail, request response, list filter, withdraw, trace, duplicate error, overlap review, guardian route, and student route DTOs from contracts/outing-permission-requests.md in apps/api/src/SafeSchool.Api/Features/Requests/Requests/PermissionRequestDtos.cs
- [ ] T059 [US1] Implement RequestRequiredFieldValidator for request type required_field_schema, outing detail fields, early leave placeholders, reason, requested date/time window, initiator role, and clear validation errors in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestRequiredFieldValidator.cs
- [ ] T060 [US1] Implement RequestDuplicatePolicyService for exact active duplicate detection and overlapping non-identical active request review routing in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestDuplicatePolicyService.cs
- [ ] T061 [US1] Implement RequestVisibilityService for school staff, linked guardian, student self, assigned approver, release-read staff, reviewer, and platform-review scoping in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestVisibilityService.cs
- [ ] T062 [US1] Implement RequestSubmissionService for draft create, submit now, student/guardian/staff initiator validation, tenant and feature checks, request type lookup, workflow starter, duplicate policy, overlap review exception, withdrawal before final state, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestSubmissionService.cs
- [ ] T063 [US1] Implement OutingRequestService for destination/purpose, departure/return window, supervision and transport expectation fields, closure requirement, overdue closure review, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Outings/OutingRequestService.cs
- [ ] T064 [US1] Implement RequestQueryService for school, guardian, and student request lists with filters by student, requester, type, status, date range, assignee, workflow version, exception state, star outcome, pagination, and visibility checks in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestQueryService.cs
- [ ] T065 [US1] Implement RequestTraceService for request to workflow, decision, guardian consent, outing, early leave, pickup, star, exception, review, status event, and audit references in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestTraceService.cs
- [ ] T066 [US1] Implement StaffRequestsController, GuardianRequestsController, StudentRequestsController, and RequestTraceController routes from contracts/outing-permission-requests.md in apps/api/src/SafeSchool.Api/Features/Requests/Requests/PermissionRequestsControllers.cs
- [ ] T067 [US1] Wire request created, submitted, withdrawn, duplicate blocked, overlap review, trace read, disabled capability, missing permission, invalid student, invalid guardian, and cross-school denial audit events in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestSubmissionAuditAdapter.cs
- [ ] T068 [P] [US1] Create guardian and student request form components, request status list, duplicate error display, overlap review badge, withdrawal action, and linked-student selector in apps/admin-web/src/features/requests/submission/RequestSubmissionForms.tsx and apps/admin-web/src/features/requests/submission/RequestStatusList.tsx
- [ ] T069 [P] [US1] Create school request list, filters, request detail drawer, outing detail panel, trace link panel, and access-denied display in apps/admin-web/src/features/requests/submission/SchoolRequestList.tsx and apps/admin-web/src/features/requests/submission/RequestTracePanel.tsx
- [ ] T070 [US1] Implement school, guardian, and student request routes using typed API clients, feature-disabled state, validation messages, list filters, withdrawal action, and status tracking in apps/admin-web/src/app/(school)/requests/page.tsx, apps/admin-web/src/app/(guardian)/requests/page.tsx, and apps/admin-web/src/app/(student)/requests/page.tsx
- [ ] T071 [P] [US1] Implement mobile request submission and status models for optional mobile request surfaces in apps/mobile/lib/features/requests/request_models.dart
- [ ] T072 [US1] Implement mobile RequestsRepository submit, withdraw, list, detail, trace, and typed error mapping in apps/mobile/lib/features/requests/request_repository.dart
- [ ] T073 [US1] Update Requests endpoint registration to map staff, guardian, student, withdraw, list, detail, and trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T074 [US1] Create OpenAPI examples for request submission, duplicate block, overlap manual review, guardian denial, student initiation denial, withdrawal, list, detail, trace, disabled capability, missing permission, and audit failure in apps/api/src/SafeSchool.Api/Features/Requests/Requests/PermissionRequestsOpenApiExamples.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T051-T074 pass.

---

## Phase 4: User Story 2 - Review and Decide Requests with Approval Workflows (Priority: P1)

**Goal**: Assigned approvers can approve, deny, request information, delegate,
escalate, or mark manual review according to configured workflow steps, with
idempotent and traceable decisions.

**Independent Test**: Configure a simple approval workflow, submit a request,
complete each required decision step with assigned approvers, and verify final
status, decision history, authorization boundaries, expiry routing, and audit
evidence.

### Tests for User Story 2

- [ ] T075 [P] [US2] Create WorkflowStep and WorkflowDecision domain tests for assigned role, assigned actor, required permission, reason-required decision, next-step calculation, append-only decision history, final state protection, and idempotency in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/WorkflowDecisionDomainTests.cs
- [ ] T076 [P] [US2] Create ApprovalWorkflowService unit tests for approve, deny, request information, delegate, escalate, mark manual review, duplicate retry, out-of-order decision, unauthorized actor, disabled actor, transferred approver, cross-school request, withdrawn request, final request, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowServiceTests.cs
- [ ] T077 [P] [US2] Create WorkflowExpiryService unit tests for configured expiry, school closure time, holiday time, pending-state preservation, manual review route, configured escalation route, no automatic approval, no automatic denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/WorkflowExpiryServiceTests.cs
- [ ] T078 [P] [US2] Create contract tests for workflow state, workflow queue, decision create, expire-step, delegate, and workflow trace routes from contracts/approval-workflow.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowContractTests.cs
- [ ] T079 [P] [US2] Create integration tests for active workflow decision path, guardian consent step handoff, staff approval step handoff, expiry routing, duplicate decision no duplicate final outcome, concurrent decision conflict, tenant isolation, permission denial, disabled capability, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowIntegrationTests.cs
- [ ] T080 [P] [US2] Create web journey tests for workflow queue, approve, deny, request information, delegate, escalate, expired step review state, duplicate decision retry display, and access-denied state in apps/admin-web/tests/requests/approval-workflow.spec.ts
- [ ] T081 [P] [US2] Create API performance test verifying assigned approvers can find and decide a pending request in under 60 seconds during review testing in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowPerformanceTests.cs

### Implementation for User Story 2

- [ ] T082 [P] [US2] Create workflow state, queue filter, queue response, decision request, decision response, expiry routing, delegation, and workflow trace DTOs matching contracts/approval-workflow.md in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowDtos.cs
- [ ] T083 [US2] Implement WorkflowStateMachine for step routing, allowed decisions, next-step calculation, final status calculation, reason requirement, pending-state preservation, and append-only transition results in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowStateMachine.cs
- [ ] T084 [US2] Implement ApprovalWorkflowService for decision validation, assigned actor/role permission, duplicate decision handling, out-of-order protection, concurrent conflict detection, final-state protection, decision persistence, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowService.cs
- [ ] T085 [US2] Implement WorkflowQueueService for current actor, role, reviewer queue, request manager, approver filters, status filters, due/expired filters, tenant scope, and pagination in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowQueueService.cs
- [ ] T086 [US2] Implement WorkflowExpiryService for expiry detection, manual review route, configured escalation route, pending-state preservation, no automatic final decision, source evidence, exception creation, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowExpiryService.cs
- [ ] T087 [US2] Implement WorkflowDelegationService for allowed delegation, delegated actor validation, delegated role validation, disabled actor denial, reassignment audit, and trace references in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowDelegationService.cs
- [ ] T088 [US2] Implement WorkflowTraceService for workflow step, decision, assignee, expiry, escalation, delegation, exception, review, and audit references in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowTraceService.cs
- [ ] T089 [US2] Implement ApprovalWorkflowController, WorkflowQueueController, WorkflowExpiryController, WorkflowDelegationController, and WorkflowTraceController routes from contracts/approval-workflow.md in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowControllers.cs
- [ ] T090 [US2] Wire approval, denial, information request, delegation, escalation, expiry, manual review route, duplicate decision, unauthorized decision, out-of-order decision, and trace read audit events in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowAuditAdapter.cs
- [ ] T091 [P] [US2] Create admin web workflow API hooks for queue, workflow state, approve, deny, request information, delegate, escalate, expire step, and trace in apps/admin-web/src/features/requests/workflows/workflowApi.ts
- [ ] T092 [P] [US2] Create workflow queue table, decision panel, delegation form, escalation banner, expiry status badge, decision history timeline, and workflow trace panel in apps/admin-web/src/features/requests/workflows/WorkflowQueue.tsx and apps/admin-web/src/features/requests/workflows/WorkflowDecisionPanel.tsx
- [ ] T093 [US2] Implement school workflow route with assigned queue, decision actions, reason validation, duplicate retry handling, expiry/escalation visibility, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/requests/workflow/page.tsx
- [ ] T094 [P] [US2] Create mobile workflow approval models and decision result models for optional approver mobile surfaces in apps/mobile/lib/features/requests/workflow_models.dart
- [ ] T095 [US2] Implement mobile workflow repository and approval screen shell for optional approver mobile surfaces in apps/mobile/lib/features/requests/workflow_repository.dart and apps/mobile/lib/features/requests/workflow_screen.dart
- [ ] T096 [US2] Update WorkflowExpiryScanner to invoke WorkflowExpiryService, batch expired steps safely, emit metrics, preserve pending state, and avoid automatic approve/deny behavior in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowExpiryScanner.cs
- [ ] T097 [US2] Update Requests endpoint registration to map workflow state, queue, decision, expiry, delegation, and trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T098 [US2] Create OpenAPI examples for approve, deny, request information, delegate, escalate, expiry route, duplicate decision, unauthorized actor, final-state decision denial, workflow queue, and workflow trace in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowOpenApiExamples.cs
- [ ] T099 [US2] Create workflow test data builder for valid template, invalid assignee, assigned role, assigned actor, pending request, final request, withdrawn request, expired step, delegated step, concurrent decision, and cross-school request cases in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowTestData.cs

**Checkpoint**: User Story 2 can be demonstrated independently after T075-T099 pass.

---

## Phase 5: User Story 3 - Manage Early Leave Requests (Priority: P1)

**Goal**: Guardians or authorized staff can submit early leave requests with
guardian-selected pickup person and staff verification evidence, and authorized
staff can read release eligibility without Phase 6 creating attendance or gate
side effects.

**Independent Test**: Submit an early leave request with student, date, release
time, reason, guardian consent, guardian-selected authorized pickup person, and
staff verification note; approve it through the workflow; and verify authorized
staff can see release eligibility without creating an attendance or gate event.

### Tests for User Story 3

- [ ] T100 [P] [US3] Create EarlyLeaveDetail and PickupEvidence domain tests for release time, reason, consent required, authorized pickup person, staff verification note, release eligibility states, approval expiry, invalid pickup, and no scan requirement in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveDomainTests.cs
- [ ] T101 [P] [US3] Create EarlyLeaveService unit tests for guardian submission, staff submission, unlinked guardian denial, invalid pickup denial, missing staff verification note block, approval expired block, release eligibility read permission, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveServiceTests.cs
- [ ] T102 [P] [US3] Create PickupVerificationService unit tests for authorized pickup person, blocked pickup person, expired pickup evidence, staff verification note requirement, duplicate verification retry, cross-school denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/PickupVerificationServiceTests.cs
- [ ] T103 [P] [US3] Create contract tests for guardian early leave submit, staff early leave submit, list, detail, pickup verification, release eligibility read, and trace routes from contracts/early-leave.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveContractTests.cs
- [ ] T104 [P] [US3] Create integration tests for approved early leave release eligibility, missing verification block, expired approval block, unlinked guardian block, unauthorized pickup review, invalid student block, disabled capability block, cross-school block, no attendance event, no gate event, no scan event, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveIntegrationTests.cs
- [ ] T105 [P] [US3] Create web journey tests for early leave submission, staff approval handoff, pickup verification note, release eligibility read, expired approval, invalid pickup, disabled capability, and access-denied state in apps/admin-web/tests/requests/early-leave.spec.ts
- [ ] T106 [P] [US3] Create API performance test verifying approved early leave release eligibility is visible to authorized staff within 1 minute after final approval and staff verification in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeavePerformanceTests.cs

### Implementation for User Story 3

- [ ] T107 [P] [US3] Create early leave submit, pickup person, pickup verification, release eligibility, list filter, detail response, and trace DTOs matching contracts/early-leave.md in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveDtos.cs
- [ ] T108 [US3] Implement EarlyLeaveService for guardian/staff submission, release time validation, guardian consent expectation, pickup evidence creation, workflow start, expiry policy, disabled feature checks, tenant checks, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveService.cs
- [ ] T109 [US3] Implement PickupVerificationService for staff verification note, authorized pickup person validation, duplicate verification idempotency, missing verification block, rejection state, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/PickupVerificationService.cs
- [ ] T110 [US3] Implement EarlyLeaveReleaseEligibilityService for final approval check, unexpired approval check, pickup verification check, release-read permission, no attendance/gate/scan side effects, and read-only evidence projection in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveReleaseEligibilityService.cs
- [ ] T111 [US3] Implement EarlyLeaveQueryService for school early leave list/detail filters by student, date, release status, pickup status, approval status, assignee, exception state, and pagination in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveQueryService.cs
- [ ] T112 [US3] Implement EarlyLeaveTraceService for early leave to request, workflow, guardian consent, pickup evidence, verification, release eligibility, exception, review, boundary guard, and audit references in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveTraceService.cs
- [ ] T113 [US3] Implement GuardianEarlyLeaveController, StaffEarlyLeaveController, PickupVerificationController, ReleaseEligibilityController, and EarlyLeaveTraceController routes from contracts/early-leave.md in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveControllers.cs
- [ ] T114 [US3] Wire early leave submitted, pickup evidence created, pickup verified, pickup rejected, release eligibility read, release eligibility blocked, approval expired, no attendance side effect, no gate side effect, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveAuditAdapter.cs
- [ ] T115 [P] [US3] Create admin web early leave API hooks for list, detail, pickup verification, release eligibility, trace, and typed denial errors in apps/admin-web/src/features/requests/early-leave/earlyLeaveApi.ts
- [ ] T116 [P] [US3] Create guardian early leave form, pickup person selector, status list, release status badge, and validation error display in apps/admin-web/src/features/requests/early-leave/GuardianEarlyLeaveForm.tsx and apps/admin-web/src/features/requests/early-leave/EarlyLeaveStatusList.tsx
- [ ] T117 [P] [US3] Create school early leave table, pickup verification form, release eligibility panel, expiry warning, invalid pickup reason panel, and trace panel in apps/admin-web/src/features/requests/early-leave/EarlyLeaveReviewPanel.tsx and apps/admin-web/src/features/requests/early-leave/ReleaseEligibilityPanel.tsx
- [ ] T118 [US3] Implement school and guardian early leave routes with submit, list, pickup verification, release eligibility read, no-side-effect labels, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/requests/early-leave/page.tsx and apps/admin-web/src/app/(guardian)/requests/early-leave/page.tsx
- [ ] T119 [P] [US3] Create mobile early leave models and repository methods for optional mobile guardian and staff surfaces in apps/mobile/lib/features/requests/early_leave_models.dart and apps/mobile/lib/features/requests/early_leave_repository.dart
- [ ] T120 [US3] Update RequestPhaseBoundaryGuard tests and implementation to assert early leave release eligibility reads never create attendance, gate, scan, or transport outcomes in apps/api/src/SafeSchool.Api/Features/Requests/Common/Boundaries/RequestPhaseBoundaryGuard.cs
- [ ] T121 [US3] Update Requests endpoint registration to map guardian early leave, staff early leave, pickup verification, release eligibility, and trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T122 [US3] Create OpenAPI examples for early leave submit, pickup verification, release eligibility success, missing verification block, expired approval block, unauthorized pickup review, unlinked guardian denial, no side-effect assertion, and trace in apps/api/src/SafeSchool.Api/Features/Requests/EarlyLeave/EarlyLeaveOpenApiExamples.cs
- [ ] T123 [US3] Create early leave test data builder for linked guardian, unlinked guardian, authorized pickup, blocked pickup, missing verification, verified pickup, expired approval, approved request, invalid student, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveTestData.cs

**Checkpoint**: User Story 3 can be demonstrated independently after T100-T123 pass.

---

## Phase 6: User Story 4 - Enforce Star-Based Permission Rules (Priority: P2)

**Goal**: School administrators or request managers can configure and apply
star-based permission rules that use Phase 5 evidence, reserve stars at
submission, consume on final approval, and release on denial, withdrawal, or
expiry without Phase 6 owning star balances.

**Independent Test**: Configure a request type that requires a star threshold or
star cost, submit requests for students with sufficient, insufficient, and
unavailable star evidence, and verify the correct rule outcome is recorded
without inventing or silently changing star balances.

### Tests for User Story 4

- [ ] T124 [P] [US4] Create StarPermissionRule and StarRuleEvaluation domain tests for threshold, star cost, eligible group, rule version, active dates, unavailable-evidence behavior, manual-review behavior, reservation state, consumed state, released state, and no star balance ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRuleDomainTests.cs
- [ ] T125 [P] [US4] Create StarPermissionRuleService unit tests for draft create, activate, suspend, invalid request type, disabled requests.star_rules capability, missing Phase 5 capability, tenant mismatch, duplicate request, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRuleServiceTests.cs
- [ ] T126 [P] [US4] Create StarRuleEvaluationService unit tests for sufficient evidence, insufficient evidence, missing evidence, stale evidence, disabled evidence, reserve at submission, consume on approval, release on denial, release on withdrawal, release on expiry, retry pending result, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarRuleEvaluationServiceTests.cs
- [ ] T127 [P] [US4] Create contract tests for star rule create/list/detail/update/activate, star evaluation read, star evaluation retry, and star evaluation trace routes from contracts/star-permission-rules.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRulesContractTests.cs
- [ ] T128 [P] [US4] Create integration tests for star-gated submission sufficient stars, insufficient stars denial/review, unavailable evidence behavior, reservation failure review, approval consumption, denial release, withdrawal release, expiry release, rule version snapshot, no invented balance, no Phase 6 balance mutation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRulesIntegrationTests.cs
- [ ] T129 [P] [US4] Create web journey tests for star rule configuration, star-gated request submit, sufficient/insufficient/unavailable outcomes, reservation state display, consumption state display, release state display, and trace links in apps/admin-web/tests/requests/star-permission-rules.spec.ts

### Implementation for User Story 4

- [ ] T130 [P] [US4] Create star rule request, rule response, evaluation response, outcome sync, retry, trace, and error DTOs matching contracts/star-permission-rules.md in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarPermissionRuleDtos.cs
- [ ] T131 [US4] Implement StarPermissionRuleService for draft create, update, activate, suspend, request type validation, active date validation, threshold validation, star cost validation, Phase 5 capability validation, versioning, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarPermissionRuleService.cs
- [ ] T132 [US4] Implement StarRuleEvaluationService for Phase 5 evidence lookup, rule snapshot capture, sufficient/insufficient/unavailable decisions, reservation at submission, pending/failed handling, retry behavior, exception creation, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarRuleEvaluationService.cs
- [ ] T133 [US4] Implement StarReservationLifecycleService for consume-on-final-approval, release-on-denial, release-on-withdrawal, release-on-expiry, idempotent Phase 5 outcome references, and no Phase 6 star balance mutation in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarReservationLifecycleService.cs
- [ ] T134 [US4] Integrate star evaluation into RequestSubmissionService so star-gated request submission reserves stars before workflow progression and routes insufficient/unavailable evidence according to request type behavior in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestSubmissionService.cs
- [ ] T135 [US4] Integrate star consumption and release into ApprovalWorkflowService, RequestSubmissionService withdrawal path, and WorkflowExpiryService expiry path in apps/api/src/SafeSchool.Api/Features/Requests/Workflows/ApprovalWorkflowService.cs, apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestSubmissionService.cs, and apps/api/src/SafeSchool.Api/Features/Requests/Workflows/WorkflowExpiryService.cs
- [ ] T136 [US4] Implement StarRuleTraceService for request, rule version, Phase 5 evidence, reservation, consumption, release, exception, review, and audit references in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarRuleTraceService.cs
- [ ] T137 [US4] Implement StarPermissionRulesController and StarRuleEvaluationController routes from contracts/star-permission-rules.md in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarPermissionRulesControllers.cs
- [ ] T138 [US4] Wire star rule created, activated, suspended, evaluation eligible, insufficient, unavailable, reserved, consumed, released, failed, retried, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarPermissionRuleAuditAdapter.cs
- [ ] T139 [P] [US4] Create admin web star rule API hooks for create, update, activate, suspend, list, detail, evaluation read, evaluation retry, and trace in apps/admin-web/src/features/requests/star-rules/starRulesApi.ts
- [ ] T140 [P] [US4] Create star rule form, threshold/cost fields, unavailable behavior selector, manual review behavior selector, active date controls, evaluation status badge, star trace panel, and no-balance-ownership notice in apps/admin-web/src/features/requests/star-rules/StarRuleForm.tsx and apps/admin-web/src/features/requests/star-rules/StarRuleTracePanel.tsx
- [ ] T141 [US4] Add star rule management and evaluation views to Requests configuration and request detail routes in apps/admin-web/src/app/(school)/requests/configuration/page.tsx and apps/admin-web/src/features/requests/submission/RequestTracePanel.tsx
- [ ] T142 [US4] Update Requests endpoint registration to map star rule management, star evaluation read, retry, and trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T143 [US4] Create OpenAPI examples for star rule create, activation, insufficient evidence, unavailable evidence, reserved, consumed, released, retry pending, no star balance ownership, disabled capability, and trace in apps/api/src/SafeSchool.Api/Features/Requests/StarRules/StarPermissionRuleOpenApiExamples.cs
- [ ] T144 [US4] Create star rule test data builder for sufficient stars, insufficient stars, unavailable evidence, stale evidence, disabled evidence, reservation success, reservation failure, consumption success, release success, active rule, suspended rule, and cross-school rule cases in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRuleTestData.cs
- [ ] T145 [US4] Update RequestException creation to include missing star evidence, insufficient stars, failed reservation, failed consumption, failed release, stale rule version, and manual-review-required star cases in apps/api/src/SafeSchool.Api/Features/Requests/Exceptions/RequestExceptionFactory.cs
- [ ] T146 [US4] Update RequestTraceService to include star rule evaluation references, Phase 5 evidence references, reservation references, consumption references, release references, and star exception references in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestTraceService.cs
- [ ] T147 [US4] Create contract documentation fixtures for star rule success, insufficient, unavailable, reservation, consumption, release, retry, and trace examples in tests/contracts/requests/star-permission-rules-fixtures.md

**Checkpoint**: User Story 4 can be demonstrated independently after T124-T147 pass.

---

## Phase 7: User Story 5 - Review Request History, Exceptions, and Corrections (Priority: P2)

**Goal**: Guardians, students, request managers, auditors, and reviewers can
search request history, inspect exceptions, and correct or reopen requests
without hiding original request and decision evidence.

**Independent Test**: Filter request history by student, request type, status,
date range, approver, and exception state; then correct or reopen an eligible
request with a reason and verify both the original and corrective records
remain visible to authorized users.

### Tests for User Story 5

- [ ] T148 [P] [US5] Create RequestException and ManualRequestReview domain tests for missing consent, invalid guardian, expired approval, exact duplicate, overlap review, conflicting decision, stale workflow, missing stars, invalid pickup, disabled feature, cross-school attempt, resolve, dismiss, reopen, close, escalate, migrate workflow, and history preservation in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestReviewDomainTests.cs
- [ ] T149 [P] [US5] Create RequestHistoryService unit tests for school filters, guardian privacy-limited filters, student self filters, staff-only detail hiding, assignee filter, workflow version filter, exception filter, star outcome filter, pagination, and tenant isolation in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryServiceTests.cs
- [ ] T150 [P] [US5] Create ManualRequestReviewService unit tests for correct, reopen, close, resolve exception, dismiss exception, escalate, migrate workflow version, required reason, idempotency, original decision preservation, reviewer permission, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/ManualRequestReviewServiceTests.cs
- [ ] T151 [P] [US5] Create RequestReviewSummaryService unit tests for student, guardian, request type, assignee, workflow, exception, star rule scopes, linked-student visibility, platform review scope, latest evidence time, and pagination in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestReviewSummaryServiceTests.cs
- [ ] T152 [P] [US5] Create contract tests for history, guardian history, student history, exceptions list/detail, reviews create/list/detail, review summaries, and trace routes from contracts/request-history-review.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryReviewContractTests.cs
- [ ] T153 [P] [US5] Create integration tests for history filters, guardian privacy, student privacy, exception creation, correction, reopen, resolve, dismiss, workflow migration, original evidence preservation, cross-school denial, disabled history capability, summary scope, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryReviewIntegrationTests.cs
- [ ] T154 [P] [US5] Create web journey tests for request history search, guardian history, student history, exception list, review action panel, reopen, correction, summary cards, trace view, disabled capability, and access-denied state in apps/admin-web/tests/requests/request-history-review.spec.ts
- [ ] T155 [P] [US5] Create API performance tests verifying 90-day request history loads under 30 seconds and one request trace loads under 60 seconds during review testing in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryPerformanceTests.cs

### Implementation for User Story 5

- [ ] T156 [P] [US5] Create history query, history response, exception response, manual review request, manual review response, review summary response, trace response, and guardian/student privacy DTOs matching contracts/request-history-review.md in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestHistoryReviewDtos.cs
- [ ] T157 [US5] Implement RequestExceptionFactory for missing consent, invalid guardian link, expired approval, exact duplicate, overlap review, conflicting decision, out-of-order decision, stale workflow, missing star evidence, insufficient stars, disabled feature, invalid pickup evidence, cross-school access attempt, and manual-review-required conditions in apps/api/src/SafeSchool.Api/Features/Requests/Exceptions/RequestExceptionFactory.cs
- [ ] T158 [US5] Implement RequestHistoryService for school, guardian, and student history filters by student, requester, guardian, type, status, date range, requested window, approver, assignee, workflow version, star outcome, exception type, review status, pagination, and visibility in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestHistoryService.cs
- [ ] T159 [US5] Implement RequestExceptionQueryService for exception list/detail, severity filters, status filters, reviewer assignment filters, source evidence references, tenant scope, and pagination in apps/api/src/SafeSchool.Api/Features/Requests/Exceptions/RequestExceptionQueryService.cs
- [ ] T160 [US5] Implement ManualRequestReviewService for correct, reopen, close, resolve, dismiss, escalate, migrate workflow version, required reason, original evidence preservation, status transition, exception resolution, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/ManualRequestReviewService.cs
- [ ] T161 [US5] Implement RequestReviewSummaryService for summary scopes, status counts, exception counts, manual review counts, star review counts, latest evidence time, guardian-linked summaries, platform review summaries, tenant scope, and pagination in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestReviewSummaryService.cs
- [ ] T162 [US5] Extend RequestTraceService to include complete lifecycle from creation through decisions, consent, star, pickup, exception, review, summary, status event, and audit evidence in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestTraceService.cs
- [ ] T163 [US5] Implement RequestHistoryController, GuardianRequestHistoryController, StudentRequestHistoryController, RequestExceptionsController, ManualRequestReviewsController, RequestReviewSummariesController, and RequestLifecycleTraceController routes from contracts/request-history-review.md in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestHistoryReviewControllers.cs
- [ ] T164 [US5] Wire history read, guardian history read, student history read, exception created, exception resolved, correction, reopen, close, escalation, workflow migration, summary read, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestHistoryReviewAuditAdapter.cs
- [ ] T165 [P] [US5] Create admin web history/review API hooks for history search, exception list/detail, manual review action, review list/detail, summary read, and trace read in apps/admin-web/src/features/requests/reviews/requestReviewsApi.ts
- [ ] T166 [P] [US5] Create guardian and student history API hooks with linked-student and self-scope typed errors in apps/admin-web/src/features/requests/reviews/requestHistoryApi.ts
- [ ] T167 [P] [US5] Create request history table, exception list, manual review form, correction/reopen dialog, summary cards, trace timeline, guardian privacy panel, and student privacy panel in apps/admin-web/src/features/requests/reviews/RequestHistoryReviewPanels.tsx and apps/admin-web/src/features/requests/reviews/RequestTraceTimeline.tsx
- [ ] T168 [US5] Implement school review route with history filters, exception queue, review actions, summary cards, trace view, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/requests/review/page.tsx
- [ ] T169 [US5] Implement guardian and student history sections in existing request routes with privacy-limited details and no staff-only assignment data in apps/admin-web/src/app/(guardian)/requests/page.tsx and apps/admin-web/src/app/(student)/requests/page.tsx
- [ ] T170 [US5] Update Requests endpoint registration to map history, exception, manual review, summary, and lifecycle trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T171 [US5] Create OpenAPI examples for history query, guardian history, student history, exception detail, manual review reopen, manual review correction, summary response, trace response, privacy filtering, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Requests/Reviews/RequestHistoryReviewOpenApiExamples.cs
- [ ] T172 [US5] Create review test data builder for history filters, linked guardian, unlinked guardian, student self, staff assignment, exception states, manual review actions, summaries, trace evidence, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryReviewTestData.cs
- [ ] T173 [US5] Create contract documentation fixtures for history, exception, manual review, summary, guardian privacy, student privacy, and trace examples in tests/contracts/requests/request-history-review-fixtures.md

**Checkpoint**: User Story 5 can be demonstrated independently after T148-T173 pass.

---

## Phase 8: User Story 6 - Configure Request Rules and Workflow Templates (Priority: P3)

**Goal**: School administrators can configure request types, required fields,
workflow templates, approver chains, escalation windows, consent rules,
duplicate policies, closure policies, and star rule attachment with versioned
evidence for future requests.

**Independent Test**: Create a request type and workflow template with required
fields, approver roles, escalation timing, guardian consent, and optional star
rules; activate it; submit a new request; and verify the request uses the
activated version while historical requests keep their original version.

### Tests for User Story 6

- [ ] T174 [P] [US6] Create RequestType domain tests for allowed initiator roles, required field schema, consent policy, workflow reference, star rule reference, expiry policy, duplicate policy, closure policy, versioning, active/suspended/retired states, and tenant uniqueness in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestTypeDomainTests.cs
- [ ] T175 [P] [US6] Create WorkflowTemplate domain tests for draft, active, suspended, retired, version creation, valid approver path, circular step rejection, impossible escalation rejection, missing required permission rejection, and historical version preservation in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/WorkflowTemplateDomainTests.cs
- [ ] T176 [P] [US6] Create RequestConfigurationService unit tests for request type create/update/activate/suspend, workflow template create/update/activate, invalid dependencies, disabled capabilities, stricter consent policy, duplicate policy, closure policy, star rule dependency, idempotency, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestConfigurationServiceTests.cs
- [ ] T177 [P] [US6] Create WorkflowTemplateValidationService unit tests for no approver path, circular steps, impossible escalation timing, invalid guardian consent settings, missing required fields, disabled star capability, invalid role, and valid template activation in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/WorkflowTemplateValidationServiceTests.cs
- [ ] T178 [P] [US6] Create contract tests for request type create/list/detail/update/activate/suspend, workflow template create/list/detail/update/activate, and feature settings read routes from contracts/request-configuration.md in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestConfigurationContractTests.cs
- [ ] T179 [P] [US6] Create integration tests for active request type used by future request, historical version preservation, invalid workflow activation rejection, stricter consent configuration, star rule attachment, feature setting read, cross-school configuration denial, disabled configuration capability, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestConfigurationIntegrationTests.cs
- [ ] T180 [P] [US6] Create web journey tests for request type creation, workflow template builder, step validation, activation, suspension, version history display, stricter consent selector, star rule attachment, disabled capability, and access-denied state in apps/admin-web/tests/requests/request-configuration.spec.ts

### Implementation for User Story 6

- [ ] T181 [P] [US6] Create request type config, workflow template config, workflow step config, activation request, suspension request, feature setting response, version history response, and validation error DTOs matching contracts/request-configuration.md in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestConfigurationDtos.cs
- [ ] T182 [US6] Implement RequestTypeConfigurationService for draft create, update, activate, suspend, version creation, initiator roles, required field schema, default one-authorized-guardian consent, stricter consent override, duplicate policy, closure policy, star rule dependency, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestTypeConfigurationService.cs
- [ ] T183 [US6] Implement WorkflowTemplateValidationService for valid path checking, circular step detection, impossible escalation timing, required permission checking, assignment validation, guardian consent step validation, star capability dependency, and activation errors in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/WorkflowTemplateValidationService.cs
- [ ] T184 [US6] Implement WorkflowTemplateConfigurationService for draft create, update, activate, suspend, version creation, step persistence, next-step rules, escalation targets, historical preservation, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/WorkflowTemplateConfigurationService.cs
- [ ] T185 [US6] Implement RequestFeatureSettingsQueryService for effective Phase 6 capability settings, dependent capability summaries, and tenant-scoped configuration readiness in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestFeatureSettingsQueryService.cs
- [ ] T186 [US6] Implement RequestConfigurationTraceService for request type version, workflow template version, activation, suspension, star rule attachment, active request references, historical request references, and audit evidence in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestConfigurationTraceService.cs
- [ ] T187 [US6] Implement RequestTypesController, WorkflowTemplatesController, RequestFeatureSettingsController, and RequestConfigurationTraceController routes from contracts/request-configuration.md in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestConfigurationControllers.cs
- [ ] T188 [US6] Wire request type created, activated, suspended, workflow template created, workflow activated, workflow rejected, stricter consent configured, star rule attached, feature settings read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestConfigurationAuditAdapter.cs
- [ ] T189 [P] [US6] Create admin web configuration API hooks for request types, workflow templates, feature settings, activation, suspension, validation errors, version history, and trace in apps/admin-web/src/features/requests/configuration/requestConfigurationApi.ts
- [ ] T190 [P] [US6] Create request type form, required field editor, consent policy selector, duplicate policy selector, closure policy selector, star rule selector, and version history panel in apps/admin-web/src/features/requests/configuration/RequestTypeForm.tsx and apps/admin-web/src/features/requests/configuration/RequestTypeVersionHistory.tsx
- [ ] T191 [P] [US6] Create workflow template builder, workflow step editor, escalation rule editor, invalid path warning, activation validation panel, and workflow version history panel in apps/admin-web/src/features/requests/configuration/WorkflowTemplateBuilder.tsx and apps/admin-web/src/features/requests/configuration/WorkflowVersionHistory.tsx
- [ ] T192 [US6] Implement school configuration route with request type list, workflow template list, feature setting read, create/edit/activate/suspend flows, validation errors, version history, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/requests/configuration/page.tsx
- [ ] T193 [US6] Update RequestSubmissionService to use active request type and workflow template versions from configuration and preserve historical request type/workflow versions at submission in apps/api/src/SafeSchool.Api/Features/Requests/Requests/RequestSubmissionService.cs
- [ ] T194 [US6] Update Requests endpoint registration to map request type, workflow template, feature setting, and configuration trace routes in apps/api/src/SafeSchool.Api/Features/Requests/RequestEndpointRegistration.cs
- [ ] T195 [US6] Create OpenAPI examples for request type create, activate, invalid activation, workflow template create, workflow circular rejection, workflow activation, feature settings, version history, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Requests/Configuration/RequestConfigurationOpenApiExamples.cs
- [ ] T196 [US6] Create configuration test data builder for request type drafts, active request types, stricter consent types, star-gated types, invalid field schemas, valid workflows, circular workflows, impossible escalation workflows, invalid role workflows, and cross-school configuration cases in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestConfigurationTestData.cs
- [ ] T197 [US6] Create contract documentation fixtures for request type, workflow template, feature setting, activation, rejection, version history, and trace examples in tests/contracts/requests/request-configuration-fixtures.md

**Checkpoint**: User Story 6 can be demonstrated independently after T174-T197 pass.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full Phase 6 slice against quality gates, boundaries,
observability, and quickstart scenarios.

- [ ] T198 [P] Run full API unit, integration, contract, authorization, tenant-isolation, audit, and performance test suites for Requests and record results in specs/006-requests-permissions/implementation-validation.md
- [ ] T199 [P] Run admin web Requests route and component tests for school, guardian, student, approver, reviewer, early leave, star rule, history, and configuration journeys and record results in specs/006-requests-permissions/implementation-validation.md
- [ ] T200 [P] Run mobile Requests tests for optional request submission, status, early leave, and approval surfaces and record results in specs/006-requests-permissions/implementation-validation.md
- [ ] T201 Review all Requests endpoints for tenant, feature, permission, guardian scope, student scope, assigned approver scope, platform review scope, and audit enforcement in apps/api/src/SafeSchool.Api/Features/Requests/
- [ ] T202 Review Phase 6 boundary guards to confirm no attendance, campus gate, NFC/QR scan, transport, wallet, learning, medical, complaint, messaging delivery, document, search, or broad dashboard side effects in apps/api/src/SafeSchool.Api/Features/Requests/Common/Boundaries/
- [ ] T203 [P] Review OpenAPI examples and contract fixtures for all six contracts and align examples with implemented DTO names in apps/api/src/SafeSchool.Api/Features/Requests/ and tests/contracts/requests/
- [ ] T204 [P] Review web accessibility, empty, loading, error, disabled capability, and access-denied states for all Requests pages in apps/admin-web/src/app/(school)/requests/, apps/admin-web/src/app/(guardian)/requests/, and apps/admin-web/src/app/(student)/requests/
- [ ] T205 Review structured logs, metrics, error reporting, audit events, and status event export for request lifecycle, workflow queues, expiry routing, star rules, early leave release eligibility, exceptions, reviews, and access denial in apps/api/src/SafeSchool.Api/Features/Requests/Audit/
- [ ] T206 Execute every validation scenario from quickstart.md and document pass/fail evidence in specs/006-requests-permissions/implementation-validation.md
- [ ] T207 Update Requests implementation README with module ownership, configuration keys, permission keys, route map, test commands, and known phase boundaries in docs/requests/README.md
- [ ] T208 Run final formatting, linting, type checking, migration validation, and test commands for API, admin web, and mobile surfaces and record command outputs in specs/006-requests-permissions/implementation-validation.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories.
- **User Stories (Phase 3+)**: Depend on Foundational completion.
- **Polish (Phase 9)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Foundation; MVP scope.
- **User Story 2 (P1)**: Starts after Foundation; can use seeded requests and workflows, then integrates with US1.
- **User Story 3 (P1)**: Starts after Foundation; can use seeded approvals, then integrates with US2.
- **User Story 4 (P2)**: Starts after Foundation; integrates with US1 submission and US2 approval lifecycle for reservation/consumption/release.
- **User Story 5 (P2)**: Starts after Foundation; can use seeded request evidence, then integrates with all prior stories for full trace coverage.
- **User Story 6 (P3)**: Starts after Foundation; can use seeded config in tests, then replaces seeded config paths with admin-managed versioned configuration.

### Within Each User Story

- Tests required by the constitution must be written and must fail before implementation.
- Domain models and DTOs before services.
- Services before controllers/endpoints.
- Tenant, feature flag, permission, idempotency, and audit enforcement before UI exposure.
- API and contract behavior before web/mobile UI integration.
- Story complete before moving to the next priority when working sequentially.

---

## Parallel Opportunities

- Setup tasks T002-T008 and T010-T013 can run in parallel after T001.
- Foundational adapter/test builder tasks T022-T027, T036-T042, and T045-T050 can run in parallel once common contracts T014-T021 are stable.
- User story test tasks marked `[P]` can run in parallel at the start of each story.
- UI component tasks marked `[P]` can run in parallel with API OpenAPI example tasks after story services and DTOs are defined.
- US2, US3, US4, US5, and US6 can be staffed in parallel after Foundation if each team uses seeded fixtures and coordinates shared service integration points.

---

## Parallel Example: User Story 1

```bash
Task: "T051 [P] [US1] Create PermissionRequest domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/PermissionRequestDomainTests.cs"
Task: "T054 [P] [US1] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Requests/OutingPermissionRequestsContractTests.cs"
Task: "T056 [P] [US1] Create web journey tests in apps/admin-web/tests/requests/request-submission.spec.ts"
```

## Parallel Example: User Story 2

```bash
Task: "T075 [P] [US2] Create WorkflowStep and WorkflowDecision domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/WorkflowDecisionDomainTests.cs"
Task: "T078 [P] [US2] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Workflows/ApprovalWorkflowContractTests.cs"
Task: "T080 [P] [US2] Create web journey tests in apps/admin-web/tests/requests/approval-workflow.spec.ts"
```

## Parallel Example: User Story 3

```bash
Task: "T100 [P] [US3] Create EarlyLeaveDetail and PickupEvidence domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveDomainTests.cs"
Task: "T103 [P] [US3] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/EarlyLeave/EarlyLeaveContractTests.cs"
Task: "T105 [P] [US3] Create web journey tests in apps/admin-web/tests/requests/early-leave.spec.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T124 [P] [US4] Create StarPermissionRule domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRuleDomainTests.cs"
Task: "T127 [P] [US4] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/StarRules/StarPermissionRulesContractTests.cs"
Task: "T129 [P] [US4] Create web journey tests in apps/admin-web/tests/requests/star-permission-rules.spec.ts"
```

## Parallel Example: User Story 5

```bash
Task: "T148 [P] [US5] Create RequestException and ManualRequestReview domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestReviewDomainTests.cs"
Task: "T152 [P] [US5] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Reviews/RequestHistoryReviewContractTests.cs"
Task: "T154 [P] [US5] Create web journey tests in apps/admin-web/tests/requests/request-history-review.spec.ts"
```

## Parallel Example: User Story 6

```bash
Task: "T174 [P] [US6] Create RequestType domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestTypeDomainTests.cs"
Task: "T178 [P] [US6] Create contract tests in apps/api/tests/SafeSchool.Api.Tests/Features/Requests/Configuration/RequestConfigurationContractTests.cs"
Task: "T180 [P] [US6] Create web journey tests in apps/admin-web/tests/requests/request-configuration.spec.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate request submission, tracking, duplicate blocking, overlap
   review, withdrawal, tenant isolation, and audit behavior independently.

### Incremental Delivery

1. Complete Setup and Foundation.
2. Add US1 request submission and tracking.
3. Add US2 approval workflow decisions.
4. Add US3 early leave release eligibility.
5. Add US4 star-based rules.
6. Add US5 history, exceptions, and reviews.
7. Add US6 configuration UI and versioned activation.
8. Run Phase 9 polish and quickstart validation.

### Parallel Team Strategy

1. Team completes Setup and Foundation together.
2. After Foundation:
   - Developer A: US1 request submission and tracking.
   - Developer B: US2 approval workflow using seeded requests.
   - Developer C: US3 early leave using seeded approvals.
   - Developer D: US4 star rules using seeded request types.
   - Developer E: US5 review/history using seeded evidence.
   - Developer F: US6 configuration using isolated config fixtures.
3. Reconcile shared integrations at each checkpoint.

## Notes

- `[P]` tasks touch different files and can run in parallel after their phase prerequisites are met.
- `[US#]` labels map tasks to the user stories in [spec.md](./spec.md).
- Every user story is independently testable with seeded fixtures if earlier story UI is not implemented yet.
- Commit after each task or logical group.
