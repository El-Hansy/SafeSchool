# Tasks: Phase 9 Communication & Notifications

**Input**: Design documents from `/specs/010-communication-notifications/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Include tests required by the constitution and Phase 9 plan. Business logic requires unit tests; API contracts, tenant/feature authorization, migrations, delivery tracking, acknowledgements, moderation, audit, and critical user journeys require integration or contract coverage.

**Organization**: Tasks are grouped by user story so each story can be implemented, tested, and reviewed independently after the shared foundation is complete.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase.
- **[Story]**: User story label from spec.md. Setup, Foundational, and Polish tasks do not use story labels.
- Every task includes exact file paths. If a path does not exist yet, create it as part of that task.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the module directories, skeleton files, and test locations that all later tasks will use.

- [X] T001 Create the Communications backend directory tree from the implementation plan in `apps/api/src/SafeSchool.Api/Features/Communications/`
- [X] T002 [P] Create the Communications backend test directory tree in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/`
- [X] T003 [P] Create the cross-service contract test directory tree in `tests/contracts/communications/`
- [X] T004 [P] Create the Communications end-to-end test directory tree in `tests/e2e/communications/`
- [X] T005 [P] Create the Communications web feature directory tree in `apps/admin-web/src/features/communications/`
- [X] T006 [P] Create the school, guardian, and student web route directories in `apps/admin-web/src/app/(school)/communications/`, `apps/admin-web/src/app/(guardian)/communications/`, and `apps/admin-web/src/app/(student)/communications/`
- [X] T007 [P] Create the Communications web test directory tree in `apps/admin-web/tests/communications/`
- [X] T008 [P] If `apps/mobile/` exists or mobile communication surfaces are enabled, create the Communications mobile feature and test directory trees in `apps/mobile/lib/features/communications/` and `apps/mobile/test/features/communications/`
- [X] T009 Create the Communications backend module registration skeleton in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T010 [P] Create the Communications module implementation notes for future LLMs in `apps/api/src/SafeSchool.Api/Features/Communications/README.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Implement shared models, guards, persistence, audit, delivery abstractions, and contract scaffolding that every user story depends on.

**Critical**: No user story implementation should begin until this phase is complete.

- [X] T011 Create communication capability constants for direct messaging, staff-to-guardian messaging, student messaging, broadcasts, announcements, notification center, external delivery channels, templates, delivery tracking, acknowledgements, preferences, history, moderation, configuration, and review summaries in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationCapabilities.cs`
- [X] T012 Create communication permission constants for all Phase 9 roles and actions in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationPermissions.cs`
- [X] T013 Create communication enum definitions for source modules, communication kinds, participant types, priorities, visibility levels, conversation statuses, reply policies, publication statuses, delivery statuses, acknowledgement states, moderation states, exception types, and review actions in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationEnums.cs`
- [X] T014 Create stable communication error codes and validation messages in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationErrors.cs`
- [X] T015 Create audit event names for all events listed in spec.md FR-023 in `apps/api/src/SafeSchool.Api/Features/Communications/Audit/CommunicationAuditEvents.cs`
- [X] T016 Create a tenant, capability, and school-account guard for Communications workflows in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationTenantFeatureGuard.cs`
- [X] T017 Create a role, permission, guardian-link, student self-scope, sender, recipient, audience, moderator, and reviewer authorization guard in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationAuthorizationGuard.cs`
- [X] T018 Create a communication idempotency helper for `client_request_id`, source event dedupe keys, and delivery retry commands in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationIdempotencyService.cs`
- [X] T019 Create a communication visibility policy that separates internal, restricted, reviewer-only, status-only, and recipient-visible details in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationVisibilityPolicy.cs`
- [X] T020 Create a restricted-detail minimization service for medical, complaint, finance, staff, safety, and student-welfare summaries in `apps/api/src/SafeSchool.Api/Features/Communications/Common/RestrictedDetailMinimizer.cs`
- [X] T021 Create a recipient eligibility policy for guardians, students, staff, roles, approved guardian links, source-event recipients, and cross-school exclusions in `apps/api/src/SafeSchool.Api/Features/Communications/Common/RecipientEligibilityPolicy.cs`
- [X] T022 Create a duplicate suppression service for source events, audience recipients, messages, broadcasts, notification intents, and delivery attempts in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationDuplicateSuppressionService.cs`
- [X] T023 Create a communication lifecycle state policy for created, queued, sent, delivered, failed, read, acknowledged, overdue, moderated, withdrawn, corrected, suppressed, and access-denied states in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationLifecyclePolicy.cs`
- [X] T024 Create quiet-hour, mandatory-category, and optional preference policy helpers in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationPreferencePolicy.cs`
- [X] T025 Create delivery channel abstraction interfaces for in-app, push, email, SMS, and other school-enabled channels in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/CommunicationDeliveryChannels.cs`
- [X] T026 [P] Create the Conversation entity with all data-model fields and tenant timestamps in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/Conversation.cs`
- [X] T027 [P] Create the ConversationParticipant entity with participant type, send scope, visibility scope, guardian link, and eligibility fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/ConversationParticipant.cs`
- [X] T028 [P] Create the Message entity with sequence, sender, body, visibility, correction, withdrawal, moderation, acknowledgement, and idempotency fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/Message.cs`
- [X] T029 [P] Create the BroadcastAnnouncement entity with title, body, audience, template, publication, correction, withdrawal, acknowledgement, and idempotency fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/BroadcastAnnouncement.cs`
- [X] T030 [P] Create the AudienceRule entity with audience type, criteria, recipient types, restricted-detail policy, moderation, status, and version fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/AudienceRule.cs`
- [X] T031 [P] Create the RecipientSnapshot entity with communication reference, recipient, student, guardian link, inclusion, exclusion, visibility, and eligibility fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/RecipientSnapshot.cs`
- [X] T032 [P] Create the NotificationSourceEvent entity with source module, source reference, student context, priority, restricted detail level, eligibility, dedupe key, and status fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/NotificationSourceEvent.cs`
- [X] T033 [P] Create the NotificationRecord entity with source event, recipient, category, priority, summary, reply policy, support action, correction, withdrawal, delivery, read, acknowledgement, exception, and idempotency fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/NotificationRecord.cs`
- [X] T034 [P] Create the CommunicationTemplate entity with category, language, title, body, required variables, default audience, restricted-detail, acknowledgement, quiet-hour, channel, moderation, and version fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationTemplate.cs`
- [X] T035 [P] Create the DeliveryAttempt entity with communication reference, recipient snapshot, channel, attempt number, delivery status, failure, exclusion, retry, provider, and final-state fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/DeliveryAttempt.cs`
- [X] T036 [P] Create the CommunicationPreference entity with actor, recipient type, category, channel, preference state, quiet hours, mandatory override, status, and version fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationPreference.cs`
- [X] T037 [P] Create the AcknowledgementRecord entity with communication reference, recipient, read time, acknowledgement time, deadline, reminder, overdue, waiver, and reason fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/AcknowledgementRecord.cs`
- [X] T038 [P] Create the CommunicationException entity with communication kind, reference, student, recipient, exception type, severity, evidence, status, reviewer, and resolution fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationException.cs`
- [X] T039 [P] Create the ModerationReview entity with communication reference, action, reason, status-before/status-after, reviewer, visibility change, and content change fields in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/ModerationReview.cs`
- [X] T040 [P] Create the CommunicationReviewSummary, CommunicationFeatureSetting, and CommunicationLifecycleEvent entities in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationReviewSummary.cs`, `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationFeatureSetting.cs`, and `apps/api/src/SafeSchool.Api/Features/Communications/Data/Entities/CommunicationLifecycleEvent.cs`
- [X] T041 Create EF Core DbContext extension and DbSet registration for all Phase 9 entities in `apps/api/src/SafeSchool.Api/Features/Communications/Data/CommunicationsDbContextExtensions.cs`
- [X] T042 [P] Create EF Core mappings and tenant indexes for Conversation, ConversationParticipant, Message, and RecipientSnapshot in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Configurations/CommunicationCoreEntityConfigurations.cs`
- [X] T043 [P] Create EF Core mappings and tenant indexes for NotificationSourceEvent, NotificationRecord, DeliveryAttempt, and AcknowledgementRecord in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Configurations/CommunicationNotificationEntityConfigurations.cs`
- [X] T044 [P] Create EF Core mappings and tenant indexes for BroadcastAnnouncement, AudienceRule, CommunicationTemplate, CommunicationPreference, and CommunicationFeatureSetting in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Configurations/CommunicationConfigurationEntityConfigurations.cs`
- [X] T045 [P] Create EF Core mappings and tenant indexes for CommunicationException, ModerationReview, CommunicationReviewSummary, and CommunicationLifecycleEvent in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Configurations/CommunicationReviewEntityConfigurations.cs`
- [X] T046 Create the Phase 9 EF Core migration for all communication tables, foreign keys, tenant indexes, recipient indexes, source event indexes, delivery indexes, acknowledgement indexes, moderation indexes, duplicate suppression indexes, and lifecycle indexes in `apps/api/src/SafeSchool.Api/Features/Communications/Data/Migrations/202605060002_AddCommunicationNotifications.cs`
- [X] T047 [P] Create seed data for communication capabilities, permissions, baseline templates, notification categories, audience rules, feature settings, and review queues in `apps/api/src/SafeSchool.Api/Features/Communications/Seed/CommunicationSeedData.cs`
- [X] T048 [P] Create shared DTOs for pagination, actor context, recipient summaries, visibility summaries, delivery summaries, acknowledgement summaries, and error responses in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationCommonDtos.cs`
- [X] T049 [P] Create the communication lifecycle event writer for review and later monitoring boundaries in `apps/api/src/SafeSchool.Api/Features/Communications/Common/CommunicationLifecycleEventWriter.cs`
- [X] T050 [P] Create the communication audit writer that records actor, tenant, student, recipient, communication, action, reason, viewed category, and result in `apps/api/src/SafeSchool.Api/Features/Communications/Audit/CommunicationAuditWriter.cs`
- [X] T051 Create shared backend test fixtures for tenants, users, guardian links, students, roles, permissions, feature flags, delivery channels, audit capture, and idempotency in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationTestFixture.cs`
- [X] T052 [P] Create contract test fixture utilities for Phase 9 route authentication, tenant headers, feature flags, idempotency, pagination, and response assertions in `tests/contracts/communications/CommunicationContractTestFixture.cs`
- [X] T053 [P] Create web test fixtures for school, staff, guardian, student, communication manager, moderator, reviewer, and auditor users in `apps/admin-web/tests/communications/communicationTestFixtures.ts`
- [X] T054 [P] If `apps/mobile/` exists or mobile communication surfaces are enabled, create mobile test fixtures for notification, message, and acknowledgement flows in `apps/mobile/test/features/communications/communication_test_fixtures.dart`
- [X] T055 Create foundational tests for tenant isolation, feature-flag denial, permission denial, restricted-detail minimization, duplicate suppression, audit emission, and no-side-effect boundaries in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationFoundationTests.cs`
- [X] T056 Create migration tests that verify all Phase 9 tables include `tenant_id`, `created_at`, `updated_at`, required indexes, and foreign keys in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationMigrationTests.cs`

