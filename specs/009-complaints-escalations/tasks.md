# Tasks: Phase 8 Complaints & Escalations

**Input**: Design documents from `/specs/009-complaints-escalations/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Include tests required by the constitution and Phase 8 plan. Business logic requires unit tests; API contracts, tenant/feature authorization, migrations, audit, status events, and critical user journeys require integration or contract coverage.

**Organization**: Tasks are grouped by user story so each story can be implemented, tested, and reviewed independently after the shared foundation is complete.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label from spec.md. Setup, Foundational, and Polish tasks do not use story labels.
- Every task includes exact file paths. If a path does not exist yet, create it as part of that task.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the module directories, skeleton files, and test locations that all later tasks will use.

- [ ] T001 Create the Complaints backend directory tree from the implementation plan in `apps/api/src/SafeSchool.Api/Features/Complaints/`
- [ ] T002 [P] Create the Complaints backend test directory tree in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/`
- [ ] T003 [P] Create the cross-service contract test directory tree in `tests/contracts/complaints/`
- [ ] T004 [P] Create the Complaints end-to-end test directory tree in `tests/e2e/complaints/`
- [ ] T005 [P] Create the Complaints web feature directory tree in `apps/admin-web/src/features/complaints/`
- [ ] T006 [P] Create the school, guardian, and student web route directories in `apps/admin-web/src/app/(school)/complaints/`, `apps/admin-web/src/app/(guardian)/complaints/`, and `apps/admin-web/src/app/(student)/complaints/`
- [ ] T007 [P] Create the Complaints web test directory tree in `apps/admin-web/tests/complaints/`
- [ ] T008 [P] If `apps/mobile/` exists or mobile complaint surfaces are enabled, create the Complaints mobile feature and test directory trees in `apps/mobile/lib/features/complaints/` and `apps/mobile/test/features/complaints/`
- [ ] T009 Create the Complaints backend module registration skeleton in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T010 [P] Create the Complaints module implementation notes for future LLMs in `apps/api/src/SafeSchool.Api/Features/Complaints/README.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Implement shared models, guards, persistence, audit, and contract scaffolding that every user story depends on.

**Critical**: No user story implementation should begin until this phase is complete.

- [ ] T011 Create complaint capability constants for `complaints.submission`, `complaints.categorization`, `complaints.assignment`, `complaints.escalation`, `complaints.feedback_resolution`, `complaints.history`, `complaints.configuration`, and `complaints.review_summaries` in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintCapabilities.cs`
- [ ] T012 Create complaint permission constants for all Phase 8 roles and actions in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintPermissions.cs`
- [ ] T013 Create complaint enum definitions for statuses, priorities, confidentiality levels, participant roles, owner types, entry types, escalation triggers, feedback states, exception types, and review actions in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintEnums.cs`
- [ ] T014 Create stable complaint error codes and validation messages in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintErrors.cs`
- [ ] T015 Create audit event names for all events listed in spec.md FR-028 in `apps/api/src/SafeSchool.Api/Features/Complaints/Audit/ComplaintAuditEvents.cs`
- [ ] T016 Create a tenant, capability, and school-account guard for Complaints workflows in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintTenantFeatureGuard.cs`
- [ ] T017 Create a role, permission, guardian-link, student self-scope, assignment, and reviewer authorization guard in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintAuthorizationGuard.cs`
- [ ] T018 Create a complaint idempotency helper for `client_request_id` mutation commands in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintIdempotencyService.cs`
- [ ] T019 Create a complaint status transition policy for submitted, received, triage, assigned, in review, waiting for information, escalated, resolved, closed, withdrawn, reopened, dismissed, and manual-review-required states in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintStatusTransitionPolicy.cs`
- [ ] T020 Create a complaint visibility policy that separates internal, restricted, reviewer-only, status-only, and complainant-visible details in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintVisibilityPolicy.cs`
- [ ] T021 [P] Create the Complaint entity with all data-model fields and tenant timestamps in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/Complaint.cs`
- [ ] T022 [P] Create the ComplaintCategory entity with versioning, required fields, owner group, target timings, feedback, and reopen fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintCategory.cs`
- [ ] T023 [P] Create the ComplaintParticipant entity with participant role, visibility scope, conflict state, and optional actor, student, guardian, staff, or school-unit references in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintParticipant.cs`
- [ ] T024 [P] Create the ComplaintAssignment entity with owner, prior owner, reason, status, and timing fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintAssignment.cs`
- [ ] T025 [P] Create the ComplaintInvestigationEntry entity with entry type, visibility, status-before/status-after, reason, and timing fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintInvestigationEntry.cs`
- [ ] T026 [P] Create the ComplaintEvidenceReference entity that stores source module, source record reference, summary, visibility, and reference status in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintEvidenceReference.cs`
- [ ] T027 [P] Create the EscalationRule and EscalationEvent entities with trigger, owner, timing, priority, visibility, and version fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/EscalationRule.cs` and `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/EscalationEvent.cs`
- [ ] T028 [P] Create the ResolutionRecord and ComplaintFeedback entities with outcome, visibility, eligibility, feedback, reopen, and status fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ResolutionRecord.cs` and `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintFeedback.cs`
- [ ] T029 [P] Create the ComplaintException and ManualComplaintReview entities with source evidence, review action, status-before/status-after, reviewer, and reason fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintException.cs` and `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ManualComplaintReview.cs`
- [ ] T030 [P] Create the ComplaintReviewSummary, ComplaintStatusEvent, and ComplaintFeatureSetting entities in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintReviewSummary.cs`, `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintStatusEvent.cs`, and `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Entities/ComplaintFeatureSetting.cs`
- [ ] T031 Create EF Core DbContext extension and DbSet registration for all Phase 8 entities in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/ComplaintsDbContextExtensions.cs`
- [ ] T032 [P] Create EF Core mappings and tenant indexes for Complaint, ComplaintCategory, ComplaintParticipant, and ComplaintAssignment in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Configurations/ComplaintCoreEntityConfigurations.cs`
- [ ] T033 [P] Create EF Core mappings and tenant indexes for ComplaintInvestigationEntry, ComplaintEvidenceReference, EscalationRule, EscalationEvent, ResolutionRecord, and ComplaintFeedback in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Configurations/ComplaintWorkflowEntityConfigurations.cs`
- [ ] T034 [P] Create EF Core mappings and tenant indexes for ComplaintException, ManualComplaintReview, ComplaintReviewSummary, ComplaintStatusEvent, and ComplaintFeatureSetting in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Configurations/ComplaintReviewEntityConfigurations.cs`
- [ ] T035 Create the Phase 8 EF Core migration for all complaint tables, foreign keys, tenant indexes, duplicate indexes, status indexes, owner indexes, and status-event indexes in `apps/api/src/SafeSchool.Api/Features/Complaints/Data/Migrations/202605060001_AddComplaintsEscalations.cs`
- [ ] T036 [P] Create seed data for complaint capabilities, permissions, baseline categories, and owner queues in `apps/api/src/SafeSchool.Api/Features/Complaints/Seed/ComplaintSeedData.cs`
- [ ] T037 [P] Create shared DTOs for pagination, list filters, actor context, visibility summaries, and error responses in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintCommonDtos.cs`
- [ ] T038 [P] Create the complaint status event writer and notification-boundary minimization logic in `apps/api/src/SafeSchool.Api/Features/Complaints/Common/ComplaintStatusEventWriter.cs`
- [ ] T039 [P] Create the complaint audit writer that records actor, tenant, student, complaint, action, reason, viewed category, and result in `apps/api/src/SafeSchool.Api/Features/Complaints/Audit/ComplaintAuditWriter.cs`
- [ ] T040 Create shared backend test fixtures for tenants, users, guardian links, students, roles, permissions, feature flags, audit capture, and idempotency in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintTestFixture.cs`
- [ ] T041 [P] Create contract test fixture utilities for Phase 8 route authentication, tenant headers, feature flags, and response assertions in `tests/contracts/complaints/ComplaintContractTestFixture.cs`
- [ ] T042 [P] Create web test fixtures for school, guardian, and student complaint users in `apps/admin-web/tests/complaints/complaintTestFixtures.ts`
- [ ] T043 [P] If `apps/mobile/` exists or mobile complaint surfaces are enabled, create mobile test fixtures for authenticated guardian and student complaint flows in `apps/mobile/test/features/complaints/complaint_test_fixtures.dart`
- [ ] T044 Create foundational tests for tenant isolation, feature-flag denial, permission denial, audit emission, and no-side-effect boundaries in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintFoundationTests.cs`
- [ ] T045 Create migration tests that verify all Phase 8 tables include `tenant_id`, `created_at`, `updated_at`, required indexes, and foreign keys in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintMigrationTests.cs`

