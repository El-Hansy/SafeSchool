# Quickstart: Phase 6 Requests & Permissions

Use this quickstart to validate that the Phase 6 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 6 scope, clarifications, and user
  stories.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, and configuration foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve attendance and campus access boundaries; Phase 6 must not generate
  attendance or gate outcomes.
- Read Phase 3 artifacts under `specs/004-transport-bus-tracking/` only to
  preserve transport boundaries; Phase 6 must not generate boarding, drop, or
  live tracking outcomes.
- Read Phase 5 star and reward artifacts when implementing star-based
  permission rules; Phase 6 consumes Phase 5 star evidence but does not own
  star balances.
- Confirm `.specify/feature.json` points to
  `specs/007-requests-permissions`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes permission requests,
   outing details, early leave details, request types, workflow templates,
   workflow steps, workflow decisions, guardian consent, star rules, star
   evaluations, pickup evidence, exceptions, manual reviews, feature settings,
   and review summaries.
3. Confirm [contracts/outing-permission-requests.md](./contracts/outing-permission-requests.md)
   covers request submission, tracking, withdrawal, duplicate blocking,
   overlap review, guardian/student/staff visibility, and audit expectations.
4. Confirm [contracts/approval-workflow.md](./contracts/approval-workflow.md)
   covers assigned workflow decisions, idempotency, expiry routing, delegation,
   escalation, and decision traceability.
5. Confirm [contracts/early-leave.md](./contracts/early-leave.md) covers early
   leave details, guardian-selected pickup person, staff verification note,
   release eligibility, expiry, and no attendance/gate/scan side effects.
6. Confirm [contracts/star-permission-rules.md](./contracts/star-permission-rules.md)
   covers Phase 5 star evidence, star reservation, consumption, release,
   missing evidence behavior, and no Phase 6 star balance ownership.
7. Confirm [contracts/request-configuration.md](./contracts/request-configuration.md)
   covers request types, required fields, workflow templates, consent rules,
   star rule attachment, expiry, duplicate policy, and versioning.
8. Confirm [contracts/request-history-review.md](./contracts/request-history-review.md)
   covers request history, guardian/student privacy-limited history,
   exceptions, manual reviews, corrections, reopenings, summaries, and
   traceability.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, guardian-link, student
   eligibility, request idempotency, audit, and visibility guards for Requests
   workflows.
2. Create Request Type and Workflow Template models, migrations, versioning,
   activation validation, feature capability checks, and configuration
   contracts.
3. Create Permission Request submission, draft, status, required-field,
   duplicate blocking, overlap review, withdrawal, and trace behavior for
   student, guardian, and staff initiators.
4. Create Approval Workflow step assignment, decision handling, idempotent
   decision commands, out-of-order protection, delegation, escalation, expiry
   routing, and audit evidence.
5. Create Guardian Consent behavior with default one-authorized-guardian
   consent and stricter request-type override support.
6. Create Early Leave details, pickup evidence, staff verification note,
   release eligibility read model, expiry rules, and no attendance/gate/scan
   side effects.
7. Create Outing details, destination/purpose, departure/return windows,
   supervision or transport expectation fields, closure expectations, and
   overdue closure review.
8. Create Star Permission Rule configuration and Phase 5 evidence integration
   for reservation at submission, consumption on approval, release on denial,
   withdrawal, or expiry, and missing evidence behavior.
9. Create Request Exception detection for invalid guardian link, expired
   approval, exact duplicate, overlap review, conflicting decisions, stale
   workflow version, missing stars, insufficient stars, invalid pickup
   evidence, disabled feature, and cross-school attempts.
10. Create Manual Request Review behavior for correction, reopen, close,
    resolve, dismiss, escalate, and workflow version migration with reason and
    preserved original evidence.
11. Create Request History and Review Summary views for school staff,
    guardians, students, approvers, auditors, and platform reviewers with
    strict permission and tenant filtering.
12. Expose request status events and reviewable evidence for later Phase 9
    notification capabilities without implementing notification delivery.
13. Complete unit, integration, contract, authorization, tenant-isolation,
    audit, and critical UI journey tests.

## Validation Scenarios

### Request Submission and Tracking

