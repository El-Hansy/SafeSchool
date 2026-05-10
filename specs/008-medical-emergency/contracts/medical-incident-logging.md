# Contract: Medical Incident Logging

This contract defines medical incident creation, severity escalation, care
actions, medication administration evidence, follow-up, closure, duplicate
handling, correction, and audit behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.incidents`
  - `medical.records` when incident logging uses profile context
- Common permissions:
  - `medical.incidents.create`
  - `medical.incidents.read`
  - `medical.incidents.review`
  - `medical.care_actions.create`
  - `medical.audit.read`

Medical incidents are tenant-scoped and preserve an append-only care timeline.
Medication administration evidence records what happened; it does not diagnose,
prescribe, dispense, or create payment outcomes.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/medical/incidents` | Create medical incident |
| GET | `/api/v1/schools/{schoolAccountId}/medical/incidents` | List medical incidents with filters |
| GET | `/api/v1/schools/{schoolAccountId}/medical/incidents/{incidentId}` | Read medical incident detail |
| POST | `/api/v1/schools/{schoolAccountId}/medical/incidents/{incidentId}/care-actions` | Add care action |
| POST | `/api/v1/schools/{schoolAccountId}/medical/incidents/{incidentId}/close` | Close incident with reason |
| POST | `/api/v1/schools/{schoolAccountId}/medical/incidents/{incidentId}/review` | Correct, dispute, reopen, resolve, or escalate incident |
| GET | `/api/v1/schools/{schoolAccountId}/medical/incidents/{incidentId}/trace` | Trace incident to care actions, notifications, reviews, and audit |

## Medical Incident Request

```yaml
student_profile_id: "student-reference"
incident_type: "Allergy"
severity: "High"
location_context: "Cafeteria"
observed_details: "Student reported symptoms after lunch."
occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
notification_required: true
follow_up_required: true
source_event_reference: "incident-source-reference"
client_request_id: "incident-unique-to-caller"
```

## Care Action Request

```yaml
action_type: "Medication Administration"
action_summary: "Administered school-held medication per instruction."
medication_instruction_id: "instruction-reference"
medication_evidence_reference: "administration-evidence-reference"
performed_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "care-action-unique-to-caller"
```

## Incident Review Request

```yaml
review_action: "Correct"
review_reason: "Corrected severity after nurse review."
resulting_status: "In Care"
client_request_id: "incident-review-unique-to-reviewer"
```

## Acceptance Rules

- Incident creation requires tenant access, enabled incident capability,
  active student, actor authority, required severity, observation, time,
  source identity, and audit evidence.
- Care actions require incident scope, actor authority, action summary, time,
  and audit evidence.
- Medication administration requires an active medication instruction or an
  authorized emergency override reason.
- Incident and care action evidence is append-only; corrections append review
  evidence and preserve original records.
- Exact duplicate incidents or care actions return the existing result.
- Conflicting non-identical incident or care action records route to manual
  review.
- Incident workflows must not create diagnosis, prescription, pharmacy,
  wallet, payment, attendance, gate, transport, request, complaint, document,
  search, or broad messaging outcomes.