**Checkpoint**: Foundation ready. User story phases may now proceed independently or in parallel.

---

## Phase 3: User Story 1 - Submit and Track Complaints (Priority: P1) MVP

**Goal**: Guardians, students, and staff can submit structured complaints and track allowed complaint status without exposing unrelated or restricted records.

**Independent Test**: Submit a complaint for an active student in one school account, verify required fields and complainant eligibility, then confirm the complainant and authorized complaint staff can see the correct status while cross-school and unrelated users cannot.

### Tests for User Story 1

- [ ] T046 [P] [US1] Create unit tests for complaint submission required fields, authenticated intake, guardian link eligibility, student self-scope, staff scope, multi-participant eligibility and visibility, and disabled feature outcomes in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Submission/ComplaintSubmissionValidatorTests.cs`
- [ ] T047 [P] [US1] Create unit tests for exact duplicate complaint detection and overlapping non-identical complaint review routing in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Submission/ComplaintDuplicateDetectionTests.cs`
- [ ] T048 [P] [US1] Create contract tests for complaint-submission.md school, guardian, and student endpoints in `tests/contracts/complaints/ComplaintSubmissionContractTests.cs`
- [ ] T049 [P] [US1] Create integration tests for staff, guardian, and student submission with tenant isolation, feature flags, permissions, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Submission/ComplaintSubmissionIntegrationTests.cs`
- [ ] T050 [P] [US1] Create web journey tests for guardian, student, and staff complaint submission and tracking in `apps/admin-web/tests/complaints/complaintSubmission.spec.ts`
- [ ] T051 [P] [US1] If `apps/mobile/` exists or mobile complaint surfaces are enabled, create mobile journey tests for guardian and student complaint submission and tracking in `apps/mobile/test/features/complaints/complaint_submission_flow_test.dart`

### Implementation for User Story 1

- [ ] T052 [P] [US1] Create submission request and response DTOs matching complaint-submission.md with primary student and additional participant collection fields in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintSubmissionDtos.cs`
- [ ] T053 [P] [US1] Create complaint read and tracking DTOs that hide restricted internal details from complainants in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintTrackingDtos.cs`
- [ ] T054 [US1] Implement complaint submission validation for required fields, active category, actor role, primary and additional participants, active student, guardian link, student self-scope, staff scope, feature flag, and tenant scope in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintSubmissionValidator.cs`
- [ ] T055 [US1] Implement tracking reference generation using a stable school-scoped format such as `CMP-YYYY-NNNN` in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintTrackingReferenceService.cs`
- [ ] T056 [US1] Implement exact duplicate and overlapping non-identical complaint detection using complainant, student, category, normalized description, and event window in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintDuplicateDetectionService.cs`
- [ ] T057 [US1] Implement evidence reference validation that checks source-module read permission and stores only permitted summaries in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintEvidenceReferenceService.cs`
- [ ] T058 [US1] Implement complaint submission orchestration with idempotency, primary and additional participant creation, status initialization, target timing, audit, and status event creation in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintSubmissionService.cs`
- [ ] T059 [US1] Implement complaint tracking reads with multi-participant visibility filtering for staff, guardians, students, complainants, assigned users, reviewers, and platform reviewers in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintTrackingService.cs`
- [ ] T060 [US1] Implement withdrawal rules for eligible non-final complaints with reason, audit, and status event evidence in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintWithdrawalService.cs`
- [ ] T061 [US1] Implement school staff complaint submission, listing, detail, and withdrawal endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/SchoolComplaintSubmissionController.cs`
- [ ] T062 [US1] Implement guardian complaint submission, listing, detail, and feedback-safe tracking endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/GuardianComplaintSubmissionController.cs`
- [ ] T063 [US1] Implement student complaint submission, listing, detail, and tracking endpoints where student submission is enabled in `apps/api/src/SafeSchool.Api/Features/Complaints/Submission/StudentComplaintSubmissionController.cs`
- [ ] T064 [US1] Add complaint submission routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T065 [US1] Implement typed web API methods for staff, guardian, and student complaint submission and tracking in `apps/admin-web/src/features/complaints/api/complaintsApi.ts`
- [ ] T066 [US1] Implement shared complaint submission form components with required field, category, event window, evidence reference, and requested outcome inputs in `apps/admin-web/src/features/complaints/components/ComplaintSubmissionForm.tsx`
- [ ] T067 [US1] Implement shared complaint status and tracking components that hide restricted internal details in `apps/admin-web/src/features/complaints/components/ComplaintTrackingView.tsx`
- [ ] T068 [US1] Implement guardian complaint routes for list, submit, and detail tracking in `apps/admin-web/src/app/(guardian)/complaints/page.tsx`, `apps/admin-web/src/app/(guardian)/complaints/new/page.tsx`, and `apps/admin-web/src/app/(guardian)/complaints/[complaintId]/page.tsx`
- [ ] T069 [US1] Implement student complaint routes for list, submit, and detail tracking in `apps/admin-web/src/app/(student)/complaints/page.tsx`, `apps/admin-web/src/app/(student)/complaints/new/page.tsx`, and `apps/admin-web/src/app/(student)/complaints/[complaintId]/page.tsx`
- [ ] T070 [US1] Implement staff complaint submission and complaint tracking routes in `apps/admin-web/src/app/(school)/complaints/page.tsx`, `apps/admin-web/src/app/(school)/complaints/new/page.tsx`, and `apps/admin-web/src/app/(school)/complaints/[complaintId]/page.tsx`
- [ ] T071 [US1] If `apps/mobile/` exists or mobile complaint surfaces are enabled, implement mobile complaint submission and tracking screen for guardian and student users in `apps/mobile/lib/features/complaints/complaint_submission_screen.dart`

