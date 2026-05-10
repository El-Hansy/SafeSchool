# Research: Phase 7 Medical & Emergency

## Decision: Keep Phase 7 bounded to school medical evidence and emergency workflows

**Rationale**: `PLAN.md` assigns Phase 7 to Medical Record, Emergency Access,
Medical Incident Logging, and Medical Notification. The spec also excludes
attendance, campus access, transport, wallet, learning reward, request
approval, complaint escalation, broad messaging, document storage, global
search, diagnosis, prescription, and broad dashboards. Keeping the boundary
explicit prevents Phase 7 from becoming a clinical, pharmacy, dispatch,
document, or messaging platform.

**Alternatives considered**:
- Implement a full clinical record or diagnosis workflow: rejected because the
  phase records school-held medical evidence and must not replace medical
  judgment.
- Implement ambulance dispatch or emergency services integration: rejected
  because the spec excludes external emergency services integration.
- Implement notification delivery in Phase 7: rejected because Phase 9 owns
  general messaging and notification delivery.

## Decision: Use the modular monolith Medical feature area

**Rationale**: The constitution favors a modular monolith and single
PostgreSQL database unless measured pressure justifies extraction. Medical
workflows share tenant, identity, guardian visibility, audit, and feature
configuration foundations with earlier phases. A Medical feature area keeps
cost low while isolating sensitive health logic by module.

**Alternatives considered**:
- Separate medical microservice: rejected because the current phase does not
  justify added operational cost.
- External student health record system as the source of truth: rejected
  because Phase 7 needs school-owned evidence and emergency workflows.
- Store medical records in a separate database: rejected because tenant,
  audit, and permission enforcement belong in the platform boundary.

## Decision: Depend on Phase 0 and Phase 1 foundations

**Rationale**: Phase 7 records are tenant-owned and student-centric. Medical
visibility, guardian views, break-glass access, staff role authority, and audit
behavior depend on Phase 0 tenant/capability/audit infrastructure and Phase 1
student, guardian, role, and permission records.

**Alternatives considered**:
- Duplicate student or guardian records inside Medical: rejected because it
  creates divergent authority and cross-tenant risk.
- Allow UI-only feature gates: rejected because backend feature enforcement is
  mandatory under the constitution.

## Decision: Guardian-submitted updates always enter pending medical review

**Rationale**: Medical data can affect emergency care. The clarification
requires all guardian-submitted updates to be reviewed by authorized school
medical staff before becoming school-verified medical evidence.

**Alternatives considered**:
- Directly apply low-risk guardian updates: rejected because field categories
  can be misclassified and still affect emergency response.
- Make direct-update fields school-configurable: rejected for Phase 7 because
  it increases review complexity before safety controls exist.
- Disallow guardian updates entirely: rejected because guardians need a channel
  to provide medical changes.

## Decision: Use minimum-necessary emergency access views

**Rationale**: Emergency users need fast access to critical allergies,
medication instructions, care plans, restrictions, contacts, and recent
relevant incidents. They do not need full medical history, staff-only notes, or
records unrelated to immediate response.

**Alternatives considered**:
- Show full medical profiles during emergencies: rejected because it exposes
  more sensitive data than needed.
- Hide medication and incident context from emergency profiles: rejected
  because it reduces emergency usefulness.

## Decision: Emergency access sessions expire after 30 minutes

**Rationale**: The clarified rule gives staff enough time for most school
emergency response and handoff workflows while limiting prolonged exposure of
sensitive health data. Continued access requires re-confirmation.

**Alternatives considered**:
- 15-minute expiry: rejected because it may interrupt active care too often.
- 60-minute expiry: rejected because it leaves sensitive data exposed longer
  than needed.
- Access until linked incident closure: rejected because incident closure may
  happen much later than emergency response.

## Decision: Break-glass is limited to school-configured pre-authorized emergency roles

**Rationale**: Break-glass must remain usable in emergencies while protecting
medical privacy. Limiting it to pre-authorized emergency roles avoids broad
staff exposure while still supporting school-defined emergency coverage.

**Alternatives considered**:
- Any active staff assigned to the student or activity: rejected because
  assignment alone is not enough authority for sensitive medical data.