**Checkpoint**: Foundation ready. User story phases may now proceed independently or in parallel.

---

## Phase 3: User Story 1 - Receive and Review Notifications (Priority: P1) MVP

**Goal**: Guardians, students, and staff can view permission-scoped notification records generated from eligible source events without exposing unrelated or restricted details.

**Independent Test**: Record an eligible school status event for one student, verify the authorized guardian and relevant staff receive a notification inside the correct school account, and confirm unrelated guardians, students, staff, and other school accounts cannot view it.

### Tests for User Story 1

- [X] T057 [P] [US1] Create unit tests for source event validation, communication eligibility, dedupe keys, and source module boundaries in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationSourceEventValidatorTests.cs`
- [X] T058 [P] [US1] Create unit tests for notification recipient resolution across guardians, students, staff, roles, approved links, and cross-school exclusions in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationRecipientResolverTests.cs`
- [X] T059 [P] [US1] Create unit tests for duplicate source event and duplicate notification intent suppression in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationDuplicateSuppressionTests.cs`
- [X] T060 [P] [US1] Create unit tests for restricted-detail minimization and recipient-visible notification summaries in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationVisibilityTests.cs`
- [X] T061 [P] [US1] Create contract tests for notification-center.md source event, school, guardian, student, correction, withdrawal endpoints, and reply-policy response fields in `tests/contracts/communications/NotificationCenterContractTests.cs`
- [X] T062 [P] [US1] Create integration tests for source event intake, notification generation, correction, withdrawal, reply-policy enforcement, tenant isolation, guardian visibility, student self-scope, feature flags, audit evidence, lifecycle evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationCenterIntegrationTests.cs`
- [X] T063 [P] [US1] Create web journey tests for guardian, student, and staff notification lists, filters, read state, correction state, withdrawal state, reply-policy display, support-action display, and detail views in `apps/admin-web/tests/communications/notificationCenter.spec.ts`
- [X] T064 [P] [US1] If `apps/mobile/` exists or mobile communication surfaces are enabled, create mobile journey tests for notification list, detail, read state, acknowledgement, correction state, withdrawal state, reply-policy display, and support-action display in `apps/mobile/test/features/communications/notification_center_flow_test.dart`

### Implementation for User Story 1

- [X] T065 [P] [US1] Create notification source event, notification query, read state, acknowledgement, correction, withdrawal, reply-policy, support-action, and response DTOs matching notification-center.md in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationCenterDtos.cs`
- [X] T066 [US1] Implement source event validation for tenant scope, source module, source event version, student context, communication eligibility, restricted detail level, feature flags, and dedupe key in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationSourceEventValidator.cs`
- [X] T067 [US1] Implement source event intake with idempotency, duplicate suppression, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationSourceEventService.cs`
- [X] T068 [US1] Implement notification template rendering with fallback summaries, required variable validation, language fallback, and restricted-detail minimization in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationTemplateRenderer.cs`
- [X] T069 [US1] Implement notification recipient resolution for approved guardians, student self-scope, staff roles, source-event recipients, exclusions, and recipient snapshots in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationRecipientResolver.cs`
- [X] T070 [US1] Implement notification duplicate suppression for source events, recipient snapshots, and repeated communication intent in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationDuplicateSuppressionService.cs`
- [X] T071 [US1] Implement notification generation orchestration with recipient snapshots, notification records, reply policy, support action, in-app delivery attempts, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationGenerationService.cs`
- [X] T072 [US1] Implement notification state mutation service for read, acknowledgement, correction, withdrawal, and no-reply or support-route enforcement with reason capture, original content preservation, corrected content preservation, recipient impact, per-recipient authorization, idempotency, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationStateMutationService.cs`
- [X] T073 [US1] Implement notification query service with filters for student, category, priority, source module, read state, acknowledgement state, correction state, withdrawal state, reply policy, date range, and recipient scope in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationQueryService.cs`
- [X] T074 [US1] Implement source event intake endpoint in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationSourceEventController.cs`
- [X] T075 [US1] Implement school staff notification list, detail, read, acknowledgement, correction, and withdrawal endpoints with reply-policy response fields in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/SchoolNotificationController.cs`
- [X] T076 [US1] Implement guardian notification list, detail, read, and acknowledgement endpoints with reply-policy and support-action response fields in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/GuardianNotificationController.cs`
- [X] T077 [US1] Implement student notification list, detail, read, and acknowledgement endpoints with reply-policy and support-action response fields where student notifications are enabled in `apps/api/src/SafeSchool.Api/Features/Communications/Notifications/StudentNotificationController.cs`
- [X] T078 [US1] Add notification center, correction, and withdrawal routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T079 [US1] Implement typed web API methods for source events, notification list, detail, read state, acknowledgement, correction, withdrawal, reply-policy mapping, and support-action display in `apps/admin-web/src/features/communications/api/notificationsApi.ts`
- [X] T080 [US1] Implement shared notification list, unread count, filters, detail, correction state, withdrawal state, reply policy, support action, read state, and acknowledgement components in `apps/admin-web/src/features/communications/components/NotificationCenter.tsx`
- [X] T081 [US1] Implement guardian notification routes for list, detail, reply-policy display, and support-action display in `apps/admin-web/src/app/(guardian)/communications/notifications/page.tsx` and `apps/admin-web/src/app/(guardian)/communications/notifications/[notificationId]/page.tsx`
- [X] T082 [US1] Implement student notification routes for list, detail, reply-policy display, and support-action display in `apps/admin-web/src/app/(student)/communications/notifications/page.tsx` and `apps/admin-web/src/app/(student)/communications/notifications/[notificationId]/page.tsx`
- [X] T083 [US1] Implement school notification routes for list, detail, correction, withdrawal, reply-policy display, and support-action display in `apps/admin-web/src/app/(school)/communications/notifications/page.tsx` and `apps/admin-web/src/app/(school)/communications/notifications/[notificationId]/page.tsx`
- [X] T084 [US1] If `apps/mobile/` exists or mobile communication surfaces are enabled, implement mobile notification center screen with correction state, withdrawal state, reply-policy display, and support-action display in `apps/mobile/lib/features/communications/notification_center_screen.dart`

**Checkpoint**: US1 is complete when source event intake, notification generation, notification correction, notification withdrawal, reply-policy display, support-action routing, notification reads, restricted-detail filtering, tenant isolation, feature gating, duplicate suppression, audit, lifecycle evidence, and no-side-effect assertions pass independently.

---

## Phase 4: User Story 2 - Send Scoped Direct Messages (Priority: P1)

**Goal**: Authorized staff, guardians, and students where enabled can send and reply to scoped messages inside accountable tenant-owned conversations.

**Independent Test**: Send a message between an authorized staff member and an approved guardian for an active student, verify both participants can view and reply in the thread, and confirm unrelated users and other school accounts cannot access the conversation.

### Tests for User Story 2

- [X] T085 [P] [US2] Create unit tests for conversation participant validation, sender authority, guardian-link eligibility, student messaging rules, and cross-school denial in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Messaging/ConversationParticipantValidatorTests.cs`
- [X] T086 [P] [US2] Create unit tests for message send, reply policy, read state, visibility, correction, withdrawal, and closed-thread behavior in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Messaging/MessageServiceTests.cs`
- [X] T087 [P] [US2] Create contract tests for direct-messaging.md school, guardian, and student endpoints in `tests/contracts/communications/DirectMessagingContractTests.cs`
- [X] T088 [P] [US2] Create integration tests for conversation creation, replies, tenant isolation, feature flags, moderation routing, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Messaging/DirectMessagingIntegrationTests.cs`
- [X] T089 [P] [US2] Create web journey tests for staff, guardian, and student conversation list, thread detail, compose, reply, and withdrawal in `apps/admin-web/tests/communications/directMessaging.spec.ts`
- [X] T090 [P] [US2] If `apps/mobile/` exists or mobile communication surfaces are enabled, create mobile journey tests for conversation list, thread detail, and reply in `apps/mobile/test/features/communications/direct_messaging_flow_test.dart`