**Checkpoint**: US1 is complete when complaint submission, tracking, duplicate handling, tenant isolation, feature gating, visibility filtering, audit, and no-side-effect assertions pass independently.

---

## Phase 4: User Story 2 - Categorize, Prioritize, and Assign Complaints (Priority: P1)

**Goal**: Complaint managers can classify complaints, apply category rules, set priority and confidentiality, assign owners, and block conflicted assignments.

**Independent Test**: Submit a complaint, categorize it with an active category, assign an owner, and verify category rules determine required fields, priority, confidentiality, target timing, and escalation route while conflicted or invalid assignments route to review.

### Tests for User Story 2

- [ ] T072 [P] [US2] Create unit tests for category rule evaluation, required fields, default priority, default confidentiality, target timing, and disabled category handling in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Categorization/ComplaintCategoryRuleEvaluatorTests.cs`
- [ ] T073 [P] [US2] Create unit tests for assignment routing, owner group validation, reassignment history, and conflict-of-interest restrictions in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Categorization/ComplaintAssignmentServiceTests.cs`
- [ ] T074 [P] [US2] Create contract tests for complaint-categorization.md category, triage, assignment, queue, and assignment-history endpoints in `tests/contracts/complaints/ComplaintCategorizationContractTests.cs`
- [ ] T075 [P] [US2] Create integration tests for categorization, reclassification, assignment, conflict detection, manual review fallback, audit, and status event creation in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Categorization/ComplaintCategorizationIntegrationTests.cs`
- [ ] T076 [P] [US2] Create web journey tests for complaint manager triage, category selection, priority, confidentiality, owner assignment, and assignment queue filtering in `apps/admin-web/tests/complaints/complaintCategorization.spec.ts`

### Implementation for User Story 2

- [ ] T077 [P] [US2] Create categorization, assignment, queue, and assignment-history DTOs matching complaint-categorization.md in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintCategorizationDtos.cs`
- [ ] T078 [US2] Implement category rule evaluation for submitter roles, required fields, default priority, confidentiality, owner group, target timing, escalation route, feedback behavior, and category version in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintCategoryRuleEvaluator.cs`
- [ ] T079 [US2] Implement conflict-of-interest detection using complaint subjects, involved parties, assigned actors, owner groups, and reviewer exceptions in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintConflictOfInterestService.cs`
- [ ] T080 [US2] Implement complaint categorization and reclassification with reason capture, previous-state preservation, audit, and status event creation in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintCategorizationService.cs`
- [ ] T081 [US2] Implement complaint assignment and reassignment with owner validation, prior owner history, target timing, conflict checks, audit, and manual-review fallback in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintAssignmentService.cs`
- [ ] T082 [US2] Implement assignment queue query filtering by actor, role, queue, priority, confidentiality, target timing, and tenant scope in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintAssignmentQueueService.cs`
- [ ] T083 [US2] Implement categorization, assignment, queue, and assignment-history endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/Categorization/ComplaintCategorizationController.cs`
- [ ] T084 [US2] Add categorization and assignment routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T085 [US2] Implement typed web API methods for categories, triage, assignment, and queues in `apps/admin-web/src/features/complaints/api/complaintCategorizationApi.ts`
- [ ] T086 [US2] Implement complaint triage panel with category, priority, confidentiality, reason, and owner assignment controls in `apps/admin-web/src/features/complaints/components/ComplaintTriagePanel.tsx`
- [ ] T087 [US2] Implement complaint assignment queue table with owner, target timing, priority, confidentiality, and conflict indicators in `apps/admin-web/src/features/complaints/components/ComplaintAssignmentQueue.tsx`
- [ ] T088 [US2] Implement school complaint triage and assignment pages in `apps/admin-web/src/app/(school)/complaints/triage/page.tsx` and `apps/admin-web/src/app/(school)/complaints/assigned/page.tsx`

