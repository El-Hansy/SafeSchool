# Quickstart: Phase 9 Communication & Notifications

Use this quickstart to validate that the Phase 9 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 9 scope, user stories, requirements,
  success criteria, edge cases, and assumptions.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, and configuration foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve attendance and campus access boundaries; Phase 9 must not generate
  attendance, entry, exit, or gate outcomes.
- Read Phase 3 artifacts under `specs/004-transport-bus-tracking/` only to
  preserve transport boundaries; Phase 9 must not generate boarding, drop, ETA,
  route, or live tracking outcomes.
- Read Phase 4 artifacts under `specs/005-wallet-payments/` only to preserve
  wallet and payment boundaries; Phase 9 must not create charges, refunds,
  payments, or spending outcomes.
- Read Phase 5 artifacts under `specs/006-learning-engagement/` only to
  preserve engagement boundaries; Phase 9 must not create stars, badges,
  rewards, challenges, or learning engagement outcomes.
- Read Phase 6 artifacts under `specs/007-requests-permissions/` only to
  preserve request workflow boundaries; Phase 9 must not approve requests.
- Read Phase 7 artifacts under `specs/008-medical-emergency/` only to preserve
  medical and emergency boundaries; Phase 9 must not create medical incidents,
  emergency access, diagnoses, prescriptions, or emergency protocol outcomes.
- Read Phase 8 artifacts under `specs/009-complaints-escalations/` only to
  preserve complaint boundaries; Phase 9 must not resolve complaints or alter
  complaint lifecycle decisions.
- Confirm `.specify/feature.json` points to
  `specs/010-communication-notifications`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes conversations,
   participants, messages, broadcasts, announcements, audience rules, recipient
   snapshots, source events, notification records, templates, delivery
   attempts, preferences, acknowledgements, exceptions, moderation reviews,
   summaries, feature settings, lifecycle events, and audit evidence.
3. Confirm [contracts/notification-center.md](./contracts/notification-center.md)
   covers source event intake, notification generation, reads, read state,
   acknowledgement, duplicate suppression, visibility, and audit expectations.
4. Confirm [contracts/direct-messaging.md](./contracts/direct-messaging.md)
   covers conversation creation, participants, message send, replies,
   withdrawal, close behavior, moderation, and audit expectations.
5. Confirm [contracts/broadcast-announcement.md](./contracts/broadcast-announcement.md)
   covers drafts, approval, scheduling, publication, audience resolution,
   recipient snapshots, correction, withdrawal, and audit expectations.
6. Confirm [contracts/delivery-acknowledgement.md](./contracts/delivery-acknowledgement.md)
   covers delivery attempts, channel outcomes, retries, exclusions, read
   receipts, acknowledgements, waivers, reminders, and overdue handling.
7. Confirm [contracts/communication-configuration.md](./contracts/communication-configuration.md)
   covers feature settings, templates, audience rules, preferences, quiet
   hours, mandatory categories, moderation, versioning, and audit behavior.
8. Confirm [contracts/communication-history-review.md](./contracts/communication-history-review.md)
   covers history search, summaries, exceptions, moderation reviews, lifecycle
   trace, correction, and privacy filtering.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, guardian-link, student
   eligibility, recipient eligibility, sender authority, audience authority,
   idempotency, audit, privacy, and visibility guards for Communications
   workflows.
2. Create conversation, participant, message, broadcast, announcement,
   audience rule, recipient snapshot, source event, notification, template,
   delivery attempt, preference, acknowledgement, exception, moderation review,
   summary, feature setting, lifecycle event, and audit models with tenant
   indexes and migrations.
3. Create Notification Center behavior for source event intake, duplicate
   suppression, template resolution, recipient resolution, notification
   creation, read state, acknowledgement, and restricted-detail minimization.
4. Create Direct Messaging behavior for conversation creation, participant
   management, replies, read state, acknowledgement requirements, moderation
   routing, withdrawal, closure, and source context references.
5. Create Broadcast and Announcement behavior for drafts, audience rules,
   recipient snapshots, approval, scheduled publication, immediate
   publication, correction, withdrawal, and duplicate recipient suppression.
6. Create Delivery and Acknowledgement behavior for in-app delivery,
   school-enabled external channel attempts, failures, retries, exclusions,
   read receipts, acknowledgement deadlines, overdue state, reminders, and
   waivers.
7. Create Preferences and Configuration behavior for feature settings,
   templates, audience rules, notification rules, channel rules, quiet hours,
   mandatory categories, moderation rules, and version preservation.
8. Create Communication Exception detection for invalid recipients, invalid
   guardian links, inactive students, disabled features, disabled channels,
   missing templates, missing audiences, moderation required, cross-school
   recipients, restricted-detail risks, duplicates, delivery failures,
   overdue acknowledgements, and manual-review-required cases.
9. Create Moderation Review behavior for approval, rejection, correction,
   withdrawal, republish, escalation, and exception resolution with reason and
   preserved original evidence.