### Implementation for User Story 2

- [X] T091 [P] [US2] Create conversation, participant, message send, message response, withdrawal, close, and thread DTOs matching direct-messaging.md in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/DirectMessagingDtos.cs`
- [X] T092 [US2] Implement conversation participant resolution for staff, guardians, students, roles, approved guardian links, ineligible participants, and recipient snapshots in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/ConversationParticipantResolver.cs`
- [X] T093 [US2] Implement conversation validation for conversation type, subject, source context, priority, reply policy, visibility, participants, feature flags, and tenant scope in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/ConversationValidator.cs`
- [X] T094 [US2] Implement conversation service for create, list, detail, participant visibility, latest activity, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/ConversationService.cs`
- [X] T095 [US2] Implement message send and reply service with sequence generation, idempotency, visibility checks, delivery attempts, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/MessageSendService.cs`
- [X] T096 [US2] Implement message read-state service with per-participant read evidence and recipient scope in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/MessageReadStateService.cs`
- [X] T097 [US2] Implement message correction and withdrawal service with reason capture, original content preservation, recipient impact, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/MessageCorrectionWithdrawalService.cs`
- [X] T098 [US2] Implement conversation close, archive, and no-reply enforcement with reason capture and audit evidence in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/ConversationStateService.cs`
- [X] T099 [US2] Implement messaging moderation gate for student communication rules, restricted content, high-risk categories, and large recipient groups in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/MessageModerationGate.cs`
- [X] T100 [US2] Implement school conversation, message, withdrawal, and close endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/SchoolMessagingController.cs`
- [X] T101 [US2] Implement guardian conversation and reply endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/GuardianMessagingController.cs`
- [X] T102 [US2] Implement student conversation and reply endpoints where student messaging is enabled in `apps/api/src/SafeSchool.Api/Features/Communications/Messaging/StudentMessagingController.cs`
- [X] T103 [US2] Add direct messaging routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T104 [US2] Implement typed web API methods for conversations, messages, replies, withdrawal, and close actions in `apps/admin-web/src/features/communications/api/directMessagingApi.ts`
- [X] T105 [US2] Implement conversation list component with participant, student context, priority, latest activity, unread state, and closed state display in `apps/admin-web/src/features/communications/components/ConversationList.tsx`
- [X] T106 [US2] Implement message thread component with visibility filtering, sequence order, read state, correction, withdrawal, and acknowledgement markers in `apps/admin-web/src/features/communications/components/MessageThread.tsx`
- [X] T107 [US2] Implement compose and reply components with participant selection, subject, priority, visibility, acknowledgement requirement, and moderation warnings in `apps/admin-web/src/features/communications/components/MessageComposer.tsx`
- [X] T108 [US2] Implement school conversation routes for list, new conversation, and detail in `apps/admin-web/src/app/(school)/communications/conversations/page.tsx`, `apps/admin-web/src/app/(school)/communications/conversations/new/page.tsx`, and `apps/admin-web/src/app/(school)/communications/conversations/[conversationId]/page.tsx`
- [X] T109 [US2] Implement guardian conversation routes for list and detail in `apps/admin-web/src/app/(guardian)/communications/conversations/page.tsx` and `apps/admin-web/src/app/(guardian)/communications/conversations/[conversationId]/page.tsx`
- [X] T110 [US2] Implement student conversation routes for list and detail in `apps/admin-web/src/app/(student)/communications/conversations/page.tsx` and `apps/admin-web/src/app/(student)/communications/conversations/[conversationId]/page.tsx`
- [X] T111 [US2] If `apps/mobile/` exists or mobile communication surfaces are enabled, implement mobile direct messaging screen in `apps/mobile/lib/features/communications/direct_messaging_screen.dart`

**Checkpoint**: US2 is complete when direct conversations, replies, participant scope, closed/no-reply handling, moderation routing, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 5: User Story 3 - Publish Broadcasts and Announcements (Priority: P1)

**Goal**: Communication managers and administrators can draft, approve, schedule, publish, correct, and withdraw broadcasts or announcements to authorized recipient snapshots.

**Independent Test**: Publish an announcement to a permitted grade, class, route, staff group, guardian group, or whole-school audience, verify the recipient snapshot is tenant-scoped and relationship-scoped, and confirm excluded or unauthorized recipients do not receive it.

### Tests for User Story 3

- [X] T112 [P] [US3] Create unit tests for audience rule evaluation, recipient criteria, recipient count confirmation, duplicate recipient suppression, and cross-school exclusions in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Broadcasts/AudienceRuleEvaluatorTests.cs`
- [X] T113 [P] [US3] Create unit tests for broadcast draft, approval, scheduling, publication, correction, withdrawal, expiration, and publication state transitions in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Broadcasts/BroadcastPublicationServiceTests.cs`
- [X] T114 [P] [US3] Create contract tests for broadcast-announcement.md draft, approval, publish, correction, withdrawal, and recipient snapshot endpoints in `tests/contracts/communications/BroadcastAnnouncementContractTests.cs`
- [X] T115 [P] [US3] Create integration tests for broadcast publication, audience snapshots, tenant isolation, feature flags, moderation, audit evidence, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Broadcasts/BroadcastAnnouncementIntegrationTests.cs`
- [X] T116 [P] [US3] Create web journey tests for administrator and communication manager broadcast draft, audience selection, approval, publication, correction, and withdrawal in `apps/admin-web/tests/communications/broadcastAnnouncement.spec.ts`