**Checkpoint**: US2 is complete when category-driven triage, assignment, reassignment, conflict blocking, queues, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 5: User Story 3 - Investigate, Respond, and Resolve Complaints (Priority: P1)

**Goal**: Assigned investigators and resolvers can add investigation entries, request information, provide complainant-visible responses, and close complaints with resolution evidence.

**Independent Test**: Assign a complaint to a resolver, add internal and visible entries, request more information, record a resolution, and verify the complainant sees only the allowed outcome summary.

### Tests for User Story 3

- [ ] T089 [P] [US3] Create unit tests for investigation entry validation, allowed status transitions, visibility levels, reason requirements, and append-only evidence in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Resolutions/ComplaintInvestigationEntryTests.cs`
- [ ] T090 [P] [US3] Create unit tests for resolution validation, closure reason requirements, outcome visibility, feedback eligibility, and restricted detail minimization in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Resolutions/ComplaintResolutionServiceTests.cs`
- [ ] T091 [P] [US3] Create contract tests for the investigation entry, timeline, resolution, and review portions of feedback-resolution.md in `tests/contracts/complaints/ComplaintResolutionContractTests.cs`
- [ ] T092 [P] [US3] Create integration tests for investigation timeline, information request, visible response, resolution closure, restricted visibility, audit, and status events in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Resolutions/ComplaintResolutionIntegrationTests.cs`
- [ ] T093 [P] [US3] Create web journey tests for resolver timeline, internal notes, visible responses, requested information, and closure in `apps/admin-web/tests/complaints/complaintResolution.spec.ts`

### Implementation for User Story 3

- [ ] T094 [P] [US3] Create investigation entry, timeline, resolution, and closure DTOs matching feedback-resolution.md in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintResolutionDtos.cs`
- [ ] T095 [US3] Implement investigation entry service for internal notes, visible responses, information requests, complainant responses, findings, corrective actions, and status changes in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintInvestigationEntryService.cs`
- [ ] T096 [US3] Implement complaint timeline service that returns permission-scoped entries, participants, assignments, escalations, resolutions, feedback, reviews, and audit-safe summaries in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintTimelineService.cs`
- [ ] T097 [US3] Implement resolution service with closure reason, outcome summary, internal notes, corrective action, feedback eligibility, reopen eligibility, audit, and status event creation in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintResolutionService.cs`
- [ ] T098 [US3] Implement validation that blocks unassigned, unauthorized, conflicted, final-state, cross-school, and restricted-detail resolution attempts in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintResolverGuard.cs`
- [ ] T099 [US3] Implement timeline, entry, resolution, and resolution-detail endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintResolutionController.cs`
- [ ] T100 [US3] Add resolution routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T101 [US3] Implement typed web API methods for complaint timeline, investigation entries, and resolution in `apps/admin-web/src/features/complaints/api/complaintResolutionApi.ts`
- [ ] T102 [US3] Implement complaint timeline component with internal, reviewer-only, restricted, and complainant-visible display rules in `apps/admin-web/src/features/complaints/components/ComplaintTimeline.tsx`
- [ ] T103 [US3] Implement resolver action panel for internal notes, visible responses, information requests, corrective actions, and closure in `apps/admin-web/src/features/complaints/components/ComplaintResolverPanel.tsx`
- [ ] T104 [US3] Implement assigned complaint detail page for investigators and resolvers in `apps/admin-web/src/app/(school)/complaints/assigned/[complaintId]/page.tsx`

**Checkpoint**: US3 is complete when resolver actions, timeline visibility, information requests, resolution closure, restricted detail filtering, audit, and no-side-effect assertions pass independently.

---

## Phase 6: User Story 4 - Escalate High-Risk, Overdue, or Conflicted Complaints (Priority: P1)

**Goal**: High-risk, overdue, disputed, unresolved, repeatedly reopened, externally reportable, and conflicted complaints escalate to the configured owner or reviewer with preserved reason and timing.

**Independent Test**: Submit complaints matching high-priority, overdue, disputed, and conflict-of-interest conditions, then verify each reaches the configured escalation owner without exposing restricted details or creating excluded-domain outcomes.

### Tests for User Story 4

- [ ] T105 [P] [US4] Create unit tests for escalation rule evaluation across high priority, safety, safeguarding, severe misconduct, externally reportable, target expired, disputed, reopened, unresolved, conflict-of-interest, and manual triggers in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Escalations/ComplaintEscalationRuleEvaluatorTests.cs`
- [ ] T106 [P] [US4] Create unit tests for target response and resolution expiry routing that never silently closes, dismisses, downgrades, or hides complaints in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Escalations/ComplaintTargetExpiryServiceTests.cs`
- [ ] T107 [P] [US4] Create contract tests for escalation-workflow.md escalation, evaluate-escalation, expire-target, queue, and trace endpoints in `tests/contracts/complaints/ComplaintEscalationContractTests.cs`
- [ ] T108 [P] [US4] Create integration tests for high-risk routing, target expiry, conflicted owner routing, missing route manual review, duplicate escalation idempotency, audit, and status events in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Escalations/ComplaintEscalationIntegrationTests.cs`
- [ ] T109 [P] [US4] Create web journey tests for escalation queue, manual escalation, overdue complaint review, and urgent review routing in `apps/admin-web/tests/complaints/complaintEscalation.spec.ts`