- Submit an eligible outing request as a guardian for a linked active student
  and confirm it enters the configured workflow in under 2 minutes during
  review testing.
- Submit an eligible request as a student only for a request type that allows
  student initiation.
- Attempt submission with missing required fields, inactive student,
  unauthorized actor, unlinked guardian, disabled feature, or cross-school
  student and confirm each is blocked without exposing unrelated records.
- Submit an exact active duplicate and confirm it is blocked.
- Submit an overlapping non-identical active request and confirm it routes to
  manual review.
- Withdraw a non-final request as the original requester and confirm pending
  approval work stops while history remains visible.

### Approval Workflow

- Configure and activate a workflow with guardian consent and staff approval
  steps.
- Approve, deny, request information, delegate, escalate, and mark manual
  review according to assigned step permissions.
- Retry the same decision command and confirm the same outcome is returned
  without duplicate decisions.
- Attempt unauthorized, out-of-order, cross-school, withdrawn, expired, and
  final-state decisions and confirm they are rejected or routed to review
  without corrupting history.
- Expire a workflow step and confirm the request remains pending while routing
  to manual review or configured escalation with no automatic approval or
  denial.

### Guardian Consent

- Require guardian consent for a request type and confirm one authorized
  guardian can satisfy consent by default.
- Configure a stricter consent rule for a request type and confirm the request
  waits until that stricter rule is satisfied or routes to review.
- Attempt consent from a pending, suspended, expired, rejected, or cross-school
  guardian link and confirm denial with audit evidence.

### Early Leave

- Submit an early leave request with release time, reason, guardian consent
  where required, guardian-selected authorized pickup person, and pickup
  evidence.
- Approve the request and record a staff verification note for the pickup
  person.
- Confirm release eligibility is visible to authorized attendance, gate, or
  school staff within 1 minute of final approval and staff verification.
- Confirm release eligibility reads create no attendance, entry, exit, scan, or
  transport outcomes.
- Attempt release eligibility with unlinked guardian, unauthorized pickup,
  missing staff verification note, expired approval, invalid student, disabled
  capability, or cross-school reference and confirm block or manual review.

### Outing Requests

- Submit an outing request with destination or purpose, departure and return
  window, supervision or transport expectation when required, and guardian
  consent when required.
- Approve the outing and confirm closure or return status is required only when
  the request type requires it.
- Mark an approved outing overdue when the time window ends without required
  closure and confirm review evidence is created.

### Star-Based Permission Rules

- Configure a star-gated request type with required threshold and star cost.
- Submit a request for a student with sufficient Phase 5 star evidence and
  confirm stars are reserved at submission.
- Approve the request and confirm reserved stars are consumed through Phase 5
  outcome reference.
- Deny, withdraw, or expire a star-gated request and confirm the star
  reservation is released.
- Submit requests with insufficient, stale, unavailable, disabled, or corrected
  Phase 5 star evidence and confirm the configured failure behavior is applied
  without inventing a star balance.
- Change the star rule after submission and confirm historical requests keep
  the rule version and outcome used at submission time.

### History, Exceptions, and Reviews

- Search request history by student, request type, status, date range,
  approver, workflow version, exception state, and star-rule outcome and
  confirm results load in under 30 seconds for the last 90 days during review
  testing.
- View guardian history for a linked student and confirm it hides staff-only
  assignment and internal review details.
- View student history and confirm only the student's own eligible records are
  visible.
- Correct, reopen, close, resolve, dismiss, escalate, or migrate a request with
  authorized manual review and confirm the original request and decision
  history remain preserved.
- Trace a sampled request from creation through final decision, exception,
  correction, summary, and audit evidence in under 60 seconds.

### Boundaries and Audit

- Confirm Phase 6 creates no attendance, campus gate, NFC/QR scan, transport,
  wallet, learning, medical, complaint, messaging, document, search, or broad
  dashboard outcome.
- Confirm 95% of eligible status changes are available to later notification
  capabilities within 2 minutes without Phase 6 sending notifications.
- Confirm audit evidence exists for creation, submission, withdrawal, approval,
  denial, information request, escalation, delegation, expiration, correction,
  reopening, star evaluation, guardian decision, early leave release
  eligibility changes, workflow template changes, exception creation, manual
  review, and access denial.