### Implementation for User Story 3

- [X] T117 [P] [US3] Create broadcast draft, publish, approval, correction, withdrawal, audience query, recipient snapshot, and response DTOs matching broadcast-announcement.md in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastAnnouncementDtos.cs`
- [X] T118 [US3] Implement audience rule evaluator for whole school, guardians, students, staff, grade, class, route, activity group, role group, source-event recipients, manual recipients, and restricted-detail policies in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/AudienceRuleEvaluator.cs`
- [X] T119 [US3] Implement recipient snapshot service with inclusion reasons, exclusion reasons, duplicate suppression, relationship evidence, and delivery eligibility in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/RecipientSnapshotService.cs`
- [X] T120 [US3] Implement broadcast and announcement validation for title, body, language, priority, category, audience, template, effective window, acknowledgement requirement, moderation, feature flags, and tenant scope in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastAnnouncementValidator.cs`
- [X] T121 [US3] Implement broadcast draft service with idempotency, template rendering, restricted-detail checks, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastDraftService.cs`
- [X] T122 [US3] Implement broadcast approval and scheduling service with pending approval, scheduled release, no-early-delivery guarantees, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastApprovalSchedulingService.cs`
- [X] T123 [US3] Implement broadcast publication service with recipient snapshots, delivery attempts, acknowledgement records, duplicate suppression, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastPublicationService.cs`
- [X] T124 [US3] Implement broadcast correction and withdrawal service with reason capture, original evidence preservation, recipient impact, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastCorrectionWithdrawalService.cs`
- [X] T125 [US3] Implement broadcast and announcement query service with filters for type, category, priority, publication status, effective window, audience, and actor scope in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastAnnouncementQueryService.cs`
- [X] T126 [US3] Implement broadcast, announcement, approval, publication, correction, withdrawal, and recipient snapshot endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastAnnouncementController.cs`
- [X] T127 [US3] Add broadcast and announcement routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T128 [US3] Implement typed web API methods for broadcasts, announcements, audience resolution, recipient snapshots, approval, publication, correction, and withdrawal in `apps/admin-web/src/features/communications/api/broadcastAnnouncementApi.ts`
- [X] T129 [US3] Implement broadcast editor component with title, body, language, priority, category, template, effective window, acknowledgement, and moderation controls in `apps/admin-web/src/features/communications/components/BroadcastEditor.tsx`
- [X] T130 [US3] Implement audience selector component with whole school, guardian, student, staff, grade, class, route, activity group, role group, source-event, and manual recipient options in `apps/admin-web/src/features/communications/components/AudienceSelector.tsx`
- [X] T131 [US3] Implement publication status component with draft, pending approval, scheduled, published, corrected, withdrawn, expired, rejected, and review-required states in `apps/admin-web/src/features/communications/components/PublicationStatusPanel.tsx`
- [X] T132 [US3] Implement recipient snapshot viewer with inclusion, exclusion, duplicate suppression, delivery eligibility, and relationship evidence display in `apps/admin-web/src/features/communications/components/RecipientSnapshotViewer.tsx`
- [X] T133 [US3] Implement school broadcast and announcement list page in `apps/admin-web/src/app/(school)/communications/broadcasts/page.tsx`
- [X] T134 [US3] Implement school broadcast and announcement draft page in `apps/admin-web/src/app/(school)/communications/broadcasts/new/page.tsx`
- [X] T135 [US3] Implement school broadcast and announcement detail page in `apps/admin-web/src/app/(school)/communications/broadcasts/[broadcastId]/page.tsx`