### Implementation for User Story 4

- [ ] T110 [P] [US4] Create escalation request, expiry request, escalation response, queue, and trace DTOs matching escalation-workflow.md in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintEscalationDtos.cs`
- [ ] T111 [US4] Implement escalation rule evaluator for configured triggers, priority changes, visibility constraints, target owners, target timings, and rule versions in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintEscalationRuleEvaluator.cs`
- [ ] T112 [US4] Implement escalation service for manual and rule-based escalation with idempotency, prior owner, new owner, reason, audit, exception fallback, and status event creation in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintEscalationService.cs`
- [ ] T113 [US4] Implement target expiry service for response and resolution windows using school-configured calendars when available and manual review fallback when timing cannot be evaluated in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintTargetExpiryService.cs`
- [ ] T114 [US4] Implement urgent review router for safety, safeguarding, medical, emergency, severe misconduct, and externally reportable signals without creating medical, emergency, or external authority outcomes in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintUrgentReviewRouter.cs`
- [ ] T115 [US4] Implement escalation queue query filtering by target owner, role, priority, target timing, confidentiality, tenant, and reviewer scope in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintEscalationQueueService.cs`
- [ ] T116 [US4] Implement escalation, evaluate-escalation, expire-target, queue, and trace endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/Escalations/ComplaintEscalationController.cs`
- [ ] T117 [US4] Add escalation routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T118 [US4] Implement typed web API methods for escalation, expiry, queue, and trace operations in `apps/admin-web/src/features/complaints/api/complaintEscalationApi.ts`
- [ ] T119 [US4] Implement escalation queue component with urgent, overdue, disputed, unresolved, repeatedly reopened, and conflicted filters in `apps/admin-web/src/features/complaints/components/ComplaintEscalationQueue.tsx`
- [ ] T120 [US4] Implement manual escalation and urgent review controls with reason capture and restricted visibility warnings in `apps/admin-web/src/features/complaints/components/ComplaintEscalationPanel.tsx`
- [ ] T121 [US4] Implement school escalation queue page in `apps/admin-web/src/app/(school)/complaints/escalations/page.tsx`

**Checkpoint**: US4 is complete when escalation triggers, target expiry, urgent routing, conflict routing, manual review fallback, audit, status events, and no-side-effect assertions pass independently.

---

## Phase 7: User Story 5 - Capture Feedback, Reopen Disputes, and Review History (Priority: P2)

**Goal**: Complainants can submit feedback or reopen requests, and authorized users can search history, view summaries, resolve exceptions, and trace complaint lifecycles.

**Independent Test**: Resolve a complaint, submit feedback or a reopen request, filter complaint history by key fields, and verify all history remains tenant-scoped and permission-scoped.

### Tests for User Story 5

- [ ] T122 [P] [US5] Create unit tests for feedback eligibility, satisfaction ratings, dissatisfaction, comments, disputes, reopen requests, configured reopen windows, and preserved original resolutions in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/History/ComplaintFeedbackServiceTests.cs`
- [ ] T123 [P] [US5] Create unit tests for history filters, summary counts, visibility filtering, lifecycle trace, and exception status transitions in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/History/ComplaintHistoryReviewTests.cs`
- [ ] T124 [P] [US5] Create contract tests for complaint-history-review.md history, summaries, exceptions, manual-review, and trace endpoints in `tests/contracts/complaints/ComplaintHistoryReviewContractTests.cs`
- [ ] T125 [P] [US5] Create contract tests for the feedback and reopen endpoints in feedback-resolution.md in `tests/contracts/complaints/ComplaintFeedbackContractTests.cs`
- [ ] T126 [P] [US5] Create integration tests for feedback submission, reopen routing, history search, summary reads, exception review, manual review correction, lifecycle trace, audit, and status events in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/History/ComplaintHistoryReviewIntegrationTests.cs`
- [ ] T127 [P] [US5] Create web journey tests for guardian/student feedback, reopen, history filtering, reviewer correction, and auditor trace in `apps/admin-web/tests/complaints/complaintHistoryReview.spec.ts`

### Implementation for User Story 5