10. Create Communication History and Review Summary views for school staff,
    communication managers, moderators, guardians, students, reviewers,
    auditors, and platform reviewers with strict permission and tenant
    filtering.
11. Expose communication lifecycle events and reviewable evidence for later
    operational monitoring without implementing broad Phase 11 dashboards.
12. Complete unit, integration, contract, authorization, tenant-isolation,
    feature-flag, idempotency, delivery, acknowledgement, moderation, audit,
    performance, and critical UI journey tests.

## Validation Scenarios

### Notification Center

- Generate a notification from an eligible source event and confirm the
  authorized guardian, student where enabled, and relevant staff can view it
  inside the correct school account.
- Open notification lists and unread counts in under 30 seconds during review
  testing.
- Confirm 95% of eligible source events create recipient-visible notification
  records within 2 minutes.
- Correct or withdraw an eligible notification and confirm original title,
  original summary, corrected content, actor, time, reason, recipient impact,
  audit evidence, and lifecycle events remain preserved.
- Confirm system notifications expose reply policy and support action metadata,
  and that no-reply notifications reject reply attempts without mutating the
  originating source workflow.
- Attempt cross-school, invalid guardian link, inactive student, disabled
  feature, restricted detail, duplicate source event, and unsupported category
  notification flows and confirm block, suppression, minimization, or review
  routing.

### Direct Messaging

- Send a scoped direct message from authorized staff to a permitted guardian,
  student where enabled, staff member, or role group in under 60 seconds.
- Reply to an eligible conversation and confirm the reply appears in the
  correct thread in under 30 seconds.
- Confirm unrelated users, cross-school users, inactive participants, disabled
  student messaging, closed threads, no-reply notifications, and moderation
  pending states cannot send or view unauthorized content.
- Withdraw or correct an eligible message and confirm original content,
  corrected content, actor, time, reason, and recipient impact remain
  preserved.

### Broadcasts and Announcements

- Publish a complete broadcast or announcement to a permitted audience in under
  2 minutes during review testing.
- Resolve audiences by whole school, guardians, students, staff, grade, class,
  route, activity group, role group, source-event recipients, and manual
  permitted recipients.
- Confirm 100% of sampled audience rules resolve only recipients inside the
  authorized school account and approved relationship, role, student
  ownership, or assignment scope.
- Withdraw, correct, or republish after some recipients have read or
  acknowledged the communication and confirm original evidence remains
  preserved.

### Delivery and Acknowledgement

- Confirm 95% of enabled delivery channel attempts record success, failure,
  exclusion, or retry-needed state within 5 minutes.
- Identify failed, overdue, unacknowledged, and review-required communications
  in under 60 seconds during review testing.
- Acknowledge a required communication and confirm recipient, time, deadline,
  reminder state, overdue state, and reviewer-visible evidence are preserved.
- Retry a failed delivery and confirm the original failure remains visible.
- Waive an acknowledgement with reviewer authority and confirm a reason is
  required.

### Configuration, Preferences, and Moderation

- Activate a template with category, language, required variables, default
  audience, detail minimization, priority, acknowledgement behavior,
  quiet-hour behavior, and channel behavior.
- Update optional preferences and confirm newly generated optional
  communications reflect the change within 2 minutes.
- Confirm mandatory school safety, security, attendance, medical, emergency,
  and legal categories remain governed by school policy even when optional
  preferences would disable delivery.
- Configure quiet hours and confirm optional communications are delayed while
  urgent or mandatory communications follow school policy.
- Route high-risk content, large audiences, restricted content, and configured
  categories through moderation and confirm no recipient receives pending
  content early.

### History, Exceptions, and Reviews

- Search communication history by student, guardian, staff member, sender,
  recipient, audience, source module, category, priority, type, delivery
  state, read state, acknowledgement state, moderation state, exception type,
  and date range.
- View guardian or student communication history and confirm restricted source
  details, reviewer-only content, staff-only details, and records outside
  approved links or self-scope are hidden.
- Resolve, dismiss, escalate, correct, withdraw, approve, reject, or republish
  a communication exception with authorized review and confirm original
  evidence remains preserved.
- Trace a sampled communication from source event or author through template,
  audience resolution, recipient snapshot, delivery attempts, read state,
  acknowledgement, moderation, exception, correction, summary, lifecycle
  events, and audit evidence in under 60 seconds.

### Boundaries and Audit

- Confirm Phase 9 creates no attendance, campus gate, NFC/QR scan, transport,
  wallet, learning reward, request approval, medical, emergency, complaint
  resolution, document storage, global search, or broad dashboard outcome.
- Confirm notifications identify their source module and whether replies are
  allowed.
- Confirm audit evidence exists for message draft, send, edit where allowed,
  withdrawal, reply, recipient selection, recipient exclusion, broadcast draft,
  approval, publication, announcement update, notification generation,
  duplicate suppression, delivery attempt, success, failure, retry, read state
  change, acknowledgement, acknowledgement expiry, preference change, template
  change, moderation decision, exception creation, summary read,
  configuration change, and access denial.