- Only nurses, clinic staff, and administrators: rejected because schools may
  need designated emergency roles outside those titles.
- Any active staff with a reason: rejected because the exposure risk is too
  broad.

## Decision: Optional offline emergency cache uses a 24-hour freshness window

**Rationale**: The spec clarifies that cached critical data is usable if synced
within 24 hours. Older cached data requires stale warning, reason, review, and
syncable access evidence. This balances emergency usefulness with safety
because medical instructions can change.

**Alternatives considered**:
- 72-hour cache: rejected because stale medication or allergy information is
  too risky.
- Always show cached data: rejected because stale data could be trusted
  silently.
- No offline medical cache: rejected because emergency workflows may need
  minimum essential data when connectivity fails.

## Decision: Preserve medication administration as evidence, not clinical authority

**Rationale**: Phase 7 records medication administration evidence only when an
active instruction or authorized override reason exists. It must not diagnose,
prescribe, dispense medication, or create pharmacy or payment outcomes.

**Alternatives considered**:
- Allow staff to create new medication instructions during incidents: rejected
  because medication instruction changes require authorized human input and
  review.
- Integrate pharmacy fulfillment: rejected because it is outside Phase 7.

## Decision: High-severity medical incidents default to a limited medical notification audience

**Rationale**: Clarification sets the default audience to approved guardians,
emergency contacts, assigned nurse or clinic staff, and the school emergency
coordinator. This covers care coordination without broad broadcast behavior.

**Alternatives considered**:
- Guardians only: rejected because staff and emergency contacts often need
  immediate coordination.
- Include teachers and school leadership by default: rejected because it risks
  over-sharing sensitive details.
- Fully school-configured default only: rejected because tasks and acceptance
  tests need a default rule.

## Decision: Medical notification creates requests and status events, not delivery management

**Rationale**: Phase 7 needs medical notification requests, contact priorities,
manual contact evidence, acknowledgements, failed-contact states, and status
events for later Phase 9 consumption. It must not implement broad messaging,
broadcasts, templates, delivery retries, or channel orchestration.

**Alternatives considered**:
- Send direct notifications from Phase 7: rejected because Phase 9 owns
  notification delivery.
- Omit notification request evidence: rejected because high-severity incidents
  require accountable contact workflows.

## Decision: Use append-only history for incidents, care actions, access, and reviews

**Rationale**: Medical records are sensitive and often disputed. Original
evidence, corrections, access sessions, care actions, contact attempts, and
review decisions must remain traceable.

**Alternatives considered**:
- Edit the latest incident in place: rejected because it hides original
  evidence.
- Keep only final incident status: rejected because auditors and guardians need
  explainable histories.

## Decision: Route conflicting non-identical records to manual review

**Rationale**: The spec requires exact duplicates to be rejected or returned as
already processed, while conflicting medical updates, incidents, care actions,
notifications, or reviews must not be silently merged or discarded.

**Alternatives considered**:
- Always accept the newest record: rejected because it could hide critical
  medical evidence.
- Always block all overlapping records: rejected because legitimate correction
  or concurrent emergency work may require reviewer resolution.

## Decision: Use permission-scoped medical history and summaries

**Rationale**: Nurses, clinic staff, guardians, students, emergency staff,
reviewers, administrators, auditors, and platform reviewers need different
views of medical records, incidents, emergency access, notifications,
exceptions, and reviews. History and summaries must apply the same tenant and
privacy filters as source records.

**Alternatives considered**:
- Provide broad medical dashboards in Phase 7: rejected because broad admin
  dashboards are outside the phase.
- Show all staff notes to guardians: rejected because the spec requires
  restricted medical details and staff-only notes to be hidden.

## Decision: Testing strategy follows sensitive-data and emergency-risk boundaries

**Rationale**: Phase 7 touches sensitive student medical data, guardian
visibility, emergency access, optional offline cache, and break-glass flows.
Unit, integration, contract, authorization, tenant-isolation, offline-cache,
audit, performance, and critical UI journey tests are required.

**Alternatives considered**:
- UI-only testing: rejected because tenant and permission boundaries are
  security-critical.
- Unit tests only: rejected because emergency access, offline cache, and
  contracts require integration and contract coverage.