**Checkpoint**: US3 is complete when broadcasts, announcements, audience snapshots, approval, scheduling, publication, correction, withdrawal, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 6: User Story 4 - Track Delivery, Reads, and Acknowledgements (Priority: P1)

**Goal**: Communication managers and authorized staff can see delivery attempts, failures, read receipts, acknowledgements, overdue recipients, retries, reminders, and waivers per recipient.

**Independent Test**: Send a required acknowledgement notification to an authorized audience, verify delivery attempts and read or acknowledgement states are recorded per recipient, and verify failed or overdue recipients appear in the correct review queue.

### Tests for User Story 4

- [X] T136 [P] [US4] Create unit tests for delivery state transitions, enabled channels, preference policy, quiet-hour policy, retries, exclusions, failures, and final states in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Delivery/DeliveryAttemptServiceTests.cs`
- [X] T137 [P] [US4] Create unit tests for read receipts, acknowledgement deadlines, overdue state, reminders, waivers, and per-recipient evidence in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Delivery/AcknowledgementTrackingServiceTests.cs`
- [X] T138 [P] [US4] Create contract tests for delivery-acknowledgement.md delivery, retry, acknowledgement, waiver, and reminder endpoints in `tests/contracts/communications/DeliveryAcknowledgementContractTests.cs`
- [X] T139 [P] [US4] Create integration tests for delivery attempts, external channel stubs, read state, acknowledgement, overdue routing, tenant isolation, feature flags, audit, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Delivery/DeliveryAcknowledgementIntegrationTests.cs`
- [X] T140 [P] [US4] Create web journey tests for delivery status, failed attempts, retries, acknowledgement dashboard, overdue recipients, reminders, and waivers in `apps/admin-web/tests/communications/deliveryAcknowledgement.spec.ts`

### Implementation for User Story 4

- [X] T141 [P] [US4] Create delivery query, delivery retry, delivery status, acknowledgement query, reminder, waiver, and response DTOs matching delivery-acknowledgement.md in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/DeliveryAcknowledgementDtos.cs`
- [X] T142 [US4] Implement communication delivery outbox service for queued delivery work, retry eligibility, duplicate attempt prevention, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/CommunicationDeliveryOutboxService.cs`
- [X] T143 [US4] Implement delivery policy service for enabled channels, mandatory categories, optional preferences, quiet hours, urgent bypass, recipient eligibility, and exclusion reasons in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/DeliveryPolicyService.cs`
- [X] T144 [US4] Implement in-app delivery service that creates delivery attempts and recipient-visible delivery state for notifications, messages, broadcasts, and announcements in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/InAppDeliveryService.cs`
- [X] T145 [US4] Implement external delivery orchestrator using school-enabled channel adapters and preserving provider references, delayed statuses, failures, and retry scheduling in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/ExternalDeliveryOrchestrator.cs`
- [X] T146 [US4] Implement delivery attempt service with attempt creation, status updates, failure reasons, exclusion reasons, final state, retry, audit, and lifecycle event creation in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/DeliveryAttemptService.cs`
- [X] T147 [US4] Implement read receipt service for notification, message, broadcast, and announcement reads with per-recipient evidence and recipient scope in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/ReadReceiptService.cs`
- [X] T148 [US4] Implement acknowledgement tracking service for required acknowledgements, deadlines, pending, read, acknowledged, overdue, waived, failed, and audit states in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/AcknowledgementTrackingService.cs`
- [X] T149 [US4] Implement overdue acknowledgement and reminder service with school policy routing, reminder eligibility, suppression, waiver, and review evidence in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/AcknowledgementReminderService.cs`
- [X] T150 [US4] Implement delivery attempt search, communication delivery status, and retry endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/DeliveryController.cs`
- [X] T151 [US4] Implement acknowledgement search, communication acknowledgement status, reminder, and waiver endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Delivery/AcknowledgementController.cs`
- [X] T152 [US4] Add delivery and acknowledgement routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T153 [US4] Implement typed web API methods for delivery attempts, retry, acknowledgement status, reminders, and waivers in `apps/admin-web/src/features/communications/api/deliveryAcknowledgementApi.ts`
- [X] T154 [US4] Implement delivery status component with recipient counts, delivered, failed, excluded, retry-scheduled, and final-state displays in `apps/admin-web/src/features/communications/components/DeliveryStatusPanel.tsx`
- [X] T155 [US4] Implement acknowledgement dashboard component with pending, read, acknowledged, overdue, waived, reminder, and per-recipient evidence displays in `apps/admin-web/src/features/communications/components/AcknowledgementDashboard.tsx`
- [X] T156 [US4] Implement school delivery status page in `apps/admin-web/src/app/(school)/communications/delivery/page.tsx`
- [X] T157 [US4] Implement school acknowledgement page in `apps/admin-web/src/app/(school)/communications/acknowledgements/page.tsx`
- [X] T158 [US4] If `apps/mobile/` exists or mobile communication surfaces are enabled, implement mobile acknowledgement screen in `apps/mobile/lib/features/communications/acknowledgement_screen.dart`

**Checkpoint**: US4 is complete when delivery attempts, retries, failures, exclusions, read state, acknowledgement deadlines, overdue handling, reminders, waivers, tenant isolation, feature gating, audit, and no-side-effect assertions pass independently.

---

## Phase 7: User Story 5 - Configure Communication Rules and Preferences (Priority: P2)

**Goal**: School administrators can configure communication capabilities, templates, audiences, channels, quiet hours, mandatory categories, moderation rules, and user preferences with version preservation.

**Independent Test**: Configure a notification category and template with default audience, channel behavior, priority, quiet-hour handling, and acknowledgement requirement; trigger an eligible event; and verify future notifications use the active configuration while historical communications retain prior rules.

### Tests for User Story 5

- [X] T159 [P] [US5] Create unit tests for communication template validation, required variables, language fallback, default audience, restricted-detail policy, acknowledgement behavior, quiet-hour behavior, channel behavior, and versioning in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Configuration/CommunicationTemplateConfigurationTests.cs`
- [X] T160 [P] [US5] Create unit tests for audience rule configuration validation, recipient criteria, recipient types, cross-school prevention, moderation requirements, and versioning in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Configuration/AudienceRuleConfigurationTests.cs`
- [X] T161 [P] [US5] Create unit tests for optional preferences, mandatory category overrides, quiet-hour rules, disabled channels, and preference policy versioning in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Configuration/CommunicationPreferencePolicyTests.cs`
- [X] T162 [P] [US5] Create contract tests for communication-configuration.md feature settings, templates, audience rules, guardian preferences, and student preferences endpoints in `tests/contracts/communications/CommunicationConfigurationContractTests.cs`
- [X] T163 [P] [US5] Create integration tests for configuration changes, template activation, audience activation, preference updates, version preservation, invalid activation rejection, audit, and historical communication rule lookup in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Configuration/CommunicationConfigurationIntegrationTests.cs`
- [X] T164 [P] [US5] Create web journey tests for administrator feature settings, template configuration, audience rules, quiet hours, moderation rules, guardian preferences, and student preferences in `apps/admin-web/tests/communications/communicationConfiguration.spec.ts`

