# Quickstart: Phase 5 Learning & Engagement

Use this quickstart to validate that the Phase 5 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 5 scope, user stories, requirements,
  success criteria, and assumptions.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, and configuration foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 4 artifacts under `specs/005-wallet-payments/` only to preserve
  wallet and payment boundaries; Phase 5 rewards must not create wallet or
  payment outcomes.
- Read Phase 6 artifacts under `specs/007-requests-permissions/` only to
  preserve request workflow boundaries and confirm Phase 6 consumes Phase 5
  star evidence without owning star balances.
- Confirm `.specify/feature.json` points to `specs/006-learning-engagement`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes courses, learning groups,
   content, progress, assignments, submissions, quizzes, attempts, responses,
   star rules, star ledger entries, star balance snapshots, rewards,
   redemptions, behavior records, exceptions, manual reviews, configuration,
   summaries, status events, feature settings, and audit evidence.
3. Confirm [contracts/course-content-delivery.md](./contracts/course-content-delivery.md)
   covers course/group setup, content publication, student/guardian visibility,
   progress recording, and traceability.
4. Confirm [contracts/assignment-tracking.md](./contracts/assignment-tracking.md)
   covers assignment creation, submission, resubmission, grading, late handling,
   visibility, and audit expectations.
5. Confirm [contracts/quiz-engine.md](./contracts/quiz-engine.md) covers quiz
   activation, attempt start, attempt completion, scoring, feedback visibility,
   idempotency, and review behavior.
6. Confirm [contracts/star-reward-system.md](./contracts/star-reward-system.md)
   covers star rules, append-only ledger entries, balance snapshots,
   reservations, consumption, releases, reward catalog, reward redemption, and
   Phase 6 star evidence export.
7. Confirm [contracts/behavior-logging.md](./contracts/behavior-logging.md)
   covers behavior categories, behavior event creation, visibility, star impact,
   disputes, corrections, and audit expectations.
8. Confirm [contracts/learning-configuration.md](./contracts/learning-configuration.md)
   covers learning rule activation, feature settings, versioning, and
   configuration validation.
9. Confirm [contracts/learning-history-review.md](./contracts/learning-history-review.md)
   covers history search, exception review, corrections, reopenings, summaries,
   and traceability.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, guardian-link, student
   eligibility, staff assignment, idempotency, audit, and visibility guards for
   Learning workflows.
2. Create Course, Learning Group, Staff Learning Assignment, and membership
   models with tenant indexes, feature checks, and test fixtures.
3. Create Learning Content publication, visibility, progress recording, and
   trace behavior for teachers, students, and guardians.
4. Create Assignment and Assignment Submission behavior for assigned students,
   required evidence, due windows, late/excused state, grading, resubmission,
   and history preservation.
5. Create Quiz, Question, Attempt, Response, scoring, timing, feedback
   visibility, attempt limit, and review behavior.
6. Create Star Rule, append-only Star Ledger, Star Balance Snapshot, reservation
   and release, duplicate source handling, and Phase 6 star evidence export.
7. Create Reward Catalog and Reward Redemption behavior with eligibility,
   star cost validation, fulfillment state, and wallet/payment boundary guards.
8. Create Behavior Category and Behavior Event behavior with visibility,
   dispute/correction handling, and optional star impact under active rules.
9. Create Learning Exception detection for invalid enrollment, inactive
   student, missing evidence, late or duplicate work, quiz conflicts, stale
   rules, duplicate stars, insufficient stars, unavailable rewards, invalid
   behavior evidence, disabled features, and cross-school attempts.
10. Create Manual Learning Review behavior for correction, reopen, close,
    resolve, dismiss, escalate, and rule-version migration with reason and
    preserved original evidence.
11. Create Learning History and Review Summary views for school staff,
    students, guardians, reward managers, behavior reviewers, auditors, and
    platform reviewers with strict permission and tenant filtering.
12. Expose learning status events and reviewable evidence for later Phase 9
    notification capabilities without implementing notification delivery.
