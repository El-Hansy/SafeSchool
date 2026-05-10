# Contract: Medical Records

This contract defines student medical profile management, condition and
allergy records, medication instructions, care plans, emergency contacts,
consent records, guardian-submitted updates, visibility filtering, duplicate
handling, and audit behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.records`
  - `medical.history` when medical history is read
- Common permissions:
  - `medical.records.manage`
  - `medical.records.read`
  - `medical.guardian_updates.submit`
  - `medical.guardian_updates.review`
  - `medical.guardian_history.read`
  - `medical.audit.read`

Medical record commands are tenant-scoped, permission-scoped, idempotent where
they mutate state, and auditable. Guardian-submitted updates always enter
pending medical review before they become school-verified evidence.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/medical/profiles` | Create or update a student medical profile |
| GET | `/api/v1/schools/{schoolAccountId}/medical/profiles/{studentProfileId}` | Read staff-visible medical profile |
| POST | `/api/v1/schools/{schoolAccountId}/medical/profiles/{studentProfileId}/archive` | Archive a medical profile with reason |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/medical/profile` | Read guardian-visible linked-student medical profile |
| GET | `/api/v1/students/me/medical/profile` | Read student-visible own medical summary where enabled |
| POST | `/api/v1/guardians/me/students/{studentProfileId}/medical/updates` | Submit guardian medical update for review |
| POST | `/api/v1/schools/{schoolAccountId}/medical/updates/{updateId}/review` | Accept, reject, partially accept, or request information for a guardian update |
| GET | `/api/v1/schools/{schoolAccountId}/medical/profiles/{studentProfileId}/trace` | Trace profile to records, updates, incidents, reviews, status events, and audit evidence |

## Medical Profile Request

```yaml
student_profile_id: "student-reference"
medical_summary: "Medical summary for authorized staff."
critical_summary: "Minimum emergency summary."
conditions:
  - condition_name: "Asthma"
    severity: "High"
    emergency_relevant: true
allergies:
  - allergen: "Peanuts"
    severity: "Critical"
    reaction_notes: "Severe reaction."
medication_instructions:
  - medication_name: "Inhaler"
    dosage_instruction: "Per school-held instruction."
    emergency_use_allowed: true
care_plans:
  - plan_title: "Asthma care plan"
    emergency_steps: "Follow school-approved emergency steps."
emergency_contacts:
  - contact_name: "Guardian One"
    relationship: "Guardian"
    contact_priority: 1
consent_records:
  - consent_area: "Emergency Care"
    consent_state: "Granted"
effective_from: "YYYY-MM-DDTHH:MM:SSZ"
effective_to: null
client_request_id: "medical-profile-unique-to-caller"
```

## Guardian Update Request

```yaml
submitted_area: "Allergy"
submitted_payload_reference: "guardian-medical-update-reference"
submission_note: "Updated allergy details."
client_request_id: "guardian-update-unique-to-caller"
```

## Guardian Update Review Request

```yaml
review_action: "Accept"
review_reason: "Verified by school nurse."
client_request_id: "guardian-update-review-unique-to-reviewer"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, role permission, and
  backend feature enforcement.
- Staff routes require school-account medical authority unless the actor has
  explicit administrator or platform review authority.
- Guardian routes require an approved active guardian link and configured
  guardian medical visibility.
- Student routes require student self-scope and enabled student medical
  summary visibility.
- Guardian updates never become school-verified evidence until reviewed.
- Sensitive and staff-only medical details are hidden from guardians, students,
  and non-medical staff unless school rules explicitly permit disclosure.
- Duplicate medical profile or guardian update commands return the existing
  result; conflicting non-identical records route to review.
- Medical record reads must not create attendance, gate, transport, wallet,
  learning reward, request approval, complaint, document, search, or broad
  messaging outcomes.
