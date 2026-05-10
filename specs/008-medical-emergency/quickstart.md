# Quickstart: Phase 7 Medical & Emergency

Use this quickstart to validate that the Phase 7 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 7 scope, clarifications, user stories,
  requirements, success criteria, and assumptions.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, API, and configuration foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, roles, permissions, and guardian visibility rules.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve attendance and campus access boundaries; Phase 7 must not generate
  attendance, entry, exit, or gate outcomes.
- Read Phase 3 artifacts under `specs/004-transport-bus-tracking/` only to
  preserve transport boundaries; Phase 7 must not generate boarding, drop, or
  live tracking outcomes.
- Read Phase 4 artifacts under `specs/005-wallet-payments/` only to preserve
  wallet and payment boundaries; Phase 7 must not create charges, refunds,
  payments, or spending outcomes.
- Read Phase 5 artifacts under `specs/006-learning-engagement/` only to
  preserve engagement boundaries; Phase 7 must not create stars, badges,
  rewards, challenges, or learning engagement outcomes.
- Read Phase 6 artifacts under `specs/007-requests-permissions/` only to
  preserve request workflow boundaries; Phase 7 must not approve requests.
- Confirm `.specify/feature.json` points to `specs/008-medical-emergency`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes student medical profiles,
   medical conditions, allergies, medication instructions, care plans,
   emergency contacts, consent records, guardian updates, emergency access,
   break-glass events, offline cache access, incidents, care actions,
   notification requests, contact attempts, exceptions, manual reviews, rule
   settings, summaries, status events, feature settings, and audit evidence.
3. Confirm [contracts/medical-records.md](./contracts/medical-records.md)
   covers profile management, guardian updates, review, visibility filtering,
   duplicate handling, and audit expectations.
4. Confirm [contracts/emergency-access.md](./contracts/emergency-access.md)
   covers emergency profile access, break-glass, 30-minute expiry, 24-hour
   offline cache freshness, stale-cache review, and no scan side effects.
5. Confirm [contracts/medical-incident-logging.md](./contracts/medical-incident-logging.md)
   covers incident creation, severity, care actions, medication evidence,
   closure, corrections, duplicate handling, and audit expectations.
6. Confirm [contracts/medical-notifications.md](./contracts/medical-notifications.md)
   covers medical notification requests, high-severity default audience,
   privacy-safe summaries, contact attempts, acknowledgements, and status event
   boundaries.
7. Confirm [contracts/medical-configuration.md](./contracts/medical-configuration.md)
   covers feature settings, rule settings, break-glass roles, emergency session
   duration, offline cache freshness, visibility, notification audience, and
   versioning.
8. Confirm [contracts/medical-history-review.md](./contracts/medical-history-review.md)
   covers history search, exceptions, manual reviews, corrections, summaries,
   lifecycle trace, and privacy filtering.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, guardian-link, student
   eligibility, medical assignment, emergency role, idempotency, audit,
   privacy, and visibility guards for Medical workflows.
2. Create medical profile, condition, allergy, medication instruction, care
   plan, emergency contact, consent, guardian update, rule setting, status
   event, and audit models with tenant indexes and migrations.
3. Create Medical Record behavior for staff profile management, guardian
   update submission, pending medical review, guardian/student visibility, and
   profile traceability.
4. Create Emergency Access behavior for critical views, minimum-necessary
   filtering, 30-minute session expiry, re-confirmation, break-glass restricted
   to pre-authorized emergency roles, mandatory review, and access audit.
5. Create optional Offline Emergency Cache behavior for emergency essentials,
   24-hour freshness, stale warning, reason capture, syncable access evidence,
   and no unrelated browsing.
6. Create Medical Incident behavior for incident logging, severity escalation,
   care action timelines, medication administration evidence, follow-up,
   closure, duplicate handling, and corrections.
7. Create Medical Notification behavior for notification requests,
   high-severity default audience, privacy-safe summaries, contact attempts,
   acknowledgements, failed contacts, and status events for later Phase 9
   delivery.
8. Create Medical Exception detection for invalid student, inactive student,
   expired instruction, conflicting medical record, missing consent, missing
   emergency reason, duplicate incident, duplicate notification, failed
   contact, stale cache, disabled feature, cross-school access, and
   manual-review-required cases.
9. Create Manual Medical Review behavior for correction, reopen, close,
   resolve, dismiss, escalate, and rule version migration with reason and
   preserved original evidence.
