# Contract: Request History and Review

This contract defines request history search, guardian/student privacy-limited
history, request exceptions, manual reviews, corrections, reopenings,
escalations, review summaries, and traceability for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.history`
  - `requests.review_summaries` for summary views
  - Related workflow capabilities for records included in history
- Common permissions:
  - `requests.history.read`
  - `requests.guardian_history.read`
  - `requests.reviews.manage`
  - `requests.summaries.read`
  - `requests.audit.read`

History and review reads are permission-scoped. Guardians see linked-student
request details allowed by their role only. Students see only their own
eligible request records. Staff see records allowed by role, assignment, or
review authority.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/requests/history` | Search school-scoped request history |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/requests/history` | Search guardian-visible linked-student request history |
| GET | `/api/v1/students/me/requests/history` | Search student-visible own request history |
| GET | `/api/v1/schools/{schoolAccountId}/requests/exceptions` | List request exceptions |
| GET | `/api/v1/schools/{schoolAccountId}/requests/exceptions/{exceptionId}` | Read exception detail |
| POST | `/api/v1/schools/{schoolAccountId}/requests/reviews` | Correct, reopen, close, resolve, dismiss, migrate, or escalate a request |
| GET | `/api/v1/schools/{schoolAccountId}/requests/reviews` | List manual request reviews |
| GET | `/api/v1/schools/{schoolAccountId}/requests/reviews/{reviewId}` | Read manual request review detail |
| GET | `/api/v1/schools/{schoolAccountId}/requests/review-summaries` | Read request review summaries |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/trace` | Trace request lifecycle, decisions, exceptions, reviews, and audit evidence |

## History Query

```yaml
student_profile_id: "student-profile-reference"
request_type_id: "request-type-reference"
status:
  - "Approved"
  - "Manual Review"
date_range:
  from: "YYYY-MM-DDT00:00:00Z"
  to: "YYYY-MM-DDT23:59:59Z"
approver_actor_id: "actor-reference"
workflow_version_id: "workflow-version-reference"
exception_state: "Open"
star_rule_outcome: "Needs Review"
page_size: 25
cursor: null
```

## History Response

```yaml
items:
  - permission_request_id: "request-reference"
    student_profile_id: "student-profile-reference"
    request_type_name: "Early Leave"
    request_status: "Approved"
    requested_start_at: "YYYY-MM-DDTHH:MM:SSZ"
    requested_end_at: "YYYY-MM-DDTHH:MM:SSZ"
    current_assignee_scope: null
    latest_decision_summary: "Approved by request manager"
    exception_state: "None"
    star_rule_outcome: "Not Required"
    updated_at: "YYYY-MM-DDTHH:MM:SSZ"
page:
  size: 25
  next_cursor: "cursor-reference"
```

## Manual Review Request

```yaml
permission_request_id: "request-reference"
request_exception_id: "exception-reference"
review_action: "Reopen"
review_reason: "Staff verified that prior denial used stale guardian link evidence."
resulting_request_status: "Manual Review"
client_request_id: "manual-review-unique-to-caller"
```

## Review Summary Response

```yaml
summary_scope: "Request Type"
scope_reference: "request-type-reference"
school_account_id: "school-account-reference"
pending_count: 12
approved_count: 30
denied_count: 4
withdrawn_count: 2
exception_open_count: 3
manual_review_count: 5
star_rule_needs_review_count: 1
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- History reads require tenant access, enabled `requests.history`, permission,
  and scope validation.
- Guardian history requires an approved active guardian link and hides
  staff-only assignment, internal review, and platform-only details.
- Student history is limited to the student's own eligible records.
- Manual reviews require explicit reviewer permission, reason, idempotent
  client identity, and audit evidence.
- Corrections, reopenings, escalations, and exception resolutions preserve the
  original request and decision history.
- Exact active duplicate and overlapping-request review outcomes must remain
  traceable.
- Summary reads require `requests.review_summaries` and must be paginated or
  filter-limited for tenant-scoped performance.