- [ ] T128 [P] [US5] Create feedback, reopen, history query, summary, exception review, manual review, and trace DTOs matching complaint-history-review.md and feedback-resolution.md in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintHistoryReviewDtos.cs`
- [ ] T129 [US5] Implement feedback service for acceptance, dissatisfaction, ratings, comments, disputes, reopen requests, configured routing, audit, and status event creation in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintFeedbackService.cs`
- [ ] T130 [US5] Implement complaint history query service with filters for student, complainant, guardian, staff submitter, category, priority, confidentiality, status, owner, escalation, date range, target timing, feedback, exception, and review state in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintHistoryQueryService.cs`
- [ ] T131 [US5] Implement complaint review summary service with permission-scoped counts for categories, owners, escalation, feedback, exceptions, aging, and resolution outcomes in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintReviewSummaryService.cs`
- [ ] T132 [US5] Implement complaint exception service for invalid student, invalid guardian link, missing fields, disabled feature, disabled category, missing owner, missing escalation route, conflicted owner, stale configuration, duplicates, restricted evidence, missed targets, cross-school access, and manual review conditions in `apps/api/src/SafeSchool.Api/Features/Complaints/Reviews/ComplaintExceptionService.cs`
- [ ] T133 [US5] Implement manual complaint review service for correction, reopen, dismiss, escalate, resolve, close, rule version migration, restricted visibility exception, reason capture, and preserved original evidence in `apps/api/src/SafeSchool.Api/Features/Complaints/Reviews/ManualComplaintReviewService.cs`
- [ ] T134 [US5] Implement complaint lifecycle trace service linking submission, categorization, assignment, investigation, escalation, resolution, feedback, reopen, review, summaries, status events, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintLifecycleTraceService.cs`
- [ ] T135 [US5] Implement feedback endpoints for guardian and student complainants in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintFeedbackController.cs`
- [ ] T136 [US5] Implement history, summaries, exceptions, manual-review, and trace endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/History/ComplaintHistoryReviewController.cs`
- [ ] T137 [US5] Add feedback, history, review, summary, exception, and trace routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T138 [US5] Implement typed web API methods for feedback, reopen, history, summaries, exceptions, manual review, and trace in `apps/admin-web/src/features/complaints/api/complaintHistoryReviewApi.ts`
- [ ] T139 [US5] Implement feedback and reopen components for guardian and student complainants in `apps/admin-web/src/features/complaints/components/ComplaintFeedbackForm.tsx`
- [ ] T140 [US5] Implement complaint history filters and results table with strict visibility filtering in `apps/admin-web/src/features/complaints/components/ComplaintHistoryTable.tsx`
- [ ] T141 [US5] Implement complaint review summary and lifecycle trace components for reviewers and auditors in `apps/admin-web/src/features/complaints/components/ComplaintReviewSummary.tsx` and `apps/admin-web/src/features/complaints/components/ComplaintLifecycleTrace.tsx`
- [ ] T142 [US5] Implement guardian and student feedback routes in `apps/admin-web/src/app/(guardian)/complaints/[complaintId]/feedback/page.tsx` and `apps/admin-web/src/app/(student)/complaints/[complaintId]/feedback/page.tsx`
- [ ] T143 [US5] Implement school history, summary, exception, and audit trace pages in `apps/admin-web/src/app/(school)/complaints/history/page.tsx`, `apps/admin-web/src/app/(school)/complaints/summaries/page.tsx`, `apps/admin-web/src/app/(school)/complaints/exceptions/page.tsx`, and `apps/admin-web/src/app/(school)/complaints/[complaintId]/trace/page.tsx`

**Checkpoint**: US5 is complete when feedback, reopen, history search, summaries, exceptions, manual review, lifecycle trace, tenant isolation, visibility filtering, audit, and no-side-effect assertions pass independently.

---

## Phase 8: User Story 6 - Configure Complaint Rules (Priority: P3)

**Goal**: School administrators can configure complaint categories, required fields, confidentiality, owner groups, target timings, escalation rules, feedback rules, reopen rules, and feature settings.

**Independent Test**: Create and activate a complaint category and escalation rule, submit a complaint, and verify the complaint uses the active configuration while historical complaints retain prior versions.

### Tests for User Story 6

- [ ] T144 [P] [US6] Create unit tests for category configuration validation, owner group validation, required fields, target timing, confidentiality, feedback rules, reopen rules, versioning, and activation rejection reasons in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Configuration/ComplaintCategoryConfigurationTests.cs`
- [ ] T145 [P] [US6] Create unit tests for escalation rule configuration validation, non-circular routes, trigger validation, target owner validation, disabled capability handling, and versioning in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Configuration/ComplaintEscalationRuleConfigurationTests.cs`
- [ ] T146 [P] [US6] Create contract tests for complaint-configuration.md feature settings, category, activation, archive, escalation-rule, and activation endpoints in `tests/contracts/complaints/ComplaintConfigurationContractTests.cs`
- [ ] T147 [P] [US6] Create integration tests for configuration changes, rule activation, version preservation, invalid activation rejection, audit, and historical complaint rule lookup in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Configuration/ComplaintConfigurationIntegrationTests.cs`
- [ ] T148 [P] [US6] Create web journey tests for administrator category configuration, escalation rule configuration, activation, archive, and historical version review in `apps/admin-web/tests/complaints/complaintConfiguration.spec.ts`

### Implementation for User Story 6