### Implementation for User Story 5

- [X] T165 [P] [US5] Create feature setting, template, audience rule, preference, quiet-hour, channel, moderation rule, activation, archive, and version DTOs matching communication-configuration.md in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationConfigurationDtos.cs`
- [X] T166 [US5] Implement communication feature setting service for direct messaging, staff-to-guardian messaging, student messaging, broadcasts, announcements, notification center, external delivery, templates, delivery tracking, acknowledgements, preferences, history, moderation, configuration, and review summary capabilities in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationFeatureSettingService.cs`
- [X] T167 [US5] Implement communication template configuration service for draft create, update, activate, archive, variable validation, language behavior, default audience, detail minimization, acknowledgement behavior, quiet-hour behavior, channel behavior, moderation, activation rejection, and versioning in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationTemplateConfigurationService.cs`
- [X] T168 [US5] Implement audience rule configuration service for draft create, update, activate, archive, recipient criteria validation, recipient type validation, restricted-detail policy, count confirmation, moderation, cross-school prevention, and versioning in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/AudienceRuleConfigurationService.cs`
- [X] T169 [US5] Implement channel, quiet-hour, mandatory category, and moderation policy configuration helpers for feature settings and template activation in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationPolicyConfigurationService.cs`
- [X] T170 [US5] Implement communication preference service for guardian, student, staff, and school defaults with optional category handling, mandatory category preservation, quiet hours, audit, and versioning in `apps/api/src/SafeSchool.Api/Features/Communications/Preferences/CommunicationPreferenceService.cs`
- [X] T171 [US5] Implement school communication configuration endpoints for feature settings, templates, audience rules, activation, archive, and versions in `apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationConfigurationController.cs`
- [X] T172 [US5] Implement guardian communication preference read and update endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Preferences/GuardianCommunicationPreferenceController.cs`
- [X] T173 [US5] Implement student communication preference read and update endpoints where student preferences are enabled in `apps/api/src/SafeSchool.Api/Features/Communications/Preferences/StudentCommunicationPreferenceController.cs`
- [X] T174 [US5] Add configuration and preference routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T175 [US5] Implement typed web API methods for feature settings, templates, audience rules, preferences, activation, archive, and versions in `apps/admin-web/src/features/communications/api/communicationConfigurationApi.ts`
- [X] T176 [US5] Implement communication template form with category, language, title, body, variables, default audience, restricted-detail policy, acknowledgement behavior, quiet-hour behavior, channel behavior, moderation, and activation errors in `apps/admin-web/src/features/communications/components/CommunicationTemplateForm.tsx`
- [X] T177 [US5] Implement audience rule form with audience type, criteria, recipient types, restricted-detail policy, count confirmation, moderation, and activation errors in `apps/admin-web/src/features/communications/components/AudienceRuleForm.tsx`
- [X] T178 [US5] Implement communication preferences component with optional categories, mandatory category labels, channels, quiet-hour display, and disabled preference reasons in `apps/admin-web/src/features/communications/components/CommunicationPreferencesForm.tsx`
- [X] T179 [US5] Implement school communication configuration pages in `apps/admin-web/src/app/(school)/communications/configuration/page.tsx`, `apps/admin-web/src/app/(school)/communications/configuration/templates/[templateId]/page.tsx`, and `apps/admin-web/src/app/(school)/communications/configuration/audience-rules/[ruleId]/page.tsx`
- [X] T180 [US5] Implement guardian communication preferences page in `apps/admin-web/src/app/(guardian)/communications/preferences/page.tsx`
- [X] T181 [US5] Implement student communication preferences page in `apps/admin-web/src/app/(student)/communications/preferences/page.tsx`

**Checkpoint**: US5 is complete when feature settings, templates, audience rules, preferences, quiet hours, mandatory categories, moderation rules, version preservation, tenant isolation, feature gating, audit, and invalid activation assertions pass independently.

---

## Phase 8: User Story 6 - Review Communication History and Exceptions (Priority: P2)

**Goal**: Auditors, reviewers, communication managers, and administrators can search communication history, resolve exceptions, review moderation items, read summaries, and trace lifecycles without exposing restricted content.

**Independent Test**: Send a direct message, publish a broadcast, generate a system notification, create a delivery failure, and verify an authorized reviewer can trace each lifecycle while unauthorized users cannot access restricted content.

### Tests for User Story 6

- [X] T182 [P] [US6] Create unit tests for communication history filters, scoped visibility, pagination, restricted-detail filtering, and date range behavior in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/History/CommunicationHistoryQueryServiceTests.cs`
- [X] T183 [P] [US6] Create unit tests for communication review summaries, exception state transitions, source evidence preservation, and reviewer scope in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/History/CommunicationReviewSummaryExceptionTests.cs`
- [X] T184 [P] [US6] Create unit tests for moderation approval, rejection, correction, withdrawal, republish, escalation, reason requirements, and original evidence preservation in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Moderation/ModerationReviewServiceTests.cs`
- [X] T185 [P] [US6] Create contract tests for communication-history-review.md history, summaries, exceptions, moderation queue, review, and trace endpoints in `tests/contracts/communications/CommunicationHistoryReviewContractTests.cs`
- [X] T186 [P] [US6] Create integration tests for history search, summary reads, exception review, moderation review, lifecycle trace, tenant isolation, restricted visibility, audit, and no excluded-domain side effects in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/History/CommunicationHistoryReviewIntegrationTests.cs`
- [X] T187 [P] [US6] Create web journey tests for history filtering, summaries, exception review, moderation queue, reviewer correction, and auditor lifecycle trace in `apps/admin-web/tests/communications/communicationHistoryReview.spec.ts`

### Implementation for User Story 6

