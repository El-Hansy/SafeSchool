# Quickstart: Phase 8 Complaints & Escalations

Use this quickstart to validate that the Phase 8 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 8 scope, user stories, requirements,
  success criteria, edge cases, and assumptions.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, and configuration foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve attendance and campus access boundaries; Phase 8 must not generate
  attendance, entry, exit, or gate outcomes.
- Read Phase 3 artifacts under `specs/004-transport-bus-tracking/` only to
  preserve transport boundaries; Phase 8 must not generate boarding, drop, or
  live tracking outcomes.
- Read Phase 4 artifacts under `specs/005-wallet-payments/` only to preserve
  wallet and payment boundaries; Phase 8 must not create charges, refunds,
  payments, or spending outcomes.
- Read Phase 5 artifacts under `specs/006-learning-engagement/` only to
  preserve engagement boundaries; Phase 8 must not create stars, badges,
  rewards, challenges, or learning engagement outcomes.
- Read Phase 6 artifacts under `specs/007-requests-permissions/` only to
  preserve request workflow boundaries; Phase 8 must not approve requests.
- Read Phase 7 artifacts under `specs/008-medical-emergency/` only to preserve
  medical and emergency boundaries; Phase 8 must not create medical incidents,
  emergency access, diagnoses, prescriptions, or emergency protocol outcomes.
- Confirm `.specify/feature.json` points to `specs/009-complaints-escalations`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes complaints, categories,
   participants, assignments, investigation entries, evidence references,
   escalation rules, escalation events, resolutions, feedback, exceptions,
   manual reviews, summaries, status events, feature settings, and audit
   evidence.
3. Confirm [contracts/complaint-submission.md](./contracts/complaint-submission.md)
   covers guardian, student, and staff complaint submission, tracking,
   withdrawal, duplicate handling, visibility, and audit expectations.
4. Confirm [contracts/complaint-categorization.md](./contracts/complaint-categorization.md)
   covers category rules, reclassification, assignment, priority,
   confidentiality, conflict-of-interest handling, and audit expectations.
5. Confirm [contracts/escalation-workflow.md](./contracts/escalation-workflow.md)
   covers rule-based and manual escalation, high-risk routing, target expiry,
   conflicted owner handling, and manual review fallback.
6. Confirm [contracts/feedback-resolution.md](./contracts/feedback-resolution.md)
   covers investigation entries, visible responses, resolution records,
   feedback, reopen requests, and preserved evidence.
7. Confirm [contracts/complaint-configuration.md](./contracts/complaint-configuration.md)
   covers feature settings, category settings, target timings, escalation
   rules, feedback and reopen rules, and versioning.
8. Confirm [contracts/complaint-history-review.md](./contracts/complaint-history-review.md)
   covers history search, summaries, exceptions, manual reviews, corrections,
   lifecycle trace, and privacy filtering.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, guardian-link, student
   eligibility, complainant ownership, assignment, conflict-of-interest,
   idempotency, audit, privacy, and visibility guards for Complaints workflows.
2. Create complaint, category, participant, assignment, investigation entry,
   evidence reference, escalation rule, escalation event, resolution, feedback,
   exception, manual review, summary, status event, feature setting, and audit
   models with tenant indexes and migrations.
3. Create Complaint Submission behavior for guardian, student, and staff
   intake, required fields, tracking references, status visibility, duplicate
   handling, withdrawal, and source evidence references.
4. Create Complaint Categorization behavior for category rule evaluation,
   priority, confidentiality, reclassification, assignment, owner routing,
   conflict-of-interest detection, and manual review fallback.
5. Create Escalation Workflow behavior for high-risk, overdue, disputed,
   unresolved, repeatedly reopened, externally reportable, and conflicted
   complaints, including target expiry routing and urgent review.
6. Create Investigation and Resolution behavior for internal notes,
   complainant-visible responses, requested information, corrective actions,
   resolution records, closure, and preserved evidence.
7. Create Feedback and Reopen behavior for acceptance, dissatisfaction,
   ratings, comments, disputes, reopen requests, configured routing, and
   original resolution preservation.
8. Create Complaint Exception detection for invalid student, invalid guardian
   link, missing fields, disabled feature, disabled category, missing owner,
   missing escalation route, conflicted owner, stale configuration, duplicates,
   restricted evidence references, missed targets, cross-school access, and
   manual-review-required cases.
