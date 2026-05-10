# Tasks: Phase 5 Learning & Engagement

**Input**: Design documents from `/specs/006-learning-engagement/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 5 plan require unit,
integration, contract, authorization, tenant-isolation, audit, performance, and
critical UI journey coverage. Write test tasks before implementation tasks in
each user-story phase and confirm they fail for the missing behavior before
completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a
task is marked `[P]`. Do not implement attendance generation, campus entry or
exit decisions, NFC or QR scan processing, transport boarding/drop decisions,
wallet or payment actions, request approval workflows, medical or emergency
workflows, complaint escalation, broad messaging, broadcasts, document storage,
global search, or broad admin dashboards. Every Phase 5 action must resolve
tenant context, check the required capability, enforce permission, prevent
cross-school visibility, preserve versioned learning evidence, handle exact
duplicates idempotently, route conflicting non-identical records to manual
review, record denied access decisions, and emit audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no
  dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses
described in [plan.md](./plan.md).

- [X] T001 Create or update API solution and Learning source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/Learning/
- [X] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, logging, and background worker dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [X] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, time provider fakes, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [X] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, route tests, and component test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [X] T005 [P] Create or update mobile package manifest with Flutter test, HTTP client, auth header, and learning surface dependencies in apps/mobile/pubspec.yaml
- [X] T006 [P] Create Learning school, guardian, and student route placeholders plus feature exports in apps/admin-web/src/app/(school)/learning/page.tsx, apps/admin-web/src/app/(guardian)/learning/page.tsx, apps/admin-web/src/app/(student)/learning/page.tsx, and apps/admin-web/src/features/learning/index.ts
- [X] T007 [P] Create Learning content, assignments, quizzes, stars, rewards, behavior, history, review, and configuration route placeholders in apps/admin-web/src/app/(school)/learning/content/page.tsx, apps/admin-web/src/app/(school)/learning/assignments/page.tsx, apps/admin-web/src/app/(school)/learning/quizzes/page.tsx, apps/admin-web/src/app/(school)/learning/stars/page.tsx, apps/admin-web/src/app/(school)/learning/rewards/page.tsx, apps/admin-web/src/app/(school)/learning/behavior/page.tsx, apps/admin-web/src/app/(school)/learning/history/page.tsx, apps/admin-web/src/app/(school)/learning/review/page.tsx, and apps/admin-web/src/app/(school)/learning/configuration/page.tsx
- [X] T008 [P] Create mobile Learning module export shell in apps/mobile/lib/features/learning/learning.dart
- [X] T009 Create API bootstrap additions for Learning endpoint registration, versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, and background summary/status-event registration in apps/api/src/SafeSchool.Api/Program.cs
- [X] T010 [P] Create API configuration placeholders for learning capabilities, guardian visibility defaults, status event export, Phase 6 star evidence export, audit, duplicate overlap thresholds, and summary refresh windows in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [X] T011 [P] Create contract test documentation index linking the seven Phase 5 contracts in tests/contracts/learning/README.md
- [X] T012 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/learning/README.md
- [X] T013 [P] Create repository generated-file ignores for Learning API, web, mobile, coverage, contract fixture, seed data, and local learning resource artifacts in .gitignore

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, identity,
guardian, persistence, idempotency, audit, API, web, and mobile foundations
required by every Phase 5 user story.

**Critical**: No user-story implementation should start until this phase is
complete.

- [X] T014 Create shared Learning tenant-owned entity base, result types, validation error type, paged response type, time provider abstraction, review reason type, and source metadata type in apps/api/src/SafeSchool.Api/Features/Learning/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningOperationResults.cs
- [X] T015 [P] Create shared Learning enums for course status, group status, membership status, staff assignment role/status, content status, completion policy, progress status, assignment status, late policy, review policy, submission status, quiz status, question type, attempt status, score status, scoring policy, feedback visibility, star ledger direction/state, reward status, redemption status, fulfillment status, behavior classification/severity/review state, exception type/severity/status, review action, rule area/status, summary scope, status event type, and capability keys in apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningEnums.cs
- [X] T016 Create or extend SafeSchoolDbContext registration for Learning entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [X] T017 Create Learning model-builder extension for tenant metadata, timestamps, revision fields, course/group indexes, membership indexes, content visibility windows, assignment due queues, submission idempotency, quiz attempt idempotency, star ledger source references, star balance lookups, reward eligibility, behavior filters, exception status, review summaries, status event export, and audit traceability in apps/api/src/SafeSchool.Api/Features/Learning/LearningDbContextModelBuilderExtensions.cs
- [X] T018 Create Phase 5 capability constants for learning.content_delivery, learning.assignments, learning.quizzes, learning.stars_rewards, learning.rewards, learning.behavior_logging, learning.progress_history, learning.configuration, and learning.review_summaries in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/LearningCapabilities.cs
- [X] T019 Create Learning permission catalog entries for course manage, content publish/read, progress read/write, assignment manage/submit/review/read, quiz manage/attempt/review/read, stars read/manage, rewards read/manage/redeem, behavior read/create/review, history read, guardian history read, configuration read/manage, reviews manage, summaries read, audit read, and platform review in apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningPermissionCatalog.cs
- [X] T020 Create Learning permission guard that wraps tenant context, feature gate, permission evaluation, approved guardian link scope, student self scope, staff course/group assignment scope, reward manager scope, behavior reviewer scope, platform reviewer scope, denied access writing, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningPermissionGuard.cs
- [X] T021 Create Learning feature gate service that enforces Phase 5 capability keys server-side and returns typed disabled-capability errors in apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningFeatureGate.cs
- [X] T022 Create IdentityAccess student profile adapter interface and test fake for active, inactive, suspended, graduated, transferred, duplicated, missing, and cross-tenant student profiles in apps/api/src/SafeSchool.Api/Features/Learning/Common/Identity/LearningStudentProfileProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/FakeLearningStudentProfileProvider.cs
- [X] T023 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, restricted visibility, staff-only visibility, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/Learning/Common/Identity/LearningGuardianLinkProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/FakeLearningGuardianLinkProvider.cs
- [X] T024 Create IdentityAccess staff learning assignment adapter interface and test fake for teacher, teaching assistant, coordinator, reviewer, reward manager, behavior reviewer, disabled actor, transferred actor, removed assignment, and missing permission cases in apps/api/src/SafeSchool.Api/Features/Learning/Common/Identity/StaffLearningAssignmentProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/FakeStaffLearningAssignmentProvider.cs
- [X] T025 Create Phase 6 star evidence export adapter interface and test fake for available, reserved, consumed, stale, corrected, disabled, unavailable, and cross-tenant star evidence reads in apps/api/src/SafeSchool.Api/Features/Learning/Common/StarEvidence/Phase6StarEvidenceExportProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/FakePhase6StarEvidenceExportProvider.cs
- [X] T026 Create Learning phase boundary guard that prevents attendance, campus gate, NFC, QR, transport, wallet, request approval, medical, complaint, document, search, broad dashboard, and direct notification delivery side effects from Phase 5 in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs
- [X] T027 Create Learning idempotency service for content publication, progress events, assignments, submissions, quiz attempts, star ledger entries, reward redemptions, behavior events, manual reviews, rule settings, and status event export in apps/api/src/SafeSchool.Api/Features/Learning/Common/Idempotency/LearningIdempotencyService.cs
- [X] T028 Create Learning audit event entity and audit writer adapter for course creation, content publication, content withdrawal, progress change, assignment creation, submission, grading, quiz activation, attempt lifecycle, quiz scoring, star award/correction/reservation/consumption/release, reward redemption/fulfillment, behavior creation/correction, exception, manual review, summary read, configuration change, and access denial in apps/api/src/SafeSchool.Api/Features/Learning/Audit/LearningAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/Learning/Audit/LearningAuditWriter.cs
- [X] T029 Create Learning trace reference type for course, group, content, progress, assignment, submission, quiz, attempt, star, reward, behavior, exception, review, rule, summary, status event, and audit links in apps/api/src/SafeSchool.Api/Features/Learning/Common/Trace/LearningTraceReference.cs
- [X] T030 Create Learning status event export model for later Phase 9 notification eligibility and Phase 6 star evidence eligibility without delivery or approval side effects in apps/api/src/SafeSchool.Api/Features/Learning/Audit/LearningStatusEvent.cs
- [X] T031 Create core domain entity files in apps/api/src/SafeSchool.Api/Features/Learning/Domain/Course.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningGroup.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningGroupMembership.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/StaffLearningAssignment.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningContentItem.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningProgressEvent.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/Assignment.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/AssignmentSubmission.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/Quiz.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/QuizQuestion.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/QuizAttempt.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/QuizResponse.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/StarRuleSetting.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/StarLedgerEntry.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/StarBalanceSnapshot.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/RewardCatalogItem.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/RewardRedemption.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/BehaviorCategory.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/BehaviorEvent.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningException.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/ManualLearningReview.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningRuleSetting.cs, apps/api/src/SafeSchool.Api/Features/Learning/Domain/LearningReviewSummary.cs, and apps/api/src/SafeSchool.Api/Features/Learning/Domain/SchoolAccountFeatureSetting.cs
- [X] T032 Create entity configurations and initial EF migration for all Phase 5 tenant-owned tables, foreign keys, revision indexes, duplicate indexes, visibility indexes, lookup indexes, status-event indexes, summary indexes, and audit indexes in apps/api/src/SafeSchool.Api/Features/Learning/Persistence/LearningEntityTypeConfigurations.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605050002_LearningFoundation.cs
- [X] T033 Create Learning DTO primitives for paging, typed errors, capability status, visibility policy, tenant scope, source references, trace links, audit references, idempotency keys, and status event payloads in apps/api/src/SafeSchool.Api/Features/Learning/Common/LearningDtos.cs
- [X] T034 Create Learning route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/learning, /api/v1/students/me/learning, and /api/v1/guardians/me/students/{studentProfileId}/learning in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T035 Create Learning OpenAPI tag registration and shared response/error examples for tenant mismatch, disabled capability, missing permission, invalid guardian link, invalid student, inactive group, duplicate command, conflicting overlap, stale rule, insufficient stars, unavailable reward, sensitive behavior, cross-school access, no-side-effect guard, and audit failure in apps/api/src/SafeSchool.Api/Features/Learning/LearningOpenApiExamples.cs
- [X] T036 Create API test fixture for tenants, capabilities, permissions, students, guardians, staff assignments, courses, groups, content, assignments, quizzes, stars, rewards, behavior, exceptions, reviews, summaries, feature settings, idempotency, status events, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/LearningTestFixture.cs
- [X] T037 [P] Create shared API test data builders for courses, learning groups, memberships, staff assignments, content, progress, assignments, submissions, quizzes, questions, attempts, responses, star rules, ledger entries, balances, rewards, redemptions, behavior categories, behavior events, exceptions, reviews, summaries, feature settings, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Fixtures/LearningTestData.cs
- [X] T038 [P] Create unit tests for LearningFeatureGate capability decisions, disabled workflow denial, independent capability keys, and backend enforcement behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningFeatureGateTests.cs
- [X] T039 [P] Create unit tests for LearningPermissionGuard tenant mismatch, guardian scope mismatch, student self scope mismatch, staff assignment mismatch, reward manager denial, behavior reviewer denial, platform review allowance, denied access audit, and allowed access in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningPermissionGuardTests.cs
- [X] T040 [P] Create unit tests for LearningIdempotencyService retry, exact duplicate, conflicting duplicate, duplicate progress, duplicate submission, duplicate quiz attempt, duplicate star award, duplicate reward redemption, duplicate behavior event, and duplicate manual review behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningIdempotencyServiceTests.cs
- [X] T041 [P] Create unit tests for LearningPhaseBoundaryGuard preventing attendance, campus gate, scan, transport, wallet, request approval, medical, complaint, document, search, broad dashboard, and direct notification delivery side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningPhaseBoundaryGuardTests.cs
- [X] T042 [P] Create unit tests for LearningAuditWriter mutation failure behavior, denied access evidence, sensitive payload protection, and status-event audit payloads in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningAuditWriterTests.cs
- [X] T043 [P] Create unit tests for LearningStatusEvent export eligibility, Phase 9 notification readiness, Phase 6 star evidence readiness, tenant scope, idempotent export, and no delivery or approval side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningStatusEventTests.cs
- [X] T044 Create StarBalanceSnapshot recalculation shell that derives balances from append-only ledger entries and never treats snapshots as authority when ledger evidence disagrees in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarBalanceSnapshotService.cs
- [X] T045 Create Learning exception factory shell for invalid enrollment, inactive student, missing evidence, late submission, duplicate submission, duplicate quiz attempt, scoring conflict, stale rule, missing source evidence, duplicate star award, insufficient stars, unavailable reward, invalid behavior record, disabled feature, cross-school access, and manual-review-required conditions in apps/api/src/SafeSchool.Api/Features/Learning/Exceptions/LearningExceptionFactory.cs
- [X] T046 Create Learning rule version service shell for content, assignment, quiz, star, reward, behavior, visibility, and review rule version capture in apps/api/src/SafeSchool.Api/Features/Learning/Configuration/LearningRuleVersionService.cs
- [X] T047 Create Learning visibility service base for student, guardian, staff, reviewer, reward manager, behavior reviewer, auditor, and platform reviewer filtering in apps/api/src/SafeSchool.Api/Features/Learning/Common/Visibility/LearningVisibilityService.cs
- [X] T048 Create Learning review reason validator for correction, reopen, close, resolve, dismiss, escalate, migrate rule version, withdrawal, denial, cancellation, and sensitive visibility changes in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewReasonValidator.cs
- [X] T049 Create Learning status event exporter shell for progress, assignment, quiz, star, reward, behavior, exception, and review events in apps/api/src/SafeSchool.Api/Features/Learning/Audit/LearningStatusEventExporter.cs
- [X] T050 Create Learning seed/bootstrap fixture for minimal course, active group, assigned student, linked guardian, teacher assignment, content, assignment, quiz, star rule, reward, behavior category, and feature settings in apps/api/src/SafeSchool.Api/Features/Learning/Fixtures/LearningSeedData.cs
- [X] T051 [P] Create school web Learning API client with tenant context, typed errors, pagination, capability-disabled handling, course/content, assignment, quiz, stars, rewards, behavior, history, review, configuration, and trace routes in apps/admin-web/src/features/learning/api/learningApi.ts
- [X] T052 [P] Create guardian web Learning API client with linked-student scope, typed errors, content, assignments, quizzes, stars, rewards, behavior, history, and trace routes in apps/admin-web/src/features/learning/api/guardianLearningApi.ts
- [X] T053 [P] Create student web Learning API client with self-scope, typed errors, content, progress, assignments, quizzes, rewards, behavior, history, and status routes in apps/admin-web/src/features/learning/api/studentLearningApi.ts
- [X] T054 [P] Create mobile Learning API client shell with auth headers, tenant context, linked-student context, typed errors, content, assignments, quizzes, stars, rewards, behavior, and history methods in apps/mobile/lib/features/learning/learning_api.dart
- [X] T055 [P] Create shared web test data builders for courses, content, assignments, submissions, quizzes, attempts, stars, rewards, behavior, exceptions, reviews, summaries, and feature settings in apps/admin-web/tests/learning/learningTestData.ts
- [X] T056 [P] Create contract fixture documentation for common learning errors, tenant isolation, feature disabled, missing permission, idempotency, visibility filtering, no-side-effect boundaries, and trace payloads in tests/contracts/learning/common-fixtures.md
- [X] T057 [P] Create backend authorization integration tests covering all Learning capabilities, role permissions, guardian scopes, student self scopes, staff course/group assignments, reviewer scopes, reward manager scopes, behavior reviewer scopes, platform review scopes, denied access audit, and cross-tenant denial in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningAuthorizationIntegrationTests.cs
- [X] T058 [P] Create migration tests verifying Learning tenant indexes, foreign keys, unique idempotency keys, star ledger append-only constraints, visibility indexes, exception indexes, summary indexes, and audit indexes in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Foundational/LearningMigrationTests.cs

**Checkpoint**: Foundation ready. User-story work can start after T014-T058 are complete.

---

## Phase 3: User Story 1 - Publish and Consume Learning Content (Priority: P1) MVP

**Goal**: Authorized staff can publish courses and learning content to assigned
students, while students and linked guardians can view allowed details and
progress without cross-tenant or out-of-scope exposure.

**Independent Test**: Publish a course content item for an active learning
group in one school account, verify assigned students and linked guardians can
view allowed details, and confirm unrelated students, guardians, staff, and
school accounts cannot discover or open it.

### Tests for User Story 1

- [X] T059 [P] [US1] Create Course domain tests for school-scoped unique course code, active owner requirement, suspended/archived mutation denial, visibility policy, tenant ownership, and audit timestamp requirements in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Courses/CourseDomainTests.cs
- [X] T060 [P] [US1] Create LearningGroup and membership domain tests for active student membership, overlapping membership idempotency, removed student history preservation, staff assignment scope, guardian visibility policy, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Groups/LearningGroupDomainTests.cs
- [X] T061 [P] [US1] Create LearningContentItem domain tests for required title, resource reference, target learners, release window, publish/withdraw/expire/archive transitions, revision preservation, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/LearningContentItemDomainTests.cs
- [X] T062 [P] [US1] Create ContentPublicationService unit tests for authorized teacher publish, coordinator publish, disabled learning.content_delivery denial, missing resource denial, withdrawn content, expired content, unassigned learner denial, cross-school denial, duplicate publish retry, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/ContentPublicationServiceTests.cs
- [X] T063 [P] [US1] Create ContentVisibilityService unit tests for student self view, linked guardian summary view, staff assignment view, staff-only details hidden, draft hidden, withdrawn history view, expired hidden, disabled feature hidden, and no cross-school existence leak in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/ContentVisibilityServiceTests.cs
- [X] T064 [P] [US1] Create LearningProgressService unit tests for start, resume, complete, correction, duplicate source event, invalid student, unassigned student, withdrawn content progress, guardian visibility update, status event export, and no attendance/transport/wallet/request side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Progress/LearningProgressServiceTests.cs
- [X] T065 [P] [US1] Create contract tests for course create/list, group create/update, content create/publish/withdraw, student content list, guardian content list, progress record, and content trace routes from contracts/course-content-delivery.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/CourseContentDeliveryContractTests.cs
- [X] T066 [P] [US1] Create integration tests for tenant isolation, learning.content_delivery disabled, missing learning.content.publish permission, active group eligibility, guardian link states, student self scope, duplicate progress, content withdrawal after progress, trace read, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/CourseContentDeliveryIntegrationTests.cs
- [X] T067 [P] [US1] Create web journey tests for teacher content publish, student content view/progress, guardian progress summary, disabled capability, access-denied state, withdrawn history state, and trace link display in apps/admin-web/tests/learning/course-content-delivery.spec.ts
- [X] T068 [P] [US1] Create API performance test verifying authorized staff can publish a complete course content item for assigned students in under 2 minutes during review testing in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/CourseContentDeliveryPerformanceTests.cs

### Implementation for User Story 1

- [X] T069 [P] [US1] Create course, group, membership, staff assignment, content create, publish, withdraw, progress, list filter, visibility, and trace DTOs matching contracts/course-content-delivery.md in apps/api/src/SafeSchool.Api/Features/Learning/Content/CourseContentDtos.cs
- [X] T070 [US1] Implement CourseService for create, update, activate, suspend, archive, school-scoped code uniqueness, active owner validation, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Courses/CourseService.cs
- [X] T071 [US1] Implement LearningGroupService for group create/update, membership add/remove/suspend, effective date windows, active student validation, duplicate membership handling, tenant checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Groups/LearningGroupService.cs
- [X] T072 [US1] Implement StaffLearningAssignmentService for teacher, teaching assistant, coordinator, reviewer, effective windows, removed staff behavior, tenant checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Groups/StaffLearningAssignmentService.cs
- [X] T073 [US1] Implement ContentPublicationService for draft create, publish, withdraw, expire, archive, target learner validation, resource reference validation, revision capture, duplicate publish handling, exception creation, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Content/ContentPublicationService.cs
- [X] T074 [US1] Implement ContentVisibilityService for student, guardian, staff, reviewer, auditor, and platform reviewer content visibility with draft/withdrawn/expired/unassigned/disabled/cross-school filtering in apps/api/src/SafeSchool.Api/Features/Learning/Content/ContentVisibilityService.cs
- [X] T075 [US1] Implement LearningProgressService for start, resume, complete, correction, progress percent validation, idempotent source event handling, source trace, status event export, and no-side-effect guard invocation in apps/api/src/SafeSchool.Api/Features/Learning/Progress/LearningProgressService.cs
- [X] T076 [US1] Implement LearningContentTraceService for content to course, group, progress, stars, exceptions, reviews, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Learning/Content/LearningContentTraceService.cs
- [X] T077 [US1] Implement CourseController, LearningGroupController, ContentController, StudentContentController, GuardianContentController, ProgressController, and ContentTraceController routes from contracts/course-content-delivery.md in apps/api/src/SafeSchool.Api/Features/Learning/Content/CourseContentControllers.cs
- [X] T078 [US1] Wire course create, group update, content create, publish, withdraw, progress start, progress complete, progress correction, trace read, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Content/CourseContentAuditAdapter.cs
- [X] T079 [US1] Wire content publication and progress status events for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Learning/Content/CourseContentStatusEventAdapter.cs
- [X] T080 [US1] Update Learning endpoint registration to map course, group, content, student content, guardian content, progress, and trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T081 [US1] Create OpenAPI examples for course create, group create, content publish, content withdraw, student list, guardian list, progress record, duplicate progress retry, disabled capability, access denial, no-side-effect assertion, and content trace in apps/api/src/SafeSchool.Api/Features/Learning/Content/CourseContentOpenApiExamples.cs
- [X] T082 [P] [US1] Create school web course/content API hooks for list, create, publish, withdraw, trace, progress summary, and typed denial errors in apps/admin-web/src/features/learning/content/courseContentApi.ts
- [X] T083 [P] [US1] Create content publish form, course/group selector, learner target selector, content status table, visibility badge, progress summary panel, and content trace panel in apps/admin-web/src/features/learning/content/ContentPublishForm.tsx and apps/admin-web/src/features/learning/content/ContentTracePanel.tsx
- [X] T084 [US1] Implement school content route with publish, withdraw, status filtering, target learner validation, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/content/page.tsx
- [X] T085 [US1] Implement student and guardian learning content routes with allowed detail filtering, progress actions, withdrawn/expired states, linked-student scope, disabled capability state, and access-denied state in apps/admin-web/src/app/(student)/learning/page.tsx and apps/admin-web/src/app/(guardian)/learning/page.tsx
- [X] T086 [P] [US1] Create mobile content models and repository methods for optional student and guardian learning content surfaces in apps/mobile/lib/features/learning/content_models.dart and apps/mobile/lib/features/learning/content_repository.dart
- [X] T087 [US1] Create course content test data builder for active course, suspended course, active group, removed membership, assigned student, unassigned student, linked guardian, withdrawn content, expired content, duplicate progress, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Content/CourseContentTestData.cs

**Checkpoint**: User Story 1 can be demonstrated independently after T059-T087 pass.

---

## Phase 4: User Story 2 - Track Assignments and Submissions (Priority: P1)

**Goal**: Teachers can create assignments, students can submit or resubmit
work, and authorized reviewers can grade, return, excuse, or route submissions
while preserving history and visibility rules.

**Independent Test**: Create an assignment for a learning group, submit work as
an assigned student, grade it as an authorized teacher, and verify submission
status, feedback, visibility, late handling, and audit evidence.

### Tests for User Story 2

- [X] T088 [P] [US2] Create Assignment domain tests for required instructions, required evidence, due window, late policy, review policy, active target learners, configuration revision capture, status transitions, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentDomainTests.cs
- [X] T089 [P] [US2] Create AssignmentSubmission domain tests for draft, submitted, resubmitted, returned, graded, excused, late, withdrawn, needs review, attempt history preservation, feedback visibility, idempotency key, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentSubmissionDomainTests.cs
- [X] T090 [P] [US2] Create AssignmentCreationService unit tests for authorized teacher create, coordinator create, disabled learning.assignments denial, invalid due window denial, missing evidence denial, inactive group denial, unassigned staff denial, duplicate create retry, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentCreationServiceTests.cs
- [X] T091 [P] [US2] Create AssignmentSubmissionService unit tests for eligible student submit, resubmit allowed, resubmit denied, missing evidence route to review, late block, late review, inactive student denial, unassigned student denial, duplicate submission retry, conflicting submission review, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentSubmissionServiceTests.cs
- [X] T092 [P] [US2] Create AssignmentReviewService unit tests for grade, return, excuse, mark late, needs review, feedback visibility, reviewer permission, prior evidence preservation, correction reason, disabled feature denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentReviewServiceTests.cs
- [X] T093 [P] [US2] Create contract tests for assignment create/list, student assignment list, guardian assignment list, submit/resubmit, review, and assignment trace routes from contracts/assignment-tracking.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentTrackingContractTests.cs
- [X] T094 [P] [US2] Create integration tests for tenant isolation, learning.assignments disabled, missing permission, teacher assignment scope, guardian visibility states, late submission behavior, duplicate submission idempotency, conflicting submission manual review, grading visibility, progress event update, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentTrackingIntegrationTests.cs
- [X] T095 [P] [US2] Create web journey tests for teacher assignment create, student submit, student resubmit, teacher grade/return/excuse, guardian outcome view, late state display, disabled capability, access-denied state, and trace link display in apps/admin-web/tests/learning/assignment-tracking.spec.ts
- [X] T096 [P] [US2] Create API performance test verifying eligible students can submit a complete assignment in under 3 minutes and authorized teachers can find the submission for review in under 60 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentTrackingPerformanceTests.cs

### Implementation for User Story 2

- [X] T097 [P] [US2] Create assignment create, list filter, student assignment, guardian assignment, submission, resubmission, review, feedback visibility, late state, status response, and trace DTOs matching contracts/assignment-tracking.md in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentTrackingDtos.cs
- [X] T098 [US2] Implement AssignmentCreationService for create, update draft, close, archive, target learner validation, due/submission window validation, required evidence validation, rule version capture, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentCreationService.cs
- [X] T099 [US2] Implement AssignmentEligibilityService for active student, group membership, course scope, guardian visibility, staff assignment, late policy, closed assignment, inactive group, disabled feature, and cross-school denial checks in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentEligibilityService.cs
- [X] T100 [US2] Implement AssignmentSubmissionService for submit, resubmit, withdraw, attempt numbering, evidence reference validation, idempotent client request handling, conflicting submission review routing, progress event update, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentSubmissionService.cs
- [X] T101 [US2] Implement AssignmentSubmissionHistoryService for append-only attempt history, prior evidence preservation, feedback history, late/excused state projection, guardian-safe history projection, and trace references in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentSubmissionHistoryService.cs
- [X] T102 [US2] Implement AssignmentReviewService for grade, return, excuse, mark late, needs review, correction, feedback visibility, reviewer permission, review reason validation, status event export, star source event handoff, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentReviewService.cs
- [X] T103 [US2] Implement AssignmentExceptionService for missing evidence, late submission, duplicate submission, invalid enrollment, inactive student, disabled feature, cross-school access, conflicting submission, and manual-review-required exception creation in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentExceptionService.cs
- [X] T104 [US2] Implement AssignmentTraceService for assignment to submissions, progress, stars, exceptions, reviews, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentTraceService.cs
- [X] T105 [US2] Implement AssignmentController, StudentAssignmentController, GuardianAssignmentController, AssignmentSubmissionController, AssignmentReviewController, and AssignmentTraceController routes from contracts/assignment-tracking.md in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentControllers.cs
- [X] T106 [US2] Wire assignment create, submission, resubmission, withdrawal, grading, return, excuse, late route, review route, trace read, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentAuditAdapter.cs
- [X] T107 [US2] Wire assignment submitted, assignment reviewed, assignment returned, assignment excused, and assignment late status events for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentStatusEventAdapter.cs
- [X] T108 [US2] Update Learning endpoint registration to map school assignment, student assignment, guardian assignment, submission, review, and trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T109 [US2] Create OpenAPI examples for assignment create, student list, guardian list, submit, resubmit, grade, return, excuse, late review, duplicate retry, conflicting review route, disabled capability, access denial, and assignment trace in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentOpenApiExamples.cs
- [X] T110 [P] [US2] Create school, student, and guardian assignment API hooks for list, create, submit, resubmit, review, feedback visibility, trace, and typed denial errors in apps/admin-web/src/features/learning/assignments/assignmentApi.ts
- [X] T111 [P] [US2] Create teacher assignment form, due window editor, required evidence editor, submission table, submission review panel, late/excused badge, feedback visibility control, and assignment trace panel in apps/admin-web/src/features/learning/assignments/AssignmentEditor.tsx and apps/admin-web/src/features/learning/assignments/AssignmentReviewPanel.tsx
- [X] T112 [P] [US2] Create student assignment list, submission form, resubmission history, feedback panel, late state display, and submitted evidence reference display in apps/admin-web/src/features/learning/assignments/StudentAssignmentList.tsx and apps/admin-web/src/features/learning/assignments/StudentSubmissionForm.tsx
- [X] T113 [US2] Implement school, student, and guardian assignment routes with create, submit, review, feedback visibility, history preservation, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/assignments/page.tsx, apps/admin-web/src/app/(student)/learning/assignments/page.tsx, and apps/admin-web/src/app/(guardian)/learning/assignments/page.tsx
- [X] T114 [P] [US2] Create mobile assignment models and repository methods for optional student and guardian assignment surfaces in apps/mobile/lib/features/learning/assignment_models.dart and apps/mobile/lib/features/learning/assignment_repository.dart
- [X] T115 [US2] Create assignment test data builder for active assignment, closed assignment, late assignment, excused submission, returned submission, graded submission, duplicate submission, conflicting submission, missing evidence, inactive student, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Assignments/AssignmentTestData.cs
- [X] T116 [US2] Create assignment contract fixtures for create, submit, review, duplicate, late, missing evidence, guardian visibility, and trace payloads in tests/contracts/learning/assignment-tracking-fixtures.md
- [X] T117 [US2] Implement AssignmentLearningOutcomePublisher for progress and star-eligible source events emitted after submission and review outcomes without awarding stars directly in apps/api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentLearningOutcomePublisher.cs
- [X] T118 [US2] Update LearningPhaseBoundaryGuard tests and implementation to assert assignment evidence references never create document storage, messaging, wallet, request, attendance, campus, transport, or search outcomes in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs

**Checkpoint**: User Story 2 can be demonstrated independently after T088-T118 pass.

---

## Phase 5: User Story 3 - Deliver Quizzes and Record Results (Priority: P1)

**Goal**: Teachers can create and activate quizzes with controlled attempts,
students can complete eligible attempts, and results preserve scoring,
feedback, timing, and review evidence.

**Independent Test**: Activate a quiz for an assigned group, complete an
attempt as an eligible student, and verify scoring, attempt limits, feedback
visibility, tenant isolation, and audit evidence.

### Tests for User Story 3

- [X] T119 [P] [US3] Create Quiz domain tests for active question revision, target learners, schedule, attempt limit, time limit, scoring policy, feedback visibility, suspend/retire/archive transitions, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizDomainTests.cs
- [X] T120 [P] [US3] Create QuizQuestion domain tests for question revision preservation, required questions, question type validation, points possible, display order, answer key reference, manual review requirement, and no mutation after attempts exist in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizQuestionDomainTests.cs
- [X] T121 [P] [US3] Create QuizAttempt domain tests for started, submitted, expired, scored, needs review, corrected, voided, attempt number, timer expiry, feedback release, idempotency key, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizAttemptDomainTests.cs
- [X] T122 [P] [US3] Create QuizConfigurationService unit tests for draft create, activate, suspend, retire, invalid schedule, invalid attempt limit, missing question set, disabled learning.quizzes denial, unassigned staff denial, duplicate create retry, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizConfigurationServiceTests.cs
- [X] T123 [P] [US3] Create QuizAttemptService unit tests for start, submit, duplicate start, duplicate submit, expired attempt, out-of-window attempt, over-limit attempt, ineligible student, missing required answer, interrupted attempt, cross-school denial, progress event update, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizAttemptServiceTests.cs
- [X] T124 [P] [US3] Create QuizScoringService unit tests for auto scoring, manual review, hybrid scoring, answer key revision preservation, score correction, feedback visibility, stale rule exception, score conflict exception, star source event handoff, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizScoringServiceTests.cs
- [X] T125 [P] [US3] Create contract tests for quiz create, activate, suspend, student quiz list, attempt start, attempt submit, review, and quiz trace routes from contracts/quiz-engine.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizEngineContractTests.cs
- [X] T126 [P] [US3] Create integration tests for tenant isolation, learning.quizzes disabled, missing permission, active quiz lifecycle, attempt limit, timer expiry, duplicate completion, required answers, feedback visibility, scoring correction, progress event update, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizEngineIntegrationTests.cs
- [X] T127 [P] [US3] Create web journey tests for teacher quiz activation, student attempt start/submit, score visibility, feedback release state, over-limit denial, expired attempt, disabled capability, access-denied state, and trace link display in apps/admin-web/tests/learning/quiz-engine.spec.ts
- [X] T128 [P] [US3] Create API performance test verifying eligible quiz results are visible within 1 minute of completion during review testing in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizEnginePerformanceTests.cs

### Implementation for User Story 3

- [X] T129 [P] [US3] Create quiz create, question, activation, suspension, student quiz, attempt start, attempt submit, response, scoring, feedback visibility, review, result, and trace DTOs matching contracts/quiz-engine.md in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizEngineDtos.cs
- [X] T130 [US3] Implement QuizConfigurationService for create, question revision management, activate, suspend, retire, schedule validation, attempt limit validation, scoring policy validation, feedback policy validation, target learner validation, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizConfigurationService.cs
- [X] T131 [US3] Implement QuizQuestionRevisionService for immutable active revisions, new revision creation, answer key correction, required question validation, manual review question handling, and historical attempt preservation in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizQuestionRevisionService.cs
- [X] T132 [US3] Implement QuizEligibilityService for active quiz, active student, assigned learner scope, valid attempt window, available attempt count, feedback visibility, guardian visibility, disabled feature, and cross-school denial checks in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizEligibilityService.cs
- [X] T133 [US3] Implement QuizAttemptService for start, submit, expire, void, attempt numbering, timer/window validation, duplicate attempt handling, response persistence, progress event update, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizAttemptService.cs
- [X] T134 [US3] Implement QuizScoringService for auto score, manual review route, hybrid score, missing answer behavior, score conflict detection, scoring correction, answer revision preservation, star source event handoff, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizScoringService.cs
- [X] T135 [US3] Implement QuizFeedbackVisibilityService for immediate, after close, teacher released, hidden, student view, guardian view, staff view, reviewer view, and staff-only note filtering in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizFeedbackVisibilityService.cs
- [X] T136 [US3] Implement QuizTraceService for quiz to questions, attempts, responses, scores, progress, stars, exceptions, reviews, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizTraceService.cs
- [X] T137 [US3] Implement QuizController, StudentQuizController, QuizAttemptController, QuizReviewController, and QuizTraceController routes from contracts/quiz-engine.md in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizControllers.cs
- [X] T138 [US3] Wire quiz create, activation, suspension, attempt start, attempt submit, attempt expire, quiz scoring, score correction, review route, trace read, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizAuditAdapter.cs
- [X] T139 [US3] Wire quiz completed, quiz scored, quiz needs review, quiz corrected, and quiz feedback released status events for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizStatusEventAdapter.cs
- [X] T140 [US3] Update Learning endpoint registration to map quiz create, activate, suspend, student quiz, attempt start, attempt submit, review, and trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T141 [US3] Create OpenAPI examples for quiz create, activation, suspension, attempt start, attempt submit, scored result, needs review, feedback hidden, duplicate attempt, expired attempt, over-limit attempt, disabled capability, access denial, and quiz trace in apps/api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizOpenApiExamples.cs
- [X] T142 [P] [US3] Create school and student quiz API hooks for list, create, activate, suspend, start, submit, review, feedback, trace, and typed denial errors in apps/admin-web/src/features/learning/quizzes/quizApi.ts
- [X] T143 [P] [US3] Create quiz editor, question set editor, activation panel, attempt limit display, scoring review panel, feedback visibility badge, and quiz trace panel in apps/admin-web/src/features/learning/quizzes/QuizEditor.tsx and apps/admin-web/src/features/learning/quizzes/QuizReviewPanel.tsx
- [X] T144 [P] [US3] Create student quiz list, attempt runner, timer display, response form, result panel, feedback hidden state, and over-limit/expired state displays in apps/admin-web/src/features/learning/quizzes/StudentQuizAttempt.tsx and apps/admin-web/src/features/learning/quizzes/StudentQuizResult.tsx
- [X] T145 [US3] Implement school and student quiz routes with create, activate, attempt, review, feedback visibility, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/quizzes/page.tsx and apps/admin-web/src/app/(student)/learning/quizzes/page.tsx
- [X] T146 [P] [US3] Create mobile quiz models and repository methods for optional student quiz surfaces in apps/mobile/lib/features/learning/quiz_models.dart and apps/mobile/lib/features/learning/quiz_repository.dart
- [X] T147 [US3] Create quiz test data builder for active quiz, suspended quiz, expired quiz, over-limit attempts, missing answers, auto score, manual review, hybrid scoring, hidden feedback, linked guardian, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Quizzes/QuizTestData.cs
- [X] T148 [US3] Create quiz contract fixtures for create, activate, attempt start, submit, score, correction, feedback visibility, duplicate, expired, over-limit, and trace payloads in tests/contracts/learning/quiz-engine-fixtures.md
- [X] T149 [US3] Update LearningPhaseBoundaryGuard tests and implementation to assert quiz attempts and results never create attendance, wallet, request approval, messaging, document, search, or broad dashboard outcomes in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs

**Checkpoint**: User Story 3 can be demonstrated independently after T119-T149 pass.

---

## Phase 6: User Story 4 - Manage Stars and Rewards (Priority: P1)

**Goal**: Authorized staff can configure star rules and rewards, qualifying
events can produce append-only star evidence, and eligible students can redeem
rewards without creating wallet or payment activity.

**Independent Test**: Configure a star rule, award stars from a qualifying
learning or behavior event, redeem a reward as an eligible student, and verify
the star ledger, balance, redemption status, reversal behavior, and evidence
available to later permission rules.

### Tests for User Story 4

- [X] T150 [P] [US4] Create StarRuleSetting domain tests for source type, eligible student scope, star amount, award cap, valid date range, review behavior, rule status, version capture, disabled feature denial, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRuleSettingDomainTests.cs
- [X] T151 [P] [US4] Create StarLedgerEntry domain tests for append-only credit, debit, reserve, consume, release, correct, expire, source reference, rule version, balance state, reason required, idempotency key, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarLedgerEntryDomainTests.cs
- [X] T152 [P] [US4] Create StarBalanceSnapshot tests for reconstructable balance, available stars, reserved stars, consumed stars, pending review stars, corrections, duplicate retries, ledger conflict detection, and snapshot-not-authority behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarBalanceSnapshotServiceTests.cs
- [X] T153 [P] [US4] Create RewardCatalogItem domain tests for star cost, eligibility scope, availability window, inventory limit, redemption limit, fulfillment policy, active/suspended/retired/expired status, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Rewards/RewardCatalogItemDomainTests.cs
- [X] T154 [P] [US4] Create RewardRedemption domain tests for requested, approved, fulfilled, cancelled, denied, released, needs review, star cost snapshot, ledger link, fulfillment state, duplicate request, review reason, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Rewards/RewardRedemptionDomainTests.cs
- [X] T155 [P] [US4] Create StarRuleService unit tests for draft create, activate, suspend, source capability validation, disabled learning.stars_rewards denial, duplicate rule retry, invalid award cap denial, versioned rule change, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRuleServiceTests.cs
- [X] T156 [P] [US4] Create StarLedgerService unit tests for award, reverse, reserve, consume, release, correct, expire, manual award, source evidence missing, duplicate source award, conflicting source award review route, insufficient stars review route, status event export, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarLedgerServiceTests.cs
- [X] T157 [P] [US4] Create RewardRedemptionService unit tests for sufficient stars, insufficient stars, unavailable reward, expired reward, inventory limit, redemption limit, duplicate redemption, cancellation release, fulfillment, guardian/staff restriction, wallet/payment boundary, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Rewards/RewardRedemptionServiceTests.cs
- [X] T158 [P] [US4] Create Phase6StarEvidenceExportService unit tests for available balance export, reserved balance export, corrected ledger export, stale snapshot handling, disabled capability denial, read-only behavior, no request approval mutation, and tenant filtering in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/Phase6StarEvidenceExportServiceTests.cs
- [X] T159 [P] [US4] Create contract tests for star rule create/activate, manual award, balance read, guardian stars read, reward create, reward redeem, reward fulfill/cancel, Phase 6 star evidence export, and star trace routes from contracts/star-reward-system.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRewardSystemContractTests.cs
- [X] T160 [P] [US4] Create integration tests for tenant isolation, disabled capability, missing permission, star award from content/assignment/quiz/behavior source, duplicate award idempotency, correction, reward redemption, star reservation, consumption, release, insufficient stars, wallet/payment boundary, Phase 6 export, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRewardSystemIntegrationTests.cs
- [X] T161 [P] [US4] Create web journey tests for star rule configuration, manual star award, student balance view, guardian balance view, reward catalog management, student reward redemption, fulfillment, insufficient-star state, wallet/payment boundary note, and trace link display in apps/admin-web/tests/learning/star-reward-system.spec.ts
- [X] T162 [P] [US4] Create API performance test verifying reward redemption with sufficient stars completes in under 1 minute and 95% of eligible star evidence changes are available to Phase 6 consumers within 2 minutes in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRewardPerformanceTests.cs

### Implementation for User Story 4

- [X] T163 [P] [US4] Create star rule, manual award, ledger entry, balance snapshot, reward catalog, reward redemption, fulfillment, guardian stars, Phase 6 evidence export, and trace DTOs matching contracts/star-reward-system.md in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardDtos.cs
- [X] T164 [US4] Implement StarRuleService for create, activate, suspend, retire, source capability validation, eligible scope validation, award cap validation, rule versioning, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRuleService.cs
- [X] T165 [US4] Implement StarSourceEventService for validating content, assignment, quiz, behavior, manual award, reward, Phase 6 reservation, expiry, source evidence availability, source uniqueness, source trace, and manual-review-required routing in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarSourceEventService.cs
- [X] T166 [US4] Implement StarLedgerService for append-only award, reverse, reserve, consume, release, correct, expire, manual award, duplicate retry, conflicting non-identical review route, ledger persistence, balance update trigger, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarLedgerService.cs
- [X] T167 [US4] Complete StarBalanceSnapshotService for available/reserved/consumed/pending totals, recalculation from ledger, snapshot update, stale snapshot detection, tenant filtering, and performance-safe read models in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarBalanceSnapshotService.cs
- [X] T168 [US4] Implement StarReservationLifecycleService for reward reservation, Phase 6-readable reservation evidence, consumption, denial release, withdrawal release, expiry release, duplicate retry, insufficient star route to review, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarReservationLifecycleService.cs
- [X] T169 [US4] Implement RewardCatalogService for create, update draft, activate, suspend, retire, availability window validation, eligibility scope validation, inventory limit validation, redemption limit validation, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Rewards/RewardCatalogService.cs
- [X] T170 [US4] Implement RewardRedemptionService for student redemption, staff redemption, eligibility validation, active reward validation, available star validation, duplicate redemption handling, reservation/consumption handoff, fulfillment policy, cancellation/release, no wallet/payment side effects, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Rewards/RewardRedemptionService.cs
- [X] T171 [US4] Implement Phase6StarEvidenceExportService for read-only available/reserved/latest ledger evidence export, freshness metadata, tenant filtering, permission checks, disabled capability denial, no request approval mutation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Stars/Phase6StarEvidenceExportService.cs
- [X] T172 [US4] Implement StarRewardTraceService for star ledger to source, balance, reward, redemption, behavior, assignment, quiz, Phase 6 evidence export, exceptions, reviews, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardTraceService.cs
- [X] T173 [US4] Implement StarController, GuardianStarsController, RewardController, RewardRedemptionController, RewardFulfillmentController, Phase6StarEvidenceController, and StarTraceController routes from contracts/star-reward-system.md in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardControllers.cs
- [X] T174 [US4] Wire star rule activation, manual award, ledger award, reversal, reservation, consumption, release, correction, reward create, reward redemption, reward fulfillment, Phase 6 export, trace read, wallet/payment boundary, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardAuditAdapter.cs
- [X] T175 [US4] Wire star changed, reward changed, reward fulfilled, star evidence exported, and star review-required status events for later Phase 9 notification eligibility and Phase 6 evidence readiness without delivery or approval side effects in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardStatusEventAdapter.cs
- [X] T176 [US4] Update Learning endpoint registration to map star rule, manual award, balance, guardian stars, reward catalog, reward redemption, fulfillment, Phase 6 evidence export, and star trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T177 [US4] Create OpenAPI examples for star rule create, activation, manual award, duplicate award, balance read, reward create, reward redeem, insufficient stars, unavailable reward, fulfillment, cancellation release, Phase 6 export, wallet/payment boundary, disabled capability, access denial, and star trace in apps/api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardOpenApiExamples.cs
- [X] T178 [P] [US4] Create school, student, and guardian star/reward API hooks for star rules, balance, ledger, manual award, reward catalog, redemption, fulfillment, Phase 6 evidence, trace, and typed denial errors in apps/admin-web/src/features/learning/stars/starRewardApi.ts
- [X] T179 [P] [US4] Create star rule editor, manual award form, star ledger table, balance panel, reward catalog editor, redemption queue, fulfillment panel, Phase 6 evidence panel, and star trace panel in apps/admin-web/src/features/learning/stars/StarRuleEditor.tsx and apps/admin-web/src/features/learning/stars/RewardRedemptionPanel.tsx
- [X] T180 [US4] Implement school, student, and guardian star/reward routes with rule management, balance display, reward catalog, redemption, fulfillment, Phase 6 evidence visibility, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/stars/page.tsx, apps/admin-web/src/app/(school)/learning/rewards/page.tsx, apps/admin-web/src/app/(student)/learning/page.tsx, and apps/admin-web/src/app/(guardian)/learning/page.tsx
- [X] T181 [P] [US4] Create mobile star and reward models and repository methods for optional student and guardian reward surfaces in apps/mobile/lib/features/learning/star_reward_models.dart and apps/mobile/lib/features/learning/star_reward_repository.dart
- [X] T182 [US4] Create star and reward test data builder for active star rule, suspended star rule, content source, assignment source, quiz source, behavior source, manual source, duplicate award, conflicting award, insufficient stars, active reward, unavailable reward, duplicate redemption, fulfilled redemption, cancelled redemption, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Stars/StarRewardTestData.cs
- [X] T183 [US4] Update LearningPhaseBoundaryGuard tests and implementation to assert stars and rewards never create wallet credits, payments, refunds, canteen purchases, request approvals, attendance, campus, transport, document, search, or messaging outcomes in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs

**Checkpoint**: User Story 4 can be demonstrated independently after T150-T183 pass.

---

## Phase 7: User Story 5 - Log Behavior and Engagement Events (Priority: P2)

**Goal**: Authorized staff can record behavior or engagement events, visibility
is filtered for students and guardians, corrections preserve original evidence,
and configured star impacts remain traceable.

**Independent Test**: Record a behavior event for an active student, verify
guardian and student visibility according to school rules, correct the event as
an authorized reviewer, and confirm the original event remains preserved.

### Tests for User Story 5

- [X] T184 [P] [US5] Create BehaviorCategory domain tests for classification, default severity, default visibility, optional star rule, draft/active/suspended/retired status, sensitive default restriction, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorCategoryDomainTests.cs
- [X] T185 [P] [US5] Create BehaviorEvent domain tests for positive/corrective/neutral classification, severity, source context, staff note, visibility policy, accepted/needs review/disputed/corrected/dismissed state, related star ledger link, correction history, idempotency key, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorEventDomainTests.cs
- [X] T186 [P] [US5] Create BehaviorCategoryService unit tests for category create, activation, sensitive visibility defaults, invalid star rule denial, disabled behavior logging denial, duplicate category retry, tenant mismatch, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorCategoryServiceTests.cs
- [X] T187 [P] [US5] Create BehaviorEventService unit tests for authorized staff record, unassigned staff denial, inactive student denial, invalid category denial, duplicate event retry, conflicting event review route, sensitive visibility filtering, dispute, correction, dismissal, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorEventServiceTests.cs
- [X] T188 [P] [US5] Create BehaviorStarImpactService unit tests for behavior category star effect, active star rule, disabled stars capability, missing source evidence, duplicate behavior star impact, corrected behavior reversal, review-required behavior, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorStarImpactServiceTests.cs
- [X] T189 [P] [US5] Create contract tests for behavior category create, behavior event create/list, student behavior list, guardian behavior list, behavior review, and behavior trace routes from contracts/behavior-logging.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorLoggingContractTests.cs
- [X] T190 [P] [US5] Create integration tests for tenant isolation, disabled behavior logging, missing permission, active student, staff authority, guardian visibility restriction, sensitive behavior hidden, duplicate event idempotency, correction preservation, star impact link, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorLoggingIntegrationTests.cs
- [X] T191 [P] [US5] Create web journey tests for behavior category management, behavior event create, guardian/student behavior view, sensitive visibility hidden, dispute/correction review, star impact trace, disabled capability, access-denied state, and trace link display in apps/admin-web/tests/learning/behavior-logging.spec.ts
- [X] T192 [P] [US5] Create API performance test verifying guardians can find allowed behavior records for linked students in under 30 seconds while staff-only details remain hidden in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorLoggingPerformanceTests.cs

### Implementation for User Story 5

- [X] T193 [P] [US5] Create behavior category, behavior event, behavior filter, student behavior, guardian behavior, behavior review, correction, visibility, star impact, and trace DTOs matching contracts/behavior-logging.md in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorLoggingDtos.cs
- [X] T194 [US5] Implement BehaviorCategoryService for create, activate, suspend, retire, sensitive default visibility, star rule reference validation, tenant checks, feature checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorCategoryService.cs
- [X] T195 [US5] Implement BehaviorEventService for event creation, active student validation, staff authority validation, category validation, visibility policy validation, duplicate event handling, conflicting event review route, exception creation, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorEventService.cs
- [X] T196 [US5] Implement BehaviorVisibilityService for student view, guardian view, staff view, reviewer view, sensitive category restriction, staff note filtering, corrected event projection, dismissed event projection, and cross-school hiding in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorVisibilityService.cs
- [X] T197 [US5] Implement BehaviorReviewService for dispute, correct, dismiss, resolve, reopen, correction reason validation, original evidence preservation, related star correction handoff, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorReviewService.cs
- [X] T198 [US5] Implement BehaviorStarImpactService for active behavior star rules, positive/corrective/neutral star effects, duplicate star impact handling, review-required behavior, correction reversal handoff, source trace, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorStarImpactService.cs
- [X] T199 [US5] Implement BehaviorTraceService for behavior event to category, student, star ledger, exceptions, reviews, corrections, status events, and audit references in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorTraceService.cs
- [X] T200 [US5] Implement BehaviorCategoryController, BehaviorEventController, StudentBehaviorController, GuardianBehaviorController, BehaviorReviewController, and BehaviorTraceController routes from contracts/behavior-logging.md in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorControllers.cs
- [X] T201 [US5] Wire behavior category create, behavior event create, behavior correction, behavior dispute, behavior dismissal, star impact, sensitive visibility denial, trace read, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorAuditAdapter.cs
- [X] T202 [US5] Wire behavior recorded, behavior corrected, behavior disputed, behavior dismissed, and behavior star impact status events for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorStatusEventAdapter.cs
- [X] T203 [US5] Update Learning endpoint registration to map behavior category, behavior event, student behavior, guardian behavior, behavior review, and behavior trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T204 [US5] Create OpenAPI examples for behavior category create, behavior event create, sensitive hidden view, student list, guardian list, review/correct, dispute, duplicate retry, star impact, disabled capability, access denial, and behavior trace in apps/api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorOpenApiExamples.cs
- [X] T205 [P] [US5] Create school, student, and guardian behavior API hooks for categories, events, reviews, visibility, trace, and typed denial errors in apps/admin-web/src/features/learning/behavior/behaviorApi.ts
- [X] T206 [P] [US5] Create behavior category editor, behavior event form, behavior event table, sensitive visibility badge, correction panel, dispute panel, star impact badge, and behavior trace panel in apps/admin-web/src/features/learning/behavior/BehaviorEventForm.tsx and apps/admin-web/src/features/learning/behavior/BehaviorReviewPanel.tsx
- [X] T207 [US5] Implement school, student, and guardian behavior routes with category management, event creation, review/correction, visibility filtering, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/behavior/page.tsx, apps/admin-web/src/app/(student)/learning/page.tsx, and apps/admin-web/src/app/(guardian)/learning/page.tsx
- [X] T208 [P] [US5] Create mobile behavior models and repository methods for optional student and guardian behavior surfaces in apps/mobile/lib/features/learning/behavior_models.dart and apps/mobile/lib/features/learning/behavior_repository.dart
- [X] T209 [US5] Create behavior test data builder for active category, sensitive category, star-impact category, positive event, corrective event, neutral event, disputed event, corrected event, duplicate event, conflicting event, inactive student, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Behavior/BehaviorTestData.cs
- [X] T210 [US5] Update LearningPhaseBoundaryGuard tests and implementation to assert behavior logging never creates medical, emergency, complaint, discipline case-management, request approval, wallet, attendance, transport, document, search, or messaging outcomes in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs

**Checkpoint**: User Story 5 can be demonstrated independently after T184-T210 pass.

---

## Phase 8: User Story 6 - Review Progress, Exceptions, and Learning Configuration (Priority: P3)

**Goal**: Authorized users can configure versioned learning rules, search
history, review exceptions, correct records with reason, and read summaries
without exposing restricted records.

**Independent Test**: Configure learning rules for a school account, search
progress and exception history for a student, correct an eligible learning
record with a reason, and verify all summary, historical, and audit evidence
remains tenant-scoped.

### Tests for User Story 6

- [X] T211 [P] [US6] Create LearningRuleSetting domain tests for rule area, rule payload, rule version, valid date range, change reason, draft/active/suspended/retired status, dependent capability validation, and tenant ownership in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Configuration/LearningRuleSettingDomainTests.cs
- [X] T212 [P] [US6] Create LearningException domain tests for source type, affected student, exception type, severity, open/assigned/resolved/dismissed/escalated/reopened status, reviewer assignment, resolution reason, source evidence reference, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Exceptions/LearningExceptionDomainTests.cs
- [X] T213 [P] [US6] Create ManualLearningReview domain tests for correct, reopen, close, resolve, dismiss, escalate, migrate rule version, review reason required, original status preservation, resulting status, reviewer authority, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/ManualLearningReviewDomainTests.cs
- [X] T214 [P] [US6] Create LearningReviewSummary tests for student, course, group, assignment, quiz, star, reward, behavior, reviewer scopes, guardian-safe projection, staff-only detail filtering, latest evidence, and tenant consistency in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningReviewSummaryTests.cs
- [X] T215 [P] [US6] Create LearningConfigurationService unit tests for feature settings read, rule draft create, rule update, activation, suspension, dependent capability disabled denial, impossible due window denial, invalid quiz attempt limit denial, invalid star/reward behavior denial, invalid visibility denial, version capture, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Configuration/LearningConfigurationServiceTests.cs
- [X] T216 [P] [US6] Create LearningHistoryService unit tests for filters by student, guardian, course, group, assignment status, quiz result, star outcome, reward status, behavior category, date range, actor, exception type, review state, pagination, tenant filtering, and staff-only detail hiding in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningHistoryServiceTests.cs
- [X] T217 [P] [US6] Create ManualLearningReviewService unit tests for correction, reopen, close, resolve, dismiss, escalate, migrate rule version, duplicate review retry, missing reason denial, unauthorized reviewer denial, original evidence preservation, status event export, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/ManualLearningReviewServiceTests.cs
- [X] T218 [P] [US6] Create contract tests for feature settings, rule settings create/list/read/update/activate/suspend/trace, history search, exception list/detail, manual review, review summaries, and lifecycle trace routes from contracts/learning-configuration.md and contracts/learning-history-review.md in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningConfigurationHistoryContractTests.cs
- [X] T219 [P] [US6] Create integration tests for tenant isolation, disabled history, disabled summaries, disabled configuration, missing permissions, guardian history filtering, student history filtering, manual correction, rule version preservation, summary reads, trace lifecycle, status event export, and audit events in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningHistoryReviewIntegrationTests.cs
- [X] T220 [P] [US6] Create web journey tests for learning configuration, rule activation, history filters, exception queue, manual correction, review summary, guardian restricted history, disabled capability, access-denied state, and trace link display in apps/admin-web/tests/learning/learning-history-review.spec.ts
- [X] T221 [P] [US6] Create API performance test verifying 90-day learning history searches complete in under 30 seconds and sampled lifecycle traces complete in under 60 seconds during review testing in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningHistoryReviewPerformanceTests.cs
- [X] T222 [P] [US6] Create audit and observability integration tests verifying required audit evidence for configuration changes, summary reads, access denials, manual reviews, exceptions, and lifecycle trace reads in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningAuditObservabilityIntegrationTests.cs

### Implementation for User Story 6

- [X] T223 [P] [US6] Create learning configuration, feature settings, rule setting, rule trace, history filter, history result, exception, manual review, review summary, lifecycle trace, and audit evidence DTOs matching contracts/learning-configuration.md and contracts/learning-history-review.md in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewDtos.cs
- [X] T224 [US6] Implement LearningConfigurationService for feature settings read, rule draft create, draft update, activation, suspension, dependent capability validation, rule payload validation, rule versioning, tenant checks, permission checks, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Configuration/LearningConfigurationService.cs
- [X] T225 [US6] Complete LearningRuleVersionService for current rule lookup, historical rule capture, activation version increment, stale rule detection, rule migration request validation, and source record version preservation in apps/api/src/SafeSchool.Api/Features/Learning/Configuration/LearningRuleVersionService.cs
- [X] T226 [US6] Implement LearningFeatureSettingsQueryService for effective Phase 5 capability reads, tenant filtering, stale setting handling, disabled feature typed errors, and non-authorizing read behavior in apps/api/src/SafeSchool.Api/Features/Learning/Configuration/LearningFeatureSettingsQueryService.cs
- [X] T227 [US6] Complete LearningExceptionFactory for invalid enrollment, inactive student, missing evidence, late submission, duplicate submission, duplicate quiz attempt, scoring conflict, stale rule, missing source evidence, duplicate star award, insufficient stars, unavailable reward, invalid behavior record, disabled feature, cross-school access, and manual-review-required creation in apps/api/src/SafeSchool.Api/Features/Learning/Exceptions/LearningExceptionFactory.cs
- [X] T228 [US6] Implement LearningExceptionQueryService for exception list/detail filters by student, source, type, severity, status, reviewer assignment, date range, tenant scope, reviewer visibility, and pagination in apps/api/src/SafeSchool.Api/Features/Learning/Exceptions/LearningExceptionQueryService.cs
- [X] T229 [US6] Implement ManualLearningReviewService for correct, reopen, close, resolve, dismiss, escalate, migrate rule version, required reason validation, reviewer permission, duplicate retry, source record preservation, status event export, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/ManualLearningReviewService.cs
- [X] T230 [US6] Implement LearningHistoryService for school, student, and guardian history filters across content, progress, assignments, quizzes, stars, rewards, behavior, exceptions, reviews, date ranges, actors, pagination, tenant filtering, and privacy-safe projection in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningHistoryService.cs
- [X] T231 [US6] Implement LearningReviewSummaryService for student, course, group, assignment, quiz, star, reward, behavior, reviewer summaries, visibility-scoped projections, summary refresh, latest evidence, and staff-only detail filtering in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewSummaryService.cs
- [X] T232 [US6] Implement LearningLifecycleTraceService for source records to progress, submissions, quiz attempts, stars, rewards, behavior, exceptions, reviews, rule versions, status events, and audit evidence in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningLifecycleTraceService.cs
- [X] T233 [US6] Implement LearningConfigurationController, LearningHistoryController, StudentHistoryController, GuardianHistoryController, LearningExceptionController, ManualLearningReviewController, LearningReviewSummaryController, and LearningTraceController routes from contracts/learning-configuration.md and contracts/learning-history-review.md in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewControllers.cs
- [X] T234 [US6] Wire configuration change, rule activation, rule suspension, history read, summary read, exception creation, manual review, correction, reopen, dismissal, escalation, trace read, disabled feature, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewAuditAdapter.cs
- [X] T235 [US6] Wire exception opened, review completed, summary refreshed, configuration changed, and trace-read status events for later Phase 9 notification eligibility without delivery side effects in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewStatusEventAdapter.cs
- [X] T236 [US6] Update Learning endpoint registration to map feature settings, rule settings, rule trace, history, student history, guardian history, exceptions, manual reviews, review summaries, and lifecycle trace routes in apps/api/src/SafeSchool.Api/Features/Learning/LearningEndpointRegistration.cs
- [X] T237 [US6] Create OpenAPI examples for feature settings, rule setting create/update/activate/suspend, invalid dependent feature, history search, guardian history filtering, exception list/detail, manual correction, rule migration, summary read, lifecycle trace, disabled capability, access denial, and audit failure in apps/api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewOpenApiExamples.cs
- [X] T238 [P] [US6] Create learning history, configuration, review, and summary API hooks for feature settings, rule settings, history filters, exception queue, manual review, summaries, lifecycle trace, and typed denial errors in apps/admin-web/src/features/learning/reviews/learningReviewApi.ts
- [X] T239 [P] [US6] Create rule setting editor, feature setting panel, history filter panel, history result table, exception queue, manual review panel, review summary panel, lifecycle trace panel, and restricted-detail display in apps/admin-web/src/features/learning/reviews/LearningHistoryPanel.tsx and apps/admin-web/src/features/learning/reviews/LearningReviewPanel.tsx
- [X] T240 [US6] Implement school history, review, and configuration routes with filters, exception queue, manual correction, summary read, lifecycle trace, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/learning/history/page.tsx, apps/admin-web/src/app/(school)/learning/review/page.tsx, and apps/admin-web/src/app/(school)/learning/configuration/page.tsx
- [X] T241 [US6] Implement student and guardian history route sections with privacy-safe filters, linked-student scope, restricted staff-only detail hiding, disabled capability state, and access-denied state in apps/admin-web/src/app/(student)/learning/page.tsx and apps/admin-web/src/app/(guardian)/learning/page.tsx
- [X] T242 [P] [US6] Create mobile history and summary models and repository methods for optional student and guardian history surfaces in apps/mobile/lib/features/learning/history_models.dart and apps/mobile/lib/features/learning/history_repository.dart
- [X] T243 [US6] Create learning history and review test data builder for rule settings, active/inactive capabilities, content lifecycle, assignment lifecycle, quiz lifecycle, star lifecycle, reward lifecycle, behavior lifecycle, exceptions, reviews, summaries, guardian restricted records, disabled capability, and cross-school cases in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/LearningReviewTestData.cs
- [X] T244 [US6] Update source workflow services to capture active rule versions on content, assignment, quiz, star, reward, behavior, correction, and review records in apps/api/src/SafeSchool.Api/Features/Learning/Configuration/LearningRuleVersionService.cs

**Checkpoint**: User Story 6 can be demonstrated independently after T211-T244 pass.

---

## Phase 9: Polish and Cross-Cutting Verification

**Purpose**: Validate the whole Phase 5 slice, tighten boundaries, update
operator documentation, and ensure every story remains independently testable.

- [X] T245 [P] Run backend unit, integration, contract, authorization, tenant-isolation, migration, performance, and audit tests for Learning and record fixes in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/
- [X] T246 [P] Run admin web lint, typecheck, and Learning journey tests and record fixes in apps/admin-web/tests/learning/
- [X] T247 [P] Run optional mobile Learning tests and record fixes in apps/mobile/test/features/learning/
- [X] T248 Review generated OpenAPI examples and contract fixtures for all seven Learning contracts in tests/contracts/learning/ and apps/api/src/SafeSchool.Api/Features/Learning/
- [X] T249 Review tenant, feature gate, permission, guardian visibility, student self-scope, staff assignment, reviewer, reward manager, behavior reviewer, and platform reviewer coverage across all Learning services in apps/api/src/SafeSchool.Api/Features/Learning/
- [X] T250 Review Phase 5 no-side-effect boundaries for attendance, campus, scan, transport, wallet, request approval, medical, complaint, messaging, document, search, and dashboard outcomes in apps/api/src/SafeSchool.Api/Features/Learning/Common/Boundaries/LearningPhaseBoundaryGuard.cs
- [X] T251 Review audit and status event coverage for all required course, content, assignment, quiz, star, reward, behavior, exception, review, summary, configuration, access denial, Phase 6 evidence, and Phase 9 notification eligibility events in apps/api/src/SafeSchool.Api/Features/Learning/Audit/
- [X] T252 Review accessibility, loading, empty, disabled-capability, access-denied, sensitive-detail-hidden, duplicate-retry, and trace-link states for all Learning web routes in apps/admin-web/src/app/(school)/learning/, apps/admin-web/src/app/(student)/learning/, and apps/admin-web/src/app/(guardian)/learning/
- [X] T253 Validate [quickstart.md](./quickstart.md) end-to-end and document reviewer notes in docs/learning/README.md
- [X] T254 Update implementation documentation with Phase 5 capability keys, permission names, endpoint groups, rule version behavior, star ledger invariants, reward wallet boundary, behavior privacy rules, Phase 6 evidence export, Phase 9 status event boundary, and troubleshooting notes in docs/learning/README.md
- [X] T255 [P] Create guardian aggregate learning overview performance test verifying linked guardians can find allowed progress, assignment outcomes, quiz outcomes, star/reward state, and behavior records in under 30 seconds while staff-only details remain hidden in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Reviews/GuardianLearningOverviewPerformanceTests.cs
- [X] T256 [P] Create LearningStatusEvent export latency integration test verifying eligible content, progress, assignment, quiz, star, reward, behavior, exception, review, and configuration status changes are available to later notification consumers within 2 minutes without delivery side effects in apps/api/tests/SafeSchool.Api.Tests/Features/Learning/Audit/LearningStatusEventLatencyIntegrationTests.cs
- [X] T257 Run repository formatting, linting, type checking, migration validation, and contract fixture validation for all Learning changes in apps/api/, apps/admin-web/, apps/mobile/, and tests/contracts/learning/

---

## Dependencies and Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1 and blocks every user story.
- **Phase 3 US1 Content**: Depends on Phase 2.
- **Phase 4 US2 Assignments**: Depends on Phase 2 and can be implemented after US1 or in parallel with care. Assignment progress integration uses the shared progress foundation.
- **Phase 5 US3 Quizzes**: Depends on Phase 2 and can be implemented after US1 or in parallel with care. Quiz progress integration uses the shared progress foundation.
- **Phase 6 US4 Stars and Rewards**: Depends on Phase 2. Star source event handoffs from content, assignments, quizzes, and behavior can be completed independently using source adapters, then wired when those stories exist.
- **Phase 7 US5 Behavior**: Depends on Phase 2 and can be implemented after US4 when star impacts are needed, or independently with star-impact tests disabled until US4 exists.
- **Phase 8 US6 History, Review, Configuration**: Depends on Phase 2 and becomes complete after the source stories it summarizes are available.
- **Phase 9 Polish**: Depends on all desired user stories.

### User Story Dependencies

- **US1 (P1)**: MVP content and progress workflow. No dependency on other user stories after foundation.
- **US2 (P1)**: Independent assignment workflow after foundation; may publish progress and star-eligible source events.
- **US3 (P1)**: Independent quiz workflow after foundation; may publish progress and star-eligible source events.
- **US4 (P1)**: Star and reward workflow after foundation; consumes source event adapters and exports read-only evidence to later Phase 6.
- **US5 (P2)**: Behavior logging after foundation; optional star impact depends on US4 star services.
- **US6 (P3)**: Configuration, history, exceptions, reviews, summaries, and lifecycle traces after foundation; summary depth increases as source stories complete.

### Within Each User Story

- Tests required by the constitution MUST be written and fail before implementation.
- Domain/entity tests before services.
- Services before controllers/routes.
- Tenant, feature flag, permission, visibility, idempotency, exception, and audit enforcement before UI exposure.
- API contracts and integration tests before or alongside web/mobile surfaces.
- Story checkpoint must pass before treating that story as complete.

### Parallel Opportunities

- Tasks marked `[P]` can run in parallel when their prerequisite phase is ready.
- Setup manifest, route placeholder, documentation, and ignore tasks can run in parallel.
- Foundational tests, web clients, mobile client shells, and fixture docs can run in parallel after core interfaces are agreed.
- Tests inside each user story can run in parallel.
- Web, mobile, OpenAPI examples, and contract fixtures can run in parallel after backend DTO shapes are stable.
- Different user stories can be staffed in parallel after Phase 2, but shared files such as LearningEndpointRegistration.cs and LearningPhaseBoundaryGuard.cs require coordination.

---

## Implementation Strategy

### MVP First

1. Complete Phase 1 and Phase 2.
2. Complete Phase 3 (US1 content and progress).
3. Validate US1 independently with contract, integration, authorization,
   tenant-isolation, audit, performance, and web journey coverage.

### Priority Delivery

1. Add US2 assignments and verify submission/review history independently.
2. Add US3 quizzes and verify attempt/scoring integrity independently.
3. Add US4 stars/rewards and verify ledger, balance, reward, and Phase 6 export
   integrity independently.
4. Add US5 behavior logging and verify visibility/correction/star-impact
   behavior independently.
5. Add US6 history, configuration, exceptions, reviews, summaries, and lifecycle
   trace after the source workflows exist.

### Safety Rules

- Never mutate star balances directly outside append-only ledger entries.
- Never overwrite assignment submissions, quiz attempts, behavior events, or
  correction history; append review evidence instead.
- Never expose cross-school existence through errors, search, trace, or summary
  responses.
- Never allow UI-only feature gates; backend feature enforcement is required on
  every mutation and read that depends on a capability.
- Never let Phase 5 trigger out-of-scope side effects. It may only export
  eligible status/evidence for later phases.
