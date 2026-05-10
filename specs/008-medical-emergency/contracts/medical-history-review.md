# Contract: Medical History and Review

This contract defines medical history search, exception review, manual
corrections, reopenings, review summaries, lifecycle traceability, privacy
filtering, and audit behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.history`
  - `medical.review_summaries` for summaries
  - `medical.configuration` when rule migration is requested
- Common permissions:
  - `medical.history.read`
  - `medical.guardian_history.read`
  - `medical.reviews.manage`
  - `medical.summaries.read`
  - `medical.audit.read`

History and review routes are tenant-scoped, permission-scoped, paginated, and
privacy-filtered for students and guardians.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/medical/history` | Search school medical history |
| GET | `/api/v1/students/me/medical/history` | Search student-visible own medical history |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/medical/history` | Search guardian-visible linked-student medical history |
| GET | `/api/v1/schools/{schoolAccountId}/medical/exceptions` | List medical exceptions |
| GET | `/api/v1/schools/{schoolAccountId}/medical/exceptions/{exceptionId}` | Read exception detail |
| POST | `/api/v1/schools/{schoolAccountId}/medical/reviews` | Correct, reopen, close, resolve, dismiss, escalate, or migrate a medical record |
| GET | `/api/v1/schools/{schoolAccountId}/medical/review-summaries` | Read medical review summaries |
| GET | `/api/v1/schools/{schoolAccountId}/medical/trace/{sourceType}/{sourceId}` | Trace a medical lifecycle record |

## History Query

```yaml
student_profile_id: "student-reference"
condition: "Asthma"
allergy: "Peanuts"
medication_instruction_id: "instruction-reference"
care_plan_id: "care-plan-reference"
incident_severity: "High"
emergency_access_state: "Break Glass"
notification_state: "Acknowledged"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
exception_state: "Open"
page: 1
page_size: 25
```

## Manual Review Request

```yaml
source_type: "Medical Incident"
source_id: "incident-reference"
review_action: "Correct"
review_reason: "Corrected after nurse review."
resulting_status: "Closed"
client_request_id: "medical-review-unique-to-reviewer"
```

## Review Summary Response

```yaml
summary_scope: "Student"
student_profile_id: "student-reference"
incident_counts:
  high: 1
  closed: 3
emergency_access_counts:
  break_glass: 1
notification_counts:
  acknowledged: 2
exception_counts:
  open: 1
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- History reads require tenant access, enabled medical history capability, role
  permission, and visibility filtering by school account, guardian link,
  student ownership, medical assignment, emergency authority, reviewer scope,
  or platform review authority.
- Guardian and student views hide staff-only notes, restricted medical details,
  internal reviewer assignment, disputed records, and inactive or superseded
  instructions unless school visibility rules permit disclosure.
- Manual review actions require explicit reviewer permission, idempotent
  client identity, required reason, and audit evidence.
- Corrections, reopenings, dismissals, escalations, and rule migrations
  preserve original medical records and append review evidence.
- Summary reads enforce the same visibility boundaries as underlying records.
- Trace responses link source records to medical profiles, emergency access,
  offline cache access, incidents, care actions, notifications, contacts,
  exceptions, reviews, status events, and audit evidence where applicable.