9. Create Manual Complaint Review behavior for correction, reopen, dismiss,
   escalate, resolve, close, rule version migration, and restricted visibility
   exceptions with reason and preserved original evidence.
10. Create Complaint History and Review Summary views for school staff,
    guardians, students, complaint managers, reviewers, auditors, and platform
    reviewers with strict permission and tenant filtering.
11. Expose complaint status events and reviewable evidence for later Phase 9
    communication capabilities without implementing notification delivery.
12. Complete unit, integration, contract, authorization, tenant-isolation,
    feature-flag, audit, status-event, performance, and critical UI journey
    tests.

## Validation Scenarios

### Complaint Submission

- Submit a complete eligible complaint as a guardian, student, and staff member
  in under 2 minutes during review testing.
- Track a submitted complaint and confirm the complainant sees current status,
  visible responses, next expected action, and allowed resolution outcome.
- Attempt unauthorized, cross-school, inactive student, unlinked guardian,
  disabled feature, disabled category, missing required field, exact duplicate,
  and overlapping non-identical submissions and confirm block, existing
  reference, link, group, or manual review routing.
- Withdraw an eligible complaint and confirm investigation or escalation work
  stops only where school rules allow.

### Categorization and Assignment

- Categorize, prioritize, and assign a newly submitted complaint in under 60
  seconds during review testing.
- Confirm category rules determine required fields, confidentiality, default
  priority, owner group, target response timing, escalation route, and feedback
  behavior.
- Reclassify a complaint and confirm reason, prior category, new category,
  priority, confidentiality, owner, target timing, and audit evidence are
  preserved.
- Assign a complaint involving a potential complaint subject and confirm
  conflicted users cannot self-assign, decide, close, or view restricted
  complainant details.

### Investigation, Resolution, and Feedback

- Record an internal investigation note, requested information, complainant
  response, visible response, corrective action, and status change.
- Resolve a complaint with resolution type, visible outcome summary,
  corrective action where applicable, closure reason, actor, time, feedback
  eligibility, and reopen eligibility.
- Confirm internal notes, restricted details, conflict review notes, and
  staff-only corrective action notes are hidden from complainant-facing views.
- Submit acceptance, dissatisfaction, rating, comment, dispute, and reopen
  request according to configured feedback and reopen rules.

### Escalation

- Escalate high-priority, safety-related, safeguarding-related, severe
  misconduct-related, externally reportable, overdue, disputed, unresolved,
  repeatedly reopened, and conflicted complaints to the configured owner or
  reviewer.
- Confirm 95% of eligible escalation events are visible to the configured
  owner or reviewer within 1 minute.
- Let a target response or resolution window expire and confirm the complaint
  routes to configured escalation or manual review without silent closure,
  dismissal, downgrade, or hiding.
- Submit a complaint that appears to require urgent safety, medical, emergency,
  or external authority action and confirm urgent review routing without
  creating medical, emergency, or external authority outcomes.

### History, Exceptions, and Reviews

- Search complaint history by student, complainant, guardian, staff submitter,
  category, priority, confidentiality, status, owner, escalation state, date
  range, target timing state, feedback state, exception type, and review state.
- View guardian or student complaint history and confirm restricted internal
  notes, conflicted-party details, safety review notes, and staff-only details
  are hidden.
- Correct, reopen, dismiss, escalate, resolve, close, or migrate a complaint
  with authorized manual review and confirm original evidence remains
  preserved.
- Trace a sampled complaint from submission through categorization, assignment,
  investigation, escalation, resolution, feedback, reopen, correction, summary,
  and audit evidence in under 60 seconds.

### Boundaries and Audit

- Confirm Phase 8 creates no attendance, campus gate, NFC/QR scan, transport,
  wallet, learning reward, request approval, medical, emergency, broad
  messaging, document, search, or broad dashboard outcome.
- Confirm 95% of eligible complaint status changes, escalation events,
  feedback submissions, and reopen requests are available to later
  communication and notification capabilities within 2 minutes without Phase 8
  delivering general messages directly.
- Confirm audit evidence exists for complaint creation, submission,
  withdrawal, categorization, reclassification, assignment, reassignment,
  priority change, confidentiality change, information request, complainant
  response, internal note, visible response, escalation, missed target,
  conflict-of-interest detection, resolution proposal, closure, feedback
  submission, reopen request, dispute, correction, exception creation, manual
  review, summary read, configuration change, and access denial.
