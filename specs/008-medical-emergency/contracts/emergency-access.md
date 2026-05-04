# Contract: Emergency Access

This contract defines critical emergency medical views, break-glass access,
30-minute session expiry, optional 24-hour offline emergency cache, stale-cache
review routing, denied access, and audit behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.emergency_access`
  - `medical.offline_emergency_cache` when offline cache is used
- Common permissions:
  - `medical.emergency.read`
  - `medical.emergency.break_glass`
  - `medical.emergency.review`
  - `medical.audit.read`

Emergency access is tenant-scoped, minimum-necessary, reason-required,
time-bounded, and auditable. Break-glass access is limited to
school-configured pre-authorized emergency roles.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/medical/emergency-access` | Start emergency access session |
| POST | `/api/v1/schools/{schoolAccountId}/medical/emergency-access/{sessionId}/reconfirm` | Re-confirm continued emergency access for another 30 minutes |
| POST | `/api/v1/schools/{schoolAccountId}/medical/emergency-access/{sessionId}/close` | Close emergency access session |
| POST | `/api/v1/schools/{schoolAccountId}/medical/break-glass` | Start break-glass emergency access |
| POST | `/api/v1/schools/{schoolAccountId}/medical/offline-cache/access` | Record offline emergency cache access evidence |
| GET | `/api/v1/schools/{schoolAccountId}/medical/emergency-access/{sessionId}/trace` | Trace emergency access to profile, incident, cache, reviews, and audit |

## Emergency Access Request

```yaml
student_profile_id: "student-reference"
access_context: "Clinic"
access_reason: "Student reported severe allergy symptoms."
requested_data_categories:
  - "Allergies"
  - "Medication Instructions"
  - "Care Plans"
  - "Emergency Contacts"
client_request_id: "emergency-access-unique-to-caller"
```

## Break-Glass Request

```yaml
student_profile_id: "student-reference"
pre_authorized_emergency_role: "Emergency Coordinator"
confirmation_text: "Emergency access required now."
reason: "Student is unconscious."
access_context: "Sports field"
client_request_id: "break-glass-unique-to-caller"
```

## Offline Cache Access Request

```yaml
student_profile_id: "student-reference"
cached_profile_version: "profile-version-reference"
last_synced_at: "YYYY-MM-DDTHH:MM:SSZ"
stale_warning_acknowledged: true
reason: "Connectivity unavailable during emergency."
client_request_id: "offline-cache-access-unique-to-caller"
```

## Emergency Profile Response

```yaml
emergency_access_session_id: "session-reference"
student_profile_id: "student-reference"
expires_at: "YYYY-MM-DDTHH:MM:SSZ"
critical_allergies:
  - allergen: "Peanuts"
    severity: "Critical"
critical_medication_instructions:
  - medication_name: "Inhaler"
    emergency_use_allowed: true
active_care_plans:
  - plan_title: "Asthma care plan"
emergency_contacts:
  - contact_name: "Guardian One"
    contact_priority: 1
review_state: "Not Required"
```

## Acceptance Rules

- Emergency access requires tenant access, enabled capability, eligible
  student, authorized actor, emergency reason, and audit evidence.
- Break-glass requires a school-configured pre-authorized emergency role,
  explicit emergency confirmation, reason, and mandatory review.
- Emergency access sessions expire after 30 minutes and require
  re-confirmation for continued access.
- Critical profile views include only active emergency-relevant allergies,
  medication instructions, care plans, restrictions, contacts, and recent
  relevant incident context.
- Offline cache data is usable only when synced within the last 24 hours.
- Cache older than 24 hours requires stale warning acknowledgement, reason,
  access evidence sync, and review routing.
- Emergency access must not provision cards, create scans, generate
  attendance, decide gate access, manage transport, or create request, wallet,
  learning, complaint, document, search, or broad messaging outcomes.
