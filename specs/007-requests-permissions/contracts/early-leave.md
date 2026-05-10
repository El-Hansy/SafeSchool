# Contract: Early Leave

This contract defines early leave request details, guardian consent, pickup
evidence, staff verification, release eligibility reads, expiry, and safety
boundaries for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.early_leave`
  - `requests.approval_workflow`
- Common permissions:
  - `requests.early_leave.read`
  - `requests.early_leave.manage`
  - `requests.early_leave.release_read`
  - `requests.pickup.verify`
  - `requests.guardian_consent.act`
  - `requests.audit.read`

Early leave release eligibility is read-only evidence for authorized staff.
Phase 6 must not create attendance, entry, exit, gate scan, QR, NFC, transport,
or student location outcomes.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/guardians/me/students/{studentProfileId}/early-leave-requests` | Submit guardian early leave request |
| POST | `/api/v1/schools/{schoolAccountId}/early-leave-requests` | Submit staff early leave request |
| GET | `/api/v1/schools/{schoolAccountId}/early-leave-requests` | List early leave requests with filters |
| GET | `/api/v1/schools/{schoolAccountId}/early-leave-requests/{requestId}` | Read early leave request detail |
| POST | `/api/v1/schools/{schoolAccountId}/early-leave-requests/{requestId}/pickup-verification` | Record staff pickup verification note |
| GET | `/api/v1/schools/{schoolAccountId}/early-leave/release-eligibility` | Read release eligibility by student/date/request filters |
| GET | `/api/v1/schools/{schoolAccountId}/early-leave-requests/{requestId}/trace` | Trace early leave request to consent, workflow, pickup, exception, review, and audit evidence |

## Early Leave Request

```yaml
student_profile_id: "student-profile-reference"
release_at: "YYYY-MM-DDTHH:MM:SSZ"
release_reason: "Medical appointment."
guardian_consent_expected: true
pickup_person:
  name: "Authorized Guardian Representative"
  relationship: "Uncle"
  authorization_reference: "guardian-selected-authorized-person"
client_request_id: "early-leave-request-unique-to-caller"
```

## Pickup Verification Request

```yaml
pickup_evidence_id: "pickup-evidence-reference"
verification_status: "Verified"
staff_verification_note: "Verified pickup person against guardian-selected authorized list."
client_request_id: "pickup-verification-unique-to-caller"
```

## Release Eligibility Response

```yaml
permission_request_id: "request-reference"
student_profile_id: "student-profile-reference"
release_at: "YYYY-MM-DDTHH:MM:SSZ"
release_eligibility_status: "Release Eligible"
approval_expires_at: "YYYY-MM-DDTHH:MM:SSZ"
pickup_person_name: "Authorized Guardian Representative"
pickup_person_relationship: "Uncle"
staff_verification_status: "Verified"
attendance_event_created: false
gate_event_created: false
scan_event_created: false
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Early leave submission requires tenant access, enabled
  `requests.early_leave`, valid student status, request type eligibility, role
  permission, and guardian link validation where applicable.
- Release eligibility requires final request approval, unexpired approval,
  guardian-selected authorized pickup person, and staff verification note.
- Missing staff verification note blocks or routes release eligibility to
  review.
- Unlinked guardians, unauthorized pickup people, expired approvals, invalid
  students, disabled capability, and cross-school references are blocked or
  routed to review.
- Release eligibility reads require `requests.early_leave.release_read` or
  another explicit role permission.
- Phase 6 release eligibility reads must not create attendance, entry, exit,
  scan, or transport side effects.