13. Complete unit, integration, contract, authorization, tenant-isolation,
    audit, performance, and critical UI journey tests.

## Validation Scenarios

### Course Content Delivery

- Publish an eligible content item to an active learning group and confirm
  assigned students can access it in under 2 minutes during review testing.
- Attempt to view draft, withdrawn, expired, unassigned, disabled-feature, or
  cross-school content and confirm it is blocked or hidden.
- Record student progress on a published content item and confirm teachers,
  students, and allowed guardians see the correct progress state.
- Withdraw content after students have viewed it and confirm historical
  progress remains reviewable.

### Assignment Tracking

- Create an assignment with due date, instructions, required evidence, and
  review settings for an active group.
- Submit assignment evidence as an assigned student in under 3 minutes.
- Resubmit when allowed and confirm prior evidence remains preserved.
- Grade or return a submission as an authorized teacher and confirm feedback
  visibility follows school rules.
- Attempt late, duplicate, missing-evidence, unassigned, inactive-student, or
  cross-school submissions and confirm block or review routing.

### Quiz Engine

- Activate a quiz with question set, schedule, attempt limit, scoring rule, and
  feedback visibility.
- Start and complete an eligible quiz attempt and confirm allowed results are
  visible within 1 minute.
- Attempt an expired, duplicate, over-limit, out-of-window, missing-answer,
  ineligible, or cross-school quiz completion and confirm prior attempts remain
  uncorrupted.
- Correct a question or scoring rule after attempts exist and confirm old
  attempts preserve the prior revision.

### Stars and Rewards

- Configure a star rule for assignment, quiz, content, behavior, and manual
  award sources.
- Trigger a qualifying source event and confirm an append-only star ledger entry
  and balance snapshot are produced.
- Retry the same star award and confirm no duplicate star balance impact.
- Reserve, consume, and release stars through reward redemption and confirm the
  ledger remains reconstructable.
- Redeem a reward with sufficient stars in under 1 minute and confirm
  fulfillment state is recorded.
- Attempt insufficient-star, unavailable-reward, expired-reward, duplicate, or
  cross-school redemption and confirm block or review routing.
- Confirm 95% of eligible star evidence changes are available to later Phase 6
  permission-rule consumers within 2 minutes.

### Behavior Logging

- Record a positive, corrective, and neutral behavior event for an active
  student as an authorized staff actor.
- Confirm guardian/student visibility follows category and event rules.
- Configure a behavior category with star impact and confirm the behavior event
  links to the star ledger entry.
- Dispute or correct a behavior event and confirm the original event, reason,
  actor, and resulting status remain visible to authorized reviewers.
- Attempt behavior creation for inactive, unassigned, unauthorized, duplicate,
  sensitive, or cross-school cases and confirm block or review routing.

### History, Exceptions, and Reviews

- Search learning history by student, course, assignment, quiz, star outcome,
  reward status, behavior category, date range, and exception state and confirm
  results load in under 30 seconds for the last 90 days.
- View guardian history for a linked student and confirm staff-only details are
  hidden.
- Correct, reopen, close, resolve, dismiss, escalate, or migrate a learning
  record with authorized manual review and confirm original evidence remains
  preserved.
- Trace a sampled student lifecycle from content publication through
  assignment, quiz, star, reward, behavior, exception, correction, summary, and
  audit evidence in under 60 seconds.

### Boundaries and Audit

- Confirm Phase 5 creates no attendance, campus gate, NFC/QR scan, transport,
  wallet, payment, request approval, medical, complaint, messaging, document,
  search, or broad dashboard outcome.
- Confirm 95% of eligible learning status changes are available to later
  notification capabilities within 2 minutes without Phase 5 sending
  notifications.
- Confirm audit evidence exists for course creation, content publication,
  assignment creation, assignment submission, grading, quiz activation, quiz
  attempt lifecycle, quiz scoring, star award, star correction, star
  reservation, star consumption, star release, reward redemption, reward
  fulfillment, behavior event creation, behavior correction, exception
  creation, manual review, summary read, configuration change, and access
  denial.