- [X] T188 [P] [US6] Create history query, summary, exception review, moderation queue, moderation review, lifecycle trace, and response DTOs matching communication-history-review.md in `apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationHistoryReviewDtos.cs`
- [X] T189 [US6] Implement communication history query service with filters for student, guardian, staff member, sender, recipient, audience, source module, category, priority, type, delivery state, read state, acknowledgement state, moderation state, exception type, and date range in `apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationHistoryQueryService.cs`
- [X] T190 [US6] Implement communication review summary service with permission-scoped counts for sent, delivered, failed, read, acknowledged, overdue, moderated, withdrawn, corrected, exception, pending, and aging states in `apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationReviewSummaryService.cs`
- [X] T191 [US6] Implement communication exception service for invalid recipient, invalid guardian link, inactive student, disabled feature, disabled channel, missing template, missing audience, moderation required, cross-school recipient, restricted-detail exposure risk, duplicates, delivery failure, acknowledgement overdue, and manual-review-required cases in `apps/api/src/SafeSchool.Api/Features/Communications/Moderation/CommunicationExceptionService.cs`
- [X] T192 [US6] Implement moderation review service for approval, rejection, correction, withdrawal, republish, resolve, escalate, document, reason capture, content change summary, and preserved original evidence in `apps/api/src/SafeSchool.Api/Features/Communications/Moderation/ModerationReviewService.cs`
- [X] T193 [US6] Implement communication lifecycle trace service linking source event or author, template, audience resolution, recipient snapshots, delivery attempts, read state, acknowledgement, moderation, exception, correction, summaries, lifecycle events, and audit evidence in `apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationLifecycleTraceService.cs`
- [X] T194 [US6] Implement history, summaries, exceptions, exception review, and trace endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationHistoryReviewController.cs`
- [X] T195 [US6] Implement moderation queue and moderation review endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/Moderation/CommunicationModerationController.cs`
- [X] T196 [US6] Add history, summary, exception, moderation, and trace routes to module registration in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs`
- [X] T197 [US6] Implement typed web API methods for history, summaries, exceptions, moderation queue, review actions, and lifecycle trace in `apps/admin-web/src/features/communications/api/communicationHistoryReviewApi.ts`
- [X] T198 [US6] Implement communication history table with strict visibility filtering, filters, pagination, and state indicators in `apps/admin-web/src/features/communications/components/CommunicationHistoryTable.tsx`
- [X] T199 [US6] Implement communication review summary component with sent, delivered, failed, read, acknowledged, overdue, moderated, withdrawn, corrected, exception, and pending counts in `apps/admin-web/src/features/communications/components/CommunicationReviewSummary.tsx`
- [X] T200 [US6] Implement moderation queue component with pending, approved, rejected, corrected, withdrawn, republished, escalated, and documented states in `apps/admin-web/src/features/communications/components/ModerationQueue.tsx`
- [X] T201 [US6] Implement communication lifecycle trace component with source event or author, template, audience, recipients, delivery, read, acknowledgement, moderation, exception, correction, lifecycle, and audit evidence sections in `apps/admin-web/src/features/communications/components/CommunicationLifecycleTrace.tsx`
- [X] T202 [US6] Implement school history, summaries, exceptions, moderation, and trace pages in `apps/admin-web/src/app/(school)/communications/history/page.tsx`, `apps/admin-web/src/app/(school)/communications/summaries/page.tsx`, `apps/admin-web/src/app/(school)/communications/exceptions/page.tsx`, `apps/admin-web/src/app/(school)/communications/moderation/page.tsx`, and `apps/admin-web/src/app/(school)/communications/[communicationKind]/[communicationId]/trace/page.tsx`

**Checkpoint**: US6 is complete when history search, review summaries, exceptions, moderation review, lifecycle trace, tenant isolation, restricted visibility, audit, and no-side-effect assertions pass independently.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Tighten quality, performance, security, documentation, and validation across completed user stories.

- [X] T203 [P] Add OpenAPI or route documentation for all Phase 9 `/api/v1/` endpoints in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationsOpenApi.cs`
- [X] T204 [P] Add a communication API client contract snapshot used by web and mobile clients in `apps/admin-web/src/features/communications/api/communications.contract.ts`
- [X] T205 [P] Add backend performance tests for notification list under 30 seconds, source event notification creation within 2 minutes, message send under 60 seconds, reply visibility under 30 seconds, broadcast publication under 2 minutes, delivery result recording within 5 minutes, failed and overdue lookup under 60 seconds, preference application within 2 minutes, lifecycle trace under 60 seconds, and version preservation in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationPerformanceTests.cs`
- [X] T206 [P] Add security regression tests for restricted source details, medical context, complaint context, finance context, staff-only details, safety notes, cross-school data, guardian link restrictions, student self-scope, recipient snapshots, moderation scope, and platform reviewer scope in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationSecurityRegressionTests.cs`
- [X] T207 Add an audit coverage test that verifies every audit event required by spec.md FR-023 is emitted by at least one workflow in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationAuditCoverageTests.cs`
- [X] T208 Add a no-side-effect regression test proving Phase 9 creates no attendance, gate, scan, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, document, search, or broad dashboard outcomes in `apps/api/tests/SafeSchool.Api.Tests/Features/Communications/CommunicationNoSideEffectTests.cs`
- [X] T209 [P] Add end-to-end smoke tests for notification center, direct messaging, broadcast publication, delivery status, acknowledgements, preferences, history, moderation, and configuration in `tests/e2e/communications/communications.e2e.spec.ts`
- [X] T210 [P] Add user-facing accessibility checks for notification lists, message threads, broadcast forms, audience selectors, acknowledgement dashboards, preference forms, moderation queues, and history tables in `apps/admin-web/tests/communications/communicationAccessibility.spec.ts`
- [X] T211 [P] If `apps/mobile/` exists or mobile communication surfaces are enabled, add mobile smoke tests for notifications, messages, and acknowledgements in `apps/mobile/test/features/communications/communications_smoke_test.dart`
- [X] T212 Update Phase 9 quickstart validation results and implementation notes in `specs/010-communication-notifications/quickstart.md`
- [X] T213 Update the Communications module README with commands, role matrix, capability matrix, route map, lifecycle states, delivery states, acknowledgement states, moderation states, and no-side-effect boundaries in `apps/api/src/SafeSchool.Api/Features/Communications/README.md`
- [X] T214 Add a communication privacy and restricted-detail implementation checklist for reviewers in `apps/api/src/SafeSchool.Api/Features/Communications/PrivacyReview.md`
- [X] T215 Add operational metric names and alert guidance for communication lifecycle, delivery, acknowledgements, moderation, exceptions, and source event processing in `apps/api/src/SafeSchool.Api/Features/Communications/CommunicationMetrics.md`
- [X] T216 Run all backend, contract, web, enabled mobile, and e2e communication tests and record the command set in `specs/010-communication-notifications/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1; blocks all user story work.
- **Phase 3 US1**: Depends on Phase 2; MVP and first independently useful increment.
- **Phase 4 US2**: Depends on Phase 2; can run after or in parallel with US1, but notification surfaces improve recipient awareness after US1.
- **Phase 5 US3**: Depends on Phase 2; can run after or in parallel with US1 and US2 because it owns broadcast paths and uses shared recipient snapshots.
- **Phase 6 US4**: Depends on Phase 2 and is most useful after US1, US2, or US3 create communications to track; can be implemented with seeded communications for independent testing.
- **Phase 7 US5**: Depends on Phase 2; can run in parallel with other stories because it owns configuration and preference paths, but full validation should be rerun after US1 through US4.
- **Phase 8 US6**: Depends on Phase 2 and is most useful after US1 through US4; can be implemented with seeded communications, delivery attempts, acknowledgements, and moderation records for independent testing.
- **Phase 9 Polish**: Depends on all desired user stories being complete.