- [ ] T149 [P] [US6] Create feature setting, category configuration, escalation rule configuration, activation, archive, and version DTOs matching complaint-configuration.md in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintConfigurationDtos.cs`
- [ ] T150 [US6] Implement complaint feature setting service for submission, categorization, assignment, escalation, feedback and resolution, history, configuration, and review summary capabilities in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintFeatureSettingService.cs`
- [ ] T151 [US6] Implement category configuration service for draft create, update, activate, archive, versioning, owner group validation, required fields, target timing, confidentiality, feedback, reopen, and activation rejection reasons in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintCategoryConfigurationService.cs`
- [ ] T152 [US6] Implement escalation rule configuration service for draft create, update, activate, archive, non-circular route validation, trigger validation, target owner validation, timing validation, and versioning in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintEscalationRuleConfigurationService.cs`
- [ ] T153 [US6] Implement feedback and reopen rule configuration helpers for category activation and historical complaint rule lookup in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintFeedbackRuleConfigurationService.cs`
- [ ] T154 [US6] Implement configuration endpoints for feature settings, categories, activation, archive, escalation rules, and rule activation in `apps/api/src/SafeSchool.Api/Features/Complaints/Configuration/ComplaintConfigurationController.cs`
- [ ] T155 [US6] Add configuration routes to module registration in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs`
- [ ] T156 [US6] Implement typed web API methods for feature settings, categories, escalation rules, activation, archive, and versions in `apps/admin-web/src/features/complaints/api/complaintConfigurationApi.ts`
- [ ] T157 [US6] Implement complaint category configuration form with required fields, allowed submitters, priority, confidentiality, owner group, target timings, feedback, reopen, and activation errors in `apps/admin-web/src/features/complaints/components/ComplaintCategoryConfigurationForm.tsx`
- [ ] T158 [US6] Implement escalation rule configuration form with triggers, target owners, timing, priority changes, visibility constraints, and non-circular validation messages in `apps/admin-web/src/features/complaints/components/ComplaintEscalationRuleConfigurationForm.tsx`
- [ ] T159 [US6] Implement school complaint configuration pages in `apps/admin-web/src/app/(school)/complaints/configuration/page.tsx`, `apps/admin-web/src/app/(school)/complaints/configuration/categories/[categoryId]/page.tsx`, and `apps/admin-web/src/app/(school)/complaints/configuration/escalation-rules/[ruleId]/page.tsx`

**Checkpoint**: US6 is complete when configuration, activation, invalid activation rejection, version preservation, historical rule lookup, audit, and no-side-effect assertions pass independently.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Tighten quality, performance, security, documentation, and validation across completed user stories.

- [ ] T160 [P] Add OpenAPI or route documentation for all Phase 8 `/api/v1/` endpoints in `apps/api/src/SafeSchool.Api/Features/Complaints/ComplaintsOpenApi.cs`
- [ ] T161 [P] Add a complaint API client contract snapshot used by web and mobile clients in `apps/admin-web/src/features/complaints/api/complaints.contract.ts`
- [ ] T162 [P] Add backend performance tests for submission under 2 minutes, categorization under 60 seconds, resolver action under 60 seconds, escalation visibility under 1 minute, status lookup under 30 seconds, feedback under 60 seconds, lifecycle trace under 60 seconds, and status event availability under 2 minutes in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintPerformanceTests.cs`
- [ ] T163 [P] Add security regression tests for restricted internal notes, conflicted-party details, safety review notes, staff-only details, cross-school data, guardian link restrictions, student self-scope, multi-participant visibility restrictions, and platform reviewer scope in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintSecurityRegressionTests.cs`
- [ ] T164 Add an audit coverage test that verifies every audit event required by spec.md FR-028 is emitted by at least one workflow in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintAuditCoverageTests.cs`
- [ ] T165 Add a no-side-effect regression test proving Phase 8 creates no attendance, gate, scan, transport, wallet, learning reward, request approval, medical, emergency, document, search, broad messaging, or broad dashboard outcomes in `apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/ComplaintNoSideEffectTests.cs`
- [ ] T166 [P] Add end-to-end smoke tests for guardian submission, student submission, staff triage, resolver closure, escalation, feedback, history, and configuration in `tests/e2e/complaints/complaints.e2e.spec.ts`
- [ ] T167 [P] Add user-facing accessibility checks for complaint forms, queues, timelines, feedback forms, and configuration forms in `apps/admin-web/tests/complaints/complaintAccessibility.spec.ts`
- [ ] T168 [P] If `apps/mobile/` exists or mobile complaint surfaces are enabled, add mobile smoke tests for submission, tracking, and feedback in `apps/mobile/test/features/complaints/complaints_smoke_test.dart`
- [ ] T169 Update Phase 8 quickstart validation results and implementation notes in `specs/009-complaints-escalations/quickstart.md`
- [ ] T170 Update the Complaints module README with commands, role matrix, capability matrix, route map, status lifecycle, and no-side-effect boundaries in `apps/api/src/SafeSchool.Api/Features/Complaints/README.md`
- [ ] T171 Run all backend, contract, web, enabled mobile, and e2e complaint tests and record the command set in `specs/009-complaints-escalations/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1; blocks all user story work.
- **Phase 3 US1**: Depends on Phase 2; MVP and first independently useful increment.
- **Phase 4 US2**: Depends on Phase 2; can run after or in parallel with US1, but full queue usefulness improves after US1.
- **Phase 5 US3**: Depends on Phase 2 and is most useful after US2 assignment exists; can be implemented with seeded assigned complaints for independent testing.
- **Phase 6 US4**: Depends on Phase 2 and is most useful after US2 and US3; can be implemented with seeded categorized or assigned complaints for independent testing.
- **Phase 7 US5**: Depends on Phase 2 and is most useful after US3 and US4; can be implemented with seeded resolved and escalated complaints for independent testing.
- **Phase 8 US6**: Depends on Phase 2; can run in parallel with other stories because it owns configuration paths, but full validation should be rerun after all stories.
- **Phase 9 Polish**: Depends on all desired user stories being complete.

### User Story Dependency Graph

```text
Setup -> Foundation -> US1 -> MVP validation
Setup -> Foundation -> US2 -> categorization validation
Setup -> Foundation -> US3 -> resolution validation
Setup -> Foundation -> US4 -> escalation validation
Setup -> Foundation -> US5 -> feedback/history validation
Setup -> Foundation -> US6 -> configuration validation

Recommended delivery order for one implementer:
US1 -> US2 -> US3 -> US4 -> US5 -> US6 -> Polish
```