10. Create Medical History and Review Summary views for school staff,
    guardians, students, emergency staff, reviewers, auditors, and platform
    reviewers with strict permission and tenant filtering.
11. Expose medical status events and reviewable evidence for later Phase 9
    notification capabilities without implementing notification delivery.
12. Complete unit, integration, contract, authorization, tenant-isolation,
    offline-cache, audit, performance, and critical UI journey tests.

## Validation Scenarios

### Medical Records

- Create or update a complete student medical profile with condition, allergy,
  medication instruction, care plan, emergency contact, and consent details in
  under 3 minutes during review testing.
- Submit a guardian medical update and confirm it remains pending medical
  review before school-verified use.
- Accept, reject, partially accept, or request information for a guardian
  update and confirm original submission evidence is preserved.
- Attempt unauthorized, cross-school, expired guardian link, disabled-feature,
  restricted visibility, duplicate, and conflicting profile operations and
  confirm block, hidden result, idempotent result, or review routing.

### Emergency Access

- Open a critical emergency profile for an active student in under 30 seconds
  and confirm only active emergency-relevant allergies, medication
  instructions, care plans, restrictions, contacts, and recent relevant
  incidents are visible.
- Start break-glass access as a pre-authorized emergency role and confirm
  mandatory review evidence is created.
- Attempt break-glass as staff assigned to a class, gate, trip, or bus without
  a pre-authorized emergency role and confirm denial without exposing records.
- Confirm emergency access sessions expire after 30 minutes and require
  re-confirmation for continued access.
- Access cached emergency data synced within 24 hours and confirm freshness is
  displayed.
- Attempt stale cache access older than 24 hours and confirm warning,
  required reason, review routing, and syncable audit evidence.
- Confirm emergency access creates no scan, attendance, campus gate, transport,
  wallet, request, complaint, document, search, or messaging outcome.

### Medical Incidents

- Log a complete medical incident with severity, observation, care action,
  contact requirement, and follow-up state in under 2 minutes.
- Add care actions for observation, first aid, medication administration,
  emergency services handoff, guardian contact, and follow-up.
- Record medication administration with an active medication instruction or an
  authorized override reason.
- Attempt medication evidence with missing consent, expired instruction, wrong
  student, unauthorized actor, disabled feature, duplicate care action, or
  conflicting care evidence and confirm block or review routing.
- Correct, dispute, reopen, close, or escalate an incident and confirm original
  incident and care action evidence remains preserved.

### Medical Notifications

- Create a notification request from a high-severity incident and confirm the
  default audience includes approved guardians, emergency contacts, assigned
  nurse or clinic staff, and the school emergency coordinator.
- Confirm high-severity notification requests are available to responsible
  staff or later notification capabilities within 2 minutes of incident
  classification.
- Record manual call, alternate contact, failed contact, delayed
  acknowledgement, and acknowledgement outcomes.
- Attempt invalid guardian link, expired contact, duplicate notification,
  closed incident, sensitive over-sharing, disabled feature, and broad
  broadcast behavior and confirm block, minimization, idempotent result, or
  review routing.

### History, Exceptions, and Reviews

- Search medical history by student, condition, allergy, medication
  instruction, care plan, emergency access event, incident severity, care
  action, medication evidence, notification state, contact acknowledgement,
  date range, actor, exception type, and review state.
- View guardian history for a linked student and confirm staff-only notes,
  restricted details, disputed records, and internal review assignments are
  hidden.
- Correct, reopen, close, resolve, dismiss, escalate, or migrate a medical
  record with authorized manual review and confirm original evidence remains
  preserved.
- Trace a sampled medical lifecycle from profile update through emergency
  access, incident logging, care action, notification request,
  acknowledgement, correction, summary, and audit evidence in under 60 seconds.

### Boundaries and Audit

- Confirm Phase 7 creates no attendance, campus gate, NFC/QR scan, transport,
  wallet, learning reward, request approval, complaint, broad messaging,
  document, search, or broad dashboard outcome.
- Confirm Phase 7 does not diagnose, prescribe, dispense medication, change
  medication instructions without authorized human input, or replace emergency
  protocols.
- Confirm 95% of eligible medical status changes are available to later
  communication and notification capabilities within 2 minutes without Phase 7
  delivering general messages directly.
- Confirm audit evidence exists for profile creation, profile update, guardian
  update submission, consent change, emergency access, break-glass access,
  emergency denial, incident creation, care action logging, medication
  evidence, notification request, contact attempt, acknowledgement, failed
  contact, correction, closure, exception creation, manual review, summary
  read, configuration change, and access denial.