### User Story Dependency Graph

```text
Setup -> Foundation -> US1 -> MVP validation
Setup -> Foundation -> US2 -> messaging validation
Setup -> Foundation -> US3 -> broadcast validation
Setup -> Foundation -> US4 -> delivery and acknowledgement validation
Setup -> Foundation -> US5 -> configuration validation
Setup -> Foundation -> US6 -> history and review validation

Recommended delivery order for one implementer:
US1 -> US2 -> US3 -> US4 -> US5 -> US6 -> Polish
```

### Within Each User Story

- Write the listed tests first and confirm they fail before implementation.
- Implement DTOs before validators and services.
- Implement validators and guards before endpoint exposure.
- Implement services before controllers.
- Implement backend endpoints before web, mobile, and e2e journeys.
- Complete tenant resolution, feature flag checks, authorization, idempotency, recipient eligibility, duplicate suppression, visibility filtering, delivery evidence, lifecycle events, audit, and no-side-effect checks before marking a story complete.

## Parallel Opportunities

- T002 through T008 can run in parallel after T001.
- T026 through T040 can run in parallel after T013 through T025 define shared enums and policies.
- T042 through T045 can run in parallel after T026 through T040.
- T051 through T054 can run in parallel after foundational paths exist.
- Test tasks at the start of each user story can run in parallel because they touch separate files.
- Web tasks and conditional mobile tasks in each user story can run after that story's backend API contract is stable.
- US5 configuration work can run in parallel with US1 through US4 after Phase 2 because it owns separate services, controllers, and UI files.
- US6 history and review work can run with seeded data after Phase 2, but final validation should run after the stories it summarizes.

## Parallel Example: User Story 1

```bash
Task: "T057 create source event validator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Notifications/NotificationSourceEventValidatorTests.cs"
Task: "T061 create contract tests in tests/contracts/communications/NotificationCenterContractTests.cs"
Task: "T063 create web journey tests in apps/admin-web/tests/communications/notificationCenter.spec.ts"
Task: "T065 create notification DTOs in apps/api/src/SafeSchool.Api/Features/Communications/Notifications/NotificationCenterDtos.cs"
```

## Parallel Example: User Story 2

```bash
Task: "T085 create participant validator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Messaging/ConversationParticipantValidatorTests.cs"
Task: "T087 create direct messaging contract tests in tests/contracts/communications/DirectMessagingContractTests.cs"
Task: "T091 create direct messaging DTOs in apps/api/src/SafeSchool.Api/Features/Communications/Messaging/DirectMessagingDtos.cs"
Task: "T104 create web API methods in apps/admin-web/src/features/communications/api/directMessagingApi.ts"
```

## Parallel Example: User Story 3

```bash
Task: "T112 create audience evaluator tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Broadcasts/AudienceRuleEvaluatorTests.cs"
Task: "T114 create broadcast contract tests in tests/contracts/communications/BroadcastAnnouncementContractTests.cs"
Task: "T117 create broadcast DTOs in apps/api/src/SafeSchool.Api/Features/Communications/Broadcasts/BroadcastAnnouncementDtos.cs"
Task: "T128 create web API methods in apps/admin-web/src/features/communications/api/broadcastAnnouncementApi.ts"
```

## Parallel Example: User Story 4

```bash
Task: "T136 create delivery attempt tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Delivery/DeliveryAttemptServiceTests.cs"
Task: "T138 create delivery contract tests in tests/contracts/communications/DeliveryAcknowledgementContractTests.cs"
Task: "T141 create delivery DTOs in apps/api/src/SafeSchool.Api/Features/Communications/Delivery/DeliveryAcknowledgementDtos.cs"
Task: "T153 create web API methods in apps/admin-web/src/features/communications/api/deliveryAcknowledgementApi.ts"
```

## Parallel Example: User Story 5

```bash
Task: "T159 create template configuration tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/Configuration/CommunicationTemplateConfigurationTests.cs"
Task: "T162 create configuration contract tests in tests/contracts/communications/CommunicationConfigurationContractTests.cs"
Task: "T165 create configuration DTOs in apps/api/src/SafeSchool.Api/Features/Communications/Configuration/CommunicationConfigurationDtos.cs"
Task: "T175 create web API methods in apps/admin-web/src/features/communications/api/communicationConfigurationApi.ts"
```

## Parallel Example: User Story 6

```bash
Task: "T182 create history query tests in apps/api/tests/SafeSchool.Api.Tests/Features/Communications/History/CommunicationHistoryQueryServiceTests.cs"
Task: "T185 create history contract tests in tests/contracts/communications/CommunicationHistoryReviewContractTests.cs"
Task: "T188 create history DTOs in apps/api/src/SafeSchool.Api/Features/Communications/History/CommunicationHistoryReviewDtos.cs"
Task: "T197 create web API methods in apps/admin-web/src/features/communications/api/communicationHistoryReviewApi.ts"
```

## Implementation Strategy

### MVP First

1. Complete Phase 1 Setup.
2. Complete Phase 2 Foundational.
3. Complete Phase 3 User Story 1.
4. Stop and validate US1 with T057 through T084 plus foundational tests.
5. Demo notification source event intake and recipient-scoped notification center before adding messaging, broadcasts, delivery dashboards, preferences, or history.

### Incremental Delivery

1. Add US1 to establish notification intake and notification center.
2. Add US2 to support scoped direct messaging.
3. Add US3 to support one-to-many broadcasts and announcements.
4. Add US4 to make delivery, read, acknowledgement, failure, and overdue evidence visible.
5. Add US5 to let schools configure templates, audiences, channels, quiet hours, preferences, and moderation rules.
6. Add US6 to provide communication history, summaries, exceptions, moderation review, and lifecycle traceability.
7. Finish Phase 9 polish and run quickstart validation.

### Notes for Lower-Cost LLM Implementation

- Read `specs/010-communication-notifications/spec.md`, `plan.md`, `data-model.md`, and the matching contract file before starting each story phase.
- Do not implement attendance, campus gate, NFC/QR scan processing, transport, wallet, learning reward, request approval, medical, emergency, complaint resolution, broad document storage, global search, or broad dashboard outcomes.
- Keep every mutation tenant-scoped, feature-gated, permission-checked, idempotent by `client_request_id` or source dedupe key, audit-visible, and lifecycle-event-aware.
- Preserve original communication evidence; implement corrections, withdrawals, moderation, and reviews as appended records.
- Keep restricted source details separate from recipient-visible summaries in backend DTOs and UI components.
- Preserve recipient snapshots at send or publication time; do not recalculate historical audiences when users view old communications.
- Treat mobile tasks as conditional: implement them only when `apps/mobile/` exists or mobile communication surfaces are enabled for Phase 9.