### Within Each User Story

- Write the listed tests first and confirm they fail before implementation.
- Implement DTOs before validators and services.
- Implement validators and guards before endpoint exposure.
- Implement services before controllers.
- Implement backend endpoints before web, mobile, and e2e journeys.
- Complete tenant resolution, feature flag checks, authorization, audit, idempotency, visibility filtering, status events, and no-side-effect checks before marking a story complete.

## Parallel Opportunities

- T002 through T008 can run in parallel after T001.
- T021 through T030 can run in parallel after T013 through T020 define shared enums and policies.
- T032 through T034 can run in parallel after T021 through T030.
- T040 through T043 can run in parallel after foundational paths exist.
- Test tasks at the start of each user story can run in parallel because they touch separate files.
- Web tasks and conditional mobile tasks in each user story can run after that story's backend API contract is stable.
- US6 configuration work can run in parallel with US1 through US5 after Phase 2 because it owns separate service, controller, and UI files.

## Parallel Example: User Story 1

```bash
Task: "T046 create submission validator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Submission/ComplaintSubmissionValidatorTests.cs"
Task: "T048 create contract tests in tests/contracts/complaints/ComplaintSubmissionContractTests.cs"
Task: "T050 create web journey tests in apps/admin-web/tests/complaints/complaintSubmission.spec.ts"
Task: "T052 create submission DTOs in apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintSubmissionDtos.cs"
Task: "T053 create tracking DTOs in apps/api/src/SafeSchool.Api/Features/Complaints/Submission/ComplaintTrackingDtos.cs"
```

## Parallel Example: User Story 2

```bash
Task: "T072 create category evaluator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Categorization/ComplaintCategoryRuleEvaluatorTests.cs"
Task: "T073 create assignment service tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Categorization/ComplaintAssignmentServiceTests.cs"
Task: "T074 create contract tests in tests/contracts/complaints/ComplaintCategorizationContractTests.cs"
Task: "T085 create web API methods in apps/admin-web/src/features/complaints/api/complaintCategorizationApi.ts"
```

## Parallel Example: User Story 3

```bash
Task: "T089 create investigation entry tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Resolutions/ComplaintInvestigationEntryTests.cs"
Task: "T090 create resolution service tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Resolutions/ComplaintResolutionServiceTests.cs"
Task: "T094 create resolution DTOs in apps/api/src/SafeSchool.Api/Features/Complaints/Resolutions/ComplaintResolutionDtos.cs"
Task: "T101 create web API methods in apps/admin-web/src/features/complaints/api/complaintResolutionApi.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T105 create escalation evaluator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Escalations/ComplaintEscalationRuleEvaluatorTests.cs"
Task: "T106 create target expiry tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Escalations/ComplaintTargetExpiryServiceTests.cs"
Task: "T107 create contract tests in tests/contracts/complaints/ComplaintEscalationContractTests.cs"
Task: "T118 create web API methods in apps/admin-web/src/features/complaints/api/complaintEscalationApi.ts"
```

## Parallel Example: User Story 5

```bash
Task: "T122 create feedback service tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/History/ComplaintFeedbackServiceTests.cs"
Task: "T123 create history review tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/History/ComplaintHistoryReviewTests.cs"
Task: "T124 create history contract tests in tests/contracts/complaints/ComplaintHistoryReviewContractTests.cs"
Task: "T138 create web API methods in apps/admin-web/src/features/complaints/api/complaintHistoryReviewApi.ts"
```

## Parallel Example: User Story 6

```bash
Task: "T144 create category configuration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Configuration/ComplaintCategoryConfigurationTests.cs"
Task: "T145 create escalation rule configuration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Complaints/Configuration/ComplaintEscalationRuleConfigurationTests.cs"
Task: "T146 create configuration contract tests in tests/contracts/complaints/ComplaintConfigurationContractTests.cs"
Task: "T156 create web API methods in apps/admin-web/src/features/complaints/api/complaintConfigurationApi.ts"
```

## Implementation Strategy

### MVP First

1. Complete Phase 1 Setup.
2. Complete Phase 2 Foundational.
3. Complete Phase 3 User Story 1.
4. Stop and validate US1 with T046 through T071 plus foundational tests.
5. Demo complaint submission and tracking before adding triage, resolution, escalation, feedback, or configuration.

### Incremental Delivery

1. Add US1 to establish complaint intake and tracking.
2. Add US2 to make complaints actionable through categorization and assignment.
3. Add US3 to complete investigation and resolution.
4. Add US4 to protect high-risk, overdue, and conflicted complaints.
5. Add US5 to provide feedback, reopen, history, summaries, and audit traceability.
6. Add US6 to let schools configure complaint policies.
7. Finish Phase 9 polish and run quickstart validation.

### Notes for Lower-Cost LLM Implementation

- Read `specs/009-complaints-escalations/spec.md`, `plan.md`, `data-model.md`, and the matching contract file before starting each story phase.
- Do not implement broad messaging, broadcasts, document storage, global search, attendance, gate, scan, transport, wallet, learning reward, request approval, medical, emergency, or broad dashboard outcomes.
- Keep every mutation tenant-scoped, feature-gated, permission-checked, idempotent by `client_request_id`, audit-visible, and status-event-aware.
- Preserve original complaint evidence; implement corrections and reviews as appended records.
- Keep internal and restricted details separate from complainant-visible details in both backend DTOs and UI components.
- Treat mobile tasks as conditional: implement them only when `apps/mobile/` exists or mobile complaint surfaces are enabled for Phase 8.
