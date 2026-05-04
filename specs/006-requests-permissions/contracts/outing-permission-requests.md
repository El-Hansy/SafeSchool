# Contract: Outing and Permission Requests

This contract defines outing and general permission request creation, status
tracking, withdrawal, duplicate handling, tenant visibility, and audit behavior
for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.outing` for outing request types
  - `requests.approval_workflow`
  - Related request type capability for school-defined permission requests
- Common permissions:
  - `requests.requests.create`
  - `requests.requests.read`
  - `requests.requests.withdraw`
  - `requests.outings.read`
  - `requests.outings.manage`
  - `requests.guardian_history.read`
  - `requests.audit.read`

Request creation and withdrawal commands must be tenant-scoped, idempotent by
caller request identity, and auditable. Exact active duplicates are blocked.
Overlapping non-identical active requests route to manual review.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/requests` | Create or submit a school-scoped request as authorized staff |
| POST | `/api/v1/guardians/me/students/{studentProfileId}/requests` | Create or submit a request for a linked student |
| POST | `/api/v1/students/me/requests` | Create or submit a student-owned request when the request type allows student initiation |
| GET | `/api/v1/schools/{schoolAccountId}/requests` | List school-scoped requests with filters |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}` | Read one request detail |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/requests` | List guardian-visible linked-student requests |
| GET | `/api/v1/students/me/requests` | List student-visible own requests |
| POST | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/withdraw` | Withdraw a request as authorized staff or reviewer |
| POST | `/api/v1/guardians/me/students/{studentProfileId}/requests/{requestId}/withdraw` | Withdraw a guardian-created linked-student request |
| POST | `/api/v1/students/me/requests/{requestId}/withdraw` | Withdraw a student-created request when allowed |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/trace` | Trace request to workflow, consent, star, exception, review, and audit evidence |

## Request Submission

```yaml
request_type_id: "request-type-reference"
student_profile_id: "student-profile-reference"
reason: "Science club outing permission."
requested_start_at: "YYYY-MM-DDTHH:MM:SSZ"
requested_end_at: "YYYY-MM-DDTHH:MM:SSZ"
request_details:
  destination_or_purpose: "Science museum"
  supervision_expectation: "Teacher supervised"
  transport_expectation: "School bus"
submit_now: true
client_request_id: "request-unique-to-caller"
```

## Request Response

```yaml
permission_request_id: "request-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
request_type_id: "request-type-reference"
request_type_version: "request-type-version-reference"
request_category: "Outing"
requester_actor_id: "actor-reference"
requester_role: "Guardian"
request_status: "Pending Approval"
requested_start_at: "YYYY-MM-DDTHH:MM:SSZ"
requested_end_at: "YYYY-MM-DDTHH:MM:SSZ"
workflow_template_version_id: "workflow-version-reference"
current_assignee_scope: "Role:RequestApprover"
duplicate_review_state: "None"
exception_state: "None"
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Withdraw Request

```yaml
withdrawal_reason: "Family plans changed."
client_request_id: "withdrawal-unique-to-caller"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, role permission, and
  backend feature enforcement.
- Guardian routes require an approved active guardian link to the student.
- Student routes are allowed only when the request type allows student
  initiation.
- Staff routes require permission inside the school account.
- Submission validates required fields, student status, request type status,
  workflow availability, guardian consent needs, star rule eligibility when
  applicable, duplicate policy, and overlap policy.
- Exact active duplicates for the same student, request type, requested date,
  and requested time window are blocked with a reviewable reason.
- Overlapping non-identical active requests for the same student and request
  type route to manual review.
- Withdrawal is allowed only before configured final or restricted review
  states and preserves prior request and decision evidence.
- Responses must never expose cross-school request existence.
